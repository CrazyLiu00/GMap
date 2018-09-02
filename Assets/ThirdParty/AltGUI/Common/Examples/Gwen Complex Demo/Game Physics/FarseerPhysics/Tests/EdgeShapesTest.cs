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

using System;

using Alt.FarseerPhysics.Collision.Shapes;
using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class EdgeShapesTest : FarseerPhysicsTest
    {
        const int MaxBodies = 256;
        double _angle;
        Body[] _bodies = new Body[MaxBodies];
        int _bodyIndex;
        CircleShape _circle;
        Fixture _fixture;
        Vector2 _normal;
        Vector2 _point;
        PolygonShape[] _polygons = new PolygonShape[4];

        
        EdgeShapesTest()
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

            {
                Vertices vertices = new Vertices(3);
                vertices.Add(new Vector2(-0.5f, 0.0f));
                vertices.Add(new Vector2(0.5f, 0.0f));
                vertices.Add(new Vector2(0.0f, 1.5f));
                _polygons[0] = new PolygonShape(vertices,20);
            }

            {
                Vertices vertices = new Vertices(3);
                vertices.Add(new Vector2(-0.1f, 0.0f));
                vertices.Add(new Vector2(0.1f, 0.0f));
                vertices.Add(new Vector2(0.0f, 1.5f));
                _polygons[1] = new PolygonShape(vertices,20);
            }

            {
                const double w = 1.0f;
                double b = w / (2.0f + (double)Math.Sqrt(2.0f));
                double s = (double)Math.Sqrt(2.0f) * b;

                Vertices vertices = new Vertices(8);
                vertices.Add(new Vector2(0.5f * s, 0.0f));
                vertices.Add(new Vector2(0.5f * w, b));
                vertices.Add(new Vector2(0.5f * w, b + s));
                vertices.Add(new Vector2(0.5f * s, w));
                vertices.Add(new Vector2(-0.5f * s, w));
                vertices.Add(new Vector2(-0.5f * w, b + s));
                vertices.Add(new Vector2(-0.5f * w, b));
                vertices.Add(new Vector2(-0.5f * s, 0.0f));
                _polygons[2] = new PolygonShape(vertices,20);
            }

            {
                _polygons[3] = new PolygonShape(20);
                _polygons[3].Vertices = PolygonTools.CreateRectangle(0.5f, 0.5f);
            }

            {
                _circle = new CircleShape(0.5f, 1);
            }

            _bodyIndex = 0;
            _angle = 0.0f;
        }

        private void Create(int index)
        {
            if (_bodies[_bodyIndex] != null)
            {
                World.RemoveBody(_bodies[_bodyIndex]);
                _bodies[_bodyIndex] = null;
            }

            double x = Rand.RandomFloat(-10.0f, 10.0f);
            double y = Rand.RandomFloat(10.0f, 20.0f);

            _bodies[_bodyIndex] = BodyFactory.CreateBody(World);
            if (index == 4)
            {
                _bodies[_bodyIndex].AngularDamping = 0.02f;
            }
            _bodies[_bodyIndex].Position = new Vector2(x, y);
            _bodies[_bodyIndex].Rotation = Rand.RandomFloat(-(double)Math.PI, (double)Math.PI);
            _bodies[_bodyIndex].BodyType = BodyType.Dynamic;

            if (index < 4)
            {
                Fixture fixture = _bodies[_bodyIndex].CreateFixture(_polygons[index]);
                fixture.Friction = 0.3f;
            }
            else
            {
                Fixture fixture = _bodies[_bodyIndex].CreateFixture(_circle);
                fixture.Friction = 0.3f;
            }

            _bodyIndex = (_bodyIndex + 1) % MaxBodies;
        }

        private void DestroyBody()
        {
            for (int i = 0; i < MaxBodies; ++i)
            {
                if (_bodies[i] != null)
                {
                    World.RemoveBody(_bodies[i]);
                    _bodies[i] = null;
                    return;
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D1)
            {
                Create(0);
            }
            else if (e.KeyCode == Keys.D2)
            {
                Create(1);
            }
            else if (e.KeyCode == Keys.D3)
            {
                Create(2);
            }
            else if (e.KeyCode == Keys.D4)
            {
                Create(3);
            }
            else if (e.KeyCode == Keys.D5)
            {
                Create(4);
            }
            else if (e.KeyCode == Keys.D)
            {
                DestroyBody();
            }

            base.OnKeyDown(e);
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            bool advanceRay = settings.Pause == false || settings.SingleStep;

            base.Update(settings);
            DrawString("Press 1-5 to drop stuff");
            

            const double l = 25.0f;
            Vector2 point1 = new Vector2(0.0f, 10.0f);
            Vector2 d = new Vector2(l * (double)Math.Cos(_angle), -l * Math.Abs((double)Math.Sin(_angle)));
            Vector2 point2 = point1 + d;

            _fixture = null;

            World.RayCast((fixture, point, normal, fraction) =>
                              {
                                  _fixture = fixture;
                                  _point = point;
                                  _normal = normal;

                                  return fraction;
                              }, point1, point2);

            
            if (_fixture != null)
            {
                DebugView.DrawPoint(_point, 0.5f, new ColorR(0.4f, 0.9f, 0.4f));

                DebugView.DrawSegment(point1, _point, new ColorR(0.8f, 0.8f, 0.8f));

                Vector2 head = _point + 0.5f * _normal;
                DebugView.DrawSegment(_point, head, new ColorR(0.9f, 0.9f, 0.4f));
            }
            else
            {
                DebugView.DrawSegment(point1, point2, new ColorR(0.8f, 0.8f, 0.8f));
            }
            

            if (advanceRay)
            {
				_angle += 0.25f * Alt.FarseerPhysics.Settings.Pi / 180.0f;
            }
        }

        internal static FarseerPhysicsTest Create()
        {
            return new EdgeShapesTest();
        }
    }
}