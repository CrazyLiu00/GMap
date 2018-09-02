//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo
{
    class Example_Gwen_UnitTest_Skin : Example__Base
    {
        Alt.GUI.Temporary.Gwen.Skin.Base m_Skin;
        Alt.GUI.Temporary.Gwen.Control.Canvas m_Canvas;


        public Example_Gwen_UnitTest_Skin(Base parent)
            : base(parent)
        {
            Margin = new Margin(-1);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            m_Skin = new Alt.GUI.Temporary.Gwen.Skin.TexturedBase(GetCanvas().Skin.Renderer, "AltData/Gwen/DefaultSkin.png");
            //m_Renderer.ShouldCacheToTexture = m_Skin is Alt.GUI.Temporary.Gwen.Skin.TexturedBase;
            m_Canvas = new Canvas(m_Skin);
            m_Canvas.SetSize(Width, Height);

            new Gwen.UnitTest.UnitTest(m_Canvas, false);

            m_Canvas.Parent = this;
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (m_Canvas != null)
            {
                m_Canvas.SetSize(Width, Height);
            }
        }
    }
}
