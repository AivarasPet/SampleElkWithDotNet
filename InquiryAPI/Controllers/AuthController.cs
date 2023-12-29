using InquiryAPI.Configuration;
using InquiryAPI.DomainObjects.Users;
using InquiryAPI.Services.UserService;
using InquiryAPI.Utils;
using Microsoft.AspNetCore.Mvc;

namespace InquiryAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _userService;
        private readonly string _jwtSecretKey;
        public AuthController(AppConfiguration appConfiguration, IUsersService userService)
        {
            _jwtSecretKey = appConfiguration.JwtSecretKey;
            _userService = userService;
        }

        public class LoginDTO
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            if (_userService.ValidateCredentials(loginDTO.Username, loginDTO.Password, out User validUser))
            {
                var token = JwtUtils.GenerateJwtToken(validUser.Id, _jwtSecretKey);
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
}
