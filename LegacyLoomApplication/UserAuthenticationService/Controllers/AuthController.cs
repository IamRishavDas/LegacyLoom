using Microsoft.AspNetCore.Mvc;
using UserAuthenticationService.DTOs.UserAuthenticationDTOs;
using UserAuthenticationService.Services;
using ServiceResponseShared;
using System.Net;
using RequestFeatureShared.Constants;

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

        [HttpPost("login/username")]
        public async Task<ActionResult<ServiceResponse<UserLoginResponse>>> LoginUsingUsername([FromBody] UserLoginRequestByUsername loginRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ServiceResponse<UserLoginResponse>.Failure("Invalid input data", errors, (int)HttpStatusCode.BadRequest));
            }

            var response = await _authService.GenerateTokenForLoginByUsername(loginRequest);
            if (!response.Success)
            {
                return Unauthorized(ServiceResponse<UserLoginResponse>.Failure(response.ErrorMessage ?? "Invalid credentials", response.Errors, (int)HttpStatusCode.Unauthorized));
            }

            Response.Headers.Append(HeaderKey.AUTHORIZATION, $"Bearer {response.Data?.Token}");
            Response.Cookies.Append(HeaderKey.AUTHORIZATION, value: response.Data?.Token, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddMinutes(60)
            });
            return StatusCode(response.StatusCode, response);
        }
        
        [HttpPost("login/email")]
        public async Task<ActionResult<ServiceResponse<UserLoginResponse>>> LoginUsingEmail([FromBody] UserLoginRequestByEmail loginRequest)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ServiceResponse<UserLoginResponse>.Failure("Invalid input data", errors, (int)HttpStatusCode.BadRequest));
            }

            var response = await _authService.GenerateTokenForLoginByEmail(loginRequest);
            if (!response.Success)
            {
                return Unauthorized(ServiceResponse<UserLoginResponse>.Failure(response.ErrorMessage ?? "Invalid credentials", response.Errors, (int)HttpStatusCode.Unauthorized));
            }

            Response.Headers.Append(HeaderKey.AUTHORIZATION, $"Bearer {response.Data?.Token}");
            Response.Cookies.Append(HeaderKey.AUTHORIZATION, value: response.Data?.Token, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddMinutes(60)
            });
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("logout")]
        public ActionResult<ServiceResponse<UserLoginResponse>> Logout()
        {
            var response = _authService.GenerateTokenForLogout();
            Response.Headers.Append(HeaderKey.AUTHORIZATION, $"Bearer {response.Data?.Token}");
            Response.Cookies.Append(HeaderKey.AUTHORIZATION, value: response.Data?.Token, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTimeOffset.UtcNow.AddMinutes(60)
            });
            return StatusCode(response.StatusCode, response);
        }
    }
}