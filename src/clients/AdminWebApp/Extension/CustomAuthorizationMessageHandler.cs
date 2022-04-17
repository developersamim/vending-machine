using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Text.Json;

public class CustomAuthorizationMessageHandler : DelegatingHandler
{
    private ILocalStorageService localStorageService { get; set; }
    private ISessionStorageService sessionStorageService { get; set; }
    public CustomAuthorizationMessageHandler(ILocalStorageService localStorageService, ISessionStorageService sessionStorageService)
    {
        this.localStorageService = localStorageService;
        this.sessionStorageService = sessionStorageService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = await GetAccessToken();

        // adding tthe token in authorization header
        if (!string.IsNullOrEmpty(accessToken))
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        // sending the request
        return await base.SendAsync(request, cancellationToken);
    }

    public async Task<string> GetAccessToken()
    {
        var tokenString = await sessionStorageService.GetItemAsync<string>("oidc.user:https://localhost:5001/:blazorWASM");
        var jDoc = JsonDocument.Parse(tokenString);
        var accessToken = jDoc.RootElement.GetProperty("access_token");
        Console.WriteLine($"mytoken: {accessToken}");
        return accessToken.ToString();
    }
}