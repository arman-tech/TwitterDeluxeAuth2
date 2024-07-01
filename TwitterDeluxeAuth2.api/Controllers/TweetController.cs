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
            if (tweetPost == null) {
                return BadRequest("Tweet is required");
            }

            if (string.IsNullOrWhiteSpace(tweetPost.Message)) {
                return BadRequest("Message is required");
            }

            if (tweetPost.Message.Length > Constants.MaxTweetMessageLength) {
                return BadRequest($"Message is too long. Max length is {Constants.MaxTweetMessageLength} characters");
            }

            if (tweetPost.Message.ContainsXss()) {
                return BadRequest("Message contains invalid characters");
            }

            try {
                // get username from bearer token
                var username = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                if (string.IsNullOrEmpty(username))
                    throw new AuthenticationFailedException("Invalid token");

                // get user by email
                var user = await _userService.GetUserByEmail(tweetPost.Email);

                // should there be a situation where the user is deleted or not found, throw an exception
                // also, ensure that the user is the same as the one in the token
                if (user == null || user.Username != username)
                    throw new AuthenticationFailedException("User not found");

                var result = await _tweetService.CreateTweet(new Tweet {
                    Id = Guid.NewGuid(),
                    MemberId = user.Id.ToString(),
                    Message = tweetPost.Message,
                    PostedDate = DateTime.Now
                });

                return Ok(result);
            }
            catch (AuthenticationFailedException ex) {
                return Unauthorized(ex.Message);
            }
            catch(Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Failed to create tweet." });
            }
        }

        [HttpGet("recent")]
        [AllowAnonymous]
        [ResponseCache(Duration = 60)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Tweet>>> GetRecentTopTenTweets() {
            try {
                var result = await _tweetService.GetRecentTopTenTweets();
                return Ok(result);
            } catch (Exception) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "Failed to get recent tweets." });
            }
        }

    }
}
