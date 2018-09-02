//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/Awesomium Demo")]
public class AltGUIAwesomiumDemo_UnityUI : AltGUIAwesomiumDemo
{	
	AltGUIAwesomium Awesomium
	{
		get
		{
			return gameObject.GetComponent<AltGUIAwesomium>();
		}
	}


	// Use this for initialization
	void Start ()
	{
		AltGUIAwesomium awesomium = Awesomium;
		if (awesomium != null)
		{
		}
	}
}
