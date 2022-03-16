using System;
using AutoMapper;
using identity_server.Models;

namespace identity_server.Mappings;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<UserRegistrationModel, ApplicationUser>()
			.ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
	}
}

