using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using serverless.Infrastructure;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(s => {
        const string clientCredentialTokenKey = "ProductFunctionToken";
        s.AddAccessTokenManagement(options =>
        {

            var request = new ClientCredentialsTokenRequest
            {
                Address = "https://localhost:5001/connect/token",
                ClientId = "product.function",
                ClientSecret = "secret",
                Scope = "server_access"
            };
            options.Client.Clients.Add(clientCredentialTokenKey, request);
        });

        s.AddHttpClient<IProductService, ProductService>(o =>
        {
            o.BaseAddress = new Uri("http://localhost:8000");
            o.DefaultRequestHeaders.Add("User-Agent", "serverless_function");
        })
        .SetHandlerLifetime(TimeSpan.FromSeconds(300))
        .AddClientAccessTokenHandler(clientCredentialTokenKey);
    })
    .Build();

host.Run();