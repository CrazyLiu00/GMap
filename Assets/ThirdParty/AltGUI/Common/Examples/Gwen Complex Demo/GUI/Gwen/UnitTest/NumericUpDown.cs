//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_NumericUpDown : GUnit
    {
        public GUnit_NumericUpDown(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

			Alt.GUI.Temporary.Gwen.Control.NumericUpDown ctrl = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(this);
            ctrl.SetBounds(10, 10, 50, 20);
            ctrl.Value = 50;
            ctrl.Max = 100;
            ctrl.Min = -100;
            ctrl.ValueChanged += OnValueChanged;
        }

        void OnValueChanged(Base control)
        {
			UnitPrint(String.Format("NumericUpDown: ValueChanged: {0}", ((Alt.GUI.Temporary.Gwen.Control.NumericUpDown)control).Value));
        }
    }
}
