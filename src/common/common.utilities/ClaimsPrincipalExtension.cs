using IdentityModel;
using System;
using System.Security.Claims;

namespace common.utilities;

public static class ClaimsPrincipalExtension
{
    public static string UserId(this ClaimsPrincipal principal)
    {
        return principal.FindFirst(JwtClaimTypes.Subject)?.Value
                   ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
