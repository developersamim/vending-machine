using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace common.utilities;

public static class StringExtension
{
    public static string ToSnakeCase(this string value)
    {
        return string.Concat(value.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
    }

    public static string ToCamelCase(this string value)
    {
        return Regex.Replace(value, "_[a-z]", m => m.ToString().TrimStart('_').ToUpper());
    }

    public static bool IsJson(this string value)
    {
        return !value.IsNullOrEmpty() &&
               (value.StartsWith("{") && value.EndsWith("}")) || //For object
               (value.StartsWith("[") && value.EndsWith("]"));
    }

    public static bool IsJsonArray(this string value)
    {
        return !value.IsNullOrEmpty() &&
               (value.StartsWith("[") && value.EndsWith("]"));
    }

    public static bool IsSame(this string value, string value2)
    {
        return string.Equals(value, value2, System.StringComparison.InvariantCultureIgnoreCase);
    }
}
