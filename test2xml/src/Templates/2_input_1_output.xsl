<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:my="http://schemas.microsoft.com/office/infopath/2003/myXSD/2015-07-31T20:00:17" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:ns1="http://www.iontrading.com/RFML/v1.0" xmlns:codegen="urn:schemas-microsoft-com:xml-msprop" xmlns:xd="http://schemas.microsoft.com/office/infopath/2003" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns:xdExtension="http://schemas.microsoft.com/office/infopath/2003/xslt/extension" xmlns:xdXDocument="http://schemas.microsoft.com/office/infopath/2003/xslt/xDocument" xmlns:xdSolution="http://schemas.microsoft.com/office/infopath/2003/xslt/solution" xmlns:xdFormatting="http://schemas.microsoft.com/office/infopath/2003/xslt/formatting" xmlns:xdImage="http://schemas.microsoft.com/office/infopath/2003/xslt/xImage" xmlns:xdUtil="http://schemas.microsoft.com/office/infopath/2003/xslt/Util" xmlns:xdMath="http://schemas.microsoft.com/office/infopath/2003/xslt/Math" xmlns:xdDate="http://schemas.microsoft.com/office/infopath/2003/xslt/Date" xmlns:sig="http://www.w3.org/2000/09/xmldsig#" xmlns:xdSignatureProperties="http://schemas.microsoft.com/office/infopath/2003/SignatureProperties" xmlns:ipApp="http://schemas.microsoft.com/office/infopath/2006/XPathExtension/ipApp" xmlns:xdEnvironment="http://schemas.microsoft.com/office/infopath/2006/xslt/environment" xmlns:xdUser="http://schemas.microsoft.com/office/infopath/2006/xslt/User" xmlns:xdServerInfo="http://schemas.microsoft.com/office/infopath/2009/xslt/ServerInfo">
	<xsl:output method="html" indent="no"/>
	<xsl:template match="ns1:testSuite">
		<html>
			<head>
				<meta content="text/html" http-equiv="Content-Type"></meta>
				<style controlStyle="controlStyle">@media screen 			{ 			BODY{margin-left:21px;background-position:21px 0px;} 			} 		BODY{color:windowtext;background-color:window;layout-grid:none;} 		.xdListItem {display:inline-block;width:100%;vertical-align:text-top;} 		.xdListBox,.xdComboBox{margin:1px;} 		.xdInlinePicture{margin:1px; BEHAVIOR: url(#default#urn::xdPicture) } 		.xdLinkedPicture{margin:1px; BEHAVIOR: url(#default#urn::xdPicture) url(#default#urn::controls/Binder) } 		.xdHyperlinkBox{word-wrap:break-word; text-overflow:ellipsis;overflow-x:hidden; OVERFLOW-Y: hidden; WHITE-SPACE:nowrap; display:inline-block;margin:1px;padding:5px;border: 1pt solid #dcdcdc;color:windowtext;BEHAVIOR: url(#default#urn::controls/Binder) url(#default#DataBindingUI)} 		.xdSection{border:1pt solid transparent ;margin:0px 0px 0px 0px;padding:0px 0px 0px 0px;} 		.xdRepeatingSection{border:1pt solid transparent;margin:0px 0px 0px 0px;padding:0px 0px 0px 0px;} 		.xdMultiSelectList{margin:1px;display:inline-block; border:1pt solid #dcdcdc; padding:1px 1px 1px 5px; text-indent:0; color:windowtext; background-color:window; overflow:auto; behavior: url(#default#DataBindingUI) url(#default#urn::controls/Binder) url(#default#MultiSelectHelper) url(#default#ScrollableRegion);} 		.xdMultiSelectListItem{display:block;white-space:nowrap}		.xdMultiSelectFillIn{display:inline-block;white-space:nowrap;text-overflow:ellipsis;;padding:1px;margin:1px;border: 1pt solid #dcdcdc;overflow:hidden;text-align:left;}		.xdBehavior_Formatting {BEHAVIOR: url(#default#urn::controls/Binder) url(#default#Formatting);} 	 .xdBehavior_FormattingNoBUI{BEHAVIOR: url(#default#CalPopup) url(#default#urn::controls/Binder) url(#default#Formatting);} 	.xdExpressionBox{margin: 1px;padding:1px;word-wrap: break-word;text-overflow: ellipsis;overflow-x:hidden;}.xdBehavior_GhostedText,.xdBehavior_GhostedTextNoBUI{BEHAVIOR: url(#default#urn::controls/Binder) url(#default#TextField) url(#default#GhostedText);}	.xdBehavior_GTFormatting{BEHAVIOR: url(#default#urn::controls/Binder) url(#default#Formatting) url(#default#GhostedText);}	.xdBehavior_GTFormattingNoBUI{BEHAVIOR: url(#default#CalPopup) url(#default#urn::controls/Binder) url(#default#Formatting) url(#default#GhostedText);}	.xdBehavior_Boolean{BEHAVIOR: url(#default#urn::controls/Binder) url(#default#BooleanHelper);}	.xdBehavior_Select{BEHAVIOR: url(#default#urn::controls/Binder) url(#default#SelectHelper);}	.xdBehavior_ComboBox{BEHAVIOR: url(#default#ComboBox)} 	.xdBehavior_ComboBoxTextField{BEHAVIOR: url(#default#ComboBoxTextField);} 	.xdRepeatingTable{BORDER-TOP-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none; BORDER-BOTTOM-STYLE: none; BORDER-COLLAPSE: collapse; WORD-WRAP: break-word;}.xdScrollableRegion{BEHAVIOR: url(#default#ScrollableRegion);} 		.xdLayoutRegion{display:inline-block;} 		.xdMaster{BEHAVIOR: url(#default#MasterHelper);} 		.xdActiveX{margin:1px; BEHAVIOR: url(#default#ActiveX);} 		.xdFileAttachment{display:inline-block;margin:1px;BEHAVIOR:url(#default#urn::xdFileAttachment);} 		.xdSharePointFileAttachment{display:inline-block;margin:2px;BEHAVIOR:url(#default#xdSharePointFileAttachment);} 		.xdAttachItem{display:inline-block;width:100%%;height:25px;margin:1px;BEHAVIOR:url(#default#xdSharePointFileAttachItem);} 		.xdSignatureLine{display:inline-block;margin:1px;background-color:transparent;border:1pt solid transparent;BEHAVIOR:url(#default#SignatureLine);} 		.xdHyperlinkBoxClickable{behavior: url(#default#HyperlinkBox)} 		.xdHyperlinkBoxButtonClickable{border-width:1px;border-style:outset;behavior: url(#default#HyperlinkBoxButton)} 		.xdPictureButton{background-color: transparent; padding: 0px; behavior: url(#default#PictureButton);} 		.xdPageBreak{display: none;}BODY{margin-right:21px;} 		.xdTextBoxRTL{display:inline-block;white-space:nowrap;text-overflow:ellipsis;;padding:1px;margin:1px;border: 1pt solid #dcdcdc;color:windowtext;background-color:window;overflow:hidden;text-align:right;word-wrap:normal;} 		.xdRichTextBoxRTL{display:inline-block;;padding:1px;margin:1px;border: 1pt solid #dcdcdc;color:windowtext;background-color:window;overflow-x:hidden;word-wrap:break-word;text-overflow:ellipsis;text-align:right;font-weight:normal;font-style:normal;text-decoration:none;vertical-align:baseline;} 		.xdDTTextRTL{height:100%;width:100%;margin-left:22px;overflow:hidden;padding:0px;white-space:nowrap;} 		.xdDTButtonRTL{margin-right:-21px;height:17px;width:20px;behavior: url(#default#DTPicker);} 		.xdMultiSelectFillinRTL{display:inline-block;white-space:nowrap;text-overflow:ellipsis;;padding:1px;margin:1px;border: 1pt solid #dcdcdc;overflow:hidden;text-align:right;}.xdTextBox{display:inline-block;white-space:nowrap;text-overflow:ellipsis;;padding:1px;margin:1px;border: 1pt solid #dcdcdc;color:windowtext;background-color:window;overflow:hidden;text-align:left;word-wrap:normal;} 		.xdRichTextBox{display:inline-block;;padding:1px;margin:1px;border: 1pt solid #dcdcdc;color:windowtext;background-color:window;overflow-x:hidden;word-wrap:break-word;text-overflow:ellipsis;text-align:left;font-weight:normal;font-style:normal;text-decoration:none;vertical-align:baseline;} 		.xdDTPicker{;display:inline;margin:1px;margin-bottom: 2px;border: 1pt solid #dcdcdc;color:windowtext;background-color:window;overflow:hidden;text-indent:0; layout-grid: none} 		.xdDTText{height:100%;width:100%;margin-right:22px;overflow:hidden;padding:0px;white-space:nowrap;} 		.xdDTButton{margin-left:-21px;height:17px;width:20px;behavior: url(#default#DTPicker);} 		.xdRepeatingTable TD {VERTICAL-ALIGN: top;}</style>
				<style tableEditor="TableStyleRulesID">TABLE.xdLayout TD {
	BORDER-TOP: medium none; BORDER-RIGHT: medium none; BORDER-BOTTOM: medium none; BORDER-LEFT: medium none
}
TABLE.msoUcTable TD {
	BORDER-TOP: 1pt solid; BORDER-RIGHT: 1pt solid; BORDER-BOTTOM: 1pt solid; BORDER-LEFT: 1pt solid
}
TABLE {
	BEHAVIOR: url (#default#urn::tables/NDTable)
}
</style>
				<style languageStyle="languageStyle">BODY {
	FONT-SIZE: 10pt; FONT-FAMILY: Calibri
}
SELECT {
	FONT-SIZE: 10pt; FONT-FAMILY: Calibri
}
TABLE {
	FONT-SIZE: 10pt; FONT-FAMILY: Calibri; TEXT-TRANSFORM: none; FONT-WEIGHT: normal; COLOR: black; FONT-STYLE: normal
}
.optionalPlaceholder {
	FONT-SIZE: 9pt; TEXT-DECORATION: none; FONT-FAMILY: Calibri; FONT-WEIGHT: normal; COLOR: #333333; FONT-STYLE: normal; PADDING-LEFT: 20px; BEHAVIOR: url(#default#xOptional)
}
.langFont {
	FONT-SIZE: 10pt; FONT-FAMILY: Calibri; WIDTH: 150px
}
.defaultInDocUI {
	FONT-SIZE: 9pt; FONT-FAMILY: Calibri
}
.optionalPlaceholder {
	PADDING-RIGHT: 20px
}
</style>
				<style themeStyle="urn:office.microsoft.com:themeSummer">TABLE {
	BORDER-TOP: medium none; BORDER-RIGHT: medium none; BORDER-COLLAPSE: collapse; BORDER-BOTTOM: medium none; BORDER-LEFT: medium none
}
TD {
	BORDER-TOP-COLOR: #d8d8d8; BORDER-LEFT-COLOR: #d8d8d8; BORDER-BOTTOM-COLOR: #d8d8d8; BORDER-RIGHT-COLOR: #d8d8d8
}
TH {
	BORDER-TOP-COLOR: #000000; BORDER-LEFT-COLOR: #000000; COLOR: black; BORDER-BOTTOM-COLOR: #000000; BORDER-RIGHT-COLOR: #000000; BACKGROUND-COLOR: #f2f2f2
}
.xdTableHeader {
	COLOR: black; BACKGROUND-COLOR: #f2f2f2
}
.light1 {
	BACKGROUND-COLOR: #ffffff
}
.dark1 {
	BACKGROUND-COLOR: #000000
}
.light2 {
	BACKGROUND-COLOR: #f7f8f4
}
.dark2 {
	BACKGROUND-COLOR: #2b4b4d
}
.accent1 {
	BACKGROUND-COLOR: #6c9a7f
}
.accent2 {
	BACKGROUND-COLOR: #bb523d
}
.accent3 {
	BACKGROUND-COLOR: #c89d11
}
.accent4 {
	BACKGROUND-COLOR: #fccf10
}
.accent5 {
	BACKGROUND-COLOR: #568ea1
}
.accent6 {
	BACKGROUND-COLOR: #decf28
}
</style>
				<style tableStyle="Professional">TR.xdTitleRow {
	MIN-HEIGHT: 83px
}
TD.xdTitleCell {
	BORDER-TOP: #bfbfbf 1pt solid; BORDER-RIGHT: #bfbfbf 1pt solid; PADDING-BOTTOM: 14px; TEXT-ALIGN: center; PADDING-TOP: 32px; PADDING-LEFT: 22px; BORDER-LEFT: #bfbfbf 1pt solid; PADDING-RIGHT: 22px; BACKGROUND-COLOR: #ffffff; valign: bottom
}
TR.xdTitleRowWithHeading {
	MIN-HEIGHT: 69px
}
TD.xdTitleCellWithHeading {
	BORDER-TOP: #bfbfbf 1pt solid; BORDER-RIGHT: #bfbfbf 1pt solid; PADDING-BOTTOM: 0px; TEXT-ALIGN: center; PADDING-TOP: 32px; PADDING-LEFT: 22px; BORDER-LEFT: #bfbfbf 1pt solid; PADDING-RIGHT: 22px; BACKGROUND-COLOR: #ffffff; valign: bottom
}
TR.xdTitleRowWithSubHeading {
	MIN-HEIGHT: 75px
}
TD.xdTitleCellWithSubHeading {
	BORDER-TOP: #bfbfbf 1pt solid; BORDER-RIGHT: #bfbfbf 1pt solid; PADDING-BOTTOM: 6px; TEXT-ALIGN: center; PADDING-TOP: 32px; PADDING-LEFT: 22px; BORDER-LEFT: #bfbfbf 1pt solid; PADDING-RIGHT: 22px; BACKGROUND-COLOR: #ffffff; valign: bottom
}
TR.xdTitleRowWithOffsetBody {
	MIN-HEIGHT: 72px
}
TD.xdTitleCellWithOffsetBody {
	BORDER-TOP: #bfbfbf 1pt solid; BORDER-RIGHT: #bfbfbf 1pt solid; PADDING-BOTTOM: 2px; TEXT-ALIGN: left; PADDING-TOP: 32px; PADDING-LEFT: 22px; BORDER-LEFT: #bfbfbf 1pt solid; PADDING-RIGHT: 22px; BACKGROUND-COLOR: #ffffff; valign: bottom
}
TR.xdTitleHeadingRow {
	MIN-HEIGHT: 37px
}
TD.xdTitleHeadingCell {
	BORDER-RIGHT: #bfbfbf 1pt solid; PADDING-BOTTOM: 14px; TEXT-ALIGN: center; PADDING-TOP: 0px; PADDING-LEFT: 22px; BORDER-LEFT: #bfbfbf 1pt solid; PADDING-RIGHT: 22px; BACKGROUND-COLOR: #ffffff; valign: top
}
TR.xdTitleSubheadingRow {
	MIN-HEIGHT: 70px
}
TD.xdTitleSubheadingCell {
	BORDER-RIGHT: #bfbfbf 1pt solid; PADDING-BOTTOM: 16px; PADDING-TOP: 8px; PADDING-LEFT: 22px; BORDER-LEFT: #bfbfbf 1pt solid; PADDING-RIGHT: 22px; BACKGROUND-COLOR: #ffffff; valign: top
}
TD.xdVerticalFill {
	BORDER-TOP: #bfbfbf 1pt solid; BORDER-BOTTOM: #bfbfbf 1pt solid; BORDER-LEFT: #bfbfbf 1pt solid; BACKGROUND-COLOR: #354d3f
}
TD.xdTableContentCellWithVerticalOffset {
	BORDER-RIGHT: #bfbfbf 1pt solid; BORDER-BOTTOM: #bfbfbf 1pt solid; PADDING-BOTTOM: 2px; TEXT-ALIGN: left; PADDING-TOP: 32px; PADDING-LEFT: 95px; BORDER-LEFT: #bfbfbf 1pt solid; PADDING-RIGHT: 0px; BACKGROUND-COLOR: #ffffff; valign: bottom
}
TR.xdTableContentRow {
	MIN-HEIGHT: 140px
}
TD.xdTableContentCell {
	BORDER-RIGHT: #bfbfbf 1pt solid; BORDER-BOTTOM: #bfbfbf 1pt solid; PADDING-BOTTOM: 0px; PADDING-TOP: 0px; PADDING-LEFT: 0px; BORDER-LEFT: #bfbfbf 1pt solid; PADDING-RIGHT: 0px; BACKGROUND-COLOR: #ffffff; valign: top
}
TD.xdTableContentCellWithVerticalFill {
	BORDER-RIGHT: #bfbfbf 1pt solid; BORDER-BOTTOM: #bfbfbf 1pt solid; PADDING-BOTTOM: 0px; PADDING-TOP: 0px; PADDING-LEFT: 1px; BORDER-LEFT: #bfbfbf 1pt solid; PADDING-RIGHT: 1px; BACKGROUND-COLOR: #ffffff; valign: top
}
TD.xdTableStyleOneCol {
	PADDING-BOTTOM: 4px; PADDING-TOP: 4px; PADDING-LEFT: 22px; PADDING-RIGHT: 22px
}
TR.xdContentRowOneCol {
	MIN-HEIGHT: 45px; valign: center
}
TR.xdHeadingRow {
	MIN-HEIGHT: 27px
}
TD.xdHeadingCell {
	BORDER-TOP: #a6c2b2 1pt solid; BORDER-BOTTOM: #a6c2b2 1pt solid; PADDING-BOTTOM: 2px; TEXT-ALIGN: center; PADDING-TOP: 2px; PADDING-LEFT: 22px; PADDING-RIGHT: 22px; BACKGROUND-COLOR: #e1eae5; valign: bottom
}
TR.xdSubheadingRow {
	MIN-HEIGHT: 28px
}
TD.xdSubheadingCell {
	BORDER-BOTTOM: #a6c2b2 1pt solid; PADDING-BOTTOM: 4px; TEXT-ALIGN: center; PADDING-TOP: 4px; PADDING-LEFT: 22px; PADDING-RIGHT: 22px; valign: bottom
}
TR.xdHeadingRowEmphasis {
	MIN-HEIGHT: 27px
}
TD.xdHeadingCellEmphasis {
	BORDER-TOP: #a6c2b2 1pt solid; BORDER-BOTTOM: #a6c2b2 1pt solid; PADDING-BOTTOM: 2px; TEXT-ALIGN: center; PADDING-TOP: 2px; PADDING-LEFT: 22px; PADDING-RIGHT: 22px; BACKGROUND-COLOR: #e1eae5; valign: bottom
}
TR.xdSubheadingRowEmphasis {
	MIN-HEIGHT: 28px
}
TD.xdSubheadingCellEmphasis {
	BORDER-BOTTOM: #a6c2b2 1pt solid; PADDING-BOTTOM: 4px; TEXT-ALIGN: center; PADDING-TOP: 4px; PADDING-LEFT: 22px; PADDING-RIGHT: 22px; valign: bottom
}
TR.xdTableLabelControlStackedRow {
	MIN-HEIGHT: 45px
}
TD.xdTableLabelControlStackedCellLabel {
	PADDING-BOTTOM: 4px; PADDING-TOP: 4px; PADDING-LEFT: 22px; PADDING-RIGHT: 5px
}
TD.xdTableLabelControlStackedCellComponent {
	PADDING-BOTTOM: 4px; PADDING-TOP: 4px; PADDING-LEFT: 5px; PADDING-RIGHT: 22px
}
TR.xdTableRow {
	MIN-HEIGHT: 30px
}
TD.xdTableCellLabel {
	PADDING-BOTTOM: 4px; PADDING-TOP: 4px; PADDING-LEFT: 22px; PADDING-RIGHT: 5px
}
TD.xdTableCellComponent {
	PADDING-BOTTOM: 4px; PADDING-TOP: 4px; PADDING-LEFT: 5px; PADDING-RIGHT: 22px
}
TD.xdTableMiddleCell {
	PADDING-BOTTOM: 4px; PADDING-TOP: 4px; PADDING-LEFT: 5px; PADDING-RIGHT: 5px
}
TR.xdTableEmphasisRow {
	MIN-HEIGHT: 30px
}
TD.xdTableEmphasisCellLabel {
	PADDING-BOTTOM: 4px; PADDING-TOP: 4px; PADDING-LEFT: 22px; PADDING-RIGHT: 5px; BACKGROUND-COLOR: #c4d6cb
}
TD.xdTableEmphasisCellComponent {
	PADDING-BOTTOM: 4px; PADDING-TOP: 4px; PADDING-LEFT: 5px; PADDING-RIGHT: 22px; BACKGROUND-COLOR: #c4d6cb
}
TD.xdTableMiddleCellEmphasis {
	PADDING-BOTTOM: 4px; PADDING-TOP: 4px; PADDING-LEFT: 5px; PADDING-RIGHT: 5px; BACKGROUND-COLOR: #c4d6cb
}
TR.xdTableOffsetRow {
	MIN-HEIGHT: 30px
}
TD.xdTableOffsetCellLabel {
	PADDING-BOTTOM: 4px; PADDING-TOP: 4px; PADDING-LEFT: 22px; PADDING-RIGHT: 5px; BACKGROUND-COLOR: #c4d6cb
}
TD.xdTableOffsetCellComponent {
	PADDING-BOTTOM: 4px; PADDING-TOP: 4px; PADDING-LEFT: 5px; PADDING-RIGHT: 22px; BACKGROUND-COLOR: #c4d6cb
}
P {
	FONT-SIZE: 11pt; COLOR: #354d3f; MARGIN-TOP: 0px
}
H1 {
	MARGIN-BOTTOM: 0px; FONT-SIZE: 24pt; FONT-WEIGHT: normal; COLOR: #354d3f; MARGIN-TOP: 0px
}
H2 {
	MARGIN-BOTTOM: 0px; FONT-SIZE: 16pt; FONT-WEIGHT: bold; COLOR: #354d3f; MARGIN-TOP: 0px
}
H3 {
	MARGIN-BOTTOM: 0px; FONT-SIZE: 12pt; TEXT-TRANSFORM: uppercase; FONT-WEIGHT: normal; COLOR: #354d3f; MARGIN-TOP: 0px
}
H4 {
	MARGIN-BOTTOM: 0px; FONT-SIZE: 10pt; FONT-WEIGHT: normal; COLOR: #262626; FONT-STYLE: italic; MARGIN-TOP: 0px
}
H5 {
	MARGIN-BOTTOM: 0px; FONT-SIZE: 10pt; FONT-WEIGHT: bold; COLOR: #354d3f; FONT-STYLE: italic; MARGIN-TOP: 0px
}
H6 {
	MARGIN-BOTTOM: 0px; FONT-SIZE: 10pt; FONT-WEIGHT: normal; COLOR: #262626; MARGIN-TOP: 0px
}
BODY {
	COLOR: black
}
</style>
			</head>
			<body scroll="auto">
				<div>
					<div>
						<img style="HEIGHT: 109px; WIDTH: 130px" src="file:///C:/test2xml/Output/ion_logo.png" width="130" height="109" Linked="true"/>
					</div>
					<div>
						<table class="xdLayout" style="WORD-WRAP: break-word; BORDER-TOP: medium none; BORDER-RIGHT: medium none; WIDTH: 16050px; BORDER-COLLAPSE: collapse; TABLE-LAYOUT: fixed; BORDER-BOTTOM: medium none; BORDER-LEFT: medium none" borderColor="buttontext" border="1">
							<colgroup>
								<col style="WIDTH: 250px"></col>
								<col style="WIDTH: 1173px"></col>
								<col style="WIDTH: 14627px"></col>
							</colgroup>
							<tbody vAlign="top">
								<tr style="MIN-HEIGHT: 96px">
									<td style="BORDER-TOP: #000000 1pt; BORDER-RIGHT: #000000 1pt; VERTICAL-ALIGN: bottom; BORDER-BOTTOM: #002060 1pt; PADDING-BOTTOM: 1px; PADDING-TOP: 1px; PADDING-LEFT: 1px; BORDER-LEFT: #000000 1pt; PADDING-RIGHT: 1px; BACKGROUND-COLOR: #dde6f5">
										<div>
											<span class="xdlabel"></span> </div>
									</td>
									<td style="BORDER-TOP: #000000 1pt; BORDER-RIGHT: #000000 1pt; VERTICAL-ALIGN: middle; BORDER-BOTTOM: #002060 1pt; PADDING-BOTTOM: 20px; PADDING-TOP: 1px; PADDING-LEFT: 1px; BORDER-LEFT: #000000 1pt; PADDING-RIGHT: 1px; BACKGROUND-COLOR: #dde6f5">
										<div>
											<font size="6" face="Arial Narrow"></font> </div>
										<h1 style="FONT-WEIGHT: normal"><span title="" class="xdTextBox" hideFocus="1" tabIndex="0" xd:xctname="PlainText" xd:CtrlId="CTRL1" xd:binding="ns1:suiteName" style="BORDER-TOP: #000000 1pt; FONT-FAMILY: Arial Narrow; BORDER-RIGHT: #000000 1pt; BORDER-BOTTOM: #000000 1pt; FONT-WEIGHT: normal; BORDER-LEFT: #000000 1pt; BACKGROUND-COLOR: #dde6f5">
												<xsl:value-of select="ns1:suiteName"/>
											</span>
										</h1>
										<div>
											<font color="#ba6400" size="6" face="Arial Narrow">Test Suite</font>
										</div>
									</td>
									<td style="BORDER-TOP: #000000 1pt; BORDER-RIGHT: #000000 1pt; VERTICAL-ALIGN: bottom; BORDER-BOTTOM: #002060 1pt; PADDING-BOTTOM: 1px; PADDING-TOP: 1px; PADDING-LEFT: 1px; BORDER-LEFT: #000000 1pt; PADDING-RIGHT: 1px; BACKGROUND-COLOR: #dde6f5">
										<div align="right">
											<font size="2" face="Calibri"></font> </div>
									</td>
								</tr>
								<tr>
									<td style="BORDER-TOP: #002060 1pt; BORDER-BOTTOM-COLOR: ">
										<div>
											<span class="xdlabel"></span> </div>
									</td>
									<td style="BORDER-TOP: #002060 1pt; BORDER-BOTTOM-COLOR: ">
										<div><xsl:apply-templates select="ns1:setting" mode="_1"/>
										</div>
									</td>
									<td style="BORDER-TOP: #002060 1pt; BORDER-BOTTOM-COLOR: ">
										<font size="2" face="Calibri">
											<div align="left"> </div>
										</font>
									</td>
								</tr>
								<tr>
									<td style="BORDER-TOP-COLOR: ; BORDER-BOTTOM-COLOR: ">
										<div>
											<span class="xdlabel"></span> </div>
									</td>
									<td style="BORDER-TOP-COLOR: ; BORDER-BOTTOM-COLOR: ">
										<div>
											<font color="#002060" size="4" face="Arial">
												<strong>Test Library References</strong>
											</font>
										</div>
										<div>
											<strong>
												<font size="4" face="Arial"></font>
											</strong> </div>
									</td>
									<td style="BORDER-TOP-COLOR: ; BORDER-BOTTOM-COLOR: ">
										<font size="2" face="Calibri">
											<div align="left"> </div>
										</font>
									</td>
								</tr>
								<tr>
									<td style="BORDER-TOP-COLOR: ">
										<div>
											<span class="xdlabel"></span> </div>
									</td>
									<td style="BORDER-TOP-COLOR: ">
										<div>
											<table title="" class="xdRepeatingTable msoUcTable" style="BORDER-TOP-STYLE: none; WORD-WRAP: break-word; WIDTH: 1170px; BORDER-COLLAPSE: collapse; TABLE-LAYOUT: fixed; BORDER-BOTTOM-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none" border="1" xd:CtrlId="CTRL168" xd:widgetIndex="0">
												<colgroup>
													<col style="WIDTH: 409px"></col>
													<col style="WIDTH: 761px"></col>
												</colgroup>
												<tbody class="xdTableHeader">
													<tr>
														<td style="BORDER-TOP: #d8d8d8 1pt solid; BORDER-RIGHT: #d8d8d8 1pt solid; BORDER-BOTTOM: #d8d8d8 1pt solid; PADDING-BOTTOM: 1px; TEXT-ALIGN: center; PADDING-TOP: 1px; PADDING-LEFT: 5px; BORDER-LEFT: #d8d8d8 1pt solid; PADDING-RIGHT: 5px">
															<div>
																<h5 style="FONT-WEIGHT: normal">
																	<strong>Setting</strong>
																</h5>
															</div>
														</td>
														<td style="BORDER-TOP: #d8d8d8 1pt solid; BORDER-RIGHT: #d8d8d8 1pt solid; BORDER-BOTTOM: #d8d8d8 1pt solid; PADDING-BOTTOM: 1px; TEXT-ALIGN: center; PADDING-TOP: 1px; PADDING-LEFT: 5px; BORDER-LEFT: #d8d8d8 1pt solid; PADDING-RIGHT: 5px">
															<div>
																<h5 style="FONT-WEIGHT: normal">
																	<strong>Argument</strong>
																</h5>
															</div>
														</td>
													</tr>
												</tbody><tbody xd:xctname="RepeatingTable">
													<xsl:for-each select="ns1:setting">
														<xsl:if test="not((ns1:name = &quot;Documentation&quot;))">
															<tr xd:caption_0="Dont show documentation">
																<td style="BORDER-TOP: #d8d8d8 1pt solid; BORDER-RIGHT: #d8d8d8 1pt solid; BORDER-BOTTOM: #d8d8d8 1pt solid; PADDING-BOTTOM: 1px; PADDING-TOP: 1px; PADDING-LEFT: 5px; BORDER-LEFT: #d8d8d8 1pt solid; PADDING-RIGHT: 5px"><span title="" class="xdTextBox" hideFocus="1" tabIndex="0" xd:xctname="PlainText" xd:CtrlId="CTRL170" xd:binding="ns1:name" style="FONT-SIZE: medium; BORDER-TOP: #dcdcdc 1pt; FONT-FAMILY: Arial Narrow; BORDER-RIGHT: #dcdcdc 1pt; WIDTH: 100%; BORDER-BOTTOM: #dcdcdc 1pt; BORDER-LEFT: #dcdcdc 1pt">
																		<xsl:value-of select="ns1:name"/>
																	</span>
																</td>
																<td style="BORDER-TOP: #d8d8d8 1pt solid; BORDER-RIGHT: #d8d8d8 1pt solid; BORDER-BOTTOM: #d8d8d8 1pt solid; PADDING-BOTTOM: 1px; PADDING-TOP: 1px; PADDING-LEFT: 5px; BORDER-LEFT: #d8d8d8 1pt solid; PADDING-RIGHT: 5px"><span title="" class="xdTextBox" hideFocus="1" tabIndex="0" xd:xctname="PlainText" xd:CtrlId="CTRL174" xd:binding="ns1:value[1]" style="FONT-SIZE: medium; BORDER-TOP: #dcdcdc 1pt; FONT-FAMILY: Arial Narrow; BORDER-RIGHT: #dcdcdc 1pt; WIDTH: 100%; BORDER-BOTTOM: #dcdcdc 1pt; BORDER-LEFT: #dcdcdc 1pt">
																		<xsl:value-of select="ns1:value[1]"/>
																	</span>
																</td>
															</tr>
														</xsl:if>
													</xsl:for-each>
												</tbody>
											</table>
										</div>
									</td>
									<td style="BORDER-TOP-COLOR: ">
										<font size="2" face="Calibri">
											<div align="left"> </div>
										</font>
									</td>
								</tr>
								<tr>
									<td style="BORDER-TOP-COLOR: ">
										<div>
											<span class="xdlabel"></span> </div>
									</td>
									<td style="BORDER-TOP-COLOR: ">
										<div>
											<font color="#002060" size="4" face="Arial">
												<strong/>
											</font> </div>
										<div>
											<font color="#002060" size="4" face="Arial">
												<strong>Test Variables</strong>
											</font>
										</div>
										<div>
											<strong>
												<font size="4" face="Arial"></font>
											</strong> </div>
										<div>
											<font face="Arial">Test variable values defined below are the default values, tests may override variable values as per requirement.</font>
										</div>
									</td>
									<td style="BORDER-TOP-COLOR: ">
										<font face="Calibri">
											<div align="left"> </div>
										</font>
									</td>
								</tr>
								<tr>
									<td style="BORDER-TOP-COLOR: ">
										<div>
											<span class="xdlabel"></span> </div>
									</td>
									<td style="BORDER-TOP-COLOR: ">
										<div>
											<table title="" class="xdRepeatingTable msoUcTable" style="BORDER-TOP-STYLE: none; WORD-WRAP: break-word; WIDTH: 1170px; BORDER-COLLAPSE: collapse; TABLE-LAYOUT: fixed; BORDER-BOTTOM-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none" border="1" xd:CtrlId="CTRL183" xd:widgetIndex="0">
												<colgroup>
													<col style="WIDTH: 408px"></col>
													<col style="WIDTH: 762px"></col>
												</colgroup>
												<tbody class="xdTableHeader">
													<tr>
														<td style="BORDER-TOP: #d8d8d8 1pt solid; BORDER-RIGHT: #d8d8d8 1pt solid; BORDER-BOTTOM: #d8d8d8 1pt solid; PADDING-BOTTOM: 1px; TEXT-ALIGN: center; PADDING-TOP: 1px; PADDING-LEFT: 5px; BORDER-LEFT: #d8d8d8 1pt solid; PADDING-RIGHT: 5px">
															<div>
																<h5 style="FONT-WEIGHT: normal">
																	<strong>Variable</strong>
																</h5>
															</div>
														</td>
														<td style="BORDER-TOP: #d8d8d8 1pt solid; BORDER-RIGHT: #d8d8d8 1pt solid; BORDER-BOTTOM: #d8d8d8 1pt solid; PADDING-BOTTOM: 1px; TEXT-ALIGN: center; PADDING-TOP: 1px; PADDING-LEFT: 5px; BORDER-LEFT: #d8d8d8 1pt solid; PADDING-RIGHT: 5px">
															<div>
																<h5 style="FONT-WEIGHT: normal">
																	<strong>Value</strong>
																</h5>
															</div>
														</td>
													</tr>
												</tbody><tbody xd:xctname="RepeatingTable">
													<xsl:for-each select="ns1:variable">
														<tr>
															<td style="BORDER-TOP: #d8d8d8 1pt solid; BORDER-RIGHT: #d8d8d8 1pt solid; BORDER-BOTTOM: #d8d8d8 1pt solid; PADDING-BOTTOM: 1px; PADDING-TOP: 1px; PADDING-LEFT: 5px; BORDER-LEFT: #d8d8d8 1pt solid; PADDING-RIGHT: 5px"><span title="" class="xdTextBox" hideFocus="1" tabIndex="0" xd:xctname="PlainText" xd:CtrlId="CTRL185" xd:binding="ns1:name" style="FONT-SIZE: medium; BORDER-TOP: #dcdcdc 1pt; FONT-FAMILY: Arial Narrow; BORDER-RIGHT: #dcdcdc 1pt; WIDTH: 100%; BORDER-BOTTOM: #dcdcdc 1pt; BORDER-LEFT: #dcdcdc 1pt">
																	<xsl:value-of select="ns1:name"/>
																</span>
															</td>
															<td style="BORDER-TOP: #d8d8d8 1pt solid; BORDER-RIGHT: #d8d8d8 1pt solid; BORDER-BOTTOM: #d8d8d8 1pt solid; PADDING-BOTTOM: 1px; PADDING-TOP: 1px; PADDING-LEFT: 5px; BORDER-LEFT: #d8d8d8 1pt solid; PADDING-RIGHT: 5px"><span title="" class="xdTextBox" hideFocus="1" tabIndex="0" xd:xctname="PlainText" xd:CtrlId="CTRL188" xd:binding="ns1:value[1]" style="FONT-SIZE: medium; BORDER-TOP: #dcdcdc 1pt; FONT-FAMILY: Arial Narrow; BORDER-RIGHT: #dcdcdc 1pt; WIDTH: 100%; BORDER-BOTTOM: #dcdcdc 1pt; BORDER-LEFT: #dcdcdc 1pt">
																	<xsl:value-of select="ns1:value[1]"/>
																</span>
															</td>
														</tr>
													</xsl:for-each>
												</tbody>
											</table>
										</div>
									</td>
									<td style="BORDER-TOP-COLOR: ">
										<font size="2" face="Calibri">
											<div align="left"> </div>
										</font>
									</td>
								</tr>
							</tbody>
						</table>
					</div>
					<div/>
					<div><xsl:apply-templates select="ns1:testCase" mode="_5"/>
					</div>
				</div>
			</body>
		</html>
	</xsl:template>
	<xsl:template match="ns1:setting" mode="_1">
		<xsl:if test="not((ns1:name != &quot;Documentation&quot;))">
			<div title="" class="xdRepeatingSection xdRepeating" style="WIDTH: 1157px; MARGIN: auto auto 0px" align="left" xd:xctname="RepeatingSection" xd:CtrlId="CTRL98" tabIndex="-1" xd:widgetIndex="0" xd:caption_0="Show only Documentation">
				<div><xsl:apply-templates select="ns1:value" mode="_2"/>
				</div>
			</div>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ns1:value" mode="_2">
		<div title="" class="xdRepeatingSection xdRepeating" style="WIDTH: 1149px; MARGIN: auto auto 0px" align="left" xd:xctname="RepeatingSection" xd:CtrlId="CTRL99" tabIndex="-1" xd:widgetIndex="0">
			<font face="Arial Narrow"><span title="" class="xdTextBox" hideFocus="1" tabIndex="0" xd:xctname="PlainText" xd:CtrlId="CTRL100" xd:binding="." xd:datafmt="&quot;string&quot;,&quot;plainMultiline&quot;" style="WORD-WRAP: break-word; FONT-SIZE: medium; BORDER-TOP: #dcdcdc 1pt; FONT-FAMILY: Arial; BORDER-RIGHT: #dcdcdc 1pt; WIDTH: 1123px; WHITE-SPACE: pre-wrap; OVERFLOW-X: auto; OVERFLOW-Y: auto; BORDER-BOTTOM: #dcdcdc 1pt; COLOR: #002060; FONT-STYLE: normal; BORDER-LEFT: #dcdcdc 1pt">
					<xsl:choose>
						<xsl:when test="function-available('xdFormatting:formatString')">
							<xsl:value-of select="xdFormatting:formatString(.,&quot;string&quot;,&quot;plainMultiline&quot;)" disable-output-escaping="yes"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="." disable-output-escaping="yes"/>
						</xsl:otherwise>
					</xsl:choose>
				</span>
			</font>
			<div>
				<font color="#002060" size="4"></font> </div>
		</div>
	</xsl:template>
	<xsl:template match="ns1:testCase" mode="_5">
		<div title="" class="xdRepeatingSection xdRepeating" style="WIDTH: 32772px; MARGIN: 0px" align="left" xd:xctname="RepeatingSection" xd:CtrlId="CTRL46" tabIndex="-1" xd:widgetIndex="0">
      <xsl:attribute name="id">
        <xsl:value-of select="ns1:id"/>
      </xsl:attribute>
      <div>
				<table class="xdLayout" style="WORD-WRAP: break-word; BORDER-TOP: medium none; BORDER-RIGHT: medium none; WIDTH: 18432px; BORDER-COLLAPSE: collapse; TABLE-LAYOUT: fixed; BORDER-BOTTOM: medium none; BORDER-LEFT: medium none" borderColor="buttontext" border="1">
					<colgroup>
						<col style="WIDTH: 18432px"></col>
					</colgroup>
					<tbody vAlign="top">
						<tr>
							<td style="BORDER-BOTTOM-COLOR: ">
								<div><xsl:apply-templates select="ns1:setting" mode="_61"/>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div><xsl:apply-templates select="ns1:setting" mode="_55"/>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div><xsl:apply-templates select="ns1:keyword" mode="_52"/>
								</div>
							</td>
						</tr>
						<tr>
							<td>
								<div><xsl:apply-templates select="ns1:keyword" mode="_49"/>
								</div>
							</td>
						</tr>
					</tbody>
				</table>
			</div>
		</div>
	</xsl:template>
	<xsl:template match="ns1:setting" mode="_61">
		<xsl:if test="not((../ns1:id = 1) or (count(../ns1:setting/ns1:name[text() = &quot;[Documentation]&quot;]) = 0))">
			<div title="" class="xdRepeatingSection xdRepeating" style="MARGIN-BOTTOM: 0px; WIDTH: 100%" align="left" xd:xctname="RepeatingSection" xd:CtrlId="CTRL166" tabIndex="-1" xd:widgetIndex="0" xd:caption_0="Show link only at the end of group 1" xd:caption_1="Show link only at the end of group 2">#↑ <a href="#top" xd:disableEditing="yes">Return to top.</a>
			</div>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ns1:setting" mode="_55">
		<xsl:if test="not((ns1:name != &quot;[Documentation]&quot;))">
			<div title="" class="xdRepeatingSection xdRepeating" style="MARGIN-BOTTOM: 0px; WIDTH: 18425px" align="left" xd:xctname="RepeatingSection" xd:CtrlId="CTRL160" tabIndex="-1" xd:widgetIndex="0" xd:caption_0="Documentation only">
				<div align="left">
					<table class="xdLayout" style="WORD-WRAP: break-word; BORDER-TOP: medium none; BORDER-RIGHT: medium none; WIDTH: 9464px; BORDER-COLLAPSE: collapse; TABLE-LAYOUT: fixed; BORDER-BOTTOM: medium none; BORDER-LEFT: medium none" borderColor="buttontext" border="1">
						<colgroup>
							<col style="WIDTH: 250px"></col>
							<col style="WIDTH: 9214px"></col>
						</colgroup>
						<tbody vAlign="top">
							<tr>
								<td>
									<div>
										<font size="2" face="Calibri"></font> </div>
								</td>
								<td>
									<div><xsl:apply-templates select="ns1:value" mode="_58"/>
									</div>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ns1:value" mode="_58">
		<span title="" class="xdRepeatingSection xdRepeating" style="BORDER-TOP: 0px; BORDER-RIGHT: 0px; WIDTH: 1154px; VERTICAL-ALIGN: top; BORDER-BOTTOM: 0px; MARGIN: 2pt -1pt auto auto; BORDER-LEFT: 0px; DISPLAY: inline-block" xd:xctname="RepeatingSection" xd:CtrlId="CTRL55" align="left" tabIndex="-1" xd:widgetIndex="0">
			<div>
				<table class="xdRepeatingTable msoUcTable" style="BORDER-TOP-STYLE: none; WORD-WRAP: break-word; WIDTH: 1146px; BORDER-COLLAPSE: collapse; TABLE-LAYOUT: fixed; BORDER-BOTTOM-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none" border="1">
					<colgroup>
						<col style="WIDTH: 1146px"></col>
					</colgroup>
					<tbody>
						<tr>
							<td style="BORDER-TOP: #d8d8d8 1pt; BORDER-RIGHT: #d8d8d8 1pt; BORDER-BOTTOM: #d8d8d8 1pt; BORDER-LEFT: #d8d8d8 1pt">
								<div><span title="" class="xdTextBox" hideFocus="1" tabIndex="0" xd:xctname="PlainText" xd:CtrlId="CTRL56" xd:binding="." xd:datafmt="&quot;string&quot;,&quot;plainMultiline&quot;" style="WORD-WRAP: break-word; FONT-SIZE: medium; BORDER-TOP: #000000 1pt; FONT-FAMILY: Arial; BORDER-RIGHT: #000000 1pt; WHITE-SPACE: pre-wrap; OVERFLOW-X: visible; OVERFLOW-Y: visible; BORDER-BOTTOM: #000000 1pt; COLOR: #002060; BORDER-LEFT: #000000 1pt">
										<xsl:choose>
											<xsl:when test="function-available('xdFormatting:formatString')">
												<xsl:value-of select="xdFormatting:formatString(.,&quot;string&quot;,&quot;plainMultiline&quot;)" disable-output-escaping="yes"/>
											</xsl:when>
											<xsl:otherwise>
												<xsl:value-of select="." disable-output-escaping="yes"/>
											</xsl:otherwise>
										</xsl:choose>
									</span>
								</div>
							</td>
						</tr>
					</tbody>
				</table>
			</div>
		</span>
	</xsl:template>
	<xsl:template match="ns1:keyword" mode="_52">
		<xsl:if test="not((count(../ns1:setting/ns1:name[text() = &quot;[Documentation]&quot;]) = 0))">
			<div title="" class="xdRepeatingSection xdRepeating" style="MARGIN-BOTTOM: 0px; WIDTH: 100%" align="left" xd:xctname="RepeatingSection" xd:CtrlId="CTRL157" tabIndex="-1" xd:widgetIndex="0" xd:caption_0="Show only when documentation is available">
				<div>
					<table class="xdLayout" style="WORD-WRAP: break-word; BORDER-TOP: medium none; BORDER-RIGHT: medium none; WIDTH: 3263px; BORDER-COLLAPSE: collapse; TABLE-LAYOUT: fixed; BORDER-BOTTOM: medium none; BORDER-LEFT: medium none" borderColor="buttontext" border="1">
						<colgroup>
							<col style="WIDTH: 250px"></col>
							<col style="WIDTH: 808px"></col>
							<col style="WIDTH: 2205px"></col>
						</colgroup>
						<tbody vAlign="top">
							<tr>
								<td style="BORDER-TOP: #000000 1pt; BORDER-RIGHT: #000000 1pt; BORDER-BOTTOM: #000000 1pt; BORDER-LEFT: #000000 1pt">
									<div>
										<span class="xdlabel"></span> </div>
								</td>
								<td style="BORDER-TOP: #000000 1pt; BORDER-RIGHT: #000000 1pt; BORDER-BOTTOM: #000000 1pt; BORDER-LEFT: #000000 1pt; PADDING-RIGHT: 1px">
									<div>
										<table class="xdLayout" style="BORDER-TOP-STYLE: none; WORD-WRAP: break-word; WIDTH: 806px; BORDER-COLLAPSE: collapse; TABLE-LAYOUT: fixed; BORDER-BOTTOM-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none" border="1">
											<colgroup>
												<col style="WIDTH: 806px"></col>
											</colgroup>
											<tbody vAlign="top">
												<tr>
													<td style="BORDER-TOP-STYLE: none; BORDER-BOTTOM: #000000 3pt; PADDING-BOTTOM: 0px; PADDING-TOP: 0px; BORDER-RIGHT-STYLE: none; PADDING-LEFT: 0px; MARGIN: 0px; BORDER-LEFT-STYLE: none; PADDING-RIGHT: 0px">
														<div><xsl:apply-templates select="ns1:argument" mode="_53"/>
														</div>
													</td>
												</tr>
											</tbody>
										</table>
									</div>
								</td>
								<td style="BORDER-LEFT: #000000 1pt">
									<div><xsl:apply-templates select="ns1:argument" mode="_54"/>
									</div>
								</td>
							</tr>
						</tbody>
					</table>
				</div>
			</div>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ns1:argument" mode="_53">
		<xsl:if test="not((ns1:tag != &quot;in&quot;) or (ns1:id = 1))">
			<span title="" class="xdRepeatingSection xdRepeating" style="BORDER-TOP: 0px; BORDER-RIGHT: 0px; WIDTH: 400px; VERTICAL-ALIGN: top; BORDER-BOTTOM: #002060 3pt solid; MARGIN: 0px; BORDER-LEFT: 0px; DISPLAY: inline-block" xd:xctname="RepeatingSection" xd:CtrlId="CTRL158" align="left" tabIndex="-1" xd:widgetIndex="0" xd:caption_0="Input arguments only" xd:caption_1="">
				<div><span title="" class="xdTextBox" hideFocus="1" tabIndex="0" xd:xctname="PlainText" xd:CtrlId="CTRL162" xd:binding="ns1:name" style="WORD-WRAP: break-word; FONT-SIZE: large; BORDER-TOP: #dcdcdc 1pt; HEIGHT: 50px; FONT-FAMILY: Arial Narrow; BORDER-RIGHT: #dcdcdc 1pt; WHITE-SPACE: pre-wrap; BORDER-BOTTOM: #dcdcdc 1pt; FONT-WEIGHT: bold; BORDER-LEFT: #dcdcdc 1pt">
						<xsl:value-of select="ns1:name"/>
					</span>
				</div>
			</span>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ns1:argument" mode="_54">
		<xsl:if test="not((ns1:tag != &quot;out&quot;))">
			<span title="" class="xdRepeatingSection xdRepeating" style="BORDER-TOP: 0px; BORDER-RIGHT: 0px; WIDTH: 1200px; VERTICAL-ALIGN: top; BORDER-BOTTOM: #002060 3pt solid; MARGIN: 0px; BORDER-LEFT: 0px; DISPLAY: inline-block" xd:xctname="RepeatingSection" xd:CtrlId="CTRL159" align="left" tabIndex="-1" xd:widgetIndex="0" xd:caption_0="Output only">
				<div><span title="" class="xdTextBox" hideFocus="1" tabIndex="0" xd:xctname="PlainText" xd:CtrlId="CTRL163" xd:binding="ns1:name" style="WORD-WRAP: break-word; FONT-SIZE: large; BORDER-TOP: #dcdcdc 1pt; HEIGHT: 50px; FONT-FAMILY: Arial Narrow; BORDER-RIGHT: #dcdcdc 1pt; WHITE-SPACE: pre-wrap; BORDER-BOTTOM: #dcdcdc 1pt; FONT-WEIGHT: bold; BORDER-LEFT: #dcdcdc 1pt">
						<xsl:value-of select="ns1:name"/>
					</span>
				</div>
			</span>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ns1:keyword" mode="_49">
		<div title="" class="xdRepeatingSection xdRepeating" style="MARGIN-BOTTOM: 0px; WIDTH: 100%" align="left" xd:xctname="RepeatingSection" xd:CtrlId="CTRL54" tabIndex="-1" xd:widgetIndex="0">
			<div>
				<table class="xdLayout" style="BORDER-TOP-STYLE: none; WORD-WRAP: break-word; WIDTH: 3724px; BORDER-COLLAPSE: collapse; TABLE-LAYOUT: fixed; BORDER-BOTTOM-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none" border="1">
					<colgroup>
						<col style="WIDTH: 250px"></col>
						<col style="WIDTH: 3474px"></col>
					</colgroup>
					<tbody vAlign="top">
						<tr>
							<td style="BORDER-TOP-STYLE: none; BORDER-RIGHT: #000000 1pt; BORDER-BOTTOM-STYLE: none; PADDING-BOTTOM: 0px; PADDING-TOP: 0px; PADDING-LEFT: 0px; MARGIN: 0px; BORDER-LEFT-STYLE: none; PADDING-RIGHT: 0px">
								<span class="xdlabel">
									<div>
										<table class="xdLayout" style="WORD-WRAP: break-word; BORDER-TOP: medium none; BORDER-RIGHT: medium none; WIDTH: 249px; BORDER-COLLAPSE: collapse; TABLE-LAYOUT: fixed; BORDER-BOTTOM: medium none; BORDER-LEFT: medium none" borderColor="buttontext" border="1">
											<colgroup>
												<col style="WIDTH: 24px"></col>
												<col style="WIDTH: 225px"></col>
											</colgroup>
											<tbody vAlign="top">
												<tr>
													<td>
														<font size="2" face="Calibri">
															<div><span title="" class="xdTextBox xdBehavior_Formatting" hideFocus="1" contentEditable="true" tabIndex="0" xd:xctname="PlainText" xd:CtrlId="CTRL105" xd:binding="../ns1:id" xd:datafmt="&quot;number&quot;,&quot;numDigits:0;negativeOrder:1;&quot;" xd:boundProp="xd:num" style="FONT-SIZE: medium; BORDER-TOP: #dcdcdc 1pt; FONT-FAMILY: Arial Narrow; BORDER-RIGHT: #dcdcdc 1pt; BORDER-BOTTOM: #dcdcdc 1pt; BORDER-LEFT: #dcdcdc 1pt">
																	<xsl:attribute name="xd:num">
																		<xsl:value-of select="../ns1:id"/>
																	</xsl:attribute>
																	<xsl:choose>
																		<xsl:when test="function-available('xdFormatting:formatString')">
																			<xsl:value-of select="xdFormatting:formatString(../ns1:id,&quot;number&quot;,&quot;numDigits:0;negativeOrder:1;&quot;)"/>
																		</xsl:when>
																		<xsl:otherwise>
																			<xsl:value-of select="../ns1:id"/>
																		</xsl:otherwise>
																	</xsl:choose>
																</span>
															</div>
														</font>
													</td>
													<td>
														<div>
															<font size="2" face="Calibri"><span title="" class="xdTextBox" hideFocus="1" tabIndex="0" xd:xctname="PlainText" xd:CtrlId="CTRL155" xd:binding="../ns1:name" style="WORD-WRAP: break-word; FONT-SIZE: medium; BORDER-TOP: #dcdcdc 1pt; FONT-FAMILY: Arial Narrow; BORDER-RIGHT: #dcdcdc 1pt; WHITE-SPACE: pre-wrap; BORDER-BOTTOM: #dcdcdc 1pt; BORDER-LEFT: #dcdcdc 1pt">
																	<xsl:value-of select="../ns1:name"/>
																</span>
															</font>
														</div>
													</td>
												</tr>
											</tbody>
										</table>
									</div>
								</span>
							</td>
							<td style="BORDER-TOP: #000000 1pt; BORDER-RIGHT: #000000 1pt; VERTICAL-ALIGN: top; BORDER-BOTTOM: #000000 1pt; PADDING-BOTTOM: 0px; PADDING-TOP: 0px; PADDING-LEFT: 0px; MARGIN: 0px; BORDER-LEFT: #000000 1pt; PADDING-RIGHT: 0px">
								<div>
									<table class="xdLayout" style="WORD-WRAP: break-word; BORDER-TOP: medium none; BORDER-RIGHT: medium none; WIDTH: 3006px; BORDER-COLLAPSE: collapse; TABLE-LAYOUT: fixed; BORDER-BOTTOM: medium none; BORDER-LEFT: medium none" borderColor="buttontext" border="1">
										<colgroup>
											<col style="WIDTH: 806px"></col>
											<col style="WIDTH: 2200px"></col>
										</colgroup>
										<tbody vAlign="top">
											<tr>
												<td style="BORDER-RIGHT: #00b050 3pt">
													<div>
														<table class="xdLayout" style="BORDER-TOP-STYLE: none; WORD-WRAP: break-word; WIDTH: 804px; BORDER-COLLAPSE: collapse; TABLE-LAYOUT: fixed; BORDER-BOTTOM-STYLE: none; BORDER-RIGHT-STYLE: none; BORDER-LEFT-STYLE: none" border="1">
															<colgroup>
																<col style="WIDTH: 804px"></col>
															</colgroup>
															<tbody vAlign="top">
																<tr>
																	<td style="BORDER-TOP-STYLE: none; BORDER-BOTTOM-STYLE: none; PADDING-BOTTOM: 0px; PADDING-TOP: 0px; BORDER-RIGHT-STYLE: none; PADDING-LEFT: 0px; MARGIN: 0px; BORDER-LEFT-STYLE: none; PADDING-RIGHT: 0px">
																		<div><xsl:apply-templates select="ns1:argument" mode="_50"/>
																		</div>
																	</td>
																</tr>
															</tbody>
														</table>
													</div>
												</td>
												<td style="BORDER-LEFT: #00b050 3pt">
													<div><xsl:apply-templates select="ns1:argument" mode="_51"/>
													</div>
												</td>
											</tr>
										</tbody>
									</table>
								</div>
								<div> </div>
							</td>
						</tr>
					</tbody>
				</table>
			</div>
		</div>
	</xsl:template>
	<xsl:template match="ns1:argument" mode="_50">
		<xsl:if test="not((ns1:tag != &quot;in&quot;) or (ns1:id = 1))">
			<span title="" class="xdRepeatingSection xdRepeating" style="BORDER-TOP: #d8d8d8 1pt solid; HEIGHT: 400px; BORDER-RIGHT: #000000 1pt; WIDTH: 400px; VERTICAL-ALIGN: top; BORDER-BOTTOM: #d8d8d8 1pt solid; MARGIN: 0px; BORDER-LEFT: #d8d8d8 1pt solid; DISPLAY: inline-block; BACKGROUND-COLOR: #f2f2f2" xd:xctname="RepeatingSection" xd:CtrlId="CTRL127" align="left" tabIndex="-1" xd:widgetIndex="0" xd:caption_0="Input arguments only" xd:caption_1="Rule 1">
				<div><span title="" class="xdTextBox" hideFocus="1" contentEditable="true" tabIndex="0" xd:xctname="PlainText" xd:CtrlId="CTRL130" xd:binding="ns1:value" style="WORD-WRAP: break-word; FONT-SIZE: small; BORDER-TOP: #dcdcdc 1pt; FONT-FAMILY: Courier New; BORDER-RIGHT: #dcdcdc 1pt; WHITE-SPACE: pre-wrap; BORDER-BOTTOM: #dcdcdc 1pt; BORDER-LEFT: #dcdcdc 1pt; BACKGROUND-COLOR: #f2f2f2">
						<xsl:value-of select="ns1:value"/>
					</span>
				</div>
			</span>
		</xsl:if>
	</xsl:template>
	<xsl:template match="ns1:argument" mode="_51">
		<xsl:if test="not((ns1:tag != &quot;out&quot;))">
			<span title="" class="xdRepeatingSection xdRepeating" style="BORDER-TOP: #d8d8d8 1pt solid; HEIGHT: 400px; BORDER-RIGHT: #d8d8d8 1pt solid; WIDTH: 1200px; VERTICAL-ALIGN: top; BORDER-BOTTOM: #d8d8d8 1pt solid; MARGIN: 0px; BORDER-LEFT: #d8d8d8 1pt solid; DISPLAY: inline-block; BACKGROUND-COLOR: #cce1e3" xd:xctname="RepeatingSection" xd:CtrlId="CTRL132" align="left" tabIndex="-1" xd:widgetIndex="0" xd:caption_0="Output only">
				<div><span title="" class="xdTextBox" hideFocus="1" contentEditable="true" tabIndex="0" xd:xctname="PlainText" xd:CtrlId="CTRL133" xd:binding="ns1:value" style="WORD-WRAP: break-word; FONT-SIZE: small; BORDER-TOP: #dcdcdc 1pt; FONT-FAMILY: Courier New; BORDER-RIGHT: #dcdcdc 1pt; WHITE-SPACE: pre-wrap; BORDER-BOTTOM: #dcdcdc 1pt; BORDER-LEFT: #dcdcdc 1pt; BACKGROUND-COLOR: #cce1e3">
						<xsl:value-of select="ns1:value"/>
					</span>
				</div>
			</span>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
