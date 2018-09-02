//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/AltGUI Control Host Demo")]
public class AltGUIControlHostDemo_UnityUI : AltGUIControlHostDemo
{	
	AltGUIControlHost Host
	{
		get
		{
			return gameObject.GetComponent<AltGUIControlHost>();
		}
	}


	// Use this for initialization
	void Start ()
	{
		AltGUIControlHost host = Host;
		if (host != null)
		{
			host.Child = new DemoControl();
		}
	}
}
