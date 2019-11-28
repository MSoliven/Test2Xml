using System.IO;
using System.Text;
using Test2Xml.Utilities;

namespace Test2Xml.Commands
{
    public class HtmlCommand : XmlCommand
    {
        private readonly IXmlTransformer _transformer;

        public HtmlCommand(Options options, IXmlTransformer transformer, ExecutionStats stats)
            : base(options, transformer, stats)
        {
            _transformer = transformer;
        }

        protected override string GetOutputFileName(string inputFileName)
        {
            return inputFileName + ".html";
        }

        private DocumentTemplate _documentTemplate;

        protected override void GenerateOutputFile(TextWriter writer)
        {
            if (BusinessObject != null)
            {
                if (Options.GenerateXml)
                {
                    if (OutputFile != null)
                    {
                        var outputFile = OutputFile.Replace(".html", ".xml");

                        using (var xmlWriter = new StreamWriter(outputFile, false, Encoding.UTF8))
                        {
                            base.GenerateOutputFile(xmlWriter);
                        }
                    }
                    else
                    {
                        // stdout
                        base.GenerateOutputFile(writer);
                    }
                }

                // Explicitly set view takes precedence over metadata
                var viewToUse = Options.View ?? SelectedView;

                _documentTemplate = _transformer.CreateHtmlFile(writer, BusinessObject, Options.Template, viewToUse);
            }
            else if (InputFile != null)
            {
                if (writer != null)
                {
                    writer.Close();
                }

                // Just copy the HTML file to the destination
                if (File.Exists(OutputFile))
                {
                    File.Delete(OutputFile);
                }
                File.Copy(InputFile, OutputFile);
            }
        }

        protected override void OnPostProcessing()
        {
            base.OnPostProcessing();

            if (_documentTemplate != null && _documentTemplate.Resources != null)
            {
                DirectoryUtilities.CopyResourcesToLocal(_documentTemplate.Resources, OutputFile);
            }
        }

        protected override void OnPostExecution()
        {
            base.OnPostExecution();

            if (Options.GenerateToc)
            {
                var tocCommand = new TocCommand(Options, _transformer);
                tocCommand.Execute();
            }
        }
    }
}
