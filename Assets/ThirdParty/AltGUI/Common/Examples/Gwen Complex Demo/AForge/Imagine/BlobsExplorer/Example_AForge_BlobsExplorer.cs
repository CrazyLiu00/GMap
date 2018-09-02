// Blobs Browser sample application
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
using Alt.GUI.Demo.BlobsExplorer;


namespace Alt.GUI.Demo
{
    class Example_AForge_BlobsExplorer : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 0;
                int y = 0;

                if (m_TopPanel != null)
                {
                    y = m_TopPanel.Height + m_TopPanel.Margin.Height;
                }

                return new SizeI(x, y);
            }
        }

        
        string Description
        {
            get
            {
				return "AltGUI AForge.NET Blobs Explorer Example";
            }
        }


        Bitmap Screenshot
        {
            get
            {
                return Example_NotAvailable_ScreenShot.LoadImage("Example_AForge_BlobsExplorer.gif");
            }
        }


		Alt.GUI.Temporary.Gwen.Control.Base m_TopPanel;
		Alt.GUI.Temporary.Gwen.Control.Base m_BottomPanel;
		Alt.GUI.Temporary.Gwen.Control.PropertyTree propertyGrid;
		Alt.GUI.Temporary.Gwen.Control.ComboBox highlightTypeCombo;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox showRectangleAroundSelectionCheck;
        BlobsBrowser blobsBrowser;
		Alt.GUI.Temporary.Gwen.Control.Label blobsCountLabel;


        public Example_AForge_BlobsExplorer(Base parent)
            : base(parent)
        {
#if SILVERLIGHT || UNITY_WEBPLAYER
			Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
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
				Alt.GUI.Temporary.Gwen.Control.StatusBar status = new Alt.GUI.Temporary.Gwen.Control.StatusBar(this);
                {
                    status.ShouldDrawBackground = false;

					blobsCountLabel = new Alt.GUI.Temporary.Gwen.Control.Label(status);
                    blobsCountLabel.AutoSizeToContents = true;
                    status.AddControl(blobsCountLabel, false);
                }

				m_BottomPanel = new Alt.GUI.Temporary.Gwen.Control.Base(this);
                {
                    m_BottomPanel.Dock = Pos.Bottom;
                    m_BottomPanel.Height = 210;
                }


				m_TopPanel = new Alt.GUI.Temporary.Gwen.Control.Base(this);
                {
                    m_TopPanel.Dock = Pos.Top;
                    m_TopPanel.Height = 30;

					highlightTypeCombo = new Alt.GUI.Temporary.Gwen.Control.ComboBox(m_TopPanel);
                    highlightTypeCombo.AddItem("Convex Hull").UserData = 0;
                    highlightTypeCombo.AddItem("Left/Right Edges").UserData = 1;
                    highlightTypeCombo.AddItem("Top/Bottom Edges").UserData = 2;
                    highlightTypeCombo.AddItem("Quadrilateral").UserData = 3;
                    highlightTypeCombo.Location = new PointI(0, 0);
                    highlightTypeCombo.Width = 133;
                    highlightTypeCombo.ItemSelected += new GwenEventHandler(highlightTypeCombo_SelectedIndexChanged);

                    showRectangleAroundSelectionCheck = new LabeledCheckBox(m_TopPanel);
                    showRectangleAroundSelectionCheck.Location = new PointI(143, 1);
                    showRectangleAroundSelectionCheck.Text = "Show rectangle around selection";
                    showRectangleAroundSelectionCheck.Width += 10;
                    showRectangleAroundSelectionCheck.CheckedChanged += new System.EventHandler(showRectangleAroundSelectionCheck_CheckedChanged);
                }


                blobsBrowser = new BlobsBrowser(this);
                blobsBrowser.Dock = Pos.Fill;
                blobsBrowser.Highlighting = BlobsExplorer.BlobsBrowser.HightlightType.ConvexHull;
#if !SILVERLIGHT && !UNITY_WEBPLAYER
                blobsBrowser.BlobSelected += new BlobsExplorer.BlobSelectionHandler(blobsBrowser_BlobSelected);
#endif
            }
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            showRectangleAroundSelectionCheck.IsChecked = blobsBrowser.ShowRectangleAroundSelection;

            LoadDemo();
        }


        // Process image
        void ProcessImage(Bitmap image)
        {
            int foundBlobsCount = blobsBrowser.SetImage(image);

            blobsCountLabel.Text = string.Format("Found blobs' count: {0}", foundBlobsCount);
            
            if (propertyGrid != null)
            {
                m_BottomPanel.RemoveChild(propertyGrid, true);
                propertyGrid = null;
            }
        }


#if !SILVERLIGHT && !UNITY_WEBPLAYER
        // Blob was selected - display its information
        void blobsBrowser_BlobSelected(object sender, Blob blob)
        {
            if (propertyGrid != null)
            {
                m_BottomPanel.RemoveChild(propertyGrid, true);
                propertyGrid = null;
            }

            if (blob != null)
            {
                propertyGrid = new PropertyTree(m_BottomPanel);
                propertyGrid.Dock = Pos.Fill;

                Properties props = propertyGrid.Add("General");
                {
                    props.Add("Area", blob.Area.ToString());
                    props.Add("CenterOfGravity", blob.CenterOfGravity.ToString());
                    props.Add("ColorMean", new Alt.GUI.Temporary.Gwen.Control.Property.Color(props),
                        blob.ColorMean.R.ToString() + " " + blob.ColorMean.G.ToString() + " " + blob.ColorMean.B.ToString());
                    props.Add("ColorStdDev", new Alt.GUI.Temporary.Gwen.Control.Property.Color(props),
                        blob.ColorStdDev.R.ToString() + " " + blob.ColorStdDev.G.ToString() + " " + blob.ColorStdDev.B.ToString());
                    props.Add("Fullness", blob.Fullness.ToString("F2").Replace(',', '.'));
                    
                    props.SplitWidth += 20;
                }

                props = propertyGrid.Add("Rectangle");
                {
                    props.Add("All",
                        blob.Rectangle.X.ToString() + " " +
                        blob.Rectangle.Y.ToString() + " " +
                        blob.Rectangle.Width.ToString() + " " +
                        blob.Rectangle.Height.ToString());
                    props.Add("X", blob.Rectangle.X.ToString());
                    props.Add("Y", blob.Rectangle.Y.ToString());
                    props.Add("Width", blob.Rectangle.Width.ToString());
                    props.Add("Height", blob.Rectangle.Height.ToString());

                    props.SplitWidth += 20;
                }

                propertyGrid.ExpandAll();
            }
        }
#endif


        void LoadDemo()
        {
            // load arrow bitmap
            Bitmap image = Bitmap.FromFile("AltData/AForge/BlobsExplorer_demo.gif");
            ProcessImage(image);
        }


        // Change type of blobs' highlighting
        void highlightTypeCombo_SelectedIndexChanged(Base control)
        {
            blobsBrowser.Highlighting = (BlobsBrowser.HightlightType)((int)highlightTypeCombo.SelectedItem.UserData);
        }


        // Toggle displaying of rectangle around selection
        void showRectangleAroundSelectionCheck_CheckedChanged(object sender, EventArgs e)
        {
            blobsBrowser.ShowRectangleAroundSelection = showRectangleAroundSelectionCheck.IsChecked;
        }
    }
}
