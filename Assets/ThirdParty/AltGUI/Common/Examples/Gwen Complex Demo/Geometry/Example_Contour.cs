//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;
using Alt.Sketch.Geometries;
using Alt.Sketch.Geometries.Reformers;
using Alt.Sketch.Geometries.Transformers;


namespace Alt.GUI.Demo
{
    class Example_Contour : Example__Base
    {
        class SpecialContourGeometry : GeometryReformer
        {
            GeometryVertexCommandAndFlags m_Flag;


            public SpecialContourGeometry(Geometry geometry, GeometryVertexCommandAndFlags flag)
                : base(geometry)
            {
                m_Flag = flag;
            }


            public override GeometryVertexCommandAndFlags GetNextVertex(out double x, out double y)
            {
                GeometryVertexCommandAndFlags cmd = base.GetNextVertex(out x, out y);

                if (IsClose(cmd))
                {
                    return ClearOrientation(cmd) | m_Flag;
                }

                return cmd;
            }
        }



		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_WidthSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_WidthSliderLabel;
        const string m_WidthSliderLabelText = "Width = ";
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_AutoDetectCheckBox;
		Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup m_PolyCloseTypeGroupBox;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbClose;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbCloseCW;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbCloseCCW;

        Brush m_FillBrush;
        Pen m_ContourPen;


        public Example_Contour(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            //  Top Label
			Alt.GUI.Temporary.Gwen.Control.Label m_TopLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_TopLabel.AutoSizeToContents = true;
            m_TopLabel.Text = "Contour Tool (GeometryContour) & Polygon Orientation";
            m_TopLabel.TextColor = Color.Yellow;
            m_TopLabel.Dock = Pos.Top;
            m_TopLabel.Margin = new Margin(0, 3, 0, 5);


            //  Width
			m_WidthSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_WidthSlider.Dock = Pos.Bottom;
            m_WidthSlider.SetSize(150, 20);
            m_WidthSlider.SetRange(-100, 100);
            m_WidthSlider.NotchCount = (int)(m_WidthSlider.Max - m_WidthSlider.Min);
            m_WidthSlider.SnapToNotches = true;
            m_WidthSlider.ValueChanged += WidthSlider_ValueChanged;

			m_WidthSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_WidthSliderLabel.AutoSizeToContents = true;
            m_WidthSliderLabel.Dock = Pos.Bottom;
            m_WidthSliderLabel.Margin = new Margin(0, 10, 0, 0);

            m_WidthSlider.Value = 0;


            //  AutoDetect
			m_AutoDetectCheckBox = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(this);
            m_AutoDetectCheckBox.Text = "Autodetect orientation if not defined";
            m_AutoDetectCheckBox.Dock = Pos.Bottom;
            m_AutoDetectCheckBox.IsChecked = true;
            m_AutoDetectCheckBox.Margin = new Margin(0, 10, 0, 0);


            //  PolyCloseType
			m_PolyCloseTypeGroupBox = new Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup(this, string.Empty);
            m_PolyCloseTypeGroupBox.Text = "Polygon Close Type:";
            m_PolyCloseTypeGroupBox.Margin = new Margin(0, 0, 0, 0);
            m_PolyCloseTypeGroupBox.Dock = Pos.Bottom;
            m_PolyCloseTypeGroupBox.AutoSizeToContents = true;

            rbClose = m_PolyCloseTypeGroupBox.AddOption("Close");
            rbClose.Margin = new Margin(0, 5, 0, 0);
            rbCloseCW = m_PolyCloseTypeGroupBox.AddOption("Close CW");
            rbCloseCCW = m_PolyCloseTypeGroupBox.AddOption("Close CCW");

            rbClose.Select();
            
            
            //
            m_FillBrush = new SolidColorBrush(Color.LimeGreen * 1.2);
            m_ContourPen = new Pen(new ColorR(0.8, 0.2, 0.2), 2);
        }


        void WidthSlider_ValueChanged(Base control)
        {
            m_WidthSliderLabel.Text = m_WidthSliderLabelText + ((int)m_WidthSlider.Value).ToString();
        }


        protected override void OnPaint(GUI.PaintEventArgs e)
        {
            base.OnPaint(e);


            Graphics graphics = e.Graphics;

            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;


            Geometry outlinePath = GeometryMatrix4Transformer.
                CreateOptimizedTransformer(Common.GraphicsPath_AltSketch,
                Matrix4.CreateScaling(0.9 * ClientWidth / Common.GraphicsPath_AltSketch_Bounds.Width));
            Rect outlinePathBounds = outlinePath.Bounds;
            Point textOffset = ((ClientSize - outlinePathBounds.Size) / 2).ToPoint();

            graphics.TranslateTransform(textOffset.X, textOffset.Y - outlinePathBounds.Top);


            GeometryVertexCommandAndFlags flag = GeometryVertexCommandAndFlags.None;
            if (rbCloseCW.IsChecked)
            {
                flag = GeometryVertexCommandAndFlags.FlagCW;
            }
            else if (rbCloseCCW.IsChecked)
            {
                flag = GeometryVertexCommandAndFlags.FlagCCW;
            }

            SpecialContourGeometry specialContourGeometry = new SpecialContourGeometry(outlinePath, flag);
            FlattenCurveGeometry curve = new FlattenCurveGeometry(specialContourGeometry);
			GeometryContour contour = new GeometryContour(curve);

            contour.Width = m_WidthSlider.Value;
            contour.AutoDetectOrientation = m_AutoDetectCheckBox.IsChecked;


            graphics.FillGeometry(m_FillBrush, contour);
            graphics.DrawGeometry(m_ContourPen, contour);
        }
    }
}
