//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Framework
{
    public struct FarseerPhysicsTestEntry
    {
        public Func<FarseerPhysicsTest> CreateTest;
        public string Name;
        public bool solidDraw;
    }
}
