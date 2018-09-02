//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Alt;
using Alt.Backend.Unity;
using Alt.GUI;
using Alt.Sketch;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;


namespace UnityEngine.UI
{
    /// <summary>
	/// AltOS Host.
	/// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltOSHost.EditorName, AltOSHost.EditorID)]
	public class AltOSHost
		: AltGUIControlHost
	{
		new public const string EditorName = "AltOS Host [Coming Soon...]";
		new public const int EditorID = 779;
		
		
		public Alt.GUI.Control AltOSControl
		{
			get
			{
				return Child as Alt.GUI.Control;
			}
		}
		
		
		public AltOSHost()
		{
			Child = new Alt.GUI.Control();
		}
	}
}
