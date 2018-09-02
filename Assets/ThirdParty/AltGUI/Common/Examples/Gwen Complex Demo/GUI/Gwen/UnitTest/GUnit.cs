//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
	public class GUnit : Alt.GUI.Temporary.Gwen.Control.Base
    {
        public UnitTest UnitTest;

        
        public GUnit(Base parent) : base(parent)
        {
        }


        public void UnitPrint(String str)
        {
            if (UnitTest != null)
            {
                UnitTest.PrintText(str);
            }
        }
    }
}
