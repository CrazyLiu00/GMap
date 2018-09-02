//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

using Alt.GUI.Temporary.Gwen;
using Alt.IO;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
	public class AltGUIGwenComplexDemoControl : GwenHost
    {
        TPSCounter m_FPSCounter = new TPSCounter();
        public double FPS
        {
            get
            {
                return m_FPSCounter.TPS;
            }
        }


        ExamplesHolder m_ExamplesHolder;


        public Cursor CurrentCursor
        {
            get
            {
                if (Canvas != null)
                {
                    return Canvas.CurrentCursor;
                }

                return Cursors.Default;
            }
        }


		public AltGUIGwenComplexDemoControl()
        {
            //  BackgroundBrush
            string bgFileName = "AltData/BG.jpg";
            if (VirtualFile.Exists(bgFileName))
            {
                Canvas.Background = new TextureBrush(Bitmap.FromFile(bgFileName));
            }
            else
            {
#if (!WINDOWS_PHONE && !WINDOWS_PHONE_7 && !WINDOWS_PHONE_71)
                RadialGradientBrush brush = new RadialGradientBrush();
                brush.GradientOrigin = new Vector2(0.186, 0.311);
                brush.GradientStops.Add(new GradientStop(0, Color.FromArgb(128, Color.Parse("#FF5C8AE6"))));
                brush.GradientStops.Add(new GradientStop(1, Color.FromArgb(192, Color.Parse("#FF1254A3") * 1.3)));

                Canvas.Background = brush;
#else
                Canvas.Background = new SolidColorBrush(Color.Parse("#FF1254A3") * 1.3);
#endif
            }

            Child = m_ExamplesHolder = new ExamplesHolder(Canvas);

            m_IntervalTimer.Reset();
        }


        void Shutdown()
        {
            if (m_ExamplesHolder != null)
            {
                m_ExamplesHolder.Shutdown();
                m_ExamplesHolder.Dispose();
                m_ExamplesHolder = null;
            }
        }


        protected override void Dispose(bool disposing)
        {
            Shutdown();
            
            base.Dispose(disposing);
        }


        void DrawAdditionLogos(Graphics graphics)
        {
            Bitmap logo = GUI.Config.Logo;
            RectI logoRect = m_ExamplesHolder.LogoArea;
            PointI logoPos = new PointI(logoRect.Right - logo.PixelWidth - 11, logoRect.Top + 15);
            Color colorMultiplier = Color.FromArgb(200, Color.White);

#if WINDOWS_PHONE || WINDOWS_PHONE_7 || WINDOWS_PHONE_71 || ANDROID || __IOS__
            logoPos.Y += 9 + logo.PixelHeight;
            foreach (Bitmap image in Config.AdditionLogos)
            {
                if (image == null)
                {
                    continue;
                }

                graphics.DrawImage(image, logoRect.Right - image.PixelWidth - 11 - 4, logoPos.Y, colorMultiplier);
                if (!image.HasAlpha)
                {
                    graphics.DrawRectangle(Color.DodgerBlue, logoRect.Right - image.PixelWidth - 11 - 4 - 1, logoPos.Y - 1,
                        image.PixelWidth + 1, image.PixelHeight + 1);
                }

                logoPos.Y += image.PixelHeight + 7;
                //logoPos.X -= image.PixelWidth + 11;
            }
#else
            logoPos.X -= 11;
            foreach (Bitmap image in Config.AdditionLogos)
            {
                if (image == null)
                {
                    continue;
                }

                if ((logoPos.Y + (logo.PixelHeight - image.PixelHeight) / 2) < 0)
                {
                    logoPos.Y -= logoPos.Y + (logo.PixelHeight - image.PixelHeight) / 2;
                }

                graphics.DrawImage(image, logoPos.X - image.PixelWidth, logoPos.Y + (logo.PixelHeight - image.PixelHeight) / 2, colorMultiplier);
                if (!image.HasAlpha)
                {
                    graphics.DrawRectangle(Color.DodgerBlue, logoPos.X - image.PixelWidth - 1, logoPos.Y + (logo.PixelHeight - image.PixelHeight) / 2 - 1,
                        image.PixelWidth + 1, image.PixelHeight + 1);
                }

                //logoPos.Y += image.PixelHeight;
                logoPos.X -= image.PixelWidth + 11;
            }
#endif
        }

        protected override void OnCanvasPostDrawBackground(PaintEventArgs paint_event)
        {
            base.OnCanvasPostDrawBackground(paint_event);

            DrawAdditionLogos(paint_event.Graphics);
        }


        IntervalTimer m_IntervalTimer = new IntervalTimer(1000);
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            //  Draw AdditionLogos before foreground render
            if (!Canvas.ShouldDrawBackground)
            {
                DrawAdditionLogos(graphics);
            }

            
            //
            m_FPSCounter.Tick();

            if (m_IntervalTimer.IsTimeOver)
            {
                m_ExamplesHolder.FPS = FPS;
            }


            base.OnPaint(e);


            //  Need to process system redraw needs (like window overlapping etc.)
            if (Size != SizeI.Empty)
            {
                //  Draw Main Logo
                Bitmap logo = GUI.Config.Logo;
                RectI logoRect = m_ExamplesHolder.LogoArea;
                PointI logoPos = new PointI(logoRect.Right - logo.PixelWidth - 10, logoRect.Top + 15);
                graphics.DrawImage(logo, logoPos);
            }
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        
        public void RegisterDemo(string category, string caption, Type exampleType, Color categoryTextColor, Color captionTextColor)
        {
            m_ExamplesHolder.RegisterDemo(category, caption, exampleType, categoryTextColor, captionTextColor);
        }


        public void SetCenterInfo(string text)
        {
            if (m_ExamplesHolder != null)
            {
                m_ExamplesHolder.SetCenterInfo(text);
            }
        }
    }
}
