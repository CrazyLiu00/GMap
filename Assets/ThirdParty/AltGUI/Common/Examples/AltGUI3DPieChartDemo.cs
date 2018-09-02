//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;

using Alt.Collections;


[AddComponentMenu("AltGUI/Examples/Common/3DPie Chart Demo")]
public abstract class AltGUI3DPieChartDemo : MonoBehaviour
{	
	protected void Start (Alt.GUI.PieChart.Temporary.Gwen.PieChartControl pieChartControl)
	{
	 	if (pieChartControl == null)
		{
			return;
		}

		pieChartControl.Values = Values;
		pieChartControl.SliceRelativeDisplacements = Displacements;
		pieChartControl.Colors = Colors;
		pieChartControl.Texts = Texts;
		pieChartControl.ToolTips = ToolTips;

		int margin = 20;
		pieChartControl.LeftMargin = (float)margin;
		pieChartControl.RightMargin = (float)margin;
		pieChartControl.TopMargin = (float)margin * 2;
		pieChartControl.BottomMargin = (float)margin * 2;
		pieChartControl.FitChart = false;//true;
		pieChartControl.SliceRelativeHeight = (float)0.25f;
		pieChartControl.InitialAngle = (float)-30;
		pieChartControl.EdgeLineWidth = (float)3;
		pieChartControl.ShadowStyle = Alt.GUI.PieChart.ShadowStyle.UniformShadow;
		pieChartControl.EdgeColorType = Alt.GUI.PieChart.EdgeColorType.EnhancedContrast;
	}


	
	Alt.Sketch.Color[] Colors
	{
		get
		{
			ArrayList colors = new ArrayList();

			int alpha = 130;
			colors.Add(Alt.Sketch.Color.FromArgb(alpha, Alt.Sketch.Color.Red));
			colors.Add(Alt.Sketch.Color.FromArgb(alpha, Alt.Sketch.Color.LimeGreen));
			colors.Add(Alt.Sketch.Color.FromArgb(alpha, Alt.Sketch.Color.Blue));
			colors.Add(Alt.Sketch.Color.FromArgb(alpha, Alt.Sketch.Color.Yellow));
			colors.Add(Alt.Sketch.Color.FromArgb(alpha, Alt.Sketch.Color.Firebrick));
			colors.Add(Alt.Sketch.Color.FromArgb(alpha, Alt.Sketch.Color.DeepSkyBlue));
			
			return (Alt.Sketch.Color[])colors.ToArray(typeof(Alt.Sketch.Color));
		}
	}
	
	decimal[] Values
	{
		get
		{
			ArrayList values = new ArrayList();

			values.Add((decimal)10);
			values.Add((decimal)15);
			values.Add((decimal)20);
			values.Add((decimal)60);
			values.Add((decimal)25);
			values.Add((decimal)25);
			
			return (decimal[])values.ToArray(typeof(decimal));
		}
	}
	
	double[] Displacements
	{
		get
		{
			ArrayList displacements = new ArrayList();

			displacements.Add((double)0.20);
			displacements.Add((double)0.05);
			displacements.Add((double)0.05);
			displacements.Add((double)0.05);
			displacements.Add((double)0.05);
			displacements.Add((double)0.05);

			return (double[])displacements.ToArray(typeof(double));
		}
	}
	
	string[] Texts
	{
		get
		{
			ArrayList texts = new ArrayList();

            texts.Add("red");
			texts.Add("green");
			texts.Add("blue");
			texts.Add("yellow");
			texts.Add("brown");
			texts.Add("cyan");
			
			return (string[])texts.ToArray(typeof(string));
		}
	}
	
	string[] ToolTips
	{
		get
		{
			ArrayList toolTips = new ArrayList();

			toolTips.Add("");
			toolTips.Add("");
			toolTips.Add("");
			toolTips.Add("");
			toolTips.Add("");
			toolTips.Add("");
			
			return (string[])toolTips.ToArray(typeof(string));
		}
	}
}
