using common.api.swagger;
using common.exception;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using user.application;
using user.domain;
using user.infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "User API", Version = "v1" });
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(builder.Configuration["SwaggerConfiguration:Auth:AuthorizeEndpoint"]),
                TokenUrl = new Uri(builder.Configuration["SwaggerConfiguration:Auth:TokenEndpoint"]),
                Scopes = new Dictionary<string, string>
                {
                    {builder.Configuration["SwaggerConfiguration:Auth:Scopes:0"], "User API - full access" }
                }
            }
        }
    });
    options.OperationFilter<AuthorizeCheckOperationFilter>("oauth2", builder.Configuration.GetSection("SwaggerConfiguration:Auth:Scopes")?.GetChildren()?.Select(x => x.Value)?.ToArray());
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddIdentityServerAuthentication("Bearer", options =>
    {
        options.ApiName = builder.Configuration["AuthenticationSetting:ApiName"];
        options.Authority = builder.Configuration["AuthenticationSetting:Authority"];
    });

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy
                .WithOrigins("https://localhost:7186")
                .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "My User API V1");

        options.OAuthClientId("user_api_swagger");
        options.OAuthAppName("User API - Swagger");
        options.OAuthUsePkce();
    });
}
app.ConfigureCustomExceptionHandler();

//app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
