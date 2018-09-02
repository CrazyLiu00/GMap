//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using Alt.Sketch;
using Alt.Sketch.Rendering;


namespace Alt.Backend.Unity
{
    class Unity_ImageBrushMaterial : ImageBrushMaterial, IUnity_BrushMaterial
	{
		UnityEngine.Material Material_Solid
		{
			get
			{
				return (Renderer as Unity_Renderer).ImageBrushMaterial_Solid;
			}
		}
		
		UnityEngine.Material Material_AA
		{
			get
			{
				return (Renderer as Unity_Renderer).ImageBrushMaterial_AA;
			}
		}
		
		
		public UnityEngine.Material GetMaterial(RenderPrimitive renderPrimitive)
		{
			UnityEngine.Texture texture = (ImageTexture as Unity_Texture).Native;
			
			UnityEngine.Material material;
			if (renderPrimitive.IsAATextured)
			{
				material = Material_AA;

				material.mainTexture = texture;
			}
			else
			{
				material = Material_Solid;
				
				material.mainTexture = texture;
			}
			
			return material;
		}


        internal Unity_ImageBrushMaterial()
        {
            //  Here BrushMaterial object is not initialized
        }
    }
}
