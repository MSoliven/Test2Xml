using System;

namespace CabExtract
{
    using System.Collections;
    using System.IO;
    using Microsoft.Deployment.Compression;
    using Microsoft.Deployment.Compression.Cab;

    /// <summary>
    /// This is not used in Linux (Use cabextract for Linux instead, see http://www.cabextract.org.uk/)
    /// </summary>
    public class CabExtractor
    {
        private readonly ArrayList _extractedFiles = new ArrayList();

        private readonly string _cabinetFile;

        private string _extractDir;

        public CabExtractor(string cabinetFile)
        {
            _cabinetFile = cabinetFile;
        }

        public ArrayList ExtractTo(string extractDir)
        {
            _extractDir = extractDir;

            Console.WriteLine("Extracting cabinet: " + _cabinetFile);
            CabInfo cab = new CabInfo(_cabinetFile);
            cab.Unpack(_extractDir, UnpackProgressHandler);

            if (_extractedFiles.Count > 0)
            {
                return _extractedFiles;
            }

            return null;
        }

        public void UnpackProgressHandler(object sender, ArchiveProgressEventArgs e)
        {
            if (e.ProgressType == ArchiveProgressType.FinishFile)
            {
                var filePath = Path.Combine(_extractDir, e.CurrentFileName);
                Console.WriteLine(string.Format("  extracting {0}", filePath));
                _extractedFiles.Add(filePath);
            }
        }
    }
}