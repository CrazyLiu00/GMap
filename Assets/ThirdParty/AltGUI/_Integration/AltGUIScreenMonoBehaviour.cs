//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace Alt.Integration.Unity
{
	//	AltSketch full screen render
	public abstract class AltGUIScreenMonoBehaviour : AltGUIMonoBehaviour
	{
		public override double ClientWidth
		{
			get
			{
				return Screen.width;
			}
		}
		public override double ClientHeight
		{
			get
			{
				return Screen.height;
			}
		}


		Alt.Sketch.PointI m_PreviousMousePosition = new Alt.Sketch.PointI(-1, -1);
		Alt.Sketch.PointI MousePosition
		{
			get
			{
				return new Alt.Sketch.PointI(
					Mathf.RoundToInt(Input.mousePosition.x),
					Mathf.RoundToInt(Screen.height - Input.mousePosition.y));
			}
		}


		protected override void ProcessMouse ()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				return;
			}
			#endif
			
			//	Move
			if (m_PreviousMousePosition != MousePosition)
			{
				if (m_PreviousMousePosition.X < 0 && m_PreviousMousePosition.Y < 0)
				{
					m_PreviousMousePosition = MousePosition;
					return;
				}
				
				m_PreviousMousePosition = MousePosition;
				
				RaiseMouseMove(new Alt.GUI.MouseEventArgs(m_PreviousMousePosition));
			}

			float delta = Input.GetAxis ("Mouse ScrollWheel");
			if (delta != 0)
			{
				RaiseMouseWheel(new Alt.GUI.MouseEventArgs(MousePosition, delta * 120 * 6));
			}

			//	Left
			if (Input.GetMouseButtonDown(0)) RaiseMouseDown(new Alt.GUI.MouseEventArgs(Alt.GUI.MouseButtons.Left, MousePosition));
			if (Input.GetMouseButtonUp(0)) RaiseMouseUp(new Alt.GUI.MouseEventArgs(Alt.GUI.MouseButtons.Left, MousePosition));

			//	Right
			if (Input.GetMouseButtonDown(1)) RaiseMouseDown(new Alt.GUI.MouseEventArgs(Alt.GUI.MouseButtons.Right, MousePosition));
			if (Input.GetMouseButtonUp(1)) RaiseMouseUp(new Alt.GUI.MouseEventArgs(Alt.GUI.MouseButtons.Right, MousePosition));

			//	Middle
			if (Input.GetMouseButtonDown(2)) RaiseMouseDown(new Alt.GUI.MouseEventArgs(Alt.GUI.MouseButtons.Middle, MousePosition));
			if (Input.GetMouseButtonUp(2)) RaiseMouseUp(new Alt.GUI.MouseEventArgs(Alt.GUI.MouseButtons.Middle, MousePosition));
		}
			

		protected override void ProcessKeyboard()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				return;
			}
			#endif
			
			UnityEngine.KeyCode[] oldKeys = m_OldKeyboardState;
			UnityEngine.KeyCode[] newKeys = Alt.GUI.UnityHelper.GetPressedKeys();
			
			// if no input, skip the rest
			if ((oldKeys.Length != 0) || (newKeys.Length != 0))
			{
				bool isShiftKeyDown = Alt.GUI.UnityHelper.IsShiftKeyDown();

				UnityEngine.KeyCode[] addedKeys = Alt.GUI.UnityHelper.FindAddedKeys(oldKeys, newKeys);
				if (addedKeys.Length != 0)
				{
					DoPressed(addedKeys, isShiftKeyDown);
				}
				else
				{
					DoAutoRepeat(isShiftKeyDown);
				}
				
				DoReleased(Alt.GUI.UnityHelper.FindAddedKeys(newKeys, oldKeys));
				
				m_OldKeyboardState = newKeys;
			}

			if (Input.anyKeyDown)
			{
				foreach (char ch in Input.inputString)
				{
					if (Alt.GUI.UnityHelper.IsInputChar(ch))
					{
						RaiseKeyPress(new Alt.GUI.KeyPressEventArgs(ch));
					}
				}
			}
		}
	}
}
