using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserAuthenticationService.DTOs.UserAuthenticationDTOs;
using UserAuthenticationService.Services;
using ServiceResponseShared;
using System.Net;

namespace UserAuthenticationService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<UserLoginResponse>>> Login([FromBody] UserLoginRequest loginRequest, [FromQuery] bool usingUserNameAndPassword = true)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ServiceResponse<UserLoginResponse>.Failure("Invalid input data", errors, (int)HttpStatusCode.BadRequest));
            }

            var response = await _authService.GenerateTokenForLogin(loginRequest, usingUserNameAndPassword);
            if (!response.Success)
            {
                return Unauthorized(ServiceResponse<UserLoginResponse>.Failure(response.ErrorMessage ?? "Invalid credentials", response.Errors, (int)HttpStatusCode.Unauthorized));
            }

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("logout")]
        public ActionResult<ServiceResponse<UserLoginResponse>> Logout()
        {
            var response = _authService.GenerateTokenForLogout();
            return StatusCode(response.StatusCode, response);
        }
    }
}