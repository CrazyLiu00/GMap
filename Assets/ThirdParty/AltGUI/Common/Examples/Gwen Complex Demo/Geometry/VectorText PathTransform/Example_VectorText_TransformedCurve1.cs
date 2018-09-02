//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.InteractiveGeometry;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.NETType;
using Alt.Sketch;
using Alt.Sketch.Geometries;
using Alt.Sketch.Geometries.Reformers;
using Alt.Sketch.Geometries.Transformers;


namespace Alt.GUI.Demo
{
    class Example_VectorText_TransformedCurve1 : Example__Base
    {
        public Example_VectorText_TransformedCurve1(Base parent)
            : base(parent)
        {
            Base control = new VectorText_TransformedCurve1(this);
            control.Dock = Pos.Fill;
        }
    }



    class VectorText_TransformedCurve1 : VectorText_TransformedCurve_Base
    {
		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_PointsNumberSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_PointsNumberSliderLabel;
        const string m_PointsNumberSliderLabelText = "Number of intermediate Points = ";
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_CloseCheckBox;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_PreserveXScaleCheckBox;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_FixedLenCheckBox;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_AnimateCheckBox;
        
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_FillCheckBox;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_OutlineCheckBox;
		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_OutlineThicknessSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_OutlineThicknessSliderLabel;
        const string m_OutlineThicknessSliderLabelText = "Outline Thickness = ";

        PolygonElement m_PolygonElement;

        double[] m_dx = new double[6];
        double[] m_dy = new double[6];

        bool m_IsAnimated = false;

        const int InternalOffset = 50;

        Random m_Random;

        Pen m_BSplinePen;
        Brush m_TextBrush;
        Brush m_TextBrush_Irrlicht;


        public VectorText_TransformedCurve1(Base parent)
            : base(parent)
        {
            //  Top Label
			Alt.GUI.Temporary.Gwen.Control.Label TopLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            TopLabel.AutoSizeToContents = true;
            TopLabel.Text = "Non-linear \"Along-A-Curve\" Single Path Transformer";
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

            m_OutlineThicknessSlider.Value = 0.6f;


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


            //  CloseCheckBox
			m_CloseCheckBox = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(this);
            m_CloseCheckBox.Text = "Close";
            m_CloseCheckBox.Dock = Pos.Bottom;


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

            TextUpperBaseLine = true;


            //  Graphics
            m_BSplinePen = new Pen(Color.FromArgb(150, 170, 50, 20), 2);
            m_TextBrush =
                //Brushes.White;
                new LinearGradientBrush(Point.Zero, new Point(Width, Height), Color.Yellow, Color.Cyan);


            //  Geometry
            m_PolygonElement = new PolygonElement(6, 5.0);
            AddElement(m_PolygonElement);
            InitPoly();
			

			//	start animation
			m_AnimateCheckBox.IsChecked = true;
		}


        void InitPoly()
        {
            int dx = (ClientRectangle.Width - 2 * InternalOffset) / 5;
            int dy = (ClientRectangle.Height - 2 * InternalOffset) / 5;

            PolygonElement polygon = m_PolygonElement;
            int x = ClientRectangle.Left + InternalOffset;
            int y = ClientRectangle.Top + InternalOffset;

            polygon.SetXn(0, x);
            polygon.SetYn(0, y);
            polygon.SetXn(1, (x += dx) + 20);
            polygon.SetYn(1, (y += dy) - 20);
            polygon.SetXn(2, (x += dx) - 20);
            polygon.SetYn(2, (y += dy) + 20);
            polygon.SetXn(3, (x += dx) + 20);
            polygon.SetYn(3, (y += dy) - 20);
            polygon.SetXn(4, (x += dx) - 20);
            polygon.SetYn(4, (y += dy) + 20);
            polygon.SetXn(5, (x += dx));
            polygon.SetYn(5, (y += dy));
        }


        void OutlineThicknessSlider_ValueChanged(Base control)
        {
            m_OutlineThicknessSliderLabel.Text = m_OutlineThicknessSliderLabelText + m_OutlineThicknessSlider.Value.ToString("F2").Replace(',', '.');

            Refresh();
        }

        void PointsNumberSlider_ValueChanged(Base control)
        {
            m_PointsNumberSliderLabel.Text = m_PointsNumberSliderLabelText + ((int)m_PointsNumberSlider.Value).ToString();

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
                        m_dx[i] = ((m_Random.Next() % 1000) - 500) * 0.011;
                        m_dy[i] = ((m_Random.Next() % 1000) - 500) * 0.011;
                    }
                }

                m_IsAnimated = m_AnimateCheckBox.IsChecked;
            }

            Refresh();
        }


        protected override void OnTick(double delta)
        {
            if (m_IsAnimated)
            {
                PolygonElement polygon = m_PolygonElement;
                double x, y;
                for (int i = 0; i < 6; i++)
                {
                    x = polygon.GetXn(i);
                    y = polygon.GetYn(i);
                    MovePoint(ref x, ref y, ref m_dx[i], ref m_dy[i]);
                    polygon.SetXn(i, x);
                    polygon.SetYn(i, y);
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


            PolygonElement polygon = m_PolygonElement;
            if (!polygon.IsModified)
            {
                //TEST return;
            }

            polygon.close(m_CloseCheckBox.IsChecked);
            PolylineGeometry poly = new PolylineGeometry(polygon.Polygon, true, m_CloseCheckBox.IsChecked);

            BSplineGeometry bspline = new BSplineGeometry(poly);
            bspline.InterpolationStep = 1.0 / m_PointsNumberSlider.Value;

            SinglePathTransform tcurve = new SinglePathTransform();
            tcurve.PreserveXScale = m_PreserveXScaleCheckBox.IsChecked;
            if (m_FixedLenCheckBox.IsChecked)
            {
                tcurve.BaseLength = 1120;
            }
            tcurve.AddPath(bspline);

            GeometryTransformer ftrans = new GeometryTransformer(GetCurveTransformedTextGeometry(tcurve.TotalLength).Item1, tcurve);

            if (m_FillCheckBox.IsChecked)
            {
                Brush brush = m_TextBrush;

                //  Irrlicht Renderer can't render smoothed gradients now
                if (graphics.RenderSystemName.Contains(Graphics.RSN_Irrlicht))
                {
                    if (m_TextBrush_Irrlicht == null)
                    {
                        m_TextBrush_Irrlicht = new SolidColorBrush(Color.Yellow);
                    }

                    brush = m_TextBrush_Irrlicht;
                }

                graphics.FillGeometry(brush, ftrans);
            }
            if (m_OutlineCheckBox.IsChecked)
            {
                graphics.DrawGeometry(m_FillCheckBox.IsChecked ? Color.Red : Color.White, ftrans, m_OutlineThicknessSlider.Value);
            }

            graphics.DrawGeometry(m_BSplinePen, bspline);


            base.OnPaint(e);
        }
    }
}
