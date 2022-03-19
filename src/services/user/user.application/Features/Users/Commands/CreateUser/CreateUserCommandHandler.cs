using AutoMapper;
using common.entityframework;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using user.application.Contracts.Persistence;
using user.application.Extension;
using user.domain;
using static common.utilities.Constant;

namespace user.application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CreateUserCommandHandler(IMapper mapper,
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork
        )
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = mapper.Map<ApplicationUser>(request);

        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            result = await userManager.AddClaimsAsync(user, new Claim[]{
                    new Claim(JwtClaimTypes.Email, user.Email),
                    //new Claim(JwtClaimTypes.GivenName, user.FirstName),
                    //new Claim(JwtClaimTypes.FamilyName, userModel.LastName),
                    new Claim(KnownUserClaim.CreateDate, DateTimeOffset.UtcNow.ToString("o")),
                    new Claim(KnownUserClaim.UserName, user.UserName),
                    new Claim(KnownUserClaim.Deposit, request.Deposit.ToString()),
                    new Claim(JwtClaimTypes.Role, request.Role)
                });
        }

        await unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
