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
	/// AltGUI NPlot Control.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUINPlot.EditorName, AltGUINPlot.EditorID)]
	public class AltGUINPlot
		: AltGUIControlHost
    {
		new public const string EditorName = "NPlot";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif


		public Alt.GUI.NPlot.Temporary.Gwen.PlotSurface2DControl PlotSurface
		{
			get
			{
				return GwenChild as Alt.GUI.NPlot.Temporary.Gwen.PlotSurface2DControl;
			}
		}
		
		
		protected override void Start ()
		{
			base.Start ();

			GwenChild = AltGUIHelper.Create_NPlotSurface(GetOrCreateGwenCanvas());
		}
	
	
		public void SetPlotDefaultBackColor()
		{
			Alt.GUI.NPlot.Temporary.Gwen.PlotSurface2DControl plotSurface = PlotSurface;
			if (plotSurface == null)
			{
				return;
			}
			
			AltGUIHelper.SetNPlotSurfaceDefaultBackColor(plotSurface);
		}


		public void SetPlotBackColor(Alt.Sketch.Color color)
		{
			Alt.GUI.NPlot.Temporary.Gwen.PlotSurface2DControl plotSurface = PlotSurface;
			if (plotSurface == null)
			{
				return;
			}
			
			plotSurface.ClientBackColor = color;
		}
	}
}
