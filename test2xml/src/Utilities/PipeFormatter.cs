using System;
using System.Text;

namespace Test2Xml.Utilities
{
    class PipeFormatter
    {
        public static bool IsPipeDelimited(string input)
        {
            var s = input.Trim();
            return s.StartsWith("|") && s.EndsWith("|");
        }

        public static string Format(string str)
        {
            var s = str.Trim();
            var rows = s.Split(new string[] {"| |"}, StringSplitOptions.RemoveEmptyEntries);
            var rowCount = rows.Length;
            
            if (rows.Length == 0)
            {
                return str;
            }

            var cols = rows[0].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            var colCount = cols.Length;

            var array = new string[rowCount, colCount];

            for (var y = 0; y < rowCount; y++)
            {
                if (y > 0)
                {
                    cols = rows[y].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                }
                for (var x = 0; x < colCount; x++)
                {
                    array[y, x] = cols[x];
                }
            }

            // Get length of widest cell per column
            //
            var maxlen = new int[colCount];

            for (var x = 0; x < colCount; x++)
            {
                for (var y = 0; y < rowCount; y++)
                {
                    if (array[y, x].Length > maxlen[x])
                    {
                        maxlen[x] = array[y, x].Length;
                    }
                }
            }

            // Apply padding
            //
            for (var y = 0; y < rowCount; y++)
            {
                var sb = new StringBuilder().Append("|");

                for (var x = 0; x < colCount; x++)
                {
                    sb.Append(array[y, x].PadRight(maxlen[x]));
                    sb.Append("|");
                }

                rows[y] = sb.ToString();
            }

            return string.Join(Environment.NewLine, rows);
        }
    }
}
