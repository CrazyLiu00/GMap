//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using UnityEngine;
using UnityEngine.UI;

using Alt.GUI.ZedGraph;
using Alt.Sketch;


[AddComponentMenu("AltGUI/Examples/Common/ZedGraph Demo")]
public abstract class AltGUIZedGraphDemo : MonoBehaviour
{	
	protected void Start (Alt.GUI.ZedGraph.Temporary.Gwen.ZedGraphControl zedGraphControl)
	{
		if (zedGraphControl == null)
		{
			return;
		}

		GraphPane myPane = zedGraphControl.GraphPane;
		
		// Set the title and axis labels
		myPane.Title.Text = "Demo Graph";
		myPane.XAxis.Title.Text = "My X Axis";
		myPane.YAxis.Title.Text = "My Y Axis";
		
		// Make up some data arrays based on the Sine function
		PointPairList list1 = new PointPairList();
		PointPairList list2 = new PointPairList();
		for (int i = 0; i < 36; i++)
		{
			double x = (double)i + 5;
			double y1 = 1.5 + Math.Sin((double)i * 0.2);
			double y2 = 3.0 * (1.5 + Math.Sin((double)i * 0.2));
			list1.Add(x, y1);
			list2.Add(x, y2);
		}
		
		// Generate a red curve with diamond
		// symbols, and "Porsche" in the legend
		myPane.AddCurve("Porsche", list1, Alt.Sketch.Color.Red, SymbolType.Diamond);
		
		// Generate a blue curve with circle
		// symbols, and "Piper" in the legend
		myPane.AddCurve("Piper", list2, Alt.Sketch.Color.Blue, SymbolType.Circle);
		
		zedGraphControl.AxisChange();
	}
}
