using AutoMapper;
using MediatR;
using product.application.Contracts.Persistence;
using product.application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product.application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository productRepository;
    private readonly IMapper mapper;

    public GetProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        this.productRepository = productRepository;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await productRepository.GetAllAsync();

        var response = mapper.Map<IEnumerable<ProductDto>>(result);

        return response;
    }
}
