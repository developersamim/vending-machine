using common.utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user.application.Features.Users.Commands.CreateUser;
using user.application.Features.Users.Commands.DeleteProfileElement;
using user.application.Features.Users.Commands.UpdateProfileElement;
using user.application.Features.Users.Queries.GetUserProfileByUserId;
using user.application.Features.Users.Queries.GetUsers;
using user.application.Models;

namespace user.api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> logger;
    private readonly IMediator mediator;

    public UserController(ILogger<UserController> logger, IMediator mediator)
    {
        this.logger = logger;
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetUser([FromQuery] string? userId)
    {
        if (userId == null)
            userId = User.UserId();
        var query = new GetUserProfileByUserIdQuery(userId);
        var result = await mediator.Send(query);

        return Ok(result);  
    }

    [HttpGet("all")]
    public async Task<ActionResult> GetUsers()
    {
        var query = new GetUsersQuery();
        var result = await mediator.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult> Post([FromBody] CreateUserCommand command)
    {
        await mediator.Send(command);

        return StatusCode(204);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateProfileElement([FromQuery] string? userId, [FromBody] Dictionary<string, object> profileElements)
    {
        if (userId is null)
            userId = User.UserId();

        var command = new UpdateProfileElementCommand()
        {
            UserId = userId,
            KeyValuePairs = profileElements
        };

        await mediator.Send(command);

        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteProfileElement([FromQuery] string? userId, [FromQuery] List<string> keys)
    {
        if (userId is null)
            userId = User.UserId();

        var command = new DeleteProfileElementCommand()
        {
            UserId = userId,
            Keys = keys
        };

        await mediator.Send(command);

        return NoContent();
    }
}
