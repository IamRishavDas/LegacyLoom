using AuthenticationManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthenticationService.DTOs;
using UserAuthenticationService.Repositories;

namespace UserAuthenticationService.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthenticationTokenProvider _tokenProvider;

        public AuthController(IUserRepository userRepository, AuthenticationTokenProvider tokenProvider)
        {
            _userRepository = userRepository;
            _tokenProvider = tokenProvider;
        }

        [HttpPost("login")]
        public ActionResult<UserLoginResponse> Login(UserLoginRequest loginRequest)
        {
            var user = _userRepository.GetUsers().Where(user => user.Username == loginRequest.UserName && user.Password == loginRequest.Password).FirstOrDefault();
            if (user == null) return Unauthorized();
            var (token, tokenExpiryTime) = _tokenProvider.GenerateJwtToken(user.Username, user.Role.ToString(), isTokenGeneratedWhileLogin: true);
            var response = new UserLoginResponse()
            {
                Id = user.Id,
                UserName = user.Username,
                Token = token,
                ExpiresIn = (int)tokenExpiryTime.Subtract(DateTime.Now).TotalSeconds,
            };
            return Ok(response);
        }
    }
}
