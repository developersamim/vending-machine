using MediatR;
using product.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product.application.Features.Products.Commands.CreateProduct;

public class CreateProductCommand : IRequest
{
    public string ProductName { get; set; }
    public int AmountAvailable { get; set; }
    public double Cost { get; set; }

    public string SellerId { get; set; }
}
