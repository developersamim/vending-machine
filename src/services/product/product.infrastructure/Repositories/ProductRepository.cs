using common.entityframework;
using Microsoft.EntityFrameworkCore;
using product.application.Contracts.Persistence;
using product.domain.Entities;
using product.infrastructure.Persisternce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product.infrastructure.Repositories;

public class ProductRepository : BaseRepositoryWithEntityBase<Product, ApplicationDbContext>, IProductRepository
{
    public ProductRepository(ApplicationDbContext dbContext)
        : base(dbContext) { }

    public async Task<IEnumerable<Product>> GetProductByName(string name)
    {
        var products = await DbContext.Product.Where(o => o.ProductName == name).ToListAsync();

        return products;
    }
}
