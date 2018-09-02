//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/Gwen Complex Demo")]
public class AltGUIGwenComplexDemo_UnityUI : AltGUIGwenComplexDemo
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
		if (host == null)
		{
			return;
		}

		//	requires for ExtBrush demo
		//host.RenderType = Alt.Sketch.Rendering.RenderType.Hardware;
		
		host.Child = new Alt.GUI.Demo.AltGUIGwenComplexDemoControl();
	}
}
