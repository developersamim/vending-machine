using System;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using mvc_app.Setting;

namespace mvc_app.Services;

public class TokenService : ITokenService
{
    private readonly ILogger<ITokenService> logger;
    private readonly IOptions<IdentityServerSetting> identityServerSetting;
    private readonly DiscoveryDocumentResponse discoveryDocument;

	public TokenService(ILogger<ITokenService> logger, IOptions<IdentityServerSetting> identityServerSetting)
	{
        this.logger = logger;
        this.identityServerSetting = identityServerSetting;

        using var httpClient = new HttpClient();
        discoveryDocument = httpClient.GetDiscoveryDocumentAsync(identityServerSetting.Value.DiscoveryUrl).Result;
        if(discoveryDocument.IsError)
        {
            logger.LogError($"Unable to get discovery docuemnt. Error is: {discoveryDocument.Error}");
            //throw new Exception("Unable to get discovery document", discoveryDocument.Exception);
        }
	}

    public async Task<TokenResponse> GetToken(string scope)
    {
        using var httpClient = new HttpClient();
        var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        {
            Address = discoveryDocument.TokenEndpoint,

            ClientId = identityServerSetting.Value.ClientName,
            ClientSecret = identityServerSetting.Value.ClientPassword,
            Scope = scope
        });

        if (tokenResponse.IsError)
        {
            logger.LogError($"Unable to get token. Error is: {tokenResponse.Error}");
            throw new Exception("Unable to get token", tokenResponse.Exception);
        }

        return tokenResponse;
    }
}

