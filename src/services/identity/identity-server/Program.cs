using identity_server.Extension;
using identity_server.IdentityServerConfig;
using identity_server.Infrastructure;
using identity_server.Models;
using IdentityServer4.Models;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

// cookie policy to deal with temporary browser incompatibilities
builder.Services.AddSameSiteCookiePolicy();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var migrationAssembly = typeof(Program).Assembly.GetName().Name;

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssembly));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(e =>
{
    e.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<ApplicationUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationAssembly));
    })
    .AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program));

// add CORS policy
builder.Services.AddCors(options =>
{
    options.DefaultPolicyName = "api";
    options.AddPolicy("api", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddTransient<IRedirectUriValidator, SwaggerDevelopmentRedirectValidator>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors(x => x
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .SetIsOriginAllowed(origin => true) // allow any origin
                   .WithOrigins(
                        "http://localhost",
                        "http://localhost:8000",
                        "http://localhost:8100",
                        "http://localhost:8200"));
}

app.InitializeDatabase(app.Environment.IsDevelopment(), app.Services);

app.UseStaticFiles();
app.UseRouting();

app.UseHttpsRedirection();

app.UseIdentityServer();

app.UseAuthorization();

app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

app.Run();
