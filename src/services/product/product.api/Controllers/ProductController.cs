using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using product.api.Models;
using product.application.Features.Products.Commands.CreateProduct;
using product.application.Features.Products.Commands.DeleteProduct;
using product.application.Features.Products.Commands.UpdateProduct;
using product.application.Features.Products.Queries.GetProductById;
using common.utilities;
using Microsoft.AspNetCore.Authorization;
using product.application.Features.Products.Queries.GetProductByName;
using product.application.Features.Products.Queries.GetProducts;

namespace product.api.Controllers
{
    [ApiController]
    [Route("[controller]")]    
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> logger;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public ProductController(ILogger<ProductController> logger, IMediator mediator, IMapper mapper)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(string productId)
        {
            var query = new GetProductByIdQuery(productId);
            var result = await mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProductByName([FromQuery] string name)
        {
            var query = new GetProductByNameQuery() { ProductName = name };
            var result = await mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetProducts()
        {
            var query = new GetProductsQuery();
            var result = await mediator.Send(query);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductDto request)
        {
            var command = mapper.Map<CreateProductCommand>(request);
            command.SellerId = User.UserId();
            await mediator.Send(command);
            return NoContent();
        }
        
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(string productId, [FromBody] UpdateProductDto request)
        {
            var command = mapper.Map<UpdateProductCommand>(request);
            command.ProductId = productId;
            command.UserId = User.UserId();

            await mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            var command = new DeleteProductCommand(productId);
            command.UserId = User.UserId();

            await mediator.Send(command);

            return NoContent();
        }
    }
}
