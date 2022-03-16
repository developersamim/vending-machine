﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.utilities;

public static class IEnumerableExtension
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> list)
    {
        if (list == null)
        {
            return true;
        }

        if (!list.Any())
        {
            return true;
        }

        return false;
    }

    public static bool HasDuplicates<T, TProp>(this IEnumerable<T> list, Func<T, TProp> selector)
    {
        var d = new HashSet<TProp>();
        foreach (var t in list)
        {
            if (!d.Add(selector(t)))
            {
                return true;
            }
        }
        return false;
    }
}
