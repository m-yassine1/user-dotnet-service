using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using user_service.Models;
using user_service.Service;

namespace user_service.Controllers
{
    [Route("user-api")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("usernames/{username}")]
        public ActionResult<ApplicationUserResponse> GetUser([FromRoute] string username)
        {
            _logger.LogInformation($"Fetching user {username}");
            return _userService.GetUser(username);
        }

        // POST api/<UserController>
        [HttpPost("users")]
        [AllowAnonymous]
        public async Task<ActionResult<MessageResponse>> CreateUser([FromBody] ApplicationUserRegisterRequest request)
        {
            _logger.LogInformation($"Creating new user {request.Username}");
            return await _userService.CreateUser(request);
        }

        [HttpPost("users/login")]
        [AllowAnonymous]
        public async Task<ActionResult<MessageResponse>> LoginUser([FromBody] LoginRequest request)
        {
            _logger.LogInformation($"Logging in user {request.Username}");
            return await _userService.LoginUser(request);
        }

        // PUT api/<UserController>/5
        [HttpPut("usernames/{username}")]
        public async Task<ActionResult<MessageResponse>> UpateUser([FromRoute] string username, [FromBody] ApplicationUserUpdateRequest request)
        {
            _logger.LogInformation($"Updating user {username} with body {request}");
            return await _userService.UpdateUser(username, request);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("usernames/{username}")]
        public async Task<ActionResult<MessageResponse>> Delete([FromRoute] string username)
        {
            _logger.LogInformation($"Deleting user {username}");
            return await _userService.DeleteUser(username);
        }
    }
}
