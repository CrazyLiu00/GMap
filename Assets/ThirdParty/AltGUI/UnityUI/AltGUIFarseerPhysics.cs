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
	/// AltGUI Farseer Physics Control.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUIFarseerPhysics.EditorName, AltGUIFarseerPhysics.EditorID)]
	public class AltGUIFarseerPhysics
		: AltGUIControlHost
    {
		new public const string EditorName = "Farseer Physics";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif
		
		
		AltGUIHelper.FarseerPhysicsContainer FarseerPhysicsContainer
		{
			get
			{
				return GwenChild as AltGUIHelper.FarseerPhysicsContainer;
			}
		}
		
		
		public Alt.GUI.FarseerPhysics.FarseerPhysicsRestartEvent onFarseerPhysicsRestartEvent
		{
			get
			{
				AltGUIHelper.FarseerPhysicsContainer farseerPhysicsContainer = FarseerPhysicsContainer;
				if (farseerPhysicsContainer == null)
				{
					return null;
				}		
				
				return farseerPhysicsContainer.onFarseerPhysicsRestartEvent;
			}
		}


		public Alt.GUI.FarseerPhysics.FarseerPhysicsEnableOrDisableFlagEvent onFarseerPhysicsEnableOrDisableFlag
		{
			get
			{
				AltGUIHelper.FarseerPhysicsContainer farseerPhysicsContainer = FarseerPhysicsContainer;
				if (farseerPhysicsContainer == null)
				{
					return null;
				}		
				
				return farseerPhysicsContainer.onFarseerPhysicsEnableOrDisableFlag;
			}
		}
		
		
		public Alt.GUI.FarseerPhysics.FarseerPhysicsKeyEvent onFarseerPhysicsKeyDown
		{
			get
			{
				AltGUIHelper.FarseerPhysicsContainer farseerPhysicsContainer = FarseerPhysicsContainer;
				if (farseerPhysicsContainer == null)
				{
					return null;
				}		
				
				return farseerPhysicsContainer.onFarseerPhysicsKeyDown;
			}
		}
		
		public Alt.GUI.FarseerPhysics.FarseerPhysicsKeyEvent onFarseerPhysicsKeyUp
		{
			get
			{
				AltGUIHelper.FarseerPhysicsContainer farseerPhysicsContainer = FarseerPhysicsContainer;
				if (farseerPhysicsContainer == null)
				{
					return null;
				}		
				
				return farseerPhysicsContainer.onFarseerPhysicsKeyUp;
			}
		}

		
		//  Mouse
		
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		MouseEvent m_onFarseerPhysicsMouseDown = new MouseEvent();
		public MouseEvent onFarseerPhysicsMouseDown
		{
			get
			{
				return m_onFarseerPhysicsMouseDown;
			}
			set
			{
				m_onFarseerPhysicsMouseDown = value;
			}
		}
		void RaiseFarseerPhysicsMouseDown(object sender, MouseEventArgs e)
		{
			m_onFarseerPhysicsMouseDown.Invoke(e);
		}
		
		
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		private MouseEvent m_onFarseerPhysicsMouseUp = new MouseEvent();
		public MouseEvent onFarseerPhysicsMouseUp
		{
			get
			{
				return m_onFarseerPhysicsMouseUp;
			}
			set
			{
				m_onFarseerPhysicsMouseUp = value;
			}
		}
		void RaiseFarseerPhysicsMouseUp(object sender, MouseEventArgs e)
		{
			m_onFarseerPhysicsMouseUp.Invoke(e);
		}
		
		
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		private MouseEvent m_onFarseerPhysicsMouseMove = new MouseEvent();
		public MouseEvent onFarseerPhysicsMouseMove
		{
			get
			{
				return m_onFarseerPhysicsMouseMove;
			}
			set
			{
				m_onFarseerPhysicsMouseMove = value;
			}
		}
		void RaiseFarseerPhysicsMouseMove(object sender, MouseEventArgs e)
		{
			m_onFarseerPhysicsMouseMove.Invoke(e);
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
		
		
		void FarseerPhysicsContainer_OnPaint(object sender, PaintEventArgs e)
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
			
			GwenChild = AltGUIHelper.Create_FarseerPhysicsContainer(GetOrCreateGwenCanvas());
			
			AltGUIHelper.FarseerPhysicsContainer farseerPhysicsContainer = FarseerPhysicsContainer;
			if (farseerPhysicsContainer == null)
			{
				return;
			}		

			farseerPhysicsContainer.Paint += FarseerPhysicsContainer_OnPaint;
			farseerPhysicsContainer.MouseDown += RaiseFarseerPhysicsMouseDown;
			farseerPhysicsContainer.MouseUp += RaiseFarseerPhysicsMouseUp;
			farseerPhysicsContainer.MouseMove += RaiseFarseerPhysicsMouseMove;
			
			farseerPhysicsContainer.Focus();
		}
		
		
		protected override void OnPaint (PaintEventArgs e)
		{
			AltGUIHelper.FarseerPhysicsContainer farseerPhysicsContainer = FarseerPhysicsContainer;
			if (farseerPhysicsContainer == null)
			{
				return;
			}
			
			farseerPhysicsContainer.DoubleBuffered = !e.Graphics.IsClippingSupported;
			if (farseerPhysicsContainer.DoubleBuffered)
			{
				farseerPhysicsContainer.Refresh();
			}

			base.OnPaint (e);
		}
	}
}
