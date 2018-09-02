//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

#if !SILVERLIGHT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Reflection;

using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.ObjectModel;

using Alt.Sketch;
using Alt.Sketch.GMap.NET;
using Alt.Sketch.GMap.NET.Markers;
using Alt.Sketch.GMap.NET.ToolTips;
using Alt.Sketch.Demo.GMap.NET;
using Alt.Sketch.Demo.GMap.NET.CustomMarkers;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;

using Alt.ComponentModel;
using Alt.IO;
using Alt.Threading;
using Alt.Xml;


namespace Alt.GUI.Demo
{
    partial class Example_GMap : Example__Base
    {
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
                return "GMap.NET (Interactive)";
            }
        }


        // layers
        readonly GMapOverlay top = new GMapOverlay();
        internal readonly GMapOverlay objects = new GMapOverlay("objects");
        internal readonly GMapOverlay routes = new GMapOverlay("routes");
        internal readonly GMapOverlay polygons = new GMapOverlay("polygons");

        // marker
        GMarkerGoogle currentMarker;

        // polygons
        GMapPolygon polygon;

        // etc
        GMapMarkerRect CurentRectMarker = null;
        //NoNeed	string mobileGpsLog = string.Empty;
        bool isMouseDown = false;
        PointLatLng start;
        PointLatLng end;


        static Example_GMap()
        {
            GMaps.Instance.UseMemoryCache = false;
            GMaps.Instance.Mode =
//#if !UNITY_WEBPLAYER && !UNITY_5
//                AccessMode.ServerAndCache;
//#else
                AccessMode.ServerAndCache;
//#endif
        }


        public Example_GMap(Base parent) :
            base(parent)
        {
            //  Create first part of UI to prevent layout artefacts
            CreateUI1();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            
            //  Create second part of UI
            CreateUI2();


            MainMap.Bearing = 0F;
            MainMap.CanDragMap = true;
            MainMap.EmptyTileColor = Color.Navy;
            MainMap.GrayScaleMode = false;
            MainMap.LevelsKeepInMemmory = 5;
            MainMap.MarkersEnabled = true;
            MainMap.MaxZoom = 17;
            MainMap.MinZoom = 2;
            MainMap.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            MainMap.Name = "MainMap";
            MainMap.NegativeMode = false;
            MainMap.PolygonsEnabled = true;
            MainMap.RetryLoadTile = 0;
            MainMap.RoutesEnabled = true;
            MainMap.SelectedAreaFillColor = Color.FromArgb(33, 65, 105, 225);
            MainMap.ShowTileGridLines = false;
            MainMap.Zoom = 0D;


            // add your custom map db provider
            //GMap.NET.CacheProviders.MySQLPureImageCache ch = new GMap.NET.CacheProviders.MySQLPureImageCache();
            //ch.ConnectionString = @"server=sql2008;User Id=trolis;Persist Security Info=True;database=gmapnetcache;password=trolis;";
            //MainMap.Manager.SecondaryCache = ch;

            // set your proxy here if need
            //GMapProvider.IsSocksProxy = true;
            //GMapProvider.WebProxy = new WebProxy("127.0.0.1", 1080);
            //GMapProvider.WebProxy.Credentials = new NetworkCredential("ogrenci@bilgeadam.com", "bilgeada");
            // or
            //GMapProvider.WebProxy = WebRequest.DefaultWebProxy;


#if !UNITY_WEBPLAYER
            // set cache mode only if no internet avaible
            if (!Stuff.PingNetwork("pingtest.com", false)
                //  Unity3D
#if UNITY_WEBPLAYER || UNITY_5
                && !Config.IsRunningOnMono
#endif
                )
            {
                MainMap.Manager.Mode = AccessMode.CacheOnly;
				new Alt.GUI.Temporary.Gwen.Control.MessageBox(this, "No internet connection available, going to CacheOnly mode.", "GMap.NET");
            }
#endif

            // config map         
            MainMap.MapProvider = GMapProviders.OpenStreetMap;
            MainMap.Position = new PointLatLng(54.6961334816182, 25.2985095977783);
            MainMap.MinZoom = 0;
            MainMap.MaxZoom = 24;
            MainMap.Zoom = 9;

            //MainMap.ScaleMode = ScaleModes.Fractional;

            // map events
            {
                MainMap.OnPositionChanged += new PositionChanged(MainMap_OnPositionChanged);

                MainMap.OnTileLoadStart += new TileLoadStart(MainMap_OnTileLoadStart);
                MainMap.OnTileLoadComplete += new TileLoadComplete(MainMap_OnTileLoadComplete);

                MainMap.OnMapZoomChanged += new MapZoomChanged(MainMap_OnMapZoomChanged);
                MainMap.OnMapTypeChanged += new MapTypeChanged(MainMap_OnMapTypeChanged);

                MainMap.OnMarkerClick += new MarkerClick(MainMap_OnMarkerClick);
                MainMap.OnMarkerEnter += new MarkerEnter(MainMap_OnMarkerEnter);
                MainMap.OnMarkerLeave += new MarkerLeave(MainMap_OnMarkerLeave);

                MainMap.OnPolygonEnter += new PolygonEnter(MainMap_OnPolygonEnter);
                MainMap.OnPolygonLeave += new PolygonLeave(MainMap_OnPolygonLeave);

                MainMap.OnRouteEnter += new RouteEnter(MainMap_OnRouteEnter);
                MainMap.OnRouteLeave += new RouteLeave(MainMap_OnRouteLeave);

                MainMap.Manager.OnTileCacheComplete += new TileCacheComplete(OnTileCacheComplete);
                MainMap.Manager.OnTileCacheStart += new TileCacheStart(OnTileCacheStart);
                MainMap.Manager.OnTileCacheProgress += new TileCacheProgress(OnTileCacheProgress);
            }

            MainMap.MouseMove += new GUI.MouseEventHandler(MainMap_MouseMove);
            MainMap.MouseDown += new GUI.MouseEventHandler(MainMap_MouseDown);
            MainMap.MouseUp += new GUI.MouseEventHandler(MainMap_MouseUp);
            MainMap.MouseDoubleClick += new GUI.MouseEventHandler(MainMap_MouseDoubleClick);

            
            // get map types
            LoadComboBoxMapTypeValues();

            // acccess mode
            LoadComboBoxAccessModeValues();


            // get position
            textBoxLat.Text = MainMap.Position.Lat.ToString(CultureInfo.InvariantCulture);
            textBoxLng.Text = MainMap.Position.Lng.ToString(CultureInfo.InvariantCulture);

            // get cache modes
            checkBoxUseRouteCache.IsChecked = MainMap.Manager.UseRouteCache;

            // get zoom  
			m_ScaleTrackBar.Minimum = MainMap.MinZoom * 100;
			m_ScaleTrackBar.Maximum = MainMap.MaxZoom * 100;
			//m_ScaleTrackBar.TickFrequency = 100;
			m_ScaleTrackBar.NotchCount = (int)(m_ScaleTrackBar.Maximum - m_ScaleTrackBar.Minimum) / 100;
			m_ScaleTrackBar.SnapToNotches = true;


#if DEBUG
            checkBoxDebug.IsChecked = true;
#endif


            // add custom layers  
            {
                MainMap.Overlays.Add(routes);
                MainMap.Overlays.Add(polygons);
                MainMap.Overlays.Add(objects);
                MainMap.Overlays.Add(top);

                routes.Routes.CollectionChanged += new NotifyCollectionChangedEventHandler(Routes_CollectionChanged);
                objects.Markers.CollectionChanged += new NotifyCollectionChangedEventHandler(Markers_CollectionChanged);
            }

            // set current marker
            currentMarker = new GMarkerGoogle(MainMap.Position, GMarkerGoogleType.arrow);
            currentMarker.IsHitTestVisible = false;
            top.Markers.Add(currentMarker);

            //MainMap.VirtualSizeEnabled = true;
            //if(false)
            {
                // add my city location for demo
                GeoCoderStatusCode status = GeoCoderStatusCode.Unknow;
                {
                    PointLatLng? pos = GMapProviders.GoogleMap.GetPoint("Lithuania, Vilnius", out status);
                    if (pos != null && status == GeoCoderStatusCode.G_GEO_SUCCESS)
                    {
                        currentMarker.Position = pos.Value;

                        GMapMarker myCity = new GMarkerGoogle(pos.Value, GMarkerGoogleType.green_small);
                        myCity.ToolTipMode = MarkerTooltipMode.Always;
                        myCity.ToolTipText = "Welcome to Lithuania! ;}";
                        objects.Markers.Add(myCity);
                    }
                }

                // add some points in lithuania
                AddLocationLithuania("Kaunas");
                AddLocationLithuania("Klaipėda");
                AddLocationLithuania("Šiauliai");
                AddLocationLithuania("Panevėžys");

                if (objects.Markers.Count > 0)
                {
                    MainMap.ZoomAndCenterMarkers(null);
                }

                RegeneratePolygon();
            }



            //  Neet to correct "map" page view
            m_Page_map_GroupBox_coordinates.AutoSizeToContents = false;
            m_Page_map_GroupBox_gmap.AutoSizeToContents = false;
            m_Page_map_GroupBox_routing.AutoSizeToContents = false;
            m_Page_map_GroupBox_markers.AutoSizeToContents = false;

			m_ScaleTrackBar.Value = (int)MainMap.Zoom * 100;

            checkBoxCurrentMarker.IsChecked = true;
            checkBoxCanDrag.IsChecked = true;


            //  Fill some fields
            MainMap_OnPositionChanged(MainMap.Position);
            MainMap_OnMapZoomChanged();
            Markers_CollectionChanged(null, null);
            Routes_CollectionChanged(null, null);
        }
        

        public override void Dispose()
        {
            if (MainMap != null)
            {
                MainMap.Dispose();
                MainMap = null;

                GMaps.Instance.CancelTileCaching();
            }

            base.Dispose();
        }


        void LoadComboBoxMapTypeValues()
        {
            //TEMP  #if !MONO

            // mono doesn't handle it, so we 'lost' provider list ;]
            //comboBoxMapType.ValueMember = "Name";
            //comboBoxMapType.DataSource = GMapProviders.List;
            //comboBoxMapType.SelectedItem = MainMap.MapProvider;

            List<GMapProvider> GMapProviders_List = GMapProviders.List;
            GMapProvider MainMap_MapProvider = MainMap.MapProvider;
            foreach (GMapProvider provider in GMapProviders_List)
            {
				Alt.GUI.Temporary.Gwen.Control.MenuItem item = comboBoxMapType.AddItem(provider.Name, provider.Id.ToString());
                item.Tag = provider;

                if (provider.Id == MainMap_MapProvider.Id)
                {
                    comboBoxMapType.SelectedItem = item;
                }
            }

            comboBoxMapType.ItemSelected += new GwenEventHandler(comboBoxMapType_DropDownClosed);

            //TEMP  #endif
        }
        void comboBoxMapType_Select(GMapProvider provider)
        {
			Alt.GUI.Temporary.Gwen.Control.MenuItem item = comboBoxMapType.SelectedItem;
            if (item != null)
            {
                GMapProvider itemProvider = item.Tag as GMapProvider;
                if (itemProvider == provider)
                {
                    return;
                }
            }

			Alt.GUI.Temporary.Gwen.Control.MenuItem toSelect = null;
            foreach (Base child in comboBoxMapType.Children)
            {
				item = child as Alt.GUI.Temporary.Gwen.Control.MenuItem;
                if (item != null)
                {
                    GMapProvider itemProvider = item.Tag as GMapProvider;
                    if (itemProvider == provider)
                    {
                        toSelect = item;
                        break;
                    }
                }
            }

            if (toSelect != null)
            {
                comboBoxMapType.SelectedItem = toSelect;
            }
        }

        void LoadComboBoxAccessModeValues()
        {
            //comboBoxMode.DataSource = Enum.GetValues(typeof(AccessMode));
            //comboBoxMode.SelectedItem = MainMap.Manager.Mode;

            AccessMode MainMap_Manager_Mode = MainMap.Manager.Mode;
            foreach (AccessMode mode in Enum.GetValues(typeof(AccessMode)))
            {
				Alt.GUI.Temporary.Gwen.Control.MenuItem item = comboBoxMode.AddItem(mode.ToString(), mode.ToString());
                item.Tag = mode;

                if (mode == MainMap_Manager_Mode)
                {
                    comboBoxMode.SelectedItem = item;
                }
            }

            comboBoxMode.ItemSelected += new GwenEventHandler(comboBoxMode_DropDownClosed);
        }


#if USE_Serialization
        public T DeepClone<T>(T obj)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                formatter.Serialize(ms, obj);

                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
#endif


        void Markers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            textBoxMarkerCount.Text = objects.Markers.Count.ToString();
        }


        void Routes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            textBoxrouteCount.Text = routes.Routes.Count.ToString();
        }


        //  -- some functions --

        void RegeneratePolygon()
        {
            List<PointLatLng> polygonPoints = new List<PointLatLng>();

            foreach (GMapMarker m in objects.Markers)
            {
                if (m is GMapMarkerRect)
                {
                    m.Tag = polygonPoints.Count;
                    polygonPoints.Add(m.Position);
                }
            }

            if (polygon == null)
            {
                polygon = new GMapPolygon(polygonPoints, "polygon test");
                polygon.IsHitTestVisible = true;
                polygons.Polygons.Add(polygon);
            }
            else
            {
                polygon.Points.Clear();
                polygon.Points.AddRange(polygonPoints);

                if (polygons.Polygons.Count == 0)
                {
                    polygons.Polygons.Add(polygon);
                }
                else
                {
                    MainMap.UpdatePolygonLocalPosition(polygon);
                }
            }
        }


        /// <summary>
        /// adds marker using geocoder
        /// </summary>
        /// <param name="place"></param>
        void AddLocationLithuania(string place)
        {
            GeoCoderStatusCode status = GeoCoderStatusCode.Unknow;
            PointLatLng? pos = GMapProviders.GoogleMap.GetPoint("Lithuania, " + place, out status);
            if (pos != null && status == GeoCoderStatusCode.G_GEO_SUCCESS)
            {
                GMarkerGoogle m = new GMarkerGoogle(pos.Value, GMarkerGoogleType.green);
                m.ToolTip = new GMapRoundedToolTip(m);

                GMapMarkerRect mBorders = new GMapMarkerRect(pos.Value);
                {
                    mBorders.InnerMarker = m;
                    mBorders.ToolTipText = place;
                    mBorders.ToolTipMode = MarkerTooltipMode.Always;
                }

                objects.Markers.Add(m);
                objects.Markers.Add(mBorders);
            }
        }


        //  -- map events --

        void OnTileCacheComplete()
        {
            Debug.WriteLine("OnTileCacheComplete");
            long size = 0;
            int db = 0;
            try
            {
                DirectoryInfo di = new DirectoryInfo(MainMap.CacheLocation);
                var dbs = di.GetFiles("*.gmdb", SearchOption.AllDirectories);
                foreach (var d in dbs)
                {
                    size += d.Length;
                    db++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            if (!IsDisposed)
            {
                GUI.MethodInvoker m = delegate
                {
                    textBoxCacheSize.Text = string.Format(CultureInfo.InvariantCulture, "{0} db in {1:00} MB", db, size / (1024.0 * 1024.0));
                    textBoxCacheStatus.Text = "all tiles saved!";
                };

                if (!IsDisposed)
                {
                    try
                    {
                        Invoke(m);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }


        void OnTileCacheStart()
        {
            Debug.WriteLine("OnTileCacheStart");

            if (!IsDisposed)
            {
                GUI.MethodInvoker m = delegate
                {
                    textBoxCacheStatus.Text = "saving tiles...";
                };
                Invoke(m);
            }
        }


        void OnTileCacheProgress(int left)
        {
            if (!IsDisposed)
            {
                GUI.MethodInvoker m = delegate
                {
                    textBoxCacheStatus.Text = left + " tile to save...";
                };
                Invoke(m);
            }
        }


        void MainMap_OnMarkerLeave(GMapMarker item)
        {
            if (item is GMapMarkerRect)
            {
                CurentRectMarker = null;

                GMapMarkerRect rc = item as GMapMarkerRect;
                rc.Pen.Color = Color.Blue;

                Debug.WriteLine("OnMarkerLeave: " + item.Position);
            }
        }


        void MainMap_OnMarkerEnter(GMapMarker item)
        {
            if (item is GMapMarkerRect)
            {
                GMapMarkerRect rc = item as GMapMarkerRect;
                rc.Pen.Color = Color.Red;

                CurentRectMarker = rc;

                Debug.WriteLine("OnMarkerEnter: " + item.Position);
            }
        }


        GMapPolygon currentPolygon = null;
        void MainMap_OnPolygonLeave(GMapPolygon item)
        {
            currentPolygon = null;
            item.Stroke.Color = Color.MidnightBlue;
            Debug.WriteLine("OnPolygonLeave: " + item.Name);
        }


        void MainMap_OnPolygonEnter(GMapPolygon item)
        {
            currentPolygon = item;
            item.Stroke.Color = Color.Red;
            Debug.WriteLine("OnPolygonEnter: " + item.Name);
        }


        GMapRoute currentRoute = null;
        void MainMap_OnRouteLeave(GMapRoute item)
        {
            currentRoute = null;
            item.Stroke.Color = Color.MidnightBlue;
            Debug.WriteLine("OnRouteLeave: " + item.Name);
        }


        void MainMap_OnRouteEnter(GMapRoute item)
        {
            currentRoute = item;
            item.Stroke.Color = Color.Red;
            Debug.WriteLine("OnRouteEnter: " + item.Name);
        }


        void MainMap_OnMapTypeChanged(GMapProvider type)
        {
            //comboBoxMapType.SelectedItem = type;
            comboBoxMapType_Select(type);

			m_ScaleTrackBar.Minimum = MainMap.MinZoom * 100;
			m_ScaleTrackBar.Maximum = MainMap.MaxZoom * 100;
        }


        void MainMap_MouseUp(object sender, GUI.MouseEventArgs e)
        {
            if (e.Button == GUI.MouseButtons.Left)
            {
                isMouseDown = false;
            }
        }


        // add demo circle
        void MainMap_MouseDoubleClick(object sender, GUI.MouseEventArgs e)
        {
            var cc = new GMapMarkerCircle(MainMap.FromLocalToLatLng(e.X, e.Y));
            objects.Markers.Add(cc);
        }


        void MainMap_MouseDown(object sender, GUI.MouseEventArgs e)
        {
            if (e.Button == GUI.MouseButtons.Left)
            {
                isMouseDown = true;

                if (currentMarker != null &&
                    currentMarker.IsVisible)
                {
                    currentMarker.Position = MainMap.FromLocalToLatLng(e.X, e.Y);

                    var px = MainMap.MapProvider.Projection.FromLatLngToPixel(currentMarker.Position.Lat, currentMarker.Position.Lng, (int)MainMap.Zoom);
                    var tile = MainMap.MapProvider.Projection.FromPixelToTileXY(px);

                    Debug.WriteLine("MouseDown: geo: " + currentMarker.Position + " | px: " + px + " | tile: " + tile);
                }
            }
        }


        // move current marker with left holding
        void MainMap_MouseMove(object sender, GUI.MouseEventArgs e)
        {
            if (e.Button == GUI.MouseButtons.Left && isMouseDown)
            {
                if (CurentRectMarker == null)
                {
                    if (currentMarker != null &&
                        currentMarker.IsVisible)
                    {
                        currentMarker.Position = MainMap.FromLocalToLatLng(e.X, e.Y);
                    }
                }
                else // move rect marker
                {
                    PointLatLng pnew = MainMap.FromLocalToLatLng(e.X, e.Y);

                    int? pIndex = (int?)CurentRectMarker.Tag;
                    if (pIndex.HasValue)
                    {
                        if (pIndex < polygon.Points.Count)
                        {
                            polygon.Points[pIndex.Value] = pnew;
                            MainMap.UpdatePolygonLocalPosition(polygon);
                        }
                    }

                    if (currentMarker.IsVisible)
                    {
                        currentMarker.Position = pnew;
                    }
                    CurentRectMarker.Position = pnew;

                    if (CurentRectMarker.InnerMarker != null)
                    {
                        CurentRectMarker.InnerMarker.Position = pnew;
                    }
                }

                MainMap.Refresh(); // force instant invalidation
            }
        }


        // MapZoomChanged
        void MainMap_OnMapZoomChanged()
        {
			m_ScaleTrackBar.Value = (int)(MainMap.Zoom * 100.0);
            textBoxZoomCurrent.Text = MainMap.Zoom.ToString();
        }


        // click on some marker
        void MainMap_OnMarkerClick(GMapMarker item, Alt.GUI.MouseEventArgs e)
        {
            if (e.Button == Alt.GUI.MouseButtons.Left)
            {
                if (item is GMapMarkerRect)
                {
                    GeoCoderStatusCode status;
                    var pos = GMapProviders.GoogleMap.GetPlacemark(item.Position, out status);
                    if (status == GeoCoderStatusCode.G_GEO_SUCCESS && pos != null)
                    {
                        GMapMarkerRect v = item as GMapMarkerRect;
                        {
                            v.ToolTipText = pos.Value.Address;
                        }
                        MainMap.Invalidate();//TEMP   false);
                    }
                }
            }
        }


        // loader start loading tiles
        void MainMap_OnTileLoadStart()
        {
            GUI.MethodInvoker m = delegate()
            {
                //TEMP  panelMenu.Text = "Menu: loading tiles...";
            };

            try
            {
                BeginInvoke(m);
            }
            catch
            {
            }
        }


        // loader end loading tiles
        void MainMap_OnTileLoadComplete(long ElapsedMilliseconds)
        {
#if DEBUG && !UNITY_5
            MainMap.ElapsedMilliseconds = ElapsedMilliseconds;
#endif

            GUI.MethodInvoker m = delegate()
            {
                //TEMP  panelMenu.Text = "Menu, last load in " + MainMap.ElapsedMilliseconds + "ms";

                textBoxMemory.Text = string.Format(CultureInfo.InvariantCulture, "{0:0.00} MB of {1:0.00} MB", MainMap.Manager.MemoryCache.Size, MainMap.Manager.MemoryCache.Capacity);

#if UNITY_WEBPLAYER || UNITY_5
				MainMap.Refresh();
#endif
            };

            try
            {
                BeginInvoke(m);
            }
            catch
            {
            }
        }


        // current point changed
        void MainMap_OnPositionChanged(PointLatLng point)
        {
            textBoxLatCurrent.Text = point.Lat.ToString(CultureInfo.InvariantCulture);
            textBoxLngCurrent.Text = point.Lng.ToString(CultureInfo.InvariantCulture);
        }


        //  -- ui events --

        // change map type
        void comboBoxMapType_DropDownClosed(Base control)
        {
			MainMap.MapProvider = comboBoxMapType.SelectedItem.Tag as GMapProvider;
            
			//MainMap.MapProvider = GMapProviders.OpenStreetMap;                
            //comboBoxMapType_Select(MainMap.MapProvider);
        }


        // change mode
        void comboBoxMode_DropDownClosed(Base control)
        {
            MainMap.Manager.Mode = (AccessMode)comboBoxMode.SelectedItem.Tag;
            MainMap.ReloadMap();
        }


        // zoom
        void trackBar1_ValueChanged(Base control)
        {
			MainMap.Zoom = m_ScaleTrackBar.Value / 100.0;
        }


        // go to
        void button8_Click(object sender, EventArgs e)
        {
            try
            {
                double lat = double.Parse(textBoxLat.Text, CultureInfo.InvariantCulture);
                double lng = double.Parse(textBoxLng.Text, CultureInfo.InvariantCulture);

                MainMap.Position = new PointLatLng(lat, lng);
            }
            catch (Exception ex)
            {
				new Alt.GUI.Temporary.Gwen.Control.MessageBox(this, "incorrect coordinate format: " + ex.Message, "Error");
            }
        }


        // goto by geocoder
        void textBoxGeo_KeyPress(object sender, GUI.KeyPressEventArgs e)
        {
            if ((GUI.Keys)e.KeyChar == GUI.Keys.Enter)
            {
                GeoCoderStatusCode status = MainMap.SetPositionByKeywords(textBoxGeo.Text);
                if (status != GeoCoderStatusCode.G_GEO_SUCCESS)
                {
					new Alt.GUI.Temporary.Gwen.Control.MessageBox(this, "Geocoder can't find: '" + textBoxGeo.Text + "', reason: " + status.ToString(), "GMap.NET");
                }
            }
        }


        // reload map
        void button1_Click(object sender, EventArgs e)
        {
            MainMap.ReloadMap();
        }


        // cache config
        void checkBoxUseCache_CheckedChanged(object sender, EventArgs e)
        {
            MainMap.Manager.UseRouteCache = checkBoxUseRouteCache.IsChecked;
            MainMap.Manager.UseGeocoderCache = checkBoxUseRouteCache.IsChecked;
            MainMap.Manager.UsePlacemarkCache = checkBoxUseRouteCache.IsChecked;
            MainMap.Manager.UseDirectionsCache = checkBoxUseRouteCache.IsChecked;
        }


        // clear cache
        void button2_Click(object sender, EventArgs e)
        {
            //TEMP  if (MessageBox.Show("Are You sure?", "Clear GMap.NET cache?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                try
                {
                    if (MainMap.Manager != null &&
                        MainMap.Manager.PrimaryCache != null)
                    {
                        MainMap.Manager.PrimaryCache.DeleteOlderThan(DateTime.Now, null);
                        new Alt.GUI.Temporary.Gwen.Control.MessageBox(this, "Done. Cache is clear.", "GMap.NET");
                    }
                }
                catch (Exception ex)
                {
					new Alt.GUI.Temporary.Gwen.Control.MessageBox(this, ex.Message, "Error");
                }
            }
        }


        // add test route
        void button3_Click(object sender, EventArgs e)
        {
            RoutingProvider rp = MainMap.MapProvider as RoutingProvider;
            if (rp == null)
            {
                rp = GMapProviders.OpenStreetMap; // use OpenStreetMap if provider does not implement routing
            }
            //地图路径
            MapRoute route = rp.GetRoute(start, end, false, false, (int)MainMap.Zoom);
            if (route != null)
            {
                // add route
                GMapRoute r = new GMapRoute(route.Points, route.Name);
                r.IsHitTestVisible = true;
                routes.Routes.Add(r);
                ////////////////////////========================================================
                // add route start/end marks
                GMapMarker m1 = new GMarkerGoogle(start, GMarkerGoogleType.green_big_go);
                m1.ToolTipText = "Start: " + route.Name;
                m1.ToolTipMode = MarkerTooltipMode.Always;

                GMapMarker m2 = new GMarkerGoogle(end, GMarkerGoogleType.red_big_stop);
                m2.ToolTipText = "End: " + end.ToString();
                m2.ToolTipMode = MarkerTooltipMode.Always;

                objects.Markers.Add(m1);
                objects.Markers.Add(m2);

                MainMap.ZoomAndCenterRoute(r);
            }
        }


        // add marker on current position
        void button4_Click(object sender, EventArgs e)
        {
            GMarkerGoogle m = new GMarkerGoogle(currentMarker.Position, GMarkerGoogleType.green_pushpin);
            GMapMarkerRect mBorders = new GMapMarkerRect(currentMarker.Position);
            {
                mBorders.InnerMarker = m;
                if (polygon != null)
                {
                    mBorders.Tag = polygon.Points.Count;
                }
                mBorders.ToolTipMode = MarkerTooltipMode.Always;
            }

            Placemark? p = null;
            if (checkBoxPlacemarkInfo.IsChecked)
            {
                GeoCoderStatusCode status;
                var ret = GMapProviders.GoogleMap.GetPlacemark(currentMarker.Position, out status);
                if (status == GeoCoderStatusCode.G_GEO_SUCCESS && ret != null)
                {
                    p = ret;
                }
            }

            if (p != null)
            {
                mBorders.ToolTipText = p.Value.Address;
            }
            else
            {
                mBorders.ToolTipText = currentMarker.Position.ToString();
            }

            objects.Markers.Add(m);
            objects.Markers.Add(mBorders);

            RegeneratePolygon();
        }


        // clear routes
        void button6_Click(object sender, EventArgs e)
        {
            routes.Routes.Clear();
        }


        // clear polygons
        void button15_Click(object sender, EventArgs e)
        {
            polygons.Polygons.Clear();
        }


        // clear markers
        void button5_Click(object sender, EventArgs e)
        {
            objects.Markers.Clear();
        }


        // show current marker
        void checkBoxCurrentMarker_CheckedChanged(object sender, EventArgs e)
        {
            currentMarker.IsVisible = checkBoxCurrentMarker.IsChecked;
        }


        // can drag
        void checkBoxCanDrag_CheckedChanged(object sender, EventArgs e)
        {
            MainMap.CanDragMap = checkBoxCanDrag.IsChecked;
        }


        // set route start
        void buttonSetStart_Click(object sender, EventArgs e)
        {
            start = currentMarker.Position;
        }


        // set route end
        void buttonSetEnd_Click(object sender, EventArgs e)
        {
            end = currentMarker.Position;
        }


        // zoom to max for markers
        void button7_Click(object sender, EventArgs e)
        {
            MainMap.ZoomAndCenterMarkers("objects");
        }


        // debug tile grid
        void checkBoxDebug_CheckedChanged(object sender, EventArgs e)
        {
            MainMap.ShowTileGridLines = checkBoxDebug.IsChecked;
        }


        // key-up events
        void MainForm_KeyUp(object sender, GUI.KeyEventArgs e)
        {
            int offset = -22;

            if (e.KeyCode == GUI.Keys.Left)
            {
                MainMap.Offset(-offset, 0);
            }
            else if (e.KeyCode == GUI.Keys.Right)
            {
                MainMap.Offset(offset, 0);
            }
            else if (e.KeyCode == GUI.Keys.Up)
            {
                MainMap.Offset(0, -offset);
            }
            else if (e.KeyCode == GUI.Keys.Down)
            {
                MainMap.Offset(0, offset);
            }
            else if (e.KeyCode == GUI.Keys.Delete)
            {
                if (currentPolygon != null)
                {
                    polygons.Polygons.Remove(currentPolygon);
                    currentPolygon = null;
                }

                if (currentRoute != null)
                {
                    routes.Routes.Remove(currentRoute);
                    currentRoute = null;
                }

                if (CurentRectMarker != null)
                {
                    objects.Markers.Remove(CurentRectMarker);

                    if (CurentRectMarker.InnerMarker != null)
                    {
                        objects.Markers.Remove(CurentRectMarker.InnerMarker);
                    }
                    CurentRectMarker = null;

                    RegeneratePolygon();
                }
            }
            else if (e.KeyCode == GUI.Keys.Escape)
            {
                MainMap.Bearing = 0;
            }
        }


        // key-press events
        void MainForm_KeyPress(object sender, GUI.KeyPressEventArgs e)
        {
            if (MainMap.Focused)
            {
                if (e.KeyChar == '+')
                {
                    MainMap.Zoom = ((int)MainMap.Zoom) + 1;
                }
                else if (e.KeyChar == '-')
                {
                    MainMap.Zoom = ((int)(MainMap.Zoom + 0.99)) - 1;
                }
                else if (e.KeyChar == 'a')
                {
                    MainMap.Bearing--;
                }
                else if (e.KeyChar == 'z')
                {
                    MainMap.Bearing++;
                }
            }
        }


        void buttonZoomUp_Click(object sender, EventArgs e)
        {
            //MainMap.Zoom = ((int)MainMap.Zoom) + 1;
			double p = m_ScaleTrackBar.Maximum - m_ScaleTrackBar.Minimum;
			double dp = p / m_ScaleTrackBar.NotchCount;
			m_ScaleTrackBar.Value += (float)(dp * 1.01);
        }


        void buttonZoomDown_Click(object sender, EventArgs e)
        {
            //MainMap.Zoom = ((int)(MainMap.Zoom + 0.99)) - 1;
			double p = m_ScaleTrackBar.Maximum - m_ScaleTrackBar.Minimum;
			double dp = p / m_ScaleTrackBar.NotchCount;
			m_ScaleTrackBar.Value -= (float)(dp * 0.99);
        }
    }
}
#endif
