using common.entityframework;
using common.exception;
using common.utilities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using user.application.Contracts.Persistence;

namespace user.application.Features.Users.Commands.DeleteProfileElement;

public class DeleteProfileElementCommandHandler : IRequestHandler<DeleteProfileElementCommand>
{

    private readonly ILogger<DeleteProfileElementCommandHandler> logger;
    private readonly IUserRepository userRepository;
    private readonly IUserClaimRepository userClaimRepository;
    private readonly IUnitOfWork unitOfWork;

    public DeleteProfileElementCommandHandler(
        ILogger<DeleteProfileElementCommandHandler> logger,
        IUserRepository userRepository,
        IUserClaimRepository userClaimRepository,
        IUnitOfWork unitOfWork)
    {
        this.logger = logger;
        this.userRepository = userRepository;
        this.userClaimRepository = userClaimRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteProfileElementCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new FailedServiceException("unable to delete claim while user is null");

        var existingClaims = await userClaimRepository.GetClaimsAsync(user);
        var found = false;

        foreach (var existingClaim in request.Keys.Select(key => existingClaims.FirstOrDefault(e => e.Type.IsSame(key))).Where(existingClaim => existingClaim != null))
        {
            await userClaimRepository.RemoveClaimAsync(user, existingClaim);
            found = true;
        }

        if (found)
            await unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
