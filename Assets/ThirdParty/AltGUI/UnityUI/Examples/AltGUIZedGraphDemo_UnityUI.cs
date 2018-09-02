//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/ZedGraph Demo")]
public class AltGUIZedGraphDemo_UnityUI : AltGUIZedGraphDemo
{	
	AltGUIZedGraph ZedGraph
	{
		get
		{
			return gameObject.GetComponent<AltGUIZedGraph>();
		}
	}


	// Use this for initialization
	void Start ()
	{
		AltGUIZedGraph zedGraph = ZedGraph;
		if (zedGraph == null)
		{
			return;
		}
	
		Start(zedGraph.ZedGraphControl);
	}
}
