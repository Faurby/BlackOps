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

    [HttpGet("/msgs")]
    public async Task<ActionResult<List<Message>>> GetMessages()
    {
        var messages = await _messagesService.GetAsync();

        return messages;
    }

    [HttpGet("/msgs/{userID:length(24)}")]
    public async Task<ActionResult<List<Message>>> GetMessagesFromUser(string userID)
    {
        var messages = await _messagesService.GetMessageFromUserIDAsync(userID);

        if (messages is null)
        {
            return NotFound();
        }

        return messages.ToList();
    }

    [HttpPost("/msgs/{userID:length(24)}")]
    public async Task<ActionResult> PostMessageAsUser(string userID, Message newMessage)
    {
        newMessage.Timestamp.ToLocalTime();
        newMessage.AuthorID = userID;

        await _messagesService.CreateAsync(newMessage);

        return CreatedAtAction(null, new { id = newMessage.Id }, newMessage);

    }

    [HttpGet("/fllws/{userID:length(24)}")]
    public async Task<ActionResult<List<string>>> GetFollowersFromUser(string userID) =>
        await _usersService.GetFollowersAsync(userID);

    //[HttpGet("/fllws/{userID:length(24)}")]

    //public async Task<ActionResult> PostUserInFollowers(string userID) =>
        //await _usersService.PostFollowerAsync(userID);


}