using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;

        public AuthService(ApplicationUserManager userManager,
            ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<(bool LoginSucceed, LoginResponseModel LoginResponseModel, bool isLocked, DateTime? lockUntil)> Login(LoginModel loginModel)
        {
            if (await _userManager.FindByEmailAsync(loginModel.UserName) is not ApplicationUser user)
                return (false, new(), false, null);

            await _userManager.AccessFailedAsync(user);

            if (await _userManager.IsLockedOutAsync(user))
                return (false, new(), true, user.LockoutEnd.Value.LocalDateTime);

            var loginResult = await _signInManager.Authenticate(user.UserName, loginModel.Password, true, true);


            if (!loginResult.Result.Succeeded && !loginResult.Result.IsNotAllowed)
                return (false, new(), true, null);

            await _userManager.ResetAccessFailedCountAsync(user);


            user.RefreshToken = loginResult.AccessRefreshToken;
            await _userManager.UpdateAsync(user);

            var rolenames = await _userManager.GetRolesAsync(user);
            var model = new LoginResponseModel()
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                AccessToken = loginResult.AccessToken,
                RefreshToken = loginResult.AccessRefreshToken,
                AccessTokenExpiry = loginResult.AccessTokenExpiry,
                RefreshTokenExpiry = loginResult.AccessRefreshTokenExpiry,
                EmailVerified = user.EmailConfirmed,
                Roles = rolenames
            };

            return (true, model, false, null);
        }
    }
}
