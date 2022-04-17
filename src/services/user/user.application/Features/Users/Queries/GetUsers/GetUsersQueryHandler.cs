using System;
using System.Linq.Expressions;
using System.Text.Json;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using user.application.Contracts.Persistence;
using user.application.Extension;
using user.application.Models;
using user.domain;

namespace user.application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserProfileDto>>
{
    private readonly ILogger<GetUsersQueryHandler> logger;
    private readonly IUserRepository userRepository;
    private readonly IUserClaimRepository userClaimRepository;
    private readonly IMapper mapper;


	public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger, IUserRepository userRepository, IUserClaimRepository userClaimRepository, IMapper mapper)
	{
        this.logger = logger;
        this.userRepository = userRepository;
        this.userClaimRepository = userClaimRepository;
        this.mapper = mapper;
	}

    public async Task<IEnumerable<UserProfileDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var userProfileDtoList = new List<UserProfileDto>();

        var users = await userRepository.GetAllAsync();
        if (users.Any())
        {
            foreach(var user in users)
            {
                Expression<Func<IdentityUserClaim<string>, bool>> predicate = uc => uc.UserId == user.Id;
                var claims = await userClaimRepository.GetAsync(predicate);
                var result = claims.Select(uc => uc.ToClaim()).ToClaimsDictionary();

                var response = JsonSerializer.Deserialize<UserProfile>(JsonSerializer.Serialize(result));
                if (response is not null)
                    response.Email = user.Email;

                var userProfileDto = mapper.Map<UserProfileDto>(response);

                userProfileDtoList.Add(userProfileDto);
            }
        }
        return userProfileDtoList;
    }
}

