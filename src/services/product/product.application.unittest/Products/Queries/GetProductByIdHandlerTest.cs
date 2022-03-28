using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using product.application.Contracts.Persistence;
using product.application.Features.Products.Queries.GetProductById;
using product.application.Mappings;
using product.application.Models;
using product.application.unittest.Mocks;
using Xunit;
using Shouldly;

namespace product.application.unittest.Products.Queries;

public class GetProductByIdHandlerTest
{
	private readonly IMapper mapper;
	private readonly Mock<IProductRepository> mockProductRepository;
	private readonly Mock<ILogger<GetProductByIdQueryHandler>> logger;

	public GetProductByIdHandlerTest()
    {
		mockProductRepository = new MockProductRepository();

		var configurationProvider = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile<MappingProfile>();
		});

		mapper = configurationProvider.CreateMapper();

		logger = new Mock<ILogger<GetProductByIdQueryHandler>>();
    }

	[Fact]
	public async Task GetProductByIdTest()
    {
		var handler = new GetProductByIdQueryHandler(logger.Object, mockProductRepository.Object, mapper);

		var result = await handler.Handle(new GetProductByIdQuery("1234"), CancellationToken.None);

		result.ShouldBeOfType<ProductDto>();
    }
}

