//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

#if SILVERLIGHT || UNITY_WEBPLAYER
using System;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo
{
    class Example_GMap_BigMapMaker_NotAvailable : Example_NotAvailable_ScreenShot
    {
        protected override string Description
        {
            get
            {
                return "GMap.NET Big Map Maker";
            }
        }


        protected override Alt.Sketch.Bitmap Screenshot
        {
            get
            {
                return LoadImage("Example_GMap_BigMapMaker.jpg");
            }
        }


        public Example_GMap_BigMapMaker_NotAvailable(Base parent)
            : base(parent)
        {
        }
    }
}
#endif