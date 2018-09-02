//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/ICSharpCode TextEditor Demo")]
public class AltGUIICSharpCodeTextEditorDemo_UnityUI : AltGUIICSharpCodeTextEditorDemo
{	
	AltGUIICSharpCodeTextEditor ICSharpCodeTextEditor
	{
		get
		{
			return gameObject.GetComponent<AltGUIICSharpCodeTextEditor>();
		}
	}


	// Use this for initialization
	void Start ()
	{
		AltGUIICSharpCodeTextEditor textEditor = ICSharpCodeTextEditor;
		if (textEditor == null)
		{
			return;
		}

		textEditor.LoadFile(FileName);
	}
}
