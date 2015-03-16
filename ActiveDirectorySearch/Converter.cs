using System.Globalization;

namespace ActiveDirectorySearch.ActiveDirectorySearch
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public static class Converter
    {
        public static string CleanOrganizationalUnit(object value)
        {
            string result;
            if (value is long)
            {
                result = ToDateTime((long)value);
            }
            else
            {
                result = GetOrganizationalUnit(value.ToString());
                if (string.IsNullOrWhiteSpace(result))
                {
                    result = value.ToString();
                }
            }

            return result;
        }

        public static string CleanCommonName(object value)
        {
            if (value is long)
            {
                return ToDateTime((long)value);
            }

            var bytes = value as byte[];
            if (bytes != null)
            {
                var arr = bytes;
                return arr.Length == 16 ? new Guid(bytes).ToString() : bytes.ToString();
            }

            var result = GetCommonName(value.ToString());
            return string.IsNullOrWhiteSpace(result) ? value.ToString() : result;
        }

        private static string GetOrganizationalUnit(string value)
        {
            return Clean(value, "OU=([^,]*)");
        }

        private static string GetCommonName(string value)
        {
            return Clean(value, "CN=([^,]*)");
        }

        private static string ToDateTime(long lngTime)
        {
            string result;
            if (lngTime == 0L || lngTime == 9223372036854775807L)
            {
                result = "Never";
            }
            else
            {
                result = DateTime.FromFileTime(lngTime).ToString(CultureInfo.InvariantCulture);
            }

            return result;
        }

        public static Image ToImage(byte[] byteArrayIn)
        {
            using (var ms = new MemoryStream(byteArrayIn))
            {
                var returnImage = Image.FromStream(ms);
                return returnImage;
            }
        }

        private static string Clean(string str, string regex)
        {
            var replacedStr = str.Replace(@"\,", "&#44;");
            const RegexOptions options = (RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline) | RegexOptions.IgnoreCase;
            var reg = new Regex(regex, options);

            return string.Join(",", (from Match mtch in reg.Matches(replacedStr) select mtch.Groups[1].Value.Replace("&#44;", ",")).ToArray());
        }
    }
}
