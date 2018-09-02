//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/HtmlLabel Demo")]
public class AltGUIHtmlLabelDemo_UnityUI : AltGUIHtmlLabelDemo
{	
	AltGUIHtmlLabel HtmlLabel
	{
		get
		{
			return gameObject.GetComponent<AltGUIHtmlLabel>();
		}
	}


	// Use this for initialization
	void Start ()
	{
		AltGUIHtmlLabel htmlLabel = HtmlLabel;
		if (htmlLabel == null)
		{
			return;
		}

		htmlLabel.Text = HTMLText;

		//	Just because source html has black text as main - and it seems not good
		if (htmlLabel.HtmlLabel != null)
		{
			htmlLabel.HtmlLabel.BackColor = Alt.Sketch.Color.FromArgb(128, Alt.Sketch.Color.White);
		}
	}
}
