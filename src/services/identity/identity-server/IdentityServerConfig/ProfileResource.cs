using System;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using common.utilities;

namespace identity_server.IdentityServerConfig;

public class ProfileResource : IdentityResources.Profile
{
    public static string[] ResourceUserClaims = {
        JwtClaimTypes.Name,
        JwtClaimTypes.FamilyName,
        JwtClaimTypes.GivenName,
        JwtClaimTypes.MiddleName,
        JwtClaimTypes.NickName,
        JwtClaimTypes.PreferredUserName,
        JwtClaimTypes.Profile,
        JwtClaimTypes.Picture,
        JwtClaimTypes.WebSite,
        JwtClaimTypes.Gender,
        JwtClaimTypes.BirthDate,
        JwtClaimTypes.ZoneInfo,
        JwtClaimTypes.Locale,
        JwtClaimTypes.UpdatedAt,
        JwtClaimTypes.Email,
        JwtClaimTypes.EmailVerified,
        JwtClaimTypes.PhoneNumber,
        JwtClaimTypes.PhoneNumberVerified,
        //
        Constant.KnownUserClaim.CreateDate,
        Constant.KnownUserClaim.ProfileVerified,
    };

    public ProfileResource()
    {
        Name = IdentityServerConstants.StandardScopes.Profile;
        DisplayName = "User Profile";
        Description = "Your user profile information (email, first name, last name, etc.)";
        Emphasize = true;
        UserClaims = ResourceUserClaims;
    }
}

