using common.api;
using common.api.swagger;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using transaction.api.Services;
using transaction.api.Setting;
using transaction.application;
using transaction.infrastructure;
using transaction.infrastructure.Setting;
//using transaction.infrastructure.Setting;

var builder = WebApplication.CreateBuilder(args);

ApiSetting apiSetting = builder.Services.AddAndBindConfigurationSection<ApiSetting>(builder.Configuration, "ApiSetting");
AuthenticationSetting authenticationSetting = builder.Services.AddAndBindConfigurationSection<AuthenticationSetting>(builder.Configuration, "AuthenticationSetting");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Transaction API", Version = "v1" });
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://localhost:5001/connect/authorize"),
                TokenUrl = new Uri("https://localhost:5001/connect/token"),
                Scopes = new Dictionary<string, string>
            {
                {"server_access", "Transaction API - full access"}
            }
            }
        }
    });
    options.OperationFilter<AuthorizeCheckOperationFilter>("oauth2", new string[] {"server_access"});
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.ApiName = "transaction_resource";
        options.Authority = "https://localhost:5001";
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IHttpContextAccessorService, HttpContextAccessorService>();
builder.Services.AddAutoMapper(typeof(Program));

const string clientCredentialTokenKey = "ClientAggregatorClientToken";

builder.Services.AddAccessTokenManagement(options =>
{
    var discovery = common.api.authentication.DiscoveryDocumentHelper.GetDiscoveryDocument(builder.Services, authenticationSetting.Authority);

    var request = new ClientCredentialsTokenRequest
    {
        Address = discovery.TokenEndpoint,
        ClientId = authenticationSetting.ClientId,
        ClientSecret = authenticationSetting.ClientSecret,
        Scope = authenticationSetting.Scopes
    };
    options.Client.Clients.Add(clientCredentialTokenKey, request);
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, apiSetting, clientCredentialTokenKey);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My Product API V1");

        options.OAuthClientId("transaction_api_swagger");
        options.OAuthAppName("Transaction API - Swagger");
        options.OAuthUsePkce();
    });
}

app.UseHttpsRedirection();

//app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
