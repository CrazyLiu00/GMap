//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using Alt.Sketch;
using Alt.Sketch.Rendering;


namespace Alt.Backend.Unity
{
    class Unity_SolidColorBrushMaterial : SolidColorBrushMaterial, IUnity_BrushMaterial
	{
		UnityEngine.Material Material_Solid
		{
			get
			{
				return (Renderer as Unity_Renderer).SolidColorBrushMaterial_Solid;
			}
		}
		
		UnityEngine.Material Material_AA
		{
			get
			{
				return (Renderer as Unity_Renderer).SolidColorBrushMaterial_AA;
			}
		}
		
		
		public UnityEngine.Material GetMaterial(RenderPrimitive renderPrimitive)
		{
			if (renderPrimitive.IsAATextured)
			{
				return Material_AA;
			}

			return Material_Solid;
		}


        internal Unity_SolidColorBrushMaterial()
        {
            //  Here BrushMaterial object is not initialized
        }
    }
}
