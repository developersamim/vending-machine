using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product.application.Features.Products.Commands.UpdateProduct;

public class UpdateProductCommand : IRequest
{
    public string ProductId { get; set; }
    public string ProductName { get; set; }
    public int AmountAvailable { get; set; }
    public double Cost { get; set; }

    public string UserId { get; set; }

    public bool ByPass { get; set; } = false;
}
