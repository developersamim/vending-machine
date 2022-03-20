using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using transaction.application.Features.Transactions.Commands.Deposit;
using common.utilities;
using transaction.api.Models;

namespace transaction.api.Controllers;

[ApiController]
[Route("[controller]")]    
[Authorize]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> logger;
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public TransactionController(ILogger<TransactionController> logger, IMediator mediator, IMapper mapper)
    {
        this.logger = logger;
        this.mediator = mediator;
        this.mapper = mapper;
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> Get(string productId)
    {
        return Ok("hi");
    }

    [HttpPost]
    [Authorize(Roles = "buyer")]
    public async Task<IActionResult> Deposit([FromBody] CreateDepositDto request)
    {
        var command = mapper.Map<DepositCommand>(request);
        command.UserId = User.UserId();

        await mediator.Send(command);

        return Ok();
    }
}


