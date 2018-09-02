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

using Alt.FarseerPhysics;
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
    /// This tests stacking. It also shows how to use World.Query()
    /// and AABB.TestOverlap().
    /// 
    /// This callback is called by World.QueryAABB(). We find all the fixtures
    /// that overlap an AABB. Of those, we use AABB.TestOverlap() to determine which fixtures
    /// overlap a circle. Up to 4 overlapped fixtures will be highlighted with a yellow border.
    /// </summary>
    public class PolyShapesCallback
    {
        const int MaxCount = 4;

        internal CircleShape Circle = new CircleShape(0, 0);
        internal AltSketchDebugViewExt DebugDraw;
		internal Alt.FarseerPhysics.Common.Transform Transform;
        int _count;

        
        void DrawFixture(Fixture fixture)
        {
            Color color = new ColorR(0.95f, 0.95f, 0.6f);
			Alt.FarseerPhysics.Common.Transform xf;
            fixture.Body.GetTransform(out xf);

            switch (fixture.Shape.ShapeType)
            {
                case ShapeType.Circle:
                    {
                        CircleShape circle = (CircleShape)fixture.Shape;

                        Vector2 center = MathUtils.Mul(ref xf, circle.Position);
                        double radius = circle.Radius;

                        DebugDraw.DrawSolidCircle(center, radius, Vector2.Zero, color);
                    }
                    break;

                case ShapeType.Polygon:
                    {
                        PolygonShape poly = (PolygonShape)fixture.Shape;
                        int vertexCount = poly.Vertices.Count;
				Debug.Assert(vertexCount <= Alt.FarseerPhysics.Settings.MaxPolygonVertices);
				Vector2[] vertices = new Vector2[Alt.FarseerPhysics.Settings.MaxPolygonVertices];

                        for (int i = 0; i < vertexCount; ++i)
                        {
                            vertices[i] = MathUtils.Mul(ref xf, poly.Vertices[i]);
                        }

                        DebugDraw.DrawSolidPolygon(vertices, vertexCount, color);
                    }
                    break;
            }
        }

        /// <summary>
        /// Called for each fixture found in the query AABB.
        /// </summary>
        /// <param name="fixture">The fixture.</param>
        /// <returns>false to terminate the query.</returns>
        public bool ReportFixture(Fixture fixture)
        {
            if (_count == MaxCount)
            {
                return false;
            }

            Body body = fixture.Body;
            Shape shape = fixture.Shape;

			Alt.FarseerPhysics.Common.Transform xf;
            body.GetTransform(out xf);

			bool overlap = Alt.FarseerPhysics.Collision.Collision.TestOverlap(shape, 0, Circle, 0, ref xf, ref Transform);

            if (overlap)
            {
                DrawFixture(fixture);
                ++_count;
            }

            return true;
        }
    }

    public class PolyShapesTest : FarseerPhysicsTest
    {
        private const int MaxBodies = 256;
        private Body[] _bodies = new Body[MaxBodies];
        private int _bodyIndex;
        private CircleShape _circle = new CircleShape(0, 0);
        private PolygonShape[] _polygons = new PolygonShape[4];

        private PolyShapesTest()
        {
            // Ground body
            BodyFactory.CreateEdge(World, new Vector2(-40.0f, 0.0f), new Vector2(40.0f, 0.0f));
            
            {
                Vertices vertices = new Vertices(3);
                vertices.Add(new Vector2(-0.5f, 0.0f));
                vertices.Add(new Vector2(0.5f, 0.0f));
                vertices.Add(new Vector2(0.0f, 1.5f));
                _polygons[0] = new PolygonShape(vertices, 1);
            }

            {
                Vertices vertices3 = new Vertices(3);
                vertices3.Add(new Vector2(-0.1f, 0.0f));
                vertices3.Add(new Vector2(0.1f, 0.0f));
                vertices3.Add(new Vector2(0.0f, 1.5f));
                _polygons[1] = new PolygonShape(vertices3, 1);
            }

            {
                const double w = 1.0f;
                double b = w / (2.0f + (double)Math.Sqrt(2.0));
                double s = (double)Math.Sqrt(2.0) * b;

                Vertices vertices8 = new Vertices(8);
                vertices8.Add(new Vector2(0.5f * s, 0.0f));
                vertices8.Add(new Vector2(0.5f * w, b));
                vertices8.Add(new Vector2(0.5f * w, b + s));
                vertices8.Add(new Vector2(0.5f * s, w));
                vertices8.Add(new Vector2(-0.5f * s, w));
                vertices8.Add(new Vector2(-0.5f * w, b + s));
                vertices8.Add(new Vector2(-0.5f * w, b));
                vertices8.Add(new Vector2(-0.5f * s, 0.0f));

                _polygons[2] = new PolygonShape(vertices8, 1);
            }

            {
                Vertices box = PolygonTools.CreateRectangle(0.5f, 0.5f);
                _polygons[3] = new PolygonShape(box, 1);
            }

            {
                _circle.Radius = 0.5f;
            }

            _bodyIndex = 0;
        }

        private void Create(int index)
        {
            if (_bodies[_bodyIndex] != null)
            {
                World.RemoveBody(_bodies[_bodyIndex]);
                _bodies[_bodyIndex] = null;
            }

            _bodies[_bodyIndex] = BodyFactory.CreateBody(World);
            _bodies[_bodyIndex].BodyType = BodyType.Dynamic;

            double x = Rand.RandomFloat(-2.0f, 2.0f);

            _bodies[_bodyIndex].Position = new Vector2(x, 10.0f);
			_bodies[_bodyIndex].Rotation = Rand.RandomFloat(-Alt.FarseerPhysics.Settings.Pi, Alt.FarseerPhysics.Settings.Pi);

            if (index == 4)
            {
                _bodies[_bodyIndex].AngularDamping = 0.02f;
            }


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
            if (e.KeyCode == Keys.D2)
            {
                Create(1);
            }
            if (e.KeyCode == Keys.D3)
            {
                Create(2);
            }
            if (e.KeyCode == Keys.D4)
            {
                Create(3);
            }
            if (e.KeyCode == Keys.D5)
            {
                Create(4);
            }
            if (e.KeyCode == Keys.A)
            {
                for (int i = 0; i < MaxBodies; i += 2)
                {
                    if (_bodies[i] != null)
                    {
                        bool enabled = _bodies[i].Enabled;
                        _bodies[i].Enabled = !enabled;
                    }
                }
            }
            if (e.KeyCode == Keys.D)
            {
                DestroyBody();
            }

            base.OnKeyDown(e);
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            base.Update(settings);

            PolyShapesCallback callback = new PolyShapesCallback();
            callback.Circle.Radius = 2.0f;
            callback.Circle.Position = new Vector2(0.0f, 1.1f);
            callback.Transform.SetIdentity();
            callback.DebugDraw = DebugView;

            AABB aabb;
            callback.Circle.ComputeAABB(out aabb, ref callback.Transform, 0);

            
            World.QueryAABB(callback.ReportFixture, ref aabb);

            Color color = new ColorR(0.4f, 0.7f, 0.8f);
            DebugView.DrawCircle(callback.Circle.Position, callback.Circle.Radius, color);
            

            DrawString("Press 1-5 to drop stuff");
            DrawString("Press a to (de)activate some bodies");
            DrawString("Press d to destroy a body");
        }

        internal static FarseerPhysicsTest Create()
        {
            return new PolyShapesTest();
        }
    }
}