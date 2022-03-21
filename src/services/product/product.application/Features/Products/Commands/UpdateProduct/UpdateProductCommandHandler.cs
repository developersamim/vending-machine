using AutoMapper;
using common.entityframework;
using common.exception;
using MediatR;
using product.application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product.application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository productRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public UpdateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.productRepository = productRepository;
        this.unitOfWork = unitOfWork;
        this.mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            throw new FailedServiceException($"Product {request.ProductId} cannot be found");

        if(request.Cost > 0)
        {
            if (request.Cost % 5 != 0)
                throw new FailedServiceException($"Cost should only be multiple of 5");
        }

        if (request.ByPass == false && request.UserId != product.SellerId)
            throw new FailedServiceException($"Update not allowed. UserId {request.UserId} does not match with sellerId {product.SellerId}");

        if(!string.IsNullOrEmpty(request.ProductName))
            product.ProductName = request.ProductName;
        if (request.Cost > 0)
            product.Cost = request.Cost;
        if(request.AmountAvailable > 0)
            product.AmountAvailable = request.AmountAvailable;        

        productRepository.Update(product);

        await unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
