using System;
using System.Linq;
using System.Text;

namespace Test2Xml.Utilities
{
    class SemicolonFormatter
    {
        public static bool IsSemicolonDelimitedExp(string input)
        {
            var s = input.Trim();
            var splits = s.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            return splits.Length > 1 && splits.All(exp => exp.IndexOf("=", StringComparison.Ordinal) > 0);
        }

        public static string Format(string str)
        {
            var s = str.Trim();
            var expressions = s.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
            var expCount = expressions.Length;

            if (expressions.Length == 0)
            {
                return str;
            }

            var array = new string[expCount, 2];
            var tokens = new string[2];
            var maxlen = 0;

            for (var i = 0; i < expCount; i++)
            {
                tokens = expressions[i].Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length > 0)
                {
                    array[i, 0] = tokens[0].Trim();

                    if (tokens.Length == 2)
                    {
                        array[i, 1] = tokens[1].Trim();
                    }
                    else
                    {
                        array[i, 1] = string.Empty;
                    }

                    if (tokens[0].Length > maxlen)
                    {
                        maxlen = tokens[0].Length;
                    }
                }
                else
                {
                    return str;
                }
            }

            // Apply padding
            //
            for (var i = 0; i < expCount; i++)
            {
                var sb = new StringBuilder();
                sb.Append(array[i, 0]);//.PadRight(maxlen));
                sb.Append(" = ");
                sb.Append(array[i, 1]);

                expressions[i] = sb.ToString();
            }

            return string.Join(Environment.NewLine, expressions);
        }
    }
}
