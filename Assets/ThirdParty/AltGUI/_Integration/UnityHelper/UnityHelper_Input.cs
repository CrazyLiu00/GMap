//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace Alt.GUI
{
	static partial class UnityHelper
    {
        //  Mouse

		public static Alt.GUI.MouseButtons ToMouseButtons(PointerEventData eventData)
		{
			Alt.GUI.MouseButtons buttons = MouseButtons.None;
			
			buttons |= eventData.button == PointerEventData.InputButton.Left ? MouseButtons.Left : MouseButtons.None;
			buttons |= eventData.button == PointerEventData.InputButton.Middle ? MouseButtons.Middle : MouseButtons.None;
			buttons |= eventData.button == PointerEventData.InputButton.Right ? MouseButtons.Right : MouseButtons.None;
			
			return buttons;
		}
		
		public static MouseEventArgs ToMouseEventArgs(PointerEventData eventData, Vector2 localPosition)
		{
			Vector2 position = localPosition;//eventData.position;
			
			return new MouseEventArgs(
				ToMouseButtons(eventData),
				eventData.clickCount,
				position.x, position.y,
				(int)(eventData.scrollDelta.y * 120));
		}



        //  Keyboard

		public static bool IsInputChar(char ch)
		{
			if (ch < ' ') return false;
			
			// OSX inserts these characters for arrow keys
			if (ch == '\uF700') return false;
			if (ch == '\uF701') return false;
			if (ch == '\uF702') return false;
			if (ch == '\uF703') return false;

			return true;
		}
    

		static UnityEngine.KeyCode[] m_SpecialKeys = new UnityEngine.KeyCode[]
		{
			UnityEngine.KeyCode.Numlock,
			UnityEngine.KeyCode.CapsLock,
			UnityEngine.KeyCode.ScrollLock,
			UnityEngine.KeyCode.RightShift,
			UnityEngine.KeyCode.LeftShift,
			UnityEngine.KeyCode.RightControl,
			UnityEngine.KeyCode.LeftControl,
			UnityEngine.KeyCode.RightAlt,
			UnityEngine.KeyCode.LeftAlt,
			UnityEngine.KeyCode.LeftCommand,
			UnityEngine.KeyCode.LeftApple,
			UnityEngine.KeyCode.LeftWindows,
			UnityEngine.KeyCode.RightCommand,
			UnityEngine.KeyCode.RightApple,
			UnityEngine.KeyCode.RightWindows,
			UnityEngine.KeyCode.AltGr,
			UnityEngine.KeyCode.Help,
			UnityEngine.KeyCode.Print,
			UnityEngine.KeyCode.SysReq,
			UnityEngine.KeyCode.Break,
			UnityEngine.KeyCode.Menu,
		};
		public static bool IsSpecialKey(UnityEngine.KeyCode key)
		{
			foreach (UnityEngine.KeyCode keyCode in m_SpecialKeys)
			{
				if (keyCode == key)
				{
					return true;
				}
			}

			return false;
		}
		public static UnityEngine.KeyCode[] GetPressedSpecialKeysOnly()
		{
			List<UnityEngine.KeyCode> temp = new List<KeyCode>();
			foreach (UnityEngine.KeyCode keyCode in m_SpecialKeys)
			{
				if (Input.GetKey(keyCode))
				{
					temp.Add(keyCode);
				}
			}
			
			return temp.ToArray();
		}
		
		public static UnityEngine.KeyCode[] GetPressedKeys()
		{
			List<UnityEngine.KeyCode> temp = new List<KeyCode>();
			foreach (UnityEngine.KeyCode keyCode in Enum.GetValues(typeof(UnityEngine.KeyCode)))
			{
				if (Input.GetKey(keyCode))
				{
					temp.Add(keyCode);
				}
			}
			
			return temp.ToArray();
		}

		public static bool IsShiftKeyDown()
		{
			return (Input.GetKeyDown(UnityEngine.KeyCode.LeftShift) || Input.GetKeyDown(UnityEngine.KeyCode.RightShift));
		}
		
		public static UnityEngine.KeyCode[] FindAddedKeys(UnityEngine.KeyCode[] baseKeys, UnityEngine.KeyCode[] diffKeys)
		{
			List<UnityEngine.KeyCode> added = new List<UnityEngine.KeyCode>();
			foreach (UnityEngine.KeyCode k in diffKeys)
			{
				if (!FindKey(k, baseKeys))
				{
					added.Add(k);
				}
			}
			return added.ToArray();
		}
		
		public static bool FindKey(UnityEngine.KeyCode key, UnityEngine.KeyCode[] keys)
		{
			for (int i = 0; i < keys.Length; ++i)
			{
				if (keys[i] == key)
				{
					return true;
				}
			}
			
			return false;
		}
	}
}
