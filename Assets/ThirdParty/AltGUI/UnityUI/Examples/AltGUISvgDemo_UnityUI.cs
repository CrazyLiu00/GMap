//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/SVG Demo")]
public class AltGUISvgDemo_UnityUI : AltGUISvgDemo
{	
	AltGUISvg SVG
	{
		get
		{
			return gameObject.GetComponent<AltGUISvg>();
		}
	}


	// Use this for initialization
	void Start ()
	{
		AltGUISvg svg = SVG;
		if (svg == null)
		{
			return;
		}

		svg.LoadSVG(FileName);
	}
}
