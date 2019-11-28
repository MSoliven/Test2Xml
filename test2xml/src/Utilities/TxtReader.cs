using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Test2Xml.Utilities
{
    public class TxtReader : TextReaderBase
    {
        private readonly Regex _spaceSplitter;
        private readonly Regex _pipeSplitter;
        private readonly string[] _pipeStarts;

        public TxtReader(TextReader reader) : base (reader)
        {
            _spaceSplitter = new Regex(" {2,}");
            _pipeSplitter = new Regex(" \\|(?= )");
            _pipeStarts = new string[] { "|", "| "};
        }

        public override string[] Split(string line, StringSplitOptions options)
        {
            string[] tokens;

            if (line.IndexOfAny(new char[] {'\t'}) > -1)
            {
                line = line.Replace("\t", "  ");
            }
            if (_pipeStarts.Any(p => p.Equals(line.Substring(0, 2))))
            {                
                if (line.EndsWith(" |"))
                {
                    line = line.Substring(1, line.Length - 2);                    
                }
                else
                {
                    line = line.Substring(1, line.Length - 1);
                }

                tokens = _pipeSplitter.Split(line);            
            }
            else
            {
                tokens = _spaceSplitter.Split(line);
            }            

            return ProcessTokens(tokens, options);
        }
    }
}
