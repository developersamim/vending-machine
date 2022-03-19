using AutoMapper;
using product.api.Models;
using product.application.Features.Products.Commands.CreateProduct;
using product.application.Features.Products.Commands.UpdateProduct;

namespace product.api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UpdateProductDto, UpdateProductCommand>();
        CreateMap<CreateProductDto, CreateProductCommand>();
    }
}
