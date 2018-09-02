//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

#if !SILVERLIGHT
using System;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;
using Alt.Sketch.Demo.GMap.NET;


namespace Alt.GUI.Demo
{
    partial class Example_GMap
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 0;
                int y = 0;
                
                if (m_TabControl != null)
                {
                    x = m_TabControl.Width + 5;
                }

                if (MainMap != null)
                {
                    y = MainMap.Y;
                }

                return new SizeI(x, y);
            }
        }


		Alt.GUI.Temporary.Gwen.Control.TabControl m_TabControl;
        internal Map MainMap;


		Alt.GUI.Temporary.Gwen.Control.VerticalSplitter m_MainSplitter;


        //  map
		Alt.GUI.Temporary.Gwen.Control.Base m_Page_map_MainPanel;
		Alt.GUI.Temporary.Gwen.Control.ScrollControl m_Page_map_ScrollControl;
		Alt.GUI.Temporary.Gwen.Control.GroupBox m_Page_map_GroupBox_coordinates;
		Alt.GUI.Temporary.Gwen.Control.GroupBox m_Page_map_GroupBox_gmap;
		Alt.GUI.Temporary.Gwen.Control.GroupBox m_Page_map_GroupBox_routing;
		Alt.GUI.Temporary.Gwen.Control.GroupBox m_Page_map_GroupBox_markers;

        //  coordinates
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxLat;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxLng;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxGeo;
		Alt.GUI.Temporary.Gwen.Control.Button button1;
		Alt.GUI.Temporary.Gwen.Control.Button button8;

        //  gmap
		Alt.GUI.Temporary.Gwen.Control.ComboBox comboBoxMapType;
		Alt.GUI.Temporary.Gwen.Control.ComboBox comboBoxMode;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox checkBoxCurrentMarker;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox checkBoxDebug;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox checkBoxCanDrag;

        //  routing
		Alt.GUI.Temporary.Gwen.Control.Button buttonSetStart;
		Alt.GUI.Temporary.Gwen.Control.Button buttonSetEnd;
		Alt.GUI.Temporary.Gwen.Control.Button button3;
		Alt.GUI.Temporary.Gwen.Control.Button button6;
		Alt.GUI.Temporary.Gwen.Control.Button button15;

        //  markers
		Alt.GUI.Temporary.Gwen.Control.Button button4;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox checkBoxPlacemarkInfo;
		Alt.GUI.Temporary.Gwen.Control.Button button7;
		Alt.GUI.Temporary.Gwen.Control.Button button5;


        //  cache
		Alt.GUI.Temporary.Gwen.Control.Button button2;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxMemory;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxCacheSize;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxCacheStatus;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox checkBoxUseRouteCache;


        //  info
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxLatCurrent;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxLngCurrent;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxZoomCurrent;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxMarkerCount;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxrouteCount;


        //  Scale
		Alt.GUI.Temporary.Gwen.Control.VerticalSlider m_ScaleTrackBar;


        readonly Color LabelColor = Color.Gray * 0.5;


        void CreateUI1()
        {
			m_MainSplitter = new Alt.GUI.Temporary.Gwen.Control.VerticalSplitter(this);
            m_MainSplitter.Dock = Pos.Fill;


			m_TabControl = new Alt.GUI.Temporary.Gwen.Control.TabControl(m_MainSplitter);
            m_TabControl.SetBounds(0, 0, 220, 200);
            m_TabControl.Dock = Pos.Right;


            m_MainSplitter.SetPanel(1, m_TabControl);
            m_MainSplitter.SetHValue(0.73f);


            //  map Tab
			Alt.GUI.Temporary.Gwen.Control.TabButton button = m_TabControl.AddPage("map");
            Base page = button.Page;
            {
                m_Page_map_ScrollControl = new ScrollControl(page);
                m_Page_map_ScrollControl.Dock = Pos.Fill;
                m_Page_map_ScrollControl.EnableScroll(false, true);
                m_Page_map_ScrollControl.AutoHideBars = true;

                m_Page_map_MainPanel = new Base(m_Page_map_ScrollControl);
                m_Page_map_MainPanel.SetBounds(0, 0, 100, 500);
                m_Page_map_MainPanel.Dock = Pos.Top;


                int btn_offset = 7;


                //  coordinates
				m_Page_map_GroupBox_coordinates = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_Page_map_MainPanel);
                m_Page_map_GroupBox_coordinates.TextColor = LabelColor;
                m_Page_map_GroupBox_coordinates.Text = "coordinates";
                m_Page_map_GroupBox_coordinates.Dock = Pos.Top;
                m_Page_map_GroupBox_coordinates.AutoSizeToContents = true;
                {
                    //  lat
					Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Page_map_GroupBox_coordinates);
                    label.TextColor = LabelColor;
                    label.AutoSizeToContents = true;
                    label.Text = "lat:";
                    label.Dock = Pos.Top;
                    //  set top offset
                    label.Margin = new Margin(0, 5, 0, 2);

					textBoxLat = new Alt.GUI.Temporary.Gwen.Control.TextBox(m_Page_map_GroupBox_coordinates);
                    textBoxLat.ReadOnly = true;
                    textBoxLat.Dock = Pos.Top;
                    textBoxLat.Text = "54.6961334816182";


                    //  lng
					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Page_map_GroupBox_coordinates);
                    label.TextColor = LabelColor;
                    label.AutoSizeToContents = true;
                    label.Text = "lng:";
                    label.Dock = Pos.Top;
                    label.Margin = new Margin(0, 0, 0, 2);

					textBoxLng = new Alt.GUI.Temporary.Gwen.Control.TextBox(m_Page_map_GroupBox_coordinates);
                    textBoxLng.ReadOnly = true;
                    textBoxLng.Dock = Pos.Top;
                    textBoxLng.Text = "25.2985095977783";


                    //  goto
					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Page_map_GroupBox_coordinates);
                    label.TextColor = LabelColor;
                    label.AutoSizeToContents = true;
                    label.Text = "goto:";
                    label.Dock = Pos.Top;
                    label.Margin = new Margin(0, 0, 0, 2);

					textBoxGeo = new Alt.GUI.Temporary.Gwen.Control.TextBox(m_Page_map_GroupBox_coordinates);
                    textBoxGeo.ReadOnly = true;
                    textBoxGeo.Dock = Pos.Top;
                    textBoxGeo.Text = "lietuva vilnius";


                    //  GoTo
					button8 = new Alt.GUI.Temporary.Gwen.Control.Button(m_Page_map_GroupBox_coordinates);
                    button8.Text = "GoTo !";
                    button8.Dock = Pos.Top;
                    //  set top offset
                    button8.Margin = new Margin(0, btn_offset, 0, 0);
                    button8.Click += new EventHandler(button8_Click);


                    //  Reload
					button1 = new Alt.GUI.Temporary.Gwen.Control.Button(m_Page_map_GroupBox_coordinates);
                    button1.Text = "Reload";
                    button1.Dock = Pos.Top;
                    //  set top offset
                    button1.Margin = new Margin(0, btn_offset, 0, 0);
                    button1.Click += new EventHandler(button1_Click);
                }


                //  gmap
				m_Page_map_GroupBox_gmap = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_Page_map_MainPanel);
                m_Page_map_GroupBox_gmap.TextColor = LabelColor;
                m_Page_map_GroupBox_gmap.Text = "gmap";
                m_Page_map_GroupBox_gmap.Dock = Pos.Top;
                m_Page_map_GroupBox_gmap.AutoSizeToContents = true;
                //  set top offset
                m_Page_map_GroupBox_gmap.Margin = new Margin(0, 10, 0, 0);
                {
                    //  type
					Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Page_map_GroupBox_gmap);
                    label.TextColor = LabelColor;
                    label.AutoSizeToContents = true;
                    label.Text = "type:";
                    label.Dock = Pos.Top;
                    //  set top offset
                    label.Margin = new Margin(0, 5, 0, 0);

					comboBoxMapType = new Alt.GUI.Temporary.Gwen.Control.ComboBox(m_Page_map_GroupBox_gmap);
                    comboBoxMapType.Dock = Pos.Top;


                    //  mode
					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_Page_map_GroupBox_gmap);
                    label.TextColor = LabelColor;
                    label.AutoSizeToContents = true;
                    label.Text = "mode:";
                    label.Dock = Pos.Top;

					comboBoxMode = new Alt.GUI.Temporary.Gwen.Control.ComboBox(m_Page_map_GroupBox_gmap);
                    comboBoxMode.Dock = Pos.Top;
#if UNITY_WEBPLAYER || UNITY_5
                    comboBoxMode.Disable();
#endif


                    //  Current Marker
					checkBoxCurrentMarker = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(m_Page_map_GroupBox_gmap);
                    checkBoxCurrentMarker.TextColor = LabelColor;
                    checkBoxCurrentMarker.UseCurrentColorAsNormal = true;
                    checkBoxCurrentMarker.Text = "Current Marker";
                    checkBoxCurrentMarker.Dock = Pos.Top;
                    //  set top offset
                    checkBoxCurrentMarker.Margin = new Margin(0, btn_offset, 0, 0);
                    checkBoxCurrentMarker.CheckedChanged += new EventHandler(checkBoxCurrentMarker_CheckedChanged);


                    //  Grid
					checkBoxDebug = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(m_Page_map_GroupBox_gmap);
                    checkBoxDebug.TextColor = LabelColor;
                    checkBoxDebug.UseCurrentColorAsNormal = true;
                    checkBoxDebug.Text = "Grid";
                    checkBoxDebug.Dock = Pos.Top;
                    //  set top offset
                    checkBoxDebug.Margin = new Margin(0, btn_offset, 0, 0);
                    checkBoxDebug.CheckedChanged += new EventHandler(checkBoxDebug_CheckedChanged);


                    //  Drag Map
					checkBoxCanDrag = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(m_Page_map_GroupBox_gmap);
                    checkBoxCanDrag.TextColor = LabelColor;
                    checkBoxCanDrag.UseCurrentColorAsNormal = true;
                    checkBoxCanDrag.Text = "Drag Map (right mouse button)";
                    checkBoxCanDrag.Dock = Pos.Top;
                    //  set top offset
                    checkBoxCanDrag.Margin = new Margin(0, btn_offset, 0, 0);
                    checkBoxCanDrag.CheckedChanged += new EventHandler(checkBoxCanDrag_CheckedChanged);
                }


                //  routing
				m_Page_map_GroupBox_routing = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_Page_map_MainPanel);
                m_Page_map_GroupBox_routing.TextColor = LabelColor;
                m_Page_map_GroupBox_routing.Text = "routing";
                m_Page_map_GroupBox_routing.Dock = Pos.Top;
                m_Page_map_GroupBox_routing.AutoSizeToContents = true;
                //  set top offset
                m_Page_map_GroupBox_routing.Margin = new Margin(0, 10, 0, 0);
                {
                    //  set Start
					buttonSetStart = new Alt.GUI.Temporary.Gwen.Control.Button(m_Page_map_GroupBox_routing);
                    buttonSetStart.Text = "set Start";
                    buttonSetStart.Dock = Pos.Top;
                    //  set top offset
                    buttonSetStart.Margin = new Margin(0, 7, 0, 0);
                    buttonSetStart.Click += new EventHandler(buttonSetStart_Click);


                    //  set End
					buttonSetEnd = new Alt.GUI.Temporary.Gwen.Control.Button(m_Page_map_GroupBox_routing);
                    buttonSetEnd.Text = "set End";
                    buttonSetEnd.Dock = Pos.Top;
                    //  set top offset
                    buttonSetEnd.Margin = new Margin(0, btn_offset, 0, 0);
                    buttonSetEnd.Click += new EventHandler(buttonSetEnd_Click);


                    //  Add Route
					button3 = new Alt.GUI.Temporary.Gwen.Control.Button(m_Page_map_GroupBox_routing);
                    button3.Text = "Add Route";
                    button3.Dock = Pos.Top;
                    //  set top offset
                    button3.Margin = new Margin(0, btn_offset, 0, 10);
                    button3.Click += new EventHandler(button3_Click);


                    //  Clear Routes
					button6 = new Alt.GUI.Temporary.Gwen.Control.Button(m_Page_map_GroupBox_routing);
                    button6.Text = "Clear Routes";
                    button6.Dock = Pos.Top;
                    //  set top offset
                    button6.Margin = new Margin(0, btn_offset, 0, 0);
                    button6.Click += new EventHandler(button6_Click);


                    //  Clear Polygons
					button15 = new Alt.GUI.Temporary.Gwen.Control.Button(m_Page_map_GroupBox_routing);
                    button15.Text = "Clear Polygons";
                    button15.Dock = Pos.Top;
                    //  set top offset
                    button15.Margin = new Margin(0, btn_offset, 0, 0);
                    button15.Click +=new EventHandler(button15_Click);
                }


                //  markers
				m_Page_map_GroupBox_markers = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_Page_map_MainPanel);
                m_Page_map_GroupBox_markers.TextColor = LabelColor;
                m_Page_map_GroupBox_markers.Text = "markers";
                m_Page_map_GroupBox_markers.Dock = Pos.Top;
                m_Page_map_GroupBox_markers.AutoSizeToContents = true;
                //  set top offset
                m_Page_map_GroupBox_markers.Margin = new Margin(0, 10, 0, 0);
                {
                    //  Add Marker
					button4 = new Alt.GUI.Temporary.Gwen.Control.Button(m_Page_map_GroupBox_markers);
                    button4.Text = "Add Marker";
                    button4.Dock = Pos.Top;
                    //  set top offset
                    button4.Margin = new Margin(0, 7, 0, 0);
                    button4.Click += new EventHandler(button4_Click);


                    //  place info
					checkBoxPlacemarkInfo = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(m_Page_map_GroupBox_markers);
                    checkBoxPlacemarkInfo.TextColor = LabelColor;
                    checkBoxPlacemarkInfo.UseCurrentColorAsNormal = true;
                    checkBoxPlacemarkInfo.Text = "place info";
                    checkBoxPlacemarkInfo.Dock = Pos.Top;
                    //  set top offset
                    checkBoxPlacemarkInfo.Margin = new Margin(0, btn_offset, 0, 0);
                    checkBoxPlacemarkInfo.IsChecked = true;


                    //  Zoom Center
					button7 = new Alt.GUI.Temporary.Gwen.Control.Button(m_Page_map_GroupBox_markers);
                    button7.Text = "Zoom Center";
                    button7.Dock = Pos.Top;
                    //  set top offset
                    button7.Margin = new Margin(0, btn_offset, 0, 0);
                    button7.Click += new EventHandler(button7_Click);


                    //  Clear All
					button5 = new Alt.GUI.Temporary.Gwen.Control.Button(m_Page_map_GroupBox_markers);
                    button5.Text = "Clear All";
                    button5.Dock = Pos.Top;
                    //  set top offset
                    button5.Margin = new Margin(0, btn_offset, 0, 0);
                    button5.Click += new EventHandler(button5_Click);
                }
            }
        }


        void CreateUI2()
        {
            Base rightPanel = new Base(m_MainSplitter);
            rightPanel.Dock = Pos.Fill;
            //rightPanel.Margin = new Margin(0);
            m_MainSplitter.SetPanel(0, rightPanel);


			Alt.GUI.Temporary.Gwen.Control.Label label_TOP = new Alt.GUI.Temporary.Gwen.Control.Label(rightPanel);
            label_TOP.TextColor = LabelColor;
            label_TOP.AutoSizeToContents = true;
            label_TOP.Text = "GMap.NET Interactive Example (use right mouse button to pan map)";
            label_TOP.TextColor = Color.Yellow;
            label_TOP.Dock = Pos.Top;
            label_TOP.Margin = new Margin(0, 3, 0, 7);

            
            //  cache Tab
			Alt.GUI.Temporary.Gwen.Control.TabButton button = m_TabControl.AddPage("cache");
			Alt.GUI.Temporary.Gwen.Control.Base page = button.Page;
            {
                int btn_offset = 7;

				button2 = new Alt.GUI.Temporary.Gwen.Control.Button(page);
                button2.Text = "Clear tiles in disk cache";
                button2.Dock = Pos.Top;
                //  set top offset
                button2.Margin = new Margin(0, btn_offset, 0, 0);
                button2.Click += new EventHandler(button2_Click);


                //  memory cache usage
				Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(page);
                label.TextColor = LabelColor;
                label.AutoSizeToContents = true;
                label.Text = "memory cache usage:";
                label.Dock = Pos.Top;
                //  set top offset
                label.Margin = new Margin(0, 12, 0, 2);

				textBoxMemory = new Alt.GUI.Temporary.Gwen.Control.TextBox(page);
                textBoxMemory.ReadOnly = true;
                textBoxMemory.Dock = Pos.Top;


                //  disk cache size
				label = new Alt.GUI.Temporary.Gwen.Control.Label(page);
                label.TextColor = LabelColor;
                label.AutoSizeToContents = true;
                label.Text = "disk cache size:";
                label.Dock = Pos.Top;
                //  set top offset
                label.Margin = new Margin(0, 12, 0, 2);

				textBoxCacheSize = new Alt.GUI.Temporary.Gwen.Control.TextBox(page);
                textBoxCacheSize.ReadOnly = true;
                textBoxCacheSize.Dock = Pos.Top;


                //  disk cache status
				label = new Alt.GUI.Temporary.Gwen.Control.Label(page);
                label.TextColor = LabelColor;
                label.AutoSizeToContents = true;
                label.Text = "disk cache status:";
                label.Dock = Pos.Top;
                //  set top offset
                label.Margin = new Margin(0, 12, 0, 2);

				textBoxCacheStatus = new Alt.GUI.Temporary.Gwen.Control.TextBox(page);
                textBoxCacheStatus.ReadOnly = true;
                textBoxCacheStatus.Dock = Pos.Top;


                //  cache routing/geocodig/etc
				checkBoxUseRouteCache = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(page);
                checkBoxUseRouteCache.TextColor = LabelColor;
                checkBoxUseRouteCache.UseCurrentColorAsNormal = true;
                checkBoxUseRouteCache.Text = "cache routing/geocodig/etc";
                checkBoxUseRouteCache.Dock = Pos.Top;
                //  set top offset
                checkBoxUseRouteCache.Margin = new Margin(0, 14, 0, 0);
                checkBoxUseRouteCache.IsChecked = true;
            }
#if UNITY_WEBPLAYER
            button.Hide();
#endif


            //  info Tab
            button = m_TabControl.AddPage("info");
            page = button.Page;
            {
                //  lat
				Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(page);
                label.TextColor = LabelColor;
                label.AutoSizeToContents = true;
                label.Text = "lat:";
                label.Dock = Pos.Top;
                //  set top offset
                label.Margin = new Margin(0, 4, 0, 0);

				textBoxLatCurrent = new Alt.GUI.Temporary.Gwen.Control.TextBox(page);
                textBoxLatCurrent.ReadOnly = true;
                textBoxLatCurrent.Dock = Pos.Top;


                //  lng
				label = new Alt.GUI.Temporary.Gwen.Control.Label(page);
                label.TextColor = LabelColor;
                label.AutoSizeToContents = true;
                label.Text = "lng:";
                label.Dock = Pos.Top;

				textBoxLngCurrent = new Alt.GUI.Temporary.Gwen.Control.TextBox(page);
                textBoxLngCurrent.ReadOnly = true;
                textBoxLngCurrent.Dock = Pos.Top;


                //  zoom
				label = new Alt.GUI.Temporary.Gwen.Control.Label(page);
                label.TextColor = LabelColor;
                label.AutoSizeToContents = true;
                label.Text = "zoom:";
                label.Dock = Pos.Top;
                //  set top offset
                label.Margin = new Margin(0, 12, 0, 0);

				textBoxZoomCurrent = new Alt.GUI.Temporary.Gwen.Control.TextBox(page);
                textBoxZoomCurrent.ReadOnly = true;
                textBoxZoomCurrent.Dock = Pos.Top;


                //  markers
				label = new Alt.GUI.Temporary.Gwen.Control.Label(page);
                label.TextColor = LabelColor;
                label.AutoSizeToContents = true;
                label.Text = "markers:";
                label.Dock = Pos.Top;

				textBoxMarkerCount = new Alt.GUI.Temporary.Gwen.Control.TextBox(page);
                textBoxMarkerCount.ReadOnly = true;
                textBoxMarkerCount.Dock = Pos.Top;


                //  routes
				label = new Alt.GUI.Temporary.Gwen.Control.Label(page);
                label.TextColor = LabelColor;
                label.AutoSizeToContents = true;
                label.Text = "routes:";
                label.Dock = Pos.Top;

				textBoxrouteCount = new Alt.GUI.Temporary.Gwen.Control.TextBox(page);
                textBoxrouteCount.ReadOnly = true;
                textBoxrouteCount.Dock = Pos.Top;
            }


            //  MAP
            MainMap = new Map(rightPanel);
            MainMap.Dock = Pos.Fill;
            //MainMap.Margin = new Margin(1, 0, 1, 0);
            //MainMap.DrawClientBorder = true;
            MainMap.DrawBorder = true;


            //  Scale
            Base m_ScaleControl = new Base(MainMap);
            m_ScaleControl.SetBounds(10, 10, 20, 200);

			Alt.GUI.Temporary.Gwen.Control.Button plus = new Alt.GUI.Temporary.Gwen.Control.Button(m_ScaleControl);
            plus.Text = "+";
            plus.Dock = Pos.Top;
            plus.Click += new EventHandler(buttonZoomUp_Click);

			Alt.GUI.Temporary.Gwen.Control.Button minus = new Alt.GUI.Temporary.Gwen.Control.Button(m_ScaleControl);
            minus.Text = "-";
            minus.Dock = Pos.Bottom;
            minus.Click += new EventHandler(buttonZoomDown_Click);

			m_ScaleTrackBar = new Alt.GUI.Temporary.Gwen.Control.VerticalSlider(m_ScaleControl);
			m_ScaleTrackBar.Dock = Pos.Fill;
			m_ScaleTrackBar.ValueChanged += new GwenEventHandler(trackBar1_ValueChanged);
        }


        protected override void Render(Alt.GUI.Temporary.Gwen.Skin.Base skin)
        {
            //  Neet to correct "map" page view
            if (m_Page_map_MainPanel != null &&
                m_Page_map_MainPanel.IsVisible)
            {
                /*int Height = m_Page_map_MainPanel.GetChildrenSize().Height;
                if (m_Page_map_MainPanel.Height != Height)
                {
                    m_Page_map_MainPanel.Height = m_Page_map_MainPanel.GetChildrenSize().Height;
                }*/
                m_Page_map_MainPanel.SizeToChildren(false, true);

                int offset = m_Page_map_ScrollControl.IsVerticalScrollBarVisibled ? 5 : 0;
                if (//m_Page_map_MainPanel.Margin == null ||
                    m_Page_map_MainPanel.Margin.Right != offset)
                {
                    m_Page_map_MainPanel.Margin = new Margin(
                        0, 0,
                        offset,
                        0);
                }
            }

            base.Render(skin);
        }
    }
}
#endif
