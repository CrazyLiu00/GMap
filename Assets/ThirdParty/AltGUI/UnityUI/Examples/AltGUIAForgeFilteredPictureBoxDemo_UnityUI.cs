//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/AForge Filtered PictureBox Demo")]
public class AltGUIAForgeFilteredPictureBoxDemo_UnityUI : AltGUIAForgeFilteredPictureBoxDemo
{	
	AltGUIAForgeFilteredPictureBox AForgeFilteredPictureBox
	{
		get
		{
			return gameObject.GetComponent<AltGUIAForgeFilteredPictureBox>();
		}
	}


	protected override Alt.Sketch.ImageSource Image
	{
		set
		{
			AltGUIAForgeFilteredPictureBox pictureBox = AForgeFilteredPictureBox;
			if (pictureBox == null)
			{
				return;
			}

			pictureBox.Image = value;
		}
	}
	
	protected override Alt.GUI.PictureBoxSizeMode SizeMode
	{
		set
		{
			AltGUIAForgeFilteredPictureBox pictureBox = AForgeFilteredPictureBox;
			if (pictureBox == null)
			{
				return;
			}

			pictureBox.SizeMode = value;
		}
	}
	
	protected override Alt.GUI.AForge.ImageFilterType ImageFilter
	{
		get
		{
			AltGUIAForgeFilteredPictureBox pictureBox = AForgeFilteredPictureBox;
			if (pictureBox == null)
			{
				return Alt.GUI.AForge.ImageFilterType.None;
			}

			return pictureBox.ImageFilter;
		}
		set
		{
			AltGUIAForgeFilteredPictureBox pictureBox = AForgeFilteredPictureBox;
			if (pictureBox == null)
			{
				return;
			}

			pictureBox.ImageFilter = value;
		}
	}
}
