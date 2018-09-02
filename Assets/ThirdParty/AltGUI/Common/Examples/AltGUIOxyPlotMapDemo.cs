//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using UnityEngine;
using UnityEngine.UI;

using Alt.Sketch;

using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;


[AddComponentMenu("AltGUI/Examples/Common/OxyPlot Map Demo")]
public abstract class AltGUIOxyPlotMapDemo : MonoBehaviour
{	
	public static PlotModel TileMapAnnotation_openstreetmap_org()
	{
		var model = new PlotModel("TileMapAnnotation (openstreetmap.org)");

		OxyColor color = OxyColors.Black;

		LinearAxis axis = new LinearAxis(AxisPosition.Bottom, 10.4, 10.6, "Longitude");
		axis.TextColor = color;
		model.Axes.Add(axis);

		axis = new LinearAxis(AxisPosition.Left, 59.88, 59.96, "Latitude");
		axis.TextColor = color;
		model.Axes.Add(axis);


		// Add the tile map annotation
		model.Annotations.Add(
			new TileMapAnnotation
			{
			Url = "http://tile.openstreetmap.org/{Z}/{X}/{Y}.png",
			CopyrightNotice = "OpenStreet map."
		});
		
		return model;
	}
	

	public static PlotModel TileMapAnnotation_statkart_no()
	{
		var model = new PlotModel("TileMapAnnotation (statkart.no)");
		
		// TODO: scale ratio between the two axes should be fixed (or depending on latitude...)
		model.Axes.Add(new LinearAxis(AxisPosition.Bottom, 10.4, 10.6, "Longitude"));
		model.Axes.Add(new LinearAxis(AxisPosition.Left, 59.88, 59.96, "Latitude"));
		
		// Add the tile map annotation
		model.Annotations.Add(
			new TileMapAnnotation
			{
			Url = "http://opencache.statkart.no/gatekeeper/gk/gk.open_gmaps?layers=toporaster2&zoom={Z}&x={X}&y={Y}",
			CopyrightNotice = "Kartgrunnlag: Statens kartverk, Geovekst og kommuner.",
			MinZoomLevel = 5,
			MaxZoomLevel = 19
		});
		
		model.Annotations.Add(new ArrowAnnotation
		                      {
			EndPoint = new DataPoint(10.563, 59.888),
			ArrowDirection = new ScreenVector(-40, -60),
			StrokeThickness = 3,
			FontSize = 20,
			FontWeight = global::OxyPlot.FontWeights.Bold,
			TextColor = OxyColors.Magenta.ChangeAlpha(160),
			Color = OxyColors.Magenta.ChangeAlpha(100)
		});
		
		return model;
	}
}
