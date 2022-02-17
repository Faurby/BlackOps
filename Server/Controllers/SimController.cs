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

    [HttpGet("/sim/latest")]
    public async Task<ActionResult<Message>> GetLatestMsg()
    {
        var messages = await _messagesService.GetAsync();

        if (messages is null)
        {
            return NotFound();
        }

        Console.WriteLine("/sim/latest text: " + messages.FirstOrDefault().Text);

        return messages.First();
    }

    [HttpPost("/sim/register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterSim user)
    {
        await _usersService.CreateAsync(user.ConvertToUser());

        Console.WriteLine("Trying to create user!");
        Console.WriteLine("Created user: " + user.username);

        return StatusCode(204, "");
    }

    [HttpGet("/sim/msgs")]
    public async Task<ActionResult<List<Message>>> GetMessages()
    {
        var messages = await _messagesService.GetAsync();

        Console.WriteLine("Returned all messages");

        return messages;
    }

    [HttpGet("/sim/msgs/{userID:length(24)}")]
    public async Task<ActionResult<List<Message>>> GetMessagesFromUser(string userID)
    {
        var messages = await _messagesService.GetMessageFromUserIDAsync(userID);

        if (messages is null)
        {
            return NotFound();
        }

        Console.WriteLine("Returned all messages of user: " + userID);

        return messages.ToList();
    }

    [HttpPost("/sim/msgs/{userID:length(24)}")]
    public async Task<ActionResult> PostMessageAsUser(string userID, Message newMessage)
    {
        newMessage.Timestamp.ToLocalTime();
        newMessage.AuthorID = userID;

        await _messagesService.CreateAsync(newMessage);

        Console.WriteLine("Posted message to user: " + userID + " msg: " + newMessage.Text);

        return CreatedAtAction(null, new { id = newMessage.Id }, newMessage);

    }

    [HttpGet("/sim/fllws/{userID:length(24)}")]
    public async Task<ActionResult<List<string>>> GetFollowersFromUser(string userID)
    {
        Console.WriteLine("Returning followers for user: " + userID);

        return await _usersService.GetFollowersAsync(userID);
    }

    //HttpPost("/fllws/{userID:length(24)}")]

    //public async Task<ActionResult> PostUserInFollowers(string userID) =>
        //await _usersService.PostFollowerAsync(userID);


}