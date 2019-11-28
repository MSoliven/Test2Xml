using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace Test2Xml.Utilities
{
    public class XmlTransformer : IXmlTransformer 
    {
        private readonly Hashtable _htTemplates;
        private readonly Hashtable _xmlSerializer;

        private static string _xmlNamespace = "http://www.iontrading.com/RFML/v1.0";

        private readonly Options _options;

        private string _outputDirectory;
        public string OutputDirectory
        {
            get { return _outputDirectory; }
            set { _outputDirectory = value; }
        }

        public XmlTransformer(Options options)
        {
            _options = options;
            _htTemplates = new Hashtable();
            _xmlSerializer = new Hashtable();
            _outputDirectory = Path.GetTempPath();
        }

        public DocumentTemplate CreateHtmlFile(TextWriter writer, object requestData, string template, string viewName)
        {
            XslCompiledTransform selectedTransform = null;
            DocumentTemplate docTemplate = null;

            foreach (var templateFile in DirectoryUtilities.GetFiles(template, DirectoryUtilities.TemplateExtensions))
            {
                // Skip unspecified views
                if (!string.IsNullOrEmpty(Path.GetExtension(viewName)))
                {
                    // .xsl views
                    if (templateFile.EndsWithIgnoreCase(".xsn"))
                    {
                        continue;
                    }
                    
                    if (!Path.GetFileName(templateFile).EqualsIgnoreCase(viewName))
                    {
                        continue;
                    }
                }
                else
                {
                    // Infopath views
                    if (!templateFile.EndsWithIgnoreCase(".xsn"))
                    {
                        continue;
                    }                    
                }

                if (TryGetXslTransform(templateFile, viewName, out selectedTransform, out docTemplate))
                {
                    break;
                }
            }

            if (selectedTransform == null)
            {
                throw new Exception(
                    "Unable to load template. Please ensure that the file is not open in another application.");
            }

            var xmlDoc = GetXmlDocument(requestData);

            LogVerbose("Generating HTML document ...");

            selectedTransform.Transform(xmlDoc, docTemplate.XsltArgumentList, writer);

            return docTemplate;
        }

        public void CreateXmlFile(TextWriter writer, object requestData, string template)
        {
            // Create the XML document object with template info in header
            var xmlDoc = GetXmlDocument(requestData);

            LogVerbose("Formatting generated XML ...");

            try
            {
                string xml = null;
                using (var stream = new MemoryStream())
                {
                    using (
                        var xmlWriter = XmlWriter.Create(stream,
                            new XmlWriterSettings {Indent = true, IndentChars = "    ", OmitXmlDeclaration = true}))
                    {
                        xmlDoc.WriteTo(xmlWriter);
                    }
                    stream.Position = 0;
                    using (var reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        xml = reader.ReadToEnd();
                    }
                }

                writer.Write(xml);
            }
            catch (Exception ex)
            {
                throw (new Exception("Failed to format generated XML.", ex));
            }
        }

        public XmlDocument GetXmlDocument(object requestData)
        {      
            XmlDocument xmlDoc = null;      
            XmlSerializer serializer = GetXmlSerializer(requestData);

            LogVerbose("Serializing the object to XML ...");

            // Serialize the object into Xml, results are placed into the memory stream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, requestData);
                memoryStream.Position = 0;

                LogVerbose("Generating XML document ...");

                // Read the memory stream into an XmlDocument object 
                using (XmlTextReader xmlReader = new XmlTextReader(memoryStream))
                {
                    // Create a new object that will hold the modified Xml data
                    xmlDoc = new XmlDocument();
                    xmlDoc.Load(xmlReader);
                }
            }

            return xmlDoc;
        }

        public XmlSerializer GetXmlSerializer(Type objType)
        {
            LogVerbose("Getting XML Serializer ...");

            if (_xmlSerializer[objType.ToString()] == null)
            {
                XmlSerializer objSerializer = new XmlSerializer(objType, _xmlNamespace);
                if (_xmlSerializer.ContainsKey(objType.ToString()))
                {
                    _xmlSerializer[objType.ToString()] = objSerializer;
                }
                else
                {
                    _xmlSerializer.Add(objType.ToString(), objSerializer);
                }
            }

            // Get a reference of the preloaded object from the array.
            return (XmlSerializer)_xmlSerializer[objType.ToString()];
        }

        private XmlSerializer GetXmlSerializer(object objToSerialize)
        {
            var objType = objToSerialize.GetType();
            return GetXmlSerializer(objType);
        }

        private DocumentTemplate GetDocumentTemplate(string templateFile)
        {
            if (_htTemplates[templateFile] == null)
            {
                if (templateFile != null)
                {
                    var templateExt  = Path.GetExtension(templateFile);

                    if (templateExt.EqualsIgnoreCase(".xsn"))
                    {
                        // Set the location of the InfoPath solution file and the output directory where the
                        // resource files in the archive will be extracted to
                        string cabinetFile = templateFile;

                        // Extract the template and associated files from the Infopath CAB file.
                        // This will also load the Xsl into XslHashtable
                        var template = InfoPathUtilities.LoadXslFromInfoPath(cabinetFile, _outputDirectory);
                        if (template != null)
                        {
                            _htTemplates.Add(templateFile, template);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else 
                    {
                        var xslCompiledTransform = new XslCompiledTransform();
                        xslCompiledTransform.Load(templateFile);

                        var template = new DocumentTemplate(xslCompiledTransform, Path.GetFileName(templateFile), null);
                        _htTemplates.Add(templateFile, template);
                    }
                }
                else
                {
                    // templateFile is null, throw an exception
                    throw (new Exception("No template file specified."));
                }
            }

            return (DocumentTemplate) _htTemplates[templateFile];

        }

        private bool TryGetXslTransform(string templateFile, string docView, out XslCompiledTransform xslTransform, out DocumentTemplate template)
        {
            template = GetDocumentTemplate(templateFile);
            if (template != null)
            {
                if (docView != null && template.XslTransforms.ContainsKey(docView))
                {
                    // Use selected view
                    xslTransform = (XslCompiledTransform) template.XslTransforms[docView];
                    return true;
                }

                xslTransform = template.DefaultXslTransform;
            }
            else
            {
                xslTransform = null;
            }
            
            return false;
        }

        private void LogVerbose(string message, bool newLine = true)
        {
            Program.LogVerbose(message, newLine);
        }

    }

}