using System.Collections;
using System.Xml;
using System.Xml.Xsl;

namespace Test2Xml.Utilities
{
    interface IDocumentTemplate
    {
        string DefaultViewName { get; }
        XslCompiledTransform DefaultXslTransform { get; }
        XmlNodeList XmlCalcInfo { get; }
        string XmlHeader { get; }
        XmlNodeList XmlViewInfo { get; }
        Hashtable XslTransforms { get; }
    }
}
