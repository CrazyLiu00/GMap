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


[AddComponentMenu("AltGUI/Examples/Common/OxyPlot Demo")]
public abstract class AltGUIOxyPlotDemo : MonoBehaviour
{	
	public static PlotModel LineSeriesWithSmoothing()
	{
		var model = new PlotModel("LineSeries with smoothing", "Tracker uses wrong points");
		var logarithmicAxis1 = new LogarithmicAxis { Position = AxisPosition.Bottom };
		model.Axes.Add(logarithmicAxis1);
		
		// Add a line series
		var s1 = new LineSeries
		{
			Color = OxyColors.SkyBlue,
			MarkerType = MarkerType.Circle,
			MarkerSize = 6,
			MarkerStroke = OxyColors.White,
			MarkerFill = OxyColors.SkyBlue,
			MarkerStrokeThickness = 1.5,
			Smooth = true
		};
		s1.Points.Add(new DataPoint(100, 100));
		s1.Points.Add(new DataPoint(400, 200));
		s1.Points.Add(new DataPoint(600, -300));
		s1.Points.Add(new DataPoint(1000, 400));
		s1.Points.Add(new DataPoint(1500, 500));
		s1.Points.Add(new DataPoint(2500, 600));
		s1.Points.Add(new DataPoint(3000, 700));
		model.Series.Add(s1);
		
		return model;
	}


	
	public static PlotModel CreateNormalDistributionModel()
	{
		// http://en.wikipedia.org/wiki/Normal_distribution
		
		var plot = new PlotModel
		{
			Title = "Normal distribution",
			Subtitle = "Probability density function"
		};
		
		plot.Axes.Add(new LinearAxis
		              {
			Position = AxisPosition.Left,
			Minimum = -0.05,
			Maximum = 1.05,
			MajorStep = 0.2,
			MinorStep = 0.05,
			TickStyle = global::OxyPlot.Axes.TickStyle.Inside
		});
		plot.Axes.Add(new LinearAxis
		              {
			Position = AxisPosition.Bottom,
			Minimum = -5.25,
			Maximum = 5.25,
			MajorStep = 1,
			MinorStep = 0.25,
			TickStyle = global::OxyPlot.Axes.TickStyle.Inside
		});
		plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 0.2));
		plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 1));
		plot.Series.Add(CreateNormalDistributionSeries(-5, 5, 0, 5));
		plot.Series.Add(CreateNormalDistributionSeries(-5, 5, -2, 0.5));
		return plot;
	}

	static DataPointSeries CreateNormalDistributionSeries(double x0, double x1, double mean, double variance)//, int n = 1001)
	{
		return CreateNormalDistributionSeries(x0, x1, mean, variance, 1001);
	}
	static DataPointSeries CreateNormalDistributionSeries(double x0, double x1, double mean, double variance, int n)// = 1001)
	{
		var ls = new LineSeries
		{
			Title = String.Format("μ={0}, σ²={1}", mean, variance)
		};
		
		for (int i = 0; i < n; i++)
		{
			double x = x0 + (x1 - x0) * i / (n - 1);
			double f = 1.0 / Math.Sqrt(2 * Math.PI * variance) * Math.Exp(-(x - mean) * (x - mean) / 2 / variance);
			ls.Points.Add(new DataPoint(x, f));
		}
		return ls;
	}



	public static PlotModel TwoColorLineSeries()
	{
		var model = new PlotModel("TwoColorLineSeries") { LegendSymbolLength = 24 };
		var s1 = new TwoColorLineSeries()
		{
			Title = "Temperature at Eidesmoen, December 1986.",
			Color = OxyColors.Red,
			Color2 = OxyColors.LightBlue,
			StrokeThickness = 3,
			Limit = 0,
			Smooth = true,
			MarkerType = MarkerType.Circle,
			MarkerSize = 4,
			MarkerStroke = OxyColors.Black,
			MarkerStrokeThickness = 1.5
		};
		var temperature = new[]
		{
			5, 0, 7, 7, 4, 3, 5, 5, 11, 4, 2, 3, 2, 1, 0, 2, -1, 0, 0, -3, -6, -13, -10, -10, 0, -4, -5, -4, 3, 0,
			-5
		};
		
		for (int i = 0; i < temperature.Length; i++)
		{
			s1.Points.Add(new DataPoint(i + 1, temperature[i]));
		}
		
		model.Series.Add(s1);
		model.Axes.Add(new LinearAxis(AxisPosition.Left) { ExtraGridlines = new[] { 0.0 } });
		
		return model;
	}

	
	
	public static PlotModel RichterMagnitudes()
	{
		// http://en.wikipedia.org/wiki/Richter_magnitude_scale
		
		var model = new PlotModel("The Richter magnitude scale")
		{
			PlotMargins = new OxyThickness(80, 0, 80, 40),
			LegendPlacement = LegendPlacement.Inside,
			LegendPosition = LegendPosition.TopCenter,
			LegendOrientation = LegendOrientation.Horizontal,
			LegendSymbolLength = 24
		};
		
			model.Axes.Add(new LinearAxis(AxisPosition.Bottom, "Richter magnitude scale") { MajorGridlineStyle = LineStyle.None, TickStyle = global::OxyPlot.Axes.TickStyle.None });
		
		var frequencyCurve = new LineSeries("Frequency")
		{
			Color = OxyColor.FromUInt32(0xff3c6c9e),
			StrokeThickness = 3,
			MarkerStroke = OxyColor.FromUInt32(0xff3c6c9e),
			MarkerFill = OxyColors.White,
			MarkerType = MarkerType.Circle,
			MarkerSize = 4,
			MarkerStrokeThickness = 3
		};
		
		frequencyCurve.Points.Add(new DataPoint(1.5, 8000 * 365 * 100));
		frequencyCurve.Points.Add(new DataPoint(2.5, 1000 * 365 * 100));
		frequencyCurve.Points.Add(new DataPoint(3.5, 49000 * 100));
		frequencyCurve.Points.Add(new DataPoint(4.5, 6200 * 100));
		frequencyCurve.Points.Add(new DataPoint(5.5, 800 * 100));
		frequencyCurve.Points.Add(new DataPoint(6.5, 120 * 100));
		frequencyCurve.Points.Add(new DataPoint(7.5, 18 * 100));
		frequencyCurve.Points.Add(new DataPoint(8.5, 1 * 100));
		frequencyCurve.Points.Add(new DataPoint(9.5, 1.0 / 20 * 100));
			model.Axes.Add(new LogarithmicAxis(AxisPosition.Left, "Frequency / 100 yr") { UseSuperExponentialFormat = true, MajorGridlineStyle = LineStyle.None, TickStyle = global::OxyPlot.Axes.TickStyle.Outside });
		model.Series.Add(frequencyCurve);
		
		var energyCurve = new LineSeries("Energy")
		{
			Color = OxyColor.FromUInt32(0xff9e6c3c),
			StrokeThickness = 3,
			MarkerStroke = OxyColor.FromUInt32(0xff9e6c3c),
			MarkerFill = OxyColors.White,
			MarkerType = MarkerType.Circle,
			MarkerSize = 4,
			MarkerStrokeThickness = 3
		};
		
		energyCurve.Points.Add(new DataPoint(1.5, 11e6));
		energyCurve.Points.Add(new DataPoint(2.5, 360e6));
		energyCurve.Points.Add(new DataPoint(3.5, 11e9));
		energyCurve.Points.Add(new DataPoint(4.5, 360e9));
		energyCurve.Points.Add(new DataPoint(5.5, 11e12));
		energyCurve.Points.Add(new DataPoint(6.5, 360e12));
		energyCurve.Points.Add(new DataPoint(7.5, 11e15));
		energyCurve.Points.Add(new DataPoint(8.5, 360e15));
		energyCurve.Points.Add(new DataPoint(9.5, 11e18));
		energyCurve.YAxisKey = "energyAxis";
		
			model.Axes.Add(new LogarithmicAxis(AxisPosition.Right, "Energy / J") { Key = "energyAxis", UseSuperExponentialFormat = true, MajorGridlineStyle = LineStyle.None, TickStyle = global::OxyPlot.Axes.TickStyle.Outside });
		model.Series.Add(energyCurve);
		
		return model;
	}


	public static PlotModel Mandelbrot()
	{
		// http://en.wikipedia.org/wiki/Mandelbrot_set
		var model = new PlotModel { Title = "The Mandelbrot set" };
		model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -1.4, Maximum = 1.4 });
		model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -2, Maximum = 1 });
		model.Axes.Add(
			new ColorAxis
			{
			Position = AxisPosition.Right,
			Minimum = 0,
			Maximum = 64,
			Palette = OxyPalettes.Jet(64),
			HighColor = OxyColors.Black
		});
		model.Series.Add(new MandelbrotSeries());

		return model;
	}

	
	/// <summary>
	/// Renders the Mandelbrot set as an image inside the current plot area.
	/// </summary>
	public class MandelbrotSeries : XYAxisSeries
	{
		/// <summary>
		/// Gets or sets the color axis.
		/// </summary>
		/// <value>
		/// The color axis.
		/// </value>
		/// <remarks>
		/// The Maximum value of the ColorAxis defines the maximum number of iterations.
		/// </remarks>
		public ColorAxis ColorAxis { get; protected set; }
		
		/// <summary>
		/// Gets or sets the color axis key.
		/// </summary>
		/// <value> The color axis key. </value>
		public string ColorAxisKey { get; set; }
		
		/// <summary>
		/// Gets the point on the series that is nearest the specified point.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <param name="interpolate">Interpolate the series if this flag is set to <c>true</c>.</param>
		/// <returns>
		/// A TrackerHitResult for the current hit.
		/// </returns>
		public override TrackerHitResult GetNearestPoint(ScreenPoint point, bool interpolate)
		{
			var p = this.InverseTransform(point);
			var it = Solve(p.X, p.Y, (int)this.ColorAxis.ActualMaximum + 1);
			return new TrackerHitResult(
				this,
				p,
				point,
				null,
				-1,
				string.Format("X: {0:0.000}\r\nY: {1:0.000}\r\nIterations: {2}", p.X, p.Y, it));
		}
		
		/// <summary>
		/// Renders the series on the specified render context.
		/// </summary>
		/// <param name="rc">The rendering context.</param>
		/// <param name="model">The model.</param>
		public override void Render(IRenderContext rc, PlotModel model)
		{
			var p0 = this.Transform(this.XAxis.ActualMinimum, this.YAxis.ActualMinimum);
			var p1 = this.Transform(this.XAxis.ActualMaximum, this.YAxis.ActualMaximum);
			var w = (int)(p1.X - p0.X);
			var h = (int)(p0.Y - p1.Y);
			int maxIterations = (int)this.ColorAxis.ActualMaximum + 1;
			var pixels = new OxyColor[h, w];
			
			ParallelFor(
				0,
				h,
				i =>
				{
				double y = this.YAxis.ActualMinimum
					+ ((double)i / (h - 1) * (this.YAxis.ActualMaximum - this.YAxis.ActualMinimum));
				for (int j = 0; j < w; j++)
				{
					double x = this.XAxis.ActualMinimum
						+ ((double)j / (w - 1)
						   * (this.XAxis.ActualMaximum - this.XAxis.ActualMinimum));
					var iterations = Solve(x, y, maxIterations);
					pixels[i, j] = this.ColorAxis.GetColor((double)iterations);
				}
			});
			
			var bitmap = OxyImage.PngFromArgb(pixels);
			
			//TEST  rc.DrawImage(bitmap, p0.X, p1.Y, p1.X - p0.X, p0.Y - p1.Y, 1, true);
			RenderingExtensions.DrawImage(rc, bitmap, p0.X, p1.Y, p1.X - p0.X, p0.Y - p1.Y, 1, true);
		}
		
		/// <summary>
		/// Ensures that the axes of the series is defined.
		/// </summary>
		protected override void EnsureAxes()
		{
			base.EnsureAxes();
			this.ColorAxis =
				this.PlotModel.GetAxisOrDefault(this.ColorAxisKey, this.PlotModel.DefaultColorAxis) as ColorAxis;
		}
		
		/// <summary>
		/// Executes a serial for loop.
		/// </summary>
		/// <param name="i0">The start index (inclusive).</param>
		/// <param name="i1">The end index (exclusive).</param>
		/// <param name="action">The action that is invoked once per iteration.</param>
		private static void SerialFor(int i0, int i1, Alt.Action<int> action)
		{
			for (int i = i0; i < i1; i++)
			{
				action(i);
			}
		}
		
		/// <summary>
		/// Executes a parallel for loop using ThreadPool.
		/// </summary>
		/// <param name="i0">The start index (inclusive).</param>
		/// <param name="i1">The end index (exclusive).</param>
		/// <param name="action">The action that is invoked once per iteration.</param>
		private static void ParallelFor(int i0, int i1, Alt.Action<int> action)
		{
			// Environment.ProcessorCount is not available here. Use 4 processors.
			int p = 4;
			
			// Initialize wait handles
			var doneEvents = new System.Threading.WaitHandle[p];
			for (int i = 0; i < p; i++)
			{
				doneEvents[i] = new System.Threading.ManualResetEvent(false);
			}
			
			// Invoke the action of a partition of the range
			Alt.Action<int, int, int> invokePartition = (k, j0, j1) =>
			{
				for (int i = j0; i < j1; i++)
				{
					action(i);
				}
				
				((System.Threading.ManualResetEvent)doneEvents[k]).Set();
			};
			
			// Start p background threads
			int n = (i1 - i0 + p - 1) / p;
			for (int i = 0; i < p; i++)
			{
				int k = i;
				int j0 = i0 + (i * n);
				var j1 = Math.Min(j0 + n, i1);
				System.Threading.ThreadPool.QueueUserWorkItem(state => invokePartition(k, j0, j1));
			}
			
			// Wait for the threads to finish
			foreach (var wh in doneEvents)
			{
				wh.WaitOne();
			}
		}
		
		/// <summary>
		/// Calculates the escape time for the specified point.
		/// </summary>
		/// <param name="x0">The x0.</param>
		/// <param name="y0">The y0.</param>
		/// <param name="maxIterations">The max number of iterations.</param>
		/// <returns>The number of iterations.</returns>
		private static int Solve(double x0, double y0, int maxIterations)
		{
			int iteration = 0;
			double x = 0;
			double y = 0;
			while ((x * x) + (y * y) <= 4 && iteration < maxIterations)
			{
				double xtemp = (x * x) - (y * y) + x0;
				y = (2 * x * y) + y0;
				x = xtemp;
				iteration++;
			}
			
			return iteration;
		}
	}
}
