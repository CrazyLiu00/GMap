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
using Alt.FarseerPhysics.Collision;
using Alt.FarseerPhysics.Collision.Shapes;
using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Dynamics.Contacts;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class BreakableTest : FarseerPhysicsTest
    {
        double _angularVelocity;
        Body _body1;
        bool _break;
        bool _broke;
        Fixture _piece1;
        Fixture _piece2;
        PolygonShape _shape2;
        Vector2 _velocity;

        
        BreakableTest()
        {
            // Ground body
            BodyFactory.CreateEdge(World, new Vector2(-40.0f, 0.0f), new Vector2(40.0f, 0.0f));

            // Breakable dynamic body
            _body1 = BodyFactory.CreateBody(World);
            _body1.BodyType = BodyType.Dynamic;
            _body1.Position = new Vector2(0.0f, 40.0f);
			_body1.Rotation = 0.25f * Alt.FarseerPhysics.Settings.Pi;

            Vertices box = PolygonTools.CreateRectangle(0.5f, 0.5f, new Vector2(-0.5f, 0.0f), 0.0f);

            PolygonShape shape1 = new PolygonShape(box, 1);
            _piece1 = _body1.CreateFixture(shape1);

            box = PolygonTools.CreateRectangle(0.5f, 0.5f, new Vector2(0.5f, 0.0f), 0.0f);
            _shape2 = new PolygonShape(box, 1);

            _piece2 = _body1.CreateFixture(_shape2);

            _break = false;
            _broke = false;
        }

        public override void Initialize()
        {
            //load texture that will represent the physics body
            Bitmap polygonTexture = Bitmap.FromFile("AltData/FarseerPhysics/Testbed/Content/Rock.png");

            //Create an array to hold the data from the texture
            uint[] data = new uint[polygonTexture.PixelWidth * polygonTexture.PixelHeight];

            //Transfer the texture data to the array
            BitmapData bitmapData = polygonTexture.LockBits(ImageLockMode.ReadOnly);
            byte[] src_buffer = bitmapData.Scan0;
            System.Buffer.BlockCopy(src_buffer, 0, data, 0, src_buffer.Length);
            polygonTexture.UnlockBits(bitmapData);

            Vertices verts = PolygonTools.CreatePolygon(data, polygonTexture.PixelWidth);
            Vector2 scale = new Vector2(0.07f, 0.07f);
            verts.Scale(ref scale);

            BodyFactory.CreateBreakableBody(World, verts, 50, new Vector2(-10, 25));

            base.Initialize();
        }

        protected override void PostSolve(Contact contact, ContactVelocityConstraint impulse)
        {
            if (_broke)
            {
                // The body already broke.
                return;
            }

            // Should the body break?
            double maxImpulse = 0.0f;
            Manifold manifold = contact.Manifold;

            for (int i = 0; i < manifold.PointCount; ++i)
            {
                maxImpulse = Math.Max(maxImpulse, impulse.points[i].normalImpulse);
            }

            if (maxImpulse > 40.0f)
            {
                // Flag the body for breaking.
                _break = true;
            }
        }

        private void Break()
        {
            // Create two bodies from one.
            Body body1 = _piece1.Body;
            Vector2 center = body1.WorldCenter;

            body1.DestroyFixture(_piece2);
            _piece2 = null;

            Body body2 = BodyFactory.CreateBody(World);
            body2.BodyType = BodyType.Dynamic;
            body2.Position = body1.Position;
            body2.Rotation = body1.Rotation;

            _piece2 = body2.CreateFixture(_shape2);

            // Compute consistent velocities for new bodies based on
            // cached velocity.
            Vector2 center1 = body1.WorldCenter;
            Vector2 center2 = body2.WorldCenter;

            Vector2 velocity1 = _velocity + MathUtils.Cross(_angularVelocity, center1 - center);
            Vector2 velocity2 = _velocity + MathUtils.Cross(_angularVelocity, center2 - center);

            body1.AngularVelocity = _angularVelocity;
            body1.LinearVelocity = velocity1;

            body2.AngularVelocity = _angularVelocity;
            body2.LinearVelocity = velocity2;
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            if (_break)
            {
                Break();
                _broke = true;
                _break = false;
            }

            // Cache velocities to improve movement on breakage.
            if (_broke == false)
            {
                _velocity = _body1.LinearVelocity;
                _angularVelocity = _body1.AngularVelocity;
            }

            base.Update(settings);
        }

        internal static FarseerPhysicsTest Create()
        {
            return new BreakableTest();
        }
    }
}