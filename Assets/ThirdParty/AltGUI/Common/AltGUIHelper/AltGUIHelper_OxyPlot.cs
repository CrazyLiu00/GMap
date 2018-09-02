//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Alt.IO;
using Alt.Sketch;


namespace Alt.GUI
{
	static partial class AltGUIHelper
	{
		internal static Alt.GUI.Temporary.Gwen.Control.OxyPlot.Plot Create_OxyPlotControl(Alt.GUI.Temporary.Gwen.Control.Base parent)
		{
			Alt.GUI.Temporary.Gwen.Control.OxyPlot.Plot plot = new Alt.GUI.Temporary.Gwen.Control.OxyPlot.Plot(parent);
			
			plot.KeyboardPanHorizontalStep = 0.1D;
			plot.KeyboardPanVerticalStep = 0.1D;
			plot.Model = CreateOxyPlotModel();
			plot.Name = "Plot";
			plot.PanCursor = Alt.GUI.Cursors.Hand;
			plot.ZoomHorizontalCursor = Alt.GUI.Cursors.SizeWE;
			plot.ZoomRectangleCursor = Alt.GUI.Cursors.SizeNWSE;
			plot.ZoomVerticalCursor = Alt.GUI.Cursors.SizeNS;

			return plot;
		}
		

		public static OxyPlot.PlotModel CreateOxyPlotModel()
		{
			OxyPlot.PlotModel plotModel = new OxyPlot.PlotModel();
			
			plotModel.Annotations = null;
			plotModel.AutoAdjustPlotMargins = true;
			plotModel.Axes = null;
			plotModel.AxisTierDistance = 4D;
			plotModel.Background = null;
			plotModel.Culture = null;
			plotModel.DefaultColors = null;
			plotModel.DefaultFont = "Arial";//Segoe UI";
			plotModel.DefaultFontSize = 12D;
			plotModel.IsLegendVisible = true;
			plotModel.LegendBackground = null;
			plotModel.LegendBorder = null;
			plotModel.LegendBorderThickness = 1D;
			plotModel.LegendColumnSpacing = 0D;
			plotModel.LegendFont = null;
			plotModel.LegendFontSize = 12D;
			plotModel.LegendFontWeight = 400D;
			plotModel.LegendItemAlignment = global::OxyPlot.HorizontalAlignment.Left;
			plotModel.LegendItemOrder = global::OxyPlot.LegendItemOrder.Normal;
			plotModel.LegendItemSpacing = 24D;
			plotModel.LegendMargin = 8D;
			plotModel.LegendOrientation = global::OxyPlot.LegendOrientation.Vertical;
			plotModel.LegendPadding = 8D;
			plotModel.LegendPlacement = global::OxyPlot.LegendPlacement.Inside;
			plotModel.LegendPosition = global::OxyPlot.LegendPosition.RightTop;
			plotModel.LegendSymbolLength = 16D;
			plotModel.LegendSymbolMargin = 4D;
			plotModel.LegendSymbolPlacement = global::OxyPlot.LegendSymbolPlacement.Left;
			plotModel.LegendTextColor = null;
			plotModel.LegendTitle = null;
			plotModel.LegendTitleColor = null;
			plotModel.LegendTitleFont = null;
			plotModel.LegendTitleFontSize = 12D;
			plotModel.LegendTitleFontWeight = 700D;
			plotModel.PlotAreaBackground = null;
			plotModel.PlotAreaBorderColor = null;
			plotModel.PlotAreaBorderThickness = 1D;
			plotModel.PlotType = global::OxyPlot.PlotType.XY;
			plotModel.Series = null;
			plotModel.Subtitle = null;
			plotModel.SubtitleColor = null;
			plotModel.SubtitleFont = null;
			plotModel.SubtitleFontSize = 14D;
			plotModel.SubtitleFontWeight = 400D;
			plotModel.TextColor = null;
			plotModel.Title = null;
			plotModel.TitleColor = null;
			plotModel.TitleFont = null;
			plotModel.TitleFontSize = 18D;
			plotModel.TitleFontWeight = 700D;
			plotModel.TitlePadding = 6D;
			
			return plotModel;
		}		
	}
}
