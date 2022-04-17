using System;
using MediatR;
using user.application.Models;

namespace user.application.Features.Users.Queries.GetUsers;

public class GetUsersQuery : IRequest<IEnumerable<UserProfileDto>>
{
}

