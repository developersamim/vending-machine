using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using product.application.Contracts.Persistence;
using product.application.Features.Products.Queries.GetProducts;
using product.application.Mappings;
using product.application.Models;
using product.application.unittest.Mock;
using Shouldly;
using Xunit;

namespace product.application.unittest.Products.Queries;

public class GetProductsHandlerTest
{
	private readonly IMapper mapper;
	private readonly Mock<ILogger<GetProductsQueryHandler>> logger;
	private readonly Mock<IProductRepository> mockProductRepository;

	public GetProductsHandlerTest()
	{
		mockProductRepository = MockProductRepository.GetProductRepository();

		var configurationProvider = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile<MappingProfile>();
		});
		mapper = configurationProvider.CreateMapper();

		logger = new Mock<ILogger<GetProductsQueryHandler>>();
	}

	[Fact]
	public async Task GetProductsTest()
    {
		var handler = new GetProductsQueryHandler(mockProductRepository.Object, mapper);

		var result = await handler.Handle(new GetProductsQuery(), CancellationToken.None);

		result.ShouldBeOfType<List<ProductDto>>();
    }
}

