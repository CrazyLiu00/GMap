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
using Alt.FarseerPhysics.Dynamics.Joints;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    /// <summary>
    /// A motor driven slider crank with joint friction.
    /// </summary>
    public class SliderCrankTest : FarseerPhysicsTest
    {
        RevoluteJoint _joint1;
        PrismaticJoint _joint2;

        private SliderCrankTest()
        {
            Body ground;
            {
                ground = BodyFactory.CreateBody(World);

                EdgeShape shape = new EdgeShape(new Vector2(-40.0f, 0.0f), new Vector2(40.0f, 0.0f));
                ground.CreateFixture(shape);
            }

            {
                Body prevBody = ground;

                // Define crank.
                {
                    PolygonShape shape = new PolygonShape(2);
                    shape.Vertices = PolygonTools.CreateRectangle(0.5f, 2.0f);

                    Body body = BodyFactory.CreateBody(World);
                    body.BodyType = BodyType.Dynamic;
                    body.Position = new Vector2(0.0f, 7.0f);

                    body.CreateFixture(shape);

                    _joint1 = new RevoluteJoint(prevBody, body, new Vector2(0f, 5f), true);
					_joint1.MotorSpeed = 1.0f * Alt.FarseerPhysics.Settings.Pi;
                    _joint1.MaxMotorTorque = 10000.0f;
                    _joint1.MotorEnabled = true;
                    World.AddJoint(_joint1);

                    prevBody = body;
                }

                // Define follower.
                {
                    PolygonShape shape = new PolygonShape(2);
                    shape.Vertices = PolygonTools.CreateRectangle(0.5f, 4.0f);

                    Body body = BodyFactory.CreateBody(World);
                    body.BodyType = BodyType.Dynamic;
                    body.Position = new Vector2(0.0f, 13.0f);

                    body.CreateFixture(shape);

                    RevoluteJoint rjd3 = new RevoluteJoint(prevBody, body, new Vector2(0, 9), true);
                    rjd3.MotorEnabled = false;
                    World.AddJoint(rjd3);

                    prevBody = body;
                }

                // Define piston
                {
                    PolygonShape shape = new PolygonShape(2);
                    shape.Vertices = PolygonTools.CreateRectangle(1.5f, 1.5f);

                    Body body = BodyFactory.CreateBody(World);
                    body.BodyType = BodyType.Dynamic;
                    body.Position = new Vector2(0.0f, 17.0f);

                    body.CreateFixture(shape);

                    RevoluteJoint rjd = new RevoluteJoint(prevBody, body, new Vector2(0, 17), true);
                    World.AddJoint(rjd);

                    _joint2 = new PrismaticJoint(ground, body, new Vector2(0.0f, 17.0f), new Vector2(0.0f, 1.0f), true);
                    _joint2.MaxMotorForce = 1000.0f;
                    _joint2.MotorEnabled = true;

                    World.AddJoint(_joint2);
                }

                // Create a payload
                {
                    PolygonShape shape = new PolygonShape(2);
                    shape.Vertices = PolygonTools.CreateRectangle(1.5f, 1.5f);

                    Body body = BodyFactory.CreateBody(World);
                    body.BodyType = BodyType.Dynamic;
                    body.Position = new Vector2(0.0f, 23.0f);

                    body.CreateFixture(shape);
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F)
            {
                _joint2.MotorEnabled = !_joint2.MotorEnabled;
                _joint2.BodyB.Awake = true;
            }
            if (e.KeyCode == Keys.M)
            {
                _joint1.MotorEnabled = !_joint1.MotorEnabled;
                _joint1.BodyB.Awake = true;
            }

            base.OnKeyDown(e);
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            base.Update(settings);
            DrawString("Keys: (f) toggle friction, (m) toggle motor");

            double torque = _joint1.GetMotorTorque(settings.Hz);
            DrawString("Motor Torque = " + torque);
        }

        internal static FarseerPhysicsTest Create()
        {
            return new SliderCrankTest();
        }
    }
}