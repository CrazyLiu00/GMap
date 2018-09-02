//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;


namespace Alt.GUI.Box2D.Demo.Testbed.Framework
{
    public struct TestEntry
    {
        public string name;
        public Func<Test> createFcn;
        public bool solidDraw;
    }
}
