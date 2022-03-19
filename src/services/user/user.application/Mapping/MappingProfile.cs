using AutoMapper;
using user.application.Features.Users.Commands.CreateUser;
using user.application.Models;
using user.domain;

namespace user.application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserProfile, UserProfileDto>();
        CreateMap<CreateUserCommand, ApplicationUser>()
            .ForMember(d =>
                d.UserName,
                opt => opt.MapFrom(s => s.Email));
    }
}
