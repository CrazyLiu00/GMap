//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.GUI;
using Alt.Sketch;


namespace UnityEngine.UI
{
    /// <summary>
	/// AltGUI OxyPlot Control.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUIOxyPlot.EditorName, AltGUIOxyPlot.EditorID)]
	public class AltGUIOxyPlot
		: AltGUIControlHost
    {
		new public const string EditorName = "OxyPlot";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif


		public Alt.GUI.Temporary.Gwen.Control.OxyPlot.Plot Plot
		{
			get
			{
				return GwenChild as Alt.GUI.Temporary.Gwen.Control.OxyPlot.Plot;
			}
		}


		public OxyPlot.PlotModel PlotModel
		{
			get
			{
				Alt.GUI.Temporary.Gwen.Control.OxyPlot.Plot plot = Plot;
				if (plot != null)
				{
					return plot.Model;
				}

				return null;
			}
			set
			{
				Alt.GUI.Temporary.Gwen.Control.OxyPlot.Plot plot = Plot;
				if (plot != null)
				{
					plot.Model = value;
				}
			}
		}
		
		
		protected override void Start ()
		{
			base.Start ();

			GwenChild = AltGUIHelper.Create_OxyPlotControl(GetOrCreateGwenCanvas());
		}
	}
}
