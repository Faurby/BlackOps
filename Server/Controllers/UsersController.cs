using Microsoft.AspNetCore.Mvc;
using MiniTwit.Shared;

namespace MiniTwit.Server;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService) =>
        _usersService = usersService;

    [HttpGet]
    public async Task<List<User>> Get() =>
        await _usersService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpGet("username/{username}")]
    public async Task<ActionResult<User>> GetUsername(string username)
    {
        var user = await _usersService.GetUsernameAsync(username);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Post(User newUser)
    {
        await _usersService.CreateAsync(newUser);

        return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, User updateUser)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        updateUser.Id = user.Id;

        await _usersService.UpdateAsync(id, updateUser);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _usersService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _usersService.RemoveAsync(user.Id);

        return NoContent();
    }

    [HttpGet("signin/{username}&{password}")]
    public async Task<ActionResult<User>> Signin(string username, string password)
    {
        var user = await _usersService.Signin(username, password);

        if (user is null)
        {
            return NotFound();
        }
            return Ok(user);
    }

    [HttpPost("follow")]
    public async Task<IActionResult> Follow(FollowDTO followDTO)
    {
        var status = await _usersService.Follow(followDTO.WhoID, followDTO.WhomID);

        switch (status)
        {
            case Status.Success:
                return Ok();
            case Status.NotFound:
                return NotFound();
            default:
                return Conflict();
        }
    }

    [HttpPost("unfollow")]
    public async Task<IActionResult> Unfollow(FollowDTO followDTO)
    {
        var status = await _usersService.Unfollow(followDTO.WhoID, followDTO.WhomID);

        switch (status)
        {
            case Status.Success:
                return Ok();
            case Status.NotFound:
                return NotFound();
            default:
                return Conflict();
        }
    }
}