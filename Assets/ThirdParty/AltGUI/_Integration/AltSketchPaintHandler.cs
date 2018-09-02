//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;

using Alt;
using Alt.Backend.Unity;
using Alt.Sketch;
using Alt.GUI;

using UnityEngine;
using UnityEngine.Serialization;


namespace UnityEngine
{
	abstract class AltSketchPaintHandler : IDisposable
	{
		internal const int DefaultMaximumFPS = 60;


		#if UNITY_EDITOR
		static Alt.Sketch.Font m_EditorTextFont;
		public static Alt.Sketch.Font EditorTextFont
		{
			get
			{
				if (m_EditorTextFont == null)
				{
					m_EditorTextFont = new Alt.Sketch.Font("Arial", 10);
				}
				
				return m_EditorTextFont;
			}
		}


		public static void DrawEditorText(Alt.GUI.PaintEventArgs e, string text, string gameObjectName, Alt.Sketch.Size clientSize, int invalidateFPS, Alt.Sketch.Rendering.RenderType renderType)
		{
			if (text.StartsWith("AltGUI "))
			{
				text = text.Substring(7);
			}

			//e.Graphics.TextRenderingHint = Alt.Sketch.TextRenderingHint.SingleBitPerPixelGridFit;

			double sizeK = 1.4;
			int hoffset = 4;
			int voffset = 6;
			e.Graphics.DrawString("AltGUI", EditorTextFont, Alt.Sketch.Brushes.LightBlue, new Alt.Sketch.Point(hoffset, 5));
			e.Graphics.DrawString(text,
			                      EditorTextFont, Alt.Sketch.Brushes.Yellow, new Alt.Sketch.Point(hoffset, voffset + EditorTextFont.Size * sizeK));
			voffset += 1;
			e.Graphics.DrawString("[" + gameObjectName + "]",
			                      EditorTextFont, Alt.Sketch.Brushes.Yellow, new Alt.Sketch.Point(hoffset, voffset + EditorTextFont.Size * sizeK * 2));
			voffset += 2;
			e.Graphics.DrawString("Max FPS: " + invalidateFPS.ToString(),
			                      EditorTextFont, Alt.Sketch.Brushes.WhiteSmoke, new Alt.Sketch.Point(hoffset, voffset + EditorTextFont.Size * sizeK * 3 + 1));
			e.Graphics.DrawString("Render Type: " + (renderType == Alt.Sketch.Rendering.RenderType.Software ? "Software" : "Hardware"),
			                      EditorTextFont, Alt.Sketch.Brushes.WhiteSmoke, new Alt.Sketch.Point(hoffset, voffset + EditorTextFont.Size * sizeK * 4 + 2));
			e.Graphics.DrawString("Size: " + clientSize.Width.ToString() + " x " + clientSize.Height.ToString(),
			                      EditorTextFont, Alt.Sketch.Brushes.WhiteSmoke, new Alt.Sketch.Point(hoffset, voffset + EditorTextFont.Size * sizeK * 5 + 3));
		}
		#endif


		protected abstract Alt.Sketch.Rendering.RenderType RenderType
		{
			get;
		}
		
		
		protected abstract Alt.Sketch.SizeI Size
		{
			get;
		}


		protected abstract void RefreshMaterial();

		protected abstract void RefreshVertices();


		protected abstract void DoPaint(Alt.GUI.PaintEventArgs e);


		Alt.TPSCounter m_FPSCounter = new Alt.TPSCounter();
		public double RealFPS
		{
			get
			{
				return m_FPSCounter.TPS;
			}
		}


		UnityEngine.Rect m_UVRect = new UnityEngine.Rect(0f, 0f, 1f, 1f);
		/// <summary>
		/// UV rectangle used by the texture.
		/// </summary>
		internal UnityEngine.Rect UVRect
		{
			get
			{
				return m_UVRect;
			}
			private set
			{
				if (m_UVRect == value)
				{
					return;
				}
				
				m_UVRect = value;

				RefreshVertices();
			}
		}


		bool m_fNeedsUpdate = true;
		/// <summary>
		/// Gets or sets a value indicating whether the RenderBox has to be updated.
		/// </summary>
		internal bool NeedsUpdate
		{
			get
			{
				if (m_fNeedsUpdate)
				{
					return true;
				}
				
				SizeI textureSize = Size;
				
				if (RenderType == Alt.Sketch.Rendering.RenderType.Software)
				{	
					if (m_Texture == null ||
					    m_Texture.width != textureSize.Width ||
					    m_Texture.height != textureSize.Height)
					{
						return true;
					}
				}
				else
				{
					if (m_RenderTexture == null ||
					    m_RenderTexture.width != textureSize.Width ||
					    m_RenderTexture.height != textureSize.Height)
					{
						return true;
					}
				}
				
				return false;
			}
			private set
			{
				m_fNeedsUpdate = value;
			}
		}

		internal void Invalidate()
		{
			NeedsUpdate = true;
		}
		

		
		Bitmap m_SoftwareBackBuffer;
		internal Bitmap SoftwareBackBuffer
		{
			get
			{
				SizeI size = Size;
				
				int w = size.Width;
				int h = size.Height;
				
				if (m_SoftwareBackBuffer == null ||
				    m_SoftwareBackBuffer.PixelWidth != w ||
				    m_SoftwareBackBuffer.PixelHeight != h)
				{
					if (m_SoftwareBackBuffer != null)
					{
						m_SoftwareBackBuffer.Dispose();
					}
					
					m_SoftwareBackBuffer = new Bitmap(w, h, PixelFormat.Format32bppArgb);
				}
				
				return m_SoftwareBackBuffer;
			}
		}


		Texture m_Texture;
		RenderTexture m_RenderTexture;
		Unity_Renderer m_Renderer;
				
		/// <summary>
		/// Returns the texture used to draw this Graphic.
		/// </summary>
		internal Texture mainTexture
		{
			get
			{
				//???	DoRenderIfNeed();
				
				return m_Texture;
			}
		}


		internal AltSketchPaintHandler()
		{
			AltGUIIntegration.Initialize();
		}


		public virtual void Dispose()
		{
			if (m_SoftwareBackBuffer != null)
			{
				m_SoftwareBackBuffer.Dispose();
				m_SoftwareBackBuffer = null;
			}

			if (m_Renderer != null)
			{
				m_Renderer.Dispose ();
				m_Renderer = null;
			}

			KillTextures();
		}

		
		
		void KillTextures()
		{
			m_Texture = null;
			
			if (m_RenderTexture == null)
			{
				return;
			}
			
			if (m_RenderTexture is RenderTexture)
			{
				(m_RenderTexture as RenderTexture).Release();
			}
			
			//m_RenderTexture.Destroy();
			
			m_RenderTexture = null;
		}


		internal long MSecSinceLastRender
		{
			get
			{
				return Alt.IntervalTimer.ConvertTicksToMSec(Alt.IntervalTimer.Ticks - m_LastRenderTime);
			}
		}
		

		void DoPaintInternal(Alt.GUI.PaintEventArgs e)
		{
			#if UNITY_EDITOR
			if (!Application.isPlaying)
			{
				e.Graphics.FillRectangle(Alt.Sketch.Color.SteelBlue, e.ClipRectangle);
				e.Graphics.DrawRectangle(Alt.Sketch.Color.DodgerBlue, e.ClipRectangle - Alt.Sketch.Size.One);
			}
			#endif

			DoPaint(e);
		}

		long m_LastRenderTime = 0;
		internal bool DoRenderIfNeed(Camera camera, Renderer renderer)
		{
			SizeI size;

			if (!NeedsUpdate ||
			    Unity_RenderManager.Instance == null ||
			    (size = Size).IsEmpty)
			{
				return false;
			}
			

			NeedsUpdate = false;

			m_LastRenderTime = Alt.IntervalTimer.Ticks;

			m_FPSCounter.Tick();


			if (RenderType == Alt.Sketch.Rendering.RenderType.Software)
			{	
				if (!(m_Texture is Texture2D))
				{
					KillTextures();
				}
				
				if (m_Texture != null &&
				    (m_Texture.width != size.Width ||
				 	m_Texture.height != size.Height))
				{
					m_Texture = null;
				}
				
				if (m_Texture == null)
				{
					m_Texture = new Texture2D(
						size.Width, size.Height,
						Unity_RenderManager.ToTextureFormat(PixelFormat.Format32bppArgb),
						false);
					m_Texture.filterMode = FilterMode.Point;

					RefreshMaterial();
				}
				
				Bitmap backBuffer = SoftwareBackBuffer;
				Alt.Sketch.Graphics graphics = Alt.Sketch.Graphics.FromImage(backBuffer);
				
				//graphics.Clear(Alt.Sketch.Color.FromArgb(0, 128, 128, 128));
				graphics.Clear(Alt.Sketch.Color.FromArgb(0, 0, 0, 0));

				DoPaintInternal(new Alt.GUI.PaintEventArgs(graphics, new Alt.Sketch.Rect(0, 0, size)));
				
				Unity_RenderManager.SetTextureData(m_Texture as Texture2D, backBuffer, backBuffer.PixelRectangle, true);
			}
			else
			{
				if (m_RenderTexture != null &&
				    (m_RenderTexture.width != size.Width ||
				 	m_RenderTexture.height != size.Height))
				{
					KillTextures();
				}
				
				if (m_RenderTexture == null)
				{
					m_RenderTexture = new RenderTexture(
						size.Width, size.Height,
						0,
						Unity_RenderManager.ToRenderTextureFormat(PixelFormat.Format32bppArgb));
					m_RenderTexture.filterMode = FilterMode.Point;
					
					m_Texture = m_RenderTexture;

					RefreshMaterial();
				}
				
				if (m_RenderTexture != null)
				{
					RenderTexture oldRenderTexture = RenderTexture.active;
					RenderTexture.active = m_RenderTexture;
					try
					{
						GL.Clear(true, true, Unity_RenderManager.ToColor(Alt.Sketch.Color.Empty));
						
						if (m_Renderer == null)
						{
							m_Renderer = new Unity_Renderer(Unity_RenderManager.Instance);
							m_Renderer.DefaultSmoothingMode = Alt.Sketch.SmoothingMode.None;
						}
						
						m_Renderer.BeginRender(camera, renderer, size.Width, size.Height, true);
						{
							Alt.Sketch.Graphics graphics = Alt.Sketch.Graphics.FromRenderer(m_Renderer);
							if (graphics != null)
							{
								DoPaintInternal(new Alt.GUI.PaintEventArgs(graphics, new Alt.Sketch.Rect(0, 0, size)));
							}
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
			}

			return true;
		}
	}
}
