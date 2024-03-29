using Microsoft.AspNetCore.Mvc;
using Backend.Core.EF;
using Backend.Core;
using Xunit.Sdk;
using System.Dynamic;
using SQLitePCL;
using Microsoft.AspNetCore.Authorization;

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

    private void updateLatest(int? latest)
    {
        GlobalVariables.Latest = latest != null ? (int)latest : -1;
    }

    [HttpGet("")]
    public IActionResult welcome()
    {
        return Ok("Welcome to DevJanitors api");
    }


    [HttpGet("clear")]
    public async Task<IActionResult> Clear()
    {
        try
        {
            var messages = await _messageRepo.ClearMessages();
            var users = await _userRepo.ClearUsers();
            var followers = await _followerRepo.ClearFollowers();
            GlobalVariables.Latest = 0;
            return Ok(messages);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(504);
        }
    }

    [HttpGet("seed")]
    public async Task<IActionResult> Seed()
    {
        try
        {
            var messages = await _messageRepo.Seed();
            var users = await _userRepo.Seed();
            return Ok("messages added: " + messages);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(504);
        }
    }

    [HttpGet("latest")]
    public IActionResult GetLatest()
    {
        return Ok(new { latest = GlobalVariables.Latest });
    }

	[HttpGet("msgs")]
    public async Task<IActionResult> GetTimeline([FromQuery(Name = "latest")] int? latest, [FromQuery(Name = "n")] int? n, [FromQuery(Name = "startIndex")] int? start, [FromQuery(Name = "endIndex")] int? end)
    {
        updateLatest(latest);
        try {
            var messages = (await _messageRepo.ReadAllAsync(n, start, end)).ToList();
            return Ok(messages);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(504);
        }
    }

    [HttpGet("my-timeline/{username}")]
    public async Task<IActionResult> GetMyTimeline(string username, [FromQuery(Name = "startIndex")] int? start, [FromQuery(Name = "endIndex")] int? end)
    {
        try
        {
            var userId = (await _userRepo.ReadByUsernameAsync(username)).Value.Id;
            var usersFollowed = (await _followerRepo.ReadAllWhomIdByWhoId(userId)).ToList().Append(userId);
            var messages = (await _messageRepo.ReadAllByAuthorIDListAsync(usersFollowed, start, end)).ToList();
            return Ok(messages);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(504);
        }
    }

    [HttpGet("msgs/{username}")]
    public async Task<IActionResult> GetUserTimeline(string username, [FromQuery(Name = "latest")] int? latest, [FromQuery(Name = "startIndex")] int? start, [FromQuery(Name = "endIndex")] int? end)
    {
        updateLatest(latest);
        try
        {
            var messages = (await _messageRepo.ReadAllByUsernameAsync(username, start, end))
            .Select(m => new
            {
                content = m.Text,
                user = username,
            });

            return Ok(messages);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(504);
        }
    }

    [HttpPost("msgs/{username}")]
    public async Task<IActionResult> PostNewMessage(string username, [FromBody] MessageCreateJson body, [FromQuery(Name = "latest")] int? latest)
    {
        updateLatest(latest);
        try
        {
            var authorResult = await _userRepo.ReadByUsernameAsync(username);
            if (authorResult.IsNone) return NotFound($"Could not find the user with username {username}");
            var authorId = authorResult.Value.Id;
            DateTime currentTime = DateTime.UtcNow;
            long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeMilliseconds();

            var messageCreateDTO = new MessageCreateDTO
            {
                AuthorId = authorId,
                Text = body.content,
                PubDate = unixTime
            };

            var result = await _messageRepo.CreateAsync(messageCreateDTO);
            return NoContent();
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(504);
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserJSON body, [FromQuery(Name = "latest")] int? latest)
    {
        updateLatest(latest);
        var userCreateDTO = new UserCreateDTO
        {
            Username = body.username,
            Email = body.email
        };
        (Response response, UserDTO user) = await _userRepo.CreateAsync(userCreateDTO);
        if (response == Core.Response.Conflict) return BadRequest();
        return NoContent();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserJSON body, [FromQuery(Name = "latest")] int? latest)
    {
        updateLatest(latest);
        var username = body.username;

        if (username == null) return BadRequest();

        var user = await _userRepo.ReadByUsernameAsync(username);
        if (user.IsNone) return NotFound($"Could not find the user with username {username}");

        return Ok(new { username = username });
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUser(int? id, string? userName)
    {
        if (id != null){
            try {
                var user = await _userRepo.ReadByIDAsync((int)id);
                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(504); //504 = Gateway timeout
            }
        }
        else if (userName != null)
        {
            try
            {
                var user = await _userRepo.ReadByUsernameAsync(userName);
                return Ok(user);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(504); //504 = Gateway timeout
            }
        }
        else
        {
            return StatusCode(502); //502 = Bad gateway
        }
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var users = await _userRepo.ReadAllAsync();
            return Ok(users);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(504);
        }
    }

    [HttpPost("new-user")]
    public async Task<IActionResult> NewUser(UserCreateDTO user)
    {
        try
        {
            var result = await _userRepo.CreateAsync(user);
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(504);
        }
    }

    [HttpGet("fllws/{username}")]
    public async Task<IActionResult> GetFollowers(string username, [FromQuery(Name = "latest")] int? latest, [FromQuery(Name = "no")] int? no)
    {
        updateLatest(latest);
        //Find user Id of user with username
        Option<UserDTO> userResult;

        try
        {
            userResult = await _userRepo.ReadByUsernameAsync(username);
            if (userResult.IsNone) return NotFound($"Could not find user with name '{username}'.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "something wen terribly wrong");
        }

        //Find followers of user with userResult.Value.Id (whomId)
        try
        {
            var followersResult = (await _followerRepo.ReadAllByWhoId(userResult.Value.Id)).ToList();

            return Ok(new { follows = followersResult });
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "something wen terribly wrong");
        }
    }

    [HttpPost("fllws/{username}")]
    public async Task<IActionResult> Follow(string username, [FromBody] FollowJSON body, [FromQuery(Name = "latest")] int? latest)
    {
        updateLatest(latest);
        //Find userId;
        int userId;
        try
        {
            var userResult = await _userRepo.ReadByUsernameAsync(username);
            if (userResult.IsNone) return StatusCode(400, $"Could not find user with name '{username}'.");
            userId = userResult.Value.Id;
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            return StatusCode(500, "Internal Server Error");
        }


        //Handle request
        if (body.follow != null) //This is a follow request
        {
            try
            {
                var followUser = await _userRepo.ReadByUsernameAsync(body.follow);
                if(followUser.IsNone){
                    _logger.LogWarning($"Could not find the user: {body.follow}");
                    return StatusCode(400, "The user you are trying to follow was not found");
                }
                var user = followUser.Value;
                var followCreate = new FollowerCreateDTO
                {
                    WhoId = userId,
                    WhomId = user.Id
                };

                var result = await _followerRepo.CreateAsync(followCreate);
                if (result.Item1 == Core.Response.NotFound) return NotFound();
                if (result.Item1 != Core.Response.Created) return StatusCode(500, "Could not follow");

                return NoContent();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
        else if (body.unfollow != null) //This is an unfollow request
        {
            try
            {
                var unfollowUser = await _userRepo.ReadByUsernameAsync(body.unfollow);
                if (unfollowUser.IsNone)
                {
                    return StatusCode(400, "The user you are trying to unfollow was not found");
                }

                var foundUser = unfollowUser.Value;

                var unfollowResult = await _followerRepo.DeleteByWhoAndWhomIds(userId, foundUser.Id);
                if (unfollowResult != Core.Response.Deleted) return StatusCode(500, "Internal Server Error. Could not unfollow");

                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(500, "Unfollow error");
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
            return ((IDictionary<string, object>)obj).ContainsKey(propertyName);

        return obj.GetType().GetProperty(propertyName) != null;
  }
}
