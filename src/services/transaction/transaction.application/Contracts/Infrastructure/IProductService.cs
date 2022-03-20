using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using transaction.domain;

namespace transaction.application.Contracts.Infrastructure;

public interface IProductService
{
    Task UpdateProduct(Product product);
    Task<Product> GetProductById(string id);
}
