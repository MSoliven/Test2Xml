using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Test2Xml.Utilities;

namespace Test2Xml.Commands
{
    public class ListViewsCommand : CommandBase
    {
        private readonly HashSet<string> _views;

        public ListViewsCommand(Options options) : base(options)
        {
            _views = new HashSet<string>();
        }

        public override void Execute()
        {
            LogVerbose("Attempting to read all available views...");


            foreach (var templateFile in DirectoryUtilities.GetFiles(Options.Template, DirectoryUtilities.TemplateExtensions))
            {
                var templateExt = Path.GetExtension(templateFile);                    
                if (templateExt == null) continue;

                if (templateExt.EqualsIgnoreCase(".xsn"))
                {
                    try
                    {
                        // Set the location of the InfoPath solution file and the output directory where the
                        // resource files in the archive will be extracted to
                        string outputDir = Path.GetTempPath();

                        // Extract the template and associated files from the Infopath CAB file.
                        // This will also load the Xsl into XslHashtable
                        var docTemplate = InfoPathUtilities.LoadXslFromInfoPath(templateFile, outputDir);
                        if (docTemplate != null)
                        {
                            var viewNames = docTemplate.XslTransforms.Keys.Cast<string>().ToArray();
                            Array.Sort(viewNames, StringComparer.InvariantCulture);

                            foreach (var viewName in viewNames)
                            {
                                if (viewName.Equals(docTemplate.DefaultViewName))
                                {
                                    var key = viewName + "|Default";
                                    if (!TryAddView(key))
                                    {
                                        LogVerbose("Warning: Duplicate view name: " + key);
                                    }
                                }
                                else
                                {
                                    if (!TryAddView(viewName))
                                    {
                                        LogVerbose("Warning: Duplicate view name: " + viewName);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("[Failed to load " + templateFile + "]");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[Exception: " + ex.Message + "]");
                    }                    
                }
                else 
                {
                    var viewName = Path.GetFileName(templateFile);

                    if (TryAddView(viewName))
                    {
                        LogVerbose("Warning: Duplicate view name: " + viewName);
                    }
                }
            }

            foreach (var viewName in _views.ToArray())
            {
                Console.WriteLine(viewName);
            }
        }

        private bool TryAddView(string viewName)
        {
            if (!_views.Contains(viewName))
            {
                _views.Add(viewName);
                return true;
            }

            return false;
        }

    }
}
