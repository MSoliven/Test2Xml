using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Xsl;

namespace Test2Xml.Utilities
{
    public class DocumentTemplate : IDocumentTemplate
    {
        public XslCompiledTransform DefaultXslTransform
        {
            get
            {
                if (_xslTransforms != null)
                {
                    return (XslCompiledTransform) _xslTransforms[_defaultView];
                }
                else
                {
                    return _xslTransform;
                }
            }
        }

        public Hashtable XslTransforms
        {
            get
            {
                return _xslTransforms;
            }
        }

        public string DefaultViewName
        {
            get
            {
                return _defaultView;
            }
        }

        public string XmlHeader
        {
            get
            {
                return _xmlHeader;
            }
        }

        public XmlNodeList XmlViewInfo
        {
            get
            {
                return _viewInfo;
            }
        }

        public XmlNodeList XmlCalcInfo
        {
            get
            {
                return _calcInfo;
            }
        }


        public IEnumerable<string> Resources
        {
            get
            {
                return _resources;
            }
        }

        public XsltArgumentList XsltArgumentList
        {
            get
            {
                return _xsltArgumentList;
            }            
        }

        private readonly Hashtable _xslTransforms;
        private readonly XslCompiledTransform _xslTransform;
        private readonly string _defaultView;
        private readonly string _xmlHeader;
        private readonly XmlNodeList _viewInfo;
        private readonly XmlNodeList _calcInfo;
        private readonly IEnumerable<string> _resources;
        private readonly XsltArgumentList _xsltArgumentList;

        public DocumentTemplate(Hashtable xslTransforms, string defaultView, IEnumerable<string> resources, string xmlHeader = null, XmlNodeList viewInfo = null, XmlNodeList calcInfo = null, XsltArgumentList xslArgs = null)
        {
            _xslTransforms = xslTransforms;
            _defaultView = defaultView;
            _resources = resources;
            _xmlHeader = xmlHeader;
            _viewInfo = viewInfo;
            _calcInfo = calcInfo;
            _xsltArgumentList = xslArgs;
        }

        public DocumentTemplate(XslCompiledTransform xslTransform, string defaultView, string[] resources)
        {
            _xslTransforms = new Hashtable();
            _xslTransforms.Add(defaultView, xslTransform);

            _xslTransform = xslTransform;
            _defaultView = defaultView;
            _resources = resources;
            _xmlHeader = null;
            _viewInfo = null;
            _calcInfo = null;
        }
    }
}
