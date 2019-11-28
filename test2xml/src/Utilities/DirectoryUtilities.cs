using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Test2Xml.Utilities
{
    class DirectoryUtilities
    {
        public static readonly string[] TemplateExtensions = { ".xsn", ".xsl" };
        public static readonly string[] InputExtensions = { ".txt", ".tsv", ".robot", ".xml", ".html", ".htm" };
        public static readonly string[] OutputExtensions = { ".xml", ".html", ".htm" };
        public static readonly string[] IgnoredFiles = { "__", "index" };

        public static bool IsHtmlFile(string file)
        {
            var ext = Path.GetExtension(file);
            return ext.EqualsIgnoreCase(".html") || ext.EqualsIgnoreCase(".htm");
        }

        public static string[] GetFiles(String path, string[] extensions)
        {
            Program.LogVerbose("Reading files...");

            var files = new List<string>();
            if (File.Exists(path))
            {
                files.Add(path);
                Program.LogVerbose("Added: " + path);
            }
            else if (Directory.Exists(path))
            {
                ProcessDirectory(path, files, extensions);
            }

            Program.LogVerbose(files.Count() + " files returned from file/path: " + path);

            return files.ToArray();
        }

        private static void ProcessDirectory(string targetDirectory, List<string> files, string[] extensions)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (var file in fileEntries)
            {
                var filename = Path.GetFileName(file);
                if (filename != null && IsIgnoreFile(filename))
                {
                    continue;
                }

                foreach (string ext in extensions)
                {
                    if (Path.GetExtension(file).EqualsIgnoreCase(ext))
                    {
                        files.Add(file);
                        Program.LogVerbose("Added: " + file);
                        break;
                    }
                }
            }

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                if (subdirectory != null && IsIgnoreFile(Path.GetFileName(subdirectory)))
                {
                    continue;
                }

                ProcessDirectory(subdirectory, files, extensions);
            }
        }

        public static TableOfContents GetTableOfContents(String path, string[] extensions)
        {
            var toc = new TableOfContents();
            if (Directory.Exists(path))
            {
                var level = "0";
                var category = new Category { id = level, name = GetDirectoryName(path).SpaceCamelCase() };
                toc.category = new[] { category };
                ProcessDirectory(path, category, extensions, level);
                toc.lastUpdate = DateTime.Now;
            }
            else
            {
                throw new ArgumentException("Existing directory is expected.", "path");
            }

            return toc;
        }

        private static void ProcessDirectory(string targetDirectory, Category targetCategory, string[] extensions, string parentLevel)
        {
            var categories = new List<Category>();
            var items = new List<SingleValue>();
            var itemCounter = 0;

            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (var file in fileEntries)
            {
                var filename = Path.GetFileNameWithoutExtension(file);
                if (filename != null && IsIgnoreFile(filename))
                {
                    continue;
                }

                foreach (string ext in extensions)
                {
                    if (Path.GetExtension(file).EqualsIgnoreCase(ext))
                    {
                        items.Add(new SingleValue() { id = ++itemCounter, name = filename.SpaceCamelCase(), value = GetAbsolutePathUrl(file) });
                        Program.LogVerbose("Added: " + file);
                        break;
                    }
                }
            }

            var lastCount = 0;
            String[] splits = null;
            splits = parentLevel.Split(new char['.'], StringSplitOptions.None);
            if (splits.Length > 1)
            {
                if (int.TryParse(splits[splits.Length - 1], out lastCount))
                {
                    splits[splits.Length - 1] = (++lastCount).ToString();
                    targetCategory.id = string.Join(".", splits);
                }
            }
            else
            {
                targetCategory.id = splits[0];
            }

            // Recurse into subdirectories of this directory. 
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            for(int i=0; i < subdirectoryEntries.Length; i++)
            {
                string subdirectory = subdirectoryEntries[i];
                var currentLevel = GetDirectoryId(targetCategory.id, i + 1);
                var category = new Category() { id = currentLevel, name = GetDirectoryName(subdirectory).SpaceCamelCase() };
                categories.Add(category);
                ProcessDirectory(subdirectory, category, extensions, currentLevel);
            }

            targetCategory.item = items.ToArray();
            targetCategory.category = categories.ToArray();
        }

        private static string GetAbsolutePathUrl(string file)
        {            
            var absolutePath = Path.GetFullPath(file);

            if (Program.IsLinux)
            {
                absolutePath = file.Replace("/home/", "/");
            }

            Program.LogVerbose("Hyperlink is " + absolutePath);
            
            return absolutePath;
        }

        private static string GetDirectoryId(string parentId, int idx)
        {
            if (parentId == "0")
            {
                return idx.ToString();
            }

            return string.Format("{0}.{1}", parentId, idx);
        }

        private static string GetDirectoryName(string path)
        {
            int idx = path.LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase);
            if (idx > -1)
            {
                return path.Substring(idx + 1);
            }
            return path;
        }

        public static bool IsIgnoreFile(string filename)
        {
            return IgnoredFiles.Any(p => filename != null && filename.StartsWithIgnoreCase(p));
        }

        public static void CopyResourcesToLocal(IEnumerable<string> resources, string outputPath)
        {
            foreach (var source in resources)
            {
                var outputDir = Path.GetDirectoryName(outputPath);
                if (outputDir != null)
                {
                    var sourceFileName = Path.GetFileName(source);
                    if (sourceFileName != null)
                    {
                        string newFilePath = Path.Combine(outputDir, sourceFileName);
                        if (!File.Exists(newFilePath))
                        {
                            File.Copy(source, newFilePath);
                        }
                    }
                }
            }
        }
    }
}
