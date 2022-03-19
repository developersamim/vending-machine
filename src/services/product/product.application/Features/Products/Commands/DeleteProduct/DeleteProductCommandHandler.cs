using common.entityframework;
using common.exception;
using MediatR;
using product.application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using common.utilities;

namespace product.application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository productRepository;
    private readonly IUnitOfWork unitOfWork;

    public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        this.productRepository = productRepository;
        this.unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product == null)
            throw new FailedServiceException($"Product {request.ProductId} can not be found");

        if (request.UserId != product.SellerId)
            throw new FailedServiceException($"Delete not allowed. UserId {request.UserId} does not match with sellerId {product.SellerId}");

        productRepository.Delete(product);

        await unitOfWork.SaveChangesAsync();

        return Unit.Value;
    }
}
