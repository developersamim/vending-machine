using Microsoft.Extensions.DependencyInjection;
using transaction.application.Contracts.Infrastructure;
using transaction.infrastructure.Setting;

namespace transaction.infrastructure.User;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddUserInfrastructure(this IServiceCollection services, ApiSetting apiSetting, string clientCredentialTokenKey)
    {
        services.AddHttpClient<IUserService, UserService>(o =>
        {
            o.BaseAddress = new Uri(apiSetting.UserApi);
            o.DefaultRequestHeaders.Add("User-Agent", apiSetting.UserAgent);
        })
            .SetHandlerLifetime(TimeSpan.FromSeconds(apiSetting.HandlerLifetimeMinutes))
            .AddClientAccessTokenHandler(clientCredentialTokenKey);

        return services;
    }
}
