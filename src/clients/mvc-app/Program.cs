using mvc_app.Services;
using mvc_app.Setting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<IdentityServerSetting>(builder.Configuration.GetSection("IdentityServerSetting"));
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "cookie";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("cookie", options =>
    {
        options.SlidingExpiration = true;
    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["InteractiveServiceSetting:AuthorityUrl"];
        options.ClientId = builder.Configuration["InteractiveServiceSetting:ClientId"];
        options.ClientSecret = builder.Configuration["InteractiveServiceSetting:ClientSecret"];

        options.ResponseType = "code";
        options.UsePkce = true;
        options.ResponseMode = "query";

        options.Scope.Add(builder.Configuration["InteractiveServiceSetting:Scopes:0"]);
        options.SaveTokens = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
