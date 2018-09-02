//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
    class Example_GifInPictureBox : Example__Base
    {
        public Example_GifInPictureBox(Base parent)
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
