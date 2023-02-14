using Microsoft.AspNetCore.Mvc;
using Backend.Core.EF;
using Backend.Core;
using Xunit.Sdk;

namespace Backend.Controllers;

[ApiController]
[Route("")]
public class MinitwitController : ControllerBase
{
    private readonly ILogger<MinitwitController> _logger;

    private readonly IMessageRepository _messageRepo;
    private readonly IUserRepository _userRepo;
    private readonly IFollowerRepository _followerRepo;
    public MinitwitController(ILogger<MinitwitController> logger, IMessageRepository messageRepo, IUserRepository userRepo, IFollowerRepository followerRepo)
    {
        _logger = logger;
        _messageRepo = messageRepo;
        _userRepo = userRepo;
        _followerRepo = followerRepo;
    }

	[HttpGet("public")]
    public async Task<IActionResult> GetTimeline()
    {
        try {
            var messages = await _messageRepo.ReadAllAsync();
            return Ok(messages);
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            return StatusCode(504);
        }
    }

    [HttpGet("seed")]
    public async Task<IActionResult> Seed()
    {
        try {
            var messages = await _messageRepo.Seed();
            var users = await _userRepo.Seed();
            return Ok(messages);
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            return StatusCode(504);
        }
    }

    [HttpPost("new-message")]
    public async Task<IActionResult> CreateMessage(MessageCreateDTO message)
    {
        try {
            var result = await _messageRepo.CreateAsync(message);
            return Ok(result);
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            return StatusCode(504);
        }
    }
    
    [HttpGet("user/{userName}")]
    public async Task<IActionResult> GetUser(string userName)
    {
        try {
            var user = await _userRepo.ReadByUsernameAsync(userName);
            return Ok(user);
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            return StatusCode(504);
        }
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        try {
            var users = await _userRepo.ReadAllAsync();
            return Ok(users);
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            return StatusCode(504);
        }
    }

    [HttpPost("new-user")]
    public async Task<IActionResult> NewUser(UserCreateDTO user)
    {
        try {
            var result = await _userRepo.CreateAsync(user);
            return Ok(result);
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            return StatusCode(504);
        }
    }

    [HttpGet("fllws/{username}")]
    public async Task<IActionResult> GetFollowers(string username)
    {
        //Find user Id of user with username
        Option<UserDTO> userResult;

        try {
            userResult = await _userRepo.ReadByUsernameAsync(username);
            if (userResult.IsNone) throw new NullException(userResult);
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            return NotFound($"Could not find user with name '{username}'.");
        }

        try {
            var followersResult = (await _followerRepo.ReadByWhomId(userResult.Value.Id)).ToList();
            var followers = new {
                followers = followersResult
            };
            return Ok(followers);

        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            return StatusCode(500);
        }
    }
}