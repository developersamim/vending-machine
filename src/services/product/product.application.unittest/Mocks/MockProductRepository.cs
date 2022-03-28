using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using product.application.Contracts.Persistence;
using product.domain.Entities;

namespace product.application.unittest.Mocks;

public class MockProductRepository : Mock<IProductRepository>
{
	public MockProductRepository MockGetById(Product product)
	{
		Setup(x => x.GetByIdAsync(It.IsAny<string>()))
			.Returns(Task.FromResult(product));

		return this;
	}

	public MockProductRepository MockGetAllProducts(IReadOnlyList<Product> products)
    {
		Setup(x => x.GetAllAsync())
			.Returns(Task.FromResult(products));

		return this;
    }
}

