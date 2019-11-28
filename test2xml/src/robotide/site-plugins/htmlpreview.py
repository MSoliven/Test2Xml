#  This needs to be in "robotide/site-plugins" folder of your RIDE installation (Usually C:\Python27\Lib\site-packages\robotide\site-plugins)
#  This is tested on RIDE 1.4 running on Python 2.7.10
#
import wx.html
from StringIO import StringIO

from robotide import robotapi
from robotide.pluginapi import Plugin, ActionInfo, TreeAwarePluginMixin
from robotide.publish import (RideTreeSelection, RideNotebookTabChanged,
                              RideTestCaseAdded, RideUserKeywordAdded, RideFileNameChanged)
from robotide.robotapi import TestCase, UserKeyword
from robotide.widgets import ButtonWithHandler, Font
from robotide.widgets import VerticalSizer, HorizontalSizer, ButtonWithHandler
from robotide.widgets import TextField, Label, HtmlDialog
from robotide.utils import Printing
from robotide.controller.commands import SaveFile
from robotide.controller.commands import SetDataFile
import subprocess
import webbrowser
import os.path
import xml.etree.ElementTree as ET

ID_SHOW_AS_XML = wx.NewId()

class HtmlPreviewPlugin(Plugin, TreeAwarePluginMixin):
    """Automatically style test suites written in ROBOT, TEXT or TSV format."""
    defaults = {"show_as_xml": False}    
    datawrapper = property(lambda self: DataFileWrapper(self.tree.get_selected_datafile_controller(), self.global_settings))

    def __init__(self, application):
        Plugin.__init__(self, application, default_settings=self.defaults)
        self._panel = None

    def enable(self):
        self.add_self_as_tree_aware_plugin()
        self.subscribe(self.OnTreeSelection, RideTreeSelection)
        self.subscribe(self.OnTabChanged, RideNotebookTabChanged)
        self.subscribe(self._update_tab_view, RideTestCaseAdded)
        self.subscribe(self._update_tab_view, RideUserKeywordAdded)
        self.subscribe(self._update_tab_view, RideFileNameChanged)
        self._open()

    def disable(self):
        self.remove_self_from_tree_aware_plugins()
        self.unsubscribe_all()
        self.unregister_actions()
        self.delete_tab(self._panel)
        self._panel = None

    def is_focused(self):
        return self.tab_is_visible(self._panel)

    def OnOpen(self, event):
        self._open()
        
    def _open(self):
        if not self._panel:
            self._panel = PreviewPanel(self, self.notebook)            
        self.show_tab(self._panel)
                
    def OnShowPreview(self, event):
        if not self._panel:
            self._panel = PreviewPanel(self, self.notebook)
        self.show_tab(self._panel)
        self._update_tab_view()

    def OnTreeSelection(self, event):
        if self.is_focused():
            self._panel.tree_node_selected(event.item)

    def OnTabChanged(self, event):
        self._update_tab_view()

    def _update_tab_view(self, event=None):
        if self.is_focused() and self.datawrapper.datafile:
            self._panel.update_tab_view()


class PreviewPanel(wx.Panel):

    def __init__(self, parent, notebook):
        self._test2xmldir = os.environ.get('TEST2XML_HOME')
        if self._test2xmldir:
            self._test2xml = os.path.join(self._test2xmldir, 'test2xml.exe')
            self._templates =  os.path.join(self._test2xmldir, 'templates')
            self._output = os.path.join(self._test2xmldir, 'output')
        wx.Panel.__init__(self, notebook)
        self._parent = parent               
        self._create_ui("HTML Preview")#
        #self._format = parent.format
        self._format = 'TXT'         
        self.__view = None
        self._printing = Printing(self)
        
    def open(self, data):
        self._data = data      

    @property
    def _pipe_separated(self):
        return 'Pipes' in self._format

    def _create_ui(self, title):
        self.SetSizer(VerticalSizer())
        self._create_editor_toolbar()
        self._parent.add_tab(self, title, allow_closing=False)
        
    def _create_editor_toolbar(self):
        self.editor_toolbar = HorizontalSizer()
        default_components = HorizontalSizer()
        self.editor_toolbar.add_expanding(default_components)
        self.Sizer.add_expanding(self.editor_toolbar, propotion=0)
        default_components.AddSpacer(20)
        self._showasxmlcb = wx.CheckBox(self, ID_SHOW_AS_XML, " Show as XML ")
        self._showasxmlcb.SetToolTip(wx.ToolTip("Check to show test suite as XML"))
        self._showasxmlcb.SetValue(self._parent.show_as_xml)
        default_components.add_with_padding(self._showasxmlcb, padding=10)           
        self._create_browser_controls(default_components)
        self._create_apply_button(default_components)
        self._showasxmlcb.Bind(wx.EVT_CHECKBOX, self.OnShowAsXmlCheckbox, self._showasxmlcb)        
    
    def _create_browser_controls(self, container_sizer):
        container_sizer.AddSpacer(20)
        self._viewLabel = Label(self, label='Select View: ')
        container_sizer.add_with_padding(self._viewLabel, padding=10)         
        self._get_available_template_views()
        self._choiceView = wx.Choice(self, wx.ID_ANY, choices=self.template_views)
        self._choiceView.SetToolTip(wx.ToolTip('Choose which view to use for generating the document'))   
        if self.template_views != []:                 
            self._choiceView.SetStringSelection(self.template_views[0])
        self._choiceView.Bind(wx.EVT_CHOICE, self.OnTemplateViewSelection, self._choiceView)
        container_sizer.add_with_padding(self._choiceView);        
        self._browse_button = ButtonWithHandler(self, 'Browse', handler=lambda e: self.browse(), width=-1, height=22)
        container_sizer.add_with_padding(self._browse_button)

    def _call_test2xml(self, args, check_output):        
        if not self._test2xmldir:
            return 'Error'
        if check_output:
            output = subprocess.check_output(self._test2xml + ' ' + args, shell=True)
            return output
        else:
            subprocess.call(self._test2xml + ' ' + args, shell=True)
            return ''            
            
    def _get_available_template_views(self):
        self.template_views = []
        if not self._test2xmldir:
            return
        args = '--views -t"' + self._templates + '"' 
        output = self._call_test2xml(args, True)        
        for view in output.split('\r\n'):
            if view != '':
                split = view.split('|')
                if len(split) > 1:
                    self.default_view = split[0]
                self.template_views.append(split[0])
                                      
    def _create_apply_button(self, container_sizer):
        container_sizer.AddSpacer(20)
        self._apply_button = ButtonWithHandler(self, 'Apply', handler=lambda e: self.apply(), width=-1, height=22) 
        container_sizer.add_with_padding(self._apply_button)
        
    def _get_output_file_path(self):
        inputfile = self._parent.datawrapper.datafile.source;
        outputfile = os.path.join(self._output, 'htmlpreview.html' );
        return outputfile         
        
    def browse(self, *args):
        if not self._test2xmldir:
            return
        inputfile = self._parent.datawrapper.datafile.source
        outputfile = self._get_output_file_path()
        template_view = self._choiceView.GetStringSelection()
        args = '--html -i"' + inputfile + '" -o"' + outputfile + '" -t"' + self._templates + '"' + ' -v"' + template_view + '"' 
        self._call_test2xml(args, False)
        webbrowser.open(outputfile, 0)  
        return True

    def apply(self, *args):
        if not self._test2xmldir:
            return        
        template_view = self._choiceView.GetStringSelection()
        vewMetadata = self._get_view_metadata()
        if vewMetadata != None:
            vewMetadata.value = template_view               
        else:
            self._parent.datawrapper.metadata.add_metadata("View", template_view) 
        self._parent.datawrapper.update_from(self._get_content(self._parent.datafile))                                 
        self.update_tab_view()
        return True
    
    def OnShowAsXmlCheckbox(self, evt):
        self._parent.save_setting("show_as_xml", evt.IsChecked())
        self.update_tab_view()
        
    def OnTemplateViewSelection(self, event):
        return True
        
    def SetTemplateView(self, view):
        items = self.template_views;
        if view not in items:
            return
        choice_index = items.index(view)
        self._choiceView.Select(choice_index)
        
    @property
    def _view(self):
        view_class = TxtView
        if isinstance(self.__view, view_class):
            return self.__view
        self._remove_current_view()
        self.__view = self._create_view(view_class)
        return self.__view

    def _remove_current_view(self):
        if self.__view:
            self.Sizer.Remove(self.__view)
            self.__view.Destroy()

    def _create_view(self, view_class):
        view = view_class(self)
        self.Sizer.Add(view, 1, wx.EXPAND|wx.ALL, border=8)
        self.Sizer.Layout()
        return view

    def tree_node_selected(self, item):
        self.update_tab_view()
        self._view.scroll_to_subitem(item)

    def _disable_controls(self, is_disable):
        if is_disable:
            self._showasxmlcb.Disable()
            self._viewLabel.Disable()
            self._choiceView.Disable()
            self._browse_button.Disable()
            self._apply_button.Disable()
        else:
            self._showasxmlcb.Enable()
            self._viewLabel.Enable()
            self._choiceView.Enable()
            self._browse_button.Enable()
            self._apply_button.Enable()
                        
    def update_tab_view(self):
        datafile = self._parent.datafile
        if not self._test2xmldir:
            self._disable_controls(True)
            content = '[ Please install TEST2XML and configure "TEST2XML_HOME" environment variable. ]'
        elif not datafile:
            self._disable_controls(True)
            content = '[ Source is not available ]'   
        elif not os.path.isfile(datafile.source):
            self._disable_controls(True)
            content = '[ Not applicable ]'
        elif not self._is_text_file(datafile.source):
            self._disable_controls(True)
            content = '[ Please convert to ROBOT, TXT or TSV format ]'     
        elif (self._parent.show_as_xml):
            self._disable_controls(False)
            content = self._get_content_xml(datafile)
        else:
            self._disable_controls(False)
            content = self._get_content(datafile)
        self._view.set_content(content.decode('utf8'))        
        vewMetadata = self._get_view_metadata()
        if vewMetadata != None:
            viewname = vewMetadata.value               
        else:
            viewname = self.default_view                          
        self.SetTemplateView(viewname)
    
    def _is_text_file(self, source):
        text_extensions = ['.robot','.txt', '.tsv', '.rst', '.rest']
        filename, extension = os.path.splitext(source)
        if extension.lower() in text_extensions:
            return True
        else:
            return False
        
    def _get_view_metadata(self):
        for item in self._parent.datawrapper.datafile.metadata._items:
            if item.name == 'View':
                return item
        return None
        
    def _get_content_xml(self, datafile):
        view = self._choiceView.GetStringSelection()     
        args = '--xml -i"' + datafile.source + '" -o stdout -t"' + self._templates + '"'            
        output = self._call_test2xml(args, True)
        return output        

    def _get_content(self, datafile):
        output = StringIO()
        try:
            datafile.save(
                output=output,
                format='txt',
                pipe_separated=self._pipe_separated,
                txt_separating_spaces=self._parent.global_settings['txt number of spaces']
            )
        except Exception, e:
            return "Creating preview of '%s' failed: %s" % (datafile.name, e)
        else:
            return output.getvalue()


class TxtView(wx.TextCtrl):

    def __init__(self, parent):
        wx.TextCtrl.__init__(self, parent, style=wx.TE_MULTILINE)
        self.SetEditable(False)
        self.SetFont(Font().fixed)

    def set_content(self, content):
        self.SetValue(content)

    def scroll_to_subitem(self, item):
        pass


class DataFileWrapper(object): # TODO: bad class name

    def __init__(self, data, settings):
        self._data = data
        self._settings = settings

    def __eq__(self, other):
        if other is None:
            return False
        return self._data == other._data

    def update_from(self, content):
        self._data.execute(SetDataFile(self._create_target_from(content)))
        self._data.execute(SaveFile())
        
    def _create_target_from(self, content):
        src = StringIO(content)
        target = self._create_target()
        FromStringIOPopulator(target).populate(src)
        return target

    def format_text(self, text):
        return self._txt_data(self._create_target_from(text))

    def mark_data_dirty(self):
        self._data.mark_dirty()

    def _create_target(self):
        data = self._data.data
        target_class = type(data)
        if isinstance(data, robotapi.TestDataDirectory):
            target = robotapi.TestDataDirectory(source=self._data.directory)
            target.initfile = data.initfile
            return target
        return target_class(source=self._data.source)

    @property
    def content(self):
        return self._txt_data(self._data.data)

    @property
    def datafile(self):
        return self._data

    @property
    def metadata(self):
        return self.datafile.metadata
    
    def _txt_data(self, data):
        output = StringIO()
        data.save(output=output, format='txt',
                  txt_separating_spaces=self._settings['txt number of spaces'])
        return output.getvalue().decode('UTF-8')


class FromStringIOPopulator(robotapi.FromFilePopulator):

    def populate(self, content):
        robotapi.TxtReader().read(content, self)
