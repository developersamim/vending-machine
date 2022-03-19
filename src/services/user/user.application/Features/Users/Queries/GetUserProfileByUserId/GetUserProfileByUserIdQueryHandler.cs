using AutoMapper;
using common.exception;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Text.Json;
using user.application.Contracts.Persistence;
using user.application.Models;
using user.domain;
using user.application.Extension;

namespace user.application.Features.Users.Queries.GetUserProfileByUserId;

public class GetUserProfileByUserIdQueryHandler : IRequestHandler<GetUserProfileByUserIdQuery, UserProfileDto>
{
    private readonly ILogger<GetUserProfileByUserIdQueryHandler> logger;
    private readonly IUserRepository userRepository;
    private readonly IUserClaimRepository userClaimRepository;
    private readonly IMapper mapper;

    public GetUserProfileByUserIdQueryHandler(ILogger<GetUserProfileByUserIdQueryHandler> logger, IUserRepository userRepository, IUserClaimRepository userClaimRepository, IMapper mapper)
    {
        this.logger = logger;
        this.userRepository = userRepository;
        this.userClaimRepository = userClaimRepository;
        this.mapper = mapper;
    }

    public async Task<UserProfileDto> Handle(GetUserProfileByUserIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id);
        if (user == null)
            throw new FailedServiceException("user cannot be null");

        Expression<Func<IdentityUserClaim<string>, bool>> predicate
        = uc => uc.UserId == user.Id;
        var claims = await userClaimRepository.GetAsync(predicate);
        var result = claims.Select(uc => uc.ToClaim()).ToClaimsDictionary();

        var response = JsonSerializer.Deserialize<UserProfile>(JsonSerializer.Serialize(result));
        if(response is not null)
            response.Email = user.Email;

        return mapper.Map<UserProfileDto>(response);
    }
}
