//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.


namespace Alt.GUI.Box2D.Demo.Testbed.Framework
{
    public class Settings
    {
        public Settings()
        {
            hz = 60.0;
            velocityIterations = 8;// 10;
            positionIterations = 3;// 8;
            drawShapes = 1;
            drawJoints = 1;
            enableWarmStarting = 1;
            enableContinuous = 1;
        }

        public double hz;
        public int velocityIterations;
        public int positionIterations;
        public uint drawShapes;
        public uint drawJoints;
        public uint drawAABBs;
        public uint drawPairs;
        public uint drawContactPoints;
        public uint drawContactNormals;
        public uint drawContactForces;
        public uint drawFrictionForces;
        public uint drawCOMs;
        public uint drawStats;
        public uint enableWarmStarting;
        public uint enableContinuous;
        public uint pause;
        public uint singleStep;
    }
}
