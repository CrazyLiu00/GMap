//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.


#define GEOMETRY_TRANSFORMATIONS


using System;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.Demo.SimpleSVG;
using Alt.Sketch;
using Alt.Sketch.Geometries;
using Alt.Sketch.Geometries.Reformers;
using Alt.Sketch.Geometries.Transformers;


namespace Alt.GUI.Demo
{
    class Example_Outline_SVG : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                return new SizeI(30, 0);
            }
        }


        //  Opacity
		Alt.GUI.Temporary.Gwen.Control.Base m_OpacityControl;
		Alt.GUI.Temporary.Gwen.Control.VerticalSlider m_OpacitySlider;
		Alt.GUI.Temporary.Gwen.Control.Button m_OpacityPlusButton;
		Alt.GUI.Temporary.Gwen.Control.Button m_OpacityMinusButton;

        //  Scale
		Alt.GUI.Temporary.Gwen.Control.Base m_ScaleControl;
		Alt.GUI.Temporary.Gwen.Control.VerticalSlider m_ScaleSlider;
		Alt.GUI.Temporary.Gwen.Control.Button m_ScalePlusButton;
		Alt.GUI.Temporary.Gwen.Control.Button m_ScaleMinusButton;

        //  Rotate
		Alt.GUI.Temporary.Gwen.Control.Base m_RotateControl;
		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_RotateSlider;
		Alt.GUI.Temporary.Gwen.Control.Button m_RotatePlusButton;
		Alt.GUI.Temporary.Gwen.Control.Button m_RotateMinusButton;


        DoubleBufferedControl m_PreviewPanel;
        DoubleBufferedControl m_DrawingPanel;


        SVGPath m_SVGPath;
        Rect m_SVGPathBounds;
        double m_SmallScale;


        double m_x;
        double m_y;
        double m_dx;
        double m_dy;
        bool m_DragFlag;


        public Example_Outline_SVG(Base parent)
            : base(parent)
        {
            m_PreviewPanel = new DoubleBufferedControl(this);
            m_PreviewPanel.SetBounds(0, 0, 300, 300);
            m_PreviewPanel.ClientBackColor = Color.FromArgb(0, 128, 128, 128);
            m_PreviewPanel.Paint += new GUI.PaintEventHandler(PreviewPanel_Paint);
            m_PreviewPanel.SoftwareRender = true;

            m_DrawingPanel = new DoubleBufferedControl(this);
            m_DrawingPanel.ClientBackColor = Color.FromArgb(0, 128, 128, 128);
            m_DrawingPanel.Dock = Pos.Fill;
            m_DrawingPanel.Paint += new GUI.PaintEventHandler(DrawingPanel_Paint);
            m_DrawingPanel.Resize += new EventHandler(DrawingPanel_Resize);
            m_DrawingPanel.MouseDown += new GUI.MouseEventHandler(DrawingPanel_MouseDown);
            m_DrawingPanel.MouseUp += new GUI.MouseEventHandler(DrawingPanel_MouseUp);
            m_DrawingPanel.MouseMove += new GUI.MouseEventHandler(DrawingPanel_MouseMove);
            m_DrawingPanel.MouseWheel += new GUI.MouseEventHandler(DrawingPanel_MouseWheel);

            
            //  Opacity (create before resize)
            m_OpacityControl = new Base(m_DrawingPanel);
            m_OpacityControl.SetBounds(10, 30, 20, 200);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            //  Opacity
			m_OpacityPlusButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_OpacityControl);
            m_OpacityPlusButton.Text = "+";
            m_OpacityPlusButton.Dock = Pos.Top;
            m_OpacityPlusButton.Click += new EventHandler(OpacityPlusButton_Click);

			m_OpacityMinusButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_OpacityControl);
            m_OpacityMinusButton.Text = "-";
            m_OpacityMinusButton.Dock = Pos.Bottom;
            m_OpacityMinusButton.Click += new EventHandler(OpacityMinusButton_Click);

			m_OpacitySlider = new Alt.GUI.Temporary.Gwen.Control.VerticalSlider(m_OpacityControl);
            m_OpacitySlider.Dock = Pos.Fill;
            m_OpacitySlider.SetRange(0.05f, 1);
            m_OpacitySlider.Value = 1;


            //  Scale
			m_ScaleControl = new Alt.GUI.Temporary.Gwen.Control.Base(m_DrawingPanel);
            m_ScaleControl.SetBounds(10, 30, 20, 200);

			m_ScalePlusButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_ScaleControl);
            m_ScalePlusButton.Text = "+";
            m_ScalePlusButton.Dock = Pos.Top;
            m_ScalePlusButton.Click += new EventHandler(ScalePlusButton_Click);

			m_ScaleMinusButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_ScaleControl);
            m_ScaleMinusButton.Text = "-";
            m_ScaleMinusButton.Dock = Pos.Bottom;
            m_ScaleMinusButton.Click += new EventHandler(ScaleMinusButton_Click);

			m_ScaleSlider = new Alt.GUI.Temporary.Gwen.Control.VerticalSlider(m_ScaleControl);
            m_ScaleSlider.Dock = Pos.Fill;
            m_ScaleSlider.SetRange(0.3f, 5);
            m_ScaleSlider.Value = 1;


            //  Rotate
			m_RotateControl = new Alt.GUI.Temporary.Gwen.Control.Base(m_DrawingPanel);
            m_RotateControl.SetBounds(30, 10, 200, 20);

			m_RotatePlusButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_RotateControl);
            m_RotatePlusButton.Text = "+";
            m_RotatePlusButton.Dock = Pos.Right;
            m_RotatePlusButton.Width = m_RotateControl.Height;
            m_RotatePlusButton.Click += new EventHandler(RotatePlusButton_Click);

			m_RotateMinusButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_RotateControl);
            m_RotateMinusButton.Text = "-";
            m_RotateMinusButton.Dock = Pos.Left;
            m_RotateMinusButton.Width = m_RotateControl.Height;
            m_RotateMinusButton.Click += new EventHandler(RotateMinusButton_Click);

			m_RotateSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(m_RotateControl);
            m_RotateSlider.Dock = Pos.Fill;
            m_RotateSlider.SetRange(-180, 180);
            m_RotateSlider.Value = 0;


            m_OpacitySlider.ValueChanged += new GwenEventHandler(Slider_ValueChanged);
            m_ScaleSlider.ValueChanged += new GwenEventHandler(Slider_ValueChanged);
            m_RotateSlider.ValueChanged += new GwenEventHandler(Slider_ValueChanged);
            //  Preview
            m_OpacitySlider.ValueChanged += new GwenEventHandler(RotateAndOpacity_Slider_ValueChanged);
            m_RotateSlider.ValueChanged += new GwenEventHandler(RotateAndOpacity_Slider_ValueChanged);



            //
            m_SVGPath = new SVGPath();

            int k = 30;
            m_x = m_DrawingPanel.ClientWidth / 2 + k;
            m_y = m_DrawingPanel.ClientHeight / 2 + k;
            m_dx = 0.0;
            m_dy = 0.0;
            m_DragFlag = false;


            try
            {
                LoadSVG("AltData/SVG/longhorn.svg");
                //LoadSVG("AltData/SVG/lion.svg");
            }
            catch (Exception)
            {
                return;
            }
        }


        void Slider_ValueChanged(Base control)
        {
            m_DrawingPanel.Refresh();
        }

        void RotateAndOpacity_Slider_ValueChanged(Base control)
        {
            m_PreviewPanel.Refresh();
        }


        void DrawingPanel_Resize(object sender, EventArgs e)
        {
            m_OpacityControl.SetBounds(m_DrawingPanel.Width - m_OpacityControl.Width - 10, 30, 20, 200);
        }


        void OpacityPlusButton_Click(object sender, EventArgs e)
        {
            m_OpacitySlider.Value += 0.1f;
        }

        void OpacityMinusButton_Click(object sender, EventArgs e)
        {
            m_OpacitySlider.Value -= 0.1f;
        }


        void RotatePlusButton_Click(object sender, EventArgs e)
        {
            m_RotateSlider.Value += 10;
        }

        void RotateMinusButton_Click(object sender, EventArgs e)
        {
            m_RotateSlider.Value -= 10;
        }


        void ScalePlusButton_Click(object sender, EventArgs e)
        {
            if (m_ScaleSlider.Value >= 1)
            {
                m_ScaleSlider.Value += 0.5f;
            }
            else
            {
                m_ScaleSlider.Value += 0.1f;
            }
        }

        void ScaleMinusButton_Click(object sender, EventArgs e)
        {
            if (m_ScaleSlider.Value > 1)
            {
                m_ScaleSlider.Value -= 0.5f;
            }
            else
            {
                m_ScaleSlider.Value -= 0.1f;
            }
        }


        void LoadSVG(string fname)
        {
            SVGParser p = new SVGParser(m_SVGPath);

            p.Parse(fname);
            m_SVGPath.ArrangeOrientations();
            m_SVGPathBounds = m_SVGPath.Bounds;

            m_ScaleSlider.Value = (float)(System.Math.Min(m_DrawingPanel.ClientWidth / m_SVGPathBounds.Width, m_DrawingPanel.ClientHeight / m_SVGPathBounds.Height) * 0.75);
            m_SmallScale = (float)System.Math.Min(m_ScaleControl.Height / m_SVGPathBounds.Width, m_ScaleControl.Height / m_SVGPathBounds.Height);
        }


        void PreviewPanel_Paint(object sender, GUI.PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;


            //  Small Filled
            Matrix4 mtx = Matrix4.CreateTranslation(-m_SVGPathBounds.Center.X, -m_SVGPathBounds.Center.Y);
            double scale = m_SmallScale;
            mtx.Scale(scale, scale, MatrixOrder.Append);
            mtx.Rotate(m_RotateSlider.Value, MatrixOrder.Append);
            mtx.Translate(
                m_ScaleControl.Right + m_SVGPathBounds.Width * scale / 2 + 10,
                m_RotateControl.Bottom + m_SVGPathBounds.Height * scale / 2 + 10, MatrixOrder.Append);

            m_SVGPath.Render(e.Graphics, mtx);
        }

        void DrawingPanel_Paint(object sender, GUI.PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            double scale = m_ScaleSlider.Value;
            e.Graphics.TranslateTransform(scale * m_x, scale * m_y);

            //  Outlined
            Matrix4 mtx = Matrix4.CreateTranslation(-m_SVGPathBounds.Center.X, -m_SVGPathBounds.Center.Y);
            mtx.Scale(scale, scale, MatrixOrder.Append);
            mtx.Rotate(m_RotateSlider.Value, MatrixOrder.Append);
            //mtx.Translate(m_x, m_y, MatrixOrder.Append);


            GeometryStroke stroke = new GeometryStroke();
            stroke.Width = m_ScaleSlider.Value;
            stroke.LineJoin = LineJoin.Round;


            e.Graphics.Opacity = m_OpacitySlider.Value;
#if GEOMETRY_TRANSFORMATIONS
            m_SVGPath.Render(e.Graphics, mtx, stroke);
#else
            e.Graphics.Transform = mtx.ToMatrix();

            m_SVGPath.Render(e.Graphics, stroke);
#endif
        }


        void DrawingPanel_MouseDown(object sender, GUI.MouseEventArgs e)
        {
            double scale = m_ScaleSlider.Value;

            m_dx = e.X - m_x * scale;
            m_dy = e.Y - m_y * scale;

            m_DragFlag = true;
        }


        void DrawingPanel_MouseMove(object sender, GUI.MouseEventArgs e)
        {
            if (e.Button != GUI.MouseButtons.Left)
            {
                m_DragFlag = false;
            }

            if (m_DragFlag)
            {
                double scale = m_ScaleSlider.Value;

                m_x = (e.X - m_dx) / scale;
                m_y = (e.Y - m_dy) / scale;

                m_DrawingPanel.Refresh();
            }
        }


        void DrawingPanel_MouseUp(object sender, GUI.MouseEventArgs e)
        {
            m_DragFlag = false;
        }


        void DrawingPanel_MouseWheel(object sender, GUI.MouseEventArgs e)
        {
            m_ScaleSlider.Value += 0.001f * e.Delta;
        }
    }
}
