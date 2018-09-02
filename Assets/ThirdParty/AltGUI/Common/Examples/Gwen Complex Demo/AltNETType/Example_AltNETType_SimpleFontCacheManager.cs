//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.NETType;
using Alt.Sketch;
using Alt.Sketch.Geometries;
using Alt.Sketch.Geometries.Reformers;
using Alt.Sketch.Geometries.Transformers;


namespace Alt.GUI.Demo
{
    class Example_AltNETType_SimpleFontCacheManager : Example_AltNETType_Base
    {
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_InvertCheckBox;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_HintingCheckBox;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_KerningCheckBox;

		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_OutlineThicknessSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_OutlineThicknessSliderLabel;
        const string m_OutlineThicknessSliderLabelPrefix = "Outline Thickness (for 72pt) = ";

		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_WidthSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_WidthSliderLabel;
        const string m_WidthSliderLabelPrefix = "Font Width = ";

		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_HeightSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_HeightSliderLabel;
        const string m_HeightSliderLabelPrefix = "Font Height = ";

		Alt.GUI.Temporary.Gwen.Control.Base m_Client;


        SimpleFontCacheManager m_AltNETTypeFontManager;
        double m_OldHeight;

        // Pipeline to process the vectors glyph paths (curves + contour)
        FlattenCurveGeometry m_FlattenGlyph;
        Matrix4 m_mtx;
        GeometryMatrix4Transformer m_TransformedFlattenGlyph;


        public Example_AltNETType_SimpleFontCacheManager(Base parent)
            : base(parent)
        {
            //  OutlineThicknessSlider
			m_OutlineThicknessSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_OutlineThicknessSlider.Dock = Pos.Bottom;
            m_OutlineThicknessSlider.SetSize(150, 20);
            m_OutlineThicknessSlider.SetRange(0.5f, 3);
            m_OutlineThicknessSlider.ValueChanged += new GwenEventHandler(control_ValueChanged);

			m_OutlineThicknessSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_OutlineThicknessSliderLabel.AutoSizeToContents = true;
            m_OutlineThicknessSliderLabel.Dock = Pos.Bottom;
            m_OutlineThicknessSliderLabel.Margin = new Margin(0, 3, 0, 0);

            m_OutlineThicknessSlider.Value = 0.9f;


            //  Width
			m_WidthSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_WidthSlider.Dock = Pos.Bottom;
            m_WidthSlider.SetRange(8, 72);
            m_WidthSlider.NotchCount = (int)(m_WidthSlider.Max - m_WidthSlider.Min);
            m_WidthSlider.SnapToNotches = true;
            m_WidthSlider.Height = 20;
            m_WidthSlider.ValueChanged += new GwenEventHandler(control_ValueChanged);

			m_WidthSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_WidthSliderLabel.AutoSizeToContents = true;
            m_WidthSliderLabel.Dock = Pos.Bottom;
            m_WidthSliderLabel.Margin = new Margin(0, 3, 0, 0);

            m_WidthSlider.Value = 48;


            //  Height
			m_HeightSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_HeightSlider.Dock = Pos.Bottom;
            m_HeightSlider.SetRange(8, 72);
            m_HeightSlider.NotchCount = (int)(m_HeightSlider.Max - m_HeightSlider.Min);
            m_HeightSlider.SnapToNotches = true;
            m_HeightSlider.Height = 20;
            m_HeightSlider.ValueChanged += new GwenEventHandler(control_ValueChanged);

			m_HeightSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_HeightSliderLabel.AutoSizeToContents = true;
            m_HeightSliderLabel.Dock = Pos.Bottom;
            m_HeightSliderLabel.Margin = new Margin(0, 5, 0, 0);

            m_HeightSlider.Value = m_WidthSlider.Value;


            //  m_Hinting
            m_HintingCheckBox = new LabeledCheckBox(this);
            m_HintingCheckBox.Margin = new Margin(0, 5, 0, 0);
            m_HintingCheckBox.Dock = Pos.Bottom;
            m_HintingCheckBox.Text = "Hinting";
            m_HintingCheckBox.IsChecked = true;
            m_HintingCheckBox.CheckChanged += new GwenEventHandler(control_CheckChanged);


            //  Kerning
            m_KerningCheckBox = new LabeledCheckBox(this);
            m_KerningCheckBox.Margin = new Margin(0, 5, 0, 0);
            m_KerningCheckBox.Dock = Pos.Bottom;
            m_KerningCheckBox.Text = "Kerning";
            m_KerningCheckBox.IsChecked = true;
            m_KerningCheckBox.CheckChanged += new GwenEventHandler(control_CheckChanged);


            //  Invert
            m_InvertCheckBox = new LabeledCheckBox(this);
            m_InvertCheckBox.Margin = new Margin(0, 10, 0, 0);
            m_InvertCheckBox.Dock = Pos.Bottom;
            m_InvertCheckBox.Text = "Invert";
            m_InvertCheckBox.IsChecked = false;
            m_InvertCheckBox.CheckChanged += new GwenEventHandler(control_CheckChanged);


            //  Font
            CreateFontPanel();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            m_Client = new DoubleBufferedControl(this);
            m_Client.ClientBackColor = Color.Empty;
            m_Client.Dock = Pos.Fill;
            m_Client.Paint += new GUI.PaintEventHandler(Client_Paint);

            
            //
            m_AltNETTypeFontManager = new SimpleFontCacheManager();

            m_FlattenGlyph = new FlattenCurveGeometry();
            //m_FlattenGlyph.ApproximationScale = 2.0;

            m_mtx = Matrix4.Identity;
            m_TransformedFlattenGlyph = new GeometryMatrix4Transformer(m_FlattenGlyph, m_mtx);


            //  update labels
            control_ValueChanged(null);
        }


        void control_ValueChanged(Base control)
        {
            if (m_Client != null)
            {
                m_OutlineThicknessSliderLabel.Text = m_OutlineThicknessSliderLabelPrefix + m_OutlineThicknessSlider.Value.ToString("F2").Replace(',', '.');
                m_WidthSliderLabel.Text = m_WidthSliderLabelPrefix + m_WidthSlider.Value.ToString("F2").Replace(',', '.');
                m_HeightSliderLabel.Text = m_HeightSliderLabelPrefix + m_HeightSlider.Value.ToString("F2").Replace(',', '.');

                ClientRefresh();
            }
        }


        void control_CheckChanged(Base control)
        {
            ClientRefresh();
        }


        double DrawText(Graphics graphics,
            string font, string text,
            double x, double y, double height,
            Color color,
            SimpleFont.GlyphRenderType glyphRenderType)
        {
            if (color == Color.Black &&
                m_InvertCheckBox.IsChecked)
            {
                color = Color.White;
            }


            if (m_AltNETTypeFontManager.LoadFont(font, glyphRenderType))
            {
                m_AltNETTypeFontManager.Hinting = m_HintingCheckBox.IsChecked;
                m_AltNETTypeFontManager.Height = m_HeightSlider.Value;
                m_AltNETTypeFontManager.Width = m_WidthSlider.Value;
                m_AltNETTypeFontManager.FlipY = true;

                double start_x = x;
                bool fHinting = m_HintingCheckBox.IsChecked;
                bool fKerning = m_KerningCheckBox.IsChecked;

                double thickness = m_OutlineThicknessSlider.Value * height / 72;

                int len = text.Length;
                for (int i = 0; i < len; i++)
                {
                    char CharValue = text[i];

                    if (CharValue == '\n')
                    {
                        x = start_x;
                        y += height * 1.25;

                        continue;
                    }

                    
                    SimpleFontCacheManager.GlyphCache glyph = m_AltNETTypeFontManager.GetGlyph(CharValue);
                    if (glyph != null)
                    {
                        if (fKerning)
                        {
                            m_AltNETTypeFontManager.AddKerning(ref x, ref y);
                        }


                        double tx = fHinting ? System.Math.Floor(x + 0.5) : x;
                        double ty = fHinting ? System.Math.Floor(y + 0.5) : y;

                        
                        switch (glyph.data_type)
                        {
                            case SimpleFont.GlyphDataType.Mono:
                            case SimpleFont.GlyphDataType.Gray8:

                                Bitmap bitmap = glyph.m_Data as Bitmap;
                                if (bitmap != null)
                                {
                                    BitmapData bitmapData = bitmap.LockBits(ImageLockMode.WriteOnly);
                                    if (bitmapData != null)
                                    {
                                        byte[] data = bitmapData.Scan0;
                                        int stride = bitmapData.Stride;
                                        int h = bitmap.PixelHeight;
                                        int w = bitmap.PixelWidth;

                                        byte r = color.R;
                                        byte g = color.G;
                                        byte b = color.B;

                                        for (int by = 0; by < h; by++)
                                        {
                                            int index = by * stride;
                                            for (int bx = 0; bx < w; bx++, index += 4)
                                            {
                                                data[index] = r;
                                                data[index + 1] = g;
                                                data[index + 2] = b;
                                            }
                                        }

                                        graphics.DrawImage(bitmap, tx + glyph.bounds.Left, ty - glyph.bounds.Top);

                                        bitmap.UnlockBits(bitmapData);
                                    }
                                }

                                break;

                            case SimpleFont.GlyphDataType.Outline:
                                m_FlattenGlyph.Geometry = glyph.m_Data as Geometry;
                                if (m_FlattenGlyph.Geometry != null)
                                {
                                    m_mtx = Matrix4.Identity;
                                    m_mtx.Translate(x, ty, MatrixOrder.Append);
                                    m_TransformedFlattenGlyph.Transform = m_mtx;

                                    graphics.DrawGeometry(color, m_TransformedFlattenGlyph, thickness);
                                }

                                break;
                        }

                        // increment pen position
                        x += glyph.advance_x;
                        y += glyph.advance_y;
                    }
                }

                y += height * 1.5;
            }

            return y;
        }


        void Client_Paint(object sender, GUI.PaintEventArgs e)
        {
			if(m_HeightSlider.Value != m_OldHeight)
			{
                m_OldHeight = m_HeightSlider.Value;
				m_WidthSlider.Value = m_HeightSlider.Value;
			}


            Color color = Color.White;
            if (m_InvertCheckBox.IsChecked)
            {
                color = Color.Black;
            }
            e.Graphics.Clear(color);


            string fontFileName = FontFileName;
            if (string.IsNullOrEmpty(fontFileName))
            {
                return;
            }


            double x = 20;
            double height = m_HeightSlider.Value;
            double y = height + 20;


            y = DrawText(e.Graphics, fontFileName, Common.ShortText, x, y, height, Color.Black, SimpleFont.GlyphRenderType.Gray8);
            y = DrawText(e.Graphics, fontFileName, Common.ShortText, x, y, height, Color.Black, SimpleFont.GlyphRenderType.Mono);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            y = DrawText(e.Graphics, fontFileName, Common.ShortText, x, y, height, Color.Black, SimpleFont.GlyphRenderType.Outline);
        }


        protected override void ClientRefresh()
        {
            if (m_Client != null)
            {
                m_Client.Refresh();
            }
        }
    }
}
