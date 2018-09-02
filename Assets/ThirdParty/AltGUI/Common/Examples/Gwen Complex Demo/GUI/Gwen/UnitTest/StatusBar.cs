//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_StatusBar : GUnit
    {
        public GUnit_StatusBar(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

			Alt.GUI.Temporary.Gwen.Control.StatusBar sb = new Alt.GUI.Temporary.Gwen.Control.StatusBar(this);
			Alt.GUI.Temporary.Gwen.Control.Label left = new Alt.GUI.Temporary.Gwen.Control.Label(sb);
            left.Text = "Label added to left";
            sb.AddControl(left, false);

			Alt.GUI.Temporary.Gwen.Control.Label right = new Alt.GUI.Temporary.Gwen.Control.Label(sb);
            right.Text = "Label added to right";
            sb.AddControl(right, true);

			Alt.GUI.Temporary.Gwen.Control.Button bl = new Alt.GUI.Temporary.Gwen.Control.Button(sb);
            bl.Text = "Left button";
            sb.AddControl(bl, false);

			Alt.GUI.Temporary.Gwen.Control.Button br = new Alt.GUI.Temporary.Gwen.Control.Button(sb);
            br.Text = "Right button";
            sb.AddControl(br, true);
        }
    }
}
