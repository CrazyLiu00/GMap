// Blobs Browser sample application
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2006-2011
// contacts@aforgenet.com

using System;
using System.Collections.Generic;
using System.Text;

using Alt.ComponentModel;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;

using AForge;
using AForge.Imaging;
using AForge.Math.Geometry;


namespace Alt.GUI.Demo.BlobsExplorer
{
#if !SILVERLIGHT && !UNITY_WEBPLAYER
    public delegate void BlobSelectionHandler(object sender, Blob blob);
#endif


	public partial class BlobsBrowser : Alt.GUI.Temporary.Gwen.Control.Label//DoubleBufferedControl
    {
        Bitmap image = null;
        int imageWidth, imageHeight;

#if !SILVERLIGHT && !UNITY_WEBPLAYER
        BlobCounter blobCounter = new BlobCounter();
        Blob[] blobs;

        // Event to notify about selected blob
        public event BlobSelectionHandler BlobSelected;
		
		int selectedBlobID;
#endif

        Dictionary<int, List<IntPoint>> leftEdges = new Dictionary<int, List<IntPoint>>();
        Dictionary<int, List<IntPoint>> rightEdges = new Dictionary<int, List<IntPoint>>();
        Dictionary<int, List<IntPoint>> topEdges = new Dictionary<int, List<IntPoint>>();
        Dictionary<int, List<IntPoint>> bottomEdges = new Dictionary<int, List<IntPoint>>();

        Dictionary<int, List<IntPoint>> hulls = new Dictionary<int, List<IntPoint>>();
        Dictionary<int, List<IntPoint>> quadrilaterals = new Dictionary<int, List<IntPoint>>();

        // Blobs' highlight types enumeration
        public enum HightlightType
        {
            ConvexHull,
            LeftAndRightEdges,
            TopAndBottomEdges,
            Quadrilateral
        }


        HightlightType highlighting = HightlightType.Quadrilateral;
        bool showRectangleAroundSelection = false;


        // Blobs' highlight type
        public HightlightType Highlighting
        {
            get
            {
                return highlighting;
            }
            set
            {
                highlighting = value;

                Refresh();// Invalidate();
            }
        }


        // Show rectangle around selection or not
        public bool ShowRectangleAroundSelection
        {
            get
            {
                return showRectangleAroundSelection;
            }
            set
            {
                showRectangleAroundSelection = value;

                Refresh();// Invalidate();
            }
        }


        public BlobsBrowser(Base parent) :
            base(parent)
        {
            MouseInputEnabled = true;
        }


        // Set image to display by the control
        public int SetImage(Bitmap image)
        {
            leftEdges.Clear();
            rightEdges.Clear();
            topEdges.Clear();
            bottomEdges.Clear();
            hulls.Clear();
            quadrilaterals.Clear();

            this.image = global::AForge.Imaging.Image.Clone(image, PixelFormat.Format24bppRgb);
            imageWidth = this.image.PixelWidth;
            imageHeight = this.image.PixelHeight;

#if !SILVERLIGHT && !UNITY_WEBPLAYER
			selectedBlobID = 0;
			
			blobCounter.ProcessImage(this.image);
            blobs = blobCounter.GetObjectsInformation();

            GrahamConvexHull grahamScan = new GrahamConvexHull();

            foreach (Blob blob in blobs)
            {
                List<IntPoint> leftEdge = new List<IntPoint>();
                List<IntPoint> rightEdge = new List<IntPoint>();
                List<IntPoint> topEdge = new List<IntPoint>();
                List<IntPoint> bottomEdge = new List<IntPoint>();

                // collect edge points
                blobCounter.GetBlobsLeftAndRightEdges(blob, out leftEdge, out rightEdge);
                blobCounter.GetBlobsTopAndBottomEdges(blob, out topEdge, out bottomEdge);

                leftEdges.Add(blob.ID, leftEdge);
                rightEdges.Add(blob.ID, rightEdge);
                topEdges.Add(blob.ID, topEdge);
                bottomEdges.Add(blob.ID, bottomEdge);

                // find convex hull
                List<IntPoint> edgePoints = new List<IntPoint>();
                edgePoints.AddRange(leftEdge);
                edgePoints.AddRange(rightEdge);

                List<IntPoint> hull = grahamScan.FindHull(edgePoints);
                hulls.Add(blob.ID, hull);

                List<IntPoint> quadrilateral = null;

                // find quadrilateral
                if (hull.Count < 4)
                {
                    quadrilateral = new List<IntPoint>(hull);
                }
                else
                {
                    quadrilateral = PointsCloud.FindQuadrilateralCorners(hull);
                }
                quadrilaterals.Add(blob.ID, quadrilateral);

                // shift all points for vizualization
                IntPoint shift = new IntPoint(1, 1);

                PointsCloud.Shift(leftEdge, shift);
                PointsCloud.Shift(rightEdge, shift);
                PointsCloud.Shift(topEdge, shift);
                PointsCloud.Shift(bottomEdge, shift);
                PointsCloud.Shift(hull, shift);
                PointsCloud.Shift(quadrilateral, shift);
            }
#endif

            UpdateDrawOffset();

            Refresh();// Invalidate();

            return
#if SILVERLIGHT || UNITY_WEBPLAYER
                0;
#else
                blobs.Length;
#endif
        }


        // Paint the control
        PointI m_DrawOffset = PointI.Empty;
        protected override void OnPaint(GUI.PaintEventArgs e)
        {
            UpdateDrawOffset();

            Graphics g = e.Graphics;
            g.TranslateTransform(m_DrawOffset);

            if (image != null)
            {
                //g.DrawImage(image, rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
                g.DrawImage(image, PointI.One);
                RectI r = image.PixelRectangle + SizeI.One;
                if (r.X > 0)
                {
                    r.X -= 1;
                    r.Width += 2;
                }
                if (r.Y > 0)
                {
                    r.Y -= 1;
                    r.Height += 2;
                }
                g.DrawRectangle(Color.DodgerBlue * 0.6, r);


#if !SILVERLIGHT && !UNITY_WEBPLAYER				
				Pen highlightPen = new Pen(Color.Red);
				Pen highlightPenBold = new Pen(Color.FromArgb(0, 255, 0), 3);
				Pen rectPen = new Pen(Color.Blue);

				foreach (Blob blob in blobs)
                {
                    Pen pen = (blob.ID == selectedBlobID) ? highlightPenBold : highlightPen;

                    if ((showRectangleAroundSelection) && (blob.ID == selectedBlobID))
                    {
                        g.DrawRectangle(rectPen, blob.Rectangle);
                    }

                    switch (highlighting)
                    {
                        case HightlightType.ConvexHull:
                            g.DrawPolygon(pen, PointsListToArray(hulls[blob.ID]));
                            break;
                        case HightlightType.LeftAndRightEdges:
                            DrawEdge(g, pen, leftEdges[blob.ID]);
                            DrawEdge(g, pen, rightEdges[blob.ID]);
                            break;
                        case HightlightType.TopAndBottomEdges:
                            DrawEdge(g, pen, topEdges[blob.ID]);
                            DrawEdge(g, pen, bottomEdges[blob.ID]);
                            break;
                        case HightlightType.Quadrilateral:
                            g.DrawPolygon(pen, PointsListToArray(quadrilaterals[blob.ID]));
                            break;
                    }
                }
#endif
            }
            else
            {
                g.FillRectangle(new SolidColorBrush(Color.FromArgb(128, 128, 128)),
                    ClientRectangle);//rect.X + 1, rect.Y + 1, rect.Width - 2, rect.Height - 2);
            }

            base.OnPaint(e);
        }


        // Update controls size and position
        void UpdateDrawOffset()
        {
            if (this.Parent != null)
            {
                RectI rc = ClientRectangle;
                int width = 320;
                int height = 240;

                if (image != null)
                {
                    // get frame size
                    width = imageWidth;
                    height = imageHeight;
                }

                // update controls size and location
                m_DrawOffset = new PointI((rc.Width - width - 2) / 2, (rc.Height - height - 2) / 2);
            }
        }


        // On mouse moving - update cursor
        protected override void OnMouseMove(GUI.MouseEventArgs e)
        {
            base.OnMouseMove(e);

            UpdateDrawOffset();

#if !SILVERLIGHT && !UNITY_WEBPLAYER
			int x = e.X - m_DrawOffset.X - 1;
			int y = e.Y - m_DrawOffset.Y - 1;
			
			if ((image != null) && (x >= 0) && (y >= 0) &&
                 (x < imageWidth) && (y < imageHeight) &&
                 (blobCounter.ObjectLabels[y * imageWidth + x] != 0))
            {
                this.Cursor = GUI.Cursors.Hand;
            }
            else
            {
                this.Cursor = GUI.Cursors.Default;
            }
#endif
        }


        // On mouse click - notify user if blob was clicked
        protected override void OnMouseClick(GUI.MouseEventArgs e)
        {
            base.OnMouseClick(e);

            UpdateDrawOffset();

#if !SILVERLIGHT && !UNITY_WEBPLAYER
			int x = e.X - m_DrawOffset.X - 1;
			int y = e.Y - m_DrawOffset.Y - 1;
			
			if ((image != null) && (x >= 0) && (y >= 0) &&
                 (x < imageWidth) && (y < imageHeight))
            {
                int blobID = blobCounter.ObjectLabels[y * imageWidth + x];

                if (blobID != 0)
                {
                    selectedBlobID = blobID;

                    Refresh();// Invalidate();

                    if (BlobSelected != null)
                    {
                        for (int i = 0; i < blobs.Length; i++)
                        {
                            if (blobs[i].ID == blobID)
                            {
                                BlobSelected(this, blobs[i]);
                            }
                        }
                    }
                }
            }
#endif
        }


        // Convert list of AForge.NET's IntPoint to array of .NET's Point
        static PointI[] PointsListToArray(List<IntPoint> list)
        {
            PointI[] array = new PointI[list.Count];

            for (int i = 0, n = list.Count; i < n; i++)
            {
                array[i] = new PointI(list[i].X, list[i].Y);
            }

            return array;
        }


        // Draw object's edge
        static void DrawEdge(Graphics g, Pen pen, List<IntPoint> edge)
        {
            PointI[] points = PointsListToArray(edge);

            if (points.Length > 1)
            {
                g.DrawLines(pen, points);
            }
            else
            {
                g.DrawLine(pen, points[0], points[0]);
            }
        }
    }
}
