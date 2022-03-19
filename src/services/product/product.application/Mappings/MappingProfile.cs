using AutoMapper;
using product.application.Features.Products.Commands.CreateProduct;
using product.application.Models;
using product.domain.Entities;

namespace product.application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateProductCommand, Product>();
        CreateMap<Product, ProductDto>();
    }
}
