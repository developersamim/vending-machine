using AutoMapper;
using transaction.domain;
using transaction.application.Features.Transactions.Commands.Buy;

namespace transaction.application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>();
    }
}
