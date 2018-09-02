//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

//#define TRY_CLIPPINGRECT


using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Alt.Sketch;
using Alt.Sketch.Rendering;

using UnityEngine;


namespace Alt.Backend.Unity
{
    interface IUnity_BrushMaterial
    {
		Material GetMaterial(RenderPrimitive renderPrimitive);
    }


    public class Unity_Renderer : Alt.Sketch.Rendering.Renderer
    {
        public Unity_RenderManager Unity_RenderManager
        {
            get
            {
                return RenderManager as Unity_RenderManager;
            }
		}


		//NoNeed	UnityEngine.RenderTexture m_RenderTexture;

		UnityEngine.Renderer m_CurrentUnityEngineRenderer;
		//NoNeed	UnityEngine.Camera m_CurrentUnityEngineCamera;


		public Unity_Renderer(Unity_RenderManager renderManager) :
			this(renderManager, null)
		{
		}

		public Unity_Renderer(Unity_RenderManager renderManager, UnityEngine.RenderTexture renderTexture) :
			base(renderManager, Alt.Sketch.Graphics.RSN_Unity)
        {
			//NoNeed	m_RenderTexture = renderTexture;

            TransformMatrixSupported = true;

            RenderLinesByPolygons = renderManager.RenderLinesByPolygons;

            //  Now supports only 1 texture (need more shaders researches)
            MaxTextureCountPerBrushMaterial = 1;// renderManager.MaxTextureCountPerBrushMaterial;
            MaxTextureCoordsSetCountPerBrushMaterial = 1;// renderManager.MaxTextureCoordsSetCountPerBrushMaterial;

            NonPowerOf2TexturesSupported = renderManager.NonPowerOf2TexturesSupported;

            //  We cah't use NeoAxis RenderToTexture yet, so use Software render
            RenderToTextureSupported = renderManager.RenderToTextureSupported;

            //  Can't to set Texture Matrix yet
            TextureMatrixSupported = false;// renderManager.TextureMatrixSupported;

			//  Clipping is not supported (We can't get ScissorRect or Viewport to work...)
            ClippingSupported =
#if TRY_CLIPPINGRECT
				renderManager.ClippingSupported;
#else
				false;
#endif

            SizeI maxTextureSize = renderManager.MaxTextureSize;
            //  LinearGradientTextureMaxSize
            {
                SizeI baseLinearGradientTextureMaxSize = base.LinearGradientTextureMaxSize;

                LinearGradientTextureMaxSize = new SizeI(
                    System.Math.Min(maxTextureSize.Width, baseLinearGradientTextureMaxSize.Width),
                    System.Math.Min(maxTextureSize.Height, baseLinearGradientTextureMaxSize.Height));
            }
            //  RadialGradientTextureMaxSize
            {
                SizeI baseRadialGradientTextureMaxSize = base.RadialGradientTextureMaxSize;

                RadialGradientTextureMaxSize = new SizeI(
                    System.Math.Min(maxTextureSize.Width, baseRadialGradientTextureMaxSize.Width),
                    System.Math.Min(maxTextureSize.Height, baseRadialGradientTextureMaxSize.Height));
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                }

                // There are no unmanaged resources to release, but
                // if we add them, they need to be released here.
            }

            base.Dispose(disposing);
        }


        protected override RenderPrimitive CreateRenderPrimitive()
        {
            return new Unity_RenderPrimitive();
        }

		
		public void BeginRender(UnityEngine.Camera camera, UnityEngine.Renderer renderer, int width, int height)
		{
			BeginRender(camera, renderer, width, height, false);
		}
		bool m_flipY = false;
		public void BeginRender(UnityEngine.Camera camera, UnityEngine.Renderer renderer, int width, int height, bool flipY)
		{
			m_flipY = flipY;

			m_CurrentUnityEngineRenderer = renderer;
			//NoNeed	m_CurrentUnityEngineCamera = camera;

			RenderTargetSize = new SizeI(width, height);

			BeginRender();
		}

        public override void BeginRender()
        {
            base.BeginRender();

            PrepareMatrices();
        }


        public override void EndRender()
        {
            //  Render cached data
            base.EndRender();

            RestoreMatrices();
        }


        Alt.Sketch.Rect m_LastClipRectangle = Alt.Sketch.Rect.FromLTRB(-1, -1, 1, 1);
		protected override void SetClipRectangle(Alt.Sketch.Rect rectangle)
        {
			rectangle = new Alt.Sketch.Rect(
				(int)(rectangle.Left + 0.5),
				(int)(rectangle.Top + 0.5),
				(int)(rectangle.Width + 0.5),
				(int)(rectangle.Height + 0.5));

			if (m_LastClipRectangle != rectangle)
			{
				m_LastClipRectangle = rectangle;
#if TRY_CLIPPINGRECT
				UpdateMatrices();
#endif
			}
        }


        protected override void Render(BrushMaterial brushMaterial, RenderPrimitive renderPrimitive)
        {
            Render(brushMaterial, renderPrimitive as Unity_RenderPrimitive);
        }

        void Render(BrushMaterial brushMaterial, Unity_RenderPrimitive renderPrimitive)
        {
            if (brushMaterial == null ||
                renderPrimitive == null)
            {
                return;
			}
			
			if (renderPrimitive.PreRender())
			{
				Material material = (brushMaterial as IUnity_BrushMaterial).GetMaterial(renderPrimitive);
				if (material != null)
				{
					ColorR color = ColorR.White;
					if (brushMaterial is SolidColorBrushMaterial)
					{
						color = (brushMaterial as SolidColorBrushMaterial).Color;
					}
					
					double alpha = color.A * brushMaterial.Opacity * brushMaterial.RenderContext.Opacity;
					color.A = alpha;
					material.color = new UnityEngine.Color((float)color.R, (float)color.G, (float)color.B, (float)color.A);
					

					Alt.Sketch.Matrix4 mat = brushMaterial.FinalTransform;
					//bool matIsIdentity = mat.IsIdentity;
					//if (!matIsIdentity)
					{
						GL.PushMatrix();

						//NoNeed	SizeI renderTargetSize = RenderTargetSize;
						if (renderPrimitive.PrimitiveTypeIsLines)
						{
							mat = mat * m_WorldMatrixLines;
						}
						else
						{
							mat = mat * m_WorldMatrix;
						}
						mat.Transpose();

						GL.MultMatrix(Unity_RenderManager.ToMatrix(mat));
					}


					try
					{
						if (m_CurrentUnityEngineRenderer != null)
						{
							m_CurrentUnityEngineRenderer.material = material;

							//	material already setted, so we passed null
							renderPrimitive.Render(null);
						}
						else
						{
							renderPrimitive.Render(material);
						}
					}
					catch
					{
					}
					finally
					{
						//if (!matIsIdentity)
						{
							GL.PopMatrix();
						}
					}
				}
			}
			renderPrimitive.PostRender();
        }


        //  Brushes

        protected override SolidColorBrushMaterial CreateSolidColorBrushMaterial()
        {
            return new Unity_SolidColorBrushMaterial();
        }

        protected override LinearGradientBrushMaterial CreateLinearGradientBrushMaterial()
        {
            return new Unity_LinearGradientBrushMaterial();
        }

        protected override RadialGradientBrushMaterial CreateRadialGradientBrushMaterial()
        {
            return new Unity_RadialGradientBrushMaterial();
        }

        protected override ImageBrushMaterial CreateImageBrushMaterial()
        {
            return new Unity_ImageBrushMaterial();
        }

        protected override ExtBrushMaterial GetOrCreateExtBrushMaterial()
        {
            return new Unity_ExtBrushMaterial();
        }
		

		//	Materials
		string defaultMaterialShader =
			//null;
			"Sprites/Default";
			//"Sprites/Diffuse";

		Material m_SolidColorBrushMaterial_Solid;
		internal Material SolidColorBrushMaterial_Solid
		{
			get
			{
				if (m_SolidColorBrushMaterial_Solid == null)
				{
					Material material = new Material(Shader.Find (defaultMaterialShader ?? "Custom/AltSketch.SolidColorBrushMaterial_Solid"));
					m_SolidColorBrushMaterial_Solid = material;
				}
				
				//  update
				//NoNeed	m_SolidColorBrushMaterial_Solid.mainTexture = (Unity_RenderManager.SolidTexture as Unity_Texture).Native;

				return m_SolidColorBrushMaterial_Solid;
			}
		}
		
		
		Material m_SolidColorBrushMaterial_AA;
		internal Material SolidColorBrushMaterial_AA
		{
			get
			{
				if (m_SolidColorBrushMaterial_AA == null)
				{
					Material material = new Material(Shader.Find (defaultMaterialShader ?? "Custom/AltSketch.SolidColorBrushMaterial_AA"));
					m_SolidColorBrushMaterial_AA = material;
				}

				//  update
				m_SolidColorBrushMaterial_AA.mainTexture = (Unity_RenderManager.AATexture as Unity_Texture).Native;
				m_SolidColorBrushMaterial_AA.mainTexture.wrapMode = TextureWrapMode.Clamp;
				m_SolidColorBrushMaterial_AA.mainTexture.filterMode = FilterMode.Bilinear;
									
				return m_SolidColorBrushMaterial_AA;
			}
		}
		
		
		Material m_ImageBrushMaterial_Solid;
		internal Material ImageBrushMaterial_Solid
		{
			get
			{
				if (m_ImageBrushMaterial_Solid == null)
				{
					Material material = new Material(Shader.Find (defaultMaterialShader ?? "Custom/AltSketch.ImageBrushMaterial_Solid"));
					m_ImageBrushMaterial_Solid = material;
				}
				
				return m_ImageBrushMaterial_Solid;
			}
		}
		
		
		Material m_ImageBrushMaterial_AA;
		internal Material ImageBrushMaterial_AA
		{
			get
			{
				if (m_ImageBrushMaterial_AA == null)
				{
					Material material = new Material(Shader.Find (defaultMaterialShader ?? "Custom/AltSketch.ImageBrushMaterial_AA"));
					m_ImageBrushMaterial_AA = material;
				}

				return m_ImageBrushMaterial_AA;
			}
		}


        
		//  Transform

		Matrix4 m_WorldMatrix;
		Matrix4 m_WorldMatrixLines;
		protected virtual void PrepareMatrices()
        {
			GL.PushMatrix();
			GL.LoadIdentity();

			UpdateMatrices();
		}

		void UpdateMatrices()
		{
			SizeI renderTargetSize = RenderTargetSize;
			SizeI viewSize =
#if TRY_CLIPPINGRECT
				!m_LastClipRectangle.IsZero ? m_LastClipRectangle.Size.ToSizeI() :
#endif
				renderTargetSize;

			if (RenderTexture.active == null)
			{
				GL.LoadPixelMatrix (0, viewSize.Width, 0, viewSize.Height);
			}
			else
			{
				if (m_flipY)
				{
					GL.LoadPixelMatrix (0, viewSize.Width, 0, viewSize.Height);
				}
				else
				{
					GL.LoadPixelMatrix (0, viewSize.Width, viewSize.Height, 0);
				}
			}

#if TRY_CLIPPINGRECT
			if (m_LastClipRectangle.IsZero)
			{
				GL.Viewport(new UnityEngine.Rect(0, 0, (float)renderTargetSize.Width, (float)renderTargetSize.Height));
			}
			else
			{
				GL.Viewport(new UnityEngine.Rect(
					(float)m_LastClipRectangle.X, (float)m_LastClipRectangle.Y,
					(float)m_LastClipRectangle.Width, (float)m_LastClipRectangle.Height));
			}
#endif

			m_WorldMatrix = Matrix4.Identity;
			m_WorldMatrix.SetTranslation(0, viewSize.Height, 0);
			m_WorldMatrix.SetScale(
				1.0,
				-1.0,
				1);
			
			m_WorldMatrixLines =
				//m_WorldMatrix;
				Matrix4.Mult(Matrix4.CreateTranslation(0.5, 0.5), m_WorldMatrix);
		}

        protected virtual void RestoreMatrices()
        {
			GL.PopMatrix();
		}
    }
}
