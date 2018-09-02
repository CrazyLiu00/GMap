//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
    abstract class Example_NotAvailable_ScreenShot : Example_NotAvailable
    {
        protected abstract string Description
        {
            get;
        }


        protected abstract Bitmap Screenshot
        {
            get;
        }


		Alt.GUI.Temporary.Gwen.Control.PictureBox m_Example_NotAvailable_PictureBox;


        public Example_NotAvailable_ScreenShot(Base parent)
            : base(parent)
        {
        }


        static Font m_Font;
        internal static Font Font
        {
            get
            {
                if (m_Font == null)
                {
                    m_Font = new Font("Arial", 12, FontStyle.Bold);
                }

                return m_Font;
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


			Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            label.AutoSizeToContents = true;
            label.Text = //Description + "\n" +//"\n(This example is not available in this Demo, please download SDK)";
				"THIS EXAMPLE IS NOT AVAILABLE IN THIS DEMO,\nPLEASE DOWNLOAD AltGUI SDK";
            label.TextColor = Color.Orange * 1.2;
            label.Dock = Pos.Top;
            label.Margin = new Margin(0, 0, 0, 5);
            label.Font = Example_NotAvailable_ScreenShot.Font;


			m_Example_NotAvailable_PictureBox = new Alt.GUI.Temporary.Gwen.Control.PictureBox(this);
            m_Example_NotAvailable_PictureBox.Margin = Margin.Two;
            m_Example_NotAvailable_PictureBox.Dock = Pos.Fill;
            
            Bitmap screenshot = Screenshot;
            if (screenshot == null)
            {
                return;
            }

            m_Example_NotAvailable_PictureBox.Image = screenshot;

            if (this.ClientRectangle.Contains(new PointI(screenshot.PixelSize)))
            {
				m_Example_NotAvailable_PictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            }
            else
            {
				m_Example_NotAvailable_PictureBox.SizeMode = PictureBoxSizeMode.Normal;
            }
        }


        protected internal static Bitmap LoadImage(string fileName)
        {
            return Bitmap.FromFile("AltData/NotAvailableExamples/" + fileName);
        }
    }
}
