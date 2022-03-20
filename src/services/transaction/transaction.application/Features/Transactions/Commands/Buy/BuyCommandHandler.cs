using AutoMapper;
using common.exception;
using common.utilities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using transaction.application.Contracts.Infrastructure;
using transaction.domain;

namespace transaction.application.Features.Transactions.Commands.Buy;

public class BuyCommandHandler : IRequestHandler<BuyCommand, BuyDto>
{
    private readonly IProductService productService;
    private readonly IUserService userService;
    private readonly IMapper mapper;

    public BuyCommandHandler(IProductService productService, IUserService userService, IMapper mapper)
    {
        this.productService = productService;
        this.userService = userService;
        this.mapper = mapper;
    }

    public async Task<BuyDto> Handle(BuyCommand request, CancellationToken cancellationToken)
    {
        var product = await productService.GetProductById(request.ProductId);
        if (product == null)
            throw new FailedServiceException($"Product {request.ProductId} cannot be found");

        var stock = product.AmountAvailable;
        stock -= request.Amount;
        if (stock < 0)
            throw new FailedServiceException($"Available stock is {product.AmountAvailable}, which is less than requested amount {request.Amount}");

        var userProfile = await userService.GetProfile(request.UserId);
        if (userProfile is null)
            throw new FailedServiceException("user cannot be found");
        double cost = request.Amount * product.Cost;
        if (cost > userProfile.Deposit)
            throw new FailedServiceException($"Total cost of this buy is {cost}, which is greater than deposit {userProfile.Deposit}");

        var keyValuePairs = new Dictionary<string, object>
        {
            [Constant.KnownUserClaim.Deposit] = userProfile.Deposit - cost
        };
        await userService.UpdateProfile(request.UserId, keyValuePairs);

        product.AmountAvailable = stock;
        await productService.UpdateProduct(product);

        return new BuyDto()
        {
            Spent = cost,
            Product = mapper.Map<ProductDto>(product)
        };
    }
}
