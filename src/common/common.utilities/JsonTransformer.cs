using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace common.utilities;

public static class JsonTransformers
{

    public static object ParseValue(Claim claim)
    {
        if (claim.ValueType is ClaimValueTypes.Integer or ClaimValueTypes.Integer32 || int.TryParse(claim.Value, out _))
        {
            if (int.TryParse(claim.Value, out var value))
            {
                return value;
            }
        }

        if (claim.ValueType == ClaimValueTypes.Integer64 || long.TryParse(claim.Value, out _))
        {
            if (long.TryParse(claim.Value, out var value))
            {
                return value;
            }
        }

        if (claim.ValueType == ClaimValueTypes.Boolean || bool.TryParse(claim.Value, out _))
        {
            if (bool.TryParse(claim.Value, out var value))
            {
                return value;
            }
        }

        if (claim.ValueType == "json" || claim.Value.IsJson())
        {
            try
            {
                if (!claim.Value.IsJsonArray())
                    return JsonConvert.DeserializeObject<ExpandoObject>(claim.Value, new ExpandoObjectConverter());

                var instance = JsonConvert.DeserializeObject<List<object>>(claim.Value);
                return instance.Select(ParseValue);
            }
            catch
            {
            }
        }


        return claim.Value;
    }

    public static object ParseValue(object element)
    {
        if (int.TryParse(element.ToString(), out _))
        {
            if (int.TryParse(element.ToString(), out var value))
            {
                return value;
            }
        }

        if (long.TryParse(element.ToString(), out _))
        {
            if (long.TryParse(element.ToString(), out var value))
            {
                return value;
            }
        }

        if (bool.TryParse(element.ToString(), out _))
        {
            if (bool.TryParse(element.ToString(), out var value))
            {
                return value;
            }
        }

        if (element.ToString().IsJsonArray())
        {
            var instance = JsonConvert.DeserializeObject<List<object>>(element.ToString());
            return instance.Select(ParseValue);
        }


        if (element.ToString().IsJson())
        {
            try
            {
                if (!element.ToString().IsJsonArray())
                    return JsonConvert.DeserializeObject<ExpandoObject>(element.ToString(), new ExpandoObjectConverter());

                var instance = JsonConvert.DeserializeObject<List<object>>(element.ToString());
                return instance.Select(ParseValue);
            }
            catch
            {

            }
        }

        return element.ToString();
    }

    public static object ParseValue(JsonElement element)
    {
        if (element.ValueKind is JsonValueKind.Number)
        {
            if (int.TryParse(element.GetRawText(), out var value))
            {
                return value;
            }
        }

        if (element.ValueKind is JsonValueKind.False or JsonValueKind.True)
        {
            if (bool.TryParse(element.GetRawText(), out var value))
            {
                return value;
            }
        }

        if (element.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return null;
        }

        if (element.ValueKind is JsonValueKind.Object)
        {
            return JsonConvert.DeserializeObject<ExpandoObject>(element.GetRawText(), new ExpandoObjectConverter());
        }


        if (element.ValueKind is JsonValueKind.Array)
        {
            return element.EnumerateArray().Select(ParseValue).ToList();
        }

        return element.GetRawText();
    }
}