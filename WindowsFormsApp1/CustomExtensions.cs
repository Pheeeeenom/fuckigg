using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fuckIGG
{
    public static class CustomExtensions
    {
        public static bool ContainsIgnoreCase(this string src, string match)
        {
            if (string.IsNullOrWhiteSpace(src) || string.IsNullOrWhiteSpace(match))
                return false;

            return src.IndexOf(match, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
