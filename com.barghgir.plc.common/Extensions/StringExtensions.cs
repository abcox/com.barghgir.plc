using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace com.barghgir.plc.common.Extensions
{
    public static class StringExtensions
    {
        public static string? RedactString(this string str)
        {
            var strLen = str?.Length ?? 0;
            if (strLen == 0) return str;

            var inStr = str ?? string.Empty;
            if (strLen <= 5)
                return "****";
            else if (strLen >= 12)
                return $"{inStr.Substring(0, 4)}****{inStr.Substring(strLen - 4, 4)}";
            else
            {
                var partLen = (strLen - 4) / 2;
                return $"{inStr.Substring(0, partLen)}****{inStr.Substring(strLen - 4, partLen)}";
            }
        }

        public static string? RedactConnectionStringPassword(this string str)
        {
            var strLen = str?.Length ?? 0;
            if (strLen == 0) return str;

            var inStr = str ?? string.Empty;
            return new Regex($"(Password)=([^;]*)(?:$|;)").Replace(inStr, "Password=****;");
        }
    }
}
