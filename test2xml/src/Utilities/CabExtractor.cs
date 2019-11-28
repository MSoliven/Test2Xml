using System;

namespace Test2Xml.Utilities
{
    using System.Collections;
    using System.IO;

    /// <summary>
    /// Linux machines: Please install cabextract for Linux (see http://www.cabextract.org.uk/)
    /// </summary>
    public class CabExtractor
    {
        private readonly ArrayList _extractedFiles = new ArrayList();

        private readonly string _cabinetFile;

        public CabExtractor(string cabinetFile)
        {
            _cabinetFile = cabinetFile;
        }

        public ArrayList ExtractTo(string extractDir)
        {
            string output = null;

            if (!string.IsNullOrEmpty(_cabinetFile) && !string.IsNullOrEmpty(extractDir))
            {
                output = ExecuteCommandSync(
                    string.Format("\"{0}\" -d {1}", _cabinetFile, extractDir));
            }

            if (output != null)
            {
                string[] lines = output.Split(new char[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    if (line.Contains(extractDir))
                    {
                        string[] split = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                        var file = split[split.Length - 1];
                        _extractedFiles.Add(file);
                        Program.LogVerbose("Extracted: " + file);
                    }
                }
            }

            if (_extractedFiles.Count > 0)
            {
                return _extractedFiles;
            }

            return null;
        }

        private string ExecuteCommandSync(string args)
        {
            string result = null;

            try
            {
                System.Diagnostics.ProcessStartInfo procStartInfo = null;

                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                if (!Program.IsLinux)
                {
                    var appPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
                    if (appPath != null)
                    {
                        var exePath = Path.Combine(appPath, "cabextract.exe");
                        if (File.Exists(exePath))
                        {
                            var cmdArgsToExecute = string.Format("/c {0} {1}", exePath, args);
                            Program.LogVerbose("Running cmd: " + cmdArgsToExecute);

                            procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", cmdArgsToExecute);
                        }
                        else
                        {
                            throw new Exception("cabextract.exe not found in test2xml folder");
                        }
                    }
                    else
                    {
                        throw new Exception("Failed to get assembly location");
                    }
                }
                else
                {
                    Program.LogVerbose("Running cabextract with the following arguments: " + args);

                    procStartInfo = new System.Diagnostics.ProcessStartInfo("cabextract", args);
                }

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                result = proc.StandardOutput.ReadToEnd();
                if (result.Contains("exception"))
                {
                    throw new Exception("Cabextract has thrown an exception: " + result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to run cabextract.", ex);
            }

            return result;
        }
    }
}