using common.exception;
using common.utilities;
using MediatR;
using System.Text.Json;
using transaction.application.Contracts.Infrastructure;

namespace transaction.application.Features.Transactions.Commands.Deposit;

public class DepositCommandHandler : IRequestHandler<DepositCommand>
{
    private readonly IUserService userService;

    public DepositCommandHandler(IUserService userService)
    {
        this.userService = userService;
    }

    public async Task<Unit> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        if (!AcceptedCent.CheckCent(request.Cent))
            throw new FailedServiceException($"Cent {request.Cent} not accepted. Accepted coins are {JsonSerializer.Serialize(AcceptedCent.cents)}");

        var userProfile = await userService.GetProfile(request.UserId);

        if (userProfile == null)
            throw new FailedServiceException("user profile cannot be found");

        var keyValuePairs = new Dictionary<string, object>
        {
            [Constant.KnownUserClaim.Deposit] = userProfile.Deposit + request.Cent,
        };

        await userService.UpdateProfile(request.UserId, keyValuePairs);

        return Unit.Value;
    }
}

