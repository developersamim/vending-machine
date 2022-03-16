using System;
using common.api.authentication;
using IdentityServer4.Models;

namespace identity_server.IdentityServerConfig;

public static class KnownScope
{
	public static ApiScope ServerAccess => new(AuthConstant.KnownScope.ServerAccess, "Server Access to API. Used by Services for API <-> API");
	public static ApiScope ClientAccess => new(AuthConstant.KnownScope.ClientAccess, "Client Access to API");
	public static ApiScope Role => new(AuthConstant.KnownScope.Role, "User Role");

	public static string ToScope(this ApiScope scope)
    {
		return scope.Name;
    }
}

