//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_ColorPickers : GUnit
    {
        public GUnit_ColorPickers(Base parent) : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ColorPicker rgbPicker = new ColorPicker(this, Color.White);
            rgbPicker.SetPosition(10, 10);

            rgbPicker.ColorChanged += ColorChanged;

            HSVColorPicker hsvPicker = new HSVColorPicker(this);
            hsvPicker.SetPosition(300, 10);
            hsvPicker.ColorChanged += ColorChanged;
        }


        void ColorChanged(Base control)
        {
            IColorPicker picker = control as IColorPicker;
            Color c = picker.SelectedColor;
            Alt.GUI.Temporary.Gwen.HSV hsv = //c.ToHSV();
                Alt.GUI.Temporary.Gwen.Util.ToHSV(c);
            String text = String.Format("Color changed: RGB: {0:X2}{1:X2}{2:X2} HSV: {3:F1} {4:F2} {5:F2}",
                                        c.R, c.G, c.B, hsv.h, hsv.s, hsv.v);
            UnitPrint(text);
        }
    }
}
