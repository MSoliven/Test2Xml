# Test2Xml
Automatic styling of Robot Framework test suites

TEST2XML is a tool for automatic styling of high-level Robot Framework test suites. The program is written in C# but the binaries should also run in Linux (requires Mono). The tool can be integrated with RIDE via the accompanying HTML Preview plugin and can be easily invoked as a Jenkins job (see screenshot below). It also has the built-in capability to generate an index page for easier browsing of the generated HTML files.

See Step-by-step guide below for deployment instructions.


General Usage:
test2xml --help

test2xml --views (list all available views)

test2xml --xml -i"Input\LoginFeature.txt" -o stdout  (display xml to standard output for troubleshooting purposes)

Generate html files from the Input folder to the Output folder using the "Tabular wide" view:
test2xml --html -i"Input" -o"Output" -t"Templates" -v"Tabular wide"

Generate table of contents file for the Output folder:
test2xml --toc -o"Output" (index.html file will be created in the output folder)

Step-by-step guide
Please follow the following instructions to integrate with RIDE (Robot Framework IDE):

1. Create "TEST2XML_HOME" system environment variable that points to the folder you created in the previous step:

2. Copy the python script (i.e. HTMLPreview.py) in "robotide\site-plugins" to the corresponding folder of your RIDE installation (Usually C:\Python27\Lib\site-packages \robotide\site-plugins).

3. You should see the plugin as soon as you restart RIDE.

Supported Input files are .TXT, .TSV and .ROBOT

Supported template files: XSN (Microsoft InfoPath), XSL 1.0

Plugin was fully tested in RIDE 1.4 and 1.5.1 running on Python 2.7.10

Should be compatible with Microsoft InfoPath 2003-2013

Test2Xml is Mono compatible (requires cabextract for Linux, see http://www.cabextract.org.uk/)
