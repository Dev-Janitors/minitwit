using Microsoft.AspNetCore.Mvc;
using Backend.Core.EF;
using Backend.Infrastructure;
namespace Backend.Controllers;

[ApiController]
[Route("")]

public class MinitwitController : ControllerBase
{
    private readonly ILogger<MinitwitController> _logger;

    private readonly IMessageRepository _messageRepo;
    public MinitwitController(ILogger<MinitwitController> logger, IMessageRepository messageRepo)
    {
        _logger = logger;
        _messageRepo = messageRepo;
    }

	[HttpGet("public")]
    public async Task<IActionResult> GetTimeline()
    {
        try {
            var messages = await _messageRepo.ReadAllAsync();
            return Ok(messages);
        } catch (Exception) {
            return StatusCode(500);
        }
    }

    [HttpGet("seed")]
    public async Task<IActionResult> Seed()
    {
        try {
            var messages = await _messageRepo.Seed();
            return Ok(messages);
        } catch (Exception) {
            return StatusCode(500);
        }
    }
    
}