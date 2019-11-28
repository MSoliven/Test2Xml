using System;
using System.Text;

namespace Test2Xml.Utilities
{
    class LongWordFormatter
    {
        public static bool IsLongWord(string input)
        {
            if (input.Length > 20)
            {
                var splits = input.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                return splits.Length == 1;
            }

            return false;
        }

        public static string Format(string str)
        {
            var maxLen = str.IsAllUpper() ? 20 : 25;
            StringBuilder sb = new StringBuilder();
            for(;;)
            {
                if (str.Length > maxLen)
                {
                    sb.Append(str.Substring(0, maxLen));
                    str = str.Substring(maxLen);
                    sb.Append(' ');
                }
                else
                {
                    sb.Append(str.Substring(0, str.Length));
                    break;
                }
            }

            return sb.ToString();
        }
    }
}
