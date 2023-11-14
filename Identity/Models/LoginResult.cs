using Microsoft.AspNetCore.Identity;

namespace Identity.Models
{
    public record LoginResult(SignInResult Result, ApplicationUser User, string AccessToken, string AccessRefreshToken,
    DateTime AccessTokenExpiry, DateTime AccessRefreshTokenExpiry);
}
