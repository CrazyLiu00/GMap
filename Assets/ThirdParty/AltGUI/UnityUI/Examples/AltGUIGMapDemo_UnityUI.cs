//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/GMap Demo")]
public class AltGUIGMapDemo_UnityUI : AltGUIGMapDemo
{	
	AltGUIGMap GMap
	{
		get
		{
			return gameObject.GetComponent<AltGUIGMap>();
		}
	}


	// Use this for initialization
	void Start ()
	{
		AltGUIGMap map = GMap;
		if (map != null)
		{
			return;
		}

		map.Position = StartPosition;
	}
}
