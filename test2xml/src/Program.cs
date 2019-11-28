
using System.Diagnostics;
using System.Runtime.Remoting.Channels;

namespace Test2Xml
{
    using System;
    using Commands;   
    using CommandLine;
    using CommandLine.Text;
    using Utilities;
    
    sealed class Program
    {
        private static readonly HeadingInfo HeadingInfo = new HeadingInfo("test2xml", "1.0");
        
        /// <summary>
        /// Application's Entry Point.
        /// </summary>
        /// <param name="args">Command line arguments splitted by the system.</param>
        private static void Main(string[] args)
        {
            Options = new Options();
            ExecutionStats stats = null;

            var parser = new Parser(with => with.HelpWriter = Console.Error);

            if (parser.ParseArgumentsStrict(args, Options, () => Environment.Exit(-2)))
            {
                try
                {
                    CommandBase cmd = null;
                    IXmlTransformer transformer = new XmlTransformer(Options);

                    if (Options.GetViews)
                    {
                        cmd = new ListViewsCommand(Options);
                    }
                    else if (Options.GenerateXml || Options.GenerateHtml)
                    {
                        stats = new ExecutionStats();
                        cmd = Options.GenerateHtml ? new HtmlCommand(Options, transformer, stats) : new XmlCommand(Options, transformer, stats);
                    }                    
                    else if (Options.GenerateToc)
                    {
                        cmd = new TocCommand(Options, transformer);    
                    }

                    if (cmd != null)
                    {
                        Stopwatch sw = null;

                        if (Options.Metrics)
                        {                            
                            sw = new Stopwatch();
                            sw.Start();
                            LogVerbose("Started timer.");
                        }

                        cmd.Execute();

                        if (sw != null)
                        {
                            sw.Stop();
                            LogVerbose("Stopped timer.");

                            var ts = sw.Elapsed;

                            Console.WriteLine("Run Time: " + String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                                ts.Hours, ts.Minutes, ts.Seconds,
                                ts.Milliseconds/10));
                        }

                        if (stats != null && Options.Output != Options.StandardOutputOptionStr)
                        {
                            Console.WriteLine(string.Format("{0} succeeded, {1} copied, {2} failed, {3} skipped", stats.Success, stats.Copied, stats.Failed, stats.Skipped));
                        }
                    }
                    else
                    {
                        Console.WriteLine(Options.GetUsage());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                    Console.WriteLine("StackTrace: " + ex.StackTrace);
                    Environment.Exit(-1);
                }
            }
        }

        public static Options Options
        {
            get; private set;
        }

        public static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        public static void LogVerbose(string message, bool newLine = true)
        {
            if (Options.Verbose)
            {
                if (newLine)
                {
                    Console.WriteLine(message);
                }
                else
                {
                    Console.Write(message);
                }
            }
        }    
    }

    public sealed class Options
    {
        [Option('i', "input", MetaValue = "FILE", HelpText = @"Specify the Robot Framework test suite file or path that contains the tests. 
            Only TXT, TSV, and ROBOT files are supported. 
            HTML files will be copied as is to the output folder when '--copyhtml' option is specified.", DefaultValue = "Input")]
        public string Input { get; set; }

        [Option('o', "output", MetaValue = "FILE", HelpText = @"Specify the output file or path (when the input is a directory)", DefaultValue = "Output")]
        public string Output { get; set; }

        [Option('t', "template", MetaValue = "FILE", HelpText = @"Specify template file or path. Only XSN and XSL 1.0 files (or directory that contains these files) are supported", DefaultValue = "Templates")]
        public string Template { get; set; }

        [Option('v', "view", MetaValue = "VIEW", HelpText = "Specify vew name to use from the templates")]
        public string View { get; set; }

        [Option("xml", HelpText = "Generate XML output")]
        public bool GenerateXml { get; set; }

        [Option("html", HelpText = "Generate HTML output")]
        public bool GenerateHtml { get; set; }

        [Option("toc", HelpText = "Generate table of contents")]
        public bool GenerateToc { get; set; }

        [Option("views", HelpText = "List available views")]
        public bool GetViews { get; set; }

        [Option("verbose", HelpText = "Turn on high-verbosity mode")]
        public bool Verbose { get; set; }

        [Option("copyhtml", HelpText = "Copy existing HTML files from the Input path to Output")]
        public bool CopyHtml { get; set; }

        [Option("metrics", HelpText = "Show performance metrics")]
        public bool Metrics { get; set; }

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

        public const string StandardOutputOptionStr = "stdout";
    }

    public sealed class ExecutionStats
    {
        public int Success { get; set; }
        public int Copied { get; set; }    
        public int Failed { get; set; }
        public int Skipped { get; set; }    
    }
}
