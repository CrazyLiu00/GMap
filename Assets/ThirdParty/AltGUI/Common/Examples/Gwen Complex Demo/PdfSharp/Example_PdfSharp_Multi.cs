//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Text;

using Alt.ComponentModel;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;

using Alt.GUI.Demo.PdfSharp;


namespace Alt.GUI.Demo
{
    public class Example_PdfSharp_Multi : Example__Base_Multi
    {
        public Example_PdfSharp_Multi(Base parent)
            : base(parent)
        {
			TitlePrefix = "AltGUI.PdfSharp Demo: ";
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SplitterSetHValue(0.2);
        }


        protected override void RegisterExamples()
        {
            string run = null;
            RegisterExample(run = "Annotations", typeof(Example_Annotations), "This sample shows how to create annotations.");
            RegisterExample("Booklet", typeof(Example_Booklet), "This sample shows how to produce a booklet by placing two pages of an existing document on one landscape orientated page of a new document.");
            RegisterExample("Bookmarks", typeof(Example_Bookmarks), "This sample shows how to create bookmarks. Bookmarks are called outlines in the PDF reference manual, that's why you deal with the class PdfOutline.");
            RegisterExample("Colors CMYK", typeof(Example_ColorsCMYK), "This sample shows how to use CMYK colors.");
            RegisterExample("Combine Documents", typeof(Example_CombineDocuments), "This sample shows how to create a new document from two existing PDF files. The pages are inserted alternately from two external documents. This may be useful for visual comparison.");
            RegisterExample("Concatenate Documents", typeof(Example_ConcatenateDocuments), "This sample shows how to concatenate the pages of several PDF documents to one single file.");
            RegisterExample("Export Images", typeof(Example_ExportImages), "This sample shows how to export JPEG images from a PDF file.");
            RegisterExample("Graphics", typeof(Example_Graphics), "This sample shows some of the capabilities of the XGraphcis class.");
            RegisterExample("Hello World", typeof(Example_HelloWorld), "This sample is the obligatory Hello World program.");
            RegisterExample("Page Sizes", typeof(Example_PageSizes), "This sample shows a document with different page sizes. Note: You can set the size of a page to any size using the Width and Height properties. This sample just shows the predefined sizes.");
            RegisterExample("Protect Document", typeof(Example_ProtectDocument), "This sample shows how to protect a document with a password.");
#if !UNITY_WEBPLAYER
            RegisterExample(run = "Preview", typeof(Example_Preview), "This sample shows the use of Gwen.Control.PdfSharp.PagePreview.");
#endif
            RegisterExample("Split Document", typeof(Example_SplitDocument), "This sample shows how to convert a PDF document with n pages into n documents with one page each.");
            RegisterExample("Text Layout", typeof(Example_TextLayout), "This sample shows how to layout text with the TextFormatter class.");
            RegisterExample("Two Pages On One", typeof(Example_TwoPagesOnOne), "This sample shows how to place two pages of an existing document on one landscape orientated page of a new document.");
            RegisterExample("Unicode", typeof(Example_Unicode), "This sample shows how to use Unicode text in PDFsharp.");
            RegisterExample("Unprotect Document", typeof(Example_UnprotectDocument), "This sample shows how to unprotect a document (if you know the password). Note that we will not explain nor give any tips how to crack a protected document with PDFsharp.");
            RegisterExample("Watermark", typeof(Example_Watermark), "This sample shows three variations how to add a watermark text to an existing PDF file.");
            RegisterExample("Work On PDF Objects", typeof(Example_WorkOnPdfObjects), "This sample shows how to work directly on these underlying PDF objects.");
            RegisterExample("XForms", typeof(Example_XForms), "This sample shows how to create an XForm object from scratch. You can think of such an object as a template, that, once created, can be drawn frequently anywhere in your PDF document.");

            Start(run);
        }
    }
}
