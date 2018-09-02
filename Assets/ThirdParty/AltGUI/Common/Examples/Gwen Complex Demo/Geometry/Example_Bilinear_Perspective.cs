//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.InteractiveGeometry;
using Alt.GUI.Demo.SimpleSVG;
using Alt.Sketch;
using Alt.Sketch.Geometries;
using Alt.Sketch.Geometries.Reformers;
using Alt.Sketch.Geometries.Transformers;


namespace Alt.GUI.Demo
{
    class Example_Bilinear_Perspective : Example__Base
    {
        RadioButtonGroup groupBox_TransformationType;
        LabeledRadioButton rbBilinear;
        //NoNeed	LabeledRadioButton rbPerspective;


        InteractiveGeometryContainer m_DrawingPanel;
        PolygonElement m_PolygonElement;


        SVGPath m_SVGPath;
        Rect m_SVGPathBounds;
        Rect m_SVGPathScaledBounds;


        Pen m_EllipsePen;
        Brush m_EllipseBrush;


        double m_Scale;
        double m_x;
        double m_y;


        public Example_Bilinear_Perspective(Base parent)
            : base(parent)
        {
            m_DrawingPanel = new InteractiveGeometryContainer(this);
            m_DrawingPanel.DoubleBuffered = true;
            m_DrawingPanel.ClientBackColor = Color.FromArgb(0, 128, 128, 128);
            m_DrawingPanel.Dock = Pos.Fill;
            m_DrawingPanel.Paint += new PaintEventHandler(DrawingPanel_Paint);
            m_DrawingPanel.Resize +=new EventHandler(DrawingPanel_Resize);


            //  Transformation Type
            groupBox_TransformationType = new RadioButtonGroup(m_DrawingPanel, string.Empty);
            groupBox_TransformationType.Text = "Type:";
            groupBox_TransformationType.Location = new PointI(10, 10);

            rbBilinear = groupBox_TransformationType.AddOption("Bilinear");
            rbBilinear.Margin = new Margin(0, 5, 0, 0);
			//NoNeed	rbPerspective =
			groupBox_TransformationType.AddOption("Perspective");
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            //  Transformation Type
            rbBilinear.Select();

            groupBox_TransformationType.SelectionChanged += new GwenEventHandler(groupBox_TransformationType_SelectionChanged);

            //  update groupBox_TransformationType location
            DrawingPanel_Resize(null, EventArgs.Empty);



            //
            m_PolygonElement = new PolygonElement(4, 5.0);
            m_PolygonElement.Color = Color.FromArgb(200, Color.Yellow);
            m_PolygonElement.LineWidth = 2;
            m_DrawingPanel.AddElement(m_PolygonElement);



            //
            m_SVGPath = new SVGPath();
            m_x = m_DrawingPanel.ClientWidth / 2;
            m_y = m_DrawingPanel.ClientHeight / 2;

            m_EllipsePen = new Pen(Color.LightGreen, 3);
            m_EllipseBrush = new SolidColorBrush(ColorR.FromArgb(0.4, 0.5, 0.3, 0.0));


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


        void DrawingPanel_Resize(object sender, EventArgs e)
        {
            if (groupBox_TransformationType != null)
            {
                groupBox_TransformationType.Location =
					new PointI(m_DrawingPanel.Width - groupBox_TransformationType.Width - 3, GUI.Config.Logo.PixelHeight + 19);
            }
        }


        void groupBox_TransformationType_SelectionChanged(Base control)
        {
            m_DrawingPanel.Refresh();
        }


        void LoadSVG(string fname)
        {
            SVGParser p = new SVGParser(m_SVGPath);

            p.Parse(fname);
            m_SVGPath.ArrangeOrientations();
            m_SVGPathBounds = m_SVGPath.Bounds;

            m_Scale = System.Math.Min(m_DrawingPanel.ClientWidth / m_SVGPathBounds.Width, m_DrawingPanel.ClientHeight / m_SVGPathBounds.Height) * 0.75;

            m_SVGPathScaledBounds = m_SVGPathBounds - m_SVGPathBounds.Location;
            m_SVGPathScaledBounds.Width *= m_Scale;
            m_SVGPathScaledBounds.Height *= m_Scale;
            m_SVGPathScaledBounds += new Point(m_x, m_y) - m_SVGPathScaledBounds.Center;
            m_PolygonElement.SetXn(0, m_SVGPathScaledBounds.Left);
            m_PolygonElement.SetYn(0, m_SVGPathScaledBounds.Top);
            m_PolygonElement.SetXn(1, m_SVGPathScaledBounds.Right);
            m_PolygonElement.SetYn(1, m_SVGPathScaledBounds.Top);
            m_PolygonElement.SetXn(2, m_SVGPathScaledBounds.Right);
            m_PolygonElement.SetYn(2, m_SVGPathScaledBounds.Bottom);
            m_PolygonElement.SetXn(3, m_SVGPathScaledBounds.Left);
            m_PolygonElement.SetYn(3, m_SVGPathScaledBounds.Bottom);            
        }


        void DrawingPanel_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;


            Matrix4 mtx = Matrix4.CreateTranslation(-m_SVGPathBounds.Center.X, -m_SVGPathBounds.Center.Y);
            mtx.Scale(m_Scale, m_Scale, MatrixOrder.Append);
            mtx.Translate(m_x, m_y, MatrixOrder.Append);


            ITransform transform = null;

            if (rbBilinear.IsChecked)
            {
                Bilinear tr = new Bilinear(
                    m_SVGPathScaledBounds.Left, m_SVGPathScaledBounds.Top,
                    m_SVGPathScaledBounds.Right, m_SVGPathScaledBounds.Bottom, m_PolygonElement.Polygon);

                if (tr.IsValid)
                {
                    transform = tr;
                }
            }
            else
            {
                Perspective tr = new Perspective(
                    m_SVGPathScaledBounds.Left, m_SVGPathScaledBounds.Top,
                    m_SVGPathScaledBounds.Right, m_SVGPathScaledBounds.Bottom, m_PolygonElement.Polygon);

                if (tr.IsValid() &&
                    Perspective.IsConvex(m_PolygonElement.Polygon))
                {
                    transform = tr;
                }
            }


            //
            GeometryTransformer transformer = null;
            if (transform != null)
            {
                transformer = new GeometryTransformer(transform);
            }


            // Render transformed SVG
            m_SVGPath.Render(e.Graphics, mtx, transformer);


            // Render transformed ellipse
            Point center = m_SVGPathScaledBounds.Center;
            EllipseGeometry FilledEllipse = new EllipseGeometry(center.X, center.Y, m_SVGPathScaledBounds.Width / 2, m_SVGPathScaledBounds.Height / 2, 200);

            GeometryStroke EllipseOutline = new GeometryStroke(FilledEllipse);
            EllipseOutline.Width = 3.0;


            Geometry TransformedFilledEllipse = FilledEllipse;
            Geometry TransformedEllipesOutline = EllipseOutline;
            if (transform != null)
            {
                TransformedFilledEllipse = new GeometryTransformer(FilledEllipse, transform);
                TransformedEllipesOutline = new GeometryTransformer(EllipseOutline, transform);
            }

            SmoothingMode saveSmoothingMode = e.Graphics.SmoothingMode;
            e.Graphics.SmoothingMode = SmoothingMode.None;
            {
                e.Graphics.FillGeometry(m_EllipseBrush, TransformedFilledEllipse);
            }
            e.Graphics.SmoothingMode = saveSmoothingMode;

            e.Graphics.FillGeometry(m_EllipsePen.Color, TransformedEllipesOutline);
        }
    }
}
