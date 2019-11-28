using System;
using System.IO;
using System.Text;
using Test2Xml.Utilities;

namespace Test2Xml.Commands
{
    public abstract class IoCommandBase : CommandBase
    {
        protected object BusinessObject { get; private set; }
        protected string InputFile { get; private set; }
        protected string OutputFile { get; private set; }

        protected string OutputDirectory
        {
            get { return Path.GetDirectoryName(OutputFile); }
        }

        private readonly ExecutionStats _stats;

        protected IoCommandBase(Options options, ExecutionStats stats)
            : base(options)
        {
            _stats = stats;
        }

        protected virtual void OnPreProcessing()
        {
            BusinessObject = null;
        }

        protected virtual void OnPostProcessing()
        {
               
        }

        protected virtual void OnPostExecution()
        {
            
        }

        protected abstract object GetBusinessObject(StreamReader reader);
        protected abstract string GetOutputFile(string inputFile);

        protected virtual void Process(string inputFile)
        {
            InputFile = inputFile;

            StreamReader reader = null;
            StreamWriter writer = StreamWriter.Null;

            try
            {
                OnPreProcessing();
                
                if (InputFile != null)
                {
                    reader = new StreamReader(inputFile, Encoding.UTF8);
                    BusinessObject = GetBusinessObject(reader);
                }

                if (Options.Output.EqualsIgnoreCase(Options.StandardOutputOptionStr))
                {
                    writer = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };
                }
                else
                {
                    OutputFile = GetOutputFile(inputFile);
                }

                if (OutputFile != null)
                {
                    writer = new StreamWriter(OutputFile, false, Encoding.UTF8);
                }

                GenerateOutputFile(writer);

                OnPostProcessing();
            }
            catch (Exception ex)
            {
                LogVerbose("Exception: " + ex.Message);
                LogVerbose("StackTrace: " + ex.StackTrace);

                if (_stats != null)
                {
                    _stats.Failed++;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (writer != null)
                {
                    writer.Close();
                }
            }

        }

        public override void Execute()
        {
            Process(null);
            OnPostExecution();
        }

        protected abstract void GenerateOutputFile(TextWriter writer);
    }
}
