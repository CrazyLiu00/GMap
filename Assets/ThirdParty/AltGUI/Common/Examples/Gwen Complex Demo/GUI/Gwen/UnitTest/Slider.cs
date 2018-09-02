//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_Slider : GUnit
    {
        public GUnit_Slider(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            {
                HorizontalSlider slider = new HorizontalSlider(this);
                slider.SetPosition(10, 10);
                slider.SetSize(150, 20);
                slider.SetRange(0, 100);
                slider.Value = 25;
                slider.ValueChanged += SliderMoved;
            }

            {
                HorizontalSlider slider = new HorizontalSlider(this);
                slider.SetPosition(10, 40);
                slider.SetSize(150, 20);
                slider.SetRange(0, 100);
                slider.Value = 20;
                slider.NotchCount = 10;
                slider.SnapToNotches = true;
                slider.ValueChanged += SliderMoved;
            }

            {
                VerticalSlider slider = new VerticalSlider(this);
                slider.SetPosition(160, 10);
                slider.SetSize(20, 200);
                slider.SetRange(0, 100);
                slider.Value = 25;
                slider.ValueChanged += SliderMoved;
            }

            {
                VerticalSlider slider = new VerticalSlider(this);
                slider.SetPosition(190, 10);
                slider.SetSize(20, 200);
                slider.SetRange(0, 100);
                slider.Value = 20;
                slider.NotchCount = 10;
                slider.SnapToNotches = true;
                slider.ValueChanged += SliderMoved;
            }
        }

        void SliderMoved(Base control)
        {
            Slider slider = control as Slider;
            UnitPrint(String.Format("Slider moved: ValueChanged: {0}", slider.Value));
        }
    }
}
