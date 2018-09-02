//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.NETType;
using Alt.GUI.InteractiveGeometry;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;
using Alt.Sketch.Geometries;
using Alt.Sketch.Geometries.Reformers;
using Alt.Sketch.Geometries.Transformers;


namespace Alt.GUI.Demo
{
    class Example_VectorText_TransformedCurve2 : Example__Base
    {
        public Example_VectorText_TransformedCurve2(Base parent)
            : base(parent)
        {
            Base control = new VectorText_TransformedCurve2(this);
            control.Dock = Pos.Fill;
        }
    }



    class VectorText_TransformedCurve2 : VectorText_TransformedCurve_Base
    {
		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_PointsNumberSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_PointsNumberSliderLabel;
        string m_PointsNumberSliderText = "Number of intermediate Points = ";
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_PreserveXScaleCheckBox;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_FixedLenCheckBox;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_AnimateCheckBox;

		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_FillCheckBox;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_OutlineCheckBox;
		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_OutlineThicknessSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_OutlineThicknessSliderLabel;
        const string m_OutlineThicknessSliderLabelText = "Outline Thickness = ";

        PolygonElement m_PolygonElement1;
        PolygonElement m_PolygonElement2;

        double[] m_dx1 = new double[6];
        double[] m_dy1 = new double[6];

        double[] m_dx2 = new double[6];
        double[] m_dy2 = new double[6];


        bool m_IsAnimated = false;

        const int InternalOffset = 50;

        Random m_Random;

        Pen m_BSplinePen;
        Brush m_TextBrush;
        Brush m_TextBrush_Irrlicht;


        public VectorText_TransformedCurve2(Base parent)
            : base(parent)
        {
            //  Top Label
			Alt.GUI.Temporary.Gwen.Control.Label TopLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            TopLabel.AutoSizeToContents = true;
            TopLabel.Text = "Non-linear \"Along-A-Curve\" Double Path Transformer";
            TopLabel.TextColor = Color.Yellow;
            TopLabel.Dock = Pos.Top;
            TopLabel.Margin = new Margin(0, 3, 0, 5);


            //  OutlineThicknessSlider
			m_OutlineThicknessSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_OutlineThicknessSlider.Dock = Pos.Bottom;
            m_OutlineThicknessSlider.SetSize(150, 20);
            m_OutlineThicknessSlider.SetRange(0.5f, 3);
            m_OutlineThicknessSlider.ValueChanged += OutlineThicknessSlider_ValueChanged;

			m_OutlineThicknessSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_OutlineThicknessSliderLabel.AutoSizeToContents = true;
            m_OutlineThicknessSliderLabel.Dock = Pos.Bottom;
            m_OutlineThicknessSliderLabel.Margin = new Margin(0, 3, 0, 0);

            m_OutlineThicknessSlider.Value = 0.7f;


            //  OutlineCheckBox
			m_OutlineCheckBox = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(this);
            m_OutlineCheckBox.Text = "Outline";
            m_OutlineCheckBox.Dock = Pos.Bottom;


            //  FillCheckBox
			m_FillCheckBox = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(this);
            m_FillCheckBox.Text = "Fill";
            m_FillCheckBox.Dock = Pos.Bottom;
            m_FillCheckBox.IsChecked = true;

            

            //  PointsNumberSlider
			m_PointsNumberSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_PointsNumberSlider.Dock = Pos.Bottom;
            m_PointsNumberSlider.SetSize(150, 20);
            m_PointsNumberSlider.SetRange(10, 400);
            m_PointsNumberSlider.NotchCount = (int)(m_PointsNumberSlider.Max - m_PointsNumberSlider.Min);
            m_PointsNumberSlider.SnapToNotches = true;
            m_PointsNumberSlider.ValueChanged += PointsNumberSlider_ValueChanged;

			m_PointsNumberSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_PointsNumberSliderLabel.AutoSizeToContents = true;
            m_PointsNumberSliderLabel.Dock = Pos.Bottom;
            m_PointsNumberSliderLabel.Margin = new Margin(0, 3, 0, 0);

            m_PointsNumberSlider.Value = 200;


            //  PreserveXScaleCheckBox
			m_PreserveXScaleCheckBox = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(this);
            m_PreserveXScaleCheckBox.Text = "Preserve X scale";
            m_PreserveXScaleCheckBox.Dock = Pos.Bottom;
            m_PreserveXScaleCheckBox.IsChecked = true;


            //  FixedLenCheckBox
			m_FixedLenCheckBox = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(this);
            m_FixedLenCheckBox.Text = "Fixed Length";
            m_FixedLenCheckBox.Dock = Pos.Bottom;


            //  AnimateCheckBox
			m_AnimateCheckBox = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(this);
            m_AnimateCheckBox.Text = "Animate";
            m_AnimateCheckBox.Dock = Pos.Bottom;
            m_AnimateCheckBox.CheckChanged += new GwenEventHandler(AnimateCheckBox_CheckChanged);
            m_AnimateCheckBox.Margin = new Margin(0, 7, 0, 0);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            //  Graphics
            m_BSplinePen = new Pen(Color.Maroon, 2);
            m_TextBrush =
                //Brushes.White;
                new LinearGradientBrush(Point.Zero, new Point(Width, Height), Color.Red, Color.LawnGreen);


            //  Geometry
            m_PolygonElement1 = new PolygonElement(6, 5.0);
            m_PolygonElement1.close(false);
            AddElement(m_PolygonElement1);

            m_PolygonElement2 = new PolygonElement(6, 5.0);
            m_PolygonElement2.close(false);
            AddElement(m_PolygonElement2);

            InitPoly();
        }


        void InitPoly()
        {
            int dx = (ClientRectangle.Width - 2 * InternalOffset) / 5;
            int dy = (ClientRectangle.Height - 2 * InternalOffset) / 5;

            PolygonElement polygon = m_PolygonElement1;
            int x = ClientRectangle.Left + InternalOffset;
            int y = ClientRectangle.Top + InternalOffset;

            int k1 = 30;
            int k2 = 20;

            polygon.SetXn(0, x + k1);
            polygon.SetYn(0, y - k1);
            polygon.SetXn(1, (x += dx) + k1 + k2);
            polygon.SetYn(1, (y += dy) - k1 - k2);
            polygon.SetXn(2, (x += dx) + k1 - k2);
            polygon.SetYn(2, (y += dy) - k1 + k2);
            polygon.SetXn(3, (x += dx) + k1 + k2);
            polygon.SetYn(3, (y += dy) - k1 - k2);
            polygon.SetXn(4, (x += dx) + k1 - k2);
            polygon.SetYn(4, (y += dy) - k1 + k2);
            polygon.SetXn(5, (x += dx) + k1);
            polygon.SetYn(5, (y += dy) - k1);

            polygon = m_PolygonElement2;
            x = ClientRectangle.Left + InternalOffset;
            y = ClientRectangle.Top + InternalOffset;

            polygon.SetXn(0, x - k1);
            polygon.SetYn(0, y + k1);
            polygon.SetXn(1, (x += dx) - k1 + k2);
            polygon.SetYn(1, (y += dy) + k1 - k2);
            polygon.SetXn(2, (x += dx) - k1 - k2);
            polygon.SetYn(2, (y += dy) + k1 + k2);
            polygon.SetXn(3, (x += dx) - k1 + k2);
            polygon.SetYn(3, (y += dy) + k1 - k2);
            polygon.SetXn(4, (x += dx) - k1 - k2);
            polygon.SetYn(4, (y += dy) + k1 + k2);
            polygon.SetXn(5, (x += dx) - k1);
            polygon.SetYn(5, (y += dy) + k1);
        }


        void OutlineThicknessSlider_ValueChanged(Base control)
        {
            m_OutlineThicknessSliderLabel.Text = m_OutlineThicknessSliderLabelText + m_OutlineThicknessSlider.Value.ToString("F2").Replace(',', '.');

            Refresh();
        }

        void PointsNumberSlider_ValueChanged(Base control)
        {
            m_PointsNumberSliderLabel.Text = m_PointsNumberSliderText + ((int)m_PointsNumberSlider.Value).ToString();

            Refresh();
        }


        void AnimateCheckBox_CheckChanged(Base control)
        {
            if (m_AnimateCheckBox.IsChecked != m_IsAnimated)
            {
                if (m_AnimateCheckBox.IsChecked)
                {
                    InitPoly();

                    if (m_Random == null)
                    {
                        m_Random = new Random();
                    }

                    int i;
                    for (i = 0; i < 6; i++)
                    {
                        m_dx1[i] = ((m_Random.Next() % 1000) - 500) * 0.01;
                        m_dy1[i] = ((m_Random.Next() % 1000) - 500) * 0.01;
                        m_dx2[i] = ((m_Random.Next() % 1000) - 500) * 0.01;
                        m_dy2[i] = ((m_Random.Next() % 1000) - 500) * 0.01;
                    }
                }

                m_IsAnimated = m_AnimateCheckBox.IsChecked;
            }

            Refresh();
        }


        void NormalizePoint(int i)
        {
            PolygonElement polygon1 = m_PolygonElement1;
            PolygonElement polygon2 = m_PolygonElement2;

            double d = MathHelper.Distance(polygon1.GetXn(i), polygon1.GetYn(i), polygon2.GetXn(i), polygon2.GetYn(i));
            double k = 20 * System.Math.Sqrt(2) * 3;
            if (d > k)
            {
                polygon2.SetXn(i, polygon1.GetXn(i) + (polygon2.GetXn(i) - polygon1.GetXn(i)) * k / d);
                polygon2.SetYn(i, polygon1.GetYn(i) + (polygon2.GetYn(i) - polygon1.GetYn(i)) * k / d);
            }
        }


        protected override void OnTick(double delta)
        {
            if (m_IsAnimated)
            {
                PolygonElement polygon1 = m_PolygonElement1;
                PolygonElement polygon2 = m_PolygonElement2;
                double x, y;
                for (int i = 0; i < 6; i++)
                {
                    x = polygon1.GetXn(i);
                    y = polygon1.GetYn(i);
                    MovePoint(ref x, ref y, ref m_dx1[i], ref m_dy1[i]);
                    polygon1.SetXn(i, x);
                    polygon1.SetYn(i, y);

                    x = polygon2.GetXn(i);
                    y = polygon2.GetYn(i);
                    MovePoint(ref x, ref y, ref m_dx2[i], ref m_dy2[i]);
                    polygon2.SetXn(i, x);
                    polygon2.SetYn(i, y);

                    NormalizePoint(i);
                }

                Refresh();
            }

            base.OnTick(delta);
        }


        void MovePoint(ref double x, ref double y, ref double dx, ref double dy)
        {
            int left = ClientRectangle.Left + InternalOffset;
            int right = ClientRectangle.Right - InternalOffset;
            int top = ClientRectangle.Top + InternalOffset;
            int bottom = ClientRectangle.Bottom - InternalOffset;

            if (x < left)
            {
                x = left;
                dx = -dx;
            }

            if (x > right)
            {
                x = right;
                dx = -dx;
            }

            if (y < top)
            {
                y = top;
                dy = -dy;
            }

            if (y > bottom)
            {
                y = bottom;
                dy = -dy;
            }

            x += dx;
            y += dy;
        }


        protected override void OnPaint(GUI.PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.None;


            //  Polygon1
            PolygonElement polygon1 = m_PolygonElement1;

            PolylineGeometry path1 = new PolylineGeometry(polygon1.Polygon, true, false);

            BSplineGeometry bspline1 = new BSplineGeometry(path1);
            bspline1.InterpolationStep = 1.0 / m_PointsNumberSlider.Value;


            //  Polygon2
            PolygonElement polygon2 = m_PolygonElement2;

            PolylineGeometry path2 = new PolylineGeometry(polygon2.Polygon, true, false);

            BSplineGeometry bspline2 = new BSplineGeometry(path2);
            bspline1.InterpolationStep = 1.0 / m_PointsNumberSlider.Value;


            DoublePathTransform tcurve = new DoublePathTransform();
            tcurve.PreserveXScale = m_PreserveXScaleCheckBox.IsChecked;
            if (m_FixedLenCheckBox.IsChecked)
            {
                tcurve.BaseLength = 1140.0;
            }
            tcurve.AddPaths(bspline1, bspline2);

            Tuple<Geometry, double> geometry = GetCurveTransformedTextGeometry(tcurve.TotalLength1);
            tcurve.BaseHeight = FontAscentInPixels + geometry.Item2 + 3;
            
            GeometryTransformer ftrans = new GeometryTransformer(geometry.Item1, tcurve);

            if (m_FillCheckBox.IsChecked)
            {
                Brush brush = m_TextBrush;

                //  Irrlicht Renderer can't render smoothed gradients now
                if (graphics.RenderSystemName.Contains(Graphics.RSN_Irrlicht))
                {
                    if (m_TextBrush_Irrlicht == null)
                    {
                        m_TextBrush_Irrlicht = new SolidColorBrush(Color.LawnGreen);
                    }

                    brush = m_TextBrush_Irrlicht;
                }

                graphics.FillGeometry(brush, ftrans);
            }
            if (m_OutlineCheckBox.IsChecked)
            {
                graphics.DrawGeometry(Color.White, ftrans, m_OutlineThicknessSlider.Value);
            }


            graphics.DrawGeometry(m_BSplinePen, bspline1);
            graphics.DrawGeometry(m_BSplinePen, bspline2);


            base.OnPaint(e);
        }
    }
}
