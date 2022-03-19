using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product.application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommand : IRequest
{
    public string ProductId { get; set; }
    public string UserId { get; set; }

    public DeleteProductCommand(string productId)
    {
        ProductId = productId;
    }
}
