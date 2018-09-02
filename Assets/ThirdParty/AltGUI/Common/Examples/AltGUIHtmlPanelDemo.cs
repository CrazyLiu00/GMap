//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;

using Alt.Sketch;


[AddComponentMenu("AltGUI/Examples/Common/HtmlPanel Demo")]
public abstract class AltGUIHtmlPanelDemo : MonoBehaviour
{	
	protected string HTMLText
	{
		get
		{
			System.IO.Stream fs = Alt.IO.VirtualFile.OpenRead("AltData/HtmlRenderer/Samples/00.Intro.htm");
			if (fs != null)
			{
				using (System.IO.StreamReader sreader = new System.IO.StreamReader(fs, System.Text.Encoding.Default))
				{
					return sreader.ReadToEnd();
				}
			}

			return "HTML file not found...";
		}
	}
}
