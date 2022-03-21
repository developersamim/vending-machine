using common.utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using transaction.application.Contracts.Infrastructure;

namespace transaction.application.Features.Transactions.Commands.Reset;

public class ResetCommandHandler : IRequestHandler<ResetCommand>
{
    private readonly IUserService userService;

    public ResetCommandHandler(IUserService userService)
    {
        this.userService = userService;
    }

    public async Task<Unit> Handle(ResetCommand request, CancellationToken cancellationToken)
    {
        var keyValuePairs = new Dictionary<string, object>
        {
            [Constant.KnownUserClaim.Deposit] = 0
        };
        await userService.UpdateProfile(request.UserId, keyValuePairs);

        return Unit.Value;
    }
}
