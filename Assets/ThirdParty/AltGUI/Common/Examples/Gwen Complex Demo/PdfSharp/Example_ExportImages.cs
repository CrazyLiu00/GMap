// Authors:
//   PDFsharp Team (mailto:PDFsharpSupport@pdfsharp.de)
//
// Copyright (c) 2005-2009 empira Software GmbH, Cologne (Germany)
//
// http://www.pdfsharp.com
// http://sourceforge.net/projects/pdfsharp
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.

//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.


using System;
using System.Collections.Generic;
using System.Diagnostics;

using Alt.IO;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.Temporary.Gwen.Control.Layout;

using Alt.GUI.PdfSharp.Drawing;
using Alt.GUI.PdfSharp.Pdf;
using Alt.GUI.PdfSharp.Pdf.Advanced;
using Alt.GUI.PdfSharp.Pdf.IO;


namespace Alt.GUI.Demo.PdfSharp
{
    public class Example_ExportImages : Example_PDFGenerator_Base
    {
        public Example_ExportImages(Base parent) :
            base(parent, "Example_ExportImages", "Export Images")
        {            
        }


        protected override void OnLoad(EventArgs ea)
        {
            base.OnLoad(ea);
        }


        // This sample shows how to export JPEG images from a PDF file.
        protected override void DoWork()
        {
            const string filename = "AltData/PDFsharp/PDFs/SomeLayout.pdf";

            PdfDocument document = PdfReader.Open(filename);

            int imageCount = 0;
            // Iterate pages
            foreach (PdfPage page in document.Pages)
            {
                // Get resources dictionary
                PdfDictionary resources = page.Elements.GetDictionary("/Resources");
                if (resources != null)
                {
                    // Get external objects dictionary
                    PdfDictionary xObjects = resources.Elements.GetDictionary("/XObject");
                    if (xObjects != null)
                    {
                        ICollection<PdfItem> items = xObjects.Elements.Values;
                        // Iterate references to external objects
                        foreach (PdfItem item in items)
                        {
                            PdfReference reference = item as PdfReference;
                            if (reference != null)
                            {
                                PdfDictionary xObject = reference.Value as PdfDictionary;
                                // Is external object an image?
                                if (xObject != null && xObject.Elements.GetString("/Subtype") == "/Image")
                                {
                                    ExportImage(xObject, ref imageCount);
                                }
                            }
                        }
                    }
                }
            }

			new Alt.GUI.Temporary.Gwen.Control.MessageBox(this, imageCount + " images exported.", "Export Images");
        }


        /// <summary>
        /// Currently extracts only JPEG images.
        /// </summary>
        static void ExportImage(PdfDictionary image, ref int count)
        {
            string filter = image.Elements.GetName("/Filter");
            switch (filter)
            {
                case "/DCTDecode":
                    ExportJpegImage(image, ref count);
                    break;

                case "/FlateDecode":
                    ExportAsPngImage(image, ref count);
                    break;
            }
        }


        /// <summary>
        /// Exports a JPEG image.
        /// </summary>
        static void ExportJpegImage(PdfDictionary image, ref int count)
        {
            // Fortunately JPEG has native support in PDF and exporting an image is just writing the stream to a file.
            byte[] stream = image.Stream.Value;
            System.IO.Stream fs = //new FileStream
                File.Open(String.Format("Image{0}.jpeg", count++), FileMode.Create, FileAccess.Write);
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs);
            bw.Write(stream);
            bw.Close();
        }


        /// <summary>
        /// Exports image in PNF format.
        /// </summary>
        static void ExportAsPngImage(PdfDictionary image, ref int count)
        {
            //int width = image.Elements.GetInteger(PdfImage.Keys.Width);
            //int Height = image.Elements.GetInteger(PdfImage.Keys.Height);
            //int bitsPerComponent = image.Elements.GetInteger(PdfImage.Keys.BitsPerComponent);

            // TODO: You can put the code here that converts vom PDF internal image format to a AltSketch bitmap
            // and use AltSketch to save it in PNG format.
            // It is the work of a day or two for the most important formats. Take a look at the file
            // PdfSharp.Pdf.Advanced/PdfImage.cs to see how we create the PDF image formats.
            // We don't need that feature at the moment and therefore will not implement it.
            // If you write the code for exporting images I would be pleased to publish it in a future release
            // of PDFsharp.
        }
    }
}
