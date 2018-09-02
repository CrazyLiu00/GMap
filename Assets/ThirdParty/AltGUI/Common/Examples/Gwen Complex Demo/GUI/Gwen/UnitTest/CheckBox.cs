//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_CheckBox : GUnit
    {
        public GUnit_CheckBox(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

			Alt.GUI.Temporary.Gwen.Control.CheckBox check = new Alt.GUI.Temporary.Gwen.Control.CheckBox(this);
            check.SetPosition(10, 10);
            check.Checked += OnChecked;
            check.UnChecked += OnUnchecked;
            check.CheckChanged += OnCheckChanged;

			Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox labeled = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(this);
            labeled.Text = "Labeled CheckBox";
            labeled.Checked += OnChecked;
            labeled.UnChecked += OnUnchecked;
            labeled.CheckChanged += OnCheckChanged;
            Alt.GUI.Temporary.Gwen.Align.PlaceDownLeft(labeled, check, 10);

			Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox labeled2 = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(this);
            labeled2.Text = "I'm autosized";
            labeled2.SizeToChildren();
            Alt.GUI.Temporary.Gwen.Align.PlaceDownLeft(labeled2, labeled, 10);

			Alt.GUI.Temporary.Gwen.Control.CheckBox check2 = new Alt.GUI.Temporary.Gwen.Control.CheckBox(this);
            check2.IsDisabled = true;
            Alt.GUI.Temporary.Gwen.Align.PlaceDownLeft(check2, labeled2, 20);
        }


        void OnChecked(Base control)
        {
            UnitPrint("CheckBox: Checked");
        }


        void OnCheckChanged(Base control)
        {
            UnitPrint("CheckBox: CheckChanged");
        }


        void OnUnchecked(Base control)
        {
            UnitPrint("CheckBox: UnChecked");
        }
    }
}
