//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_GroupBox : GUnit
    {
        public GUnit_GroupBox(Base parent) : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Color color = Color.White;

            {
				Alt.GUI.Temporary.Gwen.Control.GroupBox gb = new Alt.GUI.Temporary.Gwen.Control.GroupBox(this);
                gb.TextColor = color;
                gb.Text = "Group Box (centered)";
                gb.SetBounds(10, 10, 200, 100);
                //Align.Center(gb);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.GroupBox gb = new Alt.GUI.Temporary.Gwen.Control.GroupBox(this);
                gb.TextColor = color;
                gb.AutoSizeToContents = true;
                gb.Text = "With Label (autosized)";
                gb.SetPosition(250, 10);
				Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(gb);
                label.TextColor = color;
                label.AutoSizeToContents = true;
                label.Text = "I'm a label";
            }

            {
				Alt.GUI.Temporary.Gwen.Control.GroupBox gb = new Alt.GUI.Temporary.Gwen.Control.GroupBox(this);
                gb.TextColor = color;
                gb.AutoSizeToContents = true;
                gb.Text = "With Label (autosized)";
                gb.SetPosition(250, 50);
				Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(gb);
                label.TextColor = color;
                label.AutoSizeToContents = true;
                label.Text = "I'm a label. I'm a really long label!";
            }

            {
				Alt.GUI.Temporary.Gwen.Control.GroupBox gb = new Alt.GUI.Temporary.Gwen.Control.GroupBox(this);
                gb.TextColor = color;
                gb.AutoSizeToContents = true;
                gb.Text = "Two docked Labels (autosized)";
                gb.SetPosition(250, 100);
				Alt.GUI.Temporary.Gwen.Control.Label label1 = new Alt.GUI.Temporary.Gwen.Control.Label(gb);
                label1.TextColor = color;
                label1.AutoSizeToContents = true;
                label1.Text = "I'm a label";
                label1.Dock = Alt.GUI.Temporary.Gwen.Pos.Top;
				Alt.GUI.Temporary.Gwen.Control.Label label2 = new Alt.GUI.Temporary.Gwen.Control.Label(gb);
                label2.TextColor = color;
                label2.AutoSizeToContents = true;
                label2.Text = "I'm a label. I'm a really long label!";
                label2.Dock = Alt.GUI.Temporary.Gwen.Pos.Top;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.GroupBox gb = new Alt.GUI.Temporary.Gwen.Control.GroupBox(this);
                gb.TextColor = color;
                gb.AutoSizeToContents = true;
                gb.Text = "Empty (autosized)";
                gb.SetPosition(10, 150);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.GroupBox gb1 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(this);
                gb1.TextColor = color;
                //Control.Label gb1 = new Label(this);
                gb1.Padding = Alt.GUI.Temporary.Gwen.Padding.Five;
                gb1.Text = "Yo dawg,";
                gb1.SetPosition(10, 200);
                gb1.SetSize(350, 200);
                //gb1.AutoSizeToContents = true;
                
				Alt.GUI.Temporary.Gwen.Control.GroupBox gb2 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(gb1);
                gb2.TextColor = color;
                gb2.Text = "I herd";
                gb2.Dock = Alt.GUI.Temporary.Gwen.Pos.Left;
                gb2.Margin = Alt.GUI.Temporary.Gwen.Margin.Three;
                gb2.Padding = Alt.GUI.Temporary.Gwen.Padding.Five;
                //gb2.AutoSizeToContents = true;
                
				Alt.GUI.Temporary.Gwen.Control.GroupBox gb3 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(gb1);
                gb3.TextColor = color;
                gb3.Text = "You like";
                gb3.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
                
				Alt.GUI.Temporary.Gwen.Control.GroupBox gb4 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(gb3);
                gb4.TextColor = color;
                gb4.Text = "Group Boxes,";
                gb4.Dock = Alt.GUI.Temporary.Gwen.Pos.Top;
                gb4.AutoSizeToContents = true;

				Alt.GUI.Temporary.Gwen.Control.GroupBox gb5 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(gb3);
                gb5.TextColor = color;
                gb5.Text = "So I put Group";
                gb5.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
                //gb5.AutoSizeToContents = true;

				Alt.GUI.Temporary.Gwen.Control.GroupBox gb6 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(gb5);
                gb6.TextColor = color;
                gb6.Text = "Boxes in yo";
                gb6.Dock = Alt.GUI.Temporary.Gwen.Pos.Left;
                gb6.AutoSizeToContents = true;

				Alt.GUI.Temporary.Gwen.Control.GroupBox gb7 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(gb5);
                gb7.TextColor = color;
                gb7.Text = "Boxes so you can";
                gb7.Dock = Alt.GUI.Temporary.Gwen.Pos.Top;
                gb7.SetSize(100, 100);

				Alt.GUI.Temporary.Gwen.Control.GroupBox gb8 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(gb7);
                gb8.TextColor = color;
                gb8.Text = "Group Box while";
                gb8.Dock = Alt.GUI.Temporary.Gwen.Pos.Top;
                gb8.Margin = Alt.GUI.Temporary.Gwen.Margin.Five;
                gb8.AutoSizeToContents = true;

				Alt.GUI.Temporary.Gwen.Control.GroupBox gb9 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(gb7);
                gb9.TextColor = color;
                gb9.Text = "u Group Box";
                gb9.Dock = Alt.GUI.Temporary.Gwen.Pos.Bottom;
                gb9.Padding = Alt.GUI.Temporary.Gwen.Padding.Five;
                gb9.AutoSizeToContents = true;
            }
            
            // at the end to apply to all children
            DrawDebugOutlines = true;
        }
    }
}
