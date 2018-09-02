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
    class Example_CombinedGeometry : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                return base.LogoRightTopOffset;

				/*TEST
                int x = 0;
                if (m_UIPanel != null)
                {
                    x = m_UIPanel.Width + 5;
                }

                return new SizeI(x, 0);*/
            }
        }


		Alt.GUI.Temporary.Gwen.Control.Base m_UIPanel;

		Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup groupBox_Geometry;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbGeometry1;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbGeometry2;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbGeometry3;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbGeometry4;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbGeometry5;

		Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup groupBox_BooleanOperation;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbBminusA;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbAminusB;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbXor;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbAnd;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbOr;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbNone;

		Alt.GUI.Temporary.Gwen.Control.Base m_DrawingPanel;

		Alt.GUI.Temporary.Gwen.Control.Label m_TopLabel;

        Color m_GeometryAColor;
        Color m_GeometryBColor;
        Pen m_SpiralPen;
        Pen m_GreatBritainContourPen_SWR;
        Pen m_GreatBritainContourPen_HWR;
        Pen m_ContourPen;

        Font m_InfoFont;

        double m_X;
        double m_Y;


        public Example_CombinedGeometry(Base parent)
            : base(parent)
        {
            this.Margin = new Margin(0);


            //  Top Label
			m_TopLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_TopLabel.AutoSizeToContents = true;
            m_TopLabel.Text = "Geometry Boolean Operations using CombinedGeometry (press the left mouse button to move the geometry)";
            m_TopLabel.TextColor = Color.Yellow;
            m_TopLabel.Dock = Pos.Top;
            m_TopLabel.Margin = new Margin(0, 5, 0, 5);


			Alt.GUI.Temporary.Gwen.Control.VerticalSplitter splitter = new Alt.GUI.Temporary.Gwen.Control.VerticalSplitter(this);
            splitter.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;


            //  UI Panel
            m_UIPanel = new Base(splitter);
            m_UIPanel.Dock = Pos.Fill;


            //  Geometry
			groupBox_Geometry = new Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup(m_UIPanel, string.Empty);
            groupBox_Geometry.Text = "Geometry:";
            //groupBox_Geometry.Margin = new Margin(0, 5, 0, 0);
            groupBox_Geometry.Margin = new Margin(0,
#if WINDOWS_PHONE_7 || WINDOWS_PHONE_71
                5,
#else
				GUI.Config.Logo.PixelHeight,
#endif
                5, 0);
            groupBox_Geometry.Dock = Pos.Top;
            groupBox_Geometry.AutoSizeToContents = true;

            rbGeometry1 = groupBox_Geometry.AddOption("Spiral and Text");
            rbGeometry1.Margin = new Margin(0, 3, 0, 0);
            rbGeometry2 = groupBox_Geometry.AddOption("Great Britain and Spiral");
            rbGeometry3 = groupBox_Geometry.AddOption("Great Britain and Arrows");
            rbGeometry4 = groupBox_Geometry.AddOption("Closed Stroke");
            rbGeometry5 = groupBox_Geometry.AddOption("Two Simple Paths");
            rbGeometry5.Margin = new Margin(0, 0, 0,
#if SILVERLIGHT
                22
#else
                13
#endif
                );


            //  Boolean Op
			groupBox_BooleanOperation = new Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup(m_UIPanel, string.Empty);
            groupBox_BooleanOperation.Text = "Boolean Operation:";
            groupBox_BooleanOperation.Margin = new Margin(0, 15, 5, 0);
            groupBox_BooleanOperation.Dock = Pos.Top;
            groupBox_BooleanOperation.AutoSizeToContents = true;

            rbBminusA = groupBox_BooleanOperation.AddOption("B-A");
            rbBminusA.Margin = new Margin(0, 3, 0, 0);
            rbAminusB = groupBox_BooleanOperation.AddOption("A-B");
            rbXor = groupBox_BooleanOperation.AddOption("XOR");
            rbAnd = groupBox_BooleanOperation.AddOption("AND");
            rbOr = groupBox_BooleanOperation.AddOption("OR");
            rbNone = groupBox_BooleanOperation.AddOption("None");
            rbNone.Margin = new Margin(0, 0, 0,
#if SILVERLIGHT
                27
#else
                15
#endif
                );


            //  Drawing Panel
            m_DrawingPanel = new Base(splitter);
            m_DrawingPanel.Dock = Pos.Fill;
            m_DrawingPanel.Margin = new Margin(-1);
            //m_DrawingPanel.ClientBackColor = Color.White;
            //m_DrawingPanel.DrawBorder = true;
            //m_DrawingPanel.BorderColor = Color.DodgerBlue;
            m_DrawingPanel.Paint += new GUI.PaintEventHandler(panel2_Paint);
            m_DrawingPanel.MouseDown += new Alt.GUI.MouseEventHandler(panel2_MouseDown);
            m_DrawingPanel.MouseMove += new Alt.GUI.MouseEventHandler(panel2_MouseMove);


            //
            splitter.SetPanel(1, m_UIPanel);
            splitter.SetPanel(0, m_DrawingPanel);
            splitter.SetHValue(0.78f);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            m_InfoFont = new Font("Arial", 12, FontStyle.Regular);


            rbGeometry2.Select();
            rbAnd.Select();


            //  Neet to correct groupBoxes
            groupBox_Geometry.AutoSizeToContents = false;
            groupBox_BooleanOperation.AutoSizeToContents = false;


            //
            m_X = m_DrawingPanel.ClientRectangle.Center.X;
            m_Y = m_DrawingPanel.ClientRectangle.Center.Y;
        }


        protected override void Render(Alt.GUI.Temporary.Gwen.Skin.Base skin)
        {
            if (m_SpiralPen == null)
            {
                bool isUnityRender = skin.Renderer.Graphics.RenderSystemName == Graphics.RSN_Unity;

                m_GeometryAColor = new ColorR(isUnityRender ? 0.5 : 0.2, 1, 1, 1);
                m_GeometryBColor = new ColorR(isUnityRender ? 0.5 : 0.2, 0, 0, 0);
                m_SpiralPen = new Pen(m_GeometryAColor, 15);
                m_GreatBritainContourPen_SWR = new Pen(ColorR.Black, 0.1);
                m_GreatBritainContourPen_HWR = new Pen(ColorR.FromArgb(0.57, ColorR.Black));//, 0.1);
                m_ContourPen = new Pen(Color.FromArgb(isUnityRender ? 225 : 200, Color.Yellow));
            }

            base.Render(skin);

            groupBox_Geometry.SizeToChildren(false, true);
            groupBox_BooleanOperation.SizeToChildren(false, true);
            groupBox_Geometry.AutoSizeToContents = false;
            groupBox_BooleanOperation.AutoSizeToContents = false;
        }


        void panel2_MouseDown(object sender, GUI.MouseEventArgs e)
        {
            if (e.Button == GUI.MouseButtons.Left)
            {
                m_X = e.X;
                m_Y = e.Y;
            }
        }


        void panel2_MouseMove(object sender, GUI.MouseEventArgs e)
        {
            if (e.Button == GUI.MouseButtons.Left)
            {
                m_X = e.X;
                m_Y = e.Y;
            }
        }


        void PerformRendering(Graphics graphics, CombinedGeometry clp)
        {
            if (!rbNone.IsChecked)
            {
                if (rbBminusA.IsChecked)
                {
                    clp.Operation = CombinedGeometry.CombineMode.B_minus_A;
                }
                else if (rbAminusB.IsChecked)
                {
                    clp.Operation = CombinedGeometry.CombineMode.A_minus_B;
                }
                else if (rbXor.IsChecked)
                {
                    clp.Operation = CombinedGeometry.CombineMode.Xor;
                }
                else if (rbAnd.IsChecked)
                {
                    clp.Operation = CombinedGeometry.CombineMode.And;
                }
                else if (rbOr.IsChecked)
                {
                    clp.Operation = CombinedGeometry.CombineMode.Or;
                }

                GeometryPolyCounter counter = new GeometryPolyCounter(clp);

                GraphicsPath path = new GraphicsPath();
                path.Concat(counter);

                graphics.FillPath(new ColorR(0.65, 0.25, 0.9, 0.25), path);
                graphics.DrawPath(m_ContourPen, path);

                string info = StringFormatter.sprintf("Contours: %d      Points: %d", counter.Contours, counter.Points);
                graphics.DrawString(info, m_InfoFont, Brushes.White, Point.Five);
            }
        }


        Geometry m_GreatBritain;
        Geometry GreatBritain
        {
            get
            {
                if (m_GreatBritain == null)
                {
                    m_GreatBritain = new Geometry_GreatBritain();
                }

                return m_GreatBritain;
            }
        }


        Geometry m_Arrows;
        Geometry Arrows
        {
            get
            {
                if (m_Arrows == null)
                {
                    m_Arrows = new Geometry_Arrows();
                }

                return m_Arrows;
            }
        }


        Geometry m_Text;
        Geometry Text
        {
            get
            {
                if (m_Text == null)
                {
                    m_Text = new FlattenCurveGeometry(Common.GraphicsPath_AltSketch);
                }

                return m_Text;
            }
        }


        Geometry m_Spiral;
        Geometry Spiral
        {
            get
            {
                if (m_Spiral == null)
                {
                    m_Spiral = new Geometry_Spiral(0, 0, 10, 150, 30, 0.0);
                }

                return m_Spiral;
            }
        }


        void panel2_Paint(object sender, GUI.PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            bool isSoftwareGraphics = graphics is SoftwareGraphics;
            //if (isSoftwareGraphics)
            {
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
            }


            // Two simple paths
            if (rbGeometry5.IsChecked)
            {
                GraphicsPath ps1_path = new GraphicsPath();

                double x = m_X - m_DrawingPanel.ClientRectangle.Center.X / 2 + 100;
                double y = m_Y - m_DrawingPanel.ClientRectangle.Center.Y / 2 + 100;
                ps1_path.MoveTo(x + 140, y + 145);
                ps1_path.LineTo(x + 225, y + 44);
                ps1_path.LineTo(x + 296, y + 219);
                ps1_path.ClosePolygon();

                ps1_path.LineTo(x + 226, y + 289);
                ps1_path.LineTo(x + 82, y + 292);

                ps1_path.MoveTo(x + 220, y + 222);
                ps1_path.LineTo(x + 363, y + 249);
                ps1_path.LineTo(x + 265, y + 331);

                ps1_path.MoveTo(x + 242, y + 243);
                ps1_path.LineTo(x + 268, y + 309);
                ps1_path.LineTo(x + 325, y + 261);

                ps1_path.MoveTo(x + 259, y + 259);
                ps1_path.LineTo(x + 273, y + 288);
                ps1_path.LineTo(x + 298, y + 266);

                Rect ps1Bounds = ps1_path.Bounds;
                GeometryTransformer ps1 = GeometryMatrix4Transformer.CreateOptimizedTransformer(ps1_path, Matrix4.CreateTranslation(
                        m_X - ps1Bounds.Center.X,
                        m_Y - ps1Bounds.Center.Y));


                GraphicsPath ps2_path = new GraphicsPath();
                ps2_path.MoveTo(100 + 32, 100 + 77);
                ps2_path.LineTo(100 + 473, 100 + 263);
                ps2_path.LineTo(100 + 351, 100 + 290);
                ps2_path.LineTo(100 + 354, 100 + 374);
                Rect ps2Bounds = ps2_path.Bounds;
                GeometryTransformer ps2 = GeometryMatrix4Transformer.CreateOptimizedTransformer(ps2_path, Matrix4.CreateTranslation(
                        (m_DrawingPanel.Width - ps2Bounds.Width) / 2 - ps2Bounds.Left,
                        (m_DrawingPanel.Height - ps2Bounds.Height) / 2 - ps2Bounds.Top));


                graphics.FillGeometry(m_GeometryBColor, ps2);
                graphics.FillGeometry(m_GeometryAColor, ps1);

                CombinedGeometry clp = new CombinedGeometry(ps1, ps2, CombinedGeometry.CombineMode.Or, CombinedGeometry.FillType.NonZero, CombinedGeometry.FillType.NonZero);

                PerformRendering(graphics, clp);
            }
            // Closed stroke
            else if (rbGeometry4.IsChecked)
            {
                GraphicsPath ps1_path = new GraphicsPath();
                
                double x = m_X - m_DrawingPanel.ClientRectangle.Center.X / 2 + 100;
                double y = m_Y - m_DrawingPanel.ClientRectangle.Center.Y / 2 + 100;
                ps1_path.MoveTo(x + 140, y + 145);
                ps1_path.LineTo(x + 225, y + 44);
                ps1_path.LineTo(x + 296, y + 219);
                ps1_path.ClosePolygon();

                ps1_path.LineTo(x + 226, y + 289);
                ps1_path.LineTo(x + 82, y + 292);

                ps1_path.MoveTo(x + 220 - 50, y + 222);
                ps1_path.LineTo(x + 265 - 50, y + 331);
                ps1_path.LineTo(x + 363 - 50, y + 249);
                ps1_path.ClosePolygon(GeometryVertexCommandAndFlags.FlagCCW);

                Rect ps1Bounds = ps1_path.Bounds;
                GeometryTransformer ps1 = GeometryMatrix4Transformer.CreateOptimizedTransformer(ps1_path, Matrix4.CreateTranslation(
                        m_X - ps1Bounds.Center.X,
                        m_Y - ps1Bounds.Center.Y));


                GraphicsPath ps2_path = new GraphicsPath();
                ps2_path.MoveTo(100 + 32, 100 + 77);
                ps2_path.LineTo(100 + 473, 100 + 263);
                ps2_path.LineTo(100 + 351, 100 + 290);
                ps2_path.LineTo(100 + 354, 100 + 374);
                ps2_path.ClosePolygon();
                Rect ps2Bounds = ps2_path.Bounds;
                GeometryTransformer ps2 = GeometryMatrix4Transformer.CreateOptimizedTransformer(ps2_path, Matrix4.CreateTranslation(
                        (m_DrawingPanel.Width - ps2Bounds.Width) / 2 - ps2Bounds.Left,
                        (m_DrawingPanel.Height - ps2Bounds.Height) / 2 - ps2Bounds.Top));

                GeometryStroke stroke = new GeometryStroke(ps2, 10);


                graphics.FillGeometry(m_GeometryBColor, stroke);
                graphics.FillGeometry(m_GeometryAColor, ps1);

                CombinedGeometry clp = new CombinedGeometry(ps1, stroke, CombinedGeometry.CombineMode.Or, CombinedGeometry.FillType.NonZero, CombinedGeometry.FillType.NonZero);

                PerformRendering(graphics, clp);
            }
            // Great Britain and Arrows
            else if (rbGeometry3.IsChecked)
            {
                Geometry poly = GreatBritain;
                Rect polyBounds = poly.Bounds;
                double scale = 3;
                Matrix4 mtx = Matrix4.CreateTranslation(-polyBounds.Center.X, -polyBounds.Center.Y);
                mtx *= Matrix4.CreateScaling(scale, scale);
                mtx *= Matrix4.CreateTranslation(m_DrawingPanel.Width / 2, m_DrawingPanel.Height / 2);
                GeometryTransformer trans_gb_poly = GeometryMatrix4Transformer.CreateOptimizedTransformer(poly, mtx);

                Geometry arrows = Arrows;
                Rect arrowsBounds = arrows.Bounds;
                mtx = Matrix4.CreateTranslation(-arrowsBounds.Center.X, -arrowsBounds.Center.Y);
                mtx *= Matrix4.CreateScaling(scale, scale);
                mtx *= Matrix4.CreateTranslation(m_X, m_Y);
                GeometryTransformer trans_arrows = GeometryMatrix4Transformer.CreateOptimizedTransformer(arrows, mtx);

                CombinedGeometry clp = new CombinedGeometry(trans_gb_poly, trans_arrows,
                    CombinedGeometry.CombineMode.Or,
                    CombinedGeometry.FillType.NonZero,
                    CombinedGeometry.FillType.NonZero);

                graphics.FillGeometry(m_GeometryBColor, trans_gb_poly);
                graphics.DrawGeometry(isSoftwareGraphics ? m_GreatBritainContourPen_SWR : m_GreatBritainContourPen_HWR, trans_gb_poly);
                graphics.FillGeometry(m_GeometryAColor, trans_arrows);

                PerformRendering(graphics, clp);
            }
            // Great Britain and a Spiral
            else if (rbGeometry2.IsChecked)
            {
                Geometry poly = GreatBritain;
                Rect polyBounds = poly.Bounds;
                double scale = 3;
                Matrix4 mtx = Matrix4.CreateTranslation(-polyBounds.Center.X, -polyBounds.Center.Y);
                mtx *= Matrix4.CreateScaling(scale, scale);
                mtx *= Matrix4.CreateTranslation(m_DrawingPanel.Width / 2, m_DrawingPanel.Height / 2);
                GeometryTransformer trans_gb_poly = GeometryMatrix4Transformer.CreateOptimizedTransformer(poly, mtx);

                Geometry sp = GeometryMatrix4Transformer.CreateOptimizedTransformer(Spiral, Matrix4.CreateTranslation(m_X, m_Y));
                GeometryStroke stroke = new GeometryStroke(sp, 15);

                CombinedGeometry clp = new CombinedGeometry(trans_gb_poly, stroke,
                    CombinedGeometry.CombineMode.Or,
                    CombinedGeometry.FillType.NonZero,
                    CombinedGeometry.FillType.NonZero);

                graphics.FillGeometry(m_GeometryBColor, trans_gb_poly);
                graphics.DrawGeometry(isSoftwareGraphics ? m_GreatBritainContourPen_SWR : m_GreatBritainContourPen_HWR, trans_gb_poly);
                graphics.DrawGeometry(m_SpiralPen, sp);

                PerformRendering(graphics, clp);
            }
            // Spiral and text
            else if (rbGeometry1.IsChecked)
            {
                Geometry text = Text;
                Rect textBounds = text.Bounds;
                GeometryTransformer transformedText = GeometryMatrix4Transformer.CreateOptimizedTransformer(text,
                    Matrix4.CreateTranslation(
                        (m_DrawingPanel.Width - textBounds.Width) / 2 - textBounds.Left,
                        (m_DrawingPanel.Height - textBounds.Height) / 2 - textBounds.Top));

                Geometry sp = GeometryMatrix4Transformer.CreateOptimizedTransformer(Spiral, Matrix4.CreateTranslation(m_X, m_Y));
                GeometryStroke stroke = new GeometryStroke(sp, 15);

                CombinedGeometry clp = new CombinedGeometry(stroke, transformedText,
                    CombinedGeometry.CombineMode.Or,
                    CombinedGeometry.FillType.NonZero,
                    CombinedGeometry.FillType.NonZero);

                graphics.FillGeometry(m_GeometryBColor, transformedText);
                graphics.DrawGeometry(m_SpiralPen, sp);

                PerformRendering(graphics, clp);
            }
        }
    }
}
