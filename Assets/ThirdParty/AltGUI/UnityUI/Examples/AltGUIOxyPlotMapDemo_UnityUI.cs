//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;

using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;


[AddComponentMenu("AltGUI/Unity UI/Examples/OxyPlot Map Demo")]
public class AltGUIOxyPlotMapDemo_UnityUI : AltGUIOxyPlotMapDemo
{	
	AltGUIOxyPlot OxyPlot
	{
		get
		{
			return gameObject.GetComponent<AltGUIOxyPlot>();
		}
	}


	// Use this for initialization
	void Start ()
	{
		AltGUIOxyPlot oxyPlot = OxyPlot;
		if (oxyPlot == null)
		{
			return;
		}

		PlotModel model =
			TileMapAnnotation_openstreetmap_org();
			//TileMapAnnotation_statkart_no();
		
		oxyPlot.PlotModel = model;
		oxyPlot.Plot.ClientBackColor = model.Background != null ?
			Alt.Sketch.Ext.OxyPlot.ConverterExtensions.ToColor(model.Background) : Alt.Sketch.Color.FromArgb(128, Alt.Sketch.Color.WhiteSmoke);
	}
}
