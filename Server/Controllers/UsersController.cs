using Microsoft.AspNetCore.Mvc;
using MiniTwit.Shared;
using System.Security.Cryptography;

namespace MiniTwit.Server;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService) => _usersService = usersService;

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
        // Hash password
        string plainPassword = newUser.Password;
        string salt = PasswordEncryption.GenerateSalt(70);
        string hashedPassword = PasswordEncryption.HashPassword(plainPassword, salt, 1000, 70);
        newUser.Password = hashedPassword;
        newUser.PasswordSalt = salt;

        var status = await _usersService.CreateAsync(newUser);

        if (status == Status.Created)
        {
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);

        }
        else
        {
            return BadRequest();
        }

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

        await _usersService.RemoveAsync(user.Id!);

        return NoContent();
    }

    [HttpGet("signin/{username}&{password}")]
    public async Task<ActionResult<User>> Signin(string username, string password)
    {
        // Encrypt password to look for it in database
        string salt = await _usersService.GetSalt(username);
        string hashedPassword = PasswordEncryption.HashPassword(password, salt, 1000, 70);

        var user = await _usersService.Signin(username, hashedPassword);

        if (user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost("follow")]
    public async Task<IActionResult> Follow(FollowDTO followDTO)
    {
        if (followDTO.WhoID != null && followDTO.WhomID != null)
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
        return BadRequest();
    }

    [HttpPost("unfollow")]
    public async Task<IActionResult> Unfollow(FollowDTO followDTO)
    {
        if (followDTO.WhoID != null && followDTO.WhomID != null)
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
        return BadRequest();
    }
}

// Based on https://dev.to/1001binary/hashing-password-combining-with-salt-in-c-and-vb-net-2am9
public class PasswordEncryption
{
    public static string GenerateSalt(int saltLength)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(saltLength);
        return Convert.ToBase64String(saltBytes);
    }

    public static string HashPassword(string password, string salt, int nIterations, int hashLength)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);

        using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, nIterations))
        {
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(hashLength));
        }
    }
}