//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.


using System;
using Alt.FarseerPhysics.Collision.Shapes;
using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class EdgeShapeBenchmarkTest : FarseerPhysicsTest
    {
        int _count;
        PolygonShape _polyShape;

        
        EdgeShapeBenchmarkTest()
        {
            // Ground body
            {
                Body ground = BodyFactory.CreateBody(World);

                double x1 = -20.0f;
                double y1 = 2.0f * (double)Math.Cos(x1 / 10.0f * (double)Math.PI);
                for (int i = 0; i < 80; ++i)
                {
                    double x2 = x1 + 0.5f;
                    double y2 = 2.0f * (double)Math.Cos(x2 / 10.0f * (double)Math.PI);

                    EdgeShape shape = new EdgeShape(new Vector2(x1, y1), new Vector2(x2, y2));
                    ground.CreateFixture(shape);

                    x1 = x2;
                    y1 = y2;
                }
            }

            const double w = 1.0f;
            const double t = 2.0f;
            double b = w / (2.0f + (double)Math.Sqrt(t));
            double s = (double)Math.Sqrt(t) * b;

            Vertices vertices = new Vertices(8);
            vertices.Add(new Vector2(0.5f * s, 0.0f));
            vertices.Add(new Vector2(0.5f * w, b));
            vertices.Add(new Vector2(0.5f * w, b + s));
            vertices.Add(new Vector2(0.5f * s, w));
            vertices.Add(new Vector2(-0.5f * s, w));
            vertices.Add(new Vector2(-0.5f * w, b + s));
            vertices.Add(new Vector2(-0.5f * w, b));
            vertices.Add(new Vector2(-0.5f * s, 0.0f));

            _polyShape = new PolygonShape(vertices,20);
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            _count++;

            if (_count < 50)
            {
                const double x = 0;
                const double y = 15;

                Body body = BodyFactory.CreateBody(World);

                body.Position = new Vector2(x, y);
                body.BodyType = BodyType.Dynamic;

                Fixture fixture = body.CreateFixture(_polyShape);
                fixture.Friction = 0.3f;
            }

            base.Update(settings);
        }

        internal static FarseerPhysicsTest Create()
        {
            return new EdgeShapeBenchmarkTest();
        }
    }
}