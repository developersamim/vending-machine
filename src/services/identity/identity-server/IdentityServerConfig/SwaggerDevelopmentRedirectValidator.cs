using System;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace identity_server.IdentityServerConfig;

// allows arbitrary redirect URIs - only for swagger local dev purposes. NEVER USE IN PRODUCTION
public class SwaggerDevelopmentRedirectValidator : IRedirectUriValidator
{
    public Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
    {
        return Task.FromResult(true);
    }

    public Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
    {
        return Task.FromResult(true);
    }
}