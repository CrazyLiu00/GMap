//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/PictureBox Demo")]
public class AltGUIPictureBoxDemo_UnityUI : AltGUIPictureBoxDemo
{	
	AltGUIPictureBox PictureBox
	{
		get
		{
			return gameObject.GetComponent<AltGUIPictureBox>();
		}
	}


	protected override Alt.Sketch.ImageSource Image
	{
		set
		{
			AltGUIPictureBox pictureBox = PictureBox;
			if (pictureBox == null)
			{
				return;
			}
			
			pictureBox.Image = value;
		}
	}
	
	protected override Alt.GUI.PictureBoxSizeMode SizeMode
	{
		get
		{
			AltGUIPictureBox pictureBox = PictureBox;
			if (pictureBox == null)
			{
				return Alt.GUI.PictureBoxSizeMode.Normal;
			}
			
			return pictureBox.SizeMode;
		}
		set
		{
			AltGUIPictureBox pictureBox = PictureBox;
			if (pictureBox == null)
			{
				return;
			}
			
			pictureBox.SizeMode = value;
		}
	}
}
