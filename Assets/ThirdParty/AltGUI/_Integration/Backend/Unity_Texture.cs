//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;


namespace Alt.Backend.Unity
{
	class Unity_Texture : Alt.Sketch.Rendering.Texture
	{
		protected Unity_RenderManager m_RenderManager;
		
		
		public Texture Native
		{
			get
			{
				return Texture;
			}
		}
		
		
		protected const string NamePrefix = "AltSketchTexture_";
		
		
		public Texture m_Texture;
		public Texture Texture
		{
			get
			{
				if (!IsImageAlive)
				{
					return null;
				}
				
				if (m_Texture == null ||
				    ModificationTime != ImageSource.ModificationTime)
				{
					UpdateTexture();
				}
				
				
				return m_Texture;
			}
		}
		
		
		internal Unity_Texture(Unity_RenderManager renderManager, int width, int height, Alt.Sketch.PixelFormat format) :
			base(width, height, format)
		{
			m_RenderManager = renderManager;
		}
		
		
		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed)
			{
				if (disposing)
				{
				}
				
				KillTexture();
			}
			
			base.Dispose(disposing);
		}
		
		
		protected void KillTexture()
		{
			if (m_Texture == null)
			{
				return;
			}
			
			if (m_Texture is RenderTexture)
			{
				(m_Texture as RenderTexture).Release();
			}
			
			//m_Texture.Destroy();
			
			m_Texture = null;
		}
		
		
		protected virtual void UpdateTexture()
		{
			if (!IsImageAlive)
			{
				KillTexture();
				return;
			}
			
			
			Alt.Sketch.Bitmap bitmap = ImageSourceAsBitmap;
			if (ModificationTime == bitmap.ModificationTime)
			{
				return;
			}
			
			
			if (SrcWidth != bitmap.PixelWidth ||
			    SrcHeight != bitmap.PixelHeight ||
			    PixelFormat != bitmap.PixelFormat)
			{
				KillTexture();
			}
			
			
			if (SrcWidth < 1 ||
			    SrcHeight < 1 ||
			    PixelFormat == Alt.Sketch.PixelFormat.Unknown)
			{
				return;
			}
			
			
			//int bpp = bitmap.ByteDepth;
			if (!m_RenderManager.NonPowerOf2TexturesSupported)
			{
				TextureWidth = NextPowerOfTwo(SrcWidth);
				TextureHeight = NextPowerOfTwo(SrcHeight);
				
				if (TextureWidth != SrcWidth ||
				    TextureHeight != SrcHeight)
				{
					bitmap = bitmap.GetScaledCopy(TextureWidth, TextureHeight);
					if (bitmap == null ||
					    !bitmap.IsValid)
					{
						return;
					}
				}
			}
			else
			{
				TextureWidth = SrcWidth;
				TextureHeight = SrcHeight;
			}
			
			
			if (m_Texture == null)
			{
				// Create a new texture
				m_Texture = new Texture2D(TextureWidth, TextureHeight, Unity_RenderManager.ToTextureFormat(PixelFormat), false);
				m_Texture.filterMode = FilterMode.Point;
				
				if (m_Texture != null)
				{
					TextureWidth = m_Texture.width;
					TextureHeight = m_Texture.height;
				}
			}
			
			
			SetTextureData(bitmap, bitmap.PixelRectangle);
		}
		
		
		protected override void SetTextureData(Sketch.Bitmap src, Sketch.RectI srcRect)
		{
			Unity_RenderManager.SetTextureData(m_Texture as UnityEngine.Texture2D, src, srcRect);
			
			ModificationTime = src.ModificationTime;
		}
	}
	
	
	
	class Unity_RenderTexture : Unity_Texture
	{
		Unity_Renderer m_Renderer;
		
		
		internal Unity_RenderTexture(Unity_RenderManager renderManager, int width, int height, Alt.Sketch.PixelFormat format) :
			base(renderManager, width, height, format)
		{
		}
		
		
		protected override void Dispose(bool disposing)
		{
			if (m_Renderer != null)
			{
				m_Renderer.Dispose();
				m_Renderer = null;
			}
			
			base.Dispose(disposing);
		}
		
		
		protected override void UpdateTexture()
		{
			UpdateIfNeed(0);
		}
		
		void CheckTexture()
		{
			if (!IsImageAlive)
			{
				KillTexture();
				return;
			}
			
			
			Alt.Sketch.RenderImage renderImage = ImageSourceAsRenderImage;
			if (renderImage == null ||
			    ModificationTime == renderImage.ModificationTime)
			{
				return;
			}
			
			
			if (SrcWidth != renderImage.PixelWidth ||
			    SrcHeight != renderImage.PixelHeight ||
			    PixelFormat != renderImage.PixelFormat)
			{
				KillTexture();
			}
			
			
			if (SrcWidth < 1 ||
			    SrcHeight < 1 ||
			    PixelFormat == Alt.Sketch.PixelFormat.Unknown)
			{
				return;
			}
			
			
			//int bpp = renderImage.ByteDepth;
			if (!m_RenderManager.NonPowerOf2TexturesSupported)
			{
				TextureWidth = NextPowerOfTwo(SrcWidth);
				TextureHeight = NextPowerOfTwo(SrcHeight);
			}
			else
			{
				TextureWidth = SrcWidth;
				TextureHeight = SrcHeight;
			}
			
			
			if (m_Texture == null)
			{
				// Create a new texture
				m_Texture = new RenderTexture(TextureWidth, TextureHeight, 0, Unity_RenderManager.ToRenderTextureFormat(PixelFormat));
				
				if (m_Texture != null)
				{
					TextureWidth = m_Texture.width;
					TextureHeight = m_Texture.height;
					
					m_Texture.filterMode = FilterMode.Point;
				}
			}
		}
		
		
		protected override void UpdateIfNeed(Int64 currentTime)
		{
			if (!IsImageAlive)
			{
				KillTexture();
				return;
			}
			
			
			Alt.Sketch.RenderImage renderImage = ImageSourceAsRenderImage;
			if (renderImage == null)
			{
				return;
			}
			
			CheckTexture();
			
			
			if (ModificationTime != renderImage.ModificationTime)
			{
				//UpdateTexture();
				
				RenderTexture renderTexture = m_Texture as RenderTexture;
				if (renderTexture != null)
				{
					if (m_Renderer == null)
					{
						m_Renderer = new Unity_Renderer(m_RenderManager, renderTexture);
						m_Renderer.DefaultSmoothingMode = Alt.Sketch.SmoothingMode.None;
					}
					
					RenderTexture oldRenderTexture = RenderTexture.active;
					RenderTexture.active = renderTexture;
					try
					{
						m_Renderer.BeginRender(null, null, renderTexture.width, renderTexture.height);
						GL.Clear(true, true, new Color(0.5f, 0.5f, 0.5f, 0));//Color.clear);
						{
							renderImage.Render(Alt.Sketch.Graphics.FromRenderer(m_Renderer));
						}
						m_Renderer.EndRender();
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.ToString());
					}
					finally
					{
						RenderTexture.active = oldRenderTexture;
					}
				}
				
				ModificationTime = renderImage.ModificationTime;
			}
		}
	}
}
