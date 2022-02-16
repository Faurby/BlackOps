using Microsoft.AspNetCore.Mvc;
using MiniTwit.Shared;

namespace MiniTwit.Server;

[ApiController]
[Route("sim/[controller]")]
public class SimController : ControllerBase
{
    private readonly MessagesService _messagesService;
    private readonly UsersService _usersService;


}