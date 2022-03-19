using MediatR;
using product.application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product.application.Features.Products.Queries.GetProductByName;

public class GetProductByNameQuery : IRequest<IEnumerable<ProductDto>>
{
    public string ProductName { get; set; }
}
