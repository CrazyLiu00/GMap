//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.NPlot;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo.NPlot
{
    class Example_NPlot_Bitmap : Example__Base
    {
        PlotSurface2D m_PlotSurface2D;
        NPlotBitmap m_NPlotBitmap;


        readonly Color plotSurface_AxisColor_Default = Color.Black;
        readonly Color plotSurface_TickTextColor_Default = Color.WhiteSmoke;
        readonly Color plotSurface_Label_Default = Color.WhiteSmoke;


        public Example_NPlot_Bitmap(Base parent)
            : base(parent)
        {
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            m_PlotSurface2D = new PlotSurface2D();

            m_PlotSurface2D.Clear();
            Example_NPlot_Tour.
#if WINDOWS_PHONE_7 || WINDOWS_PHONE_71
                PlotParticles
#else
                PlotDataSet//PlotWave
#endif
                (m_PlotSurface2D);

            m_PlotSurface2D.TitleColor = plotSurface_Label_Default;

            m_PlotSurface2D.XAxis1.AxisColor = plotSurface_AxisColor_Default;
            m_PlotSurface2D.XAxis1.TickTextColor = plotSurface_TickTextColor_Default;
            m_PlotSurface2D.XAxis1.LabelColor = plotSurface_Label_Default;

            m_PlotSurface2D.YAxis1.AxisColor = plotSurface_AxisColor_Default;
            m_PlotSurface2D.YAxis1.TickTextColor = plotSurface_TickTextColor_Default;
            m_PlotSurface2D.YAxis1.LabelColor = plotSurface_Label_Default;

            m_NPlotBitmap = new NPlotBitmap(m_PlotSurface2D);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int margin = 20;
            m_NPlotBitmap.Size = new SizeI(Width - margin * 2, Height - margin * 2);

            e.Graphics.DrawImage(m_NPlotBitmap.Bitmap, new PointI(margin, margin));
        }
    }
}
