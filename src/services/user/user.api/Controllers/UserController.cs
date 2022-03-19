using common.utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user.application.Features.Users.Commands.CreateUser;
using user.application.Features.Users.Commands.DeleteProfileElement;
using user.application.Features.Users.Commands.UpdateProfileElement;
using user.application.Features.Users.Queries.GetUserProfileByUserId;
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

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<ActionResult> GetUser()
    {
        var query = new GetUserProfileByUserIdQuery(User.UserId());
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
    public async Task<ActionResult> UpdateProfileElement([FromBody] Dictionary<string, object> profileElements)
    {
        var command = new UpdateProfileElementCommand()
        {
            UserId = User.UserId(),
            KeyValuePairs = profileElements
        };

        await mediator.Send(command);

        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteProfileElement([FromQuery] List<string> keys)
    {
        var command = new DeleteProfileElementCommand()
        {
            UserId = User.UserId(),
            Keys = keys
        };

        await mediator.Send(command);

        return NoContent();
    }
}
