//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;

using Alt.Sketch;


[AddComponentMenu("AltGUI/Examples/Common/HtmlLabel Demo")]
public abstract class AltGUIHtmlLabelDemo : MonoBehaviour
{	
	protected string HTMLText
	{
		get
		{
			System.IO.Stream fs = Alt.IO.VirtualFile.OpenRead("AltData/HtmlRenderer/Samples/10.HtmlLabel.htm");
			if (fs != null)
			{
				using (System.IO.StreamReader sreader = new System.IO.StreamReader(fs, System.Text.Encoding.Default))
				{
					return sreader.ReadToEnd();
				}
			}

			return "This is an <b>HtmlLabel</b> on transparent background with <span style=\"color: re" +
				"d\">colors</span> and links: <a href=\"http://htmlrenderer.codeplex.com/\">HTML Renderer</a>";
		}
	}
}
