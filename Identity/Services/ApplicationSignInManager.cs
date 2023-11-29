using Identity.Configuration;
using Identity.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.Services
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser>
    {
        private readonly ApiConfig _apiConfig;
        UserManager<ApplicationUser> _userManager;

        public ApplicationSignInManager(
            UserManager<ApplicationUser> userManager, 
            IHttpContextAccessor contextAccessor, 
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, 
            IOptions<IdentityOptions> optionsAccessor, 
            ILogger<SignInManager<ApplicationUser>> logger, 
            IAuthenticationSchemeProvider schemes, 
            IUserConfirmation<ApplicationUser> confirmation) 
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            _apiConfig = ApiConfig.Instance;
            _userManager = userManager;
        }

        public async Task<LoginResult> Authenticate(string username, string password, bool generateToken, bool generateRefreshToken)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return new(SignInResult.Failed, default, default, default, default, default);

            // check if username exists
            if (await UserManager.FindByNameAsync(username) is not ApplicationUser user)
                return new(SignInResult.Failed, default, default, default, default, default);

            // check if password is correct
            if (!await UserManager.CheckPasswordAsync(user, password))
                return new(SignInResult.Failed, default, default, default, default, default);

            // If the user is locked out, it will return locked out result with the main user
            // we return user here to be able to have some info as locked out time,....
            if (await UserManager.IsLockedOutAsync(user))
                return new(SignInResult.LockedOut, user, default, default, default, default);

            // If the user is not confirmed, it will return not allowed result with the main user
            // we return user here to be able to have some info as locked out time,....
            if (!await CanSignInAsync(user))
                return new(SignInResult.NotAllowed, user, default, default, default, default);

            await SignInAsync(user, false);

            var userRoles = await _userManager.GetRolesAsync(user);
            // authentication successful 
            return new(SignInResult.Success, user, generateToken ? GenerateJWT(user, userRoles) : string.Empty, generateRefreshToken ? GenerateRefreshJWT(user, userRoles) : string.Empty,
                DateTime.UtcNow.AddDays(_apiConfig.KeyExpiration), DateTime.UtcNow.AddDays(_apiConfig.RefreshKeyExpiration));
        }

        private string GenerateJWT(ApplicationUser user, IList<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_apiConfig.SecretKey);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim("Role", role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddDays(_apiConfig.KeyExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshJWT(ApplicationUser user, IList<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_apiConfig.SecretRefreshKey);
            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddDays(_apiConfig.RefreshKeyExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
