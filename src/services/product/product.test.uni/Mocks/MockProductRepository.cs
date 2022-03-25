using Moq;
using product.application.Contracts.Persistence;
using product.domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace product.test.unit.Mocks;

public class MockProductRepository : Mock<IProductRepository>
{
    public MockProductRepository MockGetById(Product result)
    {
        Setup(x => x.GetByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(result));

        return this;
    }

    public MockProductRepository MockGetAllProducts(IReadOnlyList<Product> results)
    {
        Setup(x => x.GetAllAsync())
            .Returns(Task.FromResult(results));
        
        return this;
    }
}
