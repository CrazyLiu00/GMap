//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Box2D.Demo.Testbed;
using Alt.GUI.Box2D.Demo.Testbed.Framework;
using Alt.GUI.Box2D.Demo.Testbed.Tests;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
    class Example_Box2D : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                return base.LogoRightTopOffset + new SizeI(0, 38);
            }
        }


        Box2DContainer m_Box2DContainer;


        public Example_Box2D(Base parent)
            : base(parent)
        {
            m_Box2DContainer = new Box2DContainer(this);
            m_Box2DContainer.Dock = Pos.Fill;
            m_Box2DContainer.Focus();
        }

        protected override void Render(Alt.GUI.Temporary.Gwen.Skin.Base skin)
        {
            m_Box2DContainer.DoubleBuffered = !skin.Renderer.Graphics.IsClippingSupported;
            if (m_Box2DContainer.DoubleBuffered)
            {
                m_Box2DContainer.Refresh();
            }

            base.Render(skin);
        }

        protected internal override void OnActivate()
        {
            base.OnActivate();

            if (m_Box2DContainer != null)
            {
                m_Box2DContainer.Focus();
            }
        }


        class Box2DContainer : DoubleBufferedControl
        {
            public Box2DContainer(Base parent) :
                base(parent)
            {
                DoubleBuffered = false;
                KeyboardInputEnabled = true;
            }


            int testIndex = 0;
            int testSelection = 0;
            int testCount = 0;
            TestEntry entry;
            Test test;
            Alt.GUI.Box2D.Demo.Testbed.Framework.Settings settings;
            double viewZoom = 1.0;
            bool rMouseDown;
            Point lastp;

            TPSCounter m_TPSCounter = new TPSCounter();

            Font m_InfoFont;
			Alt.GUI.Temporary.Gwen.Control.Base m_PanelTop;
			Alt.GUI.Temporary.Gwen.Control.Label m_ExampleNumberLabel;

            double m_OffsetX;
            double m_OffsetY;
            double m_CenterX;
            double m_CenterY;
            double m_Scale;


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

					Alt.GUI.Temporary.Gwen.Control.Button ballButton = new Alt.GUI.Temporary.Gwen.Control.Button(controlsMain);
                    ballButton.Dock = Pos.Right;
                    ballButton.Click += new EventHandler(BallButton_Click);
                    ballButton.Text = "PUSH BALL";
                    ballButton.NormalTextColor = Color.Green;
                    ballButton.Margin = new Margin(5, 1, 5, 1);
                }


                settings = new Alt.GUI.Box2D.Demo.Testbed.Framework.Settings();

                testCount = 0;
                while (TestEntries.g_testEntries[testCount].createFcn != null)
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

            void BallButton_Click(object sender, EventArgs e)
            {
                if (test != null)
                {
                    test.LaunchBomb();
                }
            }

            void RestartButton_Click(object sender, EventArgs e)
            {
                if (entry.createFcn != null)
                {
                    test = entry.createFcn();
                }
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
                    test = entry.createFcn();
                }
                // Press space to launch a bomb.
                else if (e.KeyCode == GUI.Keys.Space ||
                    e.KeyCode == GUI.Keys.B)
                {
                    if (test != null)
                    {
                        test.LaunchBomb();
                    }
                }
                else if (e.KeyCode == GUI.Keys.P)
                // || newGamePad.IsButtonDown(Buttons.Start) && oldGamePad.IsButtonUp(Buttons.Start))
                {
                    settings.pause = settings.pause > (uint)0 ? (uint)1 : (uint)0;
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
                    m_OffsetX -= 10;
                }
                // Press right to pan right.
                else if (e.KeyCode == GUI.Keys.Right)
                {
                    m_OffsetX += 10;
                }
                // Press down to pan down.
                else if (e.KeyCode == GUI.Keys.Down)
                {
                    m_OffsetY += 10;
                }
                // Press up to pan up.
                else if (e.KeyCode == GUI.Keys.Up)
                {
                    m_OffsetY -= 10;
                }
                // Press home to reset the view.
                else if (e.KeyCode == GUI.Keys.Home)
                {
                    viewZoom = 1.0;
                    m_OffsetX = 0;
                    m_OffsetY = 0;
                }
                else
                {
                    if (test != null)
                    {
                        test.InjectKeyDown(e);
                    }
                }
            }


            protected override void OnPaint(GUI.PaintEventArgs e)
            {
                base.OnPaint(e);

                if (testSelection != testIndex)
                {
                    testIndex = testSelection;
                    entry = TestEntries.g_testEntries[testIndex];
                    test = entry.createFcn();

                    viewZoom = 1.0;
                    m_OffsetX = 0;
                    m_OffsetY = 0;

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
                test._debugDraw.m_Graphics = graphics;
                test._debugDraw.m_Font = m_InfoFont;

                m_Scale = viewZoom * System.Math.Min(ClientWidth, ClientHeight) / 39;
                m_CenterX = ClientWidth / 2 + m_OffsetX;
                m_CenterY = ClientHeight * 0.85 + m_OffsetY;
                test._debugDraw.SetViewTransform(m_CenterX, m_CenterY, m_Scale);


                m_TPSCounter.Tick();

                test.SetTextLine(30);
                settings.hz = m_TPSCounter.TPS;// settingsHz;

                test.Step(settings);

                test.DrawTitle(50, 15, entry.name);
            }

            protected override void Render(Alt.GUI.Temporary.Gwen.Skin.Base skin)
            {
                m_PanelTop.ClientBackColor = Color.FromArgb((skin.Renderer.Graphics is SoftwareGraphics) ? 96 : 192, Color.Black);

                base.Render(skin);
            }


            Vector2 ConvertScreenToWorld(double x, double y)
            {
                Matrix4 matrix = Matrix4.CreateTranslation(m_CenterX, m_CenterY);
                matrix.Scale(m_Scale, -m_Scale);
                matrix.Invert();

                matrix.Transform(ref x, ref y);
                return new Vector2(x, y);
            }


            protected override void OnMouseDown(GUI.MouseEventArgs e)
            {
                base.OnMouseDown(e);

                if (e.Button == GUI.MouseButtons.Left)
                {
                    Vector2 p = ConvertScreenToWorld(e.X, e.Y);

                    if (InputHandler.IsShiftDown)
                    {
                        test.ShiftMouseDown(p);
                    }
                    else
                    {
                        test.MouseDown(p);
                    }
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
                    test.MouseUp(ConvertScreenToWorld(e.X, e.Y));
                }
                else if (e.Button == GUI.MouseButtons.Right)
                {
                    rMouseDown = false;
                }
            }


            protected override void OnMouseMove(GUI.MouseEventArgs e)
            {
                base.OnMouseMove(e);

                if (rMouseDown)
                {
                    Vector2 diff = e.Location - lastp;
                    m_OffsetX += diff.X;
                    m_OffsetY += diff.Y;
                }
                lastp = e.Location;

                test.MouseMove(ConvertScreenToWorld(e.X, e.Y));
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


            void Restart()
            {
                entry = TestEntries.g_testEntries[testIndex];
                test = entry.createFcn();
            }

            void Pause()
            {
                settings.pause = (uint)(settings.pause > 0 ? 0 : 1);
            }

            void SingleStep()
            {
                settings.pause = 1;
                settings.singleStep = 1;
            }
        }
    }
}
