using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using common.utilities;
using IdentityModel;

namespace user.application.Extension;

public static class ClaimExtension
{
    public static List<Claim> ToClaims(this Dictionary<string, object> profile)
    {
        return new(profile.Select(e => new Claim(e.Key.ToSnakeCase(), GetClaimValue(e.Value))));
    }

    public static Dictionary<string, object> ToClaimsDictionary(this IEnumerable<Claim> claims)
    {
        var d = new Dictionary<string, object>();

        if (claims == null)
        {
            return d;
        }

        var distinctClaims = claims.Distinct(new ClaimComparer());

        foreach (var claim in distinctClaims)
        {
            var claimType = claim.Type.ToCamelCase();

            if (!d.ContainsKey(claim.Type))
            {
                d.Add(claimType, JsonTransformers.ParseValue(claim));
            }
            else
            {
                var value = d[claimType];

                if (value is List<object> list)
                {
                    list.Add(JsonTransformers.ParseValue(claim));
                }
                else
                {
                    d.Remove(claimType);
                    d.Add(claimType, new List<object> { value, JsonTransformers.ParseValue(claim) });
                }
            }
        }

        return d;
    }

    public static string GetClaimValue(object value)
    {
        // todo handle other complex types (arrays, json, etc...)
        var stringValue = value.ToString();

        if (stringValue.IsSame("true"))
            stringValue = "true";
        else if (stringValue.IsSame("false"))
            stringValue = "false";

        return stringValue;
    }
}
