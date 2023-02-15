using Microsoft.AspNetCore.Mvc;
using Backend.Core.EF;
using Backend.Core;
using Xunit.Sdk;
using System.Dynamic;

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

        //Find followers of user with userResult.Value.Id (whomId)
        try {
            var followersResult = (await _followerRepo.ReadAllByWhomId(userResult.Value.Id)).ToList();

            var followers = new {
                followers = followersResult
            };
            return Ok(followers);

        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            return StatusCode(500);
        }
    }

    [HttpPost("fllws/{username}")]
    public async Task<IActionResult> Follow(string username, [FromBody] dynamic body)
    {
        //Find userId;
        int userId;
        try {
            var userResult = await _userRepo.ReadByUsernameAsync(username);
            if (userResult.IsNone) throw new NullException(userResult);
            userId = userResult.Value.Id;
        } catch (Exception e) {
            _logger.LogError(e, e.Message);
            return NotFound($"Could not find user with name '{username}'.");
        }


        //Handle request
        if(PropertyExists(body, "follow")) //This is a follow request
        {
            try
            {
                var followCreate = new FollowerCreateDTO
                    {
                        WhoId = userId,
                        WhomId = body.follow
                    };
                var result = await _followerRepo.CreateAsync(followCreate);
                if (result.Item1 != Core.Response.Created) throw new Exception("Failed to follow");
                return Ok("Successful follow");
            } catch (Exception e) {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }
        else if (PropertyExists(body, "unfollow")) //This is an unfollow request
        {
            try
            {
                Option<FollowerDTO> followResult = await _followerRepo.ReadByWhoAndWhomId(userId, body.unfollow);
                if (followResult.IsNone) return NotFound("Could not find the follow relation.");

                var unfollowResult = await _followerRepo.DeleteByIdAsync(followResult.Value.Id);
                if (unfollowResult != Core.Response.Deleted) return StatusCode(500, "Internal Server Error. Could not unfollow");
                return Ok("Succesful unfollow");
            } catch (Exception e) {
                _logger.LogError(e, e.Message);
                return StatusCode(500);
            }
        }
        else //Error in the request body
        {
            return BadRequest("Could not handle the request");
        }
    }


    //Helper functions
    public static bool PropertyExists(dynamic obj, string propertyName)
    {
        if (obj is ExpandoObject)
            return ((IDictionary<string, object>) obj).ContainsKey(propertyName);

        return obj.GetType().GetProperty(propertyName) != null;
  }
}