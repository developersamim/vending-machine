using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using product.application.Contracts.Persistence;
using product.application.Models;
using MediatR;

namespace product.application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly ILogger<GetProductByIdQueryHandler> logger;
    private readonly IProductRepository productRepository;
    private readonly IMapper mapper;

    public GetProductByIdQueryHandler(ILogger<GetProductByIdQueryHandler> logger, IProductRepository productRepository, IMapper mapper)
    {
        this.logger = logger;
        this.productRepository = productRepository;
        this.mapper = mapper;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId);

        var result = mapper.Map<ProductDto>(product);

        return result;
    }
}