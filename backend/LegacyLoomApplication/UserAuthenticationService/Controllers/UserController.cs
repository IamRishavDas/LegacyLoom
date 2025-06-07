using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserAuthenticationService.DTOs.UserDTOs;
using UserAuthenticationService.Services;
using ServiceResponseShared;

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
        public async Task<ActionResult<ServiceResponse<IEnumerable<UserDTO>>>> GetUsers([FromQuery] bool includeDeleted = false)
        {
            var response = await _userService.GetUsersAsync(includeDeleted);
            return StatusCode(response.StatusCode, response);
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
        public async Task<ActionResult<ServiceResponse<IEnumerable<UserDTO>>>> GetDeletedUsers()
        {
            var response = await _userService.GetDeletedUsersAsync();
            return StatusCode(response.StatusCode, response);
        }
    }
}