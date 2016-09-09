using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.WpfApplication.Service.Lisence
{
    static class LisenceTool
    {
        public static string ToProductCodeFormat(string source)
        {
            return String.Format("{0}-{1}-{2}-{3}-{4}",
                source.Substring(0, 5),
                source.Substring(5, 5),
                source.Substring(10, 5),
                source.Substring(15, 5),
                source.Substring(20, 5));
        }

        public static string ToRawCodeFormat(string source)
        {
            return source.Replace("-", string.Empty);
        }
    }
}
