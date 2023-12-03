using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services
{
    public interface IAuthService
    {
        Task<(bool LoginSucceed, LoginResponseModel LoginResponseModel, bool isLocked, DateTime? lockUntil)> Login(LoginModel loginModel);
        Task<IdentityResult> RegisterConsumer(ApplicationUser user, string password, string role);
    }
}
