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
	/// AltGUI 3DPie Chart.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUI3DPieChart.EditorName, AltGUI3DPieChart.EditorID)]
	public class AltGUI3DPieChart
		: AltGUIControlHost
    {
		new public const string EditorName = "3DPie Chart";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif

		
		public Alt.GUI.PieChart.Temporary.Gwen.PieChartControl PieChartControl
		{
			get
			{
				return GwenChild as Alt.GUI.PieChart.Temporary.Gwen.PieChartControl;
			}
		}


		protected override void Start ()
		{
			base.Start ();
		
			GwenChild = AltGUIHelper.Create_PieChartControl(GetOrCreateGwenCanvas());
		}
	}
}
