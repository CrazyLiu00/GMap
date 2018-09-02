//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using UnityEngine;
using UnityEngine.UI;

using Alt.Collections;
using Alt.Data;
using Alt.GUI;
using Alt.GUI.NPlot;
using Alt.Sketch;


[AddComponentMenu("AltGUI/Examples/Common/NPlot Demo")]
public abstract class AltGUINPlotDemo : MonoBehaviour
{	
	protected void Start (Alt.GUI.NPlot.Temporary.Gwen.PlotSurface2DControl plotSurface)
	{
		if (plotSurface == null)
		{
			return;
		}

		plotSurface.Clear();
		PlotDataSet(plotSurface.Inner);
		
		plotSurface.AddInteraction(new Alt.GUI.NPlot.Temporary.Gwen.PlotSurface2DControl.Interactions.HorizontalDrag());
		plotSurface.AddInteraction(new Alt.GUI.NPlot.Temporary.Gwen.PlotSurface2DControl.Interactions.VerticalDrag());
		plotSurface.AddInteraction(new Alt.GUI.NPlot.Temporary.Gwen.PlotSurface2DControl.Interactions.AxisDrag(true));

		plotSurface.Refresh();

		AltGUIHelper.SetNPlotSurfaceDefaultBackColor(plotSurface);
	}

	
	public static void PlotDataSet(PlotSurface2D plotSurface)
	{
		plotSurface.Clear();
		//plotSurface.DateTimeToolTip = true;
		
		// obtain stock information from xml file
		DataSet ds = new DataSet();
		System.IO.Stream file = Alt.IO.VirtualFile.OpenRead("AltData/NPlot/asx_jbh.xml");
		ds.ReadXml( file, XmlReadMode.ReadSchema );
		DataTable dt = ds.Tables[0];
		//NoNeed	DataView dv = new DataView( dt );
		
		// create CandlePlot.
		CandlePlot cp = new CandlePlot();
		cp.DataSource = dt;
		cp.AbscissaData = "Date";
		cp.OpenData = "Open";
		cp.LowData = "Low";
		cp.HighData = "High";
		cp.CloseData = "Close";
		cp.BearishColor = Alt.Sketch.Color.Red;
		cp.BullishColor = Alt.Sketch.Color.Green;
		cp.Style = CandlePlot.Styles.Filled;
		
		// calculate 10 day moving average and 2*sd line
		ArrayList av10 = new ArrayList();
		ArrayList sd2_10 = new ArrayList();
		ArrayList sd_2_10 = new ArrayList();
		ArrayList dates = new ArrayList();
		for (int i=0; i<dt.Rows.Count-10; ++i)
		{
			float sum = 0.0f;
			for (int j=0; j<10; ++j)
			{
				sum += (float)dt.Rows[i+j]["Close"];
			}
			float average = sum / 10.0f;
			av10.Add( average );
			sum = 0.0f;
			for (int j=0; j<10; ++j)
			{
				sum += ((float)dt.Rows[i+j]["Close"]-average)*((float)dt.Rows[i+j]["Close"]-average);
			}
			sum /= 10.0f;
			sum = 2.0f*(float)Math.Sqrt( sum );
			sd2_10.Add( average + sum );
			sd_2_10.Add( average - sum );
			dates.Add( (DateTime)dt.Rows[i+10]["Date"] );
		}
		
		// and a line plot of close values.
		LinePlot av = new LinePlot();
		av.OrdinateData = av10;
		av.AbscissaData = dates;
		av.Color = Alt.Sketch.Color.LightGray;
		av.Pen.Width = 2.0f;
		
		LinePlot top = new LinePlot();
		top.OrdinateData = sd2_10;
		top.AbscissaData = dates;
		top.Color = Alt.Sketch.Color.LightSteelBlue;
		top.Pen.Width = 2.0f;
		
		LinePlot bottom = new LinePlot();
		bottom.OrdinateData = sd_2_10;
		bottom.AbscissaData = dates;
		bottom.Color = Alt.Sketch.Color.LightSteelBlue;
		bottom.Pen.Width = 2.0f;
		
		FilledRegion fr = new FilledRegion( top, bottom );
		//fr.RectangleBrush = new RectangleBrushes.Vertical( Color.FloralWhite, Color.GhostWhite );
		fr.RectangleBrush = new RectangleBrushes.Vertical( Alt.Sketch.Color.FromArgb(255,255,240), Alt.Sketch.Color.FromArgb(240,255,255) );
		plotSurface.SmoothingMode = SmoothingMode.AntiAlias;
		
		plotSurface.Add( fr );
		
		plotSurface.Add( new Grid() );
		
		plotSurface.Add( av );
		plotSurface.Add( top );
		plotSurface.Add( bottom );
		plotSurface.Add( cp );
		
		// now make an arrow... 
		ArrowItem arrow = new ArrowItem( new PointD( ((DateTime)dt.Rows[60]["Date"]).Ticks, 2.28 ), -80, "An interesting flat bit" );
		arrow.ArrowColor = Alt.Sketch.Color.DarkBlue;
		arrow.PhysicalLength = 50;
		
		//plotSurface.Add( arrow );
		
		plotSurface.Title = "AU:JBH";
		plotSurface.XAxis1.Label = "Date / Time";
		plotSurface.XAxis1.WorldMin += plotSurface.XAxis1.WorldLength / 4.0;
		plotSurface.XAxis1.WorldMax -= plotSurface.XAxis1.WorldLength / 2.0;
		plotSurface.YAxis1.Label = "Price [$]";
		
		plotSurface.XAxis1 = new TradingDateTimeAxis( plotSurface.XAxis1 );
	}
}
