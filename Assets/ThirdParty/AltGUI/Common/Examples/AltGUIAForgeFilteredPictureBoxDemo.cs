//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Examples/Common/AForge Filtered PictureBox Demo")]
public abstract class AltGUIAForgeFilteredPictureBoxDemo : MonoBehaviour
{	
	Alt.GUI.Timer m_Timer;

		
	protected abstract Alt.Sketch.ImageSource Image
	{
		set;
	}
		
	protected abstract Alt.GUI.PictureBoxSizeMode SizeMode
	{
		set;
	}

	protected abstract Alt.GUI.AForge.ImageFilterType ImageFilter
	{
		get;
		set;
	}


	// Use this for initialization
	void Start ()
	{
		Image = Alt.Sketch.Bitmap.FromFile("AltData/test.jpg");
		SizeMode = Alt.GUI.PictureBoxSizeMode.CenterImage;

		m_Timer = new Alt.GUI.Timer(1000);
		m_Timer.Tick += Timer_Tick;
		m_Timer.Start();
	}


	void Timer_Tick(object sender, System.EventArgs e)
	{
		Alt.GUI.AForge.ImageFilterType imageFilter = ImageFilter;
		switch (imageFilter)
		{
		case Alt.GUI.AForge.ImageFilterType.None:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.Sepia;
			break;
		}

		case Alt.GUI.AForge.ImageFilterType.Sepia:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.Invert;
			break;
		}
				
		case Alt.GUI.AForge.ImageFilterType.Invert:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.RotateChannels;
			break;
		}
				
		case Alt.GUI.AForge.ImageFilterType.RotateChannels:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.ColorFiltering;
			break;
		}
				
		case Alt.GUI.AForge.ImageFilterType.ColorFiltering:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.LevelsLinearCorrection;
			break;
		}
				
		case Alt.GUI.AForge.ImageFilterType.LevelsLinearCorrection:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.HueModifier;
			break;
		}
				
				
		case Alt.GUI.AForge.ImageFilterType.HueModifier:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.SaturationAdjusting;
			break;
		}
				
		case Alt.GUI.AForge.ImageFilterType.SaturationAdjusting:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.BrightnessAdjusting;
			break;
		}
				
		case Alt.GUI.AForge.ImageFilterType.BrightnessAdjusting:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.ContrastAdjusting;
			break;
		}
				
		case Alt.GUI.AForge.ImageFilterType.ContrastAdjusting:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.HSLFiltering;
			break;
		}
				
		case Alt.GUI.AForge.ImageFilterType.HSLFiltering:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.YCbCrFiltering;
			break;
		}
				
				
		//TEMP  YCbCr linear correction,

		case Alt.GUI.AForge.ImageFilterType.YCbCrFiltering:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.Convolution;
			break;
		}
				
				
		//TEMP  Threshold binarization,
		//TEMP  Floyd-Steinberg dithering,
		//TEMP  Ordered dithering,
				

		case Alt.GUI.AForge.ImageFilterType.Convolution:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.Sharpen;
			break;
		}
				
		case Alt.GUI.AForge.ImageFilterType.Sharpen:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.GaussianBlur;
			break;
		}
				
		case Alt.GUI.AForge.ImageFilterType.GaussianBlur:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.Jitter;
			break;
		}
				
				
		//TEMP  Difference edge detector,
		//TEMP  Homogenity edge detector,
		//TEMP  Sobel edge detector,
				

		case Alt.GUI.AForge.ImageFilterType.Jitter:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.OilPainting;
			break;
		}
				
		case Alt.GUI.AForge.ImageFilterType.OilPainting:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.Texture;
			break;
		}
				
		case Alt.GUI.AForge.ImageFilterType.Texture:
		{
			imageFilter = Alt.GUI.AForge.ImageFilterType.None;
			break;
		}				
		}

		ImageFilter = imageFilter;
	}
}
