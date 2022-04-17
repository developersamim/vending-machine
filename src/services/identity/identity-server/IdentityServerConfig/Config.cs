using System;
using System.Security.Claims;
using System.Text.Json;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Identity;

namespace identity_server.IdentityServerConfig;

public static class Config
{
    public static List<TestUser> Users
    {
        get
        {
            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = 69118,
                country = "Germany"
            };

            return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "818727",
                        Username = "alice",
                        Password = "alice",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.Name, "Alice Smith"),
                            new Claim(JwtClaimTypes.GivenName, "Alice"),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                            new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                            new Claim(JwtClaimTypes.Role, "admin"),
                            new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                            new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address), IdentityServerConstants.ClaimValueTypes.Json)
                        }
                    },
                    new TestUser
                     {
                        SubjectId = "88421113",
                        Username = "bob",
                        Password = "bob",
                        Claims =
                        {
                          new Claim(JwtClaimTypes.Name, "Bob Smith"),
                          new Claim(JwtClaimTypes.GivenName, "Bob"),
                          new Claim(JwtClaimTypes.FamilyName, "Smith"),
                          new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                          new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                          new Claim(JwtClaimTypes.Role, "user"),
                          new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                          new Claim(JwtClaimTypes.Address, JsonSerializer.Serialize(address),
                            IdentityServerConstants.ClaimValueTypes.Json)
                        }
                     }
                };
        }
    }

    public static IEnumerable<IdentityResource> IdentityResources =>
        new[]
        {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "role",
                    UserClaims = new List<string> { "role" }
                }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new[]
        {
                KnownScope.ServerAccess,
                KnownScope.ClientAccess
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
                // ensure user claims are in access token if profile scope is used
                new ApiResource(
                    IdentityServerConstants.StandardScopes.Profile,
                    "Profile Scope",
                    ProfileResource.ResourceUserClaims)
                {
                    Scopes = { IdentityServerConstants.StandardScopes.Profile }
                },

                new ApiResource("product_resource", "product api")
                {
                    Scopes = new List<string> {
                        KnownScope.ServerAccess.ToScope(),
                        KnownScope.ClientAccess.ToScope(),
                        KnownScope.Role.ToScope(),
                        IdentityServerConstants.StandardScopes.OpenId
                    },
                    //ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256())},
                    UserClaims = new List<string> {"role", "email"}
                },
                new ApiResource("user_resource", "user api")
                {
                    Scopes = new List<string> {
                        KnownScope.ServerAccess.ToScope(),
                        KnownScope.ClientAccess.ToScope(),
                        KnownScope.Role.ToScope(),
                        IdentityServerConstants.StandardScopes.OpenId
                    },
                    //ApiSecrets = new List<Secret> { new Secret("ScopeSecret".Sha256())},
                    UserClaims = new List<string> {"role", "email"}
                },
                new ApiResource("transaction_resource", "transaction api")
                {
                    Scopes= new List<string> {
                        KnownScope.ServerAccess.ToScope(),
                        KnownScope.ClientAccess.ToScope(),
                        KnownScope.Role.ToScope(),
                        IdentityServerConstants.StandardScopes.OpenId
                    },
                    UserClaims = new List<string> {"role", "email"}
                },
                //new ApiResource("product_function", "product function")
                //{
                //    Scopes= new List<string>
                //    {
                //        KnownScope.ServerAccess.ToScope(),
                //        KnownScope.ClientAccess.ToScope(),
                //        KnownScope.Role.ToScope(),
                //        IdentityServerConstants.StandardScopes.OpenId
                //    },
                //    UserClaims = new List<string> {"role", "email"}
                //}
        };

    public static IEnumerable<Client> DevelopmentClients =>
        new List<Client>
        {
                new()
                {
                    ClientId = "development.swagger",
                    ClientName = "Swagger UI for Development Identity Server",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris = { "https://notused" },
                    PostLogoutRedirectUris = { "https://notused" },
                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                    RequirePkce = true,

                    AllowOfflineAccess = true,
                    AllowedCorsOrigins = new List<string>() { "http://localhost", "https://localhost" },
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,

                    AllowedScopes = {
                        KnownScope.ClientAccess.ToScope(),
                        KnownScope.ServerAccess.ToScope(),
                        KnownScope.Role.ToScope(),
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                    }
                }
        };

    public static async Task CreateRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var adminRoleExists = await roleManager.RoleExistsAsync("Admin");
        if (!adminRoleExists) await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
                new()
                {
                    ClientName = "vending-machine app",
                    ClientId = "vending-machine.app",
                
                    // RequireConsent = false,
                    AccessTokenLifetime = 3600, // 60 minutes
                    IdentityTokenLifetime = 36000, // 10 hours

                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    RefreshTokenExpiration = TokenExpiration.Sliding,

                    AlwaysIncludeUserClaimsInIdToken = true,
                    AllowAccessTokensViaBrowser = true,

                    UpdateAccessTokenClaimsOnRefresh = true,

                    RedirectUris = new List<string>
                    {
                        "http://localhost:4200/welcome",
                        "http://localhost:4200/login",
                        "http://localhost:4200/assets/silent-renew.html",
                        "http://localhost:8100/login",
                        "http://localhost:8100/welcome",
                        "http://localhost:8100/assets/silent-renew.html",
                        "barnyard.app://welcome",
                        "barnyard.app://login",
                        "barnyard.app://assets/silent-renew.html",
                        "https://barnyard.app/welcome",
                        "https://barnyard.app/login",
                        "https://barnyard.app/assets/silent-renew.html",

                        "https://oauth.pstmn.io/v1/callback"
                    },
                    PostLogoutRedirectUris = new List<string>
                    {
                        "http://localhost:4200",
                        "http://localhost:4200/welcome",
                        "http://localhost:4200/login",
                        "http://localhost:8100",
                        "http://localhost:8100/welcome",
                        "http://localhost:8100/login",
                        "https://barnyard.app",
                        "https://barnyard.app/welcome",
                        "https://barnyard.app/login",
                        "barnyard.app://",
                        "barnyard.app://welcome",
                        "barnyard.app://login",
                    },
                    AllowedCorsOrigins = new List<string>
                    {
                        "http://localhost:4200",
                        "http://localhost:8100",
                        "https://barnyard.app"
                    },
                    AllowedScopes = new List<string>
                    {
                        KnownScope.ClientAccess.ToScope(),
                        KnownScope.Role.ToScope(),
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    }

                },
                new Client
                {
                    ClientId = "product.api",
                    ClientName = "Product API",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256())},

                    AllowedScopes =
                    {
                        KnownScope.ServerAccess.ToScope(),
                        KnownScope.ClientAccess.ToScope(),
                        KnownScope.Role.ToScope(),
                        IdentityServerConstants.StandardScopes.OpenId
                    }
                },
                new Client
                {
                    ClientId = "transaction.api",
                    ClientName = "Transaction API",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256())},

                    AllowedScopes =
                    {
                        KnownScope.ServerAccess.ToScope(),
                        KnownScope.ClientAccess.ToScope(),
                        KnownScope.Role.ToScope(),
                        IdentityServerConstants.StandardScopes.OpenId
                    }
                },
                new Client
                {
                    ClientId = "product.function",
                    ClientName = "Product Function",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256())},

                    AllowedScopes =
                    {
                        KnownScope.ServerAccess.ToScope(),
                        KnownScope.ClientAccess.ToScope(),
                        KnownScope.Role.ToScope(),
                        IdentityServerConstants.StandardScopes.OpenId
                    }
                },
                new Client
                {
                    ClientId = "mvc-app.client",
                    ClientName = "MVC Client App",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret("secret".Sha256())},

                    AllowedScopes =
                    {
                        //KnownScope.ServerAccess.ToScope(),
                        KnownScope.ClientAccess.ToScope()
                    }
                },
                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("secret".Sha256())},

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = {"https://localhost:9001/signin-oidc"},
                    FrontChannelLogoutUri = "https://localhost:9001/signout-oidc",
                    PostLogoutRedirectUris = {"https://localhost:9001/signout-callback-oidc"},

                    AllowOfflineAccess = true,
                    AllowedScopes = {
                        "openid",
                        "profile",
                        KnownScope.ClientAccess.ToScope(),
                        KnownScope.Role.ToScope()
                    },
                    RequirePkce = true,
                    RequireConsent = true,
                    AllowPlainTextPkce = false
                },
                // blazor wasm AdminWebApp
                new Client
                {
                    ClientId = "blazorWASM",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedCorsOrigins =
                    {
                        "https://localhost:7186"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        KnownScope.ClientAccess.ToScope()
                    },
                    RedirectUris =
                    {
                        "https://localhost:7186/authentication/login-callback"
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:7186/authentication/logout-callback"
                    }
                }
        };
}

