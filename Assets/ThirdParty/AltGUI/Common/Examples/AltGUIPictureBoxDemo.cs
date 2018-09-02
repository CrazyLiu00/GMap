//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;

using System.Collections;


[AddComponentMenu("AltGUI/Examples/Common/PictureBox Demo")]
public abstract class AltGUIPictureBoxDemo : MonoBehaviour
{	
	Alt.GUI.Timer m_Timer;


	protected abstract Alt.Sketch.ImageSource Image
	{
		set;
	}
	
	protected abstract Alt.GUI.PictureBoxSizeMode SizeMode
	{
		get;
		set;
	}
	

	void Start()
	{
		Image = Alt.Sketch.Bitmap.FromFile("AltData/corazon.gif");
		SizeMode = Alt.GUI.PictureBoxSizeMode.CenterImage;

		m_Timer = new Alt.GUI.Timer(1500);
		m_Timer.Tick += Timer_Tick;
		m_Timer.Start();
	}


	void Timer_Tick(object sender, System.EventArgs e)
	{
		Alt.GUI.PictureBoxSizeMode mode = SizeMode;
		switch (mode)
		{
		case Alt.GUI.PictureBoxSizeMode.Normal:
		{
			mode = Alt.GUI.PictureBoxSizeMode.StretchImage;
			break;
		}
		
		case Alt.GUI.PictureBoxSizeMode.StretchImage:
		{
			mode = Alt.GUI.PictureBoxSizeMode.AutoSize;
			break;
		}
		
		case Alt.GUI.PictureBoxSizeMode.AutoSize:
		{
			mode = Alt.GUI.PictureBoxSizeMode.CenterImage;
			break;
		}
		
		case Alt.GUI.PictureBoxSizeMode.CenterImage:
		{
			mode = Alt.GUI.PictureBoxSizeMode.Zoom;
			break;
		}
		
		case Alt.GUI.PictureBoxSizeMode.Zoom:
		{
			mode = Alt.GUI.PictureBoxSizeMode.Normal;
			break;
		}
		}

		SizeMode = mode;
	}
}
