using System.IO;
using System.Xml.Serialization;
using Test2Xml.Utilities;

namespace Test2Xml.Commands
{
    public class XmlCommand : ParseCommandBase
    {
        private readonly IXmlTransformer _transformer;

        public XmlCommand(Options options, IXmlTransformer transformer, ExecutionStats stats) : base(options, stats)
        {
            _transformer = transformer;

            if (options.Output != null)
            {
                if (Path.GetExtension(options.Output).Length > 0)
                {
                    // file
                    _transformer.OutputDirectory = Path.GetDirectoryName(options.Output);
                }
                else
                {
                    // folder
                    _transformer.OutputDirectory = options.Output;
                }
            }
            else
            {
                _transformer.OutputDirectory = Path.GetTempPath();    
            }            
        }

        protected override TestSuite Deserialize(TextReader reader)
        {
            XmlSerializer serializer = _transformer.GetXmlSerializer(typeof(TestSuite));
            return (TestSuite)serializer.Deserialize(reader);
        }

        protected override string GetOutputFileName(string inputFileName)
        {
            return inputFileName + ".xml";
        }

        protected override void GenerateOutputFile(TextWriter writer)
        {
            _transformer.CreateXmlFile(writer, BusinessObject, Options.Template);
        }
    }
}
