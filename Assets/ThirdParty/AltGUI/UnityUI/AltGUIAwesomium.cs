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
	/// AltGUI Awesomium Control.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUIAwesomium.EditorName, AltGUIAwesomium.EditorID)]
	public class AltGUIAwesomium
		: AltGUIControlHost
    {
		new public const string EditorName = "Awesomium";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif
		
		
		public Alt.GUI.Temporary.Gwen.Control.Awesomium Awesomium
		{
			get
			{
				return GwenChild as Alt.GUI.Temporary.Gwen.Control.Awesomium;
			}
		}


		protected override void Start ()
		{
			base.Start ();

			GwenChild = AltGUIHelper.Create_AwesomiumControl(GetOrCreateGwenCanvas());

			Child.BackColor = Alt.Sketch.Color.FromArgb(96, Alt.Sketch.Color.DodgerBlue);
		}
	}
}
