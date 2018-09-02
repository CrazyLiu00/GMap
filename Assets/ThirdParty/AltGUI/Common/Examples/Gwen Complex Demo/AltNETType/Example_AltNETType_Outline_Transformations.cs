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
    class Example_AltNETType_Outline_Transformations : Example_AltNETType_Base
    {
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_InvertCheckBox;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_KerningCheckBox;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_HintingCheckBox;

		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_HeightSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_HeightSliderLabel;
        const string m_HeightSliderLabelPrefix = "Font Scale (Height) = ";

		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_WidthSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_WidthSliderLabel;
        const string m_WidthSliderLabelPrefix = "Width = ";

		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_IntervalSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_IntervalSliderLabel;
        const string m_IntervalSliderLabelPrefix = "Interval = ";

		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_AuxWeightSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_AuxWeightSliderLabel;
        const string m_AuxWeightSliderLabelPrefix = "Faux Weight = ";

		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_AuxItalicSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_AuxItalicSliderLabel;
        const string m_AuxItalicSliderLabelPrefix = "Faux Italic = ";

		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_FillCheckBox;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_OutlineCheckBox;
		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_OutlineThicknessSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_OutlineThicknessSliderLabel;
        const string m_OutlineThicknessSliderLabelText = "Outline Thickness = ";


        Base m_Client;


        SimpleFontCacheManager m_AltNETTypeFontManager;

        // Pipeline to process the vectors glyph paths (curves + contour)
        Matrix4 m_mtx;
        FlattenCurveGeometry m_FlattenGlyph;
        GeometryMatrix4Transformer m_TransformedFlattenGlyph;
        GeometryAuxWeight m_WeightedTransformedFlattenGlyph;


        public Example_AltNETType_Outline_Transformations(Base parent)
            : base(parent)
        {
            //  OutlineThicknessSlider
			m_OutlineThicknessSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_OutlineThicknessSlider.Dock = Pos.Bottom;
            m_OutlineThicknessSlider.SetSize(150, 20);
            m_OutlineThicknessSlider.SetRange(0.5f, 3);
            m_OutlineThicknessSlider.ValueChanged += OutlineThicknessSlider_ValueChanged;

			m_OutlineThicknessSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_OutlineThicknessSliderLabel.AutoSizeToContents = true;
            m_OutlineThicknessSliderLabel.Dock = Pos.Bottom;
            m_OutlineThicknessSliderLabel.Margin = new Margin(0, 3, 0, 0);

            m_OutlineThicknessSlider.Value = 0.6f;


            //  OutlineCheckBox
			m_OutlineCheckBox = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(this);
            m_OutlineCheckBox.Margin = new Margin(0, 5, 0, 0);
            m_OutlineCheckBox.Text = "Outline";
            m_OutlineCheckBox.Dock = Pos.Bottom;
            m_OutlineCheckBox.CheckChanged += new GwenEventHandler(control_CheckChanged);


            //  FillCheckBox
			m_FillCheckBox = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(this);
            m_FillCheckBox.Margin = new Margin(0, 5, 0, 0);
            m_FillCheckBox.Text = "Fill";
            m_FillCheckBox.Dock = Pos.Bottom;
            m_FillCheckBox.IsChecked = true;
            m_FillCheckBox.CheckChanged += new GwenEventHandler(control_CheckChanged);

            
            //  Faux Italic
            m_AuxItalicSlider = new HorizontalSlider(this);
            m_AuxItalicSlider.Dock = Pos.Bottom;
            m_AuxItalicSlider.SetRange(-1.5f, 1.5f);
            m_AuxItalicSlider.Height = 20;
            m_AuxItalicSlider.ValueChanged += new GwenEventHandler(control_ValueChanged);

			m_AuxItalicSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_AuxItalicSliderLabel.AutoSizeToContents = true;
            m_AuxItalicSliderLabel.Dock = Pos.Bottom;
            m_AuxItalicSliderLabel.Margin = new Margin(0, 3, 0, 0);

            m_AuxItalicSlider.Value = 0;


            //  Interval
			m_IntervalSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_IntervalSlider.Dock = Pos.Bottom;
            m_IntervalSlider.SetRange(-5, 5);
            m_IntervalSlider.Height = 20;
            m_IntervalSlider.ValueChanged += new GwenEventHandler(control_ValueChanged);

			m_IntervalSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_IntervalSliderLabel.AutoSizeToContents = true;
            m_IntervalSliderLabel.Dock = Pos.Bottom;
            m_IntervalSliderLabel.Margin = new Margin(0, 3, 0, 0);

            m_IntervalSlider.Value = 0;


            //  Faux Weight
			m_AuxWeightSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_AuxWeightSlider.Dock = Pos.Bottom;
            m_AuxWeightSlider.SetRange(-0.2f, 1);
            m_AuxWeightSlider.Height = 20;
            m_AuxWeightSlider.ValueChanged += new GwenEventHandler(control_ValueChanged);

			m_AuxWeightSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_AuxWeightSliderLabel.AutoSizeToContents = true;
            m_AuxWeightSliderLabel.Dock = Pos.Bottom;
            m_AuxWeightSliderLabel.Margin = new Margin(0, 3, 0, 0);

            m_AuxWeightSlider.Value = 0;


            //  Width
			m_WidthSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_WidthSlider.Dock = Pos.Bottom;
            m_WidthSlider.SetRange(0.75f, 1.25f);
            m_WidthSlider.Height = 20;
            m_WidthSlider.ValueChanged += new GwenEventHandler(control_ValueChanged);

			m_WidthSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_WidthSliderLabel.AutoSizeToContents = true;
            m_WidthSliderLabel.Dock = Pos.Bottom;
            m_WidthSliderLabel.Margin = new Margin(0, 3, 0, 0);

            m_WidthSlider.Value = 1;


            //  Font Scale (Height)
			m_HeightSlider = new Alt.GUI.Temporary.Gwen.Control.HorizontalSlider(this);
            m_HeightSlider.Dock = Pos.Bottom;
            m_HeightSlider.SetRange(0.5f, 2);
            m_HeightSlider.Height = 20;
            m_HeightSlider.ValueChanged += new GwenEventHandler(control_ValueChanged);

			m_HeightSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_HeightSliderLabel.AutoSizeToContents = true;
            m_HeightSliderLabel.Dock = Pos.Bottom;
            m_HeightSliderLabel.Margin = new Margin(0, 5, 0, 0);

            m_HeightSlider.Value = 1;


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
			m_InvertCheckBox = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(this);
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
            

            //
            m_Client = new DoubleBufferedControl(this);
            m_Client.ClientBackColor = Color.Empty;
            m_Client.Dock = Pos.Fill;
            m_Client.Paint += new GUI.PaintEventHandler(Client_Paint);
            

            //
            m_AltNETTypeFontManager = new SimpleFontCacheManager();
            m_AltNETTypeFontManager.FlipY = true;

            m_mtx = Matrix4.Identity;
            m_FlattenGlyph = new FlattenCurveGeometry();
            //m_FlattenGlyph.ApproximationScale = 4.0;

            m_TransformedFlattenGlyph = new GeometryMatrix4Transformer(m_FlattenGlyph, m_mtx);
            m_WeightedTransformedFlattenGlyph = new GeometryAuxWeight(m_TransformedFlattenGlyph);


            //  update labels
            control_ValueChanged(null);
        }


        void control_ValueChanged(Base control)
        {
            if (m_Client != null)
            {
                m_WidthSliderLabel.Text = m_WidthSliderLabelPrefix + m_WidthSlider.Value.ToString("F2").Replace(',', '.');
                m_IntervalSliderLabel.Text = m_IntervalSliderLabelPrefix + m_IntervalSlider.Value.ToString("F2").Replace(',', '.');
                m_AuxWeightSliderLabel.Text = m_AuxWeightSliderLabelPrefix + m_AuxWeightSlider.Value.ToString("F2").Replace(',', '.');
                m_AuxItalicSliderLabel.Text = m_AuxItalicSliderLabelPrefix + m_AuxItalicSlider.Value.ToString("F2").Replace(',', '.');
                m_HeightSliderLabel.Text = m_HeightSliderLabelPrefix + m_HeightSlider.Value.ToString("F2").Replace(',', '.');

                ClientRefresh();
            }
        }


        void OutlineThicknessSlider_ValueChanged(Base control)
        {
            m_OutlineThicknessSliderLabel.Text = m_OutlineThicknessSliderLabelText + m_OutlineThicknessSlider.Value.ToString("F2").Replace(',', '.');

            ClientRefresh();
        }


        void control_CheckChanged(Base control)
        {
            ClientRefresh();
        }


        double DrawText(Graphics graphics,
            string font, string text,
            double x, double y, double height,
            Color color)
        {
            Color outlineColor = Color.Gray;
            if (color == Color.Black &&
                m_InvertCheckBox.IsChecked)
            {
                color = Color.White;
            }

            SimpleFont.GlyphRenderType gren = SimpleFont.GlyphRenderType.Outline;

            double scale_x = 100;

            if (m_AltNETTypeFontManager.LoadFont(font, gren))
            {
                m_AltNETTypeFontManager.Height = height;
                m_AltNETTypeFontManager.Width = height * scale_x;
                m_AltNETTypeFontManager.Hinting = m_HintingCheckBox.IsChecked;

                int text_index = 0;
                int text_len = text.Length;

                double start_x = x;
                bool fHinting = m_HintingCheckBox.IsChecked;
                bool fKerning = m_KerningCheckBox.IsChecked;
                double interval = m_IntervalSlider.Value;
                double italic = m_AuxItalicSlider.Value;
                double weight = m_AuxWeightSlider.Value;
                double width = m_WidthSlider.Value;
                m_WeightedTransformedFlattenGlyph.Weight = -weight * height / 15;
                bool fWeightedTransformedFlattenGlyph = System.Math.Abs(weight) > 0.05;

                bool fFill = m_FillCheckBox.IsChecked;
                bool fOutline = m_OutlineCheckBox.IsChecked;
                double thickness = m_OutlineThicknessSlider.Value;

                while (text_index < text_len)
                {
                    if (text[text_index] == '\n')
                    {
                        x = start_x;
                        y += height * 1.25;

                        text_index++;
                        continue;
                    }

                    SimpleFontCacheManager.GlyphCache glyph = m_AltNETTypeFontManager.GetGlyph(text[text_index]);
                    if (glyph != null)
                    {
                        if (fKerning)
                        {
                            m_AltNETTypeFontManager.AddKerning(ref x, ref y);
                        }

                        if (glyph.data_type == SimpleFont.GlyphDataType.Outline)
                        {
                            m_FlattenGlyph.Geometry = glyph.m_Data as Geometry;

                            double ty = fHinting ? System.Math.Floor(y + 0.5) : y;
                            m_mtx = Matrix4.Identity;
                            m_mtx.Scale(width / scale_x, 1, MatrixOrder.Append);
                            m_mtx.Multiply(Matrix4.CreateSkewing(-italic / 3, 0), MatrixOrder.Append);
                            m_mtx.Translate(start_x + x / scale_x, ty, MatrixOrder.Append);
                            m_TransformedFlattenGlyph.Transform = m_mtx;

                            Geometry geometry;
                            if (fWeightedTransformedFlattenGlyph)
                            {
                                m_WeightedTransformedFlattenGlyph.Modified();
                                geometry = m_WeightedTransformedFlattenGlyph;
                            }
                            else
                            {
                                geometry = m_TransformedFlattenGlyph;
                            }

                            if (fFill)
                            {
                                graphics.FillGeometry(color, geometry);
                            }
                            if (fOutline)
                            {
                                graphics.DrawGeometry(outlineColor, geometry, thickness);
                            }
                        }

                        // increment pen position
                        x += glyph.advance_x + interval * scale_x;
                        y += glyph.advance_y;
                    }

                    text_index++;
                }

                y += height * 1.6;
            }

            return y;
        }


        void Client_Paint(object sender, GUI.PaintEventArgs e)
        {
            Color color = Color.White;
            if (m_InvertCheckBox.IsChecked)
            {
                color = Color.Black;
            }
            e.Graphics.Clear(color);


            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;


            string fontFileName = FontFileName;
            if (string.IsNullOrEmpty(fontFileName))
            {
                return;
            }


            double x = 20;
            double y = 40;
            double k = m_HeightSlider.Value;


            y = DrawText(e.Graphics, fontFileName, Common.ShortText, x, y, k * 12, Color.Black);
            y = DrawText(e.Graphics, fontFileName, Common.ShortText, x, y, k * 16, Color.Green);
            y = DrawText(e.Graphics, fontFileName, Common.ShortText, x, y, k * 20, Color.YellowGreen);
            y = DrawText(e.Graphics, fontFileName, Common.ShortText, x, y, k * 24, Color.Orange);
            y = DrawText(e.Graphics, fontFileName, Common.ShortText, x, y, k * 36, Color.Red);
            y = DrawText(e.Graphics, fontFileName, Common.ShortText, x, y, k * 48, Color.Blue);
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
