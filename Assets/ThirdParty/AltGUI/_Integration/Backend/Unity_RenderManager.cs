//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

using Alt.Sketch.Rendering;
using Alt.Sketch;

using UnityEngine;


namespace Alt.Backend.Unity
{
	public class Unity_RenderManager : Alt.Sketch.Rendering.RenderManager
	{
		public SizeI MaxTextureSize;
		public bool RenderLinesByPolygons;
		public int MaxTextureCountPerBrushMaterial;
		public int MaxTextureCoordsSetCountPerBrushMaterial;
		public bool RenderToTextureSupported;
		public bool TextureMatrixSupported;
		public bool ClippingSupported;
		
		
		static Unity_RenderManager m_Instance;
		public static Unity_RenderManager Instance
		{
			get
			{
				//TEST
				return m_Instance ?? new Unity_RenderManager();
			}
		}
		
		
		Unity_RenderManager()
		{
			m_Instance = this;
			

			int maxTextureSize = SystemInfo.maxTextureSize;
			MaxTextureSize = new SizeI(maxTextureSize, maxTextureSize);//new SizeI(2048, 2048);

			//	TEMP (need process NPOTSupport.Restricted mode - Limited NPOT support: no mip-maps and clamp wrap mode will be forced)
			NonPowerOf2TexturesSupported = SystemInfo.npotSupport != NPOTSupport.None;
			
			RenderLinesByPolygons = false;
			MaxTextureCountPerBrushMaterial = 1;
			MaxTextureCoordsSetCountPerBrushMaterial = MaxTextureCountPerBrushMaterial;
			
			//	test RenderTexture (not available if not Unity Pro)
			/*
			RenderTexture renderTexture = null;
			try
			{
				renderTexture = new RenderTexture(2, 2, 0, RenderTextureFormat.ARGB32);
				renderTexture.Create();
				RenderToTextureSupported = renderTexture.IsCreated();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				
				RenderToTextureSupported = false;
			}
			finally
			{
				if (renderTexture != null)
				{
					renderTexture.Release();
					//renderTexture.Destroy();
					renderTexture = null;
				}
			}*/
			RenderToTextureSupported = SystemInfo.supportsRenderTextures; 
			
			TextureMatrixSupported = true;
			ClippingSupported = true;
			
			
			//	We prefer to use Software Render BackBuffers, because it's faster in most cases and
			//	hasn't proplems with premultiplied alpha
			RenderToTextureSupported = false;
		}
		
		
		protected override void Dispose(bool disposing)
		{
			m_Instance = null;
			
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
		
		
		void Unity_RenderSystemEvent()//Engine.Renderer.RenderSystemEvents name)
		{
			//if (name == Engine.Renderer.RenderSystemEvents.DeviceRestored)
			{
				OnDeviceReset();
			}
		}
		
		
		protected override Alt.Sketch.Rendering.Texture CreateTexture2D(int width, int height, Alt.Sketch.PixelFormat format)
		{
			return new Unity_Texture(this, width, height, format);
		}
		
		protected override Alt.Sketch.Rendering.Texture CreateRenderTexture(int width, int height, Alt.Sketch.PixelFormat format)
		{
			return new Unity_RenderTexture(this, width, height, format);
		}
		
		
		//static byte[] tempArray = new byte[1000000];
		public static void SetTextureData(UnityEngine.Texture2D texture, Alt.Sketch.Bitmap src, Alt.Sketch.RectI srcRect)
		{
			SetTextureData(texture, src, srcRect, false);
		}
		public static void SetTextureData(UnityEngine.Texture2D texture, Alt.Sketch.Bitmap src, Alt.Sketch.RectI srcRect, bool flip)
		{
			if (texture == null ||
			    src == null ||
			    !src.IsValid)
			{
				return;
			}
			
			
			Alt.Sketch.BitmapData srcData = src.LockBits(Sketch.ImageLockMode.ReadOnly);
			if (srcData == null)
			{
				return;
			}
			
			
			int src_x = srcRect.X;
			int src_y = srcRect.Y;
			//int src_w, src_h;
			int src_width = //src_w =
				srcRect.Width;
			int src_height = //src_h =
				srcRect.Height;
			
			int dest_x = 0;
			int dest_y = 0;
			
			int dest_w = texture.width;
			int dest_h = texture.height;
			
			if (dest_x > (dest_w - 1) ||
			    dest_y > (dest_h - 1))
			{
				src.UnlockBits(srcData);
				
				return;
			}
			
			if ((dest_x + src_width) > dest_w)
			{
				src_width = dest_w - dest_x;
			}
			
			if ((dest_y + src_height) > dest_h)
			{
				src_height = dest_h - dest_y;
			}
			
			
			byte[] src_Buffer = srcData.Scan0;
			int src_stride = srcData.Stride;
			int src_ByteDepth = srcData.ByteDepth;
			
			
			try
			{
				bool fProcessed = false;
				
				switch (srcData.PixelFormat)
				{
				case Sketch.PixelFormat.Format24bppRgb:
				{
					switch (texture.format)
					{
					case TextureFormat.RGB24:
					{
						if (src_width == dest_w &&
						    src_height == dest_h)
						{
							//texture.LoadRawTextureData(src_Buffer);
							
							byte[] colors = src.TextureTempBuffer;
							if (colors == null)
							{
								colors = new byte[src_height * src_width * 3];
							}
							int colors_index = 0;
							
							if (flip)
							{
								for (int y = src_height - 1; y >= 0; y--)
								{
									int src_Index = src_x * src_ByteDepth + (src_y + y) * src_stride;
									
									for (int x = 0; x < src_width; x++)
									{
										colors[colors_index++] = src_Buffer[src_Index + 2];
										colors[colors_index++] = src_Buffer[src_Index + 1];
										colors[colors_index++] = src_Buffer[src_Index];
										src_Index += 3;
									}
								}
							}
							else
							{
								for (int y = 0; y < src_height; y++)
								{
									int src_Index = src_x * src_ByteDepth + (src_y + y) * src_stride;
									
									for (int x = 0; x < src_width; x++)
									{
										colors[colors_index++] = src_Buffer[src_Index + 2];
										colors[colors_index++] = src_Buffer[src_Index + 1];
										colors[colors_index++] = src_Buffer[src_Index];
										src_Index += 3;
									}
								}
							}
							
							texture.LoadRawTextureData(colors);
						}
						else
						{
							UnityEngine.Color[] colors = new UnityEngine.Color[src_height * src_width];
							int colors_index = 0;
							
							if (flip)
							{
								for (int y = src_height - 1; y >= 0; y--)
								{
									int src_Index = src_x * src_ByteDepth + (src_y + y) * src_stride;
									
									for (int x = 0; x < src_width; x++)
									{
										colors[colors_index++] = new Color32(
											src_Buffer[src_Index + 2],
											src_Buffer[src_Index + 1],
											src_Buffer[src_Index],
											255);
										src_Index += 3;
									}
								}
							}
							else
							{
								for (int y = 0; y < src_height; y++)
								{
									int src_Index = src_x * src_ByteDepth + (src_y + y) * src_stride;
									
									for (int x = 0; x < src_width; x++)
									{
										colors[colors_index++] = new Color32(
											src_Buffer[src_Index + 2],
											src_Buffer[src_Index + 1],
											src_Buffer[src_Index],
											255);
										src_Index += 3;
									}
								}
							}
							
							texture.SetPixels(src_x, src_y, src_width, src_height, colors);
						}
						
						fProcessed = true;
						
						break;
					}
					case TextureFormat.RGBA32:
					{
						if (src_width == dest_w &&
						    src_height == dest_h)
						{
							byte[] colors = new byte[src_height * src_width * 4];
							int colors_index = 0;
							
							if (flip)
							{
								for (int y = src_height - 1; y >= 0; y--)
								{
									int src_Index = src_x * src_ByteDepth + (src_y + y) * src_stride;
									
									for (int x = 0; x < src_width; x++)
									{
										colors[colors_index++] = src_Buffer[src_Index + 2];
										colors[colors_index++] = src_Buffer[src_Index + 1];
										colors[colors_index++] = src_Buffer[src_Index];
										colors[colors_index++] = 255;
										src_Index += 3;
									}
								}
							}
							else
							{
								for (int y = 0; y < src_height; y++)
								{
									int src_Index = src_x * src_ByteDepth + (src_y + y) * src_stride;
									
									for (int x = 0; x < src_width; x++)
									{
										colors[colors_index++] = src_Buffer[src_Index + 2];
										colors[colors_index++] = src_Buffer[src_Index + 1];
										colors[colors_index++] = src_Buffer[src_Index];
										colors[colors_index++] = 255;
										src_Index += 3;
									}
								}
							}
							
							texture.LoadRawTextureData(colors);
						}
						else
						{
							UnityEngine.Color[] colors = new UnityEngine.Color[src_height * src_width];
							int colors_index = 0;
							
							if (flip)
							{
								for (int y = src_height - 1; y >= 0; y--)
								{
									int src_Index = src_x * src_ByteDepth + (src_y + y) * src_stride;
									
									for (int x = 0; x < src_width; x++)
									{
										colors[colors_index++] = new Color32(
											src_Buffer[src_Index + 2],
											src_Buffer[src_Index + 1],
											src_Buffer[src_Index],
											255);
										src_Index += 3;
									}
								}
							}
							else
							{
								for (int y = 0; y < src_height; y++)
								{
									int src_Index = src_x * src_ByteDepth + (src_y + y) * src_stride;
									
									for (int x = 0; x < src_width; x++)
									{
										colors[colors_index++] = new Color32(
											src_Buffer[src_Index + 2],
											src_Buffer[src_Index + 1],
											src_Buffer[src_Index],
											255);
										src_Index += 3;
									}
								}
							}
							
							texture.SetPixels(src_x, src_y, src_width, src_height, colors);
						}
						
						fProcessed = true;
						
						break;
					}
					}
					
					break;
				}
					
				case Sketch.PixelFormat.Format32bppArgb:
				{
					switch (texture.format)
					{
					case TextureFormat.RGB24:
					{
						//  We can't be here
						
						//int dest_ByteDepth = 3;
						
						break;
					}
					case TextureFormat.RGBA32:
					{
						if (src_width == dest_w &&
						    src_height == dest_h)
						{
							//texture.LoadRawTextureData(src_Buffer);
							
							byte[] colors = src.TextureTempBuffer;
							if (colors == null)
							{
								colors = new byte[src_height * src_width * 4];
							}
							int colors_index = 0;
							
							if (flip)
							{
								for (int y = src_height - 1; y >= 0; y--)
								{
									int src_Index = src_x * src_ByteDepth + (src_y + y) * src_stride;
									
									for (int x = 0; x < src_width; x++)
									{
										colors[colors_index++] = src_Buffer[src_Index + 2];
										colors[colors_index++] = src_Buffer[src_Index + 1];
										colors[colors_index++] = src_Buffer[src_Index];
										colors[colors_index++] = src_Buffer[src_Index + 3];
										src_Index += 4;
									}
								}
							}
							else
							{
								for (int y = 0; y < src_height; y++)
								{
									int src_Index = src_x * src_ByteDepth + (src_y + y) * src_stride;
									
									for (int x = 0; x < src_width; x++)
									{
										colors[colors_index++] = src_Buffer[src_Index + 2];
										colors[colors_index++] = src_Buffer[src_Index + 1];
										colors[colors_index++] = src_Buffer[src_Index];
										colors[colors_index++] = src_Buffer[src_Index + 3];
										src_Index += 4;
									}
								}
							}
							
							texture.LoadRawTextureData(colors);
						}
						else
						{
							UnityEngine.Color[] colors = new UnityEngine.Color[src_height * src_width];
							int colors_index = 0;
							
							if (flip)
							{
								for (int y = src_height - 1; y >= 0; y--)
								{
									int src_Index = src_x * src_ByteDepth + (src_y + y) * src_stride;
									
									for (int x = 0; x < src_width; x++)
									{
										colors[colors_index++] = new Color32(
											src_Buffer[src_Index + 2],
											src_Buffer[src_Index + 1],
											src_Buffer[src_Index],
											src_Buffer[src_Index + 3]);
										src_Index += 4;
									}
								}
							}
							else
							{
								for (int y = 0; y < src_height; y++)
								{
									int src_Index = src_x * src_ByteDepth + (src_y + y) * src_stride;
									
									for (int x = 0; x < src_width; x++)
									{
										colors[colors_index++] = new Color32(
											src_Buffer[src_Index + 2],
											src_Buffer[src_Index + 1],
											src_Buffer[src_Index],
											src_Buffer[src_Index + 3]);
										src_Index += 4;
									}
								}
							}
							
							texture.SetPixels(src_x, src_y, src_width, src_height, colors);
						}
						
						fProcessed = true;
						
						break;
					}
					}
					
					break;
				}
				case Sketch.PixelFormat.Format8bppAlpha:
				{
					//  not used yet
					
					break;
				}
				}
				
				
				//  universal method
				if (!fProcessed)
				{
					UnityEngine.Color[] colors = new UnityEngine.Color[src_height * src_width];
					int colors_index = 0;
					
					for (int y = 0; y < src_height; y++)
					{
						for (int x = 0; x < src_width; x++)
						{
							Alt.Sketch.Color color = src.GetPixel(x, y);
							
							colors[colors_index++] = new Color32(
								color.R,
								color.G,
								color.B,
								color.A);
						}
					}
					
					texture.SetPixels(src_x, src_y, src_width, src_height, colors);
					
					fProcessed = true;
				}
				
				if (fProcessed)
				{
					texture.Apply(false);
				}
			}
			catch //(System.Exception ex)
			{
				//Console.WriteLine(ex.ToString());
			}
			finally
			{
				src.UnlockBits(srcData);
			}
		}
		
		
		public static TextureFormat ToTextureFormat(Alt.Sketch.PixelFormat format)
		{
			switch (format)
			{
			case Alt.Sketch.PixelFormat.Format8bppAlpha:
				return TextureFormat.Alpha8;
			case Alt.Sketch.PixelFormat.Format24bppRgb:
				return TextureFormat.RGB24;
			case Alt.Sketch.PixelFormat.Format32bppArgb:
				return TextureFormat.RGBA32;
			}
			
			return TextureFormat.RGBA32;
		}
		
		
		public static RenderTextureFormat ToRenderTextureFormat(Alt.Sketch.PixelFormat format)
		{
			switch (format)
			{
			case Alt.Sketch.PixelFormat.Format8bppAlpha:
				return RenderTextureFormat.Default;
			case Alt.Sketch.PixelFormat.Format24bppRgb:
				return RenderTextureFormat.Default;
			case Alt.Sketch.PixelFormat.Format32bppArgb:
				return RenderTextureFormat.ARGB32;
			}
			
			return RenderTextureFormat.Default;
		}
		
		
		public static Matrix4x4 ToMatrix(Matrix4 matrix)
		{
			return new Matrix4x4 ()
			{
				m00 = (float)matrix.m00,
				m01 = (float)matrix.m01,
				m02 = (float)matrix.m02,
				m03 = (float)matrix.m03,
				
				m10 = (float)matrix.m10,
				m11 = (float)matrix.m11,
				m12 = (float)matrix.m12,
				m13 = (float)matrix.m13,
				
				m20 = (float)matrix.m20,
				m21 = (float)matrix.m21,
				m22 = (float)matrix.m22,
				m23 = (float)matrix.m23,
				
				m30 = (float)matrix.m30,
				m31 = (float)matrix.m31,
				m32 = (float)matrix.m32,
				m33 = (float)matrix.m33
			};
		}
		
		
		public static UnityEngine.Color ToColor(ColorR color)
		{
			return new UnityEngine.Color((float)color.R, (float)color.G, (float)color.B, (float)color.A);
		}
	}
}
