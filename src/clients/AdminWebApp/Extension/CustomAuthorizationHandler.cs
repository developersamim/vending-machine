using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

public class CustomAuthorizationHandler : DelegatingHandler
{
    public ILocalStorageService localStorageService { get; set; }
    public CustomAuthorizationHandler(ILocalStorageService localStorageService)
    {
        this.localStorageService = localStorageService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // getting token from the localstorage
        var jwtToken = await localStorageService.GetItemAsync<string>("jwt_token");

        // adding tthe tooken in authorization header
        if (jwtToken is not null)
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

        // sending the request
        return await base.SendAsync(request, cancellationToken);
    }
}