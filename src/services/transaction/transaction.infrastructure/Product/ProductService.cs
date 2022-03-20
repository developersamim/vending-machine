using AutoMapper;
using common.infrastructure;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using transaction.application.Contracts.Infrastructure;

namespace transaction.infrastructure.Product;

public class ProductService : BaseService<IProductService>, IProductService
{
    private const string ControllerUrl = "product";

    public ProductService(ILogger<IProductService> logger, HttpClient httpClient, IMapper mapper)
        : base(logger, httpClient, mapper) { }

    public async Task<domain.Product> GetProductById(string id)
    {
        var response = await Client.GetAsync($"{ControllerUrl}/{id}");
        var result = await ValidateResponse<domain.Product>(response);

        return result;
    }

    public async Task UpdateProduct(domain.Product product)
    {
        var result = await Client.PutAsJsonAsync($"{ControllerUrl}/{product.Id}", product);
        ValidateResponse(result);
    }
}
