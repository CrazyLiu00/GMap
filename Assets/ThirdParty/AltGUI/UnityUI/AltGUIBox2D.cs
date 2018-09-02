//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.GUI;
using Alt.Sketch;


namespace UnityEngine.UI
{
    /// <summary>
	/// AltGUI Box2D Control.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUIBox2D.EditorName, AltGUIBox2D.EditorID)]
	public class AltGUIBox2D
		: AltGUIControlHost
    {
		new public const string EditorName = "Box2D";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif
		
		
		AltGUIHelper.Box2DContainer Box2DContainer
		{
			get
			{
				return GwenChild as AltGUIHelper.Box2DContainer;
			}
		}


		Alt.TPSCounter m_TPSCounter = new Alt.TPSCounter();
		public double TPS
		{
			get
			{
				return m_TPSCounter.TPS;
			}
		}

		Alt.Box2D.AltSketchDebugDraw m_DebugDraw;
		Alt.Sketch.Font m_InfoFont;
		public Alt.Box2D.AltSketchDebugDraw DebugDraw
		{
			get
			{
				if (m_DebugDraw == null)
				{
					m_DebugDraw = new Alt.Box2D.AltSketchDebugDraw();
				}

				return m_DebugDraw;
			}
		}
		
		
		public Alt.GUI.Box2D.Box2DRestartEvent onBox2DRestartEvent
		{
			get
			{
				AltGUIHelper.Box2DContainer box2DContainer = Box2DContainer;
				if (box2DContainer == null)
				{
					return null;
				}		
				
				return box2DContainer.onBox2DRestartEvent;
			}
		}

		public Alt.GUI.Box2D.Box2DLaunchBomb onBox2DLaunchBomb
		{
			get
			{
				AltGUIHelper.Box2DContainer box2DContainer = Box2DContainer;
				if (box2DContainer == null)
				{
					return null;
				}		
				
				return box2DContainer.onBox2DLaunchBomb;
			}
		}
		
		
		public Alt.GUI.Box2D.Box2DKeyEvent onBox2DKeyDown
		{
			get
			{
				AltGUIHelper.Box2DContainer box2DContainer = Box2DContainer;
				if (box2DContainer == null)
				{
					return null;
				}		
				
				return box2DContainer.onBox2DKeyDown;
			}
		}

				
		//  Mouse
		
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		MouseEvent m_onBox2DMouseDown = new MouseEvent();
		public MouseEvent onBox2DMouseDown
		{
			get
			{
				return m_onBox2DMouseDown;
			}
			set
			{
				m_onBox2DMouseDown = value;
			}
		}
		void RaiseBox2DMouseDown(object sender, MouseEventArgs e)
		{
			m_onBox2DMouseDown.Invoke(e);
		}
		
		
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		private MouseEvent m_onBox2DMouseUp = new MouseEvent();
		public MouseEvent onBox2DMouseUp
		{
			get
			{
				return m_onBox2DMouseUp;
			}
			set
			{
				m_onBox2DMouseUp = value;
			}
		}
		void RaiseBox2DMouseUp(object sender, MouseEventArgs e)
		{
			m_onBox2DMouseUp.Invoke(e);
		}
		
		
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		private MouseEvent m_onBox2DMouseMove = new MouseEvent();
		public MouseEvent onBox2DMouseMove
		{
			get
			{
				return m_onBox2DMouseMove;
			}
			set
			{
				m_onBox2DMouseMove = value;
			}
		}
		void RaiseBox2DMouseMove(object sender, MouseEventArgs e)
		{
			m_onBox2DMouseMove.Invoke(e);
		}


		internal override void DoPaint(PaintEventArgs e)
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				EditorPaint(e);

				return;
			}
			#endif
			
			OnPaintBackground(e);
			
			OnPaint(e);
		}
		
		
		void Box2DContainer_OnPaint(object sender, PaintEventArgs e)
		{
			//	Needs call this later because QuickFontControl prepares QuickFont Renderer
			if (!e.Handled)
			{
				RaisePaint(e);
			}
		}
				
		
		protected override void Start ()
		{
			base.Start ();

			GwenChild = AltGUIHelper.Create_Box2DContainer(GetOrCreateGwenCanvas(), DebugDraw);

			AltGUIHelper.Box2DContainer box2DContainer = Box2DContainer;
			if (box2DContainer == null)
			{
				return;
			}		
			
			m_InfoFont = new Alt.Sketch.Font("Arial", 10.01, Alt.Sketch.FontStyle.Bold);

			box2DContainer.Paint += Box2DContainer_OnPaint;
			box2DContainer.MouseDown += RaiseBox2DMouseDown;
			box2DContainer.MouseUp += RaiseBox2DMouseUp;
			box2DContainer.MouseMove += RaiseBox2DMouseMove;

			box2DContainer.Focus();
		}


		protected override void OnPaint (PaintEventArgs e)
		{
			AltGUIHelper.Box2DContainer box2DContainer = Box2DContainer;
			if (box2DContainer == null)
			{
				return;
			}
			
			box2DContainer.DoubleBuffered = !e.Graphics.IsClippingSupported;
			if (box2DContainer.DoubleBuffered)
			{
				box2DContainer.Refresh();
			}
			
			DebugDraw.m_Graphics = e.Graphics;
			DebugDraw.m_Font = m_InfoFont;

			m_TPSCounter.Tick();

			base.OnPaint (e);
		}


		public Alt.Sketch.Vector2 ConvertScreenToWorld(double x, double y)
		{
			AltGUIHelper.Box2DContainer box2DContainer = Box2DContainer;
			if (box2DContainer == null)
			{
				return new Alt.Sketch.Vector2(x, y);
			}

			return box2DContainer.ConvertScreenToWorld(x, y);
		}
	}
}
