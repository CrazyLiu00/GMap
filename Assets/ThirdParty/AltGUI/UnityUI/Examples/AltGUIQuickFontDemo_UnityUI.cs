//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/QuickFont Demo")]
public class AltGUIQuickFontDemo_UnityUI : AltGUIQuickFontDemo
{	
	AltGUIQuickFont QuickFont
	{
		get
		{
			return gameObject.GetComponent<AltGUIQuickFont>();
		}
	}

	
	protected override Alt.GUI.QuickFont.QuickFontControl QuickFontControl
	{
		get
		{
			AltGUIQuickFont quickFont = QuickFont;
			if (quickFont == null)
			{
				return null;
			}
			
			return quickFont.QuickFontControl;
		}
	}
	
	protected override double ClientWidth
	{
		get
		{
			AltGUIQuickFont quickFont = QuickFont;
			if (quickFont == null)
			{
				return 1;
			}
			
			return quickFont.ClientWidth;
		}
	}

	protected override double ClientHeight
	{
		get
		{
			AltGUIQuickFont quickFont = QuickFont;
			if (quickFont == null)
			{
				return 1;
			}

			return quickFont.ClientHeight;
		}
	}


	// Use this for initialization
	protected override void Start ()
	{
		AltGUIQuickFont quickFont = QuickFont;
		if (quickFont == null)
		{
			return;
		}

		quickFont.onPaint.AddListener(QuickFont_onPaint);
		quickFont.onKeyDown.AddListener(QuickFont_onKeyDown);

		/*example how to change background
		if (quickFont.QuickFontControl != null)
		{
			string bgFileName = "AltData/BG.jpgd";
			if (Alt.IO.VirtualFile.Exists(bgFileName))
			{
				quickFont.QuickFontControl.Background = new TextureBrush(Bitmap.FromFile(bgFileName));
			}
			else
			{
				quickFont.QuickFontControl.BackColor = Alt.Sketch.Color.DodgerBlue;
			}
		}*/

		base.Start();
	}
}
