//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_RichLabel : GUnit
    {
        Font f1, f2, f3;


        public GUnit_RichLabel(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            RichLabel label = new RichLabel(this);
            label.SetBounds(10, 10, 400, 200);

            f1 = new Font("Arial", 15);
            label.AddText("This test uses Arial 15, White. Padding. ", Color.White, f1);

            f2 = new Font("Times New Roman Bold", 20);
            label.AddText("This text uses Times New Roman Bold 20, Pink. Padding. ", Color.Pink, f2);

            f3 = new Font("Courier New Italic", 15);
            label.AddText("This text uses Courier New Italic 15, Yellow. Padding. ", Color.Yellow, f3);

            label.AddLineBreak();

            label.AddText("This test uses Arial 15, Cyan. Padding. ", Color.Cyan * 1.2, f1);
        }


        public override void Dispose()
        {
            if (f1 != null)
            {
                f1.Dispose();
                f1 = null;
            }

            if (f2 != null)
            {
                f2.Dispose();
                f2 = null;
            }

            if (f3 != null)
            {
                f3.Dispose();
                f3 = null;
            }

            base.Dispose();
        }
    }
}
