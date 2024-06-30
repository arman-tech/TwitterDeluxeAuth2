using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TwitterDeluxeAuth2.api.Controllers {
    
    [Route("api/[controller]")]
    [ApiController]
    public class TestAuthController : ControllerBase {
        // GET: api/testauth/secure
        [HttpGet("secure")]
        [Authorize] // This attribute requires the request to be authorized
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult SecureEndpoint() {

            var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            return Ok(new { message = "You are authorized to access this endpoint {0}", username });
        }
    }
}
