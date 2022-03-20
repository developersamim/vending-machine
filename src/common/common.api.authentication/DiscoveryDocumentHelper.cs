using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace common.api.authentication;

public static class DiscoveryDocumentHelper
{
    public static DiscoveryDocumentResponse GetDiscoveryDocument(IHttpClientFactory httpClientFactory, string authority)
    {
        return httpClientFactory
            .CreateClient()
            .GetDiscoveryDocumentAsync(authority)
            .GetAwaiter()
            .GetResult();
    }

    public static DiscoveryDocumentResponse GetDiscoveryDocument(IServiceCollection services, string authority)
    {
        if (authority == null) return null;

        var sp = services.BuildServiceProvider();
        var clientFactory = sp.GetService<IHttpClientFactory>();
        var discoveryDocument = GetDiscoveryDocument(clientFactory, authority);

        if (!discoveryDocument.IsError) return discoveryDocument;

        Thread.Sleep(2000);
        discoveryDocument = GetDiscoveryDocument(clientFactory, authority);

        return discoveryDocument;
    }

}