//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

#if !UNITY_55
using System;
using System.Reflection;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;
using Alt.Sketch.Svg;
using Alt.Sketch.Svg.Transforms;


namespace Alt.GUI.Demo
{
    class Example_ExtBrush : Example__Base
    {
        class ExtBrushPanel : Base
        {
            Brush m_MaterialBrush;
            Pen m_ContourPen;
            Color m_ColorMultiplier;

            public bool UseColorMultiplier = true;


            public ExtBrushPanel(Base parent, Brush brush, Pen contourPen) :
                this(parent, brush, contourPen, Color.White)
            {
            }

            public ExtBrushPanel(Base parent, Brush brush, Pen contourPen, Color colorMultiplier)
                : base(parent)
            {
                m_MaterialBrush = brush;
                m_ContourPen = contourPen;
                m_ColorMultiplier = colorMultiplier;
            }


            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);


                Graphics graphics = e.Graphics;
                if (graphics is SoftwareGraphics)
                {
                    return;
                }


                graphics.SmoothingMode = SmoothingMode.None;

                Rect rect = this.ClientRectangle;
                rect.Deflate(20, 20);

                graphics.FillRoundedRectangle(m_MaterialBrush, rect, 20, UseColorMultiplier ? m_ColorMultiplier : Color.White);

                if (m_ContourPen != null)
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    graphics.DrawRoundedRectangle(m_ContourPen, rect, 20);
                }
            }
        }


        ExtBrushPanel m_ExtBrushPanel1;
        ExtBrushPanel m_ExtBrushPanel2;
        ExtBrushPanel m_ExtBrushPanel3;
        ExtBrushPanel m_ExtBrushPanel4;


        ExtBrush m_MaterialBrush1;
        ExtBrush m_MaterialBrush2;
        ExtBrush m_MaterialBrush3;
        ExtBrush m_MaterialBrush4;


        public Example_ExtBrush(Base parent)
            : base(parent)
        {
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            Bitmap image = Bitmap.FromFile("AltData/About.gif");

            m_MaterialBrush1 = new ExtBrush(); m_MaterialBrush1.MaterialName = "Material1"; m_MaterialBrush1.SetExtParameter("Image", image);
            m_MaterialBrush2 = new ExtBrush(); m_MaterialBrush2.MaterialName = "Material2"; m_MaterialBrush2.SetExtParameter("Image", image);
            m_MaterialBrush3 = new ExtBrush(); m_MaterialBrush3.MaterialName = "Material3"; m_MaterialBrush3.SetExtParameter("Image", image);
            m_MaterialBrush4 = new ExtBrush(); m_MaterialBrush4.MaterialName = "Material4"; m_MaterialBrush4.SetExtParameter("Image", image);


            //
            CrossSplitter splitter = new CrossSplitter(this);
            splitter.Dock = Pos.Fill;

            Pen contourPen = new Pen(Color.DodgerBlue, 3);

            ExtBrushPanel panel = m_ExtBrushPanel1 = new ExtBrushPanel(splitter, m_MaterialBrush1, contourPen, Color.LightCoral);
            panel.Dock = Pos.Fill;
            splitter.SetPanel(0, panel);

            panel = m_ExtBrushPanel2 = new ExtBrushPanel(splitter, m_MaterialBrush2, contourPen, Color.Green);
            panel.Dock = Pos.Fill;
            splitter.SetPanel(2, panel);

            panel = m_ExtBrushPanel3 = new ExtBrushPanel(splitter, m_MaterialBrush3, contourPen, Color.Red);
            panel.Dock = Pos.Fill;
            splitter.SetPanel(1, panel);

            panel = m_ExtBrushPanel4 = new ExtBrushPanel(splitter, m_MaterialBrush4, contourPen, Color.Cyan);
            panel.Dock = Pos.Fill;
            splitter.SetPanel(3, panel);
        }


        Font m_SoftwareGraphicsInfoFont;
        Brush m_SoftwareGraphicsInfoBrush;
        const string m_SoftwareGraphicsInfo = "ExtBrush example has effect only in HW Render";
        const string m_SoftwareGraphicsInfo2 = "(ExtBrushMaterial must be implemented)";
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (e.Graphics is SoftwareGraphics)
            {
                if (m_SoftwareGraphicsInfoFont == null)
                {
                    m_SoftwareGraphicsInfoFont = new Font("Arial", 14, FontStyle.Bold);
                    m_SoftwareGraphicsInfoBrush = new SolidColorBrush(Color.FromArgb(255, Color.Red * 0.8));
                }

                SizeI size = e.Graphics.MeasureString(m_SoftwareGraphicsInfo, m_SoftwareGraphicsInfoFont).ToSizeI();
                e.Graphics.DrawString(m_SoftwareGraphicsInfo, m_SoftwareGraphicsInfoFont, m_SoftwareGraphicsInfoBrush,
                    new Point((Width - size.Width) / 2, (Height - size.Height) / 2 - 50));

                size = e.Graphics.MeasureString(m_SoftwareGraphicsInfo2, m_SoftwareGraphicsInfoFont).ToSizeI();
                e.Graphics.DrawString(m_SoftwareGraphicsInfo2, m_SoftwareGraphicsInfoFont, m_SoftwareGraphicsInfoBrush,
                    new Point((Width - size.Width) / 2, (Height - size.Height) / 2 - 50 + m_SoftwareGraphicsInfoFont.Height));
            }
        }


        protected override void Render(Alt.GUI.Temporary.Gwen.Skin.Base skin)
        {
            bool useColorMultiplier = skin.Renderer.Graphics.RenderSystemName != Graphics.RSN_Axiom &&
                skin.Renderer.Graphics.RenderSystemName != Graphics.RSN_OGRE;

            m_ExtBrushPanel1.UseColorMultiplier = useColorMultiplier;
            m_ExtBrushPanel2.UseColorMultiplier = useColorMultiplier;
            m_ExtBrushPanel3.UseColorMultiplier = useColorMultiplier;
            m_ExtBrushPanel4.UseColorMultiplier = useColorMultiplier;

            base.Render(skin);
        }
    }
}
#endif