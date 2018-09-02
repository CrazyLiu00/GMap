//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Reflection;

using Alt.IO;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;

using ClipperLib;


namespace Alt.GUI.Demo
{
    using Polygon = List<IntPoint>;
    using Polygons = List<List<IntPoint>>;


    partial class Example_Clipper : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 0;
                int y = 0;

                if (m_TopLabel != null &&
                    m_DrawingPanel != null)
                {
                    x = Width - (m_DrawingPanel.X + m_DrawingPanel.Width);
                    y = m_DrawingPanel.Y + m_TopLabel.Height + m_TopLabel.Margin.Height;
                }

                return new SizeI(x, y);
            }
        }


		Alt.GUI.Temporary.Gwen.Control.VerticalSplitter m_Splitter;
		Alt.GUI.Temporary.Gwen.Control.Base m_RightPanel;

		Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup groupBox_Geometry;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbGeometry2;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbGeometry1;

		Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup groupBox_BooleanOperation;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbNone;
		//NoNeed	LabeledRadioButton rbXor;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbDifference;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbUnion;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbIntersect;

		Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup groupBox_Options;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown nudOffset;
		Alt.GUI.Temporary.Gwen.Control.Label lblCount;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown nudCount;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbNonZero;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton rbEvenOdd;

		Alt.GUI.Temporary.Gwen.Control.Button bRefresh;
		Alt.GUI.Temporary.Gwen.Control.Button bSave;

        DoubleBufferedControl m_DrawingPanel;

		Alt.GUI.Temporary.Gwen.Control.Label m_TopLabel;

        //TEMP  System.Windows.Forms.SaveFileDialog saveFileDialog1;
        Polygons subjects;
        Polygons clips;
        Polygons solution;

        //Here we are scaling all coordinates up by 100 when they're passed to Clipper 
        //via Polygon (or Polygons) objects because Clipper no longer accepts floating  
        //point values. Likewise when Clipper returns a solution in a Polygons object, 
        //we need to scale down these returned values by the same amount before displaying.
        int scale = 100; //or 1 or 10 or 10000 etc for lesser or greater precision.



        SizeI WorkArea
        {
            get
            {
                return m_DrawingPanel.ClientSize;// -new SizeI(0, 2 * Config.Logo.PixelHeight);
            }
        }


        
        public Example_Clipper(Base parent)
            : base(parent)
        {
            this.Margin = new Margin(0);


            //  Top Label
			m_TopLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_TopLabel.AutoSizeToContents = true;
            m_TopLabel.Text = "Clipper Demo. Use the mouse-wheel (or +, -, 0) to adjust the offset of the solution polygons.";
            m_TopLabel.TextColor = Color.Yellow;
            m_TopLabel.Dock = Pos.Top;
            m_TopLabel.Margin = new Margin(0, 5, 0, 5);

            

            m_Splitter = new VerticalSplitter(this);
            m_Splitter.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;


            //  UI Panel
            m_RightPanel = new Base(m_Splitter);
            m_RightPanel.Dock = Pos.Fill;


            //  Geometry
            groupBox_Geometry = new RadioButtonGroup(m_RightPanel, string.Empty);
            groupBox_Geometry.Text = "Geometry:";
            groupBox_Geometry.Dock = Pos.Top;
            groupBox_Geometry.Margin = new Margin(0,
#if WINDOWS_PHONE_7 || WINDOWS_PHONE_71
                5,
#else
			                                      GUI.Config.Logo.PixelHeight,
#endif
                5, 0);

            rbGeometry1 = groupBox_Geometry.AddOption("Triangles");
            rbGeometry1.Margin = new Margin(0, 5, 0, 0);
            rbGeometry2 = groupBox_Geometry.AddOption("Geo - Circles");

            rbGeometry2.Select();

            groupBox_Geometry.SelectionChanged += new GwenEventHandler(groupBox_Geometry_SelectionChanged);


            //  Boolean Op
            groupBox_BooleanOperation = new RadioButtonGroup(m_RightPanel, string.Empty);
            groupBox_BooleanOperation.Text = "Boolean Operation:";
            groupBox_BooleanOperation.Dock = Pos.Top;
            groupBox_BooleanOperation.Margin = new Margin(0, 15, 5, 0);

            rbIntersect = groupBox_BooleanOperation.AddOption("Intersect");
            rbIntersect.Margin = new Margin(0, 5, 0, 0);
            rbUnion = groupBox_BooleanOperation.AddOption("Union");
            rbDifference = groupBox_BooleanOperation.AddOption("Difference");
			//NoNeed	rbXor =
			groupBox_BooleanOperation.AddOption("XOR");
            rbNone = groupBox_BooleanOperation.AddOption("None");
            rbNone.Margin = new Margin(0, 0, 0, 7);

            rbIntersect.Select();

            groupBox_BooleanOperation.SelectionChanged += new GwenEventHandler(RefreshWORecreating);


            //  Options
			groupBox_Options = new Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup(m_RightPanel, string.Empty);
            groupBox_Options.Text = "Options:";
            groupBox_Options.Dock = Pos.Top;
            groupBox_Options.AutoSizeToContents = true;
            groupBox_Options.Margin = new Margin(0, 15, 5, 0);

            rbEvenOdd = groupBox_Options.AddOption("EvenOdd");
            rbEvenOdd.Margin = new Margin(0, 5, 0, 0);
            rbNonZero = groupBox_Options.AddOption("NonZero");

            rbNonZero.Select();

            groupBox_Options.SelectionChanged += new GwenEventHandler(RefreshWORecreating);

            //  Vertex Count
			lblCount = new Alt.GUI.Temporary.Gwen.Control.Label(m_RightPanel);//groupBox_Options);
            lblCount.AutoSizeToContents = true;
            lblCount.Text = "Vertex Count:";
            lblCount.Dock = Pos.Top;
            lblCount.Margin = new Margin(0, 17, 5, 7);
            //groupBox_Options.AddChild(lblCount);

			nudCount = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(m_RightPanel);//groupBox_Options);
            nudCount.Dock = Pos.Top;
            nudCount.Minimum = 3;
            nudCount.Maximum = 100;
            nudCount.Value = 50;
            nudCount.Margin = new Margin(0, 0, 5, 7);
            //groupBox_Options.AddChild(nudCount);

            nudCount.ValueChanged += new GwenEventHandler(nudCount_ValueChanged);

            //  Offset
			Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(m_RightPanel);//groupBox_Options);
            label.AutoSizeToContents = true;
            label.Text = "Offset:";
            label.Dock = Pos.Top;
            label.Margin = new Margin(0, 0, 5, 7);
            //groupBox_Options.AddChild(label);

			nudOffset = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(m_RightPanel);//groupBox_Options);
            nudOffset.Dock = Pos.Top;
            nudOffset.Minimum = -10;
            nudOffset.Maximum = 10;
            nudOffset.Value = 0;
            nudOffset.DecimalPlaces =
#if WINDOWS_PHONE_7 || WINDOWS_PHONE_71
                0;
#else
                1;
#endif
            nudOffset.Margin = new Margin(0, 0, 5, 7);
            //groupBox_Options.AddChild(nudOffset);

            nudOffset.ValueChanged += new GwenEventHandler(RefreshWORecreating);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            /*TEMP
            saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog1.DefaultExt = "svg";
            saveFileDialog1.Filter = "SVG Files (*.svg)|*.svg";*/


            //  Refresh
			bRefresh = new Alt.GUI.Temporary.Gwen.Control.Button(m_RightPanel);
            bRefresh.Text = "Refresh";
            bRefresh.Dock = Pos.Bottom;
			bRefresh.Margin = new Margin(0, 20, 5, GUI.Config.Logo.PixelHeight);

            bRefresh.Click += new EventHandler(bRefresh_Click);


            //  Save
			bSave = new Alt.GUI.Temporary.Gwen.Control.Button(m_RightPanel);
            bSave.Text = "Save as SVG File";
            bSave.Dock = Pos.Top;
            bSave.Margin = new Margin(0, 20, 0, 0);
            bSave.IsHidden = true;


            //  Drawing Panel
            Base DrawingPanelHolder = new Base(m_Splitter);
            m_DrawingPanel = new DoubleBufferedControl(DrawingPanelHolder);
            m_DrawingPanel.UseTransparentBackground = false;
            m_DrawingPanel.Dock = Pos.Fill;
            m_DrawingPanel.Paint += new GUI.PaintEventHandler(DrawingPanel_Paint);
            m_DrawingPanel.MouseWheel += new GUI.MouseEventHandler(DrawingPanel_MouseWheel);
            m_DrawingPanel.Resize += new EventHandler(DrawingPanel_Resize);
			m_DrawingPanel.Margin = new Margin(0, GUI.Config.Logo.PixelHeight, 0, GUI.Config.Logo.PixelHeight);
            m_DrawingPanel.DrawBorder = true;
            m_DrawingPanel.BorderColor = Color.DodgerBlue * 0.6;


            //
            m_Splitter.SetPanel(1, m_RightPanel);
            m_Splitter.SetPanel(0, DrawingPanelHolder);
            m_Splitter.SetHValue(0.83f);


            //  Neet to correct groupBoxes
            groupBox_Geometry.AutoSizeToContents = false;
            groupBox_BooleanOperation.AutoSizeToContents = false;
            groupBox_Options.AutoSizeToContents = false;


            //  Core
            subjects = new Polygons();
            clips = new Polygons();
            solution = new Polygons();


            this.Focus();
        }



        protected override void OnKeyDown(GUI.KeyEventArgs e)
        {
            base.OnKeyDown(e);

            float offset =
#if WINDOWS_PHONE_7 || WINDOWS_PHONE_71
                1;
#else
                0.5f;
#endif

            if (e.KeyCode == GUI.Keys.Subtract)
            {
                if (nudOffset.Value > -10)
                {
                    nudOffset.Value -= offset;
                }
            }
            else if (e.KeyCode == GUI.Keys.Plus ||
                e.KeyCode == GUI.Keys.Add)
            {
                if (nudOffset.Value < 10)
                {
                    nudOffset.Value += offset;
                }
            }
            else if (e.KeyCode == GUI.Keys.D0)
            {
                nudOffset.Value = 0;
            }
        }



        static Point[] PolygonToPointArray(Polygon pg, int scale)
        {
            Point[] result = new Point[pg.Count];
            for (int i = 0; i < pg.Count; ++i)
            {
                result[i].X = (float)pg[i].X / scale;
                result[i].Y = (float)pg[i].Y / scale;
            }
            return result;
        }


        void GenerateAustPlusRandomEllipses(int count)
        {
            subjects.Clear();
            //load map of Australia from resource ...
            System.IO.Stream polyStream = Alt.IO.VirtualFile.OpenRead("AltData/Clipper/aust.bin");
            int len = (int)polyStream.Length;
            byte[] b = new byte[len];
            polyStream.Read(b, 0, len);
            int polyCnt = BitConverter.ToInt32(b, 0);
            int k = 4;
            for (int i = 0; i < polyCnt; ++i)
            {
                int vertCnt = BitConverter.ToInt32(b, k);
                k += 4;
                Polygon pg = new Polygon(vertCnt);
                for (int j = 0; j < vertCnt; ++j)
                {
                    float x = BitConverter.ToSingle(b, k) * scale;
                    float y = BitConverter.ToSingle(b, k + 4) * scale;
                    k += 8;
                    pg.Add(new IntPoint((int)x, (int)y));
                }
                subjects.Add(pg);
            }

            clips.Clear();
            Random rand = new Random();
            GraphicsPath path = new GraphicsPath();
            PointI pt = new PointI();

            const int ellipse_size = 100, margin = 10;
            for (int i = 0; i < count; ++i)
            {
                int w = WorkArea.Width - ellipse_size - margin * 2;
                int h = WorkArea.Height - ellipse_size - margin * 2;

                pt.X = rand.Next(w) + margin;
                pt.Y = rand.Next(h) + margin;
                int size = rand.Next(ellipse_size - 20) + 20;
                path.Reset();
                path.AddEllipse(pt.X, pt.Y, size, size);
                path.Flatten();
                Polygon clip = new Polygon(path.PathPoints.Length);//Count());
                foreach (Point p in path.PathPoints)
                {
                    clip.Add(new IntPoint((int)(p.X * scale), (int)(p.Y * scale)));
                }

                clips.Add(clip);
            }
        }


        IntPoint GenerateRandomPoint(int l, int t, int r, int b, Random rand)
        {
            int Q = 10;
            IntPoint newPt = new IntPoint();
            newPt.X = (rand.Next(r / Q) * Q + l + 10) * scale;
            newPt.Y = (rand.Next(b / Q) * Q + t + 10) * scale;
            return newPt;
        }


        void GenerateRandomPolygon(int count)
        {
            int Q = 10;
            Random rand = new Random();
            int l = 10;
            int t = 10;
            int r = (WorkArea.Width - 20) / Q * Q;
            int b = (WorkArea.Height - 20) / Q * Q;

            subjects.Clear();
            clips.Clear();

            Polygon subj = new Polygon(count);
            for (int i = 0; i < count; ++i)
            {
                subj.Add(GenerateRandomPoint(l, t, r, b, rand));
            }
            subjects.Add(subj);

            Polygon clip = new Polygon(count);
            for (int i = 0; i < count; ++i)
            {
                clip.Add(GenerateRandomPoint(l, t, r, b, rand));
            }
            clips.Add(clip);
        }


        ClipType GetClipType()
        {
            if (rbIntersect.IsChecked)
            {
                return ClipType.ctIntersection;
            }

            if (rbUnion.IsChecked)
            {
                return ClipType.ctUnion;
            }

            if (rbDifference.IsChecked)
            {
                return ClipType.ctDifference;
            }

            return ClipType.ctXor;
        }


        PolyFillType GetPolyFillType()
        {
            if (rbNonZero.IsChecked)
            {
                return PolyFillType.pftNonZero;
            }

            return PolyFillType.pftEvenOdd;
        }


        void Refresh(bool fCreateNewData)
        {
            if (fCreateNewData)
            {
                int n = (int)nudCount.Value;

                if (rbGeometry2.IsChecked)
                {
                    GenerateAustPlusRandomEllipses(n);
                }
                else
                {
                    GenerateRandomPolygon(n);
                }
            }

            m_DrawingPanel.Refresh();
        }


        void DrawingPanel_Resize(object sender, EventArgs e)
        {
            Refresh(true);
        }


        void DrawingPanel_MouseWheel(object sender, GUI.MouseEventArgs e)
        {
            float offset =
#if WINDOWS_PHONE_7 || WINDOWS_PHONE_71
                1;
#else
                0.5f;
#endif

            if (e.Delta > 0 && nudOffset.Value < 10)
            {
                nudOffset.Value += offset;
            }
            else if (e.Delta < 0 && nudOffset.Value > -10)
            {
                nudOffset.Value -= offset;
            }
        }


        void nudCount_ValueChanged(Base control)
        {
            Refresh(true);
        }


        void bRefresh_Click(object sender, EventArgs e)
        {
            Refresh(true);
        }


        void RefreshWORecreating(Base control)
        {
            Refresh(false);
        }


        void groupBox_Geometry_SelectionChanged(Base control)
        {
            if (rbGeometry1.IsChecked)
            {
                lblCount.Text = "Vertex &Count:";
            }
            else
            {
                lblCount.Text = "Ellipse &Count:";
            }

            Refresh(true);
        }


        void DrawingPanel_Paint(object sender, GUI.PaintEventArgs e)
        {
            GUI.Cursor.Current = GUI.Cursors.WaitCursor;

            Graphics graphics = e.Graphics;
            SmoothingMode fillSmoothingMode = graphics is SoftwareGraphics ? SmoothingMode.AntiAlias : SmoothingMode.None;
            SmoothingMode strokeSmoothingMode = SmoothingMode.AntiAlias;
            graphics.SmoothingMode = fillSmoothingMode;
            
            graphics.Clear(Color.White);


            GraphicsPath path = new GraphicsPath();
            if (rbNonZero.IsChecked)
            {
                path.FillMode = FillMode.Winding;
            }

            //draw subjects ...
            foreach (Polygon pg in subjects)
            {
                Point[] pts = PolygonToPointArray(pg, scale);
                path.AddPolygon(pts);
                pts = null;
            }

            Pen myPen = new Pen(
                //Color.FromArgb(196, 0xC3, 0xC9, 0xCF),
                Color.FromArgb(128, 0, 0, 255),
                0.6);
            SolidColorBrush myBrush = new SolidColorBrush(
                //Color.FromArgb(127, 0xDD, 0xDD, 0xF0));
                //Color.FromArgb(255, 210, 210, 255));
                Color.FromArgb(127, Color.LightBlue));

            graphics.SmoothingMode = fillSmoothingMode;
            graphics.FillPath(myBrush, path);
            graphics.SmoothingMode = strokeSmoothingMode;
            graphics.DrawPath(myPen, path);
            path.Reset();

            //draw clips ...
            if (rbNonZero.IsChecked) path.FillMode = FillMode.Winding;
            foreach (Polygon pg in clips)
            {
                Point[] pts = PolygonToPointArray(pg, scale);
                path.AddPolygon(pts);
                pts = null;
            }
            myPen.Color = Color.FromArgb(128, Color.Red);//0xF9, 0xBE, 0xA6);
            myBrush.Color = //Color.FromArgb(127, 0xFF, 0xE0, 0xE0);
            Color.FromArgb(50, 255, 0, 0);

            graphics.SmoothingMode = fillSmoothingMode;
            graphics.FillPath(myBrush, path);
            graphics.SmoothingMode = strokeSmoothingMode;
            graphics.DrawPath(myPen, path);

            //do the clipping ...
            if ((clips.Count > 0 || subjects.Count > 0) &&
                !rbNone.IsChecked)
            {
                Polygons solution2 = new Polygons();
                Clipper c = new Clipper(0);
                c.AddPaths(subjects, PolyType.ptSubject, true);
                c.AddPaths(clips, PolyType.ptClip, true);
                solution.Clear();
                bool succeeded = c.Execute(GetClipType(), solution, GetPolyFillType(), GetPolyFillType());
                if (succeeded)
                {
                    myBrush.Color = Color.Black;
                    path.Reset();

                    //It really shouldn't matter what FillMode is used for solution
                    //polygons because none of the solution polygons overlap. 
                    //However, FillMode.Winding will show any orientation errors where 
                    //holes will be stroked (outlined) correctly but filled incorrectly  ...
                    path.FillMode = FillMode.Winding;

                    //or for something fancy ...
                    if (nudOffset.Value != 0)
                    {
                        //old   solution2 = Clipper.OffsetPolygons(solution, (double)nudOffset.Value * scale, JoinType.jtMiter);
                        ClipperOffset co = new ClipperOffset(2, 0.25);
                        co.AddPaths(solution, JoinType.jtMiter, EndType.etClosedPolygon);
                        co.Execute(ref solution2, (double)nudOffset.Value * scale);
                    }
                    else
                    {
                        solution2 = new Polygons(solution);
                    }

                    foreach (Polygon pg in solution2)
                    {
                        Point[] pts = PolygonToPointArray(pg, scale);
                        if (pts.Length//Count()
                            > 2)
                            path.AddPolygon(pts);
                        pts = null;
                    }
                    //myBrush.Color = Color.FromArgb(127, 0x66, 0xEF, 0x7F);
                    myBrush.Color = Color.FromArgb(100, 0, 255, 0);
                    myPen.Color = Color.FromArgb(255, 0, 0x33, 0);
                    myPen.Width = 1;

                    graphics.SmoothingMode = fillSmoothingMode;
                    graphics.FillPath(myBrush, path);
                    graphics.SmoothingMode = strokeSmoothingMode;
                    graphics.DrawPath(myPen, path);

                    //now do some fancy testing ...
                    Font f = new Font("Arial", 8);
                    Brush b = Brushes.Navy;
                    double subj_area = 0, clip_area = 0, int_area = 0, union_area = 0;
                    c.Clear();
                    c.AddPaths(subjects, PolyType.ptSubject, true);
                    c.Execute(ClipType.ctUnion, solution2, GetPolyFillType(), GetPolyFillType());
                    foreach (Polygon pg in solution2) subj_area += Clipper.Area(pg);
                    c.Clear();
                    c.AddPaths(clips, PolyType.ptClip, true);
                    c.Execute(ClipType.ctUnion, solution2, GetPolyFillType(), GetPolyFillType());
                    foreach (Polygon pg in solution2) clip_area += Clipper.Area(pg);
                    c.AddPaths(subjects, PolyType.ptSubject, true);
                    c.Execute(ClipType.ctIntersection, solution2, GetPolyFillType(), GetPolyFillType());
                    foreach (Polygon pg in solution2) int_area += Clipper.Area(pg);
                    c.Execute(ClipType.ctUnion, solution2, GetPolyFillType(), GetPolyFillType());
                    foreach (Polygon pg in solution2) union_area += Clipper.Area(pg);

                    StringFormat lftStringFormat = new StringFormat();
                    lftStringFormat.Alignment = StringAlignment.Near;
                    lftStringFormat.LineAlignment = StringAlignment.Near;
                    StringFormat rtStringFormat = new StringFormat();
                    rtStringFormat.Alignment = StringAlignment.Far;
                    rtStringFormat.LineAlignment = StringAlignment.Near;
                    RectI rec = new RectI(WorkArea.Width - 114, WorkArea.Height - 116, 104, 106);
                    graphics.FillRectangle(new SolidColorBrush(Color.FromArgb(196, Color.WhiteSmoke)), rec);
                    graphics.DrawRectangle(myPen, rec);
                    rec.Inflate(new SizeI(-2, 0));

                    //  because of Alt.Sketch smaller top offset
                    rec += new PointI(0, 1);
                    rec -= new SizeI(0, 1);

                    graphics.DrawString("Areas", f, b, rec, rtStringFormat);
                    rec.Offset(new PointI(0, 14));
                    graphics.DrawString("subj: ", f, b, rec, lftStringFormat);
                    graphics.DrawString((subj_area / 100000).ToString("0,0"), f, b, rec, rtStringFormat);
                    rec.Offset(new PointI(0, 12));
                    graphics.DrawString("clip: ", f, b, rec, lftStringFormat);
                    graphics.DrawString((clip_area / 100000).ToString("0,0"), f, b, rec, rtStringFormat);
                    rec.Offset(new PointI(0, 12));
                    graphics.DrawString("intersect: ", f, b, rec, lftStringFormat);
                    graphics.DrawString((int_area / 100000).ToString("0,0"), f, b, rec, rtStringFormat);
                    rec.Offset(new PointI(0, 12));
                    graphics.DrawString("---------", f, b, rec, rtStringFormat);
                    rec.Offset(new PointI(0, 10));
                    graphics.DrawString("s + c - i: ", f, b, rec, lftStringFormat);
                    graphics.DrawString(((subj_area + clip_area - int_area) / 100000).ToString("0,0"), f, b, rec, rtStringFormat);
                    rec.Offset(new PointI(0, 10));
                    graphics.DrawString("---------", f, b, rec, rtStringFormat);
                    rec.Offset(new PointI(0, 10));
                    graphics.DrawString("union: ", f, b, rec, lftStringFormat);
                    graphics.DrawString((union_area / 100000).ToString("0,0"), f, b, rec, rtStringFormat);
                    rec.Offset(new PointI(0, 10));
                    graphics.DrawString("---------", f, b, rec, rtStringFormat);
                } //end if succeeded
            } //end if something to clip

            //TEMP  pictureBox1.Image = mybitmap;
            graphics.Dispose();
            GUI.Cursor.Current = GUI.Cursors.Default;
        }


        bool LoadFromFile(string filename, Polygons ppg, double scale// = 0
            , int xOffset// = 0
            , int yOffset)// = 0)
        {
            double scaling = Math.Pow(10, scale);
            ppg.Clear();
            if (!Alt.IO.File.Exists(filename)) return false;
            System.IO.StreamReader sr = new System.IO.StreamReader(filename);
            if (sr == null) return false;
            string line;
            if ((line = sr.ReadLine()) == null) return false;
            int polyCnt, vertCnt;
            if (!Int32.TryParse(line, out polyCnt) || polyCnt < 0) return false;
            ppg.Capacity = polyCnt;
            for (int i = 0; i < polyCnt; i++)
            {
                if ((line = sr.ReadLine()) == null) return false;
                if (!Int32.TryParse(line, out vertCnt) || vertCnt < 0) return false;
                Polygon pg = new Polygon(vertCnt);
                ppg.Add(pg);
                for (int j = 0; j < vertCnt; j++)
                {
                    double x, y;
                    if ((line = sr.ReadLine()) == null) return false;
                    char[] delimiters = new char[] { ',', ' ' };
                    string[] vals = line.Split(delimiters);
                    if (vals.Length < 2) return false;
                    if (!double.TryParse(vals[0], out x)) return false;
                    if (!double.TryParse(vals[1], out y))
                        if (vals.Length < 2 || !double.TryParse(vals[2], out y)) return false;
                    x = x * scaling + xOffset;
                    y = y * scaling + yOffset;
                    pg.Add(new IntPoint((int)Math.Round(x), (int)Math.Round(y)));
                }
            }
            return true;
        }


        void SaveToFile(string filename, Polygons ppg, int scale)// = 0)
        {
            double scaling = Math.Pow(10, scale);
            System.IO.StreamWriter writer = new System.IO.StreamWriter(filename);
            if (writer == null) return;
            writer.Write("{0}\n", ppg.Count);
            foreach (Polygon pg in ppg)
            {
                writer.Write("{0}\n", pg.Count);
                foreach (IntPoint ip in pg)
                    writer.Write("{0:0.0000}, {1:0.0000}\n",
                        (double)ip.X / scaling, (double)ip.Y / scaling);
            }
            writer.Close();
        }


        void bSave_Click(object sender, EventArgs e)
        {
            /*TEMP
            //save to SVG ...
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                PolyFillType pft = GetPolyFillType();
                SVGBuilder svg = new SVGBuilder();
                svg.style.brushClr = Color.FromArgb(0x10, 0, 0, 0x9c);
                svg.style.penClr = Color.FromArgb(0xd3, 0xd3, 0xda);
                svg.AddPaths(subjects);
                svg.style.brushClr = Color.FromArgb(0x10, 0x9c, 0, 0);
                svg.style.penClr = Color.FromArgb(0xff, 0xa0, 0x7a);
                svg.AddPaths(clips);
                svg.style.brushClr = Color.FromArgb(0xAA, 0x80, 0xff, 0x9c);
                svg.style.penClr = Color.FromArgb(0, 0x33, 0);
                svg.AddPaths(solution);
                svg.SaveToFile(saveFileDialog1.FileName, 1.0 / scale);
            }*/
        }
    }
}
