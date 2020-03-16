using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiscountCatalog.WebAPI.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string str, string value, StringComparer comparer)
        {
            StringComparison comparison;
            if (comparer == StringComparer.CurrentCulture)
                comparison = StringComparison.CurrentCulture;
            else if (comparer == StringComparer.CurrentCultureIgnoreCase)
                comparison = StringComparison.CurrentCultureIgnoreCase;
            else if (comparer == StringComparer.InvariantCulture)
                comparison = StringComparison.InvariantCulture;
            else if (comparer == StringComparer.InvariantCultureIgnoreCase)
                comparison = StringComparison.InvariantCultureIgnoreCase;
            else if (comparer == StringComparer.Ordinal)
                comparison = StringComparison.Ordinal;
            else if (comparer == StringComparer.OrdinalIgnoreCase)
                comparison = StringComparison.OrdinalIgnoreCase;
            else
                comparison = StringComparison.Ordinal;

            if (str == null)
            {
                str = "";
            }

            if (str.IndexOf(value, comparison) != -1)
                return true;
            else
                return false;
        }
    }
}