using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Rld.Acs.Unility.Extension;

namespace Rld.Acs.Unility
{
    public static class ValidatorToolkit
    {
        public static bool IsPostCodeFormat(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;
            return Regex.IsMatch(str, @"^\d{6}$");
        }

        public static bool IsNumeric(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;
            return Regex.IsMatch(str, @"^[0-9]*$");
        }

        public static bool IsNumberOrChar(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;
            return Regex.IsMatch(str, "^[a-zA-Z0-9]*$");
        }

        public static bool IsPhoneNumberFormat(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;
            return Regex.IsMatch(str, "^[0-9\\-]*$");
        }

        public static bool IsIDCardFormat(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;
            return Regex.IsMatch(str, @"(^\d{17}(?:\d|x)$)|(^\d{15}$)");
        }

        public static bool IsTimeSegmentFormat(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;

            var hour = str.Substring(0, 2);
            var minute = str.Substring(3, 2);

            int intHour = 0;
            var result1 = int.TryParse(hour, out intHour);

            int intMinute = 0;
            var result2 = int.TryParse(minute, out intMinute);

            return result1 && result2;
        }

        public static bool VerifyHourFormat(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;

            var intValue = str.ToInt32();
            if (intValue == ConvertorExtension.ConvertionFailureValue) return false;

            return intValue >= 0 && intValue < 23;
        }

        public static bool VerifyMinuteFormat(string str)
        {
            if (string.IsNullOrWhiteSpace(str)) return false;

            var intValue = str.ToInt32();
            if (intValue == ConvertorExtension.ConvertionFailureValue) return false;

            return intValue >= 0 && intValue < 60;
        }

        public static bool HasSpecialChar(string str)
        {
            return Regex.IsMatch(str, @"[，。；？~！：‘“”’【】（）!@#$%^&*()=-_+{}:<>?\|';.,\[\]]");
        }
    }
}
