//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.InteractiveGeometry;
using Alt.Sketch;
using Alt.Sketch.Geometries;
using Alt.Sketch.Geometries.Reformers;


namespace Alt.GUI.Demo
{
    class Example_BSpline : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int y = 0;
                if (m_BSpline_FlowGeometryContainer != null &&
                    m_BSpline_FlowGeometryContainer.m_TopLabel != null)
                {
                    y = m_BSpline_FlowGeometryContainer.m_TopLabel.Height + 5;
                }

                return new SizeI(0, y);
            }
        }


        Example_BSpline_FlowGeometryContainer m_BSpline_FlowGeometryContainer;

        
        public Example_BSpline(Base parent)
            : base(parent)
        {
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            m_BSpline_FlowGeometryContainer = new Example_BSpline_FlowGeometryContainer(this);
            m_BSpline_FlowGeometryContainer.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
        }



        class Example_BSpline_FlowGeometryContainer : InteractiveGeometryContainer
        {
			internal Alt.GUI.Temporary.Gwen.Control.Label m_TopLabel;
			Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_PointsNumberSlider;
			Alt.GUI.Temporary.Gwen.Control.Label m_PointsNumberSliderLabel;
            string m_PointsNumberSliderText = "Number of intermediate Points = ";
			Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_CloseCheckBox;
			Alt.GUI.Temporary.Gwen.Control.Button m_Flip;

            bool m_PolygonFlip = true;
            PolygonElement m_PolygonElement;

            Pen m_BSplinePen;


            public Example_BSpline_FlowGeometryContainer(Base parent)
                : base(parent)
            {
                ClientBackColor = Color.FromArgb(50, Color.Black);
                DrawClientBorder = true;
                ClientBorderColor = Color.DodgerBlue;


                //  Top Label
				m_TopLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                m_TopLabel.AutoSizeToContents = true;
                m_TopLabel.Text = "Transformed B-Spline";
                m_TopLabel.TextColor = Color.Yellow;
                m_TopLabel.Dock = Pos.Top;
                m_TopLabel.Margin = new Margin(0, 3, 0, 5);


                //  Flip
				m_Flip = new Alt.GUI.Temporary.Gwen.Control.Button(this);
                m_Flip.Dock = Pos.Bottom;
                m_Flip.Text = "Flip";
                m_Flip.Click += new EventHandler(Flip_Click);
                m_Flip.Margin = new Margin(0, 5, 0, 0);


                //  PointsNumberSlider
				m_PointsNumberSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
                //slider.SetPosition(10, 40);
                m_PointsNumberSlider.Dock = Pos.Bottom;
                m_PointsNumberSlider.SetSize(150, 20);
                m_PointsNumberSlider.SetRange(1, 40);
                m_PointsNumberSlider.NotchCount = (int)(m_PointsNumberSlider.Max - m_PointsNumberSlider.Min);
                m_PointsNumberSlider.SnapToNotches = true;
                m_PointsNumberSlider.ValueChanged += PointsNumberSlider_ValueChanged;

				m_PointsNumberSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                m_PointsNumberSliderLabel.AutoSizeToContents = true;
                m_PointsNumberSliderLabel.Dock = Pos.Bottom;
                m_PointsNumberSliderLabel.Margin = new Margin(0, 3, 0, 0);

                m_PointsNumberSlider.Value = 30;


                //  CloseCheckBox
                m_CloseCheckBox = new LabeledCheckBox(this);
                m_CloseCheckBox.Text = "Close";
                m_CloseCheckBox.Dock = Pos.Bottom;
                m_CloseCheckBox.Margin = new Margin(0, 7, 0, 0);


                //  Graphics
                m_BSplinePen = new Pen(Color.Cyan, 2);
            }


            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);

                //  Geometry
                m_PolygonElement = new PolygonElement(6, 5.0);
                AddElement(m_PolygonElement);
                InitPoly();
            }


            void InitPoly()
            {
                PolygonElement polygon = m_PolygonElement;
                int offset = 100;
                int quad = System.Math.Min(ClientRectangle.Width, ClientRectangle.Height);
                int x = ClientRectangle.X + (ClientRectangle.Width - quad) / 2;
                int y = ClientRectangle.Y + (ClientRectangle.Height - quad) / 2;

                if (m_PolygonFlip)
                {
                    polygon.SetXn(0, x + offset);
                    polygon.SetYn(0, y + quad - offset);
                    polygon.SetXn(1, x + quad - offset);
                    polygon.SetYn(1, y + quad - offset);
                    polygon.SetXn(2, x + quad - offset);
                    polygon.SetYn(2, y + offset);
                    polygon.SetXn(3, x + offset);
                    polygon.SetYn(3, y + offset);
                }
                else
                {
                    polygon.SetXn(0, x + offset);
                    polygon.SetYn(0, y + offset);
                    polygon.SetXn(1, x + quad - offset);
                    polygon.SetYn(1, y + offset);
                    polygon.SetXn(2, x + quad - offset);
                    polygon.SetYn(2, y + quad - offset);
                    polygon.SetXn(3, x + offset);
                    polygon.SetYn(3, y + quad - offset);
                }

                polygon.SetXn(4, x + quad / 2);
                polygon.SetYn(4, y + quad / 2);
                polygon.SetXn(5, x + quad / 2);
                polygon.SetYn(5, y + quad / 1.5);
            }


            void PointsNumberSlider_ValueChanged(Base control)
            {
                m_PointsNumberSliderLabel.Text = m_PointsNumberSliderText + ((int)m_PointsNumberSlider.Value).ToString();
            }


            protected override void OnPaint(GUI.PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;


                PolygonElement polygon = m_PolygonElement;
                polygon.close(m_CloseCheckBox.IsChecked);

                PolylineGeometry poly = new PolylineGeometry(polygon.Polygon, true, m_CloseCheckBox.IsChecked);

                BSplineGeometry bspline = new BSplineGeometry(poly);
                bspline.InterpolationStep = 1.0 / m_PointsNumberSlider.Value;


                e.Graphics.DrawGeometry(m_BSplinePen, bspline);


                base.OnPaint(e);
            }


            void Flip_Click(object sender, EventArgs e)
            {
                m_PolygonFlip = !m_PolygonFlip;
                InitPoly();
            }


            protected override void Render(Alt.GUI.Temporary.Gwen.Skin.Base skin)
            {
                DoubleBuffered = !skin.Renderer.Graphics.IsClippingSupported;

                base.Render(skin);
            }
        }
    }
}
