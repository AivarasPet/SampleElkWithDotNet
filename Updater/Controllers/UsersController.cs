using InquiryAPI.DomainObjects.Users;
using InquiryAPI.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace Updater.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _userService;
        public UsersController(IUsersService userService)
        {
            _userService = userService;
        }

        public class UserDTO
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public IActionResult AddNewUser([FromBody] UserDTO userDTO)
        {
            if (_userService.Search(new UserSearchPredicate() { Username = userDTO.Username }).Any())
            {
                throw new NotSupportedException();
            }

            User user = new User()
            {
                Username = userDTO.Username,
                Password = userDTO.Password,
            };

            _userService.Save(user);

            return Ok();
        }
    }
}
