using MediatR;
using product.application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product.application.Features.Products.Queries.GetProducts;

public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
{

}
