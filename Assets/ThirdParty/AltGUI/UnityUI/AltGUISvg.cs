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
	/// AltGUI SVG Viewer.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUISvg.EditorName, AltGUISvg.EditorID)]
	public class AltGUISvg
		: AltGUIControlHost
    {
		new public const string EditorName = "SVG";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif
		
		
		AltGUIHelper.SVGControl SVGControl
		{
			get
			{
				return Child as AltGUIHelper.SVGControl;
			}
		}

		
		protected override void Start ()
		{
			base.Start ();

			Child = AltGUIHelper.Create_SVGControl();
		}

				
		public void LoadSVG(string fileName)
		{
			AltGUIHelper.SVGControl svg = SVGControl;
			if (svg == null)
			{
				return;
			}

			svg.LoadSVG(fileName);
		}
	}
}
