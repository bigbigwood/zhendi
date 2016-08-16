using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rld.Acs.Unility.Extension
{
    public static class ConvertorExtension
    {
        public const Int32 ConvertionFailureValue = -9999;
        public static Int32 ToInt32(this String source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return ConvertionFailureValue;

            Int32 target;
            return Int32.TryParse(source, out target) ? target : ConvertionFailureValue;
        }

        public static Int32 ToInt32(this object source)
        {
            if (source == null) return ConvertionFailureValue;

            var sourceString = source.ToString();
            return sourceString.ToInt32();
        }
    }
}

