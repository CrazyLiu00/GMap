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
	/// AltGUI QuickFont Control.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUIQuickFont.EditorName, AltGUIQuickFont.EditorID)]
	public class AltGUIQuickFont
		: AltGUIControlHost
    {
		new public const string EditorName = "QuickFont";
		new public const int EditorID = 779;
		
#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
#endif


		internal override void DoPaint(PaintEventArgs e)
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				EditorPaint(e);
				
				return;
			}
			#endif

			OnPaintBackground(e);
			
			OnPaint(e);
		}


		void QuickFont_OnPaint(object sender, PaintEventArgs e)
		{
			//	Needs call this later because QuickFontControl prepares QuickFont Renderer
			if (!e.Handled)
			{
				RaisePaint(e);
			}
		}

		
		public Alt.GUI.QuickFont.QuickFontControl QuickFontControl
		{
			get
			{
				return Child as Alt.GUI.QuickFont.QuickFontControl;
			}
		}
		
		
		protected override void Start ()
		{
			base.Start ();

			Child = AltGUIHelper.Create_QuickFontControl();

			Child.Paint += QuickFont_OnPaint;
		}
	}
}
