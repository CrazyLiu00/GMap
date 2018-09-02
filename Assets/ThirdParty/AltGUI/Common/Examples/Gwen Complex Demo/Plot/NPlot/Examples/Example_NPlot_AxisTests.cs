//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.NPlot;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo.NPlot
{
    class Example_NPlot_AxisTests : Example__Base
    {
        public Example_NPlot_AxisTests(Base parent)
            : base(parent)
        {
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);


            RectI boundingBox;


            LinearAxis a = new LinearAxis(0, 10);
            a.Color = Color.White;
            a.Draw(e.Graphics, new PointI(10, 10), new PointI(10, 200), out boundingBox);

            a.Reversed = true;
            a.Draw(e.Graphics, new PointI(40, 10), new PointI(40, 200), out boundingBox);

            a.SmallTickSize = 0;
            a.Draw(e.Graphics, new PointI(70, 10), new PointI(70, 200), out boundingBox);

            a.LargeTickStep = 2.5;
            a.Draw(e.Graphics, new PointI(100, 10), new PointI(100, 200), out boundingBox);

            a.NumberOfSmallTicks = 5;
            a.SmallTickSize = 2;
            a.Draw(e.Graphics, new PointI(130, 10), new PointI(130, 200), out boundingBox);

            a.AxisColor = Color.Cyan;
            a.Draw(e.Graphics, new PointI(160, 10), new PointI(160, 200), out boundingBox);

            a.TickTextColor = Color.Cyan;
            a.Draw(e.Graphics, new PointI(190, 10), new PointI(190, 200), out boundingBox);

            a.TickTextBrush = Brushes.White;
            a.AxisPen = Pens.White;
            a.Draw(e.Graphics, new PointI(220, 10), new PointI(280, 200), out boundingBox);

            a.WorldMax = 100000;
            a.WorldMin = -3;
            a.LargeTickStep = double.NaN;
            a.Draw(e.Graphics, new PointI(310, 10), new PointI(310, 200), out boundingBox);

            a.NumberFormat = "{0:0.0E+0}";
            a.Draw(e.Graphics, new PointI(360, 10), new PointI(360, 200), out boundingBox);
        }
    }
}
