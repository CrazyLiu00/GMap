//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_ProgressBar : GUnit
    {
        public GUnit_ProgressBar(Base parent) : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            {
				Alt.GUI.Temporary.Gwen.Control.ProgressBar pb = new Alt.GUI.Temporary.Gwen.Control.ProgressBar(this);
                pb.SetBounds(new RectI(110, 20, 200, 20));
                pb.Value = 0.27f;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ProgressBar pb = new Alt.GUI.Temporary.Gwen.Control.ProgressBar(this);
                pb.SetBounds(new RectI(110, 50, 200, 20));
                pb.Value = 0.66f;
                pb.Alignment = Alt.GUI.Temporary.Gwen.Pos.Right | Alt.GUI.Temporary.Gwen.Pos.CenterV;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ProgressBar pb = new Alt.GUI.Temporary.Gwen.Control.ProgressBar(this);
                pb.SetBounds(new RectI(110, 80, 200, 20));
                pb.Value = 0.88f;
                pb.Alignment = Alt.GUI.Temporary.Gwen.Pos.Left | Alt.GUI.Temporary.Gwen.Pos.CenterV;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ProgressBar pb = new Alt.GUI.Temporary.Gwen.Control.ProgressBar(this);
                pb.SetBounds(new RectI(110, 110, 200, 20));
                pb.AutoLabel = false;
                pb.Value = 0.20f;
                pb.Alignment = Alt.GUI.Temporary.Gwen.Pos.Right | Alt.GUI.Temporary.Gwen.Pos.CenterV;
                pb.SetText("40,245 MB");
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ProgressBar pb = new Alt.GUI.Temporary.Gwen.Control.ProgressBar(this);
                pb.SetBounds(new RectI(110, 140, 200, 20));
                pb.AutoLabel = false;
                pb.Value = 1.00f;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ProgressBar pb = new Alt.GUI.Temporary.Gwen.Control.ProgressBar(this);
                pb.SetBounds(new RectI(110, 170, 200, 20));
                pb.AutoLabel = false;
                pb.Value = 0.00f;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ProgressBar pb = new Alt.GUI.Temporary.Gwen.Control.ProgressBar(this);
                pb.SetBounds(new RectI(110, 200, 200, 20));
                pb.AutoLabel = false;
                pb.Value = 0.50f;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ProgressBar pb = new Alt.GUI.Temporary.Gwen.Control.ProgressBar(this);
                pb.SetBounds(new RectI(20, 20, 25, 200));
                pb.IsHorizontal = false;
                pb.Value = 0.25f;
                pb.Alignment = Alt.GUI.Temporary.Gwen.Pos.Top | Alt.GUI.Temporary.Gwen.Pos.CenterH;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ProgressBar pb = new Alt.GUI.Temporary.Gwen.Control.ProgressBar(this);
                pb.SetBounds(new RectI(50, 20, 25, 200));
                pb.IsHorizontal = false;
                pb.Value = 0.40f;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ProgressBar pb = new Alt.GUI.Temporary.Gwen.Control.ProgressBar(this);
                pb.SetBounds(new RectI(80, 20, 25, 200));
                pb.IsHorizontal = false;
                pb.Alignment = Alt.GUI.Temporary.Gwen.Pos.Bottom | Alt.GUI.Temporary.Gwen.Pos.CenterH;
                pb.Value = 0.65f;
            }
        }
    }
}
