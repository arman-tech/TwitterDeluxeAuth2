using Microsoft.AspNetCore.Mvc;
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

            if (model == null) {
                return BadRequest("User registration data is required");
            }
            if (string.IsNullOrWhiteSpace(model.Username)) {
                return BadRequest("Username is required");
            }
            if (string.IsNullOrWhiteSpace(model.Email)) {
                return BadRequest("Email is required");
            }
            if (string.IsNullOrWhiteSpace(model.Password)) {
                return BadRequest("Password is required");
            }
            if (model.Username.ContainsXss()) {
                return BadRequest("Username contains invalid characters");
            }
            if (model.Password.ContainsXss()) {
                return BadRequest("Password contains invalid characters");
            }
            if (model.Email.ContainsXss()) {
                return BadRequest("Email contains invalid characters");
            }


            try {
                var result = await _userService.RegisterUser(model);
                if (!result)
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed or user already exists." });

                return Ok(new { Status = "Success", Message = "User created successfully!" });
            }catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Failed to register user." });
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model) {
            if (model == null) {
                return BadRequest("User registration data is required");
            }
            if (string.IsNullOrWhiteSpace(model.Email)) {
                return BadRequest("Username is required");
            }
            if (string.IsNullOrWhiteSpace(model.Password)) {
                return BadRequest("Password is required");
            }

            if (model.Email.ContainsXss()) {
                return BadRequest("Username contains invalid characters");
            }
            if (model.Password.ContainsXss()) {
                return BadRequest("Password contains invalid characters");
            }

            try {
                var token = await _userService.Login(model);
                if (token == null)
                    return Unauthorized();

                return Ok(new { token = token, expiration = 2 });
            }catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Failed to login." });
            }
        }
    }
}
