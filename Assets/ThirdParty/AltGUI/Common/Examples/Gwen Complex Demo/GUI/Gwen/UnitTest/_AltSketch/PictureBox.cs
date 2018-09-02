//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_PictureBox : GUnit
    {
        public GUnit_PictureBox(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

			Alt.GUI.Temporary.Gwen.Control.PictureBox box = new Alt.GUI.Temporary.Gwen.Control.PictureBox(this);
            box.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
			box.SizeMode = PictureBoxSizeMode.CenterImage;
            box.Image = Bitmap.FromFile("AltData/corazon.gif");
        }
    }
}
