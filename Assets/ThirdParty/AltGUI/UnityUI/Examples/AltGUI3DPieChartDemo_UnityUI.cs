//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/3DPie Chart Demo")]
public class AltGUI3DPieChartDemo_UnityUI : AltGUI3DPieChartDemo
{	
	AltGUI3DPieChart PieChart
	{
		get
		{
			return gameObject.GetComponent<AltGUI3DPieChart>();
		}
	}


	// Use this for initialization
	void Start ()
	{
		AltGUI3DPieChart pieChart = PieChart;
		if (pieChart == null)
		{
			return;
		}

		Start(pieChart.PieChartControl);
	}
}
