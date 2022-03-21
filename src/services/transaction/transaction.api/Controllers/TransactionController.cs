using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using transaction.application.Features.Transactions.Commands.Deposit;
using common.utilities;
using transaction.api.Models;
using transaction.application.Features.Transactions.Commands.Buy;
using transaction.application.Features.Transactions.Commands.Reset;

namespace transaction.api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "buyer")]
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

    [HttpPost("[action]")]    
    public async Task<IActionResult> Deposit([FromBody] CreateDepositDto request)
    {
        var command = mapper.Map<DepositCommand>(request);
        command.UserId = User.UserId();

        await mediator.Send(command);

        return Ok();
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<BuyDto>> Buy([FromBody] CreateBuyDto request)
    {
        var command = mapper.Map<BuyCommand>(request);
        command.UserId = User.UserId();

        var result = await mediator.Send(command);

        return result;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Reset()
    {
        var command = new ResetCommand() { UserId = User.UserId() };
        await mediator.Send(command);

        return NoContent();
    }
}


