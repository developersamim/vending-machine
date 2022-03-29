using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using product.application.Contracts.Persistence;
using product.domain.Entities;
using System.Linq;

namespace product.application.unittest.Mock;

public class MockProductRepository
{
    public static Mock<IProductRepository> GetProductRepository()
    {
        var products = new List<Product>()
        {
            new Product
            {
                Id = "12345",
                ProductName = "coke",
                Cost = 5,
                SellerId = "32145"
            },
            new Product
            {
                Id = "22345",
                ProductName = "fanta",
                Cost = 5,
                SellerId = "32145"
            },
            new Product
            {
                Id = "32345",
                ProductName = "sprite",
                Cost = 6,
                SellerId = "22145"
            }
        };

        var mockProductRepository = new Mock<IProductRepository>();
        mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(products.First()));

        mockProductRepository.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(products);

        return mockProductRepository;
    }
}