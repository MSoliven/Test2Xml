using System.Collections.Generic;

namespace CabExtract
{
    using System;
    using CommandLine;
    using CommandLine.Text;

    /// <summary>
    /// This is not used in Linux (Use cabextract for Linux instead, see http://www.cabextract.org.uk/)
    /// </summary>
    sealed class Program
    {
        private static readonly HeadingInfo HeadingInfo = new HeadingInfo("cabextract", "1.0");

        /// <summary>
        /// Application's Entry Point.
        /// </summary>
        /// <param name="args">Command line arguments splitted by the system.</param>
        private static void Main(string[] args)
        {
            var options = new Options();
            var parser = new Parser(with => with.HelpWriter = Console.Error);

            if (parser.ParseArgumentsStrict(args, options, () => Environment.Exit(-2)))
            {
                try
                {
                    if (!string.IsNullOrEmpty(options.Directory) && !string.IsNullOrEmpty(options.FilesToExtract[0]))
                    {
                        var cabExtractor = new CabExtractor(options.FilesToExtract[0]);
                        cabExtractor.ExtractTo(options.Directory);
                    }
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine("Exception encountered: " + ex.Message);
                    Environment.Exit(-1);
                }
            }
        }
    }

    public sealed class Options
    {
        [Option('d', "directory", MetaValue = "FILE", Required = true, HelpText = "extract all files to the given directory")]
        public string Directory { get; set; }

        [ValueList(typeof(List<string>))]
        public IList<string> FilesToExtract { get; set; }

        // Marking a property of type IParserState with ParserStateAttribute allows you to
        // receive an instance of ParserState (that contains a IList<ParsingError>).
        // This is equivalent from inheriting from CommandLineOptionsBase (of previous versions)
        // with the advantage to not propagating a type of the library.
        //
        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
