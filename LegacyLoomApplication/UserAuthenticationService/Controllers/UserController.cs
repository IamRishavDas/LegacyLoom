using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserAuthenticationService.DTOs.UserDTOs;
using UserAuthenticationService.Services;
using ServiceResponseShared;
using UserAuthenticationService.RequestFeatures;
using System.Text.Json;
using RequestFeatureShared.Constants;
using UserAuthenticationService.DTOs.UserAuthenticationDTOs;

namespace UserAuthenticationService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<UserDTO>>>> GetUsers([FromQuery]UserRequestParameters userRequestParams, 
            [FromQuery] bool includeDeleted = false)
        {
            var (users, metadata) = await _userService.GetUsersAsync(userRequestParams, includeDeleted);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(users.StatusCode, users);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin,Moderator")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> GetUser(Guid id)
        {
            var response = await _userService.GetUserAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/users/email/{email}
        [HttpGet("email/{email}")]
        [Authorize(Roles = "User,Admin,Moderator")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> GetUserByEmail(string email)
        {
            var response = await _userService.GetUserByEmailAsync(email);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/users/username/{userName}
        [HttpGet("username/{userName}")]
        [Authorize(Roles = "User,Admin,Moderator")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> GetUserByUserName(string userName)
        {
            var response = await _userService.GetUserByUserNameAsync(userName);
            return StatusCode(response.StatusCode, response);
        }

        // POST: api/users
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> CreateUser([FromBody] UserCreateDTO userCreate)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ServiceResponse<UserDTO>.Failure("Invalid input data", errors, 400));
            }

            var response = await _userService.CreateUserAsync(userCreate);
            return StatusCode(response.StatusCode, response);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> UpdateUser(Guid id, [FromBody] UserUpdateDTO userUpdate)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ServiceResponse<UserDTO>.Failure("Invalid input data", errors, 400));
            }

            var response = await _userService.UpdateUserAsync(id, userUpdate);
            return StatusCode(response.StatusCode, response);
        }

        // DELETE: api/users/{id}/soft
        [HttpDelete("{id}/soft")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<ServiceResponse>> SoftDeleteUser(Guid id)
        {
            var response = await _userService.SoftDeleteUserAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // DELETE: api/users/{id}/permanent
        [HttpDelete("{id}/permanent")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse>> PermanentDeleteUser(Guid id)
        {
            var response = await _userService.PermanentDeleteUserAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/users/exists/{id}
        [HttpGet("exists/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> IsUserExist(Guid id)
        {
            var response = await _userService.IsUserExistAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        // GET: api/users/deleted
        [HttpGet("deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<UserDTO>>>> GetDeletedUsers(UserRequestParameters userRequestParams)
        {
            var (users, metadata) = await _userService.GetDeletedUsersAsync(userRequestParams);
            Response.Headers.Append(HeaderKey.PAGINATION, JsonSerializer.Serialize(metadata));
            return StatusCode(users.StatusCode, users);
        }

        [HttpPost("forgot-password/{userNameOrEmail}")]
        public async Task<IActionResult> ForgotPassword([FromRoute]string userNameOrEmail)
        {
            var response = await _userService.SendForgotPasswordOTPByUserNameOrEmail(userNameOrEmail);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("forgot-password/validate")]
        public async Task<IActionResult> ValidatePassword([FromBody] UserOtpValidationDTO userOtpValidationDTO)
        {
            var response = await _userService.ValidateOtp(userOtpValidationDTO.UserNameOrEmail, userOtpValidationDTO.OTP);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("reset-password")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<UserLoginResponse>> ResetPassword([FromBody] ResetPasswordDTO resetPasswordDTO)
        {
            var userId = User.FindFirst("UserId")?.Value;
            var response = await _userService.ResetPassword(resetPasswordDTO.Password, userId, resetPasswordDTO.UserId);
            return StatusCode(response.StatusCode, response);
        }
    }
}