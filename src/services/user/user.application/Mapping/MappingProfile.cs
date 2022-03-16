using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using user.application.Models;
using user.domain;

namespace user.application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserProfile, UserProfileDto>();
    }
}
