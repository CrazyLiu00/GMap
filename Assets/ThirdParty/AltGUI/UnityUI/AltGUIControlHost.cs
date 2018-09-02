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
	/// AltGUI Control Host.
	/// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUIControlHost.EditorName, AltGUIControlHost.EditorID)]
    public partial class AltGUIControlHost
		: Selectable, IDisposable,
        IUpdateSelectedHandler,
        IBeginDragHandler,
        IDragHandler,
        IEndDragHandler,
        //IPointerClickHandler
		IScrollHandler
    {
		public const string EditorName = "AltGUI Control Host";
		public const int EditorID = 777;

		#if UNITY_EDITOR
		internal virtual string GetEditorName()
		{
			return EditorName;
		}
		#endif


		protected override void Start ()
		{
			AltGUIIntegration.Initialize();

			base.Start ();

			ProcessResize();
		}


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


		void Child_Invalidated(object sender, InvalidateEventArgs e)
		{
			Invalidate();
		}

		public void Invalidate()
		{
			AltSketchPaint paint = paintComponent;
			if (paint != null)
			{
				paint.Invalidate();
			}
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



		public Alt.Sketch.Rendering.RenderType RenderType
		{
			get
			{
				AltSketchPaint paint = paintComponent;
				if (paint != null)
				{
					return paint.RenderType;
				}
				
				return Alt.Sketch.Rendering.RenderType.Software;
			}
			set
			{
				AltSketchPaint paint = paintComponent;
				if (paint != null)
				{
					paint.RenderType = value;
				}
			}
		}
		
		
		public int MaxFPS
		{
			get
			{
				AltSketchPaint paint = paintComponent;
				if (paint != null)
				{
					return paint.MaxFPS;
				}
				
				return UnityEngine.AltSketchPaintHandler.DefaultMaximumFPS;
			}
			set
			{
				AltSketchPaint paint = paintComponent;
				if (paint != null)
				{
					paint.MaxFPS = value;
				}
			}
		}
		
		
		/// <summary>
		/// AltSketch Paint used to display the paint data.
        /// </summary>
		[SerializeField]
        //???[FormerlySerializedAs("paint")]
		protected AltSketchPaint m_PaintComponent;


		public void Dispose()
		{
			GwenChild = null;
			Child = null;
		}

		protected override void OnDestroy()
		{
			GwenChild = null;
			Child = null;

			base.OnDestroy();
		}


		bool m_NeedInit = true;
		Alt.Sketch.Size m_TempClientSize = Alt.Sketch.Size.Empty;
		protected virtual void Update()
		{
			if (paintComponent != null)
			{
				ProcessResize();

				if (m_NeedInit)
				{
					m_NeedInit = false;

					OnLoad();
				}
			}

			ProcessMouse();
			ProcessKeyboard();

			AltGUIIntegration.Tick();
		}

		void ProcessResize()
		{
			if (m_TempClientSize != paintComponent.ClientSize)
			{
				m_TempClientSize = paintComponent.ClientSize;
				
				RaiseResize(new ResizeEventArgs(ClientSize));
			}
		}


		public AltSketchPaint paintComponent
		{
			get
			{
				return m_PaintComponent;
			}
			set
			{
				if (m_PaintComponent == value)
				{
					return;
				}

				m_PaintComponent = value;
			}
		}
		

#if UNITY_EDITOR
		protected void EditorPaint(PaintEventArgs e)
		{
			if (!Application.isPlaying)
			{
				AltSketchPaint paintComponent = this.paintComponent;
				AltSketchPaintHandler.DrawEditorText(e,
				                                     GetEditorName(),
				                                     gameObject != null ? gameObject.name : "Unknown",
				                                     ClientSize,
				                                     paintComponent != null ? paintComponent.MaxFPS : -1,
				                                     paintComponent != null ? paintComponent.RenderType : Alt.Sketch.Rendering.RenderType.Software);
				return;
			}
		}
#endif
		internal virtual void DoPaint(PaintEventArgs e)
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
			
			if (!e.Handled)
			{
				RaisePaint(e);
			}
		}
		protected void RaisePaint(PaintEventArgs e)
		{
			if (m_Paint != null)
			{
				m_Paint(this, e);
			}
			
			m_onPaint.Invoke(e);
		}

		event Alt.GUI.PaintEventHandler m_Paint;
		public event Alt.GUI.PaintEventHandler Paint
		{
			add
			{
				m_Paint += value;
			}
			remove
			{
				m_Paint -= value;
			}
		}
		
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		private PaintEvent m_onPaint = new PaintEvent();
		public PaintEvent onPaint
		{
			get
			{
				return m_onPaint;
			}
			set
			{
				m_onPaint = value;
			}
		}

		protected virtual void OnPaint(Alt.GUI.PaintEventArgs e)
		{
			ProcessResize();

			if (Child != null)
			{
				Child.InjectRender(e);
			}
		}
		
		protected virtual void OnPaintBackground(Alt.GUI.PaintEventArgs e)
		{
		}



    	#if UNITY_EDITOR
        // This is Unity's own OnValidate method which is invoked when changing values in the Inspector.
        protected override void OnValidate()
        {
            base.OnValidate();
            
			Invalidate();
        }
    	#endif


        protected override void OnEnable()
        {
            base.OnEnable();

			if (Child != null)
			{
				Child.Enabled = true;
			}
        }

        protected override void OnDisable()
        {
            //DeactivateInputField();

			if (Child != null)
			{
				Child.Enabled = false;
			}

            base.OnDisable();
        }



		bool m_IsSelected = false;
		public bool isSelected
		{
			get
			{
				return m_IsSelected;
			}
		}

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);

			if (Child != null)
			{
				Child.Focus();
			}

			m_IsSelected = true;
        }


        public override void OnDeselect(BaseEventData eventData)
        {
			m_IsSelected = false;

            base.OnDeselect(eventData);
        }
		
		
		
		//  Mouse

		protected virtual void OnMouseDown(MouseEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectMouseDown(e);
			}
		}
		public MouseEventHandler MouseDown;
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		private MouseEvent m_onMouseDown = new MouseEvent();
		public MouseEvent onMouseDown
		{
			get
			{
				return m_onMouseDown;
			}
			set
			{
				m_onMouseDown = value;
			}
		}
		void RaiseMouseDown(MouseEventArgs e)
		{
			OnMouseDown(e);
			
			if (MouseDown != null)
			{
				MouseDown(this, e);
			}

			m_onMouseDown.Invoke(e);
		}


		protected virtual void OnMouseUp(MouseEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectMouseUp(e);
			}
		}
		public MouseEventHandler MouseUp;
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		private MouseEvent m_onMouseUp = new MouseEvent();
		public MouseEvent onMouseUp
		{
			get
			{
				return m_onMouseUp;
			}
			set
			{
				m_onMouseUp = value;
			}
		}
		void RaiseMouseUp(MouseEventArgs e)
		{
			OnMouseUp(e);
			
			if (MouseUp != null)
			{
				MouseUp(this, e);
			}
			
			m_onMouseUp.Invoke(e);
		}


		protected virtual void OnMouseMove(MouseEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectMouseMove(e);
			}
		}
		public MouseEventHandler MouseMove;
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		private MouseEvent m_onMouseMove = new MouseEvent();
		public MouseEvent onMouseMove
		{
			get
			{
				return m_onMouseMove;
			}
			set
			{
				m_onMouseMove = value;
			}
		}
		void RaiseMouseMove(MouseEventArgs e)
		{
			OnMouseMove(e);
			
			if (MouseMove != null)
			{
				MouseMove(this, e);
			}
			
			m_onMouseMove.Invoke(e);
		}
		
		
		protected virtual void OnMouseWheel(MouseEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectMouseWheel(e);
			}
		}
		public MouseEventHandler MouseWheel;
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		private MouseEvent m_onMouseWheel = new MouseEvent();
		public MouseEvent onMouseWheel
		{
			get
			{
				return m_onMouseWheel;
			}
			set
			{
				m_onMouseWheel = value;
			}
		}
		void RaiseMouseWheel(MouseEventArgs e)
		{
			OnMouseWheel(e);
			
			if (MouseWheel != null)
			{
				MouseWheel(this, e);
			}
			
			m_onMouseWheel.Invoke(e);
		}
		
		
		protected virtual void OnMouseEnter(MouseEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectMouseEnter();//e);
			}
		}
		public MouseEventHandler MouseEnter;
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		private MouseEvent m_onMouseEnter = new MouseEvent();
		public MouseEvent onMouseEnter
		{
			get
			{
				return m_onMouseEnter;
			}
			set
			{
				m_onMouseEnter = value;
			}
		}
		void RaiseMouseEnter(MouseEventArgs e)
		{
			OnMouseEnter(e);
			
			if (MouseEnter != null)
			{
				MouseEnter(this, e);
			}
			
			m_onMouseEnter.Invoke(e);
		}
		
		
		protected virtual void OnMouseLeave(MouseEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectMouseLeave();//e);
			}
		}
		public MouseEventHandler MouseLeave;
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		private MouseEvent m_onMouseLeave = new MouseEvent();
		public MouseEvent onMouseLeave
		{
			get
			{
				return m_onMouseLeave;
			}
			set
			{
				m_onMouseLeave = value;
			}
		}
		void RaiseMouseLeave(MouseEventArgs e)
		{
			OnMouseLeave(e);
			
			if (MouseLeave != null)
			{
				MouseLeave(this, e);
			}
			
			m_onMouseLeave.Invoke(e);
		}



		//  Keyboard
		
		public static KeyEventArgs ToKeyEventArgs(Event e)
		{
			return new KeyEventArgs((Keys)e.keyCode);
		}
		
		public static KeyPressEventArgs ToKeyPressEventArgs(Event e)
		{
			return new KeyPressEventArgs(e.character);
		}


		protected virtual void OnKeyDown(KeyEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectKeyDown(e);
			}
		}
		public KeyEventHandler KeyDown;
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		private KeyEvent m_onKeyDown = new KeyEvent();
		public KeyEvent onKeyDown
		{
			get
			{
				return m_onKeyDown;
			}
			set
			{
				m_onKeyDown = value;
			}
		}
		void RaiseKeyDown(KeyEventArgs e)
		{
			OnKeyDown(e);
			
			if (KeyDown != null)
			{
				KeyDown(this, e);
			}

			m_onKeyDown.Invoke(e);
		}


		protected virtual void OnKeyUp(KeyEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectKeyUp(e);
			}
		}
		public KeyEventHandler KeyUp;
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		private KeyEvent m_onKeyUp = new KeyEvent();
		public KeyEvent onKeyUp
		{
			get
			{
				return m_onKeyUp;
			}
			set
			{
				m_onKeyUp = value;
			}
		}
		void RaiseKeyUp(KeyEventArgs e)
		{
			OnKeyUp(e);
			
			if (KeyUp != null)
			{
				KeyUp(this, e);
			}

			m_onKeyUp.Invoke(e);
		}


		protected virtual void OnKeyPress(Alt.GUI.KeyPressEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectKeyPress(e);
			}
		}
		public KeyPressEventHandler KeyPress;
		// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
		[SerializeField]
		private KeyPressEvent m_onKeyPress = new KeyPressEvent();
		public KeyPressEvent onKeyPress
		{
			get
			{
				return m_onKeyPress;
			}
			set
			{
				m_onKeyPress = value;
			}
		}
		void RaiseKeyPress(Alt.GUI.KeyPressEventArgs e)
		{
			OnKeyPress(e);
			
			if (KeyPress != null)
			{
				KeyPress(this, e);
			}

			m_onKeyPress.Invoke(e);
		}
		
		
		
		//  OnLoad
		
		protected virtual void OnLoad()
		{
		}

			
		
		//  OnResize

		public double ClientWidth
		{
			get
			{
				if (paintComponent != null)
				{
					return paintComponent.ClientWidth;
				}

				return m_TempClientSize.Width;
			}
		}
		public double ClientHeight
		{
			get
			{
				if (paintComponent != null)
				{
					return paintComponent.ClientHeight;
				}
				
				return m_TempClientSize.Height;
			}
		}
		public Alt.Sketch.Size ClientSize
		{
			get
			{
				if (paintComponent != null)
				{
					return paintComponent.ClientSize;
				}
				
				return m_TempClientSize;
			}
		}
		public Alt.Sketch.Rect ClientRectangle
		{
			get
			{
				if (paintComponent != null)
				{
					return paintComponent.ClientRectangle;
				}
				
				return new Alt.Sketch.Rect(0, 0, ClientSize);
			}
		}


		protected virtual void OnResize(ResizeEventArgs e)
		{
			if (Child != null)
			{
				Child.InjectResize(e);
			}
		}
		public ResizeEventHandler Resize;
		void RaiseResize(ResizeEventArgs e)
		{
			OnResize(e);
			
			if (Resize != null)
			{
				Resize(this, e);
			}
		}
    }
}
