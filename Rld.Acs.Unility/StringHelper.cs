using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.Unility
{
    public static class StringHelper
    {
        public static String ConvertToString(this string[] strings)
        {
            if (strings == null || !strings.Any())
                return string.Empty;

            return String.Join(", ", strings);
        }
    }
}
