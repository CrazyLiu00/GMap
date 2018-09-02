//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;

using Alt.FarseerPhysics;
using Alt.FarseerPhysics.Common;
using Alt.GUI.FarseerPhysics.Demo.Testbed;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Tests;


namespace Alt.GUI.Demo
{
    class Example_FarseerPhysics : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                return base.LogoRightTopOffset + new SizeI(0, 38);
            }
        }


        FarseerPhysicsContainer m_FarseerPhysicsContainer;


        public Example_FarseerPhysics(Base parent)
            : base(parent)
        {
            m_FarseerPhysicsContainer = new FarseerPhysicsContainer(this);
            m_FarseerPhysicsContainer.Dock = Pos.Fill;
            m_FarseerPhysicsContainer.Focus();
        }

        protected override void Render(Alt.GUI.Temporary.Gwen.Skin.Base skin)
        {
            m_FarseerPhysicsContainer.DoubleBuffered = !skin.Renderer.Graphics.IsClippingSupported;
            if (m_FarseerPhysicsContainer.DoubleBuffered)
            {
                m_FarseerPhysicsContainer.Refresh();
            }

            base.Render(skin);
        }
        
        protected internal override void OnActivate()
        {
            base.OnActivate();

            if (m_FarseerPhysicsContainer != null)
            {
                m_FarseerPhysicsContainer.Focus();
            }
        }


        class FarseerPhysicsContainer : DoubleBufferedControl
        {
            public FarseerPhysicsContainer(Base parent) :
                base(parent)
            {
                DoubleBuffered = false;
                KeyboardInputEnabled = true;
            }


            int testIndex = 0;
            int testSelection = 0;
            int testCount = 0;
            FarseerPhysicsTestEntry entry;
            FarseerPhysicsTest test;
            FarseerPhysicsGameSettings settings;
            double viewZoom = 1.0;
            bool rMouseDown;
            Point lastp;

            TPSCounter m_TPSCounter = new TPSCounter();

            Font m_InfoFont;
            Base m_PanelTop;
			Alt.GUI.Temporary.Gwen.Control.Label m_ExampleNumberLabel;


            protected override void OnLoad(EventArgs e)
            {
                base.OnLoad(e);


                Cursor = GUI.Cursors.Hand;

                //  GUI
                {
                    m_PanelTop = new Base(this);
                    m_PanelTop.Dock = Pos.Top;
                    m_PanelTop.ClientBackColor = Color.FromArgb(96, Color.Black);
                    m_PanelTop.Height = 38;

                    Base controlsMain = new Base(m_PanelTop);
                    controlsMain.Dock = Pos.Fill;

					Alt.GUI.Temporary.Gwen.Control.Button backButton = new Alt.GUI.Temporary.Gwen.Control.Button(controlsMain);
                    backButton.Dock = Pos.Left;
                    backButton.Click += new EventHandler(BackButton_Click);
                    backButton.SetToolTipText("Prev");
                    backButton.SetImage("AltData/Back.png", true);
                    backButton.Width = 36;
                    backButton.Margin = new Margin(1, 1, 0, 1);

					Alt.GUI.Temporary.Gwen.Control.Button forwardButton = new Alt.GUI.Temporary.Gwen.Control.Button(controlsMain);
                    forwardButton.Dock = Pos.Left;
                    forwardButton.Click += new EventHandler(ForwardButton_Click);
                    forwardButton.SetToolTipText("Next");
                    forwardButton.SetImage("AltData/Forward.png", true);
                    forwardButton.Width = 36;
                    forwardButton.Margin = new Margin(1, 1, 0, 1);


					m_ExampleNumberLabel = new Alt.GUI.Temporary.Gwen.Control.Label(controlsMain);
                    m_ExampleNumberLabel.Dock = Pos.Left;
                    m_ExampleNumberLabel.AutoSizeToContents = true;
                    m_ExampleNumberLabel.TextColor = Color.White;
                    m_ExampleNumberLabel.Text = "";
                    m_ExampleNumberLabel.Margin = new Margin(5, (m_PanelTop.Height - m_ExampleNumberLabel.Height) / 2 - 3, 5, 1);


					Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(controlsMain);
                    label.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
                    label.AutoSizeToContents = true;
                    label.Text = "Use mouse device to operate with dynamic objects\nand scene zoom / offset";
                    label.TextColor = Color.Cyan;
                    label.Margin = new Alt.GUI.Temporary.Gwen.Margin(5, //(m_PanelTop.Height - label.Height) / 2 -
                        3, 5, 0);


					Alt.GUI.Temporary.Gwen.Control.Button refreshButton = new Alt.GUI.Temporary.Gwen.Control.Button(controlsMain);
                    refreshButton.Dock = Pos.Right;
                    refreshButton.Click += new EventHandler(RestartButton_Click);
                    refreshButton.SetToolTipText("Restart");
                    refreshButton.SetImage("AltData/Refresh.png", true);
                    refreshButton.Width = 36;
                    refreshButton.Margin = new Margin(0, 1, 0, 1);
                }


                settings = new FarseerPhysicsGameSettings();

                testCount = 0;
                while (FarseerPhysicsTestEntries.TestList[testCount].CreateTest != null)
                {
                    testCount++;
                }

                testIndex = -1;
                testSelection = Alt.Box2D.MathUtils.Clamp(testIndex, 0, testCount - 1);


                m_InfoFont = new Font("Arial", 10.01, FontStyle.Bold);


                Focus();
            }


            void BackButton_Click(object sender, EventArgs e)
            {
                testSelection--;
                if (testSelection < 0)
                {
                    testSelection = testCount - 1;
                }
            }

            void ForwardButton_Click(object sender, EventArgs e)
            {
                testSelection++;
                if (testSelection == testCount)
                {
                    testSelection = 0;
                }
            }

            void RestartButton_Click(object sender, EventArgs e)
            {
                Restart();
            }


            void StartTest(int index)
            {
                entry = FarseerPhysicsTestEntries.TestList[index];
                test = entry.CreateTest();
                test.Initialize();
            }


            protected override void OnKeyDown(GUI.KeyEventArgs e)
            {
                base.OnKeyDown(e);


                // Press 'z' to zoom out.
                if (e.KeyCode == GUI.Keys.Z)
                {
                    viewZoom = Math.Min(1.1 * viewZoom, 20.0);
                }
                // Press 'x' to zoom in.
                else if (e.KeyCode == GUI.Keys.X)
                {
                    viewZoom = Math.Max(0.9 * viewZoom, 0.02);
                }
                // Press 'r' to reset.
                else if (e.KeyCode == GUI.Keys.R)
                {
                    Restart();
                }
                else if (e.KeyCode == GUI.Keys.P)
                // || newGamePad.IsButtonDown(Buttons.Start) && oldGamePad.IsButtonUp(Buttons.Start))
                {
                    settings.Pause = !settings.Pause;
                }
                // Press [ to prev test.
                else if (e.KeyCode == GUI.Keys.OemOpenBrackets)
                // || newGamePad.IsButtonDown(Buttons.LeftShoulder) && oldGamePad.IsButtonUp(Buttons.LeftShoulder))
                {
                    testSelection--;
                    if (testSelection < 0)
                    {
                        testSelection = testCount - 1;
                    }
                }
                // Press ] to next test.
                else if (e.KeyCode == GUI.Keys.OemCloseBrackets)
                // || newGamePad.IsButtonDown(Buttons.RightShoulder) && oldGamePad.IsButtonUp(Buttons.RightShoulder))
                {
                    testSelection++;
                    if (testSelection == testCount)
                    {
                        testSelection = 0;
                    }
                }
                // Press left to pan left.
                else if (e.KeyCode == GUI.Keys.Left)
                {
                    test.m_OffsetX -= 0.5;
                }
                // Press right to pan right.
                else if (e.KeyCode == GUI.Keys.Right)
                {
                    test.m_OffsetX += 0.5;
                }
                // Press down to pan down.
                else if (e.KeyCode == GUI.Keys.Down)
                {
                    test.m_OffsetY += 0.5;
                }
                // Press up to pan up.
                else if (e.KeyCode == GUI.Keys.Up)
                {
                    test.m_OffsetY -= 0.5;
                }
                // Press home to reset the view.
                else if (e.KeyCode == GUI.Keys.Home)
                {
                    ResetView();
                }
                else if (e.KeyCode == GUI.Keys.F1)
                {
                    EnableOrDisableFlag(DebugViewFlags.Shape);
                }
                else if (e.KeyCode == GUI.Keys.F2)
                {
                    EnableOrDisableFlag(DebugViewFlags.DebugPanel);
                }
                else if (e.KeyCode == GUI.Keys.F3)
                {
                    EnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
                }
                else if (e.KeyCode == GUI.Keys.F4)
                {
                    EnableOrDisableFlag(DebugViewFlags.AABB);
                }
                else if (e.KeyCode == GUI.Keys.F5)
                {
                    EnableOrDisableFlag(DebugViewFlags.CenterOfMass);
                }
                else if (e.KeyCode == GUI.Keys.F6)
                {
                    EnableOrDisableFlag(DebugViewFlags.Joint);
                }
                else if (e.KeyCode == GUI.Keys.F7)
                {
                    EnableOrDisableFlag(DebugViewFlags.ContactPoints);
                    EnableOrDisableFlag(DebugViewFlags.ContactNormals);
                }
                else if (e.KeyCode == GUI.Keys.F8)
                {
                    EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
                }
                else if (e.KeyCode == GUI.Keys.F9)
                {
                    EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
                }
                else
                {
                    if (test != null)
                    {
                        test.InjectKeyDown(e);
                    }
                }
            }

            protected override void OnKeyUp(GUI.KeyEventArgs e)
            {
                base.OnKeyUp(e);

                if (test != null)
                {
                    test.InjectKeyUp(e);
                }
            }


            void EnableOrDisableFlag(DebugViewFlags flag)
            {
                if (test != null)
                {
                    if ((test.DebugView.Flags & flag) == flag)
                    {
                        test.DebugView.RemoveFlags(flag);
                    }
                    else
                    {
                        test.DebugView.AppendFlags(flag);
                    }
                }
            }


            protected override void OnPaint(GUI.PaintEventArgs e)
            {
                base.OnPaint(e);

                if (testSelection != testIndex)
                {
                    testIndex = testSelection;

                    StartTest(testIndex);
                    ResetView();

                    m_ExampleNumberLabel.Text = "Ex " + (testIndex + 1).ToString("0") + "/" + testCount.ToString("0");
                }


                Graphics graphics = e.Graphics;
                if (!DoubleBuffered)
                {
                    graphics.FillRectangle(Color.FromArgb((graphics is SoftwareGraphics) ? 96 : 192, Color.Black), ClientRectangle);
                }
                else
                {
                    graphics.Clear(Color.FromArgb((graphics is SoftwareGraphics) ? 96 : 192, Color.Black));
                }

                graphics.SmoothingMode = entry.solidDraw ? SmoothingMode.None : SmoothingMode.AntiAlias;
                test.DebugView.m_Graphics = graphics;
                test.DebugView.m_Font = m_InfoFont;


                test.m_Scale = viewZoom * System.Math.Min(ClientWidth, ClientHeight) / 39;
                test.m_CenterX = ClientWidth / 2 + test.m_OffsetX * test.m_Scale;
                test.m_CenterY = ClientHeight / 2 + test.m_OffsetY * test.m_Scale;


                m_TPSCounter.Tick();

                settings.Hz = m_TPSCounter.TPS;

                if (test != null)
                {
                    test.TextLine = 30;
                    test.Update(settings);
                }


                test.m_CenterX = ClientWidth / 2 + test.m_OffsetX * test.m_Scale;
                test.m_CenterY = ClientHeight / 2 + test.m_OffsetY * test.m_Scale;
                test.DebugView.SetViewTransform(test.m_CenterX, test.m_CenterY, test.m_Scale);


                test.DrawTitle(30, 15, entry.Name);

                test.DebugView.RenderDebugData();
            }

            protected override void Render(Alt.GUI.Temporary.Gwen.Skin.Base skin)
            {
                m_PanelTop.ClientBackColor = Color.FromArgb((skin.Renderer.Graphics is SoftwareGraphics) ? 96 : 192, Color.Black);

                base.Render(skin);
            }


            void ResetView()
            {
                viewZoom = 1.0;
                test.m_OffsetX = 0;
                test.m_OffsetY = 12;
            }

            void Restart()
            {
                StartTest(testIndex);
            }


            protected override void OnMouseDown(GUI.MouseEventArgs e)
            {
                base.OnMouseDown(e);

                if (e.Button == GUI.MouseButtons.Left)
                {
                    test.MouseDown(e, test.ConvertScreenToWorld(e.X, e.Y));
                }
                else if (e.Button == GUI.MouseButtons.Right)
                {
                    lastp = e.Location;
                    rMouseDown = true;
                }
            }


            protected override void OnMouseUp(GUI.MouseEventArgs e)
            {
                base.OnMouseUp(e);

                if (e.Button == GUI.MouseButtons.Left)
                {
                    test.MouseUp(e, test.ConvertScreenToWorld(e.X, e.Y));
                }
                else if (e.Button == GUI.MouseButtons.Right)
                {
                    rMouseDown = false;
                }
            }


            protected override void OnMouseMove(GUI.MouseEventArgs e)
            {
                base.OnMouseMove(e);

                Vector2 wdiff = test.ConvertScreenToWorld(e.X, e.Y) - test.ConvertScreenToWorld(lastp.X, lastp.Y);
                if (rMouseDown)
                {
                    test.m_OffsetX += wdiff.X;
                    test.m_OffsetY -= wdiff.Y;
                }
                lastp = e.Location;

                test.MouseMove(e, test.ConvertScreenToWorld(e.X, e.Y), wdiff);
            }


            protected override void OnMouseWheel(GUI.MouseEventArgs e)
            {
                base.OnMouseWheel(e);

                var direction = e.Delta;
                if (direction < 0)
                {
                    viewZoom /= 1.1;
                }
                else
                {
                    viewZoom *= 1.1;
                }
            }
        }
    }
}
