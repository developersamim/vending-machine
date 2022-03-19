using System;
using identity_server.IdentityServerConfig;
using identity_server.Infrastructure;
using identity_server.Models;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using IdentityModel;
using Serilog;
using static common.utilities.Constant;

namespace identity_server.Extension;

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

        var persistedContext = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
        MigrateContext(persistedContext);

        var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
        MigrateContext(context);

        if (!context.Clients.Any())
        {
            foreach(var client in Config.Clients)
            {
                context.Clients.Add(client.ToEntity());
            }

            context.SaveChanges();
        }

        // always seed a reset on each deploy
        if (isDevelopment)
        {
            foreach (var client in Config.DevelopmentClients)
            {
                var existing = context.Clients.AsNoTracking().FirstOrDefault(e => e.ClientId == client.ClientId);
                if (existing != null)
                {
                    context.Clients.Remove(existing);
                }

                context.Clients.Add(client.ToEntity());
            }

            context.SaveChanges();
        }

        if (!context.IdentityResources.Any())
        {
            foreach(var resource in Config.IdentityResources)
            {
                context.IdentityResources.Add(resource.ToEntity());
            }

            context.SaveChanges();
        }

        if (!context.ApiScopes.Any())
        {
            foreach(var scope in Config.ApiScopes)
            {
                context.ApiScopes.Add(scope.ToEntity());
            }

            context.SaveChanges();
        }

        if (!context.ApiResources.Any())
        {
            foreach(var resource in Config.ApiResources)
            {
                context.ApiResources.Add(resource.ToEntity());
            }

            context.SaveChanges();
        }

        EnsureUsers(serviceScope);
        context.SaveChanges();

        return app;
    }

    private static void EnsureUsers(IServiceScope scope)
    {
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var alice = userMgr.FindByNameAsync("alice@gmail.com").Result;
        if (alice is null)
            CreateUser(userMgr, "alice@gmail.com", "alice@gmail.com", "alice", "smith", "seller");
        else
            Log.Debug("alice already exists");

        var john = userMgr.FindByNameAsync("john@gmail.com").Result;
        if (john is null)
            CreateUser(userMgr, "john@gmail.com", "john@gmail.com", "john", "cena", "buyer");
        else
            Log.Debug("john already exists");
    }

    private static void CreateUser(UserManager<ApplicationUser> userMgr, string userName, string email, string firstName, string lastName, string role)
    {
        var applicationUser = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true
        };
        var result = userMgr.CreateAsync(applicationUser, "P@ssw0rd").Result;
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }

        result = userMgr.AddClaimsAsync(applicationUser, new Claim[]
        {
                new Claim(JwtClaimTypes.Name, string.Concat(firstName, lastName)),
                new Claim(JwtClaimTypes.GivenName, firstName),
                new Claim(JwtClaimTypes.FamilyName, lastName),
                new Claim(JwtClaimTypes.Role, role),
                //new Claim(JwtClaimTypes.Email, email),
                //new Claim(KnownUserClaim.UserName, userName),
                new Claim(KnownUserClaim.Deposit, "0")
        }).Result;
        if (!result.Succeeded)
        {
            throw new Exception(result.Errors.First().Description);
        }

        Log.Debug($"{userName} created");
    }
}

