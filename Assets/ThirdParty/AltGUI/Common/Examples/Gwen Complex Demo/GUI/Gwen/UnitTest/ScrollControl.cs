//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_ScrollControl : GUnit
    {
        public GUnit_ScrollControl(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            {
				Alt.GUI.Temporary.Gwen.Control.ScrollControl ctrl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(this);
                ctrl.SetBounds(10, 10, 100, 100);

				Alt.GUI.Temporary.Gwen.Control.Button pTestButton = new Alt.GUI.Temporary.Gwen.Control.Button(ctrl);
                pTestButton.SetText("Twice As Big");
                pTestButton.SetBounds(0, 0, 200, 200);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ScrollControl ctrl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(this);
                ctrl.SetBounds(110, 10, 100, 100);

				Alt.GUI.Temporary.Gwen.Control.Button pTestButton = new Alt.GUI.Temporary.Gwen.Control.Button(ctrl);
                pTestButton.SetText("Same Size");
                pTestButton.SetBounds(0, 0, 100, 100);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ScrollControl ctrl = new ScrollControl(this);
                ctrl.SetBounds(210, 10, 100, 100);

				Alt.GUI.Temporary.Gwen.Control.Button pTestButton = new Alt.GUI.Temporary.Gwen.Control.Button(ctrl);
                pTestButton.SetText("Wide");
                pTestButton.SetBounds(0, 0, 200, 50);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ScrollControl ctrl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(this);
                ctrl.SetBounds(310, 10, 100, 100);

				Alt.GUI.Temporary.Gwen.Control.Button pTestButton = new Alt.GUI.Temporary.Gwen.Control.Button(ctrl);
                pTestButton.SetText("Tall");
                pTestButton.SetBounds(0, 0, 50, 200);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ScrollControl ctrl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(this);
                ctrl.SetBounds(410, 10, 100, 100);
                ctrl.EnableScroll(false, true);

				Alt.GUI.Temporary.Gwen.Control.Button pTestButton = new Alt.GUI.Temporary.Gwen.Control.Button(ctrl);
                pTestButton.SetText("Vertical");
                pTestButton.SetBounds(0, 0, 200, 200);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ScrollControl ctrl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(this);
                ctrl.SetBounds(510, 10, 100, 100);
                ctrl.EnableScroll(true, false);

				Alt.GUI.Temporary.Gwen.Control.Button pTestButton = new Alt.GUI.Temporary.Gwen.Control.Button(ctrl);
                pTestButton.SetText("Horizontal");
                pTestButton.SetBounds(0, 0, 200, 200);
            }

            // Bottom Row

            {
				Alt.GUI.Temporary.Gwen.Control.ScrollControl ctrl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(this);
                ctrl.SetBounds(10, 110, 100, 100);
                ctrl.AutoHideBars = true;

				Alt.GUI.Temporary.Gwen.Control.Button pTestButton = new Alt.GUI.Temporary.Gwen.Control.Button(ctrl);
                pTestButton.SetText("Twice As Big");
                pTestButton.SetBounds(0, 0, 200, 200);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ScrollControl ctrl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(this);
                ctrl.SetBounds(110, 110, 100, 100);
                ctrl.AutoHideBars = true;

				Alt.GUI.Temporary.Gwen.Control.Button pTestButton = new Alt.GUI.Temporary.Gwen.Control.Button(ctrl);
                pTestButton.SetText("Same Size");
                pTestButton.SetBounds(0, 0, 100, 100);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ScrollControl ctrl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(this);
                ctrl.SetBounds(210, 110, 100, 100);
                ctrl.AutoHideBars = true;

				Alt.GUI.Temporary.Gwen.Control.Button pTestButton = new Alt.GUI.Temporary.Gwen.Control.Button(ctrl);
                pTestButton.SetText("Wide");
                pTestButton.SetBounds(0, 0, 200, 50);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ScrollControl ctrl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(this);
                ctrl.SetBounds(310, 110, 100, 100);
                ctrl.AutoHideBars = true;

				Alt.GUI.Temporary.Gwen.Control.Button pTestButton = new Alt.GUI.Temporary.Gwen.Control.Button(ctrl);
                pTestButton.SetText("Tall");
                pTestButton.SetBounds(0, 0, 50, 200);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ScrollControl ctrl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(this);
                ctrl.SetBounds(410, 110, 100, 100);
                ctrl.AutoHideBars = true;
                ctrl.EnableScroll(false, true);

				Alt.GUI.Temporary.Gwen.Control.Button pTestButton = new Alt.GUI.Temporary.Gwen.Control.Button(ctrl);
                pTestButton.SetText("Vertical");
                pTestButton.SetBounds(0, 0, 200, 200);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ScrollControl ctrl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(this);
                ctrl.SetBounds(510, 110, 100, 100);
                ctrl.AutoHideBars = true;
                ctrl.EnableScroll(true, false);

				Alt.GUI.Temporary.Gwen.Control.Button pTestButton = new Alt.GUI.Temporary.Gwen.Control.Button(ctrl);
                pTestButton.SetText("Horinzontal");
                pTestButton.SetBounds(0, 0, 200, 200);
            }
        }
    }
}
