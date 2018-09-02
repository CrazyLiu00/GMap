//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using Alt.FarseerPhysics.Collision.Shapes;
using Alt.FarseerPhysics.Common;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class ConvexHullTest : FarseerPhysicsTest
    {
		int _count = Alt.FarseerPhysics.Settings.MaxPolygonVertices;
		Vector2[] _points = new Vector2[Alt.FarseerPhysics.Settings.MaxPolygonVertices];
        bool _auto;

        
        ConvexHullTest()
        {
            Generate();
            _auto = false;
        }

        
        void Generate()
        {
            Vector2 lowerBound = new Vector2(-8.0f, -8.0f);
            Vector2 upperBound = new Vector2(8.0f, 8.0f);

            for (int i = 0; i < _count; ++i)
            {
                double x = 10.0f * Rand.RandomFloat();
                double y = 10.0f * Rand.RandomFloat();

                // Clamp onto a square to help create collinearities.
                // This will stress the convex hull algorithm.
                Vector2 v = new Vector2(x, y);
                v = MathUtils.Clamp(v, lowerBound, upperBound);
                _points[i] = v;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
                _auto = !_auto;

            if (e.KeyCode == Keys.G)
                Generate();


            base.OnKeyDown(e);
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            base.Update(settings);

            PolygonShape shape = new PolygonShape(new Vertices(_points), 0f);

            DrawString("Press g to generate a new random convex hull");

            
            DebugView.DrawPolygon(shape.Vertices.ToArray(), shape.Vertices.Count, new ColorR(0.9, 0.9, 0.9));

            for (int i = 0; i < _count; ++i)
            {
                DebugView.DrawPoint(_points[i], 0.1f, new ColorR(0.9, 0.5, 0.5));
                Vector2 position = ConvertWorldToScreen(_points[i]);
                DebugView.DrawString((int)position.X, (int)position.Y, i.ToString());
            }


            if (_auto)
            {
                Generate();
            }
        }

        public static FarseerPhysicsTest Create()
        {
            return new ConvexHullTest();
        }
    }
}