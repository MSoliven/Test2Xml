using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Test2Xml.Utilities
{
    public interface IXmlTransformer
    {
        DocumentTemplate CreateHtmlFile(TextWriter writer, object requestData, string templateFile, string viewName);
        void CreateXmlFile(TextWriter writer, object requestData, string templateFile);
        XmlDocument GetXmlDocument(object requestData);
        XmlSerializer GetXmlSerializer(Type type);
        string OutputDirectory { get; set; }
    }
}
