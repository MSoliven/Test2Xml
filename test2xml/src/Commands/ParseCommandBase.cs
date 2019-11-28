using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Test2Xml.Utilities;

namespace Test2Xml.Commands
{
    public abstract class ParseCommandBase : IoCommandBase
    {
        private string _currentLine, _lastLine;
        private SectionEnum _currentSection;
        private TestCase _currentTestCase;

        private readonly List<string> _columnNames = new List<string>();
        private List<MultiValue> _settings = new List<MultiValue>();
        private List<Keyword> _keywords = new List<Keyword>();

        private enum SectionEnum
        {
            Undef,
            Settings,
            Variables,
            TestCases,
            Keywords
        }
        
        static readonly string[][] Sections =
        {
            new string[] { null }, 
            new string[] { "Setting" },
            new string[] { "Variable" }, 
            new string[] { "Test Case", "TestCase" }, 
            new string[] { "Keyword" }
        };

        private enum SuiteSettingsEnum
        {
            Undef,
            SuiteSetup,
            SuiteTeardown,
            TestSetup,
            TestTeardown,
            TestTemplate,
            TestTimeout,
            Resource,
            Metadata
        }

        static readonly string[] SuiteSettings =
        {
            null, 
            "Suite Setup", 
            "Suite Teardown", 
            "Test Setup", 
            "Test Teardown", 
            "Test Template", 
            "Test Timeout", 
            "Resource", 
            "Metadata"
        };

        private string _testTemplate;

        protected string SelectedView { get; private set; }        

		protected TestSuite TestSuite
        {
            get { return (TestSuite) BusinessObject; }            
        }

        private readonly ExecutionStats _stats;

        protected ParseCommandBase(Options options, ExecutionStats stats) : base(options, stats)
        {
            _stats = stats;
        }

        protected TestSuite Parse(TextReaderBase reader, string inputFile)
        {
            TestSuite testSuite = null;

            var settings = new List<MultiValue>();
            var variables = new List<MultiValue>();
            var keywords = new List<Keyword>();
            var testCases = new List<TestCase>();

            int lineCount = 0, settingCount = 0, variableCount = 0, testCaseCount = 0, keywordCount = 1;

            while (reader.Peek() >= 0)
            {
                if (testSuite == null)
                {
                    testSuite = new TestSuite { suiteName = ParseTestSuiteName(inputFile) };                    
                }

                _lastLine = _currentLine;
                _currentLine = reader.ReadLine();

                LogVerbose(string.Format("{0}: {1}", ++lineCount, _currentLine));

                if (string.IsNullOrWhiteSpace(_currentLine))
                {
                    continue;
                }

                if (IsNewSection(_currentLine, SectionEnum.Settings))
                {
                    _currentSection = SectionEnum.Settings;
                }
                else if (IsNewSection(_currentLine, SectionEnum.Variables))
                {
                    _currentSection = SectionEnum.Variables;
                }
                else if (IsNewSection(_currentLine, SectionEnum.TestCases))
                {
                    _currentSection = SectionEnum.TestCases;
                    settingCount = 0;

                    var splits = reader.Split(_currentLine);
                    if (splits.Length > 1)
                    {
                        for (int i = 0; i < splits.Length; i++)
                        {
                            var token = splits[i].Trim();                            
                            if (i > 0)
                            {
                                _columnNames.Add(token);
                            }
                        }
                    }
                }
                else if (IsNewSection(_currentLine, SectionEnum.Keywords))
                {
                    _currentSection = SectionEnum.Keywords;
                }
                else
                {
                    switch (_currentSection)
                    {
                        case SectionEnum.Settings:
                            settings = ParseMultiValueLine(reader, _currentLine, settings, ++settingCount);
                            break;
                        case SectionEnum.Variables:
                            variables = ParseMultiValueLine(reader, _currentLine, variables, ++variableCount);
                            break;
                        case SectionEnum.Keywords:
                            // Not really needed, showing details of keywords at test suite level is bad practice
                            // keywords = ParseKeywordLine(reader, _currentLine, keywords, ref keywordCount, false);
                            break;
                        case SectionEnum.TestCases:
                            testCases = ParseTestCaseLine(reader, _currentLine, testCases, ref testCaseCount, ref keywordCount, ref settingCount);                            
                            break;
                    }
                }
            }

            if (testSuite != null)
            {
                if (_currentTestCase != null)
                {
                    CommitTestCase();
                }

                testSuite.setting = settings.ToArray();
                testSuite.variable = variables.ToArray();
                testSuite.testCase = testCases.ToArray();
                testSuite.keyword = keywords.ToArray();
                testSuite.row = GenerateTestCaseRows(testCases);
                testSuite.lastUpdate = DateTime.Now;
            }

            return testSuite;
        }

        private static string ParseTestSuiteName(string inputFile)
        {
            var fileName = Path.GetFileNameWithoutExtension(inputFile);
            return fileName.SpaceCamelCase().Trim();
        }

        private TestCaseRow[] GenerateTestCaseRows(IEnumerable<TestCase> testCases)
        {
            var testCaseRows = new List<TestCaseRow>();
            var maxColumnCount = 0;
            var rowId = 0;

            // Pass 1: get maximum column count and collect their names
            foreach (var testCase in testCases)
            {
                if (testCase.keyword != null)
                {
                    foreach (var keyword in testCase.keyword)
                    {
                        if (keyword.argument.Length > maxColumnCount)
                        {
                            maxColumnCount = keyword.argument.Length;
                            for (var i=0; i < maxColumnCount; i++)
                            {
                                if (i < _columnNames.Count)
                                {
                                    _columnNames[i] = keyword.argument[i].name;
                                }
                                else
                                {
                                    _columnNames.Add(keyword.argument[i].name);
                                }
                            }
                        }
                    }
                }
            }

            // Pass 2: create the rows, assign the column names from the first pass
            foreach (var testCase in testCases)
            {
                var cols = new List<SingleValue>();

                if (testCase.keyword != null)
                {
                    foreach (var keyword in testCase.keyword)
                    {
                        for (var i = 0; i < maxColumnCount; i++)
                        {
                            if (i < keyword.argument.Length)
                            {
                                keyword.argument[i].name = keyword.argument[i].name.Replace("=", "");
                                cols.Add(keyword.argument[i]);
                            }
                            else
                            {
                                if (!_columnNames[i].StartsWith(DefaultColumnName))
                                {
                                    cols.Add(new SingleValue
                                    {
                                        id = i,
                                        name = _columnNames[i].Replace("=", ""),
                                        value = string.Empty,
                                        tag = GetTagBasedOnName(_columnNames[i])
                                    });
                                }
                                else
                                {
                                    cols.Add(new SingleValue
                                    {
                                        id = i,
                                        name = _columnNames[i].Replace("=", ""),
                                        value = string.Empty,
                                    });                                    
                                }
                            }
                        }
                        break;
                    }
                    var row = new TestCaseRow { id = ++rowId, column = cols.ToArray() };
                    testCaseRows.Add(row);
                }
            }

            return testCaseRows.ToArray();
        }

        private void CommitTestCase()
        {
            _currentTestCase.setting = _settings.ToArray();
            _currentTestCase.keyword = _keywords.ToArray();
            _settings.Clear();
            _keywords.Clear();
        }

        private List<TestCase> ParseTestCaseLine(TextReaderBase reader, string line, List<TestCase> testCases, 
            ref int testCaseCount, ref int keywordCount, ref int settingCount)
        {
            string[] tokens;
            if (!IsAppendToPreviousLine(reader, line, out tokens))
            {
                bool isComment, isSetting, isNewTestCase;
                EvaluateTestCaseLine(tokens, out isComment, out isSetting, out isNewTestCase);

                if (isNewTestCase)
                {
                    if (_currentTestCase != null)
                    {
                        CommitTestCase();
                        settingCount = 0;
                        keywordCount = 1;
                    }

                    _currentTestCase = new TestCase
                    {
                        id = ++testCaseCount,
                        name = tokens[0].Trim(),
                    };
                    testCases.Add(_currentTestCase);
                }

                if (tokens.Length > 0)
                {
                    if (isSetting)
                    {
                        var offset = !isNewTestCase ? 0 : 1;

                        _settings = ParseMultiValueLine(reader, _currentLine, _settings, ++settingCount, offset);
                    }
                    else if (_currentTestCase != null)
                    {
                        _keywords = ParseTestCaseKeywordLine(reader, _currentLine, _keywords, ref keywordCount, isNewTestCase);                        
                    }
                }
            }
            else 
            {
                // Append to previous line before parsing
                if (tokens.Length > 1)
                {
                    _currentLine = _lastLine + tokens[1];
                }
                else
                {
                    _currentLine = _lastLine;
                }

                var tokens2 = reader.Split(_currentLine);

                bool isComment, isSetting, isNewTestCase;
                EvaluateTestCaseLine(tokens2, out isComment, out isSetting, out isNewTestCase);

                if (isSetting)
                {
                    var offset = !isNewTestCase ? 0 : 1;

                    _settings.RemoveAt(_settings.Count - 1);
                    settingCount--;
                    _settings = ParseMultiValueLine(reader, _currentLine, _settings, ++settingCount, offset);
                }
                else if (_currentTestCase != null)
                {
                    _keywords.RemoveAt(_keywords.Count - 1);
                    keywordCount--;
                    _keywords = ParseTestCaseKeywordLine(reader, _currentLine, _keywords, ref keywordCount, isNewTestCase);
                }
                else
                {
                    testCases.RemoveAt(testCases.Count - 1);
                    testCaseCount--;
                    keywordCount--;
                    settingCount--;
                    testCases = ParseTestCaseLine(reader, _currentLine, testCases, ref testCaseCount, ref keywordCount, ref settingCount);
                }
            }

            return testCases;
        }

        private void EvaluateTestCaseLine(string[] tokens, out bool isComment, out bool isSetting, out bool isNewTestCase)
        {
            isComment = tokens.Any(s => s.StartsWith("#")) || tokens.Any(s => s.EqualsIgnoreCase("Comment"));
            isSetting = tokens.Any(s => s.StartsWith("["));
            isNewTestCase = _currentLine.StartsWith(tokens[0]);            
        }

        private List<Keyword> ParseTestCaseKeywordLine(TextReaderBase reader, string line, List<Keyword> keywords, ref int keywordCount, bool isNewTestCase)
        {
            string[] tokens;
            if (!IsAppendToPreviousLine(reader, line, out tokens))
            {
                var isTestcaseInline = isNewTestCase || (_columnNames.Count > 0 && tokens.Length == _columnNames.Count + 2);
                var isKeywordInline = (!isTestcaseInline && string.IsNullOrEmpty(_testTemplate)) || tokens[0].Trim().Equals(_testTemplate);
                var arguments = new List<SingleValue>();
                var argumentCount = 0;

                if (isTestcaseInline && tokens.Length == 1)
                {
                    return keywords;
                }

                var keyword = new Keyword(); 

                var offset = 0;
                if (isTestcaseInline && isKeywordInline)
                {
                    if (string.IsNullOrWhiteSpace(_currentTestCase.name))
                    {
                        _currentTestCase.name = tokens[0].Trim();
                    }
                    if (string.IsNullOrWhiteSpace(keyword.name))
                    {
                        if (tokens.Length > 1)
                        {
                            keyword.name = tokens[1].Trim();
                        }
                    }
                    offset = 1;
                }
                else if (isTestcaseInline)
                {
                    if (string.IsNullOrWhiteSpace(_currentTestCase.name))
                    {
                        _currentTestCase.name = tokens[0].Trim();
                    }
                    offset = 1;
                }
                else if (isKeywordInline)
                {
                    if (string.IsNullOrWhiteSpace(keyword.name))
                    {
                        keyword.name = tokens[0].Trim();
                    }
                }
                else 
                {
                    if (string.IsNullOrWhiteSpace(keyword.name))
                    {
                        keyword.name = _testTemplate;
                    }                    
                }

                for (var i = 0; i < tokens.Length; i++)
                {
                    var token = tokens[i].Trim();
                    
                    if (i >= offset)
                    {
                        if (i == offset)
                        {
                            if (token.StartsWith("${") && token.EndsWith("}="))
                            {
                                keyword.retval = new SingleValue {id = 1, name = "Return Value", value = token};
                            }
                        }                        
                        arguments = ParseKeywordPlusArguments(arguments, ++argumentCount, token, isKeywordInline);
                    }
                }
                
                keyword.argument = arguments.ToArray();
                keyword.id = keywordCount++;
                _keywords.Add(keyword);
            }
            else
            {
                // Append to previous line before parsing
                if (tokens.Length > 1)
                {
                    _currentLine = _lastLine + tokens[1];
                }
                else
                {
                    _currentLine = _lastLine;
                }

                // Replace last one added
                keywords.RemoveAt(keywords.Count - 1);
                keywordCount--;
                keywords = ParseTestCaseKeywordLine(reader, _currentLine, keywords, ref keywordCount, isNewTestCase);
            }

            return keywords;
        }

        public const string DefaultColumnName = "Argument";

        private List<SingleValue> ParseKeywordPlusArguments(List<SingleValue> arguments, int argsId, string token, bool isKeywordInline)
        {
            if (_columnNames.Count > 0)
            {
                var columnName = "";
                if (_columnNames.Count >= argsId)
                {
                    columnName = _columnNames[argsId - 1].Replace("*", "");
                }

                arguments.Add(new SingleValue()
                {
                    id = argsId,
                    name = columnName,
                    value = FormatValue(token),
                    tag = GetTagBasedOnName(columnName)
                });
            }
            else
            {
                var name = string.Format("{0} {1}", DefaultColumnName, argsId);                    

                if (isKeywordInline && argsId == 1)
                {
                    name = "Keyword";
                }
                else if (isKeywordInline)
                {
                    name = string.Format("{0} {1}", DefaultColumnName, (argsId - 1));                    
                }
                arguments.Add(new SingleValue()
                {
                    id = argsId,
                    name = name,
                    value = FormatValue(token)
                });
            }

            return arguments;
        }

        private string FormatValue(string s)
        {
            if (JsonFormatter.IsJson(s))
            {
                s = JsonFormatter.Format(s);
            }
            else if (PipeFormatter.IsPipeDelimited(s))
            {
                s = PipeFormatter.Format(s);
            }
            else if (SemicolonFormatter.IsSemicolonDelimitedExp(s))
            {
                s = SemicolonFormatter.Format(s);
            }
            else if (LongWordFormatter.IsLongWord(s))
            {
                s = LongWordFormatter.Format(s);
            }
            else
            {
                s = s.Replace("\\ ", " ");
                s = s.Replace("\\n", Environment.NewLine);
                s = s.Replace("&lt;", "<");
                s = s.Replace("&gt;", ">");
            }

            return s;
        }

        private List<MultiValue> ParseMultiValueLine(TextReaderBase reader, string line, List<MultiValue> multiValues, int multiValueId, int offset = 0)
        {            
            string[] tokens;
            if (!IsAppendToPreviousLine(reader, line, out tokens))
            {
                var multiValue = new MultiValue();
                var values = new List<string>();

                multiValue.id = multiValueId;
                multiValue.name = tokens[offset].Trim();

                for (int i = offset + 1; i < tokens.Length; i++)
                {
                    var token = tokens[i].Trim();
                    if (token.Contains("ImageUrl"))
                    {
                        var token2 = token.Split(new string[] { ": " },
                            StringSplitOptions.RemoveEmptyEntries);
                        for (var j = 0; j < token2.Length; j++)
                        {
                            if (token2[j].Contains("ImageUrl"))
                            {
                                multiValue.tag = token2[j + 1].Trim();
                            }
                        }
                    }

                    values.Add(FormatValue(token));
                }

                multiValue.value = values.ToArray();
                multiValues.Add(multiValue);

                if (_currentSection == SectionEnum.Settings)
                {
                    if (multiValue.value != null && multiValue.value.Length > 0)
                    {
                        if (IsSectionEquals(multiValue.name, SuiteSettingsEnum.TestTemplate))
                        {
                            _testTemplate = multiValue.value[offset];
                        }

                        if (IsSectionEquals(multiValue.name, SuiteSettingsEnum.Metadata))
                        {
                            if (multiValue.value[offset].EqualsIgnoreCase("view"))
                            {
                                SelectedView = multiValue.value[offset + 1];
                            }
                        }
                    }
                }
            }
            else 
            {
                // Append to previous line before parsing
                if (tokens.Length > 1)
                {
                    _currentLine = _lastLine + Environment.NewLine + tokens[1];
                }
                else
                {
                    _currentLine = _lastLine + Environment.NewLine;
                }
                // Replace last one added
                multiValues.RemoveAt(multiValues.Count - 1);  

                switch (_currentSection)
                {
                    case SectionEnum.Settings:
                        multiValues = ParseMultiValueLine(reader, _currentLine, multiValues, multiValueId);
                        break;
                    case SectionEnum.Variables:
                        multiValues = ParseMultiValueLine(reader, _currentLine, multiValues, multiValueId);
                        break;
                }               
            }

            return multiValues;
        }
        
        private bool IsAppendToPreviousLine(TextReaderBase reader, string line, out string[] tokens)
        {
            tokens = reader.Split(line);

            // If there is more data than readily fits a row, it possible to use ellipsis (...) to continue the previous line. 
            // In test case and user keyword tables, the ellipsis must be preceded by at least one empty cell. 
            // In settings and variable tables, it can be placed directly under the setting or variable name. 
            // In all tables, all empty cells before the ellipsis are ignored. 
            if (_currentSection == SectionEnum.TestCases || _currentSection == SectionEnum.Keywords)
            {
                var tokens2 = reader.Split(line, StringSplitOptions.None);
                if (string.IsNullOrWhiteSpace(tokens2[0]) && tokens[0].StartsWithEllipse())
                {
                    return true;
                }
            }
            else if (tokens[0].StartsWithEllipse())
            {
                return true;
            }

            return false;
        }

        private static bool IsNewSection(string currentLine, SectionEnum sectionEnum)
        {
            bool retVal = false;
            var array = Sections[(int)sectionEnum];
            foreach (string s in array)
            {
                var s1 = string.Format("*** {0} ***", s);
                var s2 = string.Format("*{0}*", s);
                var s3 = string.Format("*** {0}s ***", s);
                var s4 = string.Format("*{0}s*", s);

                retVal |= (currentLine.StartsWithIgnoreCase(s1) || currentLine.StartsWithIgnoreCase(s2) ||
                         currentLine.StartsWithIgnoreCase(s3) || currentLine.StartsWithIgnoreCase(s4));
            }

            return retVal;
        }

        private static bool IsSectionEquals(string sectionName, SuiteSettingsEnum settings)
        {
            return sectionName.EqualsIgnoreCase(SuiteSettings[(int)settings]);
        }

        private static bool IsKeywordColumn(string name)
        {
            return name.StartsWithIgnoreCase("*keyword") || name.StartsWithIgnoreCase("*action") ||
                   name.EqualsIgnoreCase("keyword") || name.EqualsIgnoreCase("action");
        }

        private static string GetTagBasedOnName(string columnName)
        {
            if (!columnName.EqualsIgnoreCase("sep"))
            {
                string[] outputHint = { "output", "out", "result", "expected" };
                if (outputHint.All(hint => columnName.IndexOf(hint, StringComparison.OrdinalIgnoreCase) <= -1) &&
                    !columnName.StartsWith("="))
                {
                    return "in";
                }
                return "out";
            }

            return "ignore";
        }

        protected virtual string GetOutputFileName(string inputFileName)
        {
            return inputFileName;
        }

        protected override void OnPreProcessing()
        {
            base.OnPreProcessing();
            ResetAll();
        }

        private void ResetAll()
        {
            _currentSection = SectionEnum.Undef;
            _currentLine = _lastLine = null;
            _currentTestCase = null;
            _columnNames.Clear();
            _settings.Clear();
            _keywords.Clear();
            _testTemplate = null;
             SelectedView = null;
       }

        public override void Execute()
        {
            foreach (var inputFile in DirectoryUtilities.GetFiles(Options.Input, DirectoryUtilities.InputExtensions))
            {
                var fileName = Path.GetFileNameWithoutExtension(inputFile);
                if (fileName != null && DirectoryUtilities.IsIgnoreFile(fileName))
                {
                    // Skip initialization files
                    LogVerbose("Skipped: " + inputFile);
                    _stats.Skipped++;
                    continue;
                }

                if (DirectoryUtilities.IsHtmlFile(inputFile) && (Options.GenerateHtml == false || Options.CopyHtml == false))
                {
                    // Skip HTML files when only XML is being generated or when copying is not enabled
                    LogVerbose("Skipped: " + inputFile);
                    _stats.Skipped++;
                    continue;                    
                }
                
                LogVerbose("Processing " + inputFile + "...");

                Process(inputFile);
            }

            OnPostExecution();
        }

        protected override void OnPostProcessing()
        {
            base.OnPostProcessing();

            if (TestSuite != null)
            {
                LogVerbose("Created: " + OutputFile);
                _stats.Success++;
            }
            else
            {
                LogVerbose("Copied: " + OutputFile);
                _stats.Copied++;
            }
        }

        protected override object GetBusinessObject(StreamReader reader)
        {
            var ext = Path.GetExtension(InputFile);
            switch (ext)
            {
                case ".htm":
                case ".html":
                    return null; // No transformation needed
                case ".txt":
                case ".robot":
                    return Parse(new TxtReader(reader), InputFile);
                    break;
                case ".tsv":
                    return Parse(new TsvReader(reader), InputFile);
                    break;
                case ".xml":
                    return Deserialize(reader);
                    break;
                default:
                    throw new Exception("File extension not supported!");
            }
        }

        protected override string GetOutputFile(string inputFile)
        {
            string inputFileName = Path.GetFileNameWithoutExtension(inputFile);

            if (Directory.Exists(Options.Input))
            {
                // Since input is a directory, assume that output is a directory
                if (!Directory.Exists(Options.Output))
                {
                    Directory.CreateDirectory(Options.Output);
                }

                var targetDirectory = GetTargetDirectory(inputFile);
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }
                return Path.Combine(targetDirectory, GetOutputFileName(inputFileName));
            }
            else
            {
                // Target is a file
                return Options.Output;
            }
        }

        private string GetTargetDirectory(string sourcePath)
        {
            var sourceDir = Path.GetDirectoryName(sourcePath);
            if (sourceDir != null)
            {
                var targetDir = sourceDir.Replace(Options.Input, Options.Output);
                return targetDir;
            }
            return null;
        }

        protected abstract TestSuite Deserialize(TextReader reader);
    }
}
