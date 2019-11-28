using System;
using System.Collections.Generic;
using System.IO;

namespace Test2Xml.Utilities
{
    public abstract class TextReaderBase
    {
        private const char Nbsp = '\xA0';

        private readonly TextReader _reader;

        protected TextReaderBase(TextReader reader)
        {
            _reader = reader;
        }

        public int Peek()
        {
            return _reader.Peek();
        }

        public string ReadLine()
        {
            return ProcessLine(_reader.ReadLine());
        }

        public virtual string[] Split(string line)
        { 
            return Split(line, StringSplitOptions.RemoveEmptyEntries);
        }

        public abstract string[] Split(string line, StringSplitOptions options);

        protected string ProcessLine(string line)
        {
            if (line.IndexOfAny(new char[] { Nbsp }) > -1)
            {
                line.Replace(Nbsp, ' ');
            }

            //return line.Trim();

            return line;
        }

        protected string[] ProcessTokens(string[] tokens)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                tokens[i] = ProcessToken(tokens[i]);
            }

            return tokens;
        }

        protected string[] ProcessTokens(string[] tokens, StringSplitOptions options)
        {
            var tokenList = new List<string>();

            for(int i=0; i < tokens.Length; i++)
            {
                var token = ProcessToken(tokens[i]);
                if (options.Equals(StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        tokenList.Add(token);
                    }
                }
                else
                {
                    tokenList.Add(token);    
                }                
            }

            return tokenList.ToArray();
        }

        protected string ProcessToken(string token)
        {
            if (token.Length > 1 && (token[0] == '"' && token[0] == token[token.Length - 1]))
            {
                token = token.Substring(1).Replace("\"\"", "\"");
            }

            return token;
        }
    }
}
