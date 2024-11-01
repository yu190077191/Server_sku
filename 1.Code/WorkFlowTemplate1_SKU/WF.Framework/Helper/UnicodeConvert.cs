using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace WF.Framework
{
    public class UnicodeConvert
    {
        public string StringToUnicode(string sourceText)
        {
            string dst = string.Empty;
            char[] src = sourceText.ToCharArray();
            for (int i = 0; i < src.Length; i++)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(src[i].ToString());
                string str = @"\u" + bytes[1].ToString("X2", CultureInfo.CurrentCulture) + bytes[0].ToString("X2", CultureInfo.CurrentCulture);
                dst += str;
            }

            return dst;
        }

        public string UnicodeToString(string sourceText)
        {
            string s = sourceText;
            string str;
            string numstr;
            string convertedstr;
            string pattern = @"\\u([0-9a-z]{4})";
            MatchCollection matches = Regex.Matches(s, pattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
            if (matches.Count > 0)
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    str = matches[i].ToString();
                    numstr = str.Replace("\\u", string.Empty);
                    numstr = numstr.Replace("\\", string.Empty);
                    byte[] bytes = new byte[2];
                    bytes[1] = byte.Parse(int.Parse(numstr.Substring(0, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture).ToString(), CultureInfo.CurrentCulture);
                    bytes[0] = byte.Parse(int.Parse(numstr.Substring(2, 2), NumberStyles.HexNumber, CultureInfo.CurrentCulture).ToString(), CultureInfo.CurrentCulture);
                    convertedstr = Encoding.Unicode.GetString(bytes);
                    s = s.Replace(str, convertedstr);
                }
            }

            return s;
        }
    }
}