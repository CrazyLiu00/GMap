//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
    class Example_TextOutline : Example__Base
    {
		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_OutlineThicknessSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_OutlineThicknessSliderLabel;
        const string m_OutlineThicknessSliderLabelText = "Outline Thickness = ";

        Brush m_ShadowBrush1;
        Brush m_FillBrush1;
        Pen m_ContourPen1;
        Brush m_ShadowBrush2;
        Brush m_FillBrush2;
        Pen m_ContourPen2;


        public Example_TextOutline(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            //  OutlineThicknessSlider
            m_OutlineThicknessSlider = new HorizontalSlider(this);
            m_OutlineThicknessSlider.Dock = Pos.Bottom;
            m_OutlineThicknessSlider.SetSize(150, 20);
            m_OutlineThicknessSlider.SetRange(0.5f, 3);
            m_OutlineThicknessSlider.ValueChanged += OutlineThicknessSlider_ValueChanged;

			m_OutlineThicknessSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_OutlineThicknessSliderLabel.AutoSizeToContents = true;
            m_OutlineThicknessSliderLabel.Dock = Pos.Bottom;
            m_OutlineThicknessSliderLabel.Margin = new Margin(0, 3, 0, 0);

            m_OutlineThicknessSlider.Value = 1;


            //m_ShadowBrush1 = new SolidColorBrush(Color.FromArgb(192, Color.Red));
            m_ShadowBrush1 = Brushes.LightBlue;
            m_FillBrush1 = Brushes.White;
            m_ContourPen1 = Pens.Black;

            int a = 128;
            m_ShadowBrush2 = new SolidColorBrush(Color.FromArgb(a, Color.DarkRed));
            m_FillBrush2 = new SolidColorBrush(Color.FromArgb(a, Color.DodgerBlue));
            m_ContourPen2 = new Pen(Color.FromArgb(a, Color.LightGreen));
        }


        void OutlineThicknessSlider_ValueChanged(Base control)
        {
            m_OutlineThicknessSliderLabel.Text = m_OutlineThicknessSliderLabelText + m_OutlineThicknessSlider.Value.ToString("F2").Replace(',', '.');
        }


        protected override void OnPaint(GUI.PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;

            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            GraphicsPath outlinePath = Common.GraphicsPath_AltSketch;
            Rect outlinePathBounds = Common.GraphicsPath_AltSketch_Bounds;

            Point offset = new Point(-5, -5);
            Rect bounds = Rect.Union(outlinePathBounds, new Rect(offset, outlinePathBounds.Size));
            Point textOffset = ((Size - bounds.Size) / 2).ToPoint();

            graphics.TranslateTransform(textOffset.X * 0.5, textOffset.Y * 0.63);
            graphics.FillPath(m_ShadowBrush1, outlinePath);
            graphics.TranslateTransform(offset);
            graphics.FillPath(m_FillBrush1, outlinePath);
            graphics.DrawPath(m_ContourPen1.Color, outlinePath, m_OutlineThicknessSlider.Value);

            graphics.TranslateTransform(textOffset.X, textOffset.Y * 0.6);
            graphics.FillPath(m_ShadowBrush2, outlinePath);
            graphics.TranslateTransform(offset);
            graphics.FillPath(m_FillBrush2, outlinePath);
            graphics.DrawPath(m_ContourPen2.Color, outlinePath, m_OutlineThicknessSlider.Value);
        }
    }
}
