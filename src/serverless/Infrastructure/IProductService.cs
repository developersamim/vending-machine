using serverless.Models;

namespace serverless.Infrastructure;

public interface IProductService
{
    Task<List<ProductDto>> GetProducts();
}