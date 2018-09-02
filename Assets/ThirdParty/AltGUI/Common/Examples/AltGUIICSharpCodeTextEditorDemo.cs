//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Examples/Common/ICSharpCode TextEditor Demo")]
public abstract class AltGUIICSharpCodeTextEditorDemo : MonoBehaviour
{	
	protected string FileName
	{
		get
		{
			return "AltData/ICSharpCodeTextEditor/Example_AltNETType_FreeType2_Step1.cs";
		}
	}
}
