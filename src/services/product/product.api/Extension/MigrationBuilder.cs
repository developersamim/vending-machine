using Microsoft.EntityFrameworkCore;
using product.infrastructure.Persisternce;

namespace product.api.Extension;

public static class MigrationBuilder
{
	private static void MigrateContext(DbContext context)
    {
        context.Database.Migrate();
    }
    public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app, bool isDevelopment, IServiceProvider serviceProvider)
    {
        using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
        if (serviceScope is null) return app;

        var applicationContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        MigrateContext(applicationContext);

        return app;
    }
}

