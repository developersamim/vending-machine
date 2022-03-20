using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using transaction.infrastructure.Product;
using transaction.infrastructure.Setting;
using transaction.infrastructure.User;

namespace transaction.infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, ApiSetting apiSetting, string clientCredentialTokenKey)
    {
        services.AddUserInfrastructure(apiSetting, clientCredentialTokenKey);
        services.AddProductInfrastructure(apiSetting, clientCredentialTokenKey);

        return services;
    }
}
