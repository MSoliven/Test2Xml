using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Test2Xml.Utilities
{
    public static class StringExtensions
    {
        public static string SpaceCamelCase(this String input)
        {
            StringBuilder sb = new StringBuilder();

            bool isMarked = false;
            char? prev = null;
            char? prevNumber = null;
            foreach (char curr in input)
            {
                if ((prev == null && char.IsNumber(curr)))
                {
                    prevNumber = curr;
                    continue; // filter out preceding numbering
                }

                if (prevNumber != null && char.IsNumber(prevNumber.Value) && char.IsLetter(curr))
                {
                    continue; // e.g. 2a or 2B
                }

                prevNumber = null;
                if (!char.IsLower(curr))
                {
                    if (isMarked)
                    {
                        sb.Replace("*", char.IsLower(prev.Value) ? "" : " ");
                    }
                    else
                    {
                        sb.Append("*");
                        isMarked = true;                        
                    }
                }
                else if (isMarked)
                {
                    sb.Replace("*", " ");
                    isMarked = false;
                }

                var charToAppend = curr;
                if (prev == null || prev == '_' || prev == ' ')
                {
                    charToAppend = char.ToUpper(curr);
                }

                sb.Append(charToAppend == '_' ? ' ' : charToAppend);
                prev = curr;
            }

            return sb.ToString();
        }

        public static bool EqualsIgnoreCase(this String str1, string str2)
        {
            return str1.Equals(str2, StringComparison.OrdinalIgnoreCase);
        }

        public static bool StartsWithIgnoreCase(this String str1, string str2)
        {
            return str1.StartsWith(str2, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EndsWithIgnoreCase(this String str1, string str2)
        {
            return str1.EndsWith(str2, StringComparison.OrdinalIgnoreCase);
        }

        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }

        private const string _ellipse = "...";

        public static bool IsEllipse(this String input)
        {
            var s = input.Trim();
            return s.Equals(_ellipse);
        }

        public static bool StartsWithEllipse(this String input)
        {
            var s = input.Trim();
            return s.StartsWith(_ellipse);
        }

        public static string PreWrap(this String input)
        {
            var s = input.Trim();
            return string.Format("<pre>{0}</pre>", s);
        }

        public static bool IsAllUpper(this String input)
        {
            for (var i = 0; i < input.Length; i++)
            {
                if (Char.IsLetter(input[i]) && !Char.IsUpper(input[i]))
                    return false;
            }

            return true;
        }
    }
}
