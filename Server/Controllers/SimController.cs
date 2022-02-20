using Microsoft.AspNetCore.Mvc;
using MiniTwit.Shared;

namespace MiniTwit.Server;

[ApiController]
[Route("sim/[controller]")]
public class SimController : ControllerBase
{
    private readonly MessagesService _messagesService;
    private readonly UsersService _usersService;

    private int _latest;

    public SimController(MessagesService messagesservice, UsersService usersService)
    {
        _messagesService = messagesservice;
        _usersService = usersService;
    }

    [HttpGet("/sim/latest")]
    public async Task<int> GetLatest()
    {
        return _latest;
    }

    [HttpPost("/sim/register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterSim user, [FromQuery(Name = "latest")] int? latestMessage)
    {
        var status = await _usersService.CreateAsync(user.ConvertToUser());

        await UpdateLatest(latestMessage);

        if (status == Status.Created)
        {
            return NoContent();
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpGet("/sim/msgs")]
    public async Task<ActionResult<List<Message>>> GetMessages() => await _messagesService.GetAsync();

    [HttpGet("/sim/msgs/{userID:length(24)}")]
    public async Task<ActionResult<List<Message>>> GetMessagesFromUser(string userID)
    {
        var messages = await _messagesService.GetMessageFromUserIDAsync(userID);

        if (messages is null)
        {
            return NotFound();
        }

        return messages.ToList();
    }

    [HttpPost("/sim/msgs/{username}")]
    public async Task<ActionResult> PostMessageAsUser(string username, [FromBody] MessageSim newMessage, [FromQuery(Name = "latest")] int? latestMessage)
    {
        await UpdateLatest(latestMessage);

        var status = await _messagesService.CreateAsync(await ConvertToMessage(newMessage, username));

        if (status == Status.Created)
        {
            return NoContent();
        }
        else
        {
            return Unauthorized();
        }
    }

    [HttpPost("/sim/fllws/{username}")]
    public async Task<ActionResult> FollowUser(
        string username,
        [FromQuery(Name = "latest")] int? latestMessage,
        [FromBody] FollowSim followSim)
    {
        await UpdateLatest(latestMessage);

        string whoID = (await _usersService.GetUsernameAsync(username)).Id;

        if (whoID == null)
        {
            return NotFound();
        }
        else
        {
            // whoID exists in DB, wanna follow/unfollow whomID.
            // Since we cant create 2 different HTTP Post endpoints,
            // We must create a single one. Therefore we merge the 2 "DTO"'s into 1.
            // Here we can check if unfollow is null, therefore follow must be set.

            // Unfollow another user
            if (followSim.unfollow != null)
            {
                var whomID = (await _usersService.GetUsernameAsync(followSim.unfollow)).Id;
                await _usersService.Unfollow(whoID, whomID);
            }
            // Follow another user
            else
            {
                var whomID = (await _usersService.GetUsernameAsync(followSim.follow)).Id;
                await _usersService.Follow(whoID, whomID);
            }
            return NoContent();
        }
    }

    //HttpPost("/fllws/{userID:length(24)}")]

    //public async Task<ActionResult> PostUserInFollowers(string userID) =>
    //await _usersService.PostFollowerAsync(userID);

    public async Task<Message> ConvertToMessage(MessageSim newMessage, string username)
    {
        var message = new Message();

        message.AuthorID = (await _usersService.GetUsernameAsync(username)).Id;
        message.AuthorName = username;
        message.Text = newMessage.content;
        message.Timestamp = DateTime.Now;

        return message;
    }

    public async Task UpdateLatest(int? latest)
    {
        if (latest != null)
        {
            _latest = (int)latest;
        }
    }
}