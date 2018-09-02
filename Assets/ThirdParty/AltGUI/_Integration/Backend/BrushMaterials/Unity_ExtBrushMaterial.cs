//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;

using Alt.Sketch;
using Alt.Sketch.Rendering;


namespace Alt.Backend.Unity
{
    class Unity_ExtBrushMaterial : ExtBrushMaterial, IUnity_BrushMaterial
    {
		UnityEngine.Texture Texture
        {
            get
            {
				return GetExtParameter("Texture") as UnityEngine.Texture;
            }
        }

        ImageSource Image
        {
            get
            {
                return GetExtParameter("Image") as ImageSource;
            }
        }
		
		UnityEngine.Texture ImageTexture
		{
			get
			{
				Unity_Texture t = Renderer.GetTexture(Image) as Unity_Texture;
				if (t != null)
				{
					return t.Native;
				}
				
				return null;
			}
		}


        //  TEMP: just for demos unification
        static int GetMaterialIDByAlias(string alias)
        {
            if (alias == "Material1")
            {
                return 1;
            }
            else if (alias == "Material2")
            {
                return 2;
            }
            else if (alias == "Material3")
            {
                return 3;
            }
            else if (alias == "Material4")
            {
                return 4;
            }

            return 0;
        }


		public UnityEngine.Texture GetTexture(RenderPrimitive renderPrimitive)
        {
            int mID = GetMaterialIDByAlias(MaterialName);
            switch (mID)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    {
                        return (Renderer.GetTexture(Image) as Unity_Texture).Native;
                    }
            }


            return Texture;
        }


		
		UnityEngine.Material m_Material_Solid;
		UnityEngine.Material Material_Solid
		{
			get
			{
				if (m_Material_Solid == null)
				{
					switch (GetMaterialIDByAlias(MaterialName))
					{
						case 1:
						{
							m_Material_Solid = new UnityEngine.Material(UnityEngine.Shader.Find ("Hidden/Internal-Flare"));

							break;
						}
						case 2:
						{
							m_Material_Solid = new UnityEngine.Material(UnityEngine.Shader.Find ("Particles/~Additive-Multiply"));

							break;
						}
						case 3:
						{
							m_Material_Solid = new UnityEngine.Material(UnityEngine.Shader.Find ("Transparent/Cutout/Specular"));

							break;
						}
						case 4:
						{
							m_Material_Solid = new UnityEngine.Material(UnityEngine.Shader.Find ("Hidden/Internal-Halo"));
							
							break;
						}
						default:
						{
							if (!string.IsNullOrEmpty(MaterialName))
						    {
								m_Material_Solid = new UnityEngine.Material(UnityEngine.Shader.Find (MaterialName));
							}

							break;
						}
					}
					
					if (m_Material_Solid == null)
					{
						m_Material_Solid = (Renderer as Unity_Renderer).ImageBrushMaterial_Solid;
					}
				}



				return m_Material_Solid;
			}
		}
		
		UnityEngine.Material Material_AA
		{
			get
			{
				//  Material AA for ExtBrush is not yet supported - need more research
				return null;
			}
		}
		
		
		public UnityEngine.Material GetMaterial(RenderPrimitive renderPrimitive)
		{
			//  Universal method
			if (renderPrimitive.IsAATextured)
			{
				return Material_AA;
			}
			else
			{
				UnityEngine.Material material = Material_Solid;
				if (material != null)
				{
					UnityEngine.Texture texture = Texture;
					if (texture == null)
					{
						texture = ImageTexture;
						
						if (texture == null)
						{
							return null;
						}
					}
					
					//  Set texture
					material.mainTexture = texture;
				}
				
				return material;
			}
		}


        internal Unity_ExtBrushMaterial()
        {
            //  Here BrushMaterial object is not initialized
        }


        protected override void BeginRender(RenderContext renderContext)
        {
            base.BeginRender(renderContext);


            int mID = GetMaterialIDByAlias(MaterialName);

            switch (mID)
            {
                case 1:
                    {
                        goto case 4; 
                    }
                case 2:
                    {
                        goto case 4;
                    }
                case 3:
                    {
                        goto case 4;
                    }
                case 4:
                    {
                        break;
                    }
            }
        }


        protected override void EndRender()
        {
            int mID = GetMaterialIDByAlias(MaterialName);
            switch (mID)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    {
                        break;
                    }
            }

            
            base.EndRender();
        }
    }
}
