//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;

using Alt;
using Alt.Backend.Unity;
using Alt.GUI;
using Alt.Sketch;


namespace UnityEngine.UI
{
	/// <summary>
	/// AltSketch Painting Control.
	/// </summary>
	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltSketchPaint.EditorName, AltSketchPaint.EditorID)]
	public class AltSketchPaint : MaskableGraphic, IDisposable
	{
		public const string EditorName = "AltSketch Paint";
		public const int EditorID = 779;
		
		
		class AltSketchPaintHandlerUnityUI : AltSketchPaintHandler
		{
			AltSketchPaint m_Paint;


			internal AltSketchPaintHandlerUnityUI(AltSketchPaint paint)
			{
				m_Paint = paint;
			}


			protected override Alt.Sketch.Rendering.RenderType RenderType
			{
				get
				{
					return m_Paint.RenderType;
				}
			}

			protected override Alt.Sketch.SizeI Size
			{
				get
				{
					return m_Paint.ClientSize.ToSizeI();
				}
			}


			protected override void RefreshMaterial()
			{
				m_Paint.RefreshMaterial = true;
			}

			protected override void RefreshVertices()
			{
				m_Paint.RefreshVertices = true;
			}


			protected override void DoPaint(Alt.GUI.PaintEventArgs e)
			{
				m_Paint.DoPaint(e);
			}
		}


		AltSketchPaintHandler m_AltSketchPaintHandler;
		AltSketchPaintHandler AltSketchPaintHandler
		{
			get
			{
				if (m_AltSketchPaintHandler == null)
				{
					m_AltSketchPaintHandler = new AltSketchPaintHandlerUnityUI(this);

					Invalidate();
				}

				return m_AltSketchPaintHandler;
			}
		}

		
		protected override void Start ()
		{
			AltGUIIntegration.Initialize();

			Invalidate();

			base.Start ();
		}


		/// <summary>
		/// Disposes the RenderBox.
		/// </summary>
		/// <param name="disposing">Whether Dispose has been called.</param>
		public virtual void Dispose()
		{
			if (m_AltSketchPaintHandler != null)
			{
				m_AltSketchPaintHandler.Dispose();
				m_AltSketchPaintHandler = null;
			}
		}
		
		protected override void OnDestroy()
		{
			if (m_AltSketchPaintHandler != null)
			{
				m_AltSketchPaintHandler.Dispose();
				m_AltSketchPaintHandler = null;
			}

			base.OnDestroy();
		}

		
		[SerializeField]
		Alt.Sketch.Rendering.RenderType m_RenderType = Alt.Sketch.Rendering.RenderType.Software;
		public Alt.Sketch.Rendering.RenderType RenderType
		{
			get
			{
				return m_RenderType;
			}
			set
			{
				if (m_RenderType == value)
				{
					return;
				}
				
				m_RenderType = value;
				
				Invalidate();
			}
		}


		IntervalTimer m_RenderUpdateTimer = new IntervalTimer(1000 / UnityEngine.AltSketchPaintHandler.DefaultMaximumFPS);
		[SerializeField]
		int m_MaxFPS = UnityEngine.AltSketchPaintHandler.DefaultMaximumFPS;
		public int MaxFPS
		{
			get
			{
				return m_MaxFPS;
			}
			set
			{
				if (m_MaxFPS == value)
				{
					return;
				}
				
				m_MaxFPS = value;

				m_RenderUpdateTimer.Interval = 1000 / m_MaxFPS;
			}
		}

		
		public double RealFPS
		{
			get
			{
				return AltSketchPaintHandler.RealFPS;
			}
		}
		
		
		/// <summary>
		/// Returns the texture used to draw this Graphic.
		/// </summary>
		public override Texture mainTexture
		{
			get
			{
				DoRenderIfNeed();
				
				return AltSketchPaintHandler.mainTexture ?? s_WhiteTexture;
			}
		}


		/// <summary>
		/// UV rectangle used by the texture.
		/// </summary>
		public UnityEngine.Rect UVRect
		{
			get
			{
				return AltSketchPaintHandler.UVRect;
			}
		}


		#if UNITY_EDITOR
		// This is Unity's own OnValidate method which is invoked when changing values in the Inspector.
		protected override void OnValidate()
		{
			base.OnValidate();

			m_RenderUpdateTimer.Interval = 1000 / m_MaxFPS;
			
			Invalidate();
		}		
		#endif

		
		
		/// <summary>
		/// Adjust the scale of the Graphic to make it pixel-perfect.
		/// </summary>
		public override void SetNativeSize()
		{
			Texture tex = mainTexture;
			if (tex != null)
			{
				UnityEngine.Rect uvRect = UVRect;

				int w = Mathf.RoundToInt(tex.width * uvRect.width);
				int h = Mathf.RoundToInt(tex.height * uvRect.height);

				rectTransform.anchorMax = rectTransform.anchorMin;
				rectTransform.sizeDelta = new UnityEngine.Vector2(w, h);
			}
		}
		
		
		/// <summary>
		/// Update all renderer data.
		/// </summary>
		protected override void OnFillVBO(List<UIVertex> vbo)
		{
			Texture tex = mainTexture;
			
			if (tex != null)
			{
				UnityEngine.Vector4 v = UnityEngine.Vector4.zero;

				UnityEngine.Rect uvRect = UVRect;
				
				int w = Mathf.RoundToInt(tex.width * uvRect.width);
				int h = Mathf.RoundToInt(tex.height * uvRect.height);
				
				float paddedW = ((w & 1) == 0) ? w : w + 1;
				float paddedH = ((h & 1) == 0) ? h : h + 1;
				
				v.x = 0f;
				v.y = 0f;
				v.z = w / paddedW;
				v.w = h / paddedH;
				
				v.x -= rectTransform.pivot.x;
				v.y -= rectTransform.pivot.y;
				v.z -= rectTransform.pivot.x;
				v.w -= rectTransform.pivot.y;
				
				v.x *= rectTransform.rect.width;
				v.y *= rectTransform.rect.height;
				v.z *= rectTransform.rect.width;
				v.w *= rectTransform.rect.height;
				
				vbo.Clear();
				
				var vert = UIVertex.simpleVert;
				
				vert.position = new UnityEngine.Vector2(v.x, v.y);
				vert.uv0 = new  UnityEngine.Vector2(uvRect.xMin, uvRect.yMin);
				vert.color = color;
				vbo.Add(vert);
				
				vert.position = new  UnityEngine.Vector2(v.x, v.w);
				vert.uv0 = new  UnityEngine.Vector2(uvRect.xMin, uvRect.yMax);
				vert.color = color;
				vbo.Add(vert);
				
				vert.position = new  UnityEngine.Vector2(v.z, v.w);
				vert.uv0 = new  UnityEngine.Vector2(uvRect.xMax, uvRect.yMax);
				vert.color = color;
				vbo.Add(vert);
				
				vert.position = new  UnityEngine.Vector2(v.z, v.y);
				vert.uv0 = new  UnityEngine.Vector2(uvRect.xMax, uvRect.yMin);
				vert.color = color;
				vbo.Add(vert);
			}
		}
		
				
		bool m_ForceRender = false;
		bool RefreshMaterial = false;
		bool RefreshVertices = false;
		protected virtual void Update()
		{
			//AltGUIIntegration.Tick();
			
			if (RefreshMaterial)
			{
				RefreshMaterial = false;
				
				SetMaterialDirty();
			}
			
			if (RefreshVertices)
			{
				RefreshVertices = false;
				
				SetVerticesDirty();
			}

            if (m_RenderUpdateTimer.IsTimeOver
                //TEST - Prevent m_RenderUpdateTimer.IsTimeOver delay
                || AltSketchPaintHandler.MSecSinceLastRender >= m_RenderUpdateTimer.Interval
                || m_ForceRender)
            {
                if (AltSketchPaintHandler.NeedsUpdate || m_ForceRender)
                {
                    CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
                }
            }
        }

        public void ReflashCanvas()
        {
            if (AltSketchPaintHandler.MSecSinceLastRender >= m_RenderUpdateTimer.Interval ||
                !CanvasUpdateRegistry.IsRebuildingGraphics())
            {
                CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
            }
        }


        public void Invalidate()
		{
			AltSketchPaintHandler.Invalidate();


            //TEST - Prevent m_RenderUpdateTimer.IsTimeOver delay

            //if (AltSketchPaintHandler.MSecSinceLastRender >= m_RenderUpdateTimer.Interval ||
            //    !CanvasUpdateRegistry.IsRebuildingGraphics())
            //{
            //    CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(this);
            //}
        }


        public override void Rebuild(CanvasUpdate update)
		{
			switch (update)
			{
			case CanvasUpdate.PreRender:
				DoRenderIfNeed();
				break;
			}
			
			base.Rebuild(update);
		}

		
		//  Paint
		
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
		}
		
		protected virtual void OnPaintBackground(Alt.GUI.PaintEventArgs e)
		{
		}
		

		void DoRenderIfNeed()
		{
			if (m_ForceRender)
			{
				Invalidate();
				m_ForceRender = false;
			}
			
			if (AltSketchPaintHandler.DoRenderIfNeed(GetComponent<Camera>(), GetComponent<Renderer>()))
			{
				m_RenderUpdateTimer.Reset();
			}
		}
		
		void DoPaint(Alt.GUI.PaintEventArgs e)
		{
			#if UNITY_EDITOR
			if (Application.isPlaying)
			#endif
			{
				OnPaintBackground(e);
			
				OnPaint(e);
			}

			if (!e.Handled)
			{
				AltGUIControlHost host = gameObject.GetComponentInParent<AltGUIControlHost>();
				if (host != null)
				{
					host.DoPaint(e);

					#if UNITY_EDITOR
					if (!Application.isPlaying)
					{
						return;
					}
					#endif
				}
				#if UNITY_EDITOR
				else if (!Application.isPlaying)
				{
					AltSketchPaintHandler.DrawEditorText(e,
					                                     EditorName,
					                                     gameObject != null ? gameObject.name : "Unknown",
					                                     ClientSize,
					                                     MaxFPS,
					                                     RenderType);
					return;
				}
				#endif
				
				if (m_Paint != null)
				{
					m_Paint(this, e);
				}
				
				m_onPaint.Invoke(e);
			}
		}
		
		
		
		public double ClientWidth
		{
			get
			{
				return Mathf.RoundToInt(Mathf.Abs(rectTransform.rect.width));
			}
		}
		
		public double ClientHeight
		{
			get
			{
				return Mathf.RoundToInt(Mathf.Abs(rectTransform.rect.height));
			}
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
				return new Alt.Sketch.Rect(Point.Zero, ClientSize);
			}
		}
	}
}
