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

using System.Diagnostics;

using Alt.FarseerPhysics.Collision.Shapes;
using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class VerticalStackTest : FarseerPhysicsTest
    {
        const int ColumnCount = 5;
        const int RowCount = 16;
        Body[] _bodies = new Body[RowCount * ColumnCount];
        Body _bullet;
        int[] _indices = new int[RowCount * ColumnCount];

        
        VerticalStackTest()
        {
            //Ground
            BodyFactory.CreateEdge(World, new Vector2(-40.0f, 0.0f), new Vector2(40.0f, 0.0f));
            BodyFactory.CreateEdge(World, new Vector2(20.0f, 0.0f), new Vector2(20.0f, 20.0f));

            double[] xs = new[] { 0.0, -10.0, -5.0, 5.0, 10.0 };

            for (int j = 0; j < ColumnCount; ++j)
            {
                PolygonShape shape = new PolygonShape(1);
                shape.Vertices = PolygonTools.CreateRectangle(0.5f, 0.5f);

                for (int i = 0; i < RowCount; ++i)
                {
                    int n = j * RowCount + i;
                    Debug.Assert(n < RowCount * ColumnCount);
                    _indices[n] = n;

                    const double x = 0.0f;
                    //double x = Rand.RandomFloat-0.02f, 0.02f);
                    //double x = i % 2 == 0 ? -0.025f : 0.025f;
                    Body body = BodyFactory.CreateBody(World);
                    body.BodyType = BodyType.Dynamic;
                    body.Position = new Vector2(xs[j] + x, 0.752f + 1.54f * i);
                    body.UserData = _indices[n];

                    _bodies[n] = body;

                    Fixture fixture = body.CreateFixture(shape);
                    fixture.Friction = 0.3f;
                }
            }

            _bullet = null;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.OemComma)
            {
                if (_bullet != null)
                {
                    World.RemoveBody(_bullet);
                    _bullet = null;
                }

                {
                    CircleShape shape = new CircleShape(0.25f, 20);

                    _bullet = BodyFactory.CreateBody(World);
                    _bullet.BodyType = BodyType.Dynamic;
                    _bullet.IsBullet = true;
                    _bullet.Position = new Vector2(-31.0f, 5.0f);

                    Fixture fixture = _bullet.CreateFixture(shape);
                    fixture.Restitution = 0.05f;

                    _bullet.LinearVelocity = new Vector2(400.0f, 0.0f);
                }
            }

            base.OnKeyDown(e);
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            base.Update(settings);

            DrawString("Press: (,) to launch a bullet.");

            //if (StepCount == 300)
            //{
            //    if (_bullet != null)
            //    {
            //        World.Remove(_bullet);
            //        _bullet = null;
            //    }

            //    {
            //        CircleShape shape = new CircleShape(0.25f, 20);

            //        _bullet = BodyFactory.CreateBody(World);
            //        _bullet.BodyType = BodyType.Dynamic;
            //        _bullet.Bullet = true;
            //        _bullet.Position = new Vector2(-31.0f, 5.0f);
            //        _bullet.LinearVelocity = new Vector2(400.0f, 0.0f);

            //        Fixture fixture = _bullet.CreateFixture(shape);
            //        fixture.Restitution = 0.05f;
            //    }
            //}

            //
        }

        internal static FarseerPhysicsTest Create()
        {
            return new VerticalStackTest();
        }
    }
}