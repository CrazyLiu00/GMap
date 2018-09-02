//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_TabControl : GUnit
    {
		Alt.GUI.Temporary.Gwen.Control.TabControl m_DockControl;


        public GUnit_TabControl(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            {
				m_DockControl = new Alt.GUI.Temporary.Gwen.Control.TabControl(this);
                m_DockControl.SetBounds(10, 10, 200, 200);

                {
					Alt.GUI.Temporary.Gwen.Control.TabButton button = m_DockControl.AddPage("Controls");
					Alt.GUI.Temporary.Gwen.Control.Base page = button.Page;

                    {
                        Color rbColor = Color.Gray * 0.5;

						Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup radio = new Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup(page, "Tab position");
                        radio.TextColor = rbColor;
                        radio.UseCurrentColorAsNormal = true;
                        radio.SetPosition(10, 10);

						Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rb;
                        rb = radio.AddOption("Top"); rb.Select();
                        rb.NormalTextColor = rbColor;
                        rb = radio.AddOption("Bottom");
                        rb.NormalTextColor = rbColor;
                        rb = radio.AddOption("Left");
                        rb.NormalTextColor = rbColor;
                        rb = radio.AddOption("Right");
                        rb.NormalTextColor = rbColor;

                        radio.SelectionChanged += OnDockChange;

                    }
                }

                m_DockControl.AddPage("Red");
                m_DockControl.AddPage("Green");
                m_DockControl.AddPage("Blue");
            }

            {
				Alt.GUI.Temporary.Gwen.Control.TabControl dragMe = new Alt.GUI.Temporary.Gwen.Control.TabControl(this);
                dragMe.SetBounds(220, 10, 200, 200);

                dragMe.AddPage("You");
                dragMe.AddPage("Can");
                dragMe.AddPage("Reorder").SetImage("AltData/Gwen/test16.png");
                dragMe.AddPage("These");
                dragMe.AddPage("Tabs");

                dragMe.AllowReorder = true;
            }
        }

        void OnDockChange(Base control)
        {
			Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup rc = (Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup)control;

            if (rc.SelectedLabel == "Top") m_DockControl.TabStripPosition = Alt.GUI.Temporary.Gwen.Pos.Top;
            if (rc.SelectedLabel == "Bottom") m_DockControl.TabStripPosition = Alt.GUI.Temporary.Gwen.Pos.Bottom;
            if (rc.SelectedLabel == "Left") m_DockControl.TabStripPosition = Alt.GUI.Temporary.Gwen.Pos.Left;
            if (rc.SelectedLabel == "Right") m_DockControl.TabStripPosition = Alt.GUI.Temporary.Gwen.Pos.Right;
        }
    }
}
