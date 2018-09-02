//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/RichTextBox Demo")]
public class AltGUIRichTextBoxDemo_UnityUI : AltGUIRichTextBoxDemo
{	
	AltGUIRichTextBox RichTextBox
	{
		get
		{
			return gameObject.GetComponent<AltGUIRichTextBox>();
		}
	}


	// Use this for initialization
	void Start ()
	{
		AltGUIRichTextBox richTextBox = RichTextBox;
		if (richTextBox == null)
		{
			return;
		}

		richTextBox.LoadRTF(FileName);
	}
}
