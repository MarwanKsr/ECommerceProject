using Identity.Models;

namespace Identity.Services
{
    public interface IAuthService
    {
        Task<(bool LoginSucceed, LoginResponseModel LoginResponseModel, bool isLocked, DateTime? lockUntil)> Login(LoginModel loginModel);
    }
}
