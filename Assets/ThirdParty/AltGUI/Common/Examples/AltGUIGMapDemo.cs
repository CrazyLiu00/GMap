//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;

using Alt.Sketch;


[AddComponentMenu("AltGUI/Examples/Common/GMap Demo")]
public abstract class AltGUIGMapDemo : MonoBehaviour
{
	protected GMap.NET.PointLatLng StartPosition
	{
		get
		{
			return new GMap.NET.PointLatLng(54.6961334816182, 25.2985095977783);
		}
	}
}
