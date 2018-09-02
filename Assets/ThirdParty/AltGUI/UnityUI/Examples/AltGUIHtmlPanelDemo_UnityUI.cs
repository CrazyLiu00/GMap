//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/HtmlPanel Demo")]
public class AltGUIHtmlPanelDemo_UnityUI : AltGUIHtmlPanelDemo
{	
	AltGUIHtmlPanel HtmlPanel
	{
		get
		{
			return gameObject.GetComponent<AltGUIHtmlPanel>();
		}
	}


	// Use this for initialization
	void Start ()
	{
		AltGUIHtmlPanel htmlPanel = HtmlPanel;
		if (htmlPanel == null)
		{
			return;
		}

		htmlPanel.Text = HTMLText;
	}
}
