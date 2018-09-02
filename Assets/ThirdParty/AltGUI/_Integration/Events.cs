//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine.Events;
using UnityEngine.EventSystems;

using Alt.GUI;
using Alt.Sketch;


namespace UnityEngine
{
	[Serializable]
	public class PaintEvent : UnityEvent<Alt.GUI.PaintEventArgs>
	{
	}
	
	
	[Serializable]
	public class MouseEvent : UnityEvent<Alt.GUI.MouseEventArgs>
	{
	}


	[Serializable]
	public class KeyEvent : UnityEvent<Alt.GUI.KeyEventArgs>
	{
	}
	
	[Serializable]
	public class KeyPressEvent : UnityEvent<Alt.GUI.KeyPressEventArgs>
	{
	}


	[Serializable]
	public class HtmlLinkClickedEvent : UnityEvent<Alt.GUI.HtmlLinkClickedEventArgs>
	{
	}
}
