using System.Xml;

namespace Test2Xml.XSLT
{
    public class XDocument
    {
        XmlDocument objXmlDoc = null;

        public XDocument()
        { 
        }

        public XDocument(XmlDocument objXmlDoc)
        { 
            this.objXmlDoc = objXmlDoc;
        }

        public object GetDOM(string str)
        {
            // The XSL processing in an Infopath template checks for the presence
            // of a XDocument instance. If the instance is not present, it won't perform
            // certain XPATH calculations such as xdDate:Today() and xdDate:Now().
            //
            // In order to successfully implement some of these DOM methods outside
            // the InfoPath environment, we need to fool the XSLT processor into thinking 
            // that an XDocument object actually exists.
            // 
            return this;
        }

        public string GetNamedNodeProperty(object objXmlNode, string strProperty, string strDefaultValue)
        {
            // GetNamedNodeProperty() is not relevant because DocGen (not InfoPath) generates the XML.
            // Thus, we always return the default value in order to allow the XSLT transformation 
            // process to complete.
            //
            return strDefaultValue;
        }

    }
}


