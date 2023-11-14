using Identity.Dto;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        protected ResponseDto _response;
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("api/[controller]/Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>(ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
            };

            var (loginSucceed, loginResponse, isLocked, lockedUntil) = await _authService.Login(loginModel);

            if (isLocked && lockedUntil == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add("User wrong credentials");
            }

            if (isLocked)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add($"User account is locked until {lockedUntil}");
            }

            if (!loginSucceed)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages.Add($"User not exists");
            }

            return Ok(_response);
        }
    }
}
