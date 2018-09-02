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
	/// AltGUI RichTextBox.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUIRichTextBox.EditorName, AltGUIRichTextBox.EditorID)]
	public class AltGUIRichTextBox
		: AltGUIControlHost
    {
		new public const string EditorName = "RichTextBox";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif

		
		public Alt.GUI.RichTextBox RichTextBox
		{
			get
			{
				return Child as Alt.GUI.RichTextBox;
			}
		}
		
		
		protected override void Start ()
		{
			base.Start ();

			Child = new Alt.GUI.RichTextBox();
		
			Alt.GUI.RichTextBox richTextBox = RichTextBox;
			if (richTextBox == null)
			{
				return;
			}

			richTextBox.BorderStyle = BorderStyle.None;
		}


		public void LoadRTF(string fileName)
		{
			Alt.GUI.RichTextBox richTextBox = RichTextBox;
			if (richTextBox == null)
			{
				return;
			}

			richTextBox.LoadFile(fileName);
		}
	}
}
