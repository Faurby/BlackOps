using Microsoft.AspNetCore.Mvc;
using MiniTwit.Shared;

namespace MiniTwit.Server;

[ApiController]
[Route("sim/[controller]")]
public class SimController : ControllerBase
{
    private readonly MessagesService _messagesService;
    private readonly UsersService _usersService;

    public SimController(MessagesService messagesservice, UsersService usersService)
    {
        _messagesService = messagesservice;
        _usersService = usersService;
    }

    [HttpGet("/latest")]
    public async Task<ActionResult<Message>> GetLatestMsg()
    {
        var messages = await _messagesService.GetAsync();

        if (messages is null)
        {
            return NotFound();
        }

        return messages.First();
    }

    [HttpPost("/register")]
    public async Task<IActionResult> RegisterUser(User user)
    {
        await _usersService.CreateAsync(user);

        return NoContent();
    }



}