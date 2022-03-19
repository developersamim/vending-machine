using AutoMapper;
using common.entityframework;
using MediatR;
using Microsoft.Extensions.Logging;
using product.application.Contracts.Persistence;
using product.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product.application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand>
{
    private readonly ILogger<CreateProductCommandHandler> logger;
    private readonly IProductRepository productRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public CreateProductCommandHandler(ILogger<CreateProductCommandHandler> logger, IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.logger = logger;
        this.productRepository = productRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<Unit> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = mapper.Map<Product>(request);
        product.Id = Guid.NewGuid().ToString();
        productRepository.Add(product);

        await unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
