//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.


using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Common.ConvexHull;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class ConvexHullTest2 : FarseerPhysicsTest
    {
        const int PointCount = 32;

        Vertices _chainHull;
        Vertices _giftWrap;
        Vertices _melkman;
        Vertices _pointCloud1;
        Vertices _pointCloud2;
        Vertices _pointCloud3;

        
        ConvexHullTest2()
        {
            _pointCloud1 = new Vertices(PointCount);

            for (int i = 0; i < PointCount; i++)
            {
                double x = Rand.RandomFloat(-10, 10);
                double y = Rand.RandomFloat(-10, 10);

                _pointCloud1.Add(new Vector2(x, y));
            }

            _pointCloud2 = new Vertices(_pointCloud1);
            _pointCloud3 = new Vertices(_pointCloud1);

            //Melkman DOES NOT work on point clouds. It only works on simple polygons.
            _pointCloud1.Translate(new Vector2(-20, 30));
            _melkman = Melkman.GetConvexHull(_pointCloud1);

            //Giftwrap works on point clouds
            _pointCloud2.Translate(new Vector2(20, 30));
            _giftWrap = GiftWrap.GetConvexHull(_pointCloud2);

            //Chain hull also works on point clouds
            _pointCloud3.Translate(new Vector2(20, 10));
            _chainHull = ChainHull.GetConvexHull(_pointCloud3);
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            DrawString("Melkman: Red");
            DrawString("Giftwrap: Green");
            DrawString("ChainHull: Blue");
            
            
            for (int i = 0; i < PointCount; i++)
            {
                DebugView.DrawPoint(_pointCloud1[i], 0.1f, Color.Yellow);
                DebugView.DrawPoint(_pointCloud2[i], 0.1f, Color.Yellow);
                DebugView.DrawPoint(_pointCloud3[i], 0.1f, Color.Yellow);
            }

            DebugView.DrawPolygon(_melkman.ToArray(), _melkman.Count, Color.Red);
            DebugView.DrawPolygon(_giftWrap.ToArray(), _giftWrap.Count, Color.Green);
            DebugView.DrawPolygon(_chainHull.ToArray(), _chainHull.Count, Color.Blue);
            

            base.Update(settings);
        }

        internal static FarseerPhysicsTest Create()
        {
            return new ConvexHullTest2();
        }
    }
}