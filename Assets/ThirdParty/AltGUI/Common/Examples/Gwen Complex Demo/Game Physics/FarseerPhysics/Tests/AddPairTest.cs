/*
* Farseer Physics Engine
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
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class AddPairTest : FarseerPhysicsTest
    {
        AddPairTest()
        {
            World.Gravity = Vector2.Zero;

            const double minX = -6.0;
            const double maxX = 0.0;
            const double minY = 4.0;
            const double maxY = 6.0;

            for (int i = 0; i < 200;//400;
                i++)
            {
                Body body = BodyFactory.CreateCircle(World, 0.1, 0.01, new Vector2(Rand.RandomFloat(minX, maxX), Rand.RandomFloat(minY, maxY)));
                body.BodyType = BodyType.Dynamic;
            }

            {
                Body body = BodyFactory.CreateRectangle(World, 3, 3, 1, new Vector2(-40, 5));
                body.BodyType = BodyType.Dynamic;
                body.IsBullet = true;
                body.LinearVelocity = new Vector2(150.0, 0.0);
            }
        }


        public static FarseerPhysicsTest Create()
        {
            return new AddPairTest();
        }
    }
}
