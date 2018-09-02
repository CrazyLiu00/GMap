//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Diagnostics;
using System.Globalization;

using Alt.ComponentModel;
using Alt.GUI.PdfSharp;
using Alt.GUI.PdfSharp.Pdf;
using Alt.GUI.PdfSharp.Drawing;
using Alt.GUI.PdfSharp.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.IO;
using Alt.Sketch;


namespace Alt.GUI.Demo.PdfSharp
{
    /// <summary>
    /// This sample shows the use of Gwen.Control.PdfSharp.PagePreview.
    /// </summary>
    class Example_Preview : Example__Base
    {
        Alt.GUI.PdfSharp.Temporary.Gwen.PagePreview m_PagePreview;
		Alt.GUI.Temporary.Gwen.Control.Label m_StatisBarLabel;


        public Example_Preview(Alt.GUI.Temporary.Gwen.Control.Base parent) :
            base(parent)
        {
            m_PagePreview = new Alt.GUI.PdfSharp.Temporary.Gwen.PagePreview(this);
            m_PagePreview.Dock = Pos.Fill;
            m_PagePreview.ClientBackColor = SystemColors.Control;
            m_PagePreview.DesktopColor = SystemColors.ControlDark;
            m_PagePreview.PageColor = Color.GhostWhite;
            //m_PagePreview.PageSizeF = new SizeI(595, 842);
            m_PagePreview.Zoom = Zoom.Percent75;// BestFit;
            //m_PagePreview.ZoomPercent = 77;
            m_PagePreview.Margin = new Margin(0, 1, 0, 1);
            m_PagePreview.DrawBorder = true;
            m_PagePreview.BorderColor = Color.DodgerBlue;
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

			Alt.GUI.Temporary.Gwen.Control.MenuStrip menu = new Alt.GUI.Temporary.Gwen.Control.MenuStrip(this);

			Alt.GUI.Temporary.Gwen.Control.MenuItem root = menu.AddItem("Original Size");
            {
                root.Menu.AddItem("800%").SetAction(miZoom_Click);
                root.Menu.AddItem("600%").SetAction(miZoom_Click);
                root.Menu.AddItem("400%").SetAction(miZoom_Click);
                root.Menu.AddItem("200%").SetAction(miZoom_Click);
                root.Menu.AddItem("150%").SetAction(miZoom_Click);
                root.Menu.AddItem("100%").SetAction(miZoom_Click);
                root.Menu.AddItem("75%").SetAction(miZoom_Click);
                root.Menu.AddItem("50%").SetAction(miZoom_Click);
                root.Menu.AddItem("25%").SetAction(miZoom_Click);
                root.Menu.AddItem("10%").SetAction(miZoom_Click);
                root.Menu.AddDivider();
                root.Menu.AddItem("Best Fit").SetAction(miBestFit_Click);
                root.Menu.AddItem("Full Page").SetAction(miFullPage_Click);
            }

            menu.AddItem("Full Page"//, "AltData/Gwen/test16.png"
                ).Clicked += miFullPage_Click;
            menu.AddItem("Best Fit"//, "AltData/Gwen/test16.png"
                ).Clicked += miBestFit_Click;
            menu.AddItem("Smaller"//, "AltData/Gwen/test16.png"
                ).Clicked += miSmaller_Click;
            menu.AddItem("Larger"//, "AltData/Gwen/test16.png"
                ).Clicked += miLarger_Click;
#if !SILVERLIGHT
            menu.AddItem("Create PDF"//, "AltData/Gwen/test16.png"
                ).Clicked += miCreatePDF_Click;
#endif


			Alt.GUI.Temporary.Gwen.Control.StatusBar sb = new Alt.GUI.Temporary.Gwen.Control.StatusBar(this);
			m_StatisBarLabel = new Alt.GUI.Temporary.Gwen.Control.Label(sb);
            m_StatisBarLabel.AutoSizeToContents = true;
            m_StatisBarLabel.Text = "";
            sb.AddControl(m_StatisBarLabel, false);


            m_PagePreview.SetRenderEvent(new Alt.GUI.PdfSharp.Temporary.Gwen.PagePreview.RenderEvent(Render));


            m_PagePreview.PageSize = PageSizeConverter.ToSize(PageSize.A4);
            UpdateStatusBar();
        }


        static int GetNewZoom(int currentZoom, bool larger)
        {
            int[] values = new int[]
            {
                10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 120, 140, 160, 180, 200, 
                250, 300, 350, 400, 450, 500, 600, 700, 800
            };

            if (currentZoom <= (int)Zoom.Mininum &&
                !larger)
            {
                return (int)Zoom.Mininum;
            }
            if (currentZoom >= (int)Zoom.Maximum &&
                larger)
            {
                return (int)Zoom.Maximum;
            }

            if (larger)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    if (currentZoom < values[i])
                    {
                        return values[i];
                    }
                }
            }
            else
            {
                for (int i = values.Length - 1; i >= 0; i--)
                {
                    if (currentZoom > values[i])
                    {
                        return values[i];
                    }
                }
            }

            return (int)Zoom.Percent100;
        }


        void UpdateStatusBar()
        {
            if (m_PagePreview == null ||
                m_StatisBarLabel == null)
            {
                return;
            }

            string status = String.Format("PageSize: {0}pt x {1}pt, Zoom: {2}%",
              m_PagePreview.PageSize.Width,
              m_PagePreview.PageSize.Height,
              m_PagePreview.ZoomPercent);

            m_StatisBarLabel.Text = status;
        }

        
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            UpdateStatusBar();
        }


        /// <summary>
        /// Sets the zoom factor.
        /// </summary>
        void miZoom_Click(Base control)
        {
            // Hack menu item to zoom value...
			string name = ((Alt.GUI.Temporary.Gwen.Control.MenuItem)control).Text;
            name = name.Substring(0, name.Length - 1);
            m_PagePreview.ZoomPercent = int.Parse(name, CultureInfo.InvariantCulture);
            UpdateStatusBar();
        }

        void miBestFit_Click(Base control)
        {
            m_PagePreview.Zoom = Zoom.BestFit;
            UpdateStatusBar();
        }

        void miFullPage_Click(Base control)
        {
            m_PagePreview.Zoom = Zoom.FullPage;
            UpdateStatusBar();
        }

        void miSmaller_Click(Base control)
        {
            m_PagePreview.ZoomPercent = GetNewZoom(m_PagePreview.ZoomPercent, false);
            UpdateStatusBar();
        }

        void miLarger_Click(Base control)
        {
            m_PagePreview.ZoomPercent = GetNewZoom(m_PagePreview.ZoomPercent, true);
            UpdateStatusBar();
        }


        /// <summary>
        /// Create a new PDF document and start the viewer.
        /// </summary>
        void miCreatePDF_Click(Base control)
        {
            string filename = Guid.NewGuid().ToString().ToUpper() + ".pdf";
            PdfDocument document = new PdfDocument(filename);
            document.Info.Creator = "Preview Sample";
            PdfPage page = document.AddPage();
            page.Size = PageSize.A4;
            XGraphics gfx = XGraphics.FromPdfPage(page);

            Render(gfx);

            document.Close();

            Diagnostics.ProcessHelper.Start(filename);
        }


        /// <summary>
        /// Renders the content of the page.
        /// </summary>
        public void Render(XGraphics gfx)
        {
            XRect rect;
            XPen pen;
            double x = 50, y = 100;
            XFont fontH1 = new XFont("Times", 18, XFontStyle.Bold);
            XFont font = new XFont("Times", 12);
            XFont fontItalic = new XFont("Times", 12, XFontStyle.BoldItalic);
            double ls = font.GetHeight(gfx);

            // Draw some text
            gfx.DrawString("Create PDF on the fly with PDFsharp",
                fontH1, XBrushes.Black, x, x);
            gfx.DrawString("With PDFsharp you can use the same code to draw graphic, " +
                "text and images on different targets.", font, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("The object used for drawing is the XGraphics object.",
                font, XBrushes.Black, x, y);
            y += 2 * ls;

            // Draw an arc
            pen = new XPen(XColors.Red, 4);
            pen.DashStyle = XDashStyle.Dash;
            gfx.DrawArc(pen, x + 20, y, 100, 60, 150, 120);

            // Draw a star
            XGraphicsState gs = gfx.Save();
            gfx.TranslateTransform(x + 140, y + 30);
            //  thickness 1.001 (or any not natural number) is for drawing smoothing lines
            //  in SmoothingMode.AntiAlias Graphics mode if you don't use RenderLinesByPolygons
            pen = new XPen(XColors.DarkGreen, 1.001);
            for (int idx = 0; idx < 360; idx += 10)
            {
                gfx.RotateTransform(10);
                gfx.DrawLine(pen//XPens.DarkGreen
                    , 0, 0, 30, 0);
            }
            gfx.Restore(gs);

            // Draw a rounded rectangle
            rect = new XRect(x + 230, y, 100, 60);
            pen = new XPen(XColors.DarkBlue, 2.5);
            XColor color1 = XColor.FromKnownColor(KnownColor.DarkBlue);
            XColor color2 = XColors.Red;
            XLinearGradientBrush lbrush = new XLinearGradientBrush(rect, color1, color2,
              XLinearGradientMode.Vertical);
            gfx.DrawRoundedRectangle(pen, lbrush, rect, new XSize(10, 10));

            // Draw a pie
            pen = new XPen(XColors.DarkOrange, 1.5);
            pen.DashStyle = XDashStyle.Dot;
            gfx.DrawPie(pen, XBrushes.Blue, x + 360, y, 100, 60, -130, 135);

            // Draw some more text
            y += 60 + 2 * ls;
            gfx.DrawString("With XGraphics you can draw on a PDF page as well as " +
                "on any System.Drawing.Graphics object.", font, XBrushes.Black, x, y);
            y += ls * 1.1;
            gfx.DrawString("Use the same code to", font, XBrushes.Black, x, y);
            x += 10;
            y += ls * 1.1;
            gfx.DrawString("� draw on a newly created PDF page", font, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("� draw above or beneath of the content of an existing PDF page",
                font, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("� draw in a window", font, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("� draw on a printer", font, XBrushes.Black, x, y);
            y += ls;
            gfx.DrawString("� draw in a bitmap image", font, XBrushes.Black, x, y);
            x -= 10;
            y += ls * 1.1;
            gfx.DrawString("You can also import an existing PDF page and use it like " +
                "an image, e.g. draw it on another PDF page.", font, XBrushes.Black, x, y);
            y += ls * 1.1 * 2;
            gfx.DrawString("Imported PDF pages are neither drawn nor printed; create a " +
                "PDF file to see or print them!", fontItalic, XBrushes.Firebrick, x, y);
            y += ls * 1.1;
            gfx.DrawString("Below this text is a PDF form that will be visible when " +
                "viewed or printed with a PDF viewer.", fontItalic, XBrushes.Firebrick, x, y);
            y += ls * 1.1;
            XGraphicsState state = gfx.Save();
            XRect rcImage = new XRect(100, y, 100, 100 * Math.Sqrt(2));
            gfx.DrawRectangle(XBrushes.Snow, rcImage);
            gfx.DrawImage(XPdfForm.FromFile("AltData/PDFsharp/PDFs/SomeLayout.pdf"), rcImage);
            gfx.Restore(state);
        }
    }
}
