//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Alt.Sketch;
using Alt.Sketch.Rendering;

using UnityEngine;


namespace Alt.Backend.Unity
{
    class Unity_RenderPrimitive : RenderPrimitive
    {
		RenderPrimitiveType m_ModifiedRenderPrimitiveType;
		UInt32[] m_Indices;
		UnityEngine.Color[] m_Colors;


        public Unity_RenderPrimitive()
        {
        }


        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    //KillRenderOperation();
                }

                // There are no unmanaged resources to release, but
                // if we add them, they need to be released here.
                Free();
            }

            base.Dispose(disposing);
        }


        void Free()
        {
            PrepareRender_LastResult = false;
        }


        internal void PostRender()
        {
            if (IsTemporary)
            {
                Free();
            }
        }


        bool PrepareRender_LastResult = false;
        internal bool PreRender()
        {
            if (PrepareRender_LastResult)
            {
                return true;
            }


			m_ModifiedRenderPrimitiveType = RenderPrimitiveType;
			switch (m_ModifiedRenderPrimitiveType)
			{
				case RenderPrimitiveType.PointList:
				{
					return (PrepareRender_LastResult = false);
				}
				case RenderPrimitiveType.LineStrip:
				{
					m_ModifiedRenderPrimitiveType = RenderPrimitiveType.LineList;
					m_Indices = LineListIndices;
					break;
				}
				case RenderPrimitiveType.TriangleFan:
				{
					m_ModifiedRenderPrimitiveType = RenderPrimitiveType.TriangleList;
					m_Indices = TriangleListIndices;
					break;
				}
				default:
				{
					m_Indices = Indices;
					break;
				}
			}

			if (m_Indices == null)
			{
				return (PrepareRender_LastResult = false);
			}


			int[] colors = Colors;
			if (colors == null)
			{
				return (PrepareRender_LastResult = false);
			}


			int colorsCount = colors.Length;
			m_Colors = new UnityEngine.Color[colorsCount];

			for (int i = 0; i < colorsCount; i++)
			{
				int color = colors[i];
				
				m_Colors[i] = new UnityEngine.Color(
					(float)((color >> 16) & 0xff) / 255.0f,
					(float)((color >> 8) & 0xff) / 255.0f,
					(float)((color >> 0) & 0xff) / 255.0f,
					(float)((color >> 24) & 0xff) / 255.0f);
			}

						
			return (PrepareRender_LastResult = true);
		}


		public void Render(Material material)
		{
			if (!PrepareRender_LastResult ||
			    //	Not supported
			    m_ModifiedRenderPrimitiveType == RenderPrimitiveType.PointList)
			{
				return;
			}


			Alt.Sketch.Vector3f[] positions = Positions;
			int primitiveType = PrimitiveType;

            if (positions == null ||
                positions.Length < 1 ||
			    m_Indices == null ||
			    m_Colors == null ||
			    primitiveType == -1)
            {
                //return
				PrepareRender_LastResult = false;
				return;
            }


            //  TexCoords
            Alt.Sketch.Vector2f[] texCoords0 = AATexCoords;
            Alt.Sketch.Vector2f[] texCoords1 = null;
            Alt.Sketch.Vector2f[] texCoords2 = null;
            if (texCoords0 == null)
            {
                texCoords0 = ImageTexCoords;
                if (texCoords0 == null)
                {
                    texCoords0 = AMaskTexCoords;
                }
                else
                {
                    texCoords1 = AMaskTexCoords;
                }
            }
            else
            {
                texCoords1 = ImageTexCoords;
                if (texCoords1 == null)
                {
                    texCoords1 = AMaskTexCoords;
                }
                else
                {
                    texCoords2 = AMaskTexCoords;
                }
            }

            int nNumTexCoords =
                (texCoords0 != null ? 1 : 0) +
                (texCoords1 != null ? 1 : 0) +
                (texCoords2 != null ? 1 : 0);


            int indicesCount = m_Indices.Length;


			int materialpassCount = 1;//once for already setted material
			if (material != null)
			{
				materialpassCount = material.passCount;
			}

			for (int passIndex = 0; passIndex < materialpassCount; passIndex++)
			{
				if (material != null)
				{
					material.SetPass(passIndex);
				}

				switch (nNumTexCoords)
	            {
	                case 0:
	                    {
							GL.Begin(primitiveType);
							{
				                for (int i = 0; i < indicesCount; i++)
				                {
									UInt32 index = m_Indices[i];

									//NoNeed	GL.TexCoord2(0, 0);

									GL.Color(m_Colors[index]);

									Vector3f position = positions[index];
									GL.Vertex3(position.X, position.Y, position.Z);
								}
							}
							GL.End();

	                        break;
	                    }
	                case 1:
	                    {
							GL.Begin(primitiveType);
							{
				                for (int i = 0; i < indicesCount; i++)
				                {
									UInt32 index = m_Indices[i];

									Vector2f texCoords = texCoords0[index];
									GL.TexCoord2(texCoords.X, texCoords.Y);

									GL.Color(m_Colors[index]);

									Vector3f position = positions[index];
									GL.Vertex3(position.X, position.Y, position.Z);
								}
							}
							GL.End();

	                        break;
	                    }
	                case 2:
	                    {
	                        texCoords0 = ImageTexCoords;
	                        if (texCoords0 == null)
	                        {
	                            texCoords0 = AMaskTexCoords;
	                            if (texCoords0 == null)
	                            {
	                                texCoords0 = AATexCoords;
	                            }
	                        }

							GL.Begin(primitiveType);
							{
				                for (int i = 0; i < indicesCount; i++)
				                {
									UInt32 index = m_Indices[i];

									Vector2f texCoords = texCoords0[index];
									GL.TexCoord2(texCoords.X, texCoords.Y);

									GL.Color(m_Colors[index]);

									Vector3f position = positions[index];
									GL.Vertex3(position.X, position.Y, position.Z);
								}
							}
							GL.End();

	                        break;
	                    }
	                case 3:
	                    {
	                        texCoords0 = ImageTexCoords;
	                        if (texCoords0 == null)
	                        {
	                            texCoords0 = AMaskTexCoords;
	                            if (texCoords0 == null)
	                            {
	                                texCoords0 = AATexCoords;
	                            }
	                        }

							GL.Begin(primitiveType);
							{
				                for (int i = 0; i < indicesCount; i++)
				                {
									UInt32 index = m_Indices[i];

									Vector2f texCoords = texCoords0[index];
									GL.TexCoord2(texCoords.X, texCoords.Y);

									GL.Color(m_Colors[index]);

									Vector3f position = positions[index];
									GL.Vertex3(position.X, position.Y, position.Z);
								}
							}
							GL.End();

	                        break;
	                    }
	            }
			}
		}


		int PrimitiveType
		{
			get
			{
				switch (m_ModifiedRenderPrimitiveType)
				{
					case RenderPrimitiveType.LineList:
						{
							return GL.LINES;
						}
					case RenderPrimitiveType.TriangleList:
						{
							return GL.TRIANGLES;
						}
					case RenderPrimitiveType.TriangleStrip:
						{
							return GL.TRIANGLE_STRIP;
						}
				}

				return -1;
			}
		}
    }
}
