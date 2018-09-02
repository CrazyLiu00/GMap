//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_Docking : GUnit
    {
        Font font;
		Alt.GUI.Temporary.Gwen.Control.Label outer;
        readonly Color labelColor = Color.White;


        public GUnit_Docking(Base parent)
            : base(parent)
        {
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            font = new Alt.Sketch.Font(Skin.DefaultFont.FontFamily, 20);

			outer = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            outer.SetBounds(10, 10, 400, 400);

			Alt.GUI.Temporary.Gwen.Control.Label inner1 = new Alt.GUI.Temporary.Gwen.Control.Label(outer);
            inner1.TextColor = labelColor;
            inner1.Text = "1";
            inner1.Font = font;
            inner1.SetSize(100, 100);
            inner1.Dock = Alt.GUI.Temporary.Gwen.Pos.Left;

			Alt.GUI.Temporary.Gwen.Control.Label inner2 = new Alt.GUI.Temporary.Gwen.Control.Label(outer);
            inner2.TextColor = labelColor;
            inner2.Text = "2";
            inner2.Font = font;
            inner2.SetSize(100, 100);
            inner2.Dock = Alt.GUI.Temporary.Gwen.Pos.Top;

			Alt.GUI.Temporary.Gwen.Control.Label inner3 = new Alt.GUI.Temporary.Gwen.Control.Label(outer);
            inner3.TextColor = labelColor;
            inner3.Text = "3";
            inner3.Font = font;
            inner3.SetSize(100, 100);
            inner3.Dock = Alt.GUI.Temporary.Gwen.Pos.Right;

			Alt.GUI.Temporary.Gwen.Control.Label inner4 = new Alt.GUI.Temporary.Gwen.Control.Label(outer);
            inner4.TextColor = labelColor;
            inner4.Text = "4";
            inner4.Font = font;
            inner4.SetSize(100, 100);
            inner4.Dock = Alt.GUI.Temporary.Gwen.Pos.Bottom;

			Alt.GUI.Temporary.Gwen.Control.Label inner5 = new Alt.GUI.Temporary.Gwen.Control.Label(outer);
            inner5.TextColor = labelColor;
            inner5.Text = "5";
            inner5.Font = font;
            inner5.SetSize(100, 100);
            inner5.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;

            outer.DrawDebugOutlines = true;

            inner1.UserData = CreateControls(inner1, 0, "Control 1", 440, 10);
            inner2.UserData = CreateControls(inner2, 1, "Control 2", 650, 10);
            inner3.UserData = CreateControls(inner3, 2, "Control 3", 440, 170);
            inner4.UserData = CreateControls(inner4, 3, "Control 4", 650, 170);
            inner5.UserData = CreateControls(inner5, 4, "Control 5", 440, 330);

			Alt.GUI.Temporary.Gwen.Control.Label l_padding = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            l_padding.TextColor = labelColor;
            l_padding.Text = "Padding:";
            l_padding.SetSize(60, 19);
            Alt.GUI.Temporary.Gwen.Align.PlaceDownLeft(l_padding, outer, 20);

			Alt.GUI.Temporary.Gwen.Control.HorizontalSlider padding = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            padding.Min = 0;
            padding.Max = 200;
            padding.Value = 10;
            padding.SetSize(100, 20);
            padding.ValueChanged += PaddingChanged;
            Alt.GUI.Temporary.Gwen.Align.PlaceRightBottom(padding, l_padding);

            //DrawDebugOutlines = true;
        }


        Base CreateControls(Base subject, int dock_idx, String name, int x, int y)
        {
			Alt.GUI.Temporary.Gwen.Control.GroupBox gb = new Alt.GUI.Temporary.Gwen.Control.GroupBox(this);
            gb.TextColor = labelColor;
            gb.SetBounds(x, y, 200, 150);
            gb.Text = name;

			Alt.GUI.Temporary.Gwen.Control.Label l_width = new Alt.GUI.Temporary.Gwen.Control.Label(gb);
            l_width.TextColor = labelColor;
            l_width.SetSize(35, 15);
            l_width.Text = "Width:";
         
			Alt.GUI.Temporary.Gwen.Control.HorizontalSlider width = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(gb);
            width.Name = "Width";
            width.UserData = subject;
            width.Min = 50;
            width.Max = 350;
            width.Value = 100;
            width.SetSize(55, 15);
            width.ValueChanged += WidthChanged;
            Alt.GUI.Temporary.Gwen.Align.PlaceRightBottom(width, l_width);

			Alt.GUI.Temporary.Gwen.Control.Label l_height = new Alt.GUI.Temporary.Gwen.Control.Label(gb);
            l_height.TextColor = labelColor;
            l_height.SetSize(35, 15);
            l_height.Text = "Height:";
            Alt.GUI.Temporary.Gwen.Align.PlaceRightBottom(l_height, width, 10);

			Alt.GUI.Temporary.Gwen.Control.HorizontalSlider height = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(gb);
            height.Name = "Height";
            height.UserData = subject;
            height.Min = 50;
            height.Max = 350;
            height.Value = 100;
            height.SetSize(55, 15);
            height.ValueChanged += HeightChanged;
            Alt.GUI.Temporary.Gwen.Align.PlaceRightBottom(height, l_height);

			Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup dock = new Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup(gb, "Dock");
            dock.TextColor = labelColor;
            dock.UserData = subject; // store control that we are controlling
            dock.AddOption("Left");
            dock.AddOption("Top");
            dock.AddOption("Right");
            dock.AddOption("Bottom");
            dock.AddOption("Fill");
            dock.SetSelection(dock_idx);
            Alt.GUI.Temporary.Gwen.Align.PlaceDownLeft(dock, l_width, 5);
            //dock.DrawDebugOutlines = true;
            dock.Invalidate();

			Alt.GUI.Temporary.Gwen.Control.Label l_margin = new Alt.GUI.Temporary.Gwen.Control.Label(gb);
            l_margin.TextColor = labelColor;
            l_margin.Text = "Margin:";
            l_margin.SetBounds(75, 20, 35, 15);
            //Align.PlaceRightBottom(l_margin, dock);
            // can't use Align to anchor with 'dock' because radio group is resized only after layout ~_~
            // this is become really cumbersome
            //l_margin.DrawDebugOutlines = true;

			Alt.GUI.Temporary.Gwen.Control.HorizontalSlider margin = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(gb);
            margin.Name = "Margin";
            margin.UserData = subject;
            margin.Min = 0;
            margin.Max = 50;
            margin.Value = 10;
            margin.SetSize(55, 15);
            margin.ValueChanged += MarginChanged;
            Alt.GUI.Temporary.Gwen.Align.PlaceRightBottom(margin, l_margin);

            dock.SelectionChanged += DockChanged;

            return gb;
        }


        void PaddingChanged(Base control)
        {
            Slider val = control as Slider;
            int i = (int)val.Value;
            outer.Padding = new Alt.GUI.Temporary.Gwen.Padding(i, i, i, i);
            outer.Invalidate();
        }


        void MarginChanged(Base control)
        {
            Base inner = control.UserData as Base;
			Alt.GUI.Temporary.Gwen.Control.Slider val = control as Alt.GUI.Temporary.Gwen.Control.Slider;
            int i = (int)val.Value;
            inner.Margin = new Alt.GUI.Temporary.Gwen.Margin(i, i, i, i);
            outer.Invalidate();
        }


        void WidthChanged(Base control)
        {
            Base inner = control.UserData as Base;
			Alt.GUI.Temporary.Gwen.Control.Slider val = control as Alt.GUI.Temporary.Gwen.Control.Slider;
            inner.Width = (int)val.Value;
            outer.Invalidate();
        }


        void HeightChanged(Base control)
        {
            Base inner = control.UserData as Base;
			Alt.GUI.Temporary.Gwen.Control.Slider val = control as Alt.GUI.Temporary.Gwen.Control.Slider;
            inner.Height = (int)val.Value;
            outer.Invalidate();
        }


        void DockChanged(Base control)
        {
            Base inner = (Base) control.UserData;
            RadioButtonGroup rbg = (RadioButtonGroup) control;
            Base gb = inner.UserData as Base;
			Alt.GUI.Temporary.Gwen.Control.Slider w = gb.FindChildByName("Width", true) as Slider;
			Alt.GUI.Temporary.Gwen.Control.Slider h = gb.FindChildByName("Height", true) as Slider;

            switch (rbg.SelectedIndex)
            {
                case 0:
                    inner.Dock = Alt.GUI.Temporary.Gwen.Pos.Left; 
                    break;
                case 1:
                    inner.Dock = Alt.GUI.Temporary.Gwen.Pos.Top;
                    break;
                case 2:
                    inner.Dock = Alt.GUI.Temporary.Gwen.Pos.Right;
                    break;
                case 3:
                    inner.Dock = Alt.GUI.Temporary.Gwen.Pos.Bottom;
                    break;
                case 4:
                    inner.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
                    break;
            }
            inner.SetSize((int)w.Value, (int)h.Value);
            //inner.Invalidate();
            outer.Invalidate();
        }


        public override void Dispose()
        {
            if (font != null)
            {
                font.Dispose();
                font = null;
            }

            base.Dispose();
        }
    }
}
