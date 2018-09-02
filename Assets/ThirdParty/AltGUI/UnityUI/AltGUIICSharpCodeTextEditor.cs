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
	/// AltGUI ICSharpCode TextEditor.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUIICSharpCodeTextEditor.EditorName, AltGUIICSharpCodeTextEditor.EditorID)]
	public class AltGUIICSharpCodeTextEditor
		: AltGUIControlHost
    {
		new public const string EditorName = "ICSharpCode.TextEditor";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif

		
		public Alt.GUI.ICSharpCode.TextEditor.TextEditorControl TextEditorControl
		{
			get
			{
				return Child as Alt.GUI.ICSharpCode.TextEditor.TextEditorControl;
			}
		}


		public Alt.GUI.ICSharpCode.TextEditor.Document.ITextEditorProperties TextEditorProperties
		{
			get
			{
				Alt.GUI.ICSharpCode.TextEditor.TextEditorControl textEditorControl = TextEditorControl;
				if (textEditorControl != null)
				{
					return textEditorControl.TextEditorProperties;
				}

				return null;
			}
			set
			{
				Alt.GUI.ICSharpCode.TextEditor.TextEditorControl textEditorControl = TextEditorControl;
				if (textEditorControl != null)
				{
					textEditorControl.TextEditorProperties = value;
				}
			}
		}


		public Alt.GUI.ICSharpCode.TextEditor.Document.IFoldingStrategy FoldingStrategy
		{
			get
			{
				Alt.GUI.ICSharpCode.TextEditor.TextEditorControl textEditorControl = TextEditorControl;
				if (textEditorControl != null)
				{
					return textEditorControl.Document.FoldingManager.FoldingStrategy;
				}
				
				return null;
			}
			set
			{
				Alt.GUI.ICSharpCode.TextEditor.TextEditorControl textEditorControl = TextEditorControl;
				if (textEditorControl != null)
				{
					textEditorControl.Document.FoldingManager.FoldingStrategy = value;
				
					textEditorControl.Document.FoldingManager.UpdateFoldings(null, null);
				}
			}
		}
		
		
		protected override void Start ()
		{
			base.Start ();

			Child = AltGUIHelper.Create_ICSharpCodeTextEditorControl();
		}


		public void LoadFile(string fileName)
		{
			Alt.GUI.ICSharpCode.TextEditor.TextEditorControl textEditorControl = TextEditorControl;
			if (textEditorControl == null)
			{
				return;
			}
			
			try
			{
				textEditorControl.LoadFile(fileName);
			}
			catch
			{
			}
		}
	}
}
