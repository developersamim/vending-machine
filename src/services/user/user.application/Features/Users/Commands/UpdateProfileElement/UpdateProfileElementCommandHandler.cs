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
using user.application.Extension;

namespace user.application.Features.Users.Commands.UpdateProfileElement;

public class UpdateProfileElementCommandHandler : IRequestHandler<UpdateProfileElementCommand>
{
    private readonly IUserRepository userRepository;
    private readonly IUserClaimRepository userClaimRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly ILogger<UpdateProfileElementCommandHandler> logger;

    public UpdateProfileElementCommandHandler(IUserRepository userRepository, IUserClaimRepository userClaimRepository, IUnitOfWork unitOfWork, ILogger<UpdateProfileElementCommandHandler> logger)
    {
        this.userRepository = userRepository;
        this.userClaimRepository = userClaimRepository;
        this.unitOfWork = unitOfWork;
        this.logger = logger;
    }



    public async Task<Unit> Handle(UpdateProfileElementCommand request, CancellationToken cancellationToken)
    {
        var values = request.KeyValuePairs.ToDictionary(item => item.Key, item => item.Value);

        if (values.Count <= 0) return Unit.Value;

        var user = await userRepository.GetByIdAsync(request.UserId);
        if (user == null)
            throw new FailedServiceException("user cannot be null");

        var existingClaims = await userClaimRepository.GetClaimsAsync(user);

        var newClaims = values.ToClaims();

        foreach (var newClaim in newClaims)
        {
            var existingClaim = existingClaims.FirstOrDefault(e => e.Type.IsSame(newClaim.Type));
            if (existingClaim != null)
            {
                await userClaimRepository.ReplaceClaimAsync(user, existingClaim, newClaim);
            }
            else
            {
                await userClaimRepository.AddClaimAsync(user, newClaim);
            }
        }

        await unitOfWork.SaveChangesAsync();

        return Unit.Value; 
    }
}
