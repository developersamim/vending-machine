using MediatR;
using user.application.Models;

namespace user.application.Features.Users.Queries.GetUserProfileByUserId;

public class GetUserProfileByUserIdQuery : IRequest<UserProfileDto>
{
    public string Id { get; set; }

    public GetUserProfileByUserIdQuery(string id)
    {
        Id = id;
    }
}
