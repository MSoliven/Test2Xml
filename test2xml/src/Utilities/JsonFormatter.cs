using System;
using System.CodeDom;
using System.Linq;
using System.Text;

namespace Test2Xml.Utilities
{
    class JsonFormatter
    {
        public static bool IsJson(string input)
        {
            var s = input.Trim();
            return s.StartsWith("{") && s.EndsWith("}")
                       || s.StartsWith("[") && s.EndsWith("]");
        }

        private const string IndentString = "    ";
        public static string Format(string str)
        {
            var idealLen = LongestStringOnTheLeft(str);
            var lastLen = 0;
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                var ch = str[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            //Enumerable.Range(0, ++indent).ForEach(item => sb.Append(IndentString));
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            //Enumerable.Range(0, --indent).ForEach(item => sb.Append(IndentString));
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        if (!quoted)
                        {
                            lastLen = 0;
                        }
                        sb.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && str[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ForEach(item => sb.Append(IndentString));
                        }
                        break;
                    case ':':
                        Enumerable.Range(0, (idealLen - lastLen)).ForEach(item => sb.Append(" "));
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        lastLen++;
                        break;
                }
            }
            return sb.ToString();
        }

        private static int LongestStringOnTheLeft(string json)
        {
            var maxlen = 0;

            var s = json.Trim().Substring(1, json.Length - 2);
            var commaSplit = s.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var split in commaSplit)
            {
                var colonSplit = split.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (colonSplit.Length > 0)
                {
                    if ((colonSplit[0].Length - 2)> maxlen)
                    {
                        maxlen = colonSplit[0].Length - 2;
                    }
                }
            }

            return maxlen;
        }
    }
}
