using AutoMapper;
using common.entityframework;
using MediatR;
using product.application.Contracts.Persistence;
using product.application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product.application.Features.Products.Queries.GetProductByName;

public class GetProductByNameQueryHandler : IRequestHandler<GetProductByNameQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository productRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public GetProductByNameQueryHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.productRepository = productRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetProductByName(request.ProductName);
        var result = mapper.Map<IEnumerable<ProductDto>>(products);

        return result;
    }
}
