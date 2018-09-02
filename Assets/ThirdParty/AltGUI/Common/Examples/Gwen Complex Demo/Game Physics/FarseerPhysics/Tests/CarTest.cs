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
using Alt.FarseerPhysics.Dynamics.Joints;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class CarTest : FarseerPhysicsTest
    {
        Body _car;
        double _hz;
        double _speed;
        WheelJoint _spring1;
        WheelJoint _spring2;
        Body _wheel1;
        Body _wheel2;
        double _zeta;

        
        CarTest()
        {
            _hz = 4.0;
            _zeta = 0.7;
            _speed = 50.0;

            Body ground = BodyFactory.CreateEdge(World, new Vector2(-20.0f, 0.0f), new Vector2(20.0f, 0.0f));
            {
                double[] hs = new[] { 0.25, 1.0, 4.0, 0.0, 0.0, -1.0, -2.0, -2.0, -1.25, 0.0 };

                double x = 20.0, y1 = 0.0;
                const double dx = 5.0;

                for (int i = 0; i < 10; ++i)
                {
                    double y2 = hs[i];
                    FixtureFactory.AttachEdge(new Vector2(x, y1), new Vector2(x + dx, y2), ground);
                    y1 = y2;
                    x += dx;
                }

                for (int i = 0; i < 10; ++i)
                {
                    double y2 = hs[i];
                    FixtureFactory.AttachEdge(new Vector2(x, y1), new Vector2(x + dx, y2), ground);
                    y1 = y2;
                    x += dx;
                }

                FixtureFactory.AttachEdge(new Vector2(x, 0.0), new Vector2(x + 40.0, 0.0), ground);
                x += 80.0;
                FixtureFactory.AttachEdge(new Vector2(x, 0.0), new Vector2(x + 40.0, 0.0), ground);
                x += 40.0;
                FixtureFactory.AttachEdge(new Vector2(x, 0.0), new Vector2(x + 10.0, 5.0), ground);
                x += 20.0;
                FixtureFactory.AttachEdge(new Vector2(x, 0.0), new Vector2(x + 40.0, 0.0), ground);
                x += 40.0;
                FixtureFactory.AttachEdge(new Vector2(x, 0.0), new Vector2(x, 20.0), ground);

                ground.Friction = 0.6f;
            }

            // Teeter
            {
                Body body = new Body(World);
                body.BodyType = BodyType.Dynamic;
                body.Position = new Vector2(140.0, 1.0);

                PolygonShape box = new PolygonShape(1);
                box.Vertices = PolygonTools.CreateRectangle(10.0, 0.25);
                body.CreateFixture(box);

                RevoluteJoint jd = JointFactory.CreateRevoluteJoint(World, ground, body, Vector2.Zero);
				jd.LowerLimit = -8.0 * Alt.FarseerPhysics.Settings.Pi / 180.0;
				jd.UpperLimit = 8.0 * Alt.FarseerPhysics.Settings.Pi / 180.0;
                jd.LimitEnabled = true;

                body.ApplyAngularImpulse(100.0);
            }

            //Bridge
            {
                const int N = 20;
                PolygonShape shape = new PolygonShape(1);
                shape.Vertices = PolygonTools.CreateRectangle(1.0, 0.125);

                Body prevBody = ground;
                for (int i = 0; i < N; ++i)
                {
                    Body body = new Body(World);
                    body.BodyType = BodyType.Dynamic;
                    body.Position = new Vector2(161.0 + 2.0 * i, -0.125);
                    Fixture fix = body.CreateFixture(shape);
                    fix.Friction = 0.6;

                    Vector2 anchor = new Vector2(-1, 0);
                    JointFactory.CreateRevoluteJoint(World, prevBody, body, anchor);

                    prevBody = body;
                }

                Vector2 anchor2 = new Vector2(1.0, 0);
                JointFactory.CreateRevoluteJoint(World, ground, prevBody, anchor2);
            }

            // Boxes
            {
                PolygonShape box = new PolygonShape(0.5);
                box.Vertices = PolygonTools.CreateRectangle(0.5, 0.5);

                Body body = new Body(World);
                body.BodyType = BodyType.Dynamic;
                body.Position = new Vector2(230.0, 0.5);
                body.CreateFixture(box);

                body = new Body(World);
                body.BodyType = BodyType.Dynamic;
                body.Position = new Vector2(230.0, 1.5);
                body.CreateFixture(box);

                body = new Body(World);
                body.BodyType = BodyType.Dynamic;
                body.Position = new Vector2(230.0, 2.5);
                body.CreateFixture(box);

                body = new Body(World);
                body.BodyType = BodyType.Dynamic;
                body.Position = new Vector2(230.0, 3.5);
                body.CreateFixture(box);

                body = new Body(World);
                body.BodyType = BodyType.Dynamic;
                body.Position = new Vector2(230.0, 4.5);
                body.CreateFixture(box);
            }

            // Car
            {
                Vertices vertices = new Vertices(8);
                vertices.Add(new Vector2(-1.5, -0.5));
                vertices.Add(new Vector2(1.5, -0.5));
                vertices.Add(new Vector2(1.5, 0.0));
                vertices.Add(new Vector2(0.0, 0.9));
                vertices.Add(new Vector2(-1.15, 0.9));
                vertices.Add(new Vector2(-1.5, 0.2));

                PolygonShape chassis = new PolygonShape(vertices, 1);

                CircleShape circle = new CircleShape(0.4, 1);

                _car = new Body(World);
                _car.BodyType = BodyType.Dynamic;
                _car.Position = new Vector2(0.0, 1.0);
                _car.CreateFixture(chassis);

                _wheel1 = new Body(World);
                _wheel1.BodyType = BodyType.Dynamic;
                _wheel1.Position = new Vector2(-1.0, 0.35);
                _wheel1.CreateFixture(circle);
                _wheel1.Friction = 0.9;

                _wheel2 = new Body(World);
                _wheel2.BodyType = BodyType.Dynamic;
                _wheel2.Position = new Vector2(1.0, 0.4);
                _wheel2.CreateFixture(circle);
                _wheel2.Friction = 0.9;

                Vector2 axis = new Vector2(0.0, 1.0);
                _spring1 = new WheelJoint(_car, _wheel1, _wheel1.Position, axis, true);
                _spring1.MotorSpeed = 0.0;
                _spring1.MaxMotorTorque = 10;// 20.0;
                _spring1.MotorEnabled = true;
                _spring1.Frequency = _hz;
                _spring1.DampingRatio = _zeta;
                World.AddJoint(_spring1);

                _spring2 = new WheelJoint(_car, _wheel2, _wheel2.Position, axis, true);
                _spring2.MotorSpeed = 0.0;
                _spring2.MaxMotorTorque = 10.0;
                _spring2.MotorEnabled = false;
                _spring2.Frequency = _hz;
                _spring2.DampingRatio = _zeta;
                World.AddJoint(_spring2);
            }
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                _spring1.MotorSpeed = _speed;
            }
            else if (e.KeyCode == Keys.S)
            {
                _spring1.MotorSpeed = 0.0;
            }
            else if (e.KeyCode == Keys.D)
            {
                _spring1.MotorSpeed = -_speed;
            }
            else if (e.KeyCode == Keys.Q)
            {
                _hz = Math.Max(0.0, _hz - 1.0);
                _spring1.Frequency = _hz;
                _spring2.Frequency = _hz;
            }
            else if (e.KeyCode == Keys.E)
            {
                _hz += 1.0;
                _spring1.Frequency = _hz;
                _spring2.Frequency = _hz;
            }

            base.OnKeyDown(e);
        }


        public override void Update(FarseerPhysicsGameSettings settings)
        {
            DrawString("Keys: left = a, brake = s, right = d, hz down = q, hz up = e");

            DrawString(string.Format("frequency = {0} hz, damping ratio = {1}", _hz, _zeta));

            DrawString(string.Format("actual speed = {0} rad/sec", _spring1.JointSpeed.ToString("F1")));


            base.Update(settings);


            Vector2 carPosition = _car.Position;
            carPosition.X = -carPosition.X;
            ViewCenter = carPosition;
        }


        internal static FarseerPhysicsTest Create()
        {
            return new CarTest();
        }
    }
}
