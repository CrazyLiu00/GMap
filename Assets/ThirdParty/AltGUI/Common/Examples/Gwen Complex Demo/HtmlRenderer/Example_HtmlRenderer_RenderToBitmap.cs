//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

using Alt.ComponentModel;
using Alt.GUI.HtmlRenderer;
using Alt.GUI.HtmlRenderer.Core;
using Alt.GUI.HtmlRenderer.Core.Entities;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.IO;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
    class Example_HtmlRenderer_RenderToBitmap : Example_HtmlRenderer_Base
    {
        //NoNeed	object m_Lock = new object();
        HtmlContainer m_HtmlContainer;


        Bitmap m_Image;
		Alt.GUI.Temporary.Gwen.Control.ScrollControl m_ImageScrollControl;
		Alt.GUI.Temporary.Gwen.Control.Base m_ImagePanel;

		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_FillBackground;

#if !SILVERLIGHT
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_AntiAliasText;
#endif

		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_WidthSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_WidthSliderLabel;
        const string m_WidthSliderLabelPrefix = "Bitmap Width = ";


        public Example_HtmlRenderer_RenderToBitmap(Base parent) :
            base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            m_HtmlContainer = new HtmlContainer();
            m_HtmlContainer.Refresh += OnRefresh;
            m_HtmlContainer.RenderError += OnRenderError;
            m_HtmlContainer.StylesheetLoad += OnStylesheetLoad;
            m_HtmlContainer.ImageLoad += OnImageLoad;

            
            //  FillBackground
            m_FillBackground = new LabeledCheckBox(m_LeftPanel);
            m_FillBackground.Text = "Fill Background";
            m_FillBackground.Dock = Pos.Bottom;
            m_FillBackground.Margin = new Margin(2, 10, 2, 0);
            m_FillBackground.IsChecked = true;
            m_FillBackground.CheckChanged += new GwenEventHandler(FillBackground_CheckChanged);


#if !SILVERLIGHT
            //  AntiAlias Text
            m_AntiAliasText = new LabeledCheckBox(m_LeftPanel);
            m_AntiAliasText.Text = "AntiAlias Text";
            m_AntiAliasText.Dock = Pos.Bottom;
            m_AntiAliasText.Margin = new Margin(2, 10, 2, 2);
            //m_AntiAliasText.IsChecked = true;
            m_AntiAliasText.CheckChanged += new GwenEventHandler(AntiAliasText_CheckChanged);
#endif


            //  Width
            m_WidthSlider = new HorizontalSlider(m_RightPanel);
            m_WidthSlider.Dock = Pos.Bottom;
            m_WidthSlider.SetRange(200, 1000);
            m_WidthSlider.Height = 20;
            m_WidthSlider.ValueChanged += new GwenEventHandler(WidthSlider_ValueChanged);

			m_WidthSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(m_RightPanel);
            m_WidthSliderLabel.AutoSizeToContents = true;
            m_WidthSliderLabel.Dock = Pos.Bottom;
            m_WidthSliderLabel.Margin = new Margin(0, 10, 0, 0);


            //  Image
            Base imageMainPanel = new Base(m_RightPanel);
            imageMainPanel.Dock = Pos.Fill;


			m_ImageScrollControl = new Alt.GUI.Temporary.Gwen.Control.ScrollControl(imageMainPanel);
            m_ImageScrollControl.Margin = Margin.One;
            m_ImageScrollControl.Dock = Pos.Fill;
            m_ImageScrollControl.EnableScroll(true, true);
            m_ImageScrollControl.AutoHideBars = true;
            m_ImageScrollControl.ShouldDrawBackground = false;


            m_ImagePanel = new Base(m_ImageScrollControl);
            m_ImagePanel.Margin = Margin.Two;
            m_ImagePanel.Location = PointI.Zero;
            m_ImagePanel.Size = new SizeI(100, 100);
            m_ImagePanel.Paint += new GUI.PaintEventHandler(ImagePanel_Paint);
            m_ImagePanel.Hide();


            m_WidthSlider.Value = 500;


            SelectFirstExample();
        }


        /// <summary>
        /// Release the html container resources.
        /// </summary>
        public override void Dispose()
        {
            if (m_HtmlContainer != null)
            {
                m_HtmlContainer.Refresh -= OnRefresh;
                m_HtmlContainer.RenderError -= OnRenderError;
                m_HtmlContainer.StylesheetLoad -= OnStylesheetLoad;
                m_HtmlContainer.ImageLoad -= OnImageLoad;
                m_HtmlContainer.Dispose();
                m_HtmlContainer = null;
            }

            base.Dispose();
        }


        /// <summary>
        /// Handle html renderer invalidate and re-layout as requested.
        /// </summary>
        void OnRefresh(object sender, HtmlRefreshEventArgs e)
        {
            GUI.MethodInvoker m = delegate()
            {
                m_NeedRepaint = true;
            };

            try
            {
                Invoke(m);
            }
            catch
            {
            }
        }


        public override void SetHtml(string text)
        {
            m_ImageScrollControl.ScrollToTop();
            m_ImageScrollControl.ScrollToLeft();

            m_HtmlContainer.SetHtml(text, HtmlRender.ParseStyleSheet//CssData.Parse
                (null, true));

            CreateBitmap();

            m_ImagePanel.Show();
        }


        void CreateBitmap()
        {
            int width = (int)m_WidthSlider.Value;
            m_HtmlContainer.MaxSize = new Size(width, 0);
            using (Graphics g = Graphics.CreateDefaultGraphics())
            {
                m_HtmlContainer.PerformLayout(g);
                int height = (int)m_HtmlContainer.ActualSize.Height;

                if (m_Image != null &&
                    m_Image.PixelSize != new SizeI(width, height))
                {
                    m_Image.Dispose();
                    m_Image = null;
                }

                if (!m_HtmlContainer.ActualSize.IsEmpty)
                {
                    if (m_Image == null)
                    {
                        m_Image = new Bitmap(width, height);//m_HtmlContainer.ActualSize.ToSizeI());
                    }

                    using (Graphics graphics = Graphics.FromImage(m_Image))
                    {
                        if (m_FillBackground.IsChecked)
                        {
                            graphics.Clear(Color.FromArgb(128, Color.White));
                        }
                        else
                        {
                            graphics.Clear(Color.Empty);//Transparent);
                        }

                        m_HtmlContainer.ScrollOffset = Point.Zero;
#if !SILVERLIGHT
                        graphics.TextRenderingHint = m_AntiAliasText.IsChecked ? TextRenderingHint.AntiAliasGridFit : TextRenderingHint.Default;
#endif
                        m_HtmlContainer.PerformPaint(graphics);
                    }

                    m_ImagePanel.Size = m_Image.PixelSize;
                }
            }
        }


        bool m_NeedRepaint;
        void ImagePanel_Paint(object sender, GUI.PaintEventArgs e)
        {
            if (m_NeedRepaint)
            {
                CreateBitmap();

                m_NeedRepaint = false;
            }

            if (m_Image != null)
            {
                e.Graphics.DrawImage(m_Image, PointI.Zero);
            }
        }


        void WidthSlider_ValueChanged(Base control)
        {
            if (m_ImagePanel != null)
            {
                m_WidthSliderLabel.Text = m_WidthSliderLabelPrefix + ((int)m_WidthSlider.Value).ToString();

                m_NeedRepaint = true;
            }
        }


        void AntiAliasText_CheckChanged(Base control)
        {
            m_NeedRepaint = true;
        }


        void FillBackground_CheckChanged(Base control)
        {
            m_NeedRepaint = true;
        }
    }
}
