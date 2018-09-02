//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.Temporary.Gwen.Control.Layout;
using Alt.IO;
using Alt.Sketch;


namespace Alt.GUI.Demo.PdfSharp
{
    public abstract class Example_PDFGenerator_Base : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 0;
                int y = 0;

                if (m_TopPanel != null)
                {
                    y = m_TopPanel.Height + m_TopPanel.Margin.Height;
                    x += 15;
                }

                return new SizeI(x, y);
            }
        }



		Alt.GUI.Temporary.Gwen.Control.Base m_TopPanel;
		Alt.GUI.Temporary.Gwen.Control.Label m_LabelText;


        protected Example_PDFGenerator_Base(Base parent, string plain) :
            this(parent, plain, "Process PDF")
        {
        }

        protected Example_PDFGenerator_Base(Base parent, string plain, string buutonText) :
            base(parent)
        {
            //  GUI
			m_TopPanel = new Alt.GUI.Temporary.Gwen.Control.Base(this);
            m_TopPanel.Height = 25;
            m_TopPanel.Margin = new Margin(0, 2, 2, 2);
            m_TopPanel.Dock = Pos.Top;

#if SILVERLIGHT
			Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(m_TopPanel);
			label.AutoSizeToContents = true;
			label.Text = "Processing is not yet available in Silverlight, but all functionality supported!";
#else
#if UNITY_WEBPLAYER
			Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(m_TopPanel);
			label.AutoSizeToContents = true;
			label.Text = "Processing is not yet available in Unity Web Player, but all functionality supported!";
#else
			Alt.GUI.Temporary.Gwen.Control.Button button = new Alt.GUI.Temporary.Gwen.Control.Button(m_TopPanel);
            button.Text = buutonText;
            button.AutoSizeToContents = true;
            button.NormalTextColor = Color.Green;
            button.Click += new EventHandler(button_Click);
#endif
#endif

            Base bg = new Base(this);
            bg.Dock = Pos.Fill;
            bg.ClientBackColor = Color.FromArgb(128, Color.Black);
            bg.DrawBorder = true;
            bg.BorderColor = Color.DodgerBlue;

			Alt.GUI.Temporary.Gwen.Control.ScrollControl scrollControl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(bg);
            scrollControl.Dock = Pos.Fill;
            scrollControl.EnableScroll(true, true);
            scrollControl.AutoHideBars = true;
            scrollControl.Margin = Margin.Five;
            scrollControl.ShouldDrawBackground = false;

			m_LabelText = new Alt.GUI.Temporary.Gwen.Control.Label(scrollControl);
            m_LabelText.Margin = Margin.Two;
            m_LabelText.Location = PointI.Zero;
            m_LabelText.AutoSizeToContents = true;
            m_LabelText.TextColor = Color.White;
            m_LabelText.MouseInputEnabled = true;

            System.IO.Stream src = Alt.IO.VirtualFile.OpenRead("AltData/PdfSharp/plain/" + plain + ".plain");
            if (src == null)
            {
                src = Alt.IO.VirtualFile.OpenRead("AltData/MigraDoc/plain/" + plain + ".plain");
            }
            if (src != null)
            {
                using (System.IO.StreamReader stream = new System.IO.StreamReader(src))
                {
                    m_LabelText.Text = stream.ReadToEnd();
                }
            }
        }


        void button_Click(object sender, EventArgs e)
        {
            DoWork();
        }

        protected virtual void DoWork()
        {
        }


        protected static void File_Copy(string from, string to, bool overwrite)
        {
            if (!overwrite &&
                Alt.IO.File.Exists(to))
            {
                return;
            }

            System.IO.Stream stream = Alt.IO.VirtualFile.OpenRead(from);
            if (stream != null)
            {
                return;
            }

            int len = (int)stream.Length;
            byte[] buf = new byte[len];
            stream.Read(buf, 0, len);

            using (System.IO.Stream fs = //new FileStream
                File.Open(to, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buf, 0, len);
                fs.Flush();
            }
        }


		protected override void Render(Alt.GUI.Temporary.Gwen.Skin.Base skin)
		{
			TextRenderingHint textRenderingHint = skin.Renderer.Graphics.TextRenderingHint;
			skin.Renderer.Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;

			base.Render(skin);

			skin.Renderer.Graphics.TextRenderingHint = textRenderingHint;
		}
    }
}
