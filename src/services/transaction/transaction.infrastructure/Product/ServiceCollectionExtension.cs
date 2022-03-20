using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using transaction.application.Contracts.Infrastructure;
using transaction.infrastructure.Setting;

namespace transaction.infrastructure.Product;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddProductInfrastructure(this IServiceCollection services, ApiSetting apiSetting, string clientCredentialTokenKey)
    {
        services.AddHttpClient<IProductService, ProductService>(o =>
        {
            o.BaseAddress = new Uri(apiSetting.ProductApi);
            o.DefaultRequestHeaders.Add("User-Agent", apiSetting.UserAgent);
        })
            .SetHandlerLifetime(TimeSpan.FromSeconds(apiSetting.HandlerLifetimeMinutes))
            .AddClientAccessTokenHandler(clientCredentialTokenKey);

        return services;
    }
}