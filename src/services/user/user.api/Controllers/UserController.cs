using common.utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user.application.Features.Users.Queries.GetUserProfileByUserId;
using user.application.Models;

namespace user.api.Controllers
{
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
        public async Task<ActionResult<UserProfileDto>> Get()
        {
            var query = new GetUserProfileByUserIdQuery(User.UserId());
            var result = await mediator.Send(query);

            return Ok(result);  
        }
    }
}