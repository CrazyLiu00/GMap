//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Alt.IO;
using Alt.Sketch;
using Alt.Sketch.GMap.NET;


namespace Alt.GUI
{
	static partial class AltGUIHelper
	{
		internal static Alt.GUI.Temporary.Gwen.Control.Awesomium Create_AwesomiumControl(Alt.GUI.Temporary.Gwen.Control.Base parent)
		{
			Alt.GUI.Temporary.Gwen.Control.Awesomium awesomiumControl = new Alt.GUI.Temporary.Gwen.Control.Awesomium(parent);

			return awesomiumControl;
		}


        internal static Alt.Sketch.Demo.GMap.NET.Map Create_GMap(Alt.GUI.Temporary.Gwen.Control.Base parent)
        {
            GMap.NET.GMaps.Instance.UseMemoryCache = true;
            GMap.NET.GMaps.Instance.Mode =
                //#if !UNITY_WEBPLAYER && !UNITY_5
                //                AccessMode.ServerAndCache;
                //#else
                GMap.NET.AccessMode.CacheOnly;
            //#endif

            Alt.Sketch.Demo.GMap.NET.Map map = new Alt.Sketch.Demo.GMap.NET.Map(parent);

            MapInfo.Instance.SetMap(map);
            map.DrawBorder = true;


            Alt.GUI.Temporary.Gwen.Control.Base m_ScaleControl;
            Alt.GUI.Temporary.Gwen.Control.VerticalSlider m_ScaleTrackBar;


            //  Scale
            {
                m_ScaleControl = new Alt.GUI.Temporary.Gwen.Control.Base(map);
                m_ScaleControl.SetBounds(10, 10, 20, 200);

                Alt.GUI.Temporary.Gwen.Control.Button plus = new Alt.GUI.Temporary.Gwen.Control.Button(m_ScaleControl);
                plus.Text = "+";
                plus.Dock = Alt.GUI.Temporary.Gwen.Pos.Top;

                Alt.GUI.Temporary.Gwen.Control.Button minus = new Alt.GUI.Temporary.Gwen.Control.Button(m_ScaleControl);
                minus.Text = "-";
                minus.Dock = Alt.GUI.Temporary.Gwen.Pos.Bottom;

                m_ScaleTrackBar = new Alt.GUI.Temporary.Gwen.Control.VerticalSlider(m_ScaleControl);
                m_ScaleTrackBar.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;

                plus.Click += //new EventHandler(buttonZoomUp_Click);
                    delegate (object sender, EventArgs e)
                {
                    double p = m_ScaleTrackBar.Maximum - m_ScaleTrackBar.Minimum;
                    double dp = p / m_ScaleTrackBar.NotchCount;
                    m_ScaleTrackBar.Value += (float)(dp * 1.01);
                };

                minus.Click += //new EventHandler(buttonZoomDown_Click);
                    delegate (object sender, EventArgs e)
                {
                    double p = m_ScaleTrackBar.Maximum - m_ScaleTrackBar.Minimum;
                    double dp = p / m_ScaleTrackBar.NotchCount;
                    m_ScaleTrackBar.Value -= (float)(dp * 0.99);
                };

                m_ScaleTrackBar.ValueChanged += //new Alt.GUI.Temporary.Gwen.Control.Base.GwenEventHandler(scaleTrackBar_ValueChanged);
                    delegate (Alt.GUI.Temporary.Gwen.Control.Base control)
                {
                    map.Zoom = m_ScaleTrackBar.Value / 100.0;
                };
            }


#if !UNITY_WEBPLAYER
            if (!Alt.Sketch.Demo.GMap.NET.Stuff.PingNetwork("pingtest.com", false)
#if UNITY_WEBPLAYER || UNITY_5
                && !Alt.GUI.Config.IsRunningOnMono
#endif
                )
            {
                map.Manager.Mode = GMap.NET.AccessMode.CacheOnly;
                new Alt.GUI.Temporary.Gwen.Control.MessageBox(map, "No internet connection available, going to CacheOnly mode.", "GMap.NET");
            }
#endif

            // config map         
            map.MapProvider = GMap.NET.MapProviders.GMapProviders.BingSatelliteMap;
            map.MinZoom = 0;
            map.MaxZoom = 24;
            map.Zoom = 5;

            GMap.NET.GMaps.Instance.PrimaryCache = new SqliteLoadImage();
            map.Position = new GMap.NET.PointLatLng(50, 50);

            // get zoom  
            m_ScaleTrackBar.Minimum = map.MinZoom * 100;
            m_ScaleTrackBar.Maximum = map.MaxZoom * 100;
            //m_ScaleTrackBar.TickFrequency = 100;
            m_ScaleTrackBar.NotchCount = (int)(m_ScaleTrackBar.Maximum - m_ScaleTrackBar.Minimum) / 100;
            m_ScaleTrackBar.SnapToNotches = true;

            m_ScaleTrackBar.Value = (int)map.Zoom * 100;

            // map events
            map.OnMapZoomChanged += //new GMap.NET.MapZoomChanged(MainMap_OnMapZoomChanged);
                delegate ()
            {
                m_ScaleTrackBar.Value = (int)(map.Zoom * 100.0);
            };
            map.OnMapTypeChanged += //new GMap.NET.MapTypeChanged(MainMap_OnMapTypeChanged);
                delegate (GMap.NET.MapProviders.GMapProvider type)
            {
                m_ScaleTrackBar.Minimum = map.MinZoom * 100;
                m_ScaleTrackBar.Maximum = map.MaxZoom * 100;
            };
            map.OnTileLoadComplete += delegate (long ElapsedMilliseconds) {
                MapInfo.Instance.LoadComplete();
                //MapInfo.Instance.RegisterMapEvent();
            };
            
            //GMapOverlay overlay = new GMapOverlay("icon");
            //Bitmap bitmap = Bitmap.FromFile("E:\\Work_Project\\TwoMap\\Formal_Map\\ErSituation\\Assets\\Resources\\Texture\\02Button\\01BtnHeight.png") as Bitmap;
            //Bitmap bg_map = Bitmap.FromFile("E:\\Work_Project\\TwoMap\\Formal_Map\\ErSituation\\Assets\\Resources\\Texture\\02Button\\BtnEntityPress.png");
            //Marker_Test marker = new Marker_Test(new GMap.NET.PointLatLng(40, 120), bitmap);
            //marker.name = "first";
            //marker.ToolTip = new MapStringTest(marker, bg_map);
            //marker.ToolTipMode = MarkerTooltipMode.Always;

            ////marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
            //marker.ToolTipText += "hello world\n";
            //marker.ToolTipText += "hello world\n";
            //marker.ToolTipText += "hello world\n";
            //marker.ToolTipText += "hello world\n";
            //map.Overlays.Add(overlay);
            //overlay.Markers.Add(marker);

            return map;
		}
		
		
		internal static Alt.GUI.PieChart.Temporary.Gwen.PieChartControl Create_PieChartControl(Alt.GUI.Temporary.Gwen.Control.Base parent)
		{
			Alt.GUI.PieChart.Temporary.Gwen.PieChartControl pieChartControl = new Alt.GUI.PieChart.Temporary.Gwen.PieChartControl(parent);
			return pieChartControl;
		}
		
		
		internal static Alt.GUI.PictureBox Create_PictureBox()
		{
			return new Alt.GUI.PictureBox();
		}
		
		
		internal static Alt.GUI.QuickFont.QuickFontControl Create_QuickFontControl()
		{
			Alt.GUI.QuickFont.QuickFontControl quickFont = new Alt.GUI.QuickFont.QuickFontControl();

			quickFont.BackColor = Alt.Sketch.Color.FromArgb(128, Alt.Sketch.Color.DodgerBlue);

			return quickFont;
		}

		
		internal static Alt.GUI.ZedGraph.Temporary.Gwen.ZedGraphControl Create_ZedGraphControl(Alt.GUI.Temporary.Gwen.Control.Base parent)
		{
			return new Alt.GUI.ZedGraph.Temporary.Gwen.ZedGraphControl(parent);
		}
	}
}
