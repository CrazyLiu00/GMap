//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_ImagePanel : GUnit
    {
        public GUnit_ImagePanel(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Normal
            {
                ImagePanel img = new ImagePanel(this);
                img.ImageName = "AltData/Gwen/gwen.gif";
                img.SetPosition(10, 10);
                img.SetBounds(10, 10, 100, 100);
            }

            // Missing
            {
                ImagePanel img = new ImagePanel(this);
                img.ImageName = "missingimage.png";
                img.SetPosition(120, 10);
                img.SetBounds(120, 10, 100, 100);
            }
        }
    }
}
