using common.entityframework;
using product.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product.application.Contracts.Persistence;

public interface IProductRepository : IAsyncRepository<Product>
{
    Task<IEnumerable<Product>> GetProductByName(string name);
}
