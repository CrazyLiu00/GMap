// Textures demo
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright � AForge.NET, 2006-2011
// contacts@aforgenet.com

using System;
using System.Collections.Generic;
using System.Text;

using Alt.ComponentModel;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;
using Alt.Threading;

using AForge.Imaging;
using AForge.Imaging.Textures;


namespace Alt.GUI.Demo
{
    class Example_AForge_TexturesDemo : Example__Base
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


		Alt.GUI.Temporary.Gwen.Control.Base m_TopPanel;
		Alt.GUI.Temporary.Gwen.Control.Label label1;
		Alt.GUI.Temporary.Gwen.Control.ComboBox texturesCombo;
		Alt.GUI.Temporary.Gwen.Control.PictureBox pictureBox;
		Alt.GUI.Temporary.Gwen.Control.Button regenerateButton;

        ITextureGenerator textureGenerator = null;

        
        public Example_AForge_TexturesDemo(Base parent)
            : base(parent)
        {
            //  GUI
            {
				m_TopPanel = new Alt.GUI.Temporary.Gwen.Control.Base(this);
                {
                    m_TopPanel.Dock = Pos.Top;
                    m_TopPanel.Height = 20;
                    m_TopPanel.Margin = new Margin(0, 0, 0, 10);


					label1 = new Alt.GUI.Temporary.Gwen.Control.Label(m_TopPanel);
					texturesCombo = new Alt.GUI.Temporary.Gwen.Control.ComboBox(m_TopPanel);
					regenerateButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_TopPanel);


                    label1.AutoSizeToContents = true;
                    label1.Dock = Pos.Left;
                    label1.Text = "Texture:";
                    label1.Margin = new Margin(0, 2, 0, 0);


                    texturesCombo.AddItem("Clouds").UserData = 0;
                    texturesCombo.AddItem("Marble").UserData = 1;
                    texturesCombo.AddItem("Wood").UserData = 2;
                    texturesCombo.AddItem("Labyrinth").UserData = 3;
                    texturesCombo.AddItem("Textile").UserData = 4;
                    texturesCombo.Dock = Pos.Left;
                    texturesCombo.ItemSelected += texturesCombo_SelectedIndexChanged;
                    texturesCombo.Margin = new Margin(5, 0, 0, 0);


                    regenerateButton.Dock = Pos.Left;
                    regenerateButton.Text = "Regenerate";
                    regenerateButton.Click += regenerateButton_Click;
                    regenerateButton.Margin = new Margin(10, 0, 0, 0);
                    regenerateButton.NormalTextColor = Color.Green;
                }


				pictureBox = new Alt.GUI.Temporary.Gwen.Control.PictureBox(this);
                pictureBox.Dock = Pos.Fill;
                pictureBox.DrawBorder = true;
                pictureBox.BorderColor = Color.DodgerBlue;
            }
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            texturesCombo_SelectedIndexChanged(texturesCombo);
        }



        // Texture changed
        void texturesCombo_SelectedIndexChanged(Base control)
        {
            // create texture generator
            switch ((int)texturesCombo.SelectedItem.UserData)
            {
                case 0:     // clouds
                    textureGenerator = new CloudsTexture();
                    break;
                case 1:     // marble
                    textureGenerator = new MarbleTexture();
                    break;
                case 2:     // wood
                    textureGenerator = new WoodTexture(7);
                    break;
                case 3:     // labyrinth
                    textureGenerator = new LabyrinthTexture();
                    break;
                case 4:     // textile
                    textureGenerator = new TextileTexture();
                    break;
                default:
                    textureGenerator = null;
                    break;
            }

            // show texture
            ShowTexture();
        }


        // Generate and show texture
        void ShowTexture()
        {
            // check generator
            if (textureGenerator == null)
            {
                pictureBox.Image = null;
                return;
            }

            int width = pictureBox.ClientRectangle.Width;
            int height = pictureBox.ClientRectangle.Height;

            // generate texture
            float[,] texture = textureGenerator.Generate(width, height);

            // create bitmap from the texture
            Bitmap image = TextureTools.ToBitmap(texture);

            // show image
            pictureBox.Image = image;
        }


        // Regenerate texture
        void regenerateButton_Click(object sender, EventArgs e)
        {
            if (textureGenerator != null)
            {
                textureGenerator.Reset();
                ShowTexture();
            }
        }
    }
}
