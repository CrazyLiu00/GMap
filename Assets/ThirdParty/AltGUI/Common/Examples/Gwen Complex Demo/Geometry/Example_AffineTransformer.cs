//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

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
    class Example_AffineTransformer : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                return new SizeI(30, 0);
            }
        }


        Bitmap m_Bitmap;
        Bitmap Bitmap
        {
            get
            {
                //  Recreate
                if (m_Bitmap != null &&
                    (m_Bitmap.PixelWidth != ClientWidth || m_Bitmap.PixelHeight != ClientHeight))
                {
                    m_Bitmap.Dispose();
                    m_Bitmap = null;
                }

                if (m_Bitmap == null)
                {
                    m_Bitmap = new Bitmap(ClientWidth, ClientHeight, PixelFormat.Format32bppArgb);

                    using (Graphics graphics = Graphics.FromImage(m_Bitmap))
                    {
                        graphics.Clear(Color.Transparent);
                    }
                }

                return m_Bitmap;
            }
        }


        //  Opacity
		Alt.GUI.Temporary.Gwen.Control.Base m_OpacityControl;
        double m_Opacity;
        const double OpacityIncrement = 0.05;
		Alt.GUI.Temporary.Gwen.Control.VerticalSlider m_OpacitySlider;
		Alt.GUI.Temporary.Gwen.Control.Button m_OpacityPlusButton;
		Alt.GUI.Temporary.Gwen.Control.Button m_OpacityMinusButton;


        SVGPath m_SVGPath;
        Rect m_SVGPathBounds;
        double m_Rotation = 0;
        double m_Scale;
        double m_SkewX = 0;
        double m_SkewY = 0;


        double m_x;
        double m_y;
        double m_dx;
        double m_dy;
        bool m_DragFlag;


        public Example_AffineTransformer(Base parent)
            : base(parent)
        {
            //  Opacity (create before resize)
            m_OpacityControl = new Base(this);
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
            m_OpacitySlider.Value = (float)(m_Opacity = 0.05f);


            //
            m_SVGPath = new SVGPath();

            m_x = ClientWidth / 2;
            m_y = ClientHeight / 2;
            m_dx = 0.0;
            m_dy = 0.0;
            m_DragFlag = false;


            try
            {
                LoadSVG("AltData/SVG/lion.svg");
            }
            catch (SVGException)
            {
                //TEMP  message(.ToString(e.Message));

                return;
            }
        }


        void NeedRefresh()
        {
            if (m_OpacitySlider != null)
            {
                m_Opacity = m_OpacitySlider.Value - OpacityIncrement;
            }
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            m_OpacityControl.SetBounds(Width - m_OpacityControl.Width - 10, 30, 20, 200);

            m_x = ClientWidth / 2;
            m_y = ClientHeight / 2;

            NeedRefresh();
        }


        void OpacityPlusButton_Click(object sender, EventArgs e)
        {
            m_OpacitySlider.Value += 0.1f;
        }

        void OpacityMinusButton_Click(object sender, EventArgs e)
        {
            m_OpacitySlider.Value -= 0.1f;
        }


        void LoadSVG(string fname)
        {
            SVGParser p = new SVGParser(m_SVGPath);

            p.Parse(fname);
            m_SVGPath.ArrangeOrientations();
            m_SVGPathBounds = m_SVGPath.Bounds;

            m_Scale = (float)(System.Math.Min(ClientWidth / m_SVGPathBounds.Width, ClientHeight / m_SVGPathBounds.Height) * 0.75);
        }


        protected override void OnPaint(GUI.PaintEventArgs e)
        {
            base.OnPaint(e);


            Bitmap bitmap = Bitmap;
            if (bitmap != null)
            {
                if (m_Opacity < 1)
                {
                    m_Opacity += OpacityIncrement;
                    if (m_Opacity > 1)
                    {
                        m_Opacity = 1;
                    }

                    Matrix4 mtx = Matrix4.CreateTranslation(-m_SVGPathBounds.Center.X, -m_SVGPathBounds.Center.Y);
                    double scale = m_Scale;
                    mtx.Scale(scale, scale, MatrixOrder.Append);
                    mtx.Rotate(m_Rotation, MatrixOrder.Append);
                    mtx.Multiply(Matrix4.CreateSkewing(m_SkewX / 500.0, m_SkewY / 500.0), MatrixOrder.Append);
                    mtx.Translate(m_x, m_y, MatrixOrder.Append);

                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.Opacity = m_Opacity;
                        m_SVGPath.Render(graphics, mtx);
                    }
                }

                e.Graphics.DrawImage(bitmap);            
            }            
        }


        void UpdateTransform(double width, double height, double x, double y)
        {
            x -= m_x;
            y -= m_y;
            m_Rotation = MathHelper.RadiansToDegrees(Math.Atan2(y, x));
            m_Scale = Math.Sqrt(y * y + x * x) / 100.0;
        }


        protected void Move(GUI.MouseEventArgs e)
        {
            double x = e.X;
            double y = e.Y;

            if (e.Button == GUI.MouseButtons.Left)
            {
                int width = (int)Width;
                int height = (int)Height;
                UpdateTransform(width, height, x, y);

                NeedRefresh();
            }
            else if (e.Button == GUI.MouseButtons.Right)
            {
                m_SkewX = x - m_x;
                m_SkewY = y - m_y;

                NeedRefresh();
            }
        }


        protected override void OnMouseDown(GUI.MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == GUI.MouseButtons.Left &&
                (ModifierKeys & GUI.Keys.Control) != 0)
            {
                m_dx = e.X - m_x;
                m_dy = e.Y - m_y;
                m_DragFlag = true;
            }
            else
            {
                Move(e);
            }
        }


        protected override void OnMouseMove(GUI.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button != GUI.MouseButtons.Left ||
                (ModifierKeys & GUI.Keys.Control) == 0)
            {
                m_DragFlag = false;
            }

            if (m_DragFlag)
            {
                m_x = e.X - m_dx;
                m_y = e.Y - m_dy;

                NeedRefresh();
            }
            else
            {
                Move(e);
            }
        }


        protected override void OnMouseUp(GUI.MouseEventArgs e)
        {
            base.OnMouseUp(e);

            m_DragFlag = false;
        }


        protected override void OnMouseWheel(GUI.MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            m_Scale += 0.0005f * e.Delta;

            if (m_Scale < 0.1)
            {
                m_Scale = 0.1;
            }

            NeedRefresh();
        }
    }
}
