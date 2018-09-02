//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System.Collections;

using UnityEngine;


[AddComponentMenu("AltGUI/Native Demo")]
public class AltGUI_Native_Demo : Alt.Integration.Unity.AltGUIScreenMonoBehaviour
{
	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();

		Child = new Alt.GUI.Demo.AltGUIGwenComplexDemoControl();
	}
}
