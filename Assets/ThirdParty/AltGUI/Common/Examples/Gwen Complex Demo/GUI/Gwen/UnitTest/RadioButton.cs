//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_RadioButton : GUnit
    {
        public GUnit_RadioButton(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

			Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup rbg = new Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup(this, "Sample radio group");
            rbg.TextColor = Color.White;
            rbg.SetPosition(10, 10);

            rbg.AddOption("Option 1");
            rbg.AddOption("Option 2");
            rbg.AddOption("Option 3");
            rbg.AddOption("\u0627\u0644\u0622\u0646 \u0644\u062D\u0636\u0648\u0631");
            //rbg.SizeToContents(); // it's auto

            rbg.SelectionChanged += OnChange;

            LabeledRadioButton rb1 = new LabeledRadioButton(this);
            rb1.Text = "Option 1";
            rb1.SetPosition(300, 10);

            LabeledRadioButton rb2 = new LabeledRadioButton(this);
            rb2.Text = "Option 2222222222222222222222222222222222";
            rb2.SetPosition(300, 30);

            LabeledRadioButton rb3 = new LabeledRadioButton(this);
            rb3.Text = "\u0627\u0644\u0622\u0646 \u0644\u062D\u0636\u0648\u0631";
            rb3.SetPosition(300, 50);

            //this.DrawDebugOutlines = true;
        }

        void OnChange(Base control)
        {
            RadioButtonGroup rbc = control as RadioButtonGroup;
            LabeledRadioButton rb = rbc.Selected;
            UnitPrint(String.Format("RadioButton: SelectionChanged: {0}", rb.Text));
        }
    }
}
