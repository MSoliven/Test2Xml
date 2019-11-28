using System;
using System.IO;

namespace Test2Xml.Utilities
{
    public class TsvReader : TextReaderBase
    {
        public TsvReader(TextReader reader) : base(reader)
        {
        }

        public override string[] Split(string line, StringSplitOptions options)
        {
            var tokens = line.Split(new char[] { '\t' }, options);
            return ProcessTokens(tokens);
        }
    }
}
