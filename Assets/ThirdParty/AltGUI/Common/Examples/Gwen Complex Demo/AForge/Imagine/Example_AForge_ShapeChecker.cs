// Simple Shape Checker sample application
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright � AForge.NET, 2006-2011
// contacts@aforgenet.com

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Alt.ComponentModel;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;
using Alt.Threading;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;


namespace Alt.GUI.Demo
{
    class Example_AForge_ShapeChecker : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 0;
                int y = 0;

                if (m_Top1 != null &&
                    m_Top2 != null)
                {
                    y = m_Top1.Height + m_Top2.Height;
                }

                return new SizeI(x, y);
            }
        }


        string Description
        {
            get
            {
				return "AltGUI AForge.NET Simple Shape Checker Example";
            }
        }


        Bitmap Screenshot
        {
            get
            {
                return Example_NotAvailable_ScreenShot.LoadImage("Example_AForge_ShapeChecker.jpg");
            }
        }


		Alt.GUI.Temporary.Gwen.Control.Base m_Top1;
		Alt.GUI.Temporary.Gwen.Control.Base m_Top2;
		Alt.GUI.Temporary.Gwen.Control.PictureBox pictureBox;
		Alt.GUI.Temporary.Gwen.Control.Button loadDemoImage1ToolStripMenuItem;
		Alt.GUI.Temporary.Gwen.Control.Button loadDemoImage2ToolStripMenuItem;
		Alt.GUI.Temporary.Gwen.Control.Button loadDemoImage3ToolStripMenuItem;
		Alt.GUI.Temporary.Gwen.Control.Button loadDemoImage4ToolStripMenuItem;

                
        public Example_AForge_ShapeChecker(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


			Alt.GUI.Temporary.Gwen.Control.Label label;
#if SILVERLIGHT || UNITY_WEBPLAYER
			label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            label.AutoSizeToContents = true;
            label.Text = //Description + "\n" + "(This example is not available in this Demo, please download SDK)";
				"THIS EXAMPLE IS NOT AVAILABLE IN THIS DEMO,\nPLEASE DOWNLOAD AltGUI SDK";
            label.TextColor = Color.Orange * 1.2;
            label.Dock = Pos.Top;
            label.Margin = new Margin(0, 0, 0, 5);
            label.Font = Example_NotAvailable_ScreenShot.Font;
#endif


            //  GUI
            {
                m_Top1 = new Base(this);
                {
                    m_Top1.Dock = Pos.Top;
                    m_Top1.Height = 30;

					loadDemoImage1ToolStripMenuItem = new Alt.GUI.Temporary.Gwen.Control.Button(m_Top1);
					loadDemoImage2ToolStripMenuItem = new Alt.GUI.Temporary.Gwen.Control.Button(m_Top1);
					loadDemoImage3ToolStripMenuItem = new Alt.GUI.Temporary.Gwen.Control.Button(m_Top1);
					loadDemoImage4ToolStripMenuItem = new Alt.GUI.Temporary.Gwen.Control.Button(m_Top1);

                    loadDemoImage1ToolStripMenuItem.Text = "Load demo image 1";
                    loadDemoImage1ToolStripMenuItem.Click += new System.EventHandler(loadDemoImage1ToolStripMenuItem_Click);
                    loadDemoImage1ToolStripMenuItem.Dock = Pos.Left;
                    loadDemoImage1ToolStripMenuItem.AutoSizeToContents = true;
                    loadDemoImage1ToolStripMenuItem.NormalTextColor = Color.Red * 0.8;

                    loadDemoImage2ToolStripMenuItem.Text = "Load demo image 2";
                    loadDemoImage2ToolStripMenuItem.Click += new System.EventHandler(loadDemoImage2ToolStripMenuItem_Click);
                    loadDemoImage2ToolStripMenuItem.Dock = Pos.Left;
                    loadDemoImage2ToolStripMenuItem.Margin = new Margin(10, 0, 0, 0);
                    loadDemoImage2ToolStripMenuItem.AutoSizeToContents = true;
                    loadDemoImage2ToolStripMenuItem.NormalTextColor = Color.Brown;

                    loadDemoImage3ToolStripMenuItem.Text = "Load demo image 3";
                    loadDemoImage3ToolStripMenuItem.Click += new System.EventHandler(loadDemoImage3ToolStripMenuItem_Click);
                    loadDemoImage3ToolStripMenuItem.Dock = Pos.Left;
                    loadDemoImage3ToolStripMenuItem.Margin = new Margin(10, 0, 0, 0);
                    loadDemoImage3ToolStripMenuItem.AutoSizeToContents = true;
                    loadDemoImage3ToolStripMenuItem.NormalTextColor = Color.Green;

                    loadDemoImage4ToolStripMenuItem.Text = "Load demo image 4";
                    loadDemoImage4ToolStripMenuItem.Click += new System.EventHandler(loadDemoImage4ToolStripMenuItem_Click);
                    loadDemoImage4ToolStripMenuItem.Dock = Pos.Left;
                    loadDemoImage4ToolStripMenuItem.Margin = new Margin(10, 0, 0, 0);
                    loadDemoImage4ToolStripMenuItem.AutoSizeToContents = true;
                    loadDemoImage4ToolStripMenuItem.NormalTextColor = Color.Blue;
                }


				m_Top2 = new Alt.GUI.Temporary.Gwen.Control.Base(this);
                {
                    m_Top2.Dock = Pos.Top;
                    m_Top2.Height = 25;

					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Top2);
                    label.AutoSizeToContents = true;
                    label.Text = "Legend:";
                    label.Dock = Pos.Left;
                    label.TextColor = Color.Orange;
                    label.Margin = new Margin(0, 4, 0, 0);


                    SizeI boxSize = new SizeI(m_Top2.Height, m_Top2.Height);
                    Margin boxMargin = new Margin(10, 0, 0, 0);
                    Margin labelMargin = new Margin(5, 4, 0, 0);


                    //  1
					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Top2);
                    label.ClientBackColor = Color.Yellow;
                    label.DrawBorder = true;
                    label.BorderColor = Color.DodgerBlue;
                    label.Dock = Pos.Left;
                    label.Margin = boxMargin;
                    label.Size = boxSize;

					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Top2);
                    label.Dock = Pos.Left;
                    label.Margin = labelMargin;
                    label.AutoSizeToContents = true;
                    label.Text = "Circles";


                    //  2
					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Top2);
                    label.ClientBackColor = Color.Red;
                    label.DrawBorder = true;
                    label.BorderColor = Color.DodgerBlue;
                    label.Dock = Pos.Left;
                    label.Margin = boxMargin;
                    label.Size = boxSize;

					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Top2);
                    label.Dock = Pos.Left;
                    label.Margin = labelMargin;
                    label.AutoSizeToContents = true;
                    label.Text = "Quadrilaterals";


                    //  3
					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Top2);
                    label.ClientBackColor = Color.Brown;
                    label.DrawBorder = true;
                    label.BorderColor = Color.DodgerBlue;
                    label.Dock = Pos.Left;
                    label.Margin = boxMargin;
                    label.Size = boxSize;
                    label.SetToolTipText("Trapezoid, Parallelogram, Rectangle, Rhombus or Square");
                    label.MouseInputEnabled = true;

					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Top2);
                    label.Dock = Pos.Left;
                    label.Margin = labelMargin;
                    label.AutoSizeToContents = true;
                    label.Text = "Known quadrilaterals";
                    label.SetToolTipText("Trapezoid, Parallelogram, Rectangle, Rhombus or Square");
                    label.MouseInputEnabled = true;


                    //  4
					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Top2);
                    label.ClientBackColor = Color.Blue;
                    label.DrawBorder = true;
                    label.BorderColor = Color.DodgerBlue;
                    label.Dock = Pos.Left;
                    label.Margin = boxMargin;
                    label.Size = boxSize;

					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Top2);
                    label.Dock = Pos.Left;
                    label.Margin = labelMargin;
                    label.AutoSizeToContents = true;
                    label.Text = "Triangles";


                    //  5
					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Top2);
                    label.ClientBackColor = Color.Green;
                    label.DrawBorder = true;
                    label.BorderColor = Color.DodgerBlue;
                    label.Dock = Pos.Left;
                    label.Margin = boxMargin;
                    label.Size = boxSize;
                    label.SetToolTipText("Equilateral, Isosceles, Rectangled or Rectangled Isosceles Triangle");
                    label.MouseInputEnabled = true;

					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Top2);
                    label.Dock = Pos.Left;
                    label.Margin = labelMargin;
                    label.AutoSizeToContents = true;
                    label.Text = "Known triangles";
                    label.SetToolTipText("Equilateral, Isosceles, Rectangled or Rectangled Isosceles Triangle");
                    label.MouseInputEnabled = true;
                }


				pictureBox = new Alt.GUI.Temporary.Gwen.Control.PictureBox(this);
                pictureBox.Dock = Pos.Fill;
				pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            }


            LoadDemo("coins.jpg");
        }



        // Load first demo image
        void loadDemoImage1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDemo("coins.jpg");
        }


        // Load second demo image
        void loadDemoImage2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDemo("demo.gif");
        }


        // Load third demo image
        void loadDemoImage3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDemo("demo1.gif");
        }


        // Load fourth demo image
        void loadDemoImage4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDemo("demo2.gif");
        }


        // Load one of the embedded demo image
        void LoadDemo(string fileName)
        {
            // load arrow bitmap
            Bitmap image = Bitmap.FromFile("AltData/AForge/" + fileName);
            ProcessImage(image);
        }


        // Process image
        void ProcessImage(Bitmap bitmap)
        {
#if !SILVERLIGHT && !UNITY_WEBPLAYER
            // lock image
            BitmapData bitmapData = bitmap.LockBits(
                new RectI(0, 0, bitmap.PixelWidth, bitmap.PixelHeight),
                ImageLockMode.ReadWrite, bitmap.PixelFormat);

            // step 1 - turn background to black
            ColorFiltering colorFilter = new ColorFiltering();

            colorFilter.Red = new IntRange(0, 64);
            colorFilter.Green = new IntRange(0, 64);
            colorFilter.Blue = new IntRange(0, 64);
            colorFilter.FillOutsideRange = false;

            colorFilter.ApplyInPlace(bitmapData);

            // step 2 - locating objects
            BlobCounter blobCounter = new BlobCounter();

            blobCounter.FilterBlobs = true;
            blobCounter.MinHeight = 5;
            blobCounter.MinWidth = 5;

            blobCounter.ProcessImage(bitmapData);
            Blob[] blobs = blobCounter.GetObjectsInformation();
            bitmap.UnlockBits(bitmapData);

            // step 3 - check objects' type and highlight
            SimpleShapeChecker shapeChecker = new SimpleShapeChecker();

            Graphics g = Graphics.FromImage(bitmap);
            Pen yellowPen = new Pen(Color.Yellow, 2); // circles
            Pen redPen = new Pen(Color.Red, 2);       // quadrilateral
            Pen brownPen = new Pen(Color.Brown, 2);   // quadrilateral with known sub-type
            Pen greenPen = new Pen(Color.Green, 2);   // known triangle
            Pen bluePen = new Pen(Color.Blue, 2);     // triangle

            for (int i = 0, n = blobs.Length; i < n; i++)
            {
                List<IntPoint> edgePoints = blobCounter.GetBlobsEdgePoints(blobs[i]);

                global::AForge.Point center;
                float radius;

                // is circle ?
                if (shapeChecker.IsCircle(edgePoints, out center, out radius))
                {
                    g.DrawEllipse(yellowPen,
                        (float)(center.X - radius), (float)(center.Y - radius),
                        (float)(radius * 2), (float)(radius * 2));
                }
                else
                {
                    List<IntPoint> corners;

                    // is triangle or quadrilateral
                    if (shapeChecker.IsConvexPolygon(edgePoints, out corners))
                    {
                        // get sub-type
                        PolygonSubType subType = shapeChecker.CheckPolygonSubType(corners);

                        Pen pen;

                        if (subType == PolygonSubType.Unknown)
                        {
                            pen = (corners.Count == 4) ? redPen : bluePen;
                        }
                        else
                        {
                            pen = (corners.Count == 4) ? brownPen : greenPen;
                        }

                        g.DrawPolygon(pen, ToPointsArray(corners));
                    }
                }
            }

            yellowPen.Dispose();
            redPen.Dispose();
            greenPen.Dispose();
            bluePen.Dispose();
            brownPen.Dispose();
            g.Dispose();
#endif

            // and to picture box
            pictureBox.Image = bitmap;
        }


        // Conver list of AForge.NET's points to array of .NET points
        PointI[] ToPointsArray(List<IntPoint> points)
        {
            PointI[] array = new PointI[points.Count];

            for (int i = 0, n = points.Count; i < n; i++)
            {
                array[i] = new PointI(points[i].X, points[i].Y);
            }

            return array;
        }
    }
}
