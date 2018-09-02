/*
* Farseer Physics Engine:
* Copyright (c) 2012 Ian Qvist
* 
* Original source Box2D:
* Copyright (c) 2006-2011 Erin Catto http://www.box2d.org 
* 
* This software is provided 'as-is', without any express or implied 
* warranty.  In no event will the authors be held liable for any damages 
* arising from the use of this software. 
* Permission is granted to anyone to use this software for any purpose, 
* including commercial applications, and to alter it and redistribute it 
* freely, subject to the following restrictions: 
* 1. The origin of this software must not be misrepresented; you must not 
* claim that you wrote the original software. If you use this software 
* in a product, an acknowledgment in the product documentation would be 
* appreciated but is not required. 
* 2. Altered source versions must be plainly marked as such, and must not be 
* misrepresented as being the original software. 
* 3. This notice may not be removed or altered from any source distribution. 
*/

//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.


using Alt.FarseerPhysics.Collision.Shapes;
using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class CompoundShapesTest : FarseerPhysicsTest
    {
        CompoundShapesTest()
        {
            BodyFactory.CreateEdge(World, new Vector2(50.0f, 0.0f), new Vector2(-50.0f, 0.0f));

            {
                CircleShape circle1 = new CircleShape(0.5f, 2);
                circle1.Position = new Vector2(-0.5f, 0.5f);

                CircleShape circle2 = new CircleShape(0.5f, 0);
                circle2.Position = new Vector2(0.5f, 0.5f);

                for (int i = 0; i < 10; ++i)
                {
                    double x = Rand.RandomFloat(-0.1f, 0.1f);

                    Body body = BodyFactory.CreateBody(World);
                    body.BodyType = BodyType.Dynamic;
                    body.Position = new Vector2(x + 5.0f, 1.05f + 2.5f * i);
					body.Rotation = Rand.RandomFloat(-Alt.FarseerPhysics.Settings.Pi, Alt.FarseerPhysics.Settings.Pi);

                    body.CreateFixture(circle1);
                    body.CreateFixture(circle2);
                }
            }

            {
                Vertices box = PolygonTools.CreateRectangle(0.25f, 0.5f);
                PolygonShape polygon1 = new PolygonShape(box, 2);

				box = PolygonTools.CreateRectangle(0.25f, 0.5f, new Vector2(0.0f, -0.5f), 0.5f * Alt.FarseerPhysics.Settings.Pi);
                PolygonShape polygon2 = new PolygonShape(box, 2);

                for (int i = 0; i < 10; ++i)
                {
                    double x = Rand.RandomFloat(-0.1f, 0.1f);

                    Body body = BodyFactory.CreateBody(World);
                    body.BodyType = BodyType.Dynamic;
                    body.Position = new Vector2(x - 5.0f, 1.05f + 2.5f * i);
					body.Rotation = Rand.RandomFloat(-Alt.FarseerPhysics.Settings.Pi, Alt.FarseerPhysics.Settings.Pi);

                    body.CreateFixture(polygon1);
                    body.CreateFixture(polygon2);
                }
            }

            {
				Alt.FarseerPhysics.Common.Transform xf1 = new Alt.FarseerPhysics.Common.Transform();
				xf1.q.Set(0.3524f * Alt.FarseerPhysics.Settings.Pi);
				xf1.p = Alt.FarseerPhysics.Common.MathUtils.Mul(ref xf1.q, new Vector2(1.0f, 0.0f));

                Vertices vertices = new Vertices(3);

				vertices.Add(Alt.FarseerPhysics.Common.MathUtils.Mul(ref xf1, new Vector2(-1.0f, 0.0f)));
				vertices.Add(Alt.FarseerPhysics.Common.MathUtils.Mul(ref xf1, new Vector2(1.0f, 0.0f)));
				vertices.Add(Alt.FarseerPhysics.Common.MathUtils.Mul(ref xf1, new Vector2(0.0f, 0.5f)));

                PolygonShape triangle1 = new PolygonShape(vertices, 2);

				Alt.FarseerPhysics.Common.Transform xf2 = new Alt.FarseerPhysics.Common.Transform();
				xf2.q.Set(-0.3524f * Alt.FarseerPhysics.Settings.Pi);
                xf2.p = MathUtils.Mul(ref xf2.q, new Vector2(-1.0f, 0.0f));

				vertices[0] = Alt.FarseerPhysics.Common.MathUtils.Mul(ref xf2, new Vector2(-1.0f, 0.0f));
				vertices[1] = Alt.FarseerPhysics.Common.MathUtils.Mul(ref xf2, new Vector2(1.0f, 0.0f));
				vertices[2] = Alt.FarseerPhysics.Common.MathUtils.Mul(ref xf2, new Vector2(0.0f, 0.5f));

                PolygonShape triangle2 = new PolygonShape(vertices, 2);

                for (int i = 0; i < 10; ++i)
                {
                    double x = Rand.RandomFloat(-0.1f, 0.1f);

                    Body body = BodyFactory.CreateBody(World);
                    body.BodyType = BodyType.Dynamic;
                    body.Position = new Vector2(x, 2.05f + 2.5f * i);

                    body.CreateFixture(triangle1);
                    body.CreateFixture(triangle2);
                }
            }

            {
                Vertices box = PolygonTools.CreateRectangle(1.5f, 0.15f);
                PolygonShape bottom = new PolygonShape(box, 4);

                box = PolygonTools.CreateRectangle(0.15f, 2.7f, new Vector2(-1.45f, 2.35f), 0.2f);
                PolygonShape left = new PolygonShape(box, 4);

                box = PolygonTools.CreateRectangle(0.15f, 2.7f, new Vector2(1.45f, 2.35f), -0.2f);
                PolygonShape right = new PolygonShape(box, 4);

                Body body = BodyFactory.CreateBody(World);
                body.BodyType = BodyType.Dynamic;
                body.Position = new Vector2(0.0f, 2.0f);

                body.CreateFixture(bottom);
                body.CreateFixture(left);
                body.CreateFixture(right);
            }
        }

        internal static FarseerPhysicsTest Create()
        {
            return new CompoundShapesTest();
        }
    }
}