using common.api.swagger;
using common.exception;
using Microsoft.OpenApi.Models;
using product.api;
using product.api.Extension;
using product.api.Services;
using product.application;
using product.infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Product API", Version = "v1" });
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
                {"server_access", "Product API - full access"}
            }
            }
        }
    });
    options.OperationFilter<AuthorizeCheckOperationFilter>("oauth2", new string[] {"server_access"});
});

builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.ApiName = "product_resource";
        options.Authority = "https://localhost:5001";
    });

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IHttpContextAccessorService, HttpContextAccessorService>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My Product API V1");

        options.OAuthClientId("product_api_swagger");
        options.OAuthAppName("Product API - Swagger");
        options.OAuthUsePkce();
    });
}
app.ConfigureCustomExceptionHandler();

app.InitializeDatabase(app.Environment.IsDevelopment(), app.Services);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
