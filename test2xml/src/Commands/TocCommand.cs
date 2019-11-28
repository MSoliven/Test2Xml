using System;
using System.IO;
using Test2Xml.Utilities;

namespace Test2Xml.Commands
{
    public class TocCommand : IoCommandBase
    {
        private readonly IXmlTransformer _transformer;

        public TocCommand(Options options, IXmlTransformer transformer)
            : base(options, null)
        {
            _transformer = transformer;
        }

        protected override object GetBusinessObject(StreamReader notused)
        {
            return DirectoryUtilities.GetTableOfContents(Options.Output, DirectoryUtilities.OutputExtensions);
        }

        protected override string GetOutputFile(string inputFile)
        {
            if (Directory.Exists(Options.Output))
            {
                return Path.Combine(Options.Output, "index.html");
            }
            else
            {
                throw new Exception("Output must be an existing directory.");                
            }
        }

        private DocumentTemplate _documentTemplate;

        protected override void GenerateOutputFile(TextWriter writer)
        {
            var templateFile = Path.Combine(Options.Template, "__toc__.xsn");
            _documentTemplate = _transformer.CreateHtmlFile(writer, GetBusinessObject(null), templateFile, null);
        }

        protected override void OnPostProcessing()
        {
            base.OnPostProcessing();

            Console.WriteLine("Created: " + OutputFile);

            if (_documentTemplate != null && _documentTemplate.Resources != null)
            {
                DirectoryUtilities.CopyResourcesToLocal(_documentTemplate.Resources, OutputFile);
            }
        }
    }
}
