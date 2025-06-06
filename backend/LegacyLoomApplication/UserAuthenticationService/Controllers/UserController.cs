using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserAuthenticationService.Models;
using UserAuthenticationService.Repositories;

namespace UserAuthenticationService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return _userRepository.GetUsers();
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User")]
        public ActionResult<User> GetUser(Guid id)
        {
            return _userRepository.GetUser(id);
        }
    }
}
