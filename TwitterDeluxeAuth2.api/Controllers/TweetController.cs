using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterDeluxeAuth2.api.Exceptions;
using TwitterDeluxeAuth2.api.Models;
using TwitterThrice.common;
using TwitterThrice.domain;

namespace TwitterDeluxeAuth2.api.Controllers {


    [Route("api/[controller]")]
    [ApiController]
    public class TweetController : Controller {

        private readonly ITweetService _tweetService;
        private readonly IUserService _userService; 
        public TweetController(ITweetService tweetService, IUserService userService) {
            _tweetService = tweetService;
            _userService = userService;
        }
        
        [HttpPost("create")]
        [Authorize] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> CreateTweet([FromBody] TweetPost tweetPost) {

            try {
                // get username from bearer token
                var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(username))
                    throw new AuthenticationFailedException("Invalid token");

                // get user by username
                var user = await _userService.GetUserByUsername(username);

                // should there be a situation where the user is deleted or not found, throw an exception
                if (user == null)
                    throw new AuthenticationFailedException("User not found");

                var result = await _tweetService.CreateTweet(new Tweet {
                    Id = Guid.NewGuid(),
                    MemberId = user.Id.ToString(),
                    Message = tweetPost.Message,
                    PostedDate = DateTime.Now
                });

                return Ok(result);
            }
            catch (AuthenticationFailedException afe) {
                return Unauthorized(afe.Message);
            }
        }

        [HttpGet("recent")]
        [AllowAnonymous]
        [ResponseCache(Duration = 60)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetRecentTopTenTweets() {
            var result = await _tweetService.GetRecentTopTenTweets();
            return Ok(result);
        }

        private bool EmailIsValid(string email, string onFileEmail) {
            return BCrypt.Net.BCrypt.Verify(email, onFileEmail);
        }

    }
}
