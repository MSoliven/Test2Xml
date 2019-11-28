using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using XDocument = System.Xml.Linq.XDocument;

namespace Test2Xml.Utilities
{
	public class InfoPathUtilities
	{
        public static DocumentTemplate LoadXslFromInfoPath(string cabinetFilePath, string outputDirectory)
        {    
            return LoadXslFromInfoPath(cabinetFilePath, outputDirectory, null);
        }
        
        public static DocumentTemplate LoadXslFromInfoPath(string cabinetFilePath, string outputDirectory, string validNamespace)
        {
            Hashtable xslTransform = new Hashtable();

            if (!File.Exists(cabinetFilePath))
            {
                throw( new Exception("Template '" + cabinetFilePath + "' does not exist."));
            }

            string defaultViewFile = null;
            string defaultView = null;
            string infoPathXmlHeader  = null;

            XslCompiledTransform defaultXsl = null;
            XmlNodeList viewInfo = null;
            XmlNodeList calcInfo = null;

            var resources = new List<string>();

            var tempPath = Path.GetTempPath();

            LogVerbose("Extracting files from " + cabinetFilePath + " to " + tempPath + "...");

            // Extract the solution's manifest file into the system's temporary directory, 
            CabExtractor cab = new CabExtractor(cabinetFilePath);
            ArrayList extractedFiles = cab.ExtractTo(tempPath);

            if (extractedFiles == null)
            {
                LogVerbose("No files extracted from " + cabinetFilePath + ".");
                return null;
            }

            // While the manifest file appears to be always extracted and read first from the archive, 
            // it is safer not to depend on this assumption as Microsoft may change this in the future.
            // Instead, we loop through the list of extracted files until a default view is
            // found. This is necessary because the .xsl file will be skipped when the default view
            // taken from the manifest has not been read yet
            while(defaultViewFile == null)
            {
                // Walk through each file extracted from the cab file
                for (int j=0; j < extractedFiles.Count; j++)
                {
                    // Get the full filepath of the extracted file
                    string extractedFile = extractedFiles[j].ToString();

                    switch(Path.GetExtension(extractedFile).ToLower())
                    {
                        case ".xsf":
                            LogVerbose("Getting manifest info " + extractedFile);

                            GetManifestInfo(extractedFile, out viewInfo, out calcInfo, out defaultViewFile);

                            if (defaultViewFile == null)
                            {
                                throw (new Exception("Failed reading default view from '" + cabinetFilePath + "'."));
                            }
                            // We don't need it after the check                                  
                            if (File.Exists(extractedFile))
                            {
                                LogVerbose("Deleting " + extractedFile + "...");

                                File.Delete(extractedFile);
                            }
                            break;            
                        case ".xsd":
                            LogVerbose("Checking schema " + extractedFile);

                            // Check if the solution has a valid (correctly versioned) schema
                            if (validNamespace != null && !IsValidTargetNamespace(extractedFile, validNamespace))
                            {
                                throw (new Exception("Template '" + cabinetFilePath + "' does not match schema."));
                            }
                            // We don't need it after the check     
                            if (File.Exists(extractedFile))
                            {
                                LogVerbose("Deleting " + extractedFile + "...");

                                File.Delete(extractedFile);
                            }
                            break;
                        case ".xsl":
                            if (viewInfo != null)
                            {
                                LogVerbose("Loading view " + extractedFile);

                                var xslCompiledTransform = new XslCompiledTransform();                                        
                                xslCompiledTransform.Load(AddCustomLogicToViewFile(extractedFile));

                                // Find the corresponding view name of the extracted file           
                                string strViewName = GetViewName(viewInfo, Path.GetFileName(extractedFile));

                                // Check if it's the default view
                                if (defaultViewFile.Equals(Path.GetFileName(extractedFile)))
                                {
                                    defaultXsl  = xslCompiledTransform;                                  
                                    defaultView = strViewName;
                                }

                                if (strViewName != null && strViewName.Trim() != "")
                                {
                                    // Add to the view hashtable collection using the view name as the key
                                    xslTransform.Add(strViewName, xslCompiledTransform);
                                }

                                // We don't need it after loading                                 
                                if (File.Exists(extractedFile))
                                {
                                    LogVerbose("Deleting " + extractedFile + "...");

                                    File.Delete(extractedFile);
                                }
                            }
                            break;
                        case ".xml":
                            LogVerbose("Reading header information from " + extractedFile + "...");

                            // Read the header of the InfoPath xml document
                            if (infoPathXmlHeader == null && Path.GetFileName(extractedFile).Equals("template.xml"))
                            {
                                infoPathXmlHeader = GetXmlHeaderFromInfoPath(extractedFile);
                            }
                            // Delete the file
                            if (File.Exists(extractedFile))
                            {
                                LogVerbose("Deleting " + extractedFile);

                                File.Delete(extractedFile);
                            }
                            break;
                        default:
                            // 
                            // Copy any infopath resources to the output directory, we will
                            // keep the one extracted in tempdir as temporary html files 
                            // still needs it.
                            //
                            if (!string.IsNullOrWhiteSpace(outputDirectory))
                            {
                                if (!Directory.Exists(outputDirectory))
                                {
                                    LogVerbose("Creating directory " + outputDirectory);

                                    // Make sure the target directory exist!
                                    Directory.CreateDirectory(outputDirectory);
                                }
                            }

                            // Target filepath name
                            string newFilePath = Path.Combine(outputDirectory, Path.GetFileName(extractedFile));
                            if (!File.Exists(newFilePath))
                            {
                                LogVerbose("Copying " + extractedFile + " to " + newFilePath);

                                // Now make a copy of the resource
                                File.Copy(extractedFile, newFilePath);
                            }
                            resources.Add(newFilePath);
                            break;
                    }
                }   
            }   // while (strDefaultView == null)

            var xslArgs = new XsltArgumentList();

            xslArgs.AddExtensionObject("http://schemas.microsoft.com/office/infopath/2003/xslt/formatting", new XSLT.Formatting());
            xslArgs.AddExtensionObject("http://schemas.microsoft.com/office/infopath/2003/xslt/xDocument", new XSLT.XDocument());
            xslArgs.AddExtensionObject("http://schemas.microsoft.com/office/infopath/2003/xslt/Date", new XSLT.Date());
            xslArgs.AddExtensionObject("http://schemas.microsoft.com/office/infopath/2003/xslt/Math", new XSLT.Math());

            // Create a document template and add it to the hashtable
            return new DocumentTemplate(xslTransform, defaultView, resources, infoPathXmlHeader, viewInfo, calcInfo, xslArgs);            
        }

        public static string GetViewName(XmlNodeList viewInfo, string fileName)
        {            
            for (var i=0; i < viewInfo.Count; i++)
            {          
                for (var j=0; j < viewInfo[i].ChildNodes.Count; j++)
                {
                    if (viewInfo[i].ChildNodes[j].Name.Equals("xsf:mainpane"))
                    {
                        // Found, now read the value of the 'transform' attribute
                        var xmlAttributeCollection = viewInfo[i].ChildNodes[j].Attributes;
                        if (xmlAttributeCollection != null)
                        {
                            var currentViewFile = xmlAttributeCollection["transform"].Value;
                            if (currentViewFile.EqualsIgnoreCase(fileName))
                            {
                                var attributeCollection = viewInfo[i].Attributes;
                                if (attributeCollection != null) return attributeCollection["name"].Value;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public static void GetManifestInfo(string manifestFile, out XmlNodeList viewInfo, out XmlNodeList calcInfo, out string defaultViewFile)
        {   
            defaultViewFile = null;
            FileStream file = null;
            viewInfo = null;
            calcInfo = null;
            
            try
            {
                file = new FileStream(manifestFile, FileMode.Open, FileAccess.Read);
                                            
                // Create an XmlDocument by reading the manifest file
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(file);

                // Walk through each element in the Xml document's hierarchy and look for the default
                // .xsl transform file, such as 'View 1' below:
                //
                // <xsf:xDocumentClass>
                //   ...
                //
                //    <xsf:views default="View 1">
                //         <xsf:view name="View 1" caption="View 1">
                //             <xsf:mainpane transform="view1.xsl"></xsf:mainpane>
                //         </xsf:view>
                //         <xsf:view name="View 2" caption="View 2">
                //             <xsf:mainpane transform="view2.xsl"></xsf:mainpane>
                //             ...
                //         </xsf:view>
                //         ...
                //
                //    </xsf:views>      
                //

                // Get the "views" parent element
                viewInfo = xmlDoc.GetElementsByTagName("xsf:views");

                XmlNode viewsNode = viewInfo[0];
                if (viewsNode.Attributes != null)
                {
                    var defView = viewsNode.Attributes["default"].Value;

                    viewInfo = xmlDoc.GetElementsByTagName("xsf:view");

                    // Walk through each views and find the default
                    for (int i=0; i < viewInfo.Count; i++)
                    {
                        var xmlAttributeCollection = viewInfo[i].Attributes;
                        if (xmlAttributeCollection != null && defView.Equals(xmlAttributeCollection["name"].Value))
                        {
                            for (int j=0; j < viewInfo[i].ChildNodes.Count; j++)
                            {
                                if (viewInfo[i].ChildNodes[j].Name.Equals("xsf:mainpane"))
                                {
                                    // Found, now read the value of the 'transform' attribute
                                    var attributeCollection = viewInfo[i].ChildNodes[j].Attributes;
                                    if (attributeCollection != null)
                                        defaultViewFile = attributeCollection["transform"].Value;
                                }
                            }
                        }
                    }
                }

                //
                // Get calculatedField node list
                // 
                calcInfo = xmlDoc.GetElementsByTagName("xsf:calculatedField");
            }

            finally
            {   
                if (file != null)
                {
                    file.Close();
                }
            }
        }

        public static string GetViewHeader(DocumentTemplate objXmlDocTemplate, string strView)
        {
            return GetViewPrintSetting(objXmlDocTemplate, strView, "xsf:header");
        }

        public static string GetViewFooter(DocumentTemplate objXmlDocTemplate, string strView)
        {
            return GetViewPrintSetting(objXmlDocTemplate, strView, "xsf:footer");
        }

        public static string GetViewPrintSetting(DocumentTemplate xmlDocTemplate, string view, string attribute)
        {
            XmlDocument doc;
            string attributeValue = "";

            try
            {
                doc = xmlDocTemplate.XmlViewInfo[0].OwnerDocument;

                // loads all print settings for views.
                XmlNodeList printSettings = doc.GetElementsByTagName("xsf:printSettings");

                // Print settings can exist for each view.
                foreach (XmlNode xmlNode in printSettings)
                {
                    if (xmlNode.ParentNode != null && xmlNode.ParentNode.Attributes != null && xmlNode.ParentNode.Attributes["name"].Value == view)
                    {
                        foreach (XmlNode node in xmlNode.ChildNodes)
                        {
                            if (node.Name == attribute)
                            {
                                attributeValue = node.InnerText;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                //EventLogger.WriteEventLog(e, "Unable to load print setting.", System.Diagnostics.EventLogEntryType.Error);
            }

            return attributeValue;
        }

        public static string GetXmlHeaderFromInfoPath(string inputFile)
        {      
            XmlTextReader textReader = null;
            StringBuilder header = new StringBuilder();
            bool exit = false;
   
            try
            {
                // Use a text reader that ignores whitespaces.
                textReader = new XmlTextReader(inputFile);
                textReader.WhitespaceHandling = WhitespaceHandling.None;

                // Parse the file and capture the header nodes
                while (textReader.Read() && exit == false)
                {
                    switch (textReader.NodeType)
                    {
                        case XmlNodeType.ProcessingInstruction:
                        case XmlNodeType.XmlDeclaration:
                            header.Append( string.Format("<?{0} {1}?>", textReader.Name, textReader.Value));
                            break;
                        default:
                            exit = true;
                            break;
                    }       
                }
            }

            finally
            {
                if (textReader != null)
                {
                    textReader.Close();
                }
            }

            // Return the header string
            return header.ToString();
        }

        public static bool IsValidTargetNamespace(string inputFile, string validNamespace)
        {
            FileStream file = null;
            
            try
            {                       
                file = new FileStream(inputFile, FileMode.Open, FileAccess.Read);
         
                // Create an XmlDocument by reading the schema file
                XmlDocument objXmlDoc = new XmlDocument();
                objXmlDoc.Load(file);

                // Walk through each element in the Xml document's hierarchy and look for the specified attribute
                XmlNodeList nodeList = objXmlDoc.GetElementsByTagName("xs:schema");
                XmlNode node = nodeList[0];

                // Check for a valid target namespace 
                if (node.Attributes != null && node.Attributes["targetNamespace"].Value.Equals(validNamespace))
                {
                    return true;
                }
            }

            finally
            {
                if (file != null)
                {
                    file.Close();
                }
            }

            return false;
        }

        public static bool IsViewInLandscapeMode(DocumentTemplate objXmlDocTemplate, string strView)
        {
            var isLandscape = false;

            var doc = objXmlDocTemplate.XmlViewInfo[0].OwnerDocument;
            if (doc != null)
            {
                var objViewInfo = doc.GetElementsByTagName("xsf:view");
                foreach (XmlNode node in objViewInfo)
                {
                    if (node.Attributes != null && strView.Equals(node.Attributes["name"].Value))
                    {
                        for (int j = 0; j < node.ChildNodes.Count; j++)
                        {
                            if (node.ChildNodes[j].Name.Equals("xsf:printSettings"))
                            {
                                var xmlAttributeCollection = node.ChildNodes[j].Attributes;
                                if (xmlAttributeCollection != null)
                                {
                                    var orientation = xmlAttributeCollection["orientation"];
                                    isLandscape = orientation != null && orientation.Value == "landscape" ? true : false;
                                }
                                break;
                            }
                        }
                        break;
                    }

                }
            }

            return isLandscape;
        }

        public static string AddCustomLogicToViewFile(string inputFile)
        {
            FileStream inputStream  = null;
            FileStream outputStream  = null;
            StreamReader reader   = null;
            StreamWriter writer   = null;

            string outputFile = Path.GetTempPath() + Path.GetFileNameWithoutExtension(inputFile) + ".bak";
                
            try
            {
                var outputString = new StringBuilder();

                //
                // Open the view file for reading and writing
                //
                inputStream = new FileStream(inputFile,  FileMode.Open, FileAccess.Read);                                            
                reader    = new StreamReader(inputStream);               

                // Read the XML content into a string buffer
                string strXml = reader.ReadToEnd();

                // Do '<img>' modification
                RemoveLinkedImageDimensions(ref strXml, outputString);

                // Add the rest of the XML file
                outputString.Append(strXml);

                strXml = AddCustomXslToViewFile(outputString.ToString());
                outputString.Clear();
                outputString.Append(strXml);

                // Enable local HTML anchors
                outputString = outputString.Replace("http:///#", "#");

                // Preserve formatting
                outputString = outputString.Replace("WHITE-SPACE: normal;", "WHITE-SPACE: pre-wrap;");

                //
                // Write the output to disk 
                //
                outputStream = new FileStream(outputFile,  FileMode.Create, FileAccess.Write); 
                outputStream.Close();
                           
                writer = new StreamWriter(outputFile, true, Encoding.UTF8);
                writer.Write(outputString.ToString());
            }

            finally
            {   
                if (inputStream != null)
                {
                    inputStream.Close();
                }

                if (outputStream != null)
                {
                    outputStream.Close();
                }

                if (reader != null)
                {
                    reader.Close();
                }

                if (writer != null)
                {
                    writer.Close();
                }
            }

            return outputFile;
        }

        private static readonly XNamespace _xslNamespace = "http://www.w3.org/1999/XSL/Transform";

        private static string AddCustomXslToViewFile(string inputXsl)
        {
            var reader = new XmlTextReader(new MemoryStream(new UTF8Encoding().GetBytes(inputXsl)));
            XmlNameTable table = reader.NameTable;
            if (table == null) return inputXsl;

            var nsm = new XmlNamespaceManager(table);
            nsm.AddNamespace("xsl", _xslNamespace.ToString());

            var xsl = XDocument.Load(reader, LoadOptions.None);
            if (xsl.Root == null) return inputXsl;

            // Set HTML anchor for test cases (i.e. #1 to #n)
            //
            var testCaseDiv = xsl.XPathSelectElements("//xsl:template[@match='ns1:testCase']/div", nsm);
            testCaseDiv.ForEach(t => t.AddFirst(AddXslForSettingAnchorId("ns1:id")));

            return xsl.ToString();
        }

        private static XElement AddXslForSettingAnchorId(string value)
        {
            // This will add the following:
            //
            //  <xsl:attribute name="id">
            //      <xsl:value-of select="{anchorId}"/>
            //  </xsl:attribute> 

            var attribute = new XElement(_xslNamespace + "attribute");
            attribute.Add(new XAttribute("name", "id"));
            var valueOf = new XElement(_xslNamespace + "value-of");
            valueOf.Add(new XAttribute("select", value));
            attribute.Add(valueOf);
            return attribute;
        }

        ///********************************************************************
        /// Method RemoveLinkedImageDimensions
        /// 
        /// <summary>
        ///         This method modifies the input file in order to remove the height
        ///         and width dimensions of an <img> tag in the view file.
        /// </summary>
        /// 
        /// <param name="xml">The xsl view file to modify</param>
        /// <param name="outputString">The StringBuilder object where the output string is built</param>
        /// 
        private static void RemoveLinkedImageDimensions(ref string xml, StringBuilder outputString)
        {
            const string toFind = "<img ";

            int pos1 = 0;

            bool blnLinkedImage = false;

            //
            // Find the occurance of '<img ' and remove its height and width dimensions
            // **ONLY** if the image is linked to an external file.
            //
            while((pos1 = xml.IndexOf(toFind, pos1 + toFind.Length, StringComparison.Ordinal)) >= 0)
            {
                // Get the position of the closing tag
                var pos2 = 0;
                if ((pos2 = xml.IndexOf(">", pos1 + toFind.Length, StringComparison.Ordinal)) >= 0)
                {
                    // NOTE:
                    //
                    //    Surprisingly, not all builds of InfoPath embed a 'linked=true' attribute 
                    //    on an HTML <img> tag if the image is linked to an external file. 
                    // 
                    //    Question: With that said, how do we know if an image is indeed linked ?
                    //
                    //    Answer:   An image is linked if a 'linked=true' attribute exist 
                    //              **OR** 
                    //              the value of the 'src' attribute contains a '\' or '/' character 
                    //              as InfoPath always uses the absolute location of the file.
                    //
                    // BEFORE:
                    //          <img style="WIDTH: 165px; HEIGHT: 44px" height="44" src="../../../XXX.gif" width="165" border="0" />                           
                    //
                    // AFTER:
                    //          <img src="../../../XXX.gif" border="0" /> 
                    //
                        
                    var originalTag = xml.Substring(pos1, pos2 - pos1 + 1);

                    //
                    // Check if the image is linked
                    //

                    // Test for the 'linked=true' attribute
                    blnLinkedImage |= originalTag.Trim().ToLower().IndexOf("linked=\"true\"", StringComparison.Ordinal) >= 0;

                    // Get the 'src' attribute's value
                    var temp = GetTagAttributeValue(originalTag, "src");

                    // Test for the '\' or '/' characters
                    blnLinkedImage |= (temp.IndexOf("\\", StringComparison.Ordinal) >= 0) || (temp.IndexOf("/", StringComparison.Ordinal) >= 0);

                    // Make a partial copy of the original Xml
                    temp = xml.Substring(0, pos2 + 1);

                    if (blnLinkedImage)
                    {
                        var modifiedTag = RemoveTagAttribute(originalTag, "style");
                        modifiedTag = RemoveTagAttribute(modifiedTag, "width");
                        modifiedTag = RemoveTagAttribute(modifiedTag, "height");
                        modifiedTag = MakeAbsolutePathRelativeToOutput(modifiedTag);

                        // Replace tag
                        temp = temp.Replace(originalTag, modifiedTag);
                    }

                    outputString.Append(temp);

                    // Remove the part that we already searched 
                    xml = xml.Substring(pos2 + 1);

                    // Reposition pos1 to the start of the file
                    pos1 = 0;
                }
            }
        }

        ///********************************************************************
        /// Method MakeAbsolutePathRelativeToOutput
        /// 
        /// <summary>
        ///         Make path compatible in a Linux system
        /// </summary>
        /// 
        /// <param name="inputTag">The HTML or XML tag string in question, enclosed in <> angle brackets</param>
        /// 
        /// <returns>The modified HTML or XML tag string</returns>
        /// 
        private static string MakeAbsolutePathRelativeToOutput(string inputTag)
        {
            var origSource = GetTagAttributeValue(inputTag, "src");            
            if (!string.IsNullOrWhiteSpace(origSource) && (inputTag.IndexOf(":\\", StringComparison.OrdinalIgnoreCase) > -1 || inputTag.IndexOf(":/", StringComparison.OrdinalIgnoreCase) > -1))
            {
                var fileName = GetFileNameEx(origSource);
                if (fileName != null)
                {
                    var assemblyDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    if (assemblyDir != null)
                    {
                        var newSource = Path.Combine(assemblyDir, "Output", fileName);

                        if (Program.IsLinux)
                        {
                            newSource = newSource.Replace("/home/", "/"); 
                        }

                        inputTag = inputTag.Replace(origSource, newSource);
                        LogVerbose(string.Format("Replaced absolute path '{0}' with '{1}'", origSource, newSource));
                    }
                }
            }

            // Return unchanged
            return inputTag;
        }

        // This won't work as expected in Linux when prefixed by a drive letter 
        // var fileName = Path.GetFileName(origSource);
        private static string GetFileNameEx(string path)
        {
            int idx = path.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase);
            if (idx > -1)
            {
                return path.Substring(idx + 1);
            }

            idx = path.LastIndexOf("/", StringComparison.OrdinalIgnoreCase);
            if (idx > -1)
            {
                return path.Substring(idx + 1);
            }

            return null;
	    }

        ///********************************************************************
        /// Method RemoveTagAttribute
        /// 
        /// <summary>
        ///         Removes an attribute from an XML or HTML element tag
        /// </summary>
        /// 
        /// <param name="inputTag">The HTML or XML tag string in question, enclosed in <> angle brackets</param>
        /// <param name="strAttributeName">The attribute name</param>
        /// 
        /// <returns>The modified HTML or XML tag string</returns>
        /// 
        private static string RemoveTagAttribute(string inputTag, string strAttributeName)
        {
            int pos1;

            // NOTE: The tag must be well-formed (i.e. its value must be enclosed in quotation marks).
            //       Otherwise, it will be ignored

            // Get position of the tag
            if ((pos1 = inputTag.ToLower().IndexOf(strAttributeName.ToLower() + "=\"", StringComparison.Ordinal )) >= 0)
            {
                // Found, Get position of the enclosing quotation mark
                int pos2;
                if ((pos2 = inputTag.IndexOf("\"", pos1 + strAttributeName.Length + 2, StringComparison.Ordinal)) >= 0)
                {
                    var toRemove = inputTag.Substring(pos1, pos2 - pos1 + 1);

                    // Found, return modified string 
                    return inputTag.Replace(toRemove, "");
                }
            }

            // Return unchanged
            return inputTag;
        }

        ///********************************************************************
        /// Method GetTagAttributeValue
        /// 
        /// <summary>
        ///         Returns the specified HTML tag's attribute value
        /// </summary>
        /// 
        /// <param name="inputTag">The HTML or XML tag string in question, enclosed in <> angle brackets</param>
        /// <param name="attributeName">The attribute name</param>
        /// 
        /// <returns>The attribute value</returns>
        /// 
        private static string GetTagAttributeValue(string inputTag, string attributeName)
        {
            int pos1;

            // NOTE: The tag must be well-formed (i.e. its value must be enclosed in quotation marks).
            //       Otherwise, it will be ignored

            // Get position of the tag
            if ((pos1 = inputTag.ToLower().IndexOf(attributeName.ToLower() + "=\"", StringComparison.Ordinal )) >= 0)
            {
                // Found, Get position of the enclosing quotation mark
                int pos2;
                if ((pos2 = inputTag.IndexOf("\"", pos1 + attributeName.Length + 2, StringComparison.Ordinal)) >= 0)
                {
                    // Found, return value 
                    return inputTag.Substring(pos1 + attributeName.Length + 2, pos2 - (pos1 + attributeName.Length + 2));
                }
            }

            // Not found
            return null;
        }

        private static void LogVerbose(string message, bool newLine = true)
        {
            Program.LogVerbose(message, newLine);
        }

	}
}
