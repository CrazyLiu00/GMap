//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.NETType;
using Alt.Sketch;
using Alt.Sketch.Geometries;
using Alt.Sketch.Geometries.Reformers;
using Alt.Sketch.Geometries.Transformers;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;

using Alt.GUI.Demo.FontAtlas;


namespace Alt.GUI.Demo
{
    class Example_AltNETType_FontAtlas : Example_AltNETType_Base
    {
        Bitmap m_AtlasBitmap;


		Alt.GUI.Temporary.Gwen.Control.HorizontalSlider m_HeightSlider;
		Alt.GUI.Temporary.Gwen.Control.Label m_HeightSliderLabel;
        const string m_HeightSliderLabelPrefix = "Font Height = ";

        Base m_Client;


        public Example_AltNETType_FontAtlas(Base parent)
            : base(parent)
        {
            //  Height
            m_HeightSlider = new HorizontalSlider(this);
            m_HeightSlider.Dock = Pos.Bottom;
            m_HeightSlider.SetRange(8, 72);
            m_HeightSlider.NotchCount = (int)(m_HeightSlider.Max - m_HeightSlider.Min);
            m_HeightSlider.SnapToNotches = true;
            m_HeightSlider.Height = 20;
            m_HeightSlider.ValueChanged += new GwenEventHandler(control_ValueChanged);

			m_HeightSliderLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_HeightSliderLabel.AutoSizeToContents = true;
            m_HeightSliderLabel.Dock = Pos.Bottom;
            m_HeightSliderLabel.Margin = new Margin(0, 7, 0, 0);

            m_HeightSlider.Value = 72;


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


            SelectFontFamily("Open Sans");
            SelectFontStyle(FontStyle.Italic);


            //  update labels
            control_ValueChanged(null);
        }


        void control_ValueChanged(Base control)
        {
            if (m_Client != null)
            {
                m_HeightSliderLabel.Text = m_HeightSliderLabelPrefix + m_HeightSlider.Value.ToString("F2").Replace(',', '.');

                ClientRefresh();
            }
        }


        void Client_Paint(object sender, GUI.PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            Color color = Color.WhiteSmoke;
            graphics.Clear(color);

            if (m_AtlasBitmap != null)
            {
                int x = (m_Client.ClientWidth - m_AtlasBitmap.PixelWidth) / 2;
                int y = (m_Client.ClientHeight - m_AtlasBitmap.PixelHeight) / 2;
                x = Math.Min(x, y);
                x = 15;

                RectI rect = new RectI(x - 1, y - 1, m_AtlasBitmap.PixelSize + SizeI.One);
                graphics.FillRectangle(Color.White, rect);

                graphics.DrawImage(m_AtlasBitmap, x, y);

                graphics.DrawRectangle(Color.DodgerBlue * 0.6, rect);

                x = x * 2 + m_AtlasBitmap.PixelWidth;
                y = m_Client.ClientHeight / 2;
                m_FontAtlas.GetFont(0).DrawString(graphics, x, y - 170, "Font 1");
                m_FontAtlas.GetFont(1).DrawString(graphics, x, y - 70, "Font 2");
                m_FontAtlas.GetFont(2).DrawString(graphics, x, y - 10, "Font 3");
                m_FontAtlas.GetFont(3).DrawString(graphics, x, y + 50, "Font 4");
                m_FontAtlas.GetFont(4).DrawString(graphics, x, y + 100, "Font 5");
            }


            Font font = new Font("Times New Roman", 36);

            string s = "Texture Atlas";
            SizeI size = graphics.MeasureString(s, font).ToSizeI();
            graphics.DrawString(s, font, Brushes.Red, (m_Client.ClientSize - size).ToPointI() - new Point(10, 3));
        }


        protected override void ClientRefresh()
        {
            if (m_Client != null)
            {
                CreateAtlas();

                m_Client.Refresh();
            }
        }



        FontAtlasManager m_FontAtlas;
        string szLetters = " !\"#&'()*,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ\\_abcdefghijklmnopqrstuvwxyzБбЙйНнСсУуЪъ";

        void CreateAtlas()
        {
            string fontFileName = FontFileName;
            if (string.IsNullOrEmpty(fontFileName))
            {
                return;
            }

            m_FontAtlas = new FontAtlasManager();

            m_FontAtlas.AddFont(fontFileName, (int)m_HeightSlider.Value, szLetters);
            m_FontAtlas.AddFont(fontFileName, 36, szLetters);
            m_FontAtlas.AddFont(fontFileName, 24, szLetters);
            m_FontAtlas.AddFont(fontFileName, 18, szLetters);
            m_FontAtlas.AddFont(fontFileName, 14, szLetters);
            
            m_FontAtlas.CreateAtlas();
            m_AtlasBitmap = m_FontAtlas.Bitmap;
        }
    }
}
