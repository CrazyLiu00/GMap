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
using System.Diagnostics;

#if WINDOWS_PHONE_7 || WINDOWS_PHONE_71
using Alt.Diagnostics;
#endif

using Alt.FarseerPhysics.Collision;
using Alt.FarseerPhysics.Collision.Shapes;
using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    /// <summary>
    /// This stress tests the dynamic tree broad-phase. This also shows that tile
    /// based collision is _not_ smooth due to Box2D not knowing about adjacency.
    /// </summary>
    public class TilesTest : FarseerPhysicsTest
    {
        private const int Count = 20;
        private int _fixtureCount;
        private long _createTime;

        private TilesTest()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            {
                const double a = 0.5f;
                Body ground = BodyFactory.CreateBody(World, new Vector2(0, -a));
                const int N = 50;// 200;
                const int M = 1;// 10;
                Vector2 position = Vector2.Zero;
                position.Y = 0.0f;
                for (int j = 0; j < M; ++j)
                {
                    position.X = -N * a;
                    for (int i = 0; i < N; ++i)
                    {
                        PolygonShape shape = new PolygonShape(0);
                        shape.Vertices = PolygonTools.CreateRectangle(a, a, position, 0.0f);
                        ground.CreateFixture(shape);
                        ++_fixtureCount;
                        position.X += 2.0f * a;
                    }
                    position.Y -= 2.0f * a;
                }
            }

            {
                const double a = 0.5f;
                Vertices box = PolygonTools.CreateRectangle(a, a);
                PolygonShape shape = new PolygonShape(box, 5);

                Vector2 x = new Vector2(-7.0f, 0.75f);
                Vector2 deltaX = new Vector2(0.5625f, 1.25f);
                Vector2 deltaY = new Vector2(1.125f, 0.0f);

                for (int i = 0; i < Count; ++i)
                {
                    Vector2 y = x;

                    for (int j = i; j < Count; ++j)
                    {
                        Body body = BodyFactory.CreateBody(World);
                        body.BodyType = BodyType.Dynamic;
                        body.Position = y;
                        body.CreateFixture(shape);
                        ++_fixtureCount;
                        y += deltaY;
                    }

                    x += deltaX;
                }
            }

            timer.Stop();
            _createTime = timer.ElapsedMilliseconds;
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            ContactManager cm = World.ContactManager;
            DynamicTreeBroadPhase dt = (DynamicTreeBroadPhase)cm.BroadPhase;

            int height = dt.TreeHeight;
            int leafCount = dt.ProxyCount;
            double minimumNodeCount = 2 * leafCount - 1;
            double minimumHeight = (double)Math.Ceiling(Math.Log(minimumNodeCount) / Math.Log(2.0f));
            DrawString(string.Format("dynamic tree Height = {0}, min = {1}", height, (int)minimumHeight));
            DrawString(string.Format("create time = {0} ms, fixture count = {1}", _createTime, _fixtureCount));

            base.Update(settings);
        }

        internal static FarseerPhysicsTest Create()
        {
            return new TilesTest();
        }
    }
}