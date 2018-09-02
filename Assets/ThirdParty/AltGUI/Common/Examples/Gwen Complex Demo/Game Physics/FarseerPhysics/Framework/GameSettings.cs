//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Framework
{
    public class FarseerPhysicsGameSettings
    {
        public double Hz;
        public bool Pause;
        public bool SingleStep;


        public FarseerPhysicsGameSettings()
        {
#if WINDOWS_PHONE
			Hz = 30.0f;
#else
            Hz = 60.0f;
#endif
        }
    }
}
