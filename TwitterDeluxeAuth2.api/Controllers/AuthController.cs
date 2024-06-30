using Microsoft.AspNetCore.Mvc;
using TwitterDeluxeAuth2.api.Models;
using TwitterThrice.common;
using TwitterThrice.domain;

namespace TwitterDeluxeAuth2.api.Controllers {


    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller {

        private readonly IUserService _userService;

        public AuthController(IUserService userService) {
            _userService = userService;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto model) {
            var result = await _userService.RegisterUser(model);
            if (!result)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed or user already exists." });

            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model) {
            var token = await _userService.Login(model);
            if (token == null)
                return Unauthorized();

            return Ok(new { token = token, expiration = 2 }); 
        }
    }
}
