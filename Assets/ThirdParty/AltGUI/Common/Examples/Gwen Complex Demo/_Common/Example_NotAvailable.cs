//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo
{
    abstract class Example_NotAvailable : Example__Base
    {
        public Example_NotAvailable(Base parent)
            : base(parent)
        {
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }
    }
}
