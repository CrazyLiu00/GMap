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
    class Example_PolarTransform : Example__Base
    {
		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_SpiralSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_SpiralSliderLabel;
        const string m_SpiralSliderLabelText = "Spiral = ";

		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_BaseYSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_BaseYSliderLabel;
        const string m_BaseYSliderLabelText = "Base Y = ";


		Alt.GUI.Temporary.Gwen.Control.TextBox m_TextBox;


        public Example_PolarTransform(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            //  Top Label
			Alt.GUI.Temporary.Gwen.Control.Label m_TopLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_TopLabel.AutoSizeToContents = true;
            m_TopLabel.Text = "Polar Transformer";
            m_TopLabel.TextColor = Color.Yellow;
            m_TopLabel.Dock = Pos.Top;
            m_TopLabel.Margin = new Margin(10, 3, 0, 5);


            //  Spiral
			m_SpiralSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_SpiralSlider.Dock = Pos.Bottom;
            m_SpiralSlider.SetSize(150, 20);
            m_SpiralSlider.SetRange(-0.3f, 0.2f);
            m_SpiralSlider.ValueChanged += Slider_ValueChanged;

			m_SpiralSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_SpiralSliderLabel.AutoSizeToContents = true;
            m_SpiralSliderLabel.Dock = Pos.Bottom;
            m_SpiralSliderLabel.Margin = new Margin(0, 5, 0, 0);


            //  BaseY
			m_BaseYSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_BaseYSlider.Dock = Pos.Bottom;
            m_BaseYSlider.SetSize(150, 20);
            m_BaseYSlider.SetRange(20, 200);
            m_BaseYSlider.NotchCount = (int)(m_BaseYSlider.Max - m_BaseYSlider.Min);
            m_BaseYSlider.SnapToNotches = true;
            m_BaseYSlider.ValueChanged += Slider_ValueChanged;

			m_BaseYSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_BaseYSliderLabel.AutoSizeToContents = true;
            m_BaseYSliderLabel.Dock = Pos.Bottom;
            m_BaseYSliderLabel.Margin = new Margin(0, 10, 0, 0);


            m_SpiralSlider.Value = -0.18f;
            m_BaseYSlider.Value = 60;


            //  Text
			m_TextBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(this);
            m_TextBox.SetText(Common.AltSketch_Vector_Graphics_Library);
            m_TextBox.Font = new Font("Times New Roman", 42);
            m_TextBox.TextColor = Color.Red;
            m_TextBox.SizeToContents();
            m_TextBox.Dock = Pos.Bottom;
            m_TextBox.Margin = new Margin(5, 10, 5, 10);
            m_TextBox.ReadOnly = true;

            m_TextBox.CursorPos = 9;
            m_TextBox.CursorEnd = 3;
            m_TextBox.Focus();
        }


        void Slider_ValueChanged(Base control)
        {
            m_SpiralSliderLabel.Text = m_SpiralSliderLabelText + m_SpiralSlider.Value.ToString("F2").Replace(',', '.');
            m_BaseYSliderLabel.Text = m_BaseYSliderLabelText + ((int)m_BaseYSlider.Value).ToString();
        }



        protected override void OnPaint(GUI.PaintEventArgs e)
        {
            base.OnPaint(e);


            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;


			PolarTransform polarTransform = new PolarTransform();
			polarTransform.FullCircle = -600;
			polarTransform.BaseScale = -1;
            polarTransform.SetBaseOffset(0, m_BaseYSlider.Value);
			polarTransform.SetTranslation(ClientWidth / 2.0, ClientHeight / 2.0 - 30.0);
            polarTransform.Spiral = m_SpiralSlider.Value;


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


        void DrawGeometry(Graphics graphics, PolarTransform transform, Color color, Geometry geometry)
        {
            DrawGeometry(graphics, transform, color, geometry, 1);
        }

        void DrawGeometry(Graphics graphics, PolarTransform transform, Color color, Geometry geometry, double thickness)
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

        void FillGeometry(Graphics graphics, PolarTransform transform, Color color, Geometry geometry)
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
