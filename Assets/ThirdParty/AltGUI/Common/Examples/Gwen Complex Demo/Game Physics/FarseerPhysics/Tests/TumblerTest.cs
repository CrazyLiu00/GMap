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


using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Dynamics.Joints;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class TumblerTest : FarseerPhysicsTest
    {
        const int Count = 500;//800;
        int _count;


        TumblerTest()
        {
            Body ground = BodyFactory.CreateBody(World);

            Body tumblerBody = BodyFactory.CreateBody(World, new Vector2(0, 10));
            tumblerBody.SleepingAllowed = false;
            tumblerBody.BodyType = BodyType.Dynamic;

            FixtureFactory.AttachRectangle(1, 20, 5, new Vector2(10, 0), tumblerBody);
            FixtureFactory.AttachRectangle(1, 20, 5, new Vector2(-10, 0), tumblerBody);
            FixtureFactory.AttachRectangle(20, 1, 5, new Vector2(0, 10), tumblerBody);
            FixtureFactory.AttachRectangle(20, 1, 5, new Vector2(0, -10), tumblerBody);

            RevoluteJoint joint = JointFactory.CreateRevoluteJoint(World, ground, tumblerBody, new Vector2(0, 10), Vector2.Zero);
            joint.ReferenceAngle = 0.0f;
			joint.MotorSpeed = 0.05f * Alt.FarseerPhysics.Settings.Pi;
            joint.MaxMotorTorque = 1e8f;
            joint.MotorEnabled = true;
        }


        public override void Update(FarseerPhysicsGameSettings settings)
        {
            base.Update(settings);

            if (_count < Count)
            {
                Body box = BodyFactory.CreateRectangle(World, 0.125f * 2, 0.125f * 2, 1, new Vector2(0, 10));
                box.BodyType = BodyType.Dynamic;
                ++_count;
            }
        }


        public static FarseerPhysicsTest Create()
        {
            return new TumblerTest();
        }
    }
}
