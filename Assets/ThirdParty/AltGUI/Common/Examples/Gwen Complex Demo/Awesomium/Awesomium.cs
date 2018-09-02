//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

//#define USE_Awesomium

using System;
using System.Collections.Generic;
using System.Diagnostics;

#if USE_Awesomium
using Awesomium.Core;
#endif

using Alt.GUI.Temporary.Gwen.Control.Layout;
using Alt.Sketch;
using Alt.Threading;


namespace Alt.GUI.Temporary.Gwen.Control
{
    public class Awesomium : Base
    {
        Bitmap Screenshot
        {
            get
            {
                return Bitmap.FromFile("AltData/NotAvailableExamples/" + "Example_Awesomium.jpg");
            }
        }


		Alt.GUI.Temporary.Gwen.Control.Base m_ControlsPanel;
		Alt.GUI.Temporary.Gwen.Control.Button m_BackButton;
		Alt.GUI.Temporary.Gwen.Control.Button m_ForwardButton;
		Alt.GUI.Temporary.Gwen.Control.Button m_HomeButton;
		Alt.GUI.Temporary.Gwen.Control.Button m_StopButton;
		Alt.GUI.Temporary.Gwen.Control.Button m_RefreshButton;
		Alt.GUI.Temporary.Gwen.Control.TextBox m_AddressTextBox;
		Alt.GUI.Temporary.Gwen.Control.Button m_GoButton;
		Alt.GUI.Temporary.Gwen.Control.Label m_Tittle;

		internal Alt.GUI.Temporary.Gwen.Control.Base m_WebViewPanel;


#if USE_Awesomium
        //Awesomium main component: m_WebView
        WebView m_WebView;
       
        Bitmap m_Image;
		Alt.GUI.Temporary.Gwen.Control.Label m_TargetLabel;
#else
		Alt.GUI.Temporary.Gwen.Control.PictureBox m_Example_NotAvailable_PictureBox;
#endif


        public Awesomium(Base parent) :
            base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


#if !USE_Awesomium
			Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            label.AutoSizeToContents = true;
            label.Text = "THIS EXAMPLE IS NOT AVAILABLE IN THIS DEMO,\nPLEASE DOWNLOAD AltGUI SDK";
            label.TextColor = Color.Orange * 1.3;
            label.Dock = Pos.Top;
            label.Margin = new Margin(1, 1, 1, 0);
            label.Font = new Font("Arial", 12, FontStyle.Bold);
#endif


			Alt.GUI.Temporary.Gwen.Control.Base topPanel = new Alt.GUI.Temporary.Gwen.Control.Base(this);
            topPanel.Dock = Pos.Top;
            topPanel.Height = 22;

			m_Tittle = new Alt.GUI.Temporary.Gwen.Control.Label(topPanel);
            m_Tittle.AutoSizeToContents = true;
			m_Tittle.Font = new Font("Arial", 12, FontStyle.Bold);
			m_Tittle.TextColor = Color.WhiteSmoke;// * 1.2;
            m_Tittle.Dock = Pos.Left;
            m_Tittle.Margin = new Margin(1, 3, 1, 4);
#if !USE_Awesomium
            m_Tittle.Text = "AltSoftLab.com";
#endif
            

			Alt.GUI.Temporary.Gwen.Control.Base controlsMain = new Alt.GUI.Temporary.Gwen.Control.Base(this);
            controlsMain.DrawBorder = true;
            controlsMain.BorderColor = Color.DodgerBlue;
            controlsMain.Dock = Pos.Top;
            controlsMain.Height = 38;
            controlsMain.Margin = new Margin(2, 0, 2, 0);

            m_ControlsPanel = new Base(controlsMain);
            m_ControlsPanel.Dock = Pos.Fill;


			m_BackButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_ControlsPanel);
            m_BackButton.Dock = Pos.Left;
            m_BackButton.Click += new EventHandler(BackButton_Click);
            m_BackButton.SetToolTipText("Back");
            m_BackButton.SetImage("AltData/Back.png", true);
            m_BackButton.Width = 36;
            m_BackButton.Margin = new Margin(1, 1, 0, 1);

			m_ForwardButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_ControlsPanel);
            m_ForwardButton.Dock = Pos.Left;
            m_ForwardButton.Click += new EventHandler(ForwardButton_Click);
            m_ForwardButton.SetToolTipText("Forward");
            m_ForwardButton.SetImage("AltData/Forward.png", true);
            m_ForwardButton.Width = 36;
            m_ForwardButton.Margin = new Margin(1, 1, 0, 1);

			m_HomeButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_ControlsPanel);
            m_HomeButton.Dock = Pos.Left;
            m_HomeButton.Click += new EventHandler(HomeButton_Click);
            m_HomeButton.SetToolTipText("Home");
            m_HomeButton.SetImage("AltData/Awesomium/Home.png", true);
            m_HomeButton.Width = 36;
            m_HomeButton.Margin = new Margin(5, 1, 5, 1);

			m_RefreshButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_ControlsPanel);
            m_RefreshButton.Dock = Pos.Left;
            m_RefreshButton.Click += new EventHandler(RefreshButton_Click);
            m_RefreshButton.SetToolTipText("Refresh");
            m_RefreshButton.SetImage("AltData/Refresh.png", true);
            m_RefreshButton.Width = 36;
            m_RefreshButton.Margin = new Margin(0, 1, 0, 1);

			m_StopButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_ControlsPanel);
            m_StopButton.Dock = Pos.Left;
            m_StopButton.Click += new EventHandler(StopButton_Click);
            m_StopButton.SetToolTipText("Stop");
            m_StopButton.SetImage("AltData/Stop.png", true);
            m_StopButton.Width = 36;
            m_StopButton.Margin = new Margin(1, 1, 0, 1);


			m_AddressTextBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(m_ControlsPanel);
            m_AddressTextBox.Dock = Pos.Fill;
            m_AddressTextBox.Margin = new Margin(5, 6, 5, 7);
            m_AddressTextBox.KeyDown += new GUI.KeyEventHandler(AddressTextBox_KeyDown);
            m_AddressTextBox.Font = new Font("Arial", 12);
            m_AddressTextBox.TextColor = Color.DarkBlue;


			m_GoButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_ControlsPanel);
            m_GoButton.Dock = Pos.Right;
            m_GoButton.Click += new EventHandler(GoButton_Click);
            m_GoButton.SetToolTipText("Go");
            m_GoButton.SetImage("AltData/Awesomium/Go.png", true);
            m_GoButton.Width = 37;
            m_GoButton.Margin = new Margin(0, 1, 1, 1);
            m_GoButton.HoverEnter += new GwenEventHandler(GoButton_HoverEnter);
            m_GoButton.HoverLeave += new GwenEventHandler(GoButton_HoverLeave);


            m_WebViewPanel = new Base(this);
            m_WebViewPanel.DrawBorder = true;
            m_WebViewPanel.BorderColor = Color.DodgerBlue;
            m_WebViewPanel.Dock = Pos.Fill;
            m_WebViewPanel.Margin = new Margin(2, 0, 2, 2);
#if USE_Awesomium
            m_WebViewPanel.Paint += new PaintEventHandler(ImagePanel_Paint);
            m_WebViewPanel.Resize += new EventHandler(WebViewPanel_Resize);
            m_WebViewPanel.MouseDown += new MouseEventHandler(WebViewPanel_MouseDown);
            m_WebViewPanel.MouseUp += new MouseEventHandler(WebViewPanel_MouseUp);
            m_WebViewPanel.MouseMove += new MouseEventHandler(WebViewPanel_MouseMove);
            m_WebViewPanel.MouseWheel += new MouseEventHandler(WebViewPanel_MouseWheel);
            m_WebViewPanel.KeyDown += new KeyEventHandler(WebViewPanel_KeyDown);
            m_WebViewPanel.KeyUp += new KeyEventHandler(WebViewPanel_KeyUp);
            m_WebViewPanel.KeyPress += new KeyPressEventHandler(WebViewPanel_KeyPress);
            m_WebViewPanel.MouseInputEnabled = true;
            m_WebViewPanel.KeyboardInputEnabled = true;
            m_WebViewPanel.Focus();


            m_TargetLabel = new Label(m_WebViewPanel);
            m_TargetLabel.AutoSizeToContents = true;
            m_TargetLabel.MouseInputEnabled = false;
            m_TargetLabel.TextColor = Color.DarkBlue;
            m_TargetLabel.X = 1;
            m_TargetLabel.ClientBackColor = Color.FromArgb(192, Color.White);
            

            //  Awesomium
            WebConfig config = new WebConfig();
            WebCore.Initialize(config);

            m_WebView = WebCore.CreateWebView(800, 600);
            //m_WebView.IsTransparent = true;
            m_WebView.FocusView();
            m_WebView.CursorChanged += new CursorChangedEventHandler(WebView_CursorChanged);
            m_WebView.AddressChanged += new UrlEventHandler(WebView_AddressChanged);
            m_WebView.TargetURLChanged += new UrlEventHandler(WebView_TargetURLChanged);
            m_WebView.TitleChanged += new TitleChangedEventHandler(WebView_TitleChanged);
            m_WebView.ToolTipChanged += new ToolTipChangedEventHandler(WebView_ToolTipChanged);

            WebCore.HomeURL =
                //new Uri("http://www.google.com");
                new Uri("http://www.altsoftlab.com");

            m_WebView.GoToHome();
#else
            m_AddressTextBox.Text = "http://www.altsoftlab.com";

            m_WebViewPanel.ClientBackColor = Color.Transparent;
            m_WebViewPanel.Resize += new EventHandler(WebViewPanel_Resize);

            Alt.GUI.Temporary.Gwen.Control.ScrollControl scrollControl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(m_WebViewPanel);
            scrollControl.Margin = Margin.Zero;
            scrollControl.Dock = Pos.Fill;
            scrollControl.EnableScroll(true, true);
            scrollControl.AutoHideBars = true;
			scrollControl.ScrollBarsShouldDrawBackground = false;

            m_Example_NotAvailable_PictureBox = new Alt.GUI.Temporary.Gwen.Control.PictureBox(scrollControl);
            m_Example_NotAvailable_PictureBox.Margin = Margin.Zero;

            Bitmap screenshot = Screenshot;
            if (screenshot != null)
            {
                m_Example_NotAvailable_PictureBox.Image = screenshot;
                m_Example_NotAvailable_PictureBox.Size = screenshot.PixelSize;
				m_Example_NotAvailable_PictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            }
#endif
        }


        public override void Dispose()
        {
#if USE_Awesomium
            if (m_WebView != null)
            {
                m_WebView.Dispose();
                m_WebView = null;
            }

            // Shut down Awesomium before exiting.
            WebCore.Shutdown();
#endif

            base.Dispose();
        }


        void BackButton_Click(object sender, EventArgs e)
        {
#if USE_Awesomium
            if (m_WebView != null)
            {
                m_WebView.GoBack();
            }
#endif
        }

        void ForwardButton_Click(object sender, EventArgs e)
        {
#if USE_Awesomium
            if (m_WebView != null)
            {
                m_WebView.GoForward();
            }
#endif
        }

        void HomeButton_Click(object sender, EventArgs e)
        {
#if USE_Awesomium
            if (m_WebView != null)
            {
                m_WebView.GoToHome();
            }
#endif
        }

        void RefreshButton_Click(object sender, EventArgs e)
        {
#if USE_Awesomium
            if (m_WebView != null)
            {
                m_WebView.Reload(true);
            }
#endif
        }

        void StopButton_Click(object sender, EventArgs e)
        {
#if USE_Awesomium
            if (m_WebView != null)
            {
                m_WebView.Stop();
            }
#endif
        }

        void GoButton_Click(object sender, EventArgs e)
        {
            Go();
        }

        void Go()
        {
#if USE_Awesomium
            if (m_WebView != null)
            {
                string uri = m_AddressTextBox.Text;
                if (string.IsNullOrEmpty(uri))
                {
                    return;
                }

                uri = uri.ToLowerInvariant();

                if (!uri.StartsWith("http://") &&
                    !uri.StartsWith("https://"))
                {
                    if (!uri.StartsWith("www."))
                    {
                        uri = "www." + uri;
                    }

                    uri = "http://" + uri;
                }

                try
                {
                    Uri u = new Uri(uri);

                    m_WebView.Source = u;
                }
                catch
                {
                }
            }
#endif
        }

        void AddressTextBox_KeyDown(object sender, GUI.KeyEventArgs e)
        {
            if (e.KeyCode == GUI.Keys.Enter)
            {
                Go();
            }
        }


        void WebViewPanel_Resize(object sender, EventArgs e)
        {
#if USE_Awesomium
            if (m_WebView != null &&
                m_WebViewPanel != null &&
                (m_WebView.Width != m_WebViewPanel.ClientWidth || m_WebView.Height != m_WebViewPanel.ClientHeight))
            {
                m_WebView.Width = m_WebViewPanel.ClientWidth;
                m_WebView.Height = m_WebViewPanel.ClientHeight;
            }
#else
            if (m_Example_NotAvailable_PictureBox.Image != null)
            {
                int x = (m_WebViewPanel.Width - m_Example_NotAvailable_PictureBox.Image.PixelWidth - 15) / 2;
                if (x < 0)
                {
                    x = 0;
                }
                m_Example_NotAvailable_PictureBox.Location = new PointI(x, 0);
            }
#endif
        }


        void GoButton_HoverEnter(Base control)
        {
            m_GoButton.SetImage("AltData/Awesomium/Go2.png", true);
        }

        void GoButton_HoverLeave(Base control)
        {
            m_GoButton.SetImage("AltData/Awesomium/Go.png", true);
        }


#if USE_Awesomium
        protected internal override void OnTick(double delta)
        {
            base.OnTick(delta);

            if (m_WebView != null)
            {
                WebCore.Update();

                m_BackButton.IsDisabled = !m_WebView.CanGoBack();
                m_ForwardButton.IsDisabled = !m_WebView.CanGoForward();
            }
        }


        void ImagePanel_Paint(object sender, PaintEventArgs e)
        {
            if (m_WebView == null)
            {
                return;
            }


            BitmapSurface surface = (BitmapSurface)m_WebView.Surface;
            if (surface == null)
            {
                return;
            }


            if (surface.IsDirty)
            {
                int sw = surface.Width;
                int sh = surface.Height;

                if (m_Image != null &&
                    (m_Image.PixelWidth != sw || m_Image.PixelHeight != sh))
                {
                    m_Image.Dispose();
                    m_Image = null;
                }


                int size = surface.RowSpan * surface.Height;
                if (m_Image == null)
                {
                    byte[] src = new byte[size];
                    System.Runtime.InteropServices.Marshal.Copy(surface.Buffer, src, 0, size);
                    m_Image = new Bitmap(sw, sh, sw * 4, PixelFormat.Format32bppArgb, src);
                }
                else
                {
                    BitmapData data = m_Image.LockBits(ImageLockMode.WriteOnly);

                    System.Runtime.InteropServices.Marshal.Copy(surface.Buffer, data.Scan0, 0, size);

                    m_Image.UnlockBits(data);
                }


                surface.IsDirty = false;
            }


            e.Graphics.DrawImage(m_Image, PointI.Zero);
        }


        void WebView_AddressChanged(object sender, UrlEventArgs e)
        {
            if (m_WebView != null)
            {
                m_AddressTextBox.Text = m_WebView.Source.ToString();
            }
        }


        void WebView_TargetURLChanged(object sender, UrlEventArgs e)
        {
            if (m_TargetLabel == null)
            {
                return;
            }

            string url = null;

            if (e != null &&
                e.Url != null)
            {
                url = e.Url.ToString();
            }

            if (string.IsNullOrEmpty(url))
            {
                m_TargetLabel.Hide();
                return;
            }

            m_TargetLabel.Text = url;
            m_TargetLabel.Show();

            UpdateTargetLabelPos();
        }

        void UpdateTargetLabelPos()
        {
            if (m_TargetLabel != null &&
                m_WebViewPanel != null)
            {
                m_TargetLabel.Y = m_WebViewPanel.Height - m_TargetLabel.Height - 1;
            }
        }


        void WebView_ToolTipChanged(object sender, ToolTipChangedEventArgs e)
        {
            if (m_WebViewPanel != null)
            {
                string s = e.ToolTip;
                if (string.IsNullOrEmpty(s))
                {
                    m_WebViewPanel.ToolTip = null;
                }
                else
                {
                    m_WebViewPanel.SetToolTipText(s);
                }
            }
        }

        void WebView_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            if (m_Tittle != null)
            {
                m_Tittle.Text = e.Title;
            }
        }


        MouseButton ToMouseButton(MouseButtons buttons)
        {
            if (buttons == MouseButtons.Right)
            {
                return MouseButton.Right;
            }
            
            if (buttons == MouseButtons.Middle)
            {
                return MouseButton.Middle;
            }

            return MouseButton.Left;
        }

        void WebViewPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.XButton1)
            {
                if (m_WebView != null)
                {
                    m_WebView.GoBack();
                }

                return;
            }
            else if (e.Button == MouseButtons.XButton2)
            {
                if (m_WebView != null)
                {
                    m_WebView.GoForward();
                }

                return;
            }


            if (m_WebView != null)
            {
                m_WebView.FocusView();
                m_WebView.InjectMouseDown(ToMouseButton(e.Button));
            }
        }

        void WebViewPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.XButton1 ||
                e.Button == MouseButtons.XButton2)
            {
                return;
            }


            if (m_WebView != null)
            {
                m_WebView.FocusView();
                m_WebView.InjectMouseUp(ToMouseButton(e.Button));
            }
        }

        void WebViewPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_WebView != null)
            {
                m_WebView.FocusView();
                m_WebView.InjectMouseMove(e.X, e.Y);
            }
        }

        void WebViewPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if (m_WebView != null)
            {
                m_WebView.FocusView();
                m_WebView.InjectMouseWheel(e.Delta, 0);
            }
        }


        void WebViewPanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (m_WebView != null)
            {
                m_WebView.FocusView();

                WebKeyboardEvent keyEv = new WebKeyboardEvent();
                keyEv.Type = WebKeyboardEventType.KeyDown;
                keyEv.VirtualKeyCode = TranslateKeyCode(e.KeyCode);
                keyEv.Modifiers = GetModifiers();
                //keyEv.NativeKeyCode = ;
                m_WebView.InjectKeyboardEvent(keyEv);
            }
        }

        void WebViewPanel_KeyUp(object sender, KeyEventArgs e)
        {
            if (m_WebView != null)
            {
                m_WebView.FocusView();

                WebKeyboardEvent keyEv = new WebKeyboardEvent();
                keyEv.Type = WebKeyboardEventType.KeyUp;
                keyEv.VirtualKeyCode = TranslateKeyCode(e.KeyCode);
                keyEv.Modifiers = GetModifiers();
                //keyEv.NativeKeyCode = ;
                m_WebView.InjectKeyboardEvent(keyEv);
            }
        }

        void WebViewPanel_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (m_WebView != null)
            {
                m_WebView.FocusView();

                WebKeyboardEvent keyEv = new WebKeyboardEvent();
                keyEv.Type = WebKeyboardEventType.Char;
                keyEv.Text = e.KeyChar.ToString();
                keyEv.Modifiers = GetModifiers();
                m_WebView.InjectKeyboardEvent(keyEv);
            }
        }


        void WebView_CursorChanged(object sender, CursorChangedEventArgs e)
        {
            if (m_WebViewPanel == null)
            {
                return;
            }


            switch (e.CursorType)
            {
                case CursorType.Pointer:
                    {
                        m_WebViewPanel.Cursor = Cursors.Arrow;
                        break;
                    }
                case CursorType.Cross:
                    {
                        m_WebViewPanel.Cursor = Cursors.Cross;
                        break;
                    }
                case CursorType.Hand:
                    {
                        m_WebViewPanel.Cursor = Cursors.Hand;
                        break;
                    }
                case CursorType.IBeam:
                    {
                        m_WebViewPanel.Cursor = Cursors.IBeam;
                        break;
                    }
                case CursorType.Wait:
                    {
                        m_WebViewPanel.Cursor = Cursors.WaitCursor;
                        break;
                    }
                case CursorType.Help:
                    {
                        m_WebViewPanel.Cursor = Cursors.Help;
                        break;
                    }
                case CursorType.EastResize:
                    {
                        m_WebViewPanel.Cursor = Cursors.SizeWE;
                        break;
                    }
                case CursorType.NorthResize:
                    {
                        m_WebViewPanel.Cursor = Cursors.SizeNS;
                        break;
                    }
                case CursorType.NorthEastResize:
                    {
                        m_WebViewPanel.Cursor = Cursors.SizeNESW;
                        break;
                    }
                case CursorType.NorthWestResize:
                    {
                        m_WebViewPanel.Cursor = Cursors.SizeNWSE;
                        break;
                    }
                case CursorType.SouthResize:
                    {
                        m_WebViewPanel.Cursor = Cursors.SizeNS;
                        break;
                    }
                case CursorType.SouthEastResize:
                    {
                        m_WebViewPanel.Cursor = Cursors.SizeNWSE;
                        break;
                    }
                case CursorType.SouthWestResize:
                    {
                        m_WebViewPanel.Cursor = Cursors.SizeNESW;
                        break;
                    }
                case CursorType.WestResize:
                    {
                        m_WebViewPanel.Cursor = Cursors.SizeWE;
                        break;
                    }
                case CursorType.NorthSouthResize:
                    {
                        m_WebViewPanel.Cursor = Cursors.SizeNS;
                        break;
                    }
                case CursorType.EastWestResize:
                    {
                        m_WebViewPanel.Cursor = Cursors.SizeWE;
                        break;
                    }
                case CursorType.NorthEastSouthWestResize:
                    {
                        m_WebViewPanel.Cursor = Cursors.SizeNESW;
                        break;
                    }
                case CursorType.NorthWestSouthEastResize:
                    {
                        m_WebViewPanel.Cursor = Cursors.SizeNWSE;
                        break;
                    }
                case CursorType.ColumnResize:
                    {
                        m_WebViewPanel.Cursor = Cursors.VSplit;
                        break;
                    }
                case CursorType.RowResize:
                    {
                        m_WebViewPanel.Cursor = Cursors.HSplit;
                        break;
                    }
                case CursorType.MiddlePanning:
                    {
                        m_WebViewPanel.Cursor = Cursors.SizeAll;
                        break;
                    }
                case CursorType.EastPanning:
                    {
                        m_WebViewPanel.Cursor = Cursors.PanEast;
                        break;
                    }
                case CursorType.NorthPanning:
                    {
                        m_WebViewPanel.Cursor = Cursors.PanNorth;
                        break;
                    }
                case CursorType.NorthEastPanning:
                    {
                        m_WebViewPanel.Cursor = Cursors.PanNE;
                        break;
                    }
                case CursorType.NorthWestPanning:
                    {
                        m_WebViewPanel.Cursor = Cursors.PanNW;
                        break;
                    }
                case CursorType.SouthPanning:
                    {
                        m_WebViewPanel.Cursor = Cursors.PanSouth;
                        break;
                    }
                case CursorType.SouthEastPanning:
                    {
                        m_WebViewPanel.Cursor = Cursors.PanSE;
                        break;
                    }
                case CursorType.SouthWestPanning:
                    {
                        m_WebViewPanel.Cursor = Cursors.PanSW;
                        break;
                    }
                case CursorType.WestPanning:
                    {
                        m_WebViewPanel.Cursor = Cursors.PanWest;
                        break;
                    }
                case CursorType.Move:
                    {
                        m_WebViewPanel.Cursor = Cursors.SizeAll;
                        break;
                    }
                case CursorType.VerticalText:
                    {
                        m_WebViewPanel.Cursor = Cursors.Arrow;
                        break;
                    }
                case CursorType.Cell:
                    {
                        m_WebViewPanel.Cursor = Cursors.Arrow;
                        break;
                    }
                case CursorType.ContextMenu:
                    {
                        m_WebViewPanel.Cursor = Cursors.Arrow;
                        break;
                    }
                case CursorType.Alias:
                    {
                        m_WebViewPanel.Cursor = Cursors.Arrow;
                        break;
                    }
                case CursorType.Progress:
                    {
                        m_WebViewPanel.Cursor = Cursors.Arrow;
                        break;
                    }
                case CursorType.NoDrop:
                    {
                        m_WebViewPanel.Cursor = Cursors.No;
                        break;
                    }
                case CursorType.Copy:
                    {
                        m_WebViewPanel.Cursor = Cursors.Arrow;
                        break;
                    }
                case CursorType.None:
                    {
                        m_WebViewPanel.Cursor = null;
                        break;
                    }
                case CursorType.NotAllowed:
                    {
                        m_WebViewPanel.Cursor = Cursors.No;
                        break;
                    }
                case CursorType.ZoomIn:
                    {
                        m_WebViewPanel.Cursor = Cursors.Arrow;
                        break;
                    }
                case CursorType.ZoomOut:
                    {
                        m_WebViewPanel.Cursor = Cursors.Arrow;
                        break;
                    }
                case CursorType.Grab:
                    {
                        m_WebViewPanel.Cursor = Cursors.Arrow;
                        break;
                    }
                case CursorType.Grabbing:
                    {
                        m_WebViewPanel.Cursor = Cursors.Arrow;
                        break;
                    }
                case CursorType.Custom:
                    {
                        m_WebViewPanel.Cursor = Cursors.Arrow;
                        break;
                    }
            }
        }


        Modifiers GetModifiers()
        {
            Modifiers m = 0;

            if (InputHandler.IsShiftDown)
            {
                m |= Modifiers.ShiftKey;
            }
            
            if (InputHandler.IsControlDown)
            {
                m |= Modifiers.ControlKey;
            }
            
            if (InputHandler.IsAltDown)
            {
                m |= Modifiers.AltKey;
            }

            //m |= Modifiers.MetaKey;
            //m |= Modifiers.IsKeypad;
            //m |= Modifiers.IsAutorepeat;

            return m;
        }


        Dictionary<Keys, VirtualKey> m_TranslateKeyCodeDict;
        VirtualKey TranslateKeyCode(Keys key)
        {
            if (m_TranslateKeyCodeDict == null)
            {
                m_TranslateKeyCodeDict = new Dictionary<Keys, VirtualKey>();


                //     Unknown.
                m_TranslateKeyCodeDict.Add(Keys.Unknown, VirtualKey.UNKNOWN);
                //     AK_BACK (08) BACKSPACE key
                m_TranslateKeyCodeDict.Add(Keys.Back, VirtualKey.BACK);
                //     AK_TAB (09) TAB key.
                m_TranslateKeyCodeDict.Add(Keys.Tab, VirtualKey.TAB);
                //     AK_CLEAR (0C) CLEAR key.
                m_TranslateKeyCodeDict.Add(Keys.Clear, VirtualKey.CLEAR);
                //     AK_RETURN (0D).
                m_TranslateKeyCodeDict.Add(Keys.Return, VirtualKey.RETURN);
                //     AK_SHIFT (10) SHIFT key.
                m_TranslateKeyCodeDict.Add(Keys.Shift, VirtualKey.SHIFT);
                //     AK_CONTROL (11) CTRL key.
                m_TranslateKeyCodeDict.Add(Keys.Control, VirtualKey.CONTROL);
                //     AK_MENU (12) ALT key.
                m_TranslateKeyCodeDict.Add(Keys.Menu, VirtualKey.MENU);
                //     AK_PAUSE (13) PAUSE key.
                m_TranslateKeyCodeDict.Add(Keys.Pause, VirtualKey.PAUSE);
                //     AK_CAPITAL (14) CAPS LOCK key.
                m_TranslateKeyCodeDict.Add(Keys.Capital, VirtualKey.CAPITAL);
                //     AK_HANGUEL (15) IME Hanguel mode (maintained for compatibility, use AK_HANGUL).
                //      AK_HANGUL (15) IME Hangul mode.
                m_TranslateKeyCodeDict.Add(Keys.HangulMode, VirtualKey.HANGUL);
                //     AK_KANA (15) Input Method Editor (IME) Kana mode.
                //DUB   m_TranslateKeyCodeDict.Add(Keys.KanaMode, VirtualKey.KANA);
                //     AK_JUNJA (17) IME Junja mode.
                m_TranslateKeyCodeDict.Add(Keys.JunjaMode, VirtualKey.JUNJA);
                //     .
                m_TranslateKeyCodeDict.Add(Keys.FinalMode, VirtualKey.FINAL);
                //     AK_KANJI (19) IME Kanji mode.
                m_TranslateKeyCodeDict.Add(Keys.KanjiMode, VirtualKey.KANJI);
                //     AK_HANJA (19) IME Hanja mode.
                //DUB   m_TranslateKeyCodeDict.Add(Keys.HanjaMode, VirtualKey.HANJA);
                //     AK_ESCAPE (1B) ESC key.
                m_TranslateKeyCodeDict.Add(Keys.Escape, VirtualKey.ESCAPE);                
                //     AK_CONVERT (1C) IME convert.
                m_TranslateKeyCodeDict.Add(Keys.IMEConvert, VirtualKey.CONVERT);                
                //     AK_NONCONVERT (1D) IME nonconvert.
                m_TranslateKeyCodeDict.Add(Keys.IMENonconvert, VirtualKey.NONCONVERT);                
                //     AK_ACCEPT (1E) IME accept.
                m_TranslateKeyCodeDict.Add(Keys.IMEAccept, VirtualKey.ACCEPT);
                //     AK_MODECHANGE (1F) IME mode change request.
                m_TranslateKeyCodeDict.Add(Keys.IMEModeChange, VirtualKey.MODECHANGE);
                //     AK_SPACE (20) SPACEBAR.                
                m_TranslateKeyCodeDict.Add(Keys.Space, VirtualKey.SPACE);
                //     AK_PRIOR (21) PAGE UP key.
                m_TranslateKeyCodeDict.Add(Keys.Prior, VirtualKey.PRIOR);
                //     AK_NEXT (22) PAGE DOWN key.
                m_TranslateKeyCodeDict.Add(Keys.Next, VirtualKey.NEXT);
                //     AK_END (23) END key.
                m_TranslateKeyCodeDict.Add(Keys.End, VirtualKey.END);
                //     AK_HOME (24) HOME key.
                m_TranslateKeyCodeDict.Add(Keys.Home, VirtualKey.HOME);
                //     AK_LEFT (25) LEFT ARROW key.
                m_TranslateKeyCodeDict.Add(Keys.Left, VirtualKey.LEFT);
                //     AK_UP (26) UP ARROW key.
                m_TranslateKeyCodeDict.Add(Keys.Up, VirtualKey.UP);
                //     AK_RIGHT (27) RIGHT ARROW key.
                m_TranslateKeyCodeDict.Add(Keys.Right, VirtualKey.RIGHT);
                //     AK_DOWN (28) DOWN ARROW key.
                m_TranslateKeyCodeDict.Add(Keys.Down, VirtualKey.DOWN);
                //     AK_SELECT (29) SELECT key.
                m_TranslateKeyCodeDict.Add(Keys.Select, VirtualKey.SELECT);
                //     AK_PRINT (2A) PRINT key.
                m_TranslateKeyCodeDict.Add(Keys.Print, VirtualKey.PRINT);
                //     AK_EXECUTE (2B) EXECUTE key.
                m_TranslateKeyCodeDict.Add(Keys.Execute, VirtualKey.EXECUTE);
                //     AK_SNAPSHOT (2C) PRINT SCREEN key.
                m_TranslateKeyCodeDict.Add(Keys.Snapshot, VirtualKey.SNAPSHOT);
                //     AK_INSERT (2D) INS key.
                m_TranslateKeyCodeDict.Add(Keys.Insert, VirtualKey.INSERT);
                //     AK_DELETE (2E) DEL key.
                m_TranslateKeyCodeDict.Add(Keys.Delete, VirtualKey.DELETE);
                //     AK_HELP (2F) HELP key.
                m_TranslateKeyCodeDict.Add(Keys.Help, VirtualKey.HELP);
                //     (30) 0 key.
                m_TranslateKeyCodeDict.Add(Keys.D0, VirtualKey.NUM_0);
                //     (31) 1 key.
                m_TranslateKeyCodeDict.Add(Keys.D1, VirtualKey.NUM_1);
                //     (32) 2 key.
                m_TranslateKeyCodeDict.Add(Keys.D2, VirtualKey.NUM_2);
                //     (33) 3 key.
                m_TranslateKeyCodeDict.Add(Keys.D3, VirtualKey.NUM_3);
                //     (34) 4 key.
                m_TranslateKeyCodeDict.Add(Keys.D4, VirtualKey.NUM_4);
                //     (35) 5 key,.
                m_TranslateKeyCodeDict.Add(Keys.D5, VirtualKey.NUM_5);
                //     (36) 6 key.
                m_TranslateKeyCodeDict.Add(Keys.D6, VirtualKey.NUM_6);
                //     (37) 7 key.
                m_TranslateKeyCodeDict.Add(Keys.D7, VirtualKey.NUM_7);
                //     (38) 8 key.
                m_TranslateKeyCodeDict.Add(Keys.D8, VirtualKey.NUM_8);
                //     (39) 9 key.
                m_TranslateKeyCodeDict.Add(Keys.D9, VirtualKey.NUM_9);
                //     (41) A key.
                m_TranslateKeyCodeDict.Add(Keys.A, VirtualKey.A);
                //     (42) B key.
                m_TranslateKeyCodeDict.Add(Keys.B, VirtualKey.B);
                //     (43) C key.
                m_TranslateKeyCodeDict.Add(Keys.C, VirtualKey.C);
                //     (44) D key.
                m_TranslateKeyCodeDict.Add(Keys.D, VirtualKey.D);
                //     (45) E key.
                m_TranslateKeyCodeDict.Add(Keys.E, VirtualKey.E);
                //     (46) F key.
                m_TranslateKeyCodeDict.Add(Keys.F, VirtualKey.F);
                //     (47) G key.
                m_TranslateKeyCodeDict.Add(Keys.G, VirtualKey.G);
                //     (48) H key.
                m_TranslateKeyCodeDict.Add(Keys.H, VirtualKey.H);
                //     (49) I key.
                m_TranslateKeyCodeDict.Add(Keys.I, VirtualKey.I);
                //     (4A) J key.
                m_TranslateKeyCodeDict.Add(Keys.J, VirtualKey.J);
                //     (4B) K key.
                m_TranslateKeyCodeDict.Add(Keys.K, VirtualKey.K);
                //     (4C) L key.
                m_TranslateKeyCodeDict.Add(Keys.L, VirtualKey.L);
                //     (4D) M key.
                m_TranslateKeyCodeDict.Add(Keys.M, VirtualKey.M);
                //     (4E) N key.
                m_TranslateKeyCodeDict.Add(Keys.N, VirtualKey.N);
                //     (4F) O key.
                m_TranslateKeyCodeDict.Add(Keys.O, VirtualKey.O);
                //     (50) P key.
                m_TranslateKeyCodeDict.Add(Keys.P, VirtualKey.P);
                //     (51) Q key.
                m_TranslateKeyCodeDict.Add(Keys.Q, VirtualKey.Q);
                //     (52) R key.
                m_TranslateKeyCodeDict.Add(Keys.R, VirtualKey.R);
                //     (53) S key.
                m_TranslateKeyCodeDict.Add(Keys.S, VirtualKey.S);
                //     (54) T key.
                m_TranslateKeyCodeDict.Add(Keys.T, VirtualKey.T);
                //     (55) U key.
                m_TranslateKeyCodeDict.Add(Keys.U, VirtualKey.U);
                //     (56) V key.
                m_TranslateKeyCodeDict.Add(Keys.V, VirtualKey.V);
                //     (57) W key.
                m_TranslateKeyCodeDict.Add(Keys.W, VirtualKey.W);
                //     (58) X key.
                m_TranslateKeyCodeDict.Add(Keys.X, VirtualKey.X);
                //     (59) Y key.
                m_TranslateKeyCodeDict.Add(Keys.Y, VirtualKey.Y);
                //     (5A) Z key.
                m_TranslateKeyCodeDict.Add(Keys.Z, VirtualKey.Z);
                //     AK_LWIN (5B) Left Windows key (Microsoft Natural keyboard).
                m_TranslateKeyCodeDict.Add(Keys.LWin, VirtualKey.LWIN);
                //     AK_RWIN (5C) Right Windows key (Natural keyboard).
                m_TranslateKeyCodeDict.Add(Keys.RWin, VirtualKey.RWIN);
                //     AK_APPS (5D) Applications key (Natural keyboard).
                m_TranslateKeyCodeDict.Add(Keys.Apps, VirtualKey.APPS);
                //     AK_SLEEP (5F) Computer Sleep key.
                m_TranslateKeyCodeDict.Add(Keys.Sleep, VirtualKey.SLEEP);
                //     AK_NUMPAD0 (60) Numeric keypad 0 key.
                m_TranslateKeyCodeDict.Add(Keys.NumPad0, VirtualKey.NUMPAD0);
                //     AK_NUMPAD1 (61) Numeric keypad 1 key.
                m_TranslateKeyCodeDict.Add(Keys.NumPad1, VirtualKey.NUMPAD1);
                //     AK_NUMPAD2 (62) Numeric keypad 2 key.
                m_TranslateKeyCodeDict.Add(Keys.NumPad2, VirtualKey.NUMPAD2);
                //     AK_NUMPAD3 (63) Numeric keypad 3 key.
                m_TranslateKeyCodeDict.Add(Keys.NumPad3, VirtualKey.NUMPAD3);
                //     AK_NUMPAD4 (64) Numeric keypad 4 key.
                m_TranslateKeyCodeDict.Add(Keys.NumPad4, VirtualKey.NUMPAD4);
                //     AK_NUMPAD5 (65) Numeric keypad 5 key.
                m_TranslateKeyCodeDict.Add(Keys.NumPad5, VirtualKey.NUMPAD5);
                //     AK_NUMPAD6 (66) Numeric keypad 6 key.
                m_TranslateKeyCodeDict.Add(Keys.NumPad6, VirtualKey.NUMPAD6);
                //     AK_NUMPAD7 (67) Numeric keypad 7 key.
                m_TranslateKeyCodeDict.Add(Keys.NumPad7, VirtualKey.NUMPAD7);
                //     AK_NUMPAD8 (68) Numeric keypad 8 key.
                m_TranslateKeyCodeDict.Add(Keys.NumPad8, VirtualKey.NUMPAD8);
                //     AK_NUMPAD9 (69) Numeric keypad 9 key.
                m_TranslateKeyCodeDict.Add(Keys.NumPad9, VirtualKey.NUMPAD9);
                //     AK_MULTIPLY (6A) Multiply key.
                m_TranslateKeyCodeDict.Add(Keys.Multiply, VirtualKey.MULTIPLY);
                //     AK_ADD (6B) Add key.
                m_TranslateKeyCodeDict.Add(Keys.Add, VirtualKey.ADD);
                //     AK_SEPARATOR (6C) Separator key.
                m_TranslateKeyCodeDict.Add(Keys.Separator, VirtualKey.SEPARATOR);
                //     AK_SUBTRACT (6D) Subtract key.
                m_TranslateKeyCodeDict.Add(Keys.Subtract, VirtualKey.SUBTRACT);
                //     AK_DECIMAL (6E) Decimal key.
                m_TranslateKeyCodeDict.Add(Keys.Decimal, VirtualKey.DECIMAL);
                //     AK_DIVIDE (6F) Divide key.
                m_TranslateKeyCodeDict.Add(Keys.Divide, VirtualKey.DIVIDE);
                //     AK_F1 (70) F1 key.
                m_TranslateKeyCodeDict.Add(Keys.F1, VirtualKey.F1);
                //     AK_F2 (71) F2 key.
                m_TranslateKeyCodeDict.Add(Keys.F2, VirtualKey.F2);
                //     AK_F3 (72) F3 key.
                m_TranslateKeyCodeDict.Add(Keys.F3, VirtualKey.F3);
                //     AK_F4 (73) F4 key.
                m_TranslateKeyCodeDict.Add(Keys.F4, VirtualKey.F4);
                //     AK_F5 (74) F5 key.
                m_TranslateKeyCodeDict.Add(Keys.F5, VirtualKey.F5);
                //     AK_F6 (75) F6 key.
                m_TranslateKeyCodeDict.Add(Keys.F6, VirtualKey.F6);
                //     AK_F7 (76) F7 key.
                m_TranslateKeyCodeDict.Add(Keys.F7, VirtualKey.F7);
                //     AK_F8 (77) F8 key.
                m_TranslateKeyCodeDict.Add(Keys.F8, VirtualKey.F8);
                //     AK_F9 (78) F9 key.
                m_TranslateKeyCodeDict.Add(Keys.F9, VirtualKey.F9);
                //     AK_F10 (79) F10 key.
                m_TranslateKeyCodeDict.Add(Keys.F10, VirtualKey.F10);
                //     AK_F11 (7A) F11 key.
                m_TranslateKeyCodeDict.Add(Keys.F11, VirtualKey.F11);
                //     AK_F12 (7B) F12 key.
                m_TranslateKeyCodeDict.Add(Keys.F12, VirtualKey.F12);
                //     AK_F13 (7C) F13 key.
                m_TranslateKeyCodeDict.Add(Keys.F13, VirtualKey.F13);
                //     AK_F14 (7D) F14 key.
                m_TranslateKeyCodeDict.Add(Keys.F14, VirtualKey.F14);
                //     AK_F15 (7E) F15 key.
                m_TranslateKeyCodeDict.Add(Keys.F15, VirtualKey.F15);
                //     AK_F16 (7F) F16 key.
                m_TranslateKeyCodeDict.Add(Keys.F16, VirtualKey.F16);
                //     AK_F17 (80H) F17 key.
                m_TranslateKeyCodeDict.Add(Keys.F17, VirtualKey.F17);
                //     AK_F18 (81H) F18 key.
                m_TranslateKeyCodeDict.Add(Keys.F18, VirtualKey.F18);
                //     AK_F19 (82H) F19 key.
                m_TranslateKeyCodeDict.Add(Keys.F19, VirtualKey.F19);
                //     AK_F20 (83H) F20 key.
                m_TranslateKeyCodeDict.Add(Keys.F20, VirtualKey.F20);
                //     AK_F21 (84H) F21 key.
                m_TranslateKeyCodeDict.Add(Keys.F21, VirtualKey.F21);
                //     AK_F22 (85H) F22 key.
                m_TranslateKeyCodeDict.Add(Keys.F22, VirtualKey.F22);
                //     AK_F23 (86H) F23 key.
                m_TranslateKeyCodeDict.Add(Keys.F23, VirtualKey.F23);
                //     AK_F24 (87H) F24 key.
                m_TranslateKeyCodeDict.Add(Keys.F24, VirtualKey.F24);
                //     AK_NUMLOCK (90) NUM LOCK key.
                m_TranslateKeyCodeDict.Add(Keys.NumLock, VirtualKey.NUMLOCK);
                //     AK_SCROLL (91) SCROLL LOCK key.
                m_TranslateKeyCodeDict.Add(Keys.Scroll, VirtualKey.SCROLL);
                //     AK_LSHIFT (A0) Left SHIFT key.
                m_TranslateKeyCodeDict.Add(Keys.LShiftKey, VirtualKey.LSHIFT);
                //     AK_RSHIFT (A1) Right SHIFT key.
                m_TranslateKeyCodeDict.Add(Keys.RShiftKey, VirtualKey.RSHIFT);
                //     AK_LCONTROL (A2) Left CONTROL key.
                m_TranslateKeyCodeDict.Add(Keys.LControlKey, VirtualKey.LCONTROL);
                //     AK_RCONTROL (A3) Right CONTROL key.
                m_TranslateKeyCodeDict.Add(Keys.RControlKey, VirtualKey.RCONTROL);
                //     AK_LMENU (A4) Left MENU key.
                m_TranslateKeyCodeDict.Add(Keys.LMenu, VirtualKey.LMENU);
                //     AK_RMENU (A5) Right MENU key.
                m_TranslateKeyCodeDict.Add(Keys.RMenu, VirtualKey.RMENU);
                //     AK_BROWSER_BACK (A6) Windows 2000/XP: Browser Back key.
                m_TranslateKeyCodeDict.Add(Keys.BrowserBack, VirtualKey.BROWSER_BACK);
                //     AK_BROWSER_FORWARD (A7) Windows 2000/XP: Browser Forward key.
                m_TranslateKeyCodeDict.Add(Keys.BrowserForward, VirtualKey.BROWSER_FORWARD);
                //     AK_BROWSER_REFRESH (A8) Windows 2000/XP: Browser Refresh key.
                m_TranslateKeyCodeDict.Add(Keys.BrowserRefresh, VirtualKey.BROWSER_REFRESH);
                //     AK_BROWSER_STOP (A9) Windows 2000/XP: Browser Stop key.
                m_TranslateKeyCodeDict.Add(Keys.BrowserStop, VirtualKey.BROWSER_STOP);
                //     AK_BROWSER_SEARCH (AA) Windows 2000/XP: Browser Search key.
                m_TranslateKeyCodeDict.Add(Keys.BrowserSearch, VirtualKey.BROWSER_SEARCH);
                //     AK_BROWSER_FAVORITES (AB) Windows 2000/XP: Browser Favorites key.
                m_TranslateKeyCodeDict.Add(Keys.BrowserFavorites, VirtualKey.BROWSER_FAVORITES);
                //     AK_BROWSER_HOME (AC) Windows 2000/XP: Browser Start and Home key.
                m_TranslateKeyCodeDict.Add(Keys.BrowserHome, VirtualKey.BROWSER_HOME);
                //     AK_VOLUME_MUTE (AD) Windows 2000/XP: Volume Mute key.
                m_TranslateKeyCodeDict.Add(Keys.VolumeMute, VirtualKey.VOLUME_MUTE);
                //     AK_VOLUME_DOWN (AE) Windows 2000/XP: Volume Down key.
                m_TranslateKeyCodeDict.Add(Keys.VolumeDown, VirtualKey.VOLUME_DOWN);
                //     .
                m_TranslateKeyCodeDict.Add(Keys.VolumeUp, VirtualKey.VOLUME_UP);
                //     AK_MEDIA_NEXT_TRACK (B0) Windows 2000/XP: Next Track key.
                m_TranslateKeyCodeDict.Add(Keys.MediaNextTrack, VirtualKey.MEDIA_NEXT_TRACK);
                //     AK_MEDIA_PREV_TRACK (B1) Windows 2000/XP: Previous Track key.
                m_TranslateKeyCodeDict.Add(Keys.MediaPreviousTrack, VirtualKey.MEDIA_PREV_TRACK);
                //     AK_MEDIA_STOP (B2) Windows 2000/XP: Stop Media key.
                m_TranslateKeyCodeDict.Add(Keys.MediaStop, VirtualKey.MEDIA_STOP);
                //     AK_MEDIA_PLAY_PAUSE (B3) Windows 2000/XP: Play/Pause Media key.
                m_TranslateKeyCodeDict.Add(Keys.MediaPlayPause, VirtualKey.MEDIA_PLAY_PAUSE);
                //     AK_LAUNCH_MAIL (B4) Windows 2000/XP: Start Mail key.
                m_TranslateKeyCodeDict.Add(Keys.LaunchMail, VirtualKey.MEDIA_LAUNCH_MAIL);
                //     AK_LAUNCH_MEDIA_SELECT (B5) Windows 2000/XP: Select Media key.
                m_TranslateKeyCodeDict.Add(Keys.SelectMedia, VirtualKey.MEDIA_LAUNCH_MEDIA_SELECT);
                //     AK_LAUNCH_APP1 (B6) Windows 2000/XP: Start Application 1 key.
                m_TranslateKeyCodeDict.Add(Keys.LaunchApplication1, VirtualKey.MEDIA_LAUNCH_APP1);
                //     AK_LAUNCH_APP2 (B7) Windows 2000/XP: Start Application 2 key.
                m_TranslateKeyCodeDict.Add(Keys.LaunchApplication2, VirtualKey.MEDIA_LAUNCH_APP2);
                //     AK_OEM_1 (BA) Used for miscellaneous characters, it can vary by keyboard.
                //     Windows 2000/XP: For the US standard keyboard, the ',:' key.
                m_TranslateKeyCodeDict.Add(Keys.Oem1, VirtualKey.OEM_1);
                //     AK_OEM_PLUS (BB) Windows 2000/XP: For any country/region, the '+' key.
                m_TranslateKeyCodeDict.Add(Keys.OemPlus, VirtualKey.OEM_PLUS);
                //     AK_OEM_COMMA (BC) Windows 2000/XP: For any country/region, the ',' key.
                m_TranslateKeyCodeDict.Add(Keys.OemComma, VirtualKey.OEM_COMMA);
                //     AK_OEM_MINUS (BD) Windows 2000/XP: For any country/region, the '-' key.
                m_TranslateKeyCodeDict.Add(Keys.OemMinus, VirtualKey.OEM_MINUS);
                //     AK_OEM_PERIOD (BE) Windows 2000/XP: For any country/region, the '.' key.
                m_TranslateKeyCodeDict.Add(Keys.OemPeriod, VirtualKey.OEM_PERIOD);
                //     AK_OEM_2 (BF) Used for miscellaneous characters, it can vary by keyboard.
                //     Windows 2000/XP: For the US standard keyboard, the '/?' key.
                m_TranslateKeyCodeDict.Add(Keys.Oem2, VirtualKey.OEM_2);
                //     AK_OEM_3 (C0) Used for miscellaneous characters, it can vary by keyboard.
                //     Windows 2000/XP: For the US standard keyboard, the '`~' key.
                m_TranslateKeyCodeDict.Add(Keys.Oem3, VirtualKey.OEM_3);
                //     AK_OEM_4 (DB) Used for miscellaneous characters, it can vary by keyboard.
                //     Windows 2000/XP: For the US standard keyboard, the '[{' key.
                m_TranslateKeyCodeDict.Add(Keys.Oem4, VirtualKey.OEM_4);
                //     AK_OEM_5 (DC) Used for miscellaneous characters, it can vary by keyboard.
                //     Windows 2000/XP: For the US standard keyboard, the '\|' key.
                m_TranslateKeyCodeDict.Add(Keys.Oem5, VirtualKey.OEM_5);
                //     AK_OEM_6 (DD) Used for miscellaneous characters, it can vary by keyboard.
                //     Windows 2000/XP: For the US standard keyboard, the ']}' key.
                m_TranslateKeyCodeDict.Add(Keys.Oem6, VirtualKey.OEM_6);
                //     AK_OEM_7 (DE) Used for miscellaneous characters, it can vary by keyboard.
                //     Windows 2000/XP: For the US standard keyboard, the 'single-quote/double-quote'
                //     key.
                m_TranslateKeyCodeDict.Add(Keys.Oem7, VirtualKey.OEM_7);
                //     AK_OEM_8 (DF) Used for miscellaneous characters, it can vary by keyboard.
                m_TranslateKeyCodeDict.Add(Keys.Oem8, VirtualKey.OEM_8);
                //     AK_OEM_102 (E2) Windows 2000/XP: Either the angle bracket key or the backslash
                //     key on the RT 102-key keyboard.
                m_TranslateKeyCodeDict.Add(Keys.Oem102, VirtualKey.OEM_102);
                //     AK_PROCESSKEY (E5) Windows 95/98/Me, Windows NT 4.0, Windows 2000/XP: IME
                //     PROCESS key.
                m_TranslateKeyCodeDict.Add(Keys.ProcessKey, VirtualKey.PROCESSKEY);
                //     AK_PACKET (E7) Windows 2000/XP: Used to pass Unicode characters as if they
                //     were keystrokes. The AK_PACKET key is the low word of a 32-bit Virtual Key
                //     value used for non-keyboard input methods. For more information, see Remark
                //     in KEYBDINPUT,SendInput, WM_KEYDOWN, and WM_KEYUP.
                m_TranslateKeyCodeDict.Add(Keys.Packet, VirtualKey.PACKET);
                //     AK_ATTN (F6) Attn key.
                m_TranslateKeyCodeDict.Add(Keys.Attn, VirtualKey.ATTN);
                //     AK_CRSEL (F7) CrSel key.
                m_TranslateKeyCodeDict.Add(Keys.Crsel, VirtualKey.CRSEL);
                //     AK_EXSEL (F8) ExSel key.
                m_TranslateKeyCodeDict.Add(Keys.Exsel, VirtualKey.EXSEL);
                //     AK_EREOF (F9) Erase EOF key.
                m_TranslateKeyCodeDict.Add(Keys.EraseEof, VirtualKey.EREOF);
                //     AK_PLAY (FA) Play key.
                m_TranslateKeyCodeDict.Add(Keys.Play, VirtualKey.PLAY);
                //     AK_ZOOM (FB) Zoom key.
                m_TranslateKeyCodeDict.Add(Keys.Zoom, VirtualKey.ZOOM);
                //     AK_NONAME (FC) Reserved for future use.
                m_TranslateKeyCodeDict.Add(Keys.NoName, VirtualKey.NONAME);
                //     AK_PA1 (FD) PA1 key.
                m_TranslateKeyCodeDict.Add(Keys.Pa1, VirtualKey.PA1);
                //     AK_OEM_CLEAR (FE) Clear key.
                m_TranslateKeyCodeDict.Add(Keys.OemClear, VirtualKey.OEM_CLEAR);
            }

            VirtualKey vkey;
            if (m_TranslateKeyCodeDict.TryGetValue(key, out vkey))
            {
                return vkey;
            }

            return (VirtualKey)key;
        }
#endif
    }
}
