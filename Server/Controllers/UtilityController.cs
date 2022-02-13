using Microsoft.AspNetCore.Mvc;
using MiniTwit.Shared;

namespace MiniTwit.Server;

[ApiController]
[Route("api/[controller]")]
public class UtilityController : ControllerBase
{
    private readonly Utility _utility;

    public UtilityController(Utility utility) => _utility = utility;

    [HttpGet("md5/{str}")]
    public async Task<ActionResult<UtilityDTO>> Get(string str)
    {
        var hashed = await _utility.MD5(str);

        if (hashed is null)
        {
            return NotFound();
        }

        return hashed;
    }
}