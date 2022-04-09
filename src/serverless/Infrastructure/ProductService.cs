using Microsoft.Extensions.Logging;
using serverless.Models;
using System.Net.Http.Json;

namespace serverless.Infrastructure;

public class ProductService : IProductService
{
    private readonly ILogger<IProductService> logger;
    private readonly HttpClient httpClient;
    private const string ControllerUrl = "product";

    public ProductService(ILogger<IProductService> logger, HttpClient httpClient)
    {
        this.logger = logger;
        this.httpClient = httpClient;
    }

    public async Task<List<ProductDto>> GetProducts()
    {
        var response = await httpClient.GetAsync($"{ControllerUrl}/getproducts");
        if (!response.IsSuccessStatusCode)
            throw new Exception("Unable to get products");

        var content = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        return content;
    }
}