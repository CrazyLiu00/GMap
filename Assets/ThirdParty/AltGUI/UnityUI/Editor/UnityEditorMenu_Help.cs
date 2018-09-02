//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace UnityEditor.UI
{
	/// <summary>
	/// This script adds the UI menu options to the Unity Editor.
	/// </summary>

	static partial class AltGUIUnityEditorMenu
	{
		/// <summary>
		/// Get the URL pointing to the documentation for the specified component.
		/// </summary>

		public static string GetHelpURL (Type type)
		{
			return "http://www.altsoftlab.com";
		}

		/// <summary>
		/// Show generic help.
		/// </summary>

		public static void ShowHelp ()
		{
			Application.OpenURL("http://www.altsoftlab.com");
		}

		/// <summary>
		/// Show help for the specific topic.
		/// </summary>

		public static void ShowHelp (Type type)
		{
			string url = GetHelpURL(type);
			if (url == null) url = "http://www.altsoftlab.com";// + type;
			Application.OpenURL(url);
		}
	}
}
