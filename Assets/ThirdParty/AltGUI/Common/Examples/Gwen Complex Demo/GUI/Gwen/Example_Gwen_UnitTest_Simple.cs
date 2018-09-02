//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
    class Example_Gwen_UnitTest_Simple : Example__Base
    {
        Color m_CenterBG = Color.Transparent;//FromArgb(50, Color.White);


        public Example_Gwen_UnitTest_Simple(Base parent)
            : base(parent)
        {
            Margin = new Margin(-1);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            Gwen.UnitTest.UnitTest ut = new Gwen.UnitTest.UnitTest(this, false);
            if (m_CenterBG.A != 0)
            {
                ut.m_Center.ClientBackColor = m_CenterBG;
            }
        }
    }
}
