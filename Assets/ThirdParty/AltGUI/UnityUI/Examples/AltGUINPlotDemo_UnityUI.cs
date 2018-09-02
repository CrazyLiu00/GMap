//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/NPlot Demo")]
public class AltGUINPlotDemo_UnityUI : AltGUINPlotDemo
{	
	AltGUINPlot NPlot
	{
		get
		{
			return gameObject.GetComponent<AltGUINPlot>();
		}
	}


	// Use this for initialization
	void Start ()
	{
		AltGUINPlot nPlot = NPlot;
		if (nPlot == null)
		{
			return;
		}

		Start(nPlot.PlotSurface);
	}
}
