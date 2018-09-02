//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections;
using System.Collections.Generic;

using Alt.Backend.Unity;

using UnityEngine;


namespace Alt.Integration.Unity
{
	public abstract class AltGUIMonoBehaviour : MonoBehaviour
	{		
		Alt.GUI.Control m_Child;
		public Alt.GUI.Control Child
		{
			get
			{
				return m_Child;
			}
			set
			{
				if (m_Child == value)
				{
					return;
				}
				
				if (m_Child != null)
				{
					m_Child.Invalidated -= Child_Invalidated;
				}
				
				m_Child = value;
				
				if (m_Child != null)
				{
					m_Child.Invalidated += Child_Invalidated;
					
					m_Child.CreateControl();
					
					m_Child.InjectResize(new Alt.GUI.ResizeEventArgs(this.ClientSize));
				}
				
				Invalidate();
			}
		}
		
		
		void Child_Invalidated(object sender, Alt.GUI.InvalidateEventArgs e)
		{
			Invalidate();
		}
		

		bool m_fInvalidate = true;
		public void Invalidate()
		{
			if (m_fInvalidate)
			{
				return;
			}

			m_fInvalidate = true;
		}
		
		
		//	Temporary
		
		Alt.GUI.Temporary.Gwen.GwenHost GwenHost
		{
			get
			{
				return Child as Alt.GUI.Temporary.Gwen.GwenHost;
			}
		}
		
		protected Alt.GUI.Temporary.Gwen.Control.Canvas GetOrCreateGwenCanvas()
		{
			if (GwenHost == null)
			{
				Child = new Alt.GUI.Temporary.Gwen.GwenHost();
				Child.BackColor = Alt.Sketch.Color.Transparent;
				GwenHost.Canvas.ShouldDrawBackground = false;
			}
			
			return GwenHost.Canvas;
		}
		
		Alt.GUI.Temporary.Gwen.Control.Base m_GwenChild;
		public Alt.GUI.Temporary.Gwen.Control.Base GwenChild
		{
			get
			{
				return m_GwenChild;
			}
			set
			{
				if (m_GwenChild == value)
				{
					return;
				}
				
				GetOrCreateGwenCanvas();
				
				m_GwenChild = value;
				if (m_GwenChild != null)
				{
					m_GwenChild.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
				}

				GwenHost.Child = value;
				
				if (m_GwenChild != null)
				{
					m_GwenChild.Focus();
				}
			}
		}



		//	Unity
		void OnPostRender()
		{
			//	We process all logic in this func
			Render ((int)ClientWidth, (int)ClientHeight);
		}


		protected virtual void Start()
		{
			Alt.GUI.AltGUIIntegration.Initialize();
		}

		protected virtual void Update()
		{
			Alt.GUI.AltGUIIntegration.Tick();

			ProcessInput();
		}

		
		protected virtual void OnDestroy()
		{
			GwenChild = null;
			Child = null;
		}



		protected void DestroyRender()
		{
			if (m_Renderer != null)
			{
				m_Renderer.Dispose ();
				m_Renderer = null;
			}
		
			//TEST
			//if (Unity_RenderManager.Instance != null)
			//{
			//	Unity_RenderManager.Instance.Dispose ();
			//}
		}



		//

		int m_Width;
		int m_Height;

		
		public abstract double ClientWidth
		{
			get;
		}
		public abstract double ClientHeight
		{
			get;
		}
		public Alt.Sketch.Size ClientSize
		{
			get
			{
				return new Alt.Sketch.Size(ClientWidth, ClientHeight);
			}
		}
		public Alt.Sketch.Rect ClientRectangle
		{
			get
			{
				return new Alt.Sketch.Rect(0, 0, ClientSize);
			}
		}

		
		
		bool m_AutoRefresh = false;
		public bool AutoRefresh
		{
			get
			{
				return m_AutoRefresh;
			}
			set
			{
				m_AutoRefresh = value;
			}
		}



		protected void Render(int width, int height)
		{
			if (m_Renderer == null)
			{
				//TEST	new Unity_RenderManager ();
				m_Renderer = new Unity_Renderer(Unity_RenderManager.Instance);
				m_Renderer.DefaultSmoothingMode = Alt.Sketch.SmoothingMode.None;
			}

			if (m_Width != width ||
			    m_Height != height)
			{
				m_Width = width;
				m_Height = height;
				
				RaiseResize(new Alt.GUI.ResizeEventArgs(m_Width, m_Height));
			}

			OnPreBackground ();
			{
				DoPaintBackground();

				OnRender ();

				DoPaint();
			}
			OnPostPaint ();
		}
		
		protected virtual void OnPreBackground ()
		{
		}
		
		protected virtual void OnPostPaint ()
		{
		}


		protected void ProcessInput()
		{
			ProcessMouse();
			ProcessKeyboard();
		}
				
		protected virtual void ProcessMouse()
		{
		}

		protected virtual void ProcessKeyboard()
		{
		}


		
		//  Paint
		
		public Alt.GUI.PaintEventHandler Paint;
		
		protected virtual void OnPaint(Alt.GUI.PaintEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectRender(e);
			}
		}

		protected virtual void OnRender()
		{
		}
		
		protected virtual void OnPaintBackground(Alt.GUI.PaintEventArgs e)
		{
		}


		Unity_Renderer m_Renderer;
		protected Unity_Renderer Renderer
		{
			get
			{
				return m_Renderer;
			}
		}

		void DoPaintBackground()
		{
			if (m_Renderer == null)
			{
				return;
			}
			
			m_Renderer.BeginRender(GetComponent<Camera>(), GetComponent<Renderer>(), m_Width, m_Height);
			{
				OnPaintBackground(new Alt.GUI.PaintEventArgs(Alt.Sketch.Graphics.FromRenderer(m_Renderer),
				                                     new Alt.Sketch.RectI(Alt.Sketch.PointI.Zero, m_Width, m_Height)));
			}
			m_Renderer.EndRender();
		}
		
		void DoPaint()
		{
			if (m_Renderer == null)
			{
				return;
			}
			
			m_Renderer.BeginRender(GetComponent<Camera>(), GetComponent<Renderer>(), m_Width, m_Height);
			{
				Alt.GUI.PaintEventArgs e = new Alt.GUI.PaintEventArgs(Alt.Sketch.Graphics.FromRenderer(m_Renderer),
				                                      new Alt.Sketch.RectI(Alt.Sketch.PointI.Zero, m_Width, m_Height));
				
				OnPaint(e);
				
				if (!e.Handled &&
				    Paint != null)
				{
					Paint(this, e);
				}
			}
			m_Renderer.EndRender();
		}



		//  Mouse
		
		protected virtual void OnMouseDown(Alt.GUI.MouseEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectMouseDown(e);
			}
		}
		public Alt.GUI.MouseEventHandler MouseDown;
		protected void RaiseMouseDown(Alt.GUI.MouseEventArgs e)
		{
			OnMouseDown(e);
			
			if (MouseDown != null)
			{
				MouseDown(this, e);
			}
		}
		
		protected virtual void OnMouseUp(Alt.GUI.MouseEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectMouseUp(e);
			}
		}
		public Alt.GUI.MouseEventHandler MouseUp;
		protected void RaiseMouseUp(Alt.GUI.MouseEventArgs e)
		{
			OnMouseUp(e);
			
			if (MouseUp != null)
			{
				MouseUp(this, e);
			}
		}
		
		protected virtual void OnMouseMove(Alt.GUI.MouseEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectMouseMove(e);
			}
		}
		public Alt.GUI.MouseEventHandler MouseMove;
		protected void RaiseMouseMove(Alt.GUI.MouseEventArgs e)
		{
			OnMouseMove(e);
			
			if (MouseMove != null)
			{
				MouseMove(this, e);
			}
		}
		
		protected virtual void OnMouseWheel(Alt.GUI.MouseEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectMouseWheel(e);
			}
		}
		public Alt.GUI.MouseEventHandler MouseWheel;
		protected void RaiseMouseWheel(Alt.GUI.MouseEventArgs e)
		{
			OnMouseWheel(e);
			
			if (MouseWheel != null)
			{
				MouseWheel(this, e);
			}
		}
		
		
		
		//  Keyboard
		
		protected virtual void OnKeyDown(Alt.GUI.KeyEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectKeyDown(e);
			}
		}
		public Alt.GUI.KeyEventHandler KeyDown;
		protected void RaiseKeyDown(Alt.GUI.KeyEventArgs e)
		{
			OnKeyDown(e);
			
			if (KeyDown != null)
			{
				KeyDown(this, e);
			}
		}
		
		protected virtual void OnKeyUp(Alt.GUI.KeyEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectKeyUp(e);
			}
		}
		public Alt.GUI.KeyEventHandler KeyUp;
		protected void RaiseKeyUp(Alt.GUI.KeyEventArgs e)
		{
			OnKeyUp(e);
			
			if (KeyUp != null)
			{
				KeyUp(this, e);
			}
		}
		
		protected virtual void OnKeyPress(Alt.GUI.KeyPressEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectKeyPress(e);
			}
		}
		public Alt.GUI.KeyPressEventHandler KeyPress;
		protected void RaiseKeyPress(Alt.GUI.KeyPressEventArgs e)
		{
			OnKeyPress(e);
			
			if (KeyPress != null)
			{
				KeyPress(this, e);
			}
		}
		
		
		
		//  Resize
		protected virtual void OnResize(Alt.GUI.ResizeEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectResize(e);
			}
		}
		public Alt.GUI.ResizeEventHandler Resize;
		void RaiseResize(Alt.GUI.ResizeEventArgs e)
		{
			OnResize(e);
			
			if (Resize != null)
			{
				Resize(this, e);
			}
		}

		
		
		//  Cursor
		
		public class UpdateCursorEventArgs : System.EventArgs
		{
			bool m_Handled = false;
			public bool Handled
			{
				get
				{
					return m_Handled;
				}
				set
				{
					m_Handled = value;
				}
			}
			
			public UpdateCursorEventArgs()
			{
			}
		}
		
		public delegate void UpdateCursorEventHandler(object sender, UpdateCursorEventArgs e);
		public event UpdateCursorEventHandler UpdateCursorEvent;
		
		protected virtual void UpdateCursor()
		{
			if (UpdateCursorEvent != null)
			{
				UpdateCursorEventArgs args = new UpdateCursorEventArgs();
				UpdateCursorEvent(this, args);
				
				if (args.Handled)
				{
					return;
				}
			}
			
			UpdateCursor(Alt.GUI.Cursor.Current);
		}
		

		public void UpdateCursor(Alt.GUI.Cursor cursor)
		{
			if (cursor == null ||
			    cursor.StdCursor == Alt.GUI.StdCursor.Unknown)
			{
				Cursor.visible = false;
				return;
			}
			
			//EngineApp.Instance.SystemCursorFileName = ToCursorFileName(m_LastCursor);
			Cursor.visible = true;
		}



		//	Keyboard
		
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
				RaiseKeyDown(new GUI.KeyEventArgs(Alt.GUI.UnityHelper.TranslateKeyCode(k)));
				
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
				RaiseKeyUp(new GUI.KeyEventArgs(Alt.GUI.UnityHelper.TranslateKeyCode(k)));
			}
		}
		
		IntervalTimer m_DoAutoRepeatIntervalTimer;
		IntervalTimer DoAutoRepeatIntervalTimer
		{
			get
			{
				if (m_DoAutoRepeatIntervalTimer == null)
				{
					m_DoAutoRepeatIntervalTimer = new IntervalTimer();
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
	}
}
