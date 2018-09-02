//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.


using System.Collections.Generic;
using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class CirclePenetrationTest : FarseerPhysicsTest
    {
        CirclePenetrationTest()
        {
            World.Gravity = Vector2.Zero;

            List<Vertices> borders = new List<Vertices>(4);

            const double borderWidth = 0.2f;
            const double width = 40f;
            const double height = 25f;

            //Bottom
            borders.Add(PolygonTools.CreateRectangle(width, borderWidth, new Vector2(0, height), 0));

            //Left
            borders.Add(PolygonTools.CreateRectangle(borderWidth, height, new Vector2(-width, 0), 0));

            //Top
            borders.Add(PolygonTools.CreateRectangle(width, borderWidth, new Vector2(0, -height), 0));

            //Right
            borders.Add(PolygonTools.CreateRectangle(borderWidth, height, new Vector2(width, 0), 0));

            Body body = BodyFactory.CreateCompoundPolygon(World, borders, 1, new Vector2(0, 20));

            foreach (Fixture fixture in body.FixtureList)
            {
                fixture.Restitution = 1f;
                fixture.Friction = 0;
            }

            Body circle = BodyFactory.CreateCircle(World, 0.32f, 1);
            circle.BodyType = BodyType.Dynamic;
            circle.Restitution = 1f;
            circle.Friction = 0;

            circle.ApplyLinearImpulse(new Vector2(200, 50));
        }

        internal static FarseerPhysicsTest Create()
        {
            return new CirclePenetrationTest();
        }
    }
}