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
	/// AltGUI ZedGraph Control.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUIZedGraph.EditorName, AltGUIZedGraph.EditorID)]
	public class AltGUIZedGraph
		: AltGUIControlHost
    {
		new public const string EditorName = "ZedGraph";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif


		public Alt.GUI.ZedGraph.Temporary.Gwen.ZedGraphControl ZedGraphControl
		{
			get
			{
				return GwenChild as Alt.GUI.ZedGraph.Temporary.Gwen.ZedGraphControl;
			}
		}

		/// <summary>
		/// The graph pane the chart is show in.
		/// </summary>
		public Alt.GUI.ZedGraph.PaneBase Pane
		{
			get
			{
				Alt.GUI.ZedGraph.Temporary.Gwen.ZedGraphControl zedGraphControl = ZedGraphControl;
				if (zedGraphControl != null)
				{
					return zedGraphControl.GraphPane;
				}

				return null;
			}
		}
		
		/// <summary>
		/// The graph pane the chart is show in.
		/// </summary>
		public Alt.GUI.ZedGraph.MasterPane MasterPane
		{
			get
			{
				Alt.GUI.ZedGraph.Temporary.Gwen.ZedGraphControl zedGraphControl = ZedGraphControl;
				if (zedGraphControl != null)
				{
					return zedGraphControl.MasterPane;
				}
				
				return null;
			}
		}
		
		/// <summary>
		/// The graph pane the chart is show in (same as .Pane).
		/// </summary>
		public Alt.GUI.ZedGraph.GraphPane GraphPane
		{
			get
			{
				Alt.GUI.ZedGraph.Temporary.Gwen.ZedGraphControl zedGraphControl = ZedGraphControl;
				if (zedGraphControl != null)
				{
					return zedGraphControl.GraphPane;
				}
				
				return null;
			}
		}
		
		
		protected override void Start ()
		{
			base.Start ();

			GwenChild = AltGUIHelper.Create_ZedGraphControl(GetOrCreateGwenCanvas());
		}
	}
}
