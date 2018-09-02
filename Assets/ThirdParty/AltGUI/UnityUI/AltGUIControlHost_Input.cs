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
	partial class AltGUIControlHost
    {		
		public Vector2 ScreenToLocal(Vector2 screen)
		{
			if (m_PaintComponent == null)
			{
				return screen;
			}
			
			var theCanvas = m_PaintComponent.canvas;
			if (theCanvas == null)
			{
				return screen;
			}
			
			Vector3 pos = Vector3.zero;
			if (theCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
			{
				pos = m_PaintComponent.transform.InverseTransformPoint(screen);
			}
			else if (theCanvas.worldCamera != null)
			{
				Ray mouseRay = theCanvas.worldCamera.ScreenPointToRay(screen);
				float dist;
				Plane plane = new Plane(m_PaintComponent.transform.forward, m_PaintComponent.transform.position);
				plane.Raycast(mouseRay, out dist);
				pos = m_PaintComponent.transform.InverseTransformPoint(mouseRay.GetPoint(dist));
			}
			
			return new Vector2(pos.x, pos.y);
		}
		
		public Vector2 ScreenToLocalOatLeftTop(Vector2 screen)
		{
			Vector2 pos = ScreenToLocal(screen);
			RectTransform rt = m_PaintComponent.GetComponent<RectTransform>();
			return new Vector2(pos.x - rt.rect.xMin, - pos.y - rt.rect.yMin);
		}
		
		
		Vector2 m_PreviousMousePosition = new Vector2(-1, -1);
		
		void ProcessMouse ()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				return;
			}
			#endif
			
			Vector2 mousePosition = Input.mousePosition;
			
			//	Move
			if (m_PreviousMousePosition != mousePosition)
			{
				if (m_PreviousMousePosition.x < 0 && m_PreviousMousePosition.y < 0)
				{
					m_PreviousMousePosition = mousePosition;
					return;
				}
				
				m_PreviousMousePosition = mousePosition;
				
				if (m_fMouseIn && !m_fDragging)
				{
					//RaiseMouseMove(new Alt.GUI.MouseEventArgs(m_PreviousMousePosition));
					Vector2 pos = ScreenToLocalOatLeftTop(mousePosition);                
					RaiseMouseMove(new MouseEventArgs(pos.x, pos.y));
				}
			}
		}
		
		
		bool m_fMouseIn = false;
		public override void OnPointerEnter (PointerEventData eventData)
		{
			m_fMouseIn = true;
			base.OnPointerEnter (eventData);
			
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				return;
			}
			#endif
			
			Vector2 pos = ScreenToLocalOatLeftTop(eventData.position);
			RaiseMouseEnter(UnityHelper.ToMouseEventArgs(eventData, pos));
		}
		
		public override void OnPointerExit (PointerEventData eventData)
		{
			m_fMouseIn = false;
			base.OnPointerExit (eventData);
			
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				return;
			}
			#endif
			
			Vector2 pos = ScreenToLocalOatLeftTop(eventData.position);                
			RaiseMouseLeave(UnityHelper.ToMouseEventArgs(eventData, pos));
		}
		
		bool m_fDragging = false;
		public virtual void OnBeginDrag (PointerEventData eventData)
		{
			m_fDragging = true;
		}
		public virtual void OnDrag(PointerEventData eventData)
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				return;
			}
			#endif
			
			Vector2 pos = ScreenToLocalOatLeftTop(eventData.position);                
			RaiseMouseMove(UnityHelper.ToMouseEventArgs(eventData, pos));
			
			//eventData.Use();
		}
		public virtual void OnEndDrag (PointerEventData eventData)
		{
			m_fDragging = false;
		}
		
		
		public override void OnPointerDown(PointerEventData eventData)
		{
			EventSystem.current.SetSelectedGameObject(gameObject, eventData);
			
			base.OnPointerDown(eventData);
			
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				return;
			}
			#endif
			
			Vector2 pos = ScreenToLocalOatLeftTop(eventData.position);                
			RaiseMouseDown(UnityHelper.ToMouseEventArgs(eventData, pos));
			
			//eventData.Use();
		}
		
		public override void OnPointerUp (PointerEventData eventData)
		{
			base.OnPointerUp (eventData);
			
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				return;
			}
			#endif
			
			Vector2 pos = ScreenToLocalOatLeftTop(eventData.position);                
			RaiseMouseUp(UnityHelper.ToMouseEventArgs(eventData, pos));
			
			//eventData.Use();
		}
		
		public virtual void OnScroll (PointerEventData eventData)
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				return;
			}
			#endif
			
			if (m_fMouseIn)
			{
				Vector2 pos = ScreenToLocalOatLeftTop(eventData.position);                
				RaiseMouseWheel(UnityHelper.ToMouseEventArgs(eventData, pos));
			}

			//eventData.Use();
		}



		
		
		protected UnityEngine.KeyCode[] m_OldKeyboardState = new KeyCode[0];
		int autoRepeatDelay = 20;//msec
		bool autoRepeatStarted = false;
		List<UnityEngine.KeyCode> autoRepeatKeys = new List<UnityEngine.KeyCode>();
		int autoRepeatTimeElapsed = 0;
		
		protected int AutoRepeatDelay
		{
			get
			{
				return autoRepeatDelay;
			}
			set
			{
				autoRepeatDelay = value;
			}
		}
		
		
		protected void DoPressed(UnityEngine.KeyCode[] pressed, bool shiftDown)
		{
			if (pressed.Length > 0)
			{
				ClearAutoRepeat();
				autoRepeatKeys.AddRange(pressed);
			}
			
			DoPress(pressed, shiftDown);
		}
		void DoPress(IEnumerable<UnityEngine.KeyCode> pressed, bool shiftDown)
		{
			foreach (UnityEngine.KeyCode k in pressed)
			{
				RaiseKeyDown(new Alt.GUI.KeyEventArgs(Alt.GUI.UnityHelper.TranslateKeyCode(k)));
				
				//InjectChar(k, shiftDown);
			}
		}
		
		protected void DoReleased(UnityEngine.KeyCode[] released)
		{
			if (released.Length > 0)
			{
				ClearAutoRepeat();
			}
			
			foreach (UnityEngine.KeyCode k in released)
			{
				RaiseKeyUp(new Alt.GUI.KeyEventArgs(Alt.GUI.UnityHelper.TranslateKeyCode(k)));
			}
		}
		
		Alt.IntervalTimer m_DoAutoRepeatIntervalTimer;
		Alt.IntervalTimer DoAutoRepeatIntervalTimer
		{
			get
			{
				if (m_DoAutoRepeatIntervalTimer == null)
				{
					m_DoAutoRepeatIntervalTimer = new Alt.IntervalTimer();
					m_DoAutoRepeatIntervalTimer.Start();
				}
				
				return m_DoAutoRepeatIntervalTimer;
			}
		}
		protected void DoAutoRepeat(bool shiftDown)
		{
			autoRepeatTimeElapsed += (int)DoAutoRepeatIntervalTimer.ElapsedTimeMSec;
			DoAutoRepeatIntervalTimer.Reset();
			
			bool repeat = false;
			if (autoRepeatStarted)
			{
				repeat = autoRepeatTimeElapsed >= AutoRepeatDelay;
			}
			else
			{
				repeat = autoRepeatTimeElapsed >= 500;
			}
			
			if (repeat)
			{
				autoRepeatStarted = true;
				autoRepeatTimeElapsed = 0;
				
				DoPress(autoRepeatKeys, shiftDown);
			}
		}
		
		protected void ClearAutoRepeat()
		{
			autoRepeatKeys.Clear();
			autoRepeatTimeElapsed = 0;
			DoAutoRepeatIntervalTimer.Reset();
			autoRepeatStarted = false;
		}
		
		void ProcessKeyboard()
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				return;
			}
			#endif
			
			if (isSelected)
			{
				UnityEngine.KeyCode[] oldKeys = m_OldKeyboardState;
				UnityEngine.KeyCode[] newKeys = Alt.GUI.UnityHelper.GetPressedSpecialKeysOnly();//GetPressedKeys();
				
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
			}
		}


		
		void KeyPressed(Event evt)
		{
			//	Special keys handled in ProcessKeyboard
			if (!UnityHelper.IsSpecialKey(evt.keyCode))
			{
				/*NotUsed yet
				var currentEventModifiers = evt.modifiers;
				RuntimePlatform rp = Application.platform;
				bool isMac = (rp == RuntimePlatform.OSXEditor || rp == RuntimePlatform.OSXPlayer || rp == RuntimePlatform.OSXWebPlayer);
				bool ctrl = isMac ? (currentEventModifiers & EventModifiers.Command) != 0 : (currentEventModifiers & EventModifiers.Control) != 0;
				bool shift = (currentEventModifiers & EventModifiers.Shift) != 0;*/

				RaiseKeyDown(new Alt.GUI.KeyEventArgs(Alt.GUI.UnityHelper.TranslateKeyCode(evt.keyCode)));
			}

			if (UnityHelper.IsInputChar(evt.character))
			{
				RaiseKeyPress(new Alt.GUI.KeyPressEventArgs(evt.character));
			}
		}
		
		void KeyReleased(Event evt)
		{
			//	Special keys handled in ProcessKeyboard
			if (!UnityHelper.IsSpecialKey(evt.keyCode))
			{
				/*NotUsed yet
				var currentEventModifiers = evt.modifiers;
				RuntimePlatform rp = Application.platform;
				bool isMac = (rp == RuntimePlatform.OSXEditor || rp == RuntimePlatform.OSXPlayer || rp == RuntimePlatform.OSXWebPlayer);
				bool ctrl = isMac ? (currentEventModifiers & EventModifiers.Command) != 0 : (currentEventModifiers & EventModifiers.Control) != 0;
				bool shift = (currentEventModifiers & EventModifiers.Shift) != 0;*/
				
				RaiseKeyUp(new Alt.GUI.KeyEventArgs(Alt.GUI.UnityHelper.TranslateKeyCode(evt.keyCode)));
			}
		}

		
		/// <summary>ProcessMove
		/// Handle the specified event.
		/// </summary>
		Event m_ProcessingEvent = new Event();
		
		public virtual void OnUpdateSelected(BaseEventData eventData)
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				return;
			}
			#endif
			
			if (isSelected)
			{
				while (Event.PopEvent(m_ProcessingEvent))
				{
					if (m_ProcessingEvent.rawType == EventType.KeyDown)
					{
						KeyPressed(m_ProcessingEvent);
					}
					else if (m_ProcessingEvent.rawType == EventType.KeyUp)
					{
						KeyReleased(m_ProcessingEvent);
					}
				}
			}
			
			eventData.Use();
		}
	}
}
