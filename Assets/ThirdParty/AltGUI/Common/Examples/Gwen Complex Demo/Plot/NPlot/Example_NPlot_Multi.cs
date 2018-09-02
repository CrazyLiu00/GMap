//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Text;

using Alt.ComponentModel;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.Demo.NPlot;


namespace Alt.GUI.Demo
{
    public class Example_NPlot_Multi : Example__Base_Multi
    {
        public Example_NPlot_Multi(Base parent)
            : base(parent)
        {
			TitlePrefix = "AltGUI.NPlot Demos: ";
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SplitterSetHValue(0.11);
        }


        protected override void RegisterExamples()
        {
            string run = null;
#if !WINDOWS_PHONE_7 && !WINDOWS_PHONE_71
            RegisterExample(run = "Multi Plot", typeof(Example_NPlot_MultiPlot));
#endif

            RegisterExample(
#if WINDOWS_PHONE_7 || WINDOWS_PHONE_71
                run =
#endif                
                "Tour", typeof(Example_NPlot_Tour));
            
            RegisterExample("Bitmap", typeof(Example_NPlot_Bitmap));
            
            RegisterExample("Axis Tests", typeof(Example_NPlot_AxisTests));

            Start(run);
        }
    }
}
