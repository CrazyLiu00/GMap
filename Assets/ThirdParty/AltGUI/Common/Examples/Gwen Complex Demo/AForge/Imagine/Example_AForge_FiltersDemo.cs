// Image Processing filters demo
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
using AForge.Imaging.Filters;
using AForge.Imaging.Textures;


namespace Alt.GUI.Demo
{
    class Example_AForge_FiltersDemo : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 0;
                int y = 0;

                if (m_Menu != null)
                {
                    y = m_Menu.Height;
                }

                return new SizeI(x, y);
            }
        }


        string Description
        {
            get
            {
				return "AltGUI AForge.NET Image Processing Filters Demo";
            }
        }


        Bitmap Screenshot
        {
            get
            {
                return Example_NotAvailable_ScreenShot.LoadImage("Example_AForge_FiltersDemo.jpg");
            }
        }


		Alt.GUI.Temporary.Gwen.Control.PictureBox pictureBox;

		Alt.GUI.Temporary.Gwen.Control.MenuStrip m_Menu;
		Alt.GUI.Temporary.Gwen.Control.MenuItem sizeItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem normalSizeItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem stretchedSizeItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem centeredSizeItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem filtersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem noneFiltersItem;
#if !SILVERLIGHT
		Alt.GUI.Temporary.Gwen.Control.MenuItem sepiaFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem invertFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem rotateChannelFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem grayscaleFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem colorFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem hueModifierFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem saturationAdjustingFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem brightnessAdjustingFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem contrastAdjustingFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem hslFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem yCbCrLinearFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem yCbCrFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem thresholdFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem floydFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem orderedDitheringFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem convolutionFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem sharpenFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem differenceEdgesFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem homogenityEdgesFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem sobelEdgesFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem rgbLinearFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem jitterFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem oilFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem gaussianFiltersItem;
		Alt.GUI.Temporary.Gwen.Control.MenuItem textureFiltersItem;
#endif


        Bitmap sourceImage;
        Bitmap filteredImage;


        public Example_AForge_FiltersDemo(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


#if SILVERLIGHT || UNITY_WEBPLAYER
			Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            label.AutoSizeToContents = true;
            label.Text = //Description + "\n" + "(This example is not available in this Demo, please download SDK)";
				"THIS EXAMPLE IS NOT AVAILABLE IN THIS DEMO,\nPLEASE DOWNLOAD AltGUI SDK";
            label.TextColor = Color.Orange * 1.2;
            label.Dock = Pos.Top;
			label.Margin = new Alt.GUI.Temporary.Gwen.Margin(0, 0, 0, 5);
            label.Font = Example_NotAvailable_ScreenShot.Font;
#endif


            //  GUI
            {
				m_Menu = new Alt.GUI.Temporary.Gwen.Control.MenuStrip(this);
                {
					Alt.GUI.Temporary.Gwen.Control.MenuItem root = filtersItem = m_Menu.AddItem("Filters");
                    {
                        (noneFiltersItem = root.Menu.AddItem("None")).SetAction(noneFiltersItem_Click);

                        root.Menu.AddDivider();

#if !SILVERLIGHT
                        //TEMP  (grayscaleFiltersItem = root.Menu.AddItem("Grayscale")).SetAction(grayscaleFiltersItem_Click);
                        (sepiaFiltersItem = root.Menu.AddItem("Sepia")).SetAction(sepiaFiltersItem_Click);
                        (invertFiltersItem = root.Menu.AddItem("Invert")).SetAction(invertFiltersItem_Click);
                        (rotateChannelFiltersItem = root.Menu.AddItem("Rotate channel")).SetAction(rotateChannelFiltersItem_Click);
                        (colorFiltersItem = root.Menu.AddItem("Color filtering")).SetAction(colorFiltersItem_Click);
                        (rgbLinearFiltersItem = root.Menu.AddItem("Levels linear correction")).SetAction(rgbLinearFiltersItem_Click);

                        root.Menu.AddDivider();

                        (hueModifierFiltersItem = root.Menu.AddItem("Hue modifier")).SetAction(hueModifierFiltersItem_Click);
                        (saturationAdjustingFiltersItem = root.Menu.AddItem("Saturation adjusting")).SetAction(saturationAdjustingFiltersItem_Click);
                        (brightnessAdjustingFiltersItem = root.Menu.AddItem("Brightness adjusting")).SetAction(brightnessAdjustingFiltersItem_Click);
                        (contrastAdjustingFiltersItem = root.Menu.AddItem("Contrast adjusting")).SetAction(contrastAdjustingFiltersItem_Click);
                        (hslFiltersItem = root.Menu.AddItem("HSL filtering")).SetAction(hslFiltersItem_Click);

                        root.Menu.AddDivider();

                        //TEMP  (yCbCrLinearFiltersItem = root.Menu.AddItem("YCbCr linear correction")).SetAction(yCbCrLinearFiltersItem_Click);
                        (yCbCrFiltersItem = root.Menu.AddItem("YCbCr filtering")).SetAction(yCbCrFiltersItem_Click);

                        root.Menu.AddDivider();

                        //TEMP  (thresholdFiltersItem = root.Menu.AddItem("Threshold binarization")).SetAction(thresholdFiltersItem_Click);
                        //TEMP  (floydFiltersItem = root.Menu.AddItem("Floyd-Steinberg dithering")).SetAction(floydFiltersItem_Click);
                        //TEMP  (orderedDitheringFiltersItem = root.Menu.AddItem("Ordered dithering")).SetAction(orderedDitheringFiltersItem_Click);

                        //TEMP  root.Menu.AddDivider();

                        (convolutionFiltersItem = root.Menu.AddItem("Convolution")).SetAction(convolutionFiltersItem_Click);
                        (sharpenFiltersItem = root.Menu.AddItem("Sharpen")).SetAction(sharpenFiltersItem_Click);
                        (gaussianFiltersItem = root.Menu.AddItem("Gaussian blur")).SetAction(gaussianFiltersItem_Click);

                        root.Menu.AddDivider();

                        //TEMP  (differenceEdgesFiltersItem = root.Menu.AddItem("Difference edge detector")).SetAction(differenceEdgesFiltersItem_Click);
                        //TEMP  (homogenityEdgesFiltersItem = root.Menu.AddItem("Homogenity edge detector")).SetAction(homogenityEdgesFiltersItem_Click);
                        //TEMP  (sobelEdgesFiltersItem = root.Menu.AddItem("Sobel edge detector")).SetAction(sobelEdgesFiltersItem_Click);

                        //TEMP  root.Menu.AddDivider();

                        (jitterFiltersItem = root.Menu.AddItem("Jitter")).SetAction(jitterFiltersItem_Click);
                        (oilFiltersItem = root.Menu.AddItem("Oil Painting")).SetAction(oilFiltersItem_Click);
                        (textureFiltersItem = root.Menu.AddItem("Texture")).SetAction(textureFiltersItem_Click);
#else
                        root.Menu.AddItem("Image filters are not available in Silverlight yet");
#endif
                    }


                    root = sizeItem = m_Menu.AddItem("Size mode");
                    {
                        sizeItem.Click += new EventHandler(sizeItem_Popup);

                        (normalSizeItem = root.Menu.AddItem("Normal")).SetAction(normalSizeItem_Click);
                        (stretchedSizeItem = root.Menu.AddItem("Stretched")).SetAction(stretchedSizeItem_Click);
                        (centeredSizeItem = root.Menu.AddItem("Centered")).SetAction(centeredSizeItem_Click);
                    }
                }


				pictureBox = new Alt.GUI.Temporary.Gwen.Control.PictureBox(this);
                pictureBox.DrawBorder = true;
                pictureBox.BorderColor = Color.DodgerBlue;
                pictureBox.Dock = Pos.Fill;
            }

            // set default size mode of picture box
			pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            LoadImage();
        }



        void LoadImage()
        {
            try
            {
                // load image
                sourceImage = Bitmap.FromFile("AltData/test.jpg");

                // check pixel format
                if (//TEMP  (sourceImage.PixelFormat == PixelFormat.Format16bppGrayScale) ||
                     (Bitmap.GetPixelFormatSize(sourceImage.PixelFormat) > 32))
                {
					new Alt.GUI.Temporary.Gwen.Control.MessageBox(this, "The demo application supports only color images.", "Error");
                    // free image
                    sourceImage.Dispose();
                    sourceImage = null;
                }
                else
                {
                    // make sure the image has 24 bpp format
                    if (sourceImage.PixelFormat != PixelFormat.Format24bppRgb)
                    {
                        Bitmap temp = global::AForge.Imaging.Image.Clone(sourceImage, PixelFormat.Format24bppRgb);
                        sourceImage.Dispose();
                        sourceImage = temp;
                    }
                }

                ClearCurrentImage();

                // display image
                pictureBox.Image = sourceImage;
                noneFiltersItem.IsChecked = true;

                // enable filters menu
                filtersItem.Enabled = (sourceImage != null);
            }
            catch
            {
				new Alt.GUI.Temporary.Gwen.Control.MessageBox(this, "Failed loading the image", "Error");
            }
        }



        // On Size mode->Normal menu item
        void normalSizeItem_Click(Base control)
        {
			pictureBox.SizeMode = PictureBoxSizeMode.Normal;
        }


        // On Size mode->Stretched menu item
		void stretchedSizeItem_Click(Alt.GUI.Temporary.Gwen.Control.Base control)
        {
			pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }


        // On Size mode->Centered size menu item
        void centeredSizeItem_Click(Base control)
        {
			pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
        }


        // On Size menu item popup
        void sizeItem_Popup(object sender, EventArgs e)
        {
			normalSizeItem.IsChecked = (pictureBox.SizeMode == PictureBoxSizeMode.Normal);
			stretchedSizeItem.IsChecked = (pictureBox.SizeMode == PictureBoxSizeMode.StretchImage);
			centeredSizeItem.IsChecked = (pictureBox.SizeMode == PictureBoxSizeMode.CenterImage);
        }


        // Clear current image in picture box
        void ClearCurrentImage()
        {
            // clear current image from picture box
            pictureBox.Image = null;
            // free current image
            if ((noneFiltersItem.IsChecked == false) && (filteredImage != null))
            {
                filteredImage.Dispose();
                filteredImage = null;
            }

            // uncheck all menu items
            foreach (Base node in filtersItem.Menu.Children)
            {
				Alt.GUI.Temporary.Gwen.Control.MenuItem item = node as Alt.GUI.Temporary.Gwen.Control.MenuItem;
                if (item != null)
                {
                    item.IsChecked = false;
                }
            }
        }


        // On Filters->None item
        void noneFiltersItem_Click(Base control)
        {
            ClearCurrentImage();
            // display source image
            pictureBox.Image = sourceImage;
            noneFiltersItem.IsChecked = true;
        }


#if !SILVERLIGHT
        // Apply filter to the source image and show the filtered image
        void ApplyFilter(IFilter filter)
        {
#if !UNITY_WEBPLAYER
            ClearCurrentImage();
            // apply filter
            filteredImage = filter.Apply(sourceImage);
            // display filtered image
            pictureBox.Image = filteredImage;
#endif
        }


        /*TEMP
        // On Filters->Grayscale item
        void grayscaleFiltersItem_Click(Base control)
        {
            ApplyFilter(Grayscale.CommonAlgorithms.BT709);
            grayscaleFiltersItem.IsChecked = true;
        }*/


        // On Filters->Sepia item
        void sepiaFiltersItem_Click(Base control)
        {
            ApplyFilter(new Sepia());
            sepiaFiltersItem.IsChecked = true;
        }


        // On Filters->Invert item
        void invertFiltersItem_Click(Base control)
        {
            ApplyFilter(new Invert());
            invertFiltersItem.IsChecked = true;
        }


        // On Filters->Rotate Channels item
        void rotateChannelFiltersItem_Click(Base control)
        {
            ApplyFilter(new RotateChannels());
            rotateChannelFiltersItem.IsChecked = true;
        }


        // On Filters->Color filtering
        void colorFiltersItem_Click(Base control)
        {
            ApplyFilter(new ColorFiltering(new IntRange(25, 230), new IntRange(25, 230), new IntRange(25, 230)));
            colorFiltersItem.IsChecked = true;
        }


        // On Filters->Hue modifier
        void hueModifierFiltersItem_Click(Base control)
        {
            ApplyFilter(new HueModifier(50));
            hueModifierFiltersItem.IsChecked = true;
        }


        // On Filters->Saturation adjusting
        void saturationAdjustingFiltersItem_Click(Base control)
        {
            ApplyFilter(new SaturationCorrection(0.15f));
            saturationAdjustingFiltersItem.IsChecked = true;
        }


        // On Filters->Brightness adjusting
        void brightnessAdjustingFiltersItem_Click(Base control)
        {
            ApplyFilter(new BrightnessCorrection());
            brightnessAdjustingFiltersItem.IsChecked = true;
        }


        // On Filters->Contrast adjusting
        void contrastAdjustingFiltersItem_Click(Base control)
        {
            ApplyFilter(new ContrastCorrection());
            contrastAdjustingFiltersItem.IsChecked = true;
        }


        // On Filters->HSL filtering
        void hslFiltersItem_Click(Base control)
        {
            ApplyFilter(new HSLFiltering(new IntRange(330, 30), new Range(0, 1), new Range(0, 1)));
            hslFiltersItem.IsChecked = true;
        }


        /*TEMP
        // On Filters->YCbCr filtering
        void yCbCrLinearFiltersItem_Click(Base control)
        {
            YCbCrLinear filter = new YCbCrLinear();

            filter.InCb = new Range(-0.3f, 0.3f);

            ApplyFilter(filter);
            yCbCrLinearFiltersItem.IsChecked = true;
        }*/


        // On Filters->YCbCr filtering
        void yCbCrFiltersItem_Click(Base control)
        {
            ApplyFilter(new YCbCrFiltering(new Range(0.2f, 0.9f), new Range(-0.3f, 0.3f), new Range(-0.3f, 0.3f)));
            yCbCrFiltersItem.IsChecked = true;
        }


        /*TEMP
        // On Filters->Threshold binarization
        void thresholdFiltersItem_Click(Base control)
        {
            // save original image
            Bitmap originalImage = sourceImage;
            // get grayscale image
            sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(sourceImage);
            // apply threshold filter
            ApplyFilter(new Threshold());
            // delete grayscale image and restore original
            sourceImage.Dispose();
            sourceImage = originalImage;

            thresholdFiltersItem.IsChecked = true;
        }


        // On Filters->Floyd-Steinberg dithering
        void floydFiltersItem_Click(Base control)
        {
            // save original image
            Bitmap originalImage = sourceImage;
            // get grayscale image
            sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(sourceImage);
            // apply threshold filter
            ApplyFilter(new FloydSteinbergDithering());
            // delete grayscale image and restore original
            sourceImage.Dispose();
            sourceImage = originalImage;

            floydFiltersItem.IsChecked = true;
        }


        // On Filters->Ordered dithering
        void orderedDitheringFiltersItem_Click(Base control)
        {
            // save original image
            Bitmap originalImage = sourceImage;
            // get grayscale image
            sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(sourceImage);
            // apply threshold filter
            ApplyFilter(new OrderedDithering());
            // delete grayscale image and restore original
            sourceImage.Dispose();
            sourceImage = originalImage;

            orderedDitheringFiltersItem.IsChecked = true;
        }*/


        // On Filters->Correlation
        void convolutionFiltersItem_Click(Base control)
        {
            ApplyFilter(new Convolution(new int[,] {
								{ 1, 2, 3, 2, 1 },
								{ 2, 4, 5, 4, 2 },
								{ 3, 5, 6, 5, 3 },
								{ 2, 4, 5, 4, 2 },
								{ 1, 2, 3, 2, 1 } }));
            convolutionFiltersItem.IsChecked = true;
        }


        // On Filters->Sharpen
        void sharpenFiltersItem_Click(Base control)
        {
            ApplyFilter(new Sharpen());
            sharpenFiltersItem.IsChecked = true;
        }


        /*TEMP
        // On Filters->Difference edge detector
        void differenceEdgesFiltersItem_Click(Base control)
        {
            // save original image
            Bitmap originalImage = sourceImage;
            // get grayscale image
            sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(sourceImage);
            // apply edge filter
            ApplyFilter(new DifferenceEdgeDetector());
            // delete grayscale image and restore original
            sourceImage.Dispose();
            sourceImage = originalImage;

            differenceEdgesFiltersItem.IsChecked = true;
        }


        // On Filters->Homogenity edge detector
        void homogenityEdgesFiltersItem_Click(Base control)
        {
            // save original image
            Bitmap originalImage = sourceImage;
            // get grayscale image
            sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(sourceImage);
            // apply edge filter
            ApplyFilter(new HomogenityEdgeDetector());
            // delete grayscale image and restore original
            sourceImage.Dispose();
            sourceImage = originalImage;

            homogenityEdgesFiltersItem.IsChecked = true;
        }


        // On Filters->Sobel edge detector
        void sobelEdgesFiltersItem_Click(Base control)
        {
            // save original image
            Bitmap originalImage = sourceImage;
            // get grayscale image
            sourceImage = Grayscale.CommonAlgorithms.RMY.Apply(sourceImage);
            // apply edge filter
            ApplyFilter(new SobelEdgeDetector());
            // delete grayscale image and restore original
            sourceImage.Dispose();
            sourceImage = originalImage;

            sobelEdgesFiltersItem.IsChecked = true;
        }*/


        // On Filters->Levels Linear Correction
        void rgbLinearFiltersItem_Click(Base control)
        {
            LevelsLinear filter = new LevelsLinear();

            filter.InRed = new IntRange(30, 230);
            filter.InGreen = new IntRange(50, 240);
            filter.InBlue = new IntRange(10, 210);

            ApplyFilter(filter);
            rgbLinearFiltersItem.IsChecked = true;
        }


        // On Filters->Jitter
        void jitterFiltersItem_Click(Base control)
        {
            ApplyFilter(new Jitter());
            jitterFiltersItem.IsChecked = true;
        }


        // On Filters->Oil Painting
        void oilFiltersItem_Click(Base control)
        {
            ApplyFilter(new OilPainting());
            oilFiltersItem.IsChecked = true;
        }


        // On Filters->Gaussin blur
        void gaussianFiltersItem_Click(Base control)
        {
            ApplyFilter(new GaussianBlur(2.0, 7));
            gaussianFiltersItem.IsChecked = true;
        }


        // On Filters->Texture
        void textureFiltersItem_Click(Base control)
        {
            ApplyFilter(new Texturer(new TextileTexture(), 1.0, 0.8));
            textureFiltersItem.IsChecked = true;
        }
#endif
    }
}
