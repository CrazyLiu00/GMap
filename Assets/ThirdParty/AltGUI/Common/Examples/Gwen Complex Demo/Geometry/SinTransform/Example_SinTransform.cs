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
    class Example_SinTransform : Example__Base
    {
		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_AmplitudeSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_AmplitudeSliderLabel;
        const string m_AmplitudeSliderLabelText = "Amplitude = ";

		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_PeriodSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_PeriodSliderLabel;
        const string m_PeriodSliderLabelText = "Period = ";

		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_ShiftSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_ShiftSliderLabel;
        const string m_ShiftSliderLabelText = "Shift = ";


		Alt.GUI.Temporary.Gwen.Control.TextBox m_TextBox;


        public Example_SinTransform(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            //  Top Label
			Alt.GUI.Temporary.Gwen.Control.Label m_TopLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_TopLabel.AutoSizeToContents = true;
            m_TopLabel.Text = "Sin Transformer";
            m_TopLabel.TextColor = Color.Yellow;
            m_TopLabel.Dock = Pos.Top;
            m_TopLabel.Margin = new Margin(10, 3, 0, 5);


            //  ShiftX
			m_ShiftSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_ShiftSlider.Dock = Pos.Bottom;
            m_ShiftSlider.SetSize(150, 20);
            m_ShiftSlider.SetRange(0, 500);
            m_ShiftSlider.NotchCount = (int)(m_ShiftSlider.Max - m_ShiftSlider.Min);
            m_ShiftSlider.SnapToNotches = true;
            m_ShiftSlider.ValueChanged += Slider_ValueChanged;

			m_ShiftSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_ShiftSliderLabel.AutoSizeToContents = true;
            m_ShiftSliderLabel.Dock = Pos.Bottom;
            m_ShiftSliderLabel.Margin = new Margin(0, 10, 0, 0);


            //  Period
			m_PeriodSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_PeriodSlider.Dock = Pos.Bottom;
            m_PeriodSlider.SetSize(150, 20);
            m_PeriodSlider.SetRange(100, 500);
            m_PeriodSlider.NotchCount = (int)(m_PeriodSlider.Max - m_PeriodSlider.Min);
            m_PeriodSlider.SnapToNotches = true;
            m_PeriodSlider.ValueChanged += Slider_ValueChanged;

			m_PeriodSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_PeriodSliderLabel.AutoSizeToContents = true;
            m_PeriodSliderLabel.Dock = Pos.Bottom;
            m_PeriodSliderLabel.Margin = new Margin(0, 10, 0, 0);


            //  Amplitude
			m_AmplitudeSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_AmplitudeSlider.Dock = Pos.Bottom;
            m_AmplitudeSlider.SetSize(150, 20);
            m_AmplitudeSlider.SetRange(10, 100);
            m_AmplitudeSlider.NotchCount = (int)(m_AmplitudeSlider.Max - m_AmplitudeSlider.Min);
            m_AmplitudeSlider.SnapToNotches = true;
            m_AmplitudeSlider.ValueChanged += Slider_ValueChanged;

			m_AmplitudeSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_AmplitudeSliderLabel.AutoSizeToContents = true;
            m_AmplitudeSliderLabel.Dock = Pos.Bottom;
            m_AmplitudeSliderLabel.Margin = new Margin(0, 5, 0, 0);


            m_AmplitudeSlider.Value = 50;
            m_PeriodSlider.Value = 500;
            m_ShiftSlider.Value = 0;


            //  Text
			m_TextBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(this);
            m_TextBox.SetText(Common.AltSketch_Vector_Graphics_Library);
            m_TextBox.Font = new Font("Times New Roman", 42);
            m_TextBox.TextColor = Color.OrangeRed;
            //m_TextBox.SelectionColor = Color.Yellow;
            m_TextBox.SizeToContents();
            m_TextBox.Dock = Pos.Bottom;
            m_TextBox.Margin = new Margin(5, 10, 5, 10);
            m_TextBox.ReadOnly = true;

            m_TextBox.CursorPos = 19;
            m_TextBox.CursorEnd = 7;
            m_TextBox.Focus();
        }


        void Slider_ValueChanged(Base control)
        {
            m_AmplitudeSliderLabel.Text = m_AmplitudeSliderLabelText + ((int)m_AmplitudeSlider.Value).ToString();
            m_PeriodSliderLabel.Text = m_PeriodSliderLabelText + ((int)m_PeriodSlider.Value).ToString();
            m_ShiftSliderLabel.Text = m_ShiftSliderLabelText + ((int)m_ShiftSlider.Value).ToString();
        }



        protected override void OnPaint(GUI.PaintEventArgs e)
        {
            base.OnPaint(e);


            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;


			SinTransform polarTransform = new SinTransform();
            polarTransform.SetSinScale(m_PeriodSlider.Value, m_AmplitudeSlider.Value);
            polarTransform.Shift = m_ShiftSlider.Value;
            polarTransform.SetTranslation((ClientWidth - m_TextBox.Width) / 2.0, (ClientHeight - m_TextBox.Height) / 2.0 - 10.0);


            //  Draw Background
            RectangleGeometry borderGeometry = new RectangleGeometry(0, 0, m_TextBox.Width, m_TextBox.Height);
            //  Just for little optimization
            GeometrySegmentator borderGeometrySegmentator = new GeometrySegmentator(borderGeometry);
            GeometryTransformer borderGeometrySegmentatorPolarTransformer = new GeometryTransformer(borderGeometrySegmentator, polarTransform);

            graphics.FillGeometry(Color.WhiteSmoke, borderGeometrySegmentatorPolarTransformer);


            //  Selection
            if (m_TextBox.HasSelectionAndVisible)
            {
                RectangleGeometry selectionGeometry = new RectangleGeometry(m_TextBox.SelectionBounds
                    //  Just to prevent some artefacts
                    - new PointI(0, 1) + new SizeI(0, 2));

                FillGeometry(graphics, polarTransform, m_TextBox.SelectionColor, selectionGeometry);
            }


            //  Caret
            if (m_TextBox.IsCaretVisible)
            {
                RectangleGeometry caretGeometry = new RectangleGeometry(m_TextBox.CaretBounds);

                FillGeometry(graphics, polarTransform, Color.Black, caretGeometry);
            }


            //  Draw Border
            graphics.DrawGeometry(Color.DodgerBlue, borderGeometrySegmentatorPolarTransformer, 2);


            //  Text
            Font font = m_TextBox.Font;
            GraphicsPath textGeometry = new GraphicsPath();
            textGeometry.AddString(
                m_TextBox.Text,
                font.FontFamily, font.Style, font.Size * 96.0 / 72.0, new Point(4, 1), StringFormat.GenericTypographic);
            FlattenCurveGeometry textFlattenGeometry = new FlattenCurveGeometry(textGeometry);
            //  Need for clip text
            Geometry clippedTextFlattenGeometry = new FlattenCurveGeometry(
                new CombinedGeometry(textFlattenGeometry, new RectangleGeometry(1, 1, m_TextBox.Width - 2, m_TextBox.Height - 2), CombinedGeometry.CombineMode.And));

            FillGeometry(graphics, polarTransform, m_TextBox.TextColor, clippedTextFlattenGeometry);
        }


        void DrawGeometry(Graphics graphics, SinTransform transform, Color color, Geometry geometry)
        {
            DrawGeometry(graphics, transform, color, geometry, 1);
        }

        void DrawGeometry(Graphics graphics, SinTransform transform, Color color, Geometry geometry, double thickness)
        {
            if (graphics == null ||
                geometry == null)
            {
                return;
            }

            GeometrySegmentator geometrySegmentator = new GeometrySegmentator(geometry);
            GeometryTransformer geometrySegmentatorPolarTransformer = new GeometryTransformer(geometrySegmentator, transform);

            graphics.DrawGeometry(color, geometrySegmentatorPolarTransformer, thickness);
        }

        void FillGeometry(Graphics graphics, SinTransform transform, Color color, Geometry geometry)
        {
            if (graphics == null ||
                geometry == null)
            {
                return;
            }

            GeometrySegmentator geometrySegmentator = new GeometrySegmentator(geometry);
            GeometryTransformer geometrySegmentatorPolarTransformer = new GeometryTransformer(geometrySegmentator, transform);

            graphics.FillGeometry(color, geometrySegmentatorPolarTransformer);
        }
    }
}
