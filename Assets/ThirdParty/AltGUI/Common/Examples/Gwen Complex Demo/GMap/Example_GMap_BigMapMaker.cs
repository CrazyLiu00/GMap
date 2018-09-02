//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

#if !SILVERLIGHT
using System;
using System.Collections.Generic;
using System.Diagnostics;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.Temporary.Gwen.Control.Layout;
using Alt.Sketch;
using Alt.Threading;

using GMap.NET;
using GMap.NET.Projections;
using GMap.NET.MapProviders;

using Alt.Sketch.GMap.NET;


namespace Alt.GUI.Demo
{
    class Example_GMap_BigMapMaker : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 15;
                int y = 0;

                if (m_TopLabel != null)
                {
                    y = m_TopLabel.Height + m_TopLabel.Margin.Height;
                }

                return new SizeI(x, y);
            }
        }

        
        public string Category
        {
            get
            {
                return "GIS";
            }
        }

        public string Caption
        {
            get
            {
                return "GMap.NET (Big Map Maker)";
            }
        }


		Alt.GUI.Temporary.Gwen.Control.HorizontalSplitter m_Splitter;
        Bitmap m_Image;
        object m_ImageLock = new object();
		Alt.GUI.Temporary.Gwen.Control.Base m_ImagePanel;
		Alt.GUI.Temporary.Gwen.Control.Label m_TopLabel;
        const string label_TOP_text = "GMap.NET Big Map Maker Example";

		Alt.GUI.Temporary.Gwen.Control.ListBox m_Log;
        List<string> m_LogCache = new List<string>();

        Thread m_BGThread;


        public Example_GMap_BigMapMaker(Base parent) :
            base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


			m_Splitter = new Alt.GUI.Temporary.Gwen.Control.HorizontalSplitter(this);
            m_Splitter.Dock = Pos.Fill;


			Alt.GUI.Temporary.Gwen.Control.Base imageMainPanel = new Alt.GUI.Temporary.Gwen.Control.Base(m_Splitter);
            imageMainPanel.Dock = Pos.Fill;


			m_TopLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);//imageMainPanel);
            m_TopLabel.AutoSizeToContents = true;
            m_TopLabel.Text = label_TOP_text + " (please, wait while map image creating)";
            m_TopLabel.TextColor = Color.Yellow;
            m_TopLabel.Dock = Pos.Top;
            m_TopLabel.Margin = new Margin(0, 3, 0, 7);


			Alt.GUI.Temporary.Gwen.Control.ScrollControl scrollControl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(imageMainPanel);
            scrollControl.Margin = Margin.One;
            scrollControl.Dock = Pos.Fill;
            scrollControl.EnableScroll(true, true);
            scrollControl.AutoHideBars = true;


			Alt.GUI.Temporary.Gwen.Control.Base logMainPanel = new Alt.GUI.Temporary.Gwen.Control.Base(m_Splitter);

			Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(logMainPanel);
            label.AutoSizeToContents = true;
            label.Text = "Processing Log:";
            label.TextColor = Color.Yellow;
            label.Dock = Pos.Top;
            label.Margin = new Margin(0, 0, 0, 5);

			m_Log = new Alt.GUI.Temporary.Gwen.Control.ListBox(logMainPanel);
            m_Log.Dock = Pos.Fill;

            m_Splitter.SetPanel(0, imageMainPanel);
            m_Splitter.SetPanel(1, logMainPanel);
            m_Splitter.SetVValue(0.75f);


            m_ImagePanel = new Base(scrollControl);
            m_ImagePanel.Margin = Margin.Two;
            m_ImagePanel.Location = PointI.Zero;
            m_ImagePanel.Size = new SizeI(100, 100);
            m_ImagePanel.Paint += new GUI.PaintEventHandler(ImagePanel_Paint);


            m_BGThread = new Thread(new ThreadStart(this.CreateBitmap));
            m_BGThread.SetApartmentState(ApartmentState.STA);
            m_BGThread.Start();
        }


        public override void Dispose()
        {
            GMaps.Instance.CancelTileCaching();

            if (m_BGThread != null &&
                m_BGThread.IsAlive)
            {
                m_BGThread.Abort();
            }

            base.Dispose();
        }


        void CreateBitmap()
        {
            GMaps.Instance.UseMemoryCache = true;
            GMaps.Instance.Mode =
                //#if !UNITY_WEBPLAYER && !UNITY_5
                //  AccessMode.ServerAndCache;
                //#else
                AccessMode.ServerAndCache;
                //#endif

            GMapProvider.TileImageProxy = AltSketchImageProxy.Instance;

            GMapProvider provider = GMapProviders.BingMap;
            provider.OnInitialized();

            int zoom = 12;

            RectLatLng area = RectLatLng.FromLTRB(25.013809204101563, 54.832138557519563, 25.506134033203125, 54.615623046071839);
            if (!area.IsEmpty)
            {
                try
                {
                    List<GPoint> tileArea = provider.Projection.GetAreaTileList(area, zoom, 0);
                    string bigImage = zoom + "-" + provider + "-vilnius";//.png";

                    Console_WriteLine("Preparing: " + bigImage);
                    Console_WriteLine("Zoom: " + zoom);
                    Console_WriteLine("Type: " + provider.ToString());
                    Console_WriteLine("Area: " + area);

                    // current area
                    GPoint topLeftPx = provider.Projection.FromLatLngToPixel(area.LocationTopLeft, zoom);
                    GPoint rightButtomPx = provider.Projection.FromLatLngToPixel(area.Bottom, area.Right, zoom);
                    GPoint pxDelta = new GPoint(rightButtomPx.X - topLeftPx.X, rightButtomPx.Y - topLeftPx.Y);

                    int padding = 22;
                    {
                        //using (Bitmap bmpDestination = new Bitmap((int)(pxDelta.X + padding * 2), (int)(pxDelta.Y + padding * 2)))
                        lock (m_ImageLock)
                        {
                            m_Image = new Bitmap((int)(pxDelta.X + padding * 2), (int)(pxDelta.Y + padding * 2));
                        }
                        {
                            //using (Graphics gfx = Graphics.FromImage(bmpDestination))
                            {
                                //gfx.CompositingMode = CompositingMode.SourceOver;

                                // get tiles & combine into one
                                foreach (var p in tileArea)
                                {
                                    Console_WriteLine("Downloading [" + p + "]: " + (tileArea.IndexOf(p) + 1) + " of " + tileArea.Count);

                                    foreach (var tp in provider.Overlays)
                                    {
                                        Exception ex;
                                        AltSketchImage tile = GMaps.Instance.GetImageFrom(tp, p, zoom, out ex) as AltSketchImage;
                                        if (tile != null)
                                        {
                                            using (tile)
                                            {
                                                long x = p.X * provider.Projection.TileSize.Width - topLeftPx.X + padding;
                                                long y = p.Y * provider.Projection.TileSize.Width - topLeftPx.Y + padding;
                                                {
                                                    lock (m_ImageLock)
                                                    {
                                                        using (Graphics gfx = Graphics.FromImage(m_Image))
                                                        {
                                                            gfx.CompositingMode = CompositingMode.SourceOver;

                                                            gfx.DrawImage(tile.Img, x, y, provider.Projection.TileSize.Width, provider.Projection.TileSize.Height);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            // draw info
                            {
                                RectI rect = new RectI();
                                {
                                    rect.Location = new PointI(padding, padding);
                                    rect.Size = new SizeI((int)pxDelta.X, (int)pxDelta.Y);
                                }
                                using (Font f = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold))
                                {
                                    //using (Graphics gfx = Graphics.FromImage(bmpDestination))
                                    lock (m_ImageLock)
                                    {
                                        using (Graphics gfx = Graphics.FromImage(m_Image))
                                        {
                                            // draw bounds & coordinates
                                            using (Pen p = new Pen(Brushes.Red, 3))
                                            {
                                                p.DashStyle = DashStyle.DashDot;

                                                gfx.DrawRectangle(p, rect);

                                                string topleft = area.LocationTopLeft.ToString();
                                                Size s = gfx.MeasureString(topleft, f);

                                                gfx.DrawString(topleft, f, p.Brush, rect.X + s.Height / 2, rect.Y + s.Height / 2);

                                                string rightBottom = new PointLatLng(area.Bottom, area.Right).ToString();
                                                Size s2 = gfx.MeasureString(rightBottom, f);

                                                gfx.DrawString(rightBottom, f, p.Brush, rect.Right - s2.Width - s2.Height / 2, rect.Bottom - s2.Height - s2.Height / 2);
                                            }

                                            // draw scale
                                            using (Pen p = new Pen(Brushes.Blue, 1))
                                            {
                                                double rez = provider.Projection.GetGroundResolution(zoom, area.Bottom);
                                                int px100 = (int)(100.0 / rez); // 100 meters
                                                int px1000 = (int)(1000.0 / rez); // 1km   

                                                gfx.DrawRectangle(p, rect.X + 10, rect.Bottom - 20, px1000, 10);
                                                gfx.DrawRectangle(p, rect.X + 10, rect.Bottom - 20, px100, 10);

                                                string leftBottom = "scale: 100m | 1Km";
                                                Size s = gfx.MeasureString(leftBottom, f);
                                                gfx.DrawString(leftBottom, f, p.Brush, rect.X + 10, rect.Bottom - s.Height - 20);
                                            }
                                        }
                                    }
                                }
                            }

                            //  bmpDestination.Save(bigImage, ImageFormat.Png);
                            lock (m_ImageLock)
                            {
                                //m_Image = bmpDestination;
                            }
                        }
                    }

                    // ok, lets see what we get
                    {
                        Console_WriteLine("Done! Starting Image: " + bigImage);

                        //TEMP  Process.Start(bigImage);
                    }
                }
                catch (System.Threading.ThreadAbortException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    Console_WriteLine("Error: " + ex.ToString());
                }
            }
        }


        void Console_WriteLine(string text)
        {
            lock (m_LogCache)
            {
                m_LogCache.Add(text);
            }
        }


        void ImagePanel_Paint(object sender, GUI.PaintEventArgs e)
        {
            if (m_BGThread != null)
            {
                if (m_BGThread.IsAlive)
                {
                    lock (m_LogCache)
                    {
                        if (m_LogCache.Count > 0)
                        {
                            foreach (string text in m_LogCache)
                            {
                                TableRow row = m_Log.AddRow(text);
                                row.SetTextColor(Color.DarkBlue);
                            }

                            m_LogCache.Clear();

                            m_Log.ScrollToBottom();
                        }
                    }
                }
                else
                {
                    m_BGThread = null;
                    m_TopLabel.Text = label_TOP_text;
                }
            }


            Bitmap image;
            lock (m_ImageLock)
            {
                if (m_Image == null)
                {
                    return;
                }

                image = m_Image;

                if (image == null)
                {
                    return;
                }


                m_ImagePanel.Size = image.PixelSize;

                e.Graphics.DrawImage(image, PointI.Zero);
            }
        }
    }
}
#endif
