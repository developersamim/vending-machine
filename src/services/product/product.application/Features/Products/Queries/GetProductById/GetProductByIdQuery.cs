using MediatR;
using product.application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product.application.Features.Products.Queries.GetProductById;

public class GetProductByIdQuery : IRequest<ProductDto>
{
    public string ProductId { get; set; }

    public GetProductByIdQuery(string productId)
    {
        ProductId = productId;
    }
}
