//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_Label : GUnit
    {
        Font font1;
        Font font2;
        Font font3;


        public GUnit_Label(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Color color = Color.White;

            {
				Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                label.Text = "Standard label (not autosized)";
                label.TextColor = color;
                label.SetBounds(10, 10, 150, 12);
            }
            {
				Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                label.AutoSizeToContents = true;
                label.Text = "Chinese: \u4E45\u6709\u5F52\u5929\u613F \u7EC8\u8FC7\u9B3C\u95E8\u5173";
                label.TextColor = color;
                label.SetPosition(10, 30);
            }
            {
				Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                label.AutoSizeToContents = true;
                label.Text = "Japanese: \u751F\u3080\u304E\u3000\u751F\u3054\u3081\u3000\u751F\u305F\u307E\u3054";
                label.TextColor = color;
                label.SetPosition(10, 50);
            }
            {
				Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                label.AutoSizeToContents = true;
                label.Text = "Korean: \uADF9\uC9C0\uD0D0\uD5D8\u3000\uD611\uD68C\uACB0\uC131\u3000\uCCB4\uACC4\uC801\u3000\uC5F0\uAD6C";
                label.TextColor = color;
                label.SetPosition(10, 70);
            }
            {
				Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                label.AutoSizeToContents = true;
                label.Text = "Hindi: \u092F\u0947 \u0905\u0928\u0941\u091A\u094D\u091B\u0947\u0926 \u0939\u093F\u0928\u094D\u0926\u0940 \u092E\u0947\u0902 \u0939\u0948\u0964";
                label.TextColor = color;
                label.SetPosition(10, 90);
            }
            {
				Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                label.AutoSizeToContents = true;
                label.Text = "Arabic: \u0627\u0644\u0622\u0646 \u0644\u062D\u0636\u0648\u0631 \u0627\u0644\u0645\u0624\u062A\u0645\u0631 \u0627\u0644\u062F\u0648\u0644\u064A";
                label.TextColor = color;
                label.SetPosition(10, 110);
            }
            {
				Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                label.AutoSizeToContents = true;
                label.MouseInputEnabled = true; // needed for tooltip
                label.Text = "Wow, Coloured Text (and tooltip)";
                label.TextColor = Color.Cyan;
                label.SetToolTipText("I'm a tooltip");
                font3 = new Font("Motorwerk", 20);
				((Alt.GUI.Temporary.Gwen.Control.Label) label.ToolTip).Font = font3;
                label.SetPosition(10, 130);
            }
            {
				Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                label.AutoSizeToContents = true;
                label.Text = "Coloured Text With Alpha";
                //label.TextColor = Color.FromArgb(200, 0, 0, 255);
                //label.TextColor = Color.FromArgb(100, 255, 0, 0);
                label.TextColor = Color.FromArgb(200, Color.Lime * 1.2);
                label.SetPosition(10, 150);
            }
            {
                // Note that when using a custom font, this font object has to stick around
                // for the lifetime of the label. Rethink, or is that ideal?
                font1 = new Font("Comic Sans MS", 25);

				Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                label.AutoSizeToContents = true;
                label.Text = "Custom Font (Comic Sans 25)";
                label.TextColor = color;
                label.SetPosition(10, 170);
                label.Font = font1;
            }
            {
                font2 = new Font("French Script MT", 35);

				Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                label.AutoSizeToContents = true;
                label.Font = font2;
                label.SetPosition(10, 210);
                label.Text = "Custom Font (French Script MT 35)";
                label.TextColor = color;
            }

            // alignment test
            {
				Alt.GUI.Temporary.Gwen.Control.Label txt = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                txt.SetPosition(10, 280);
                txt.Text = "Alignment test";
                txt.TextColor = color;
                txt.AutoSizeToContents = true;

				Alt.GUI.Temporary.Gwen.Control.Label outer = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                outer.SetBounds(10, 300, 190, 190);
                
				Alt.GUI.Temporary.Gwen.Control.Label l11 = new Alt.GUI.Temporary.Gwen.Control.Label(outer);
                l11.SetBounds(10, 10, 50, 50);
                l11.Text = "TL";
                l11.TextColor = color;
                l11.Alignment = Alt.GUI.Temporary.Gwen.Pos.Top | Alt.GUI.Temporary.Gwen.Pos.Left;

				Alt.GUI.Temporary.Gwen.Control.Label l12 = new Alt.GUI.Temporary.Gwen.Control.Label(outer);
                l12.SetBounds(70, 10, 50, 50);
                l12.Text = "T";
                l12.TextColor = color;
                l12.Alignment = Alt.GUI.Temporary.Gwen.Pos.Top | Alt.GUI.Temporary.Gwen.Pos.CenterH;

				Alt.GUI.Temporary.Gwen.Control.Label l13 = new Alt.GUI.Temporary.Gwen.Control.Label(outer);
                l13.SetBounds(130, 10, 50, 50);
                l13.Text = "TR";
                l13.TextColor = color;
                l13.Alignment = Alt.GUI.Temporary.Gwen.Pos.Top | Alt.GUI.Temporary.Gwen.Pos.Right;

				Alt.GUI.Temporary.Gwen.Control.Label l21 = new Alt.GUI.Temporary.Gwen.Control.Label(outer);
                l21.SetBounds(10, 70, 50, 50);
                l21.Text = "L";
                l21.TextColor = color;
                l21.Alignment = Alt.GUI.Temporary.Gwen.Pos.Left | Alt.GUI.Temporary.Gwen.Pos.CenterV;

				Alt.GUI.Temporary.Gwen.Control.Label l22 = new Alt.GUI.Temporary.Gwen.Control.Label(outer);
                l22.SetBounds(70, 70, 50, 50);
                l22.Text = "C";
                l22.TextColor = color;
                l22.Alignment = Alt.GUI.Temporary.Gwen.Pos.CenterH | Alt.GUI.Temporary.Gwen.Pos.CenterV;

				Alt.GUI.Temporary.Gwen.Control.Label l23 = new Alt.GUI.Temporary.Gwen.Control.Label(outer);
                l23.SetBounds(130, 70, 50, 50);
                l23.Text = "R";
                l23.TextColor = color;
                l23.Alignment = Alt.GUI.Temporary.Gwen.Pos.Right | Alt.GUI.Temporary.Gwen.Pos.CenterV;

				Alt.GUI.Temporary.Gwen.Control.Label l31 = new Alt.GUI.Temporary.Gwen.Control.Label(outer);
                l31.SetBounds(10, 130, 50, 50);
                l31.Text = "BL";
                l31.TextColor = color;
                l31.Alignment = Alt.GUI.Temporary.Gwen.Pos.Bottom | Alt.GUI.Temporary.Gwen.Pos.Left;

				Alt.GUI.Temporary.Gwen.Control.Label l32 = new Alt.GUI.Temporary.Gwen.Control.Label(outer);
                l32.SetBounds(70, 130, 50, 50);
                l32.Text = "B";
                l32.TextColor = color;
                l32.Alignment = Alt.GUI.Temporary.Gwen.Pos.Bottom | Alt.GUI.Temporary.Gwen.Pos.CenterH;

				Alt.GUI.Temporary.Gwen.Control.Label l33 = new Alt.GUI.Temporary.Gwen.Control.Label(outer);
                l33.SetBounds(130, 130, 50, 50);
                l33.Text = "BR";
                l33.TextColor = color;
                l33.Alignment = Alt.GUI.Temporary.Gwen.Pos.Bottom | Alt.GUI.Temporary.Gwen.Pos.Right;

                outer.DrawDebugOutlines = true;
            }
        }


        public override void Dispose()
        {
            if (font1 != null)
            {
                font1.Dispose();
                font1 = null;
            }

            if (font2 != null)
            {
                font2.Dispose();
                font2 = null;
            }

            if (font3 != null)
            {
                font3.Dispose();
                font3 = null;
            }

            base.Dispose();
        }
    }
}
