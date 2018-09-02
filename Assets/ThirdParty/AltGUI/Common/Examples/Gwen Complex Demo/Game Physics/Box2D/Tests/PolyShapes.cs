/*
* Box2D.XNA port of Box2D:
* Copyright (c) 2009 Brandon Furtwangler, Nathan Furtwangler
*
* Original source Box2D:
* Copyright (c) 2006-2009 Erin Catto http://www.gphysics.com 
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

using Alt.Box2D;
using Alt.GUI.Box2D.Demo.Testbed.Framework;
using Alt.Sketch;


namespace Alt.GUI.Box2D.Demo.Testbed.Tests
{
    /// This tests stacking. It also shows how to use b2World::Query
    /// and b2TestOverlap.

    /// This callback is called by b2World::QueryAABB. We find all the fixtures
    /// that overlap an AABB. Of those, we use b2TestOverlap to determine which fixtures
    /// overlap a circle. Up to 4 overlapped fixtures will be highlighted with a yellow border.
    public class PolyShapesCallback
    {
        static int e_maxCount = 4;


        void DrawFixture(Fixture fixture)
	    {
		    Color color = new ColorR(0.95f, 0.95f, 0.6f);
            Alt.Box2D.Transform xf;
            fixture.GetBody().GetTransform(out xf);

		    switch (fixture.ShapeType)
		    {
		    case ShapeType.Circle:
			    {
				    CircleShape circle = (CircleShape)fixture.GetShape();

				    Vector2 center = MathUtils.Multiply(ref xf, circle._p);
				    double radius = circle._radius;

				    _debugDraw.DrawCircle(center, radius, color);
			    }
			    break;

		    case ShapeType.Polygon:
			    {
				    PolygonShape poly = (PolygonShape)fixture.GetShape();
				    int vertexCount = poly._vertexCount;
                    Debug.Assert(vertexCount <= Alt.Box2D.Settings.b2_maxPolygonVertices);
                    FixedArray8<Vector2> vertices = new FixedArray8<Vector2>();

				    for (int i = 0; i < vertexCount; ++i)
				    {
					    vertices[i] = MathUtils.Multiply(ref xf, poly._vertices[i]);
				    }

				    _debugDraw.DrawPolygon(ref vertices, vertexCount, color);
			    }
			    break;
		    }
	    }


        /// Called for each fixture found in the query AABB.
        /// @return false to terminate the query.
        public bool ReportFixture(FixtureProxy fixtureProxy)
        {
            var fixture = fixtureProxy.fixture;

            if (_count == e_maxCount)
            {
                return false;
            }

            Body body = fixture.GetBody();
            Shape shape = fixture.GetShape();

            Alt.Box2D.Transform xf;
            body.GetTransform(out xf);

            bool overlap = AABB.TestOverlap(shape, 0, _circle, 0, ref xf, ref _transform);

            if (overlap)
            {
                DrawFixture(fixture);
                ++_count;
            }

            return true;
        }

        internal CircleShape _circle = new CircleShape();
        internal Alt.Box2D.Transform _transform;
        internal DebugDraw _debugDraw;
        internal int _count;
    }



    public class PolyShapes : Test
    {
        const int k_maxBodies = 256;

        int _bodyIndex;
        Body[] _bodies = new Body[k_maxBodies];
        PolygonShape[] _polygons = new PolygonShape[4];
        CircleShape _circle = new CircleShape();


        public PolyShapes()
	    {
            for (int i = 0; i < 4; i++)
            {
                _polygons[i] = new PolygonShape();
            }

		    // Ground body
		    {
			    BodyDef bd = new BodyDef();
			    Body ground = _world.CreateBody(bd);

			    PolygonShape shape = new PolygonShape();
			    shape.SetAsEdge(new Vector2(-40.0f, 0.0f), new Vector2(40.0f, 0.0f));
			    ground.CreateFixture(shape, 0.0f);
		    }

		    {
			    Vector2[] vertices = new Vector2[3];
			    vertices[0] = new Vector2(-0.5f, 0.0f);
			    vertices[1] = new Vector2(0.5f, 0.0f);
			    vertices[2] = new Vector2(0.0f, 1.5f);
			    _polygons[0].Set(vertices, 3);
		    }
    		
		    {
			    Vector2[] vertices3 = new Vector2[3];
			    vertices3[0] = new Vector2(-0.1f, 0.0f);
			    vertices3[1] = new Vector2(0.1f, 0.0f);
			    vertices3[2] = new Vector2(0.0f, 1.5f);
			    _polygons[1].Set(vertices3, 3);
		    }

		    {
			    double w = 1.0f;
			    double b = w / (2.0f + (double)Math.Sqrt(2.0));
			    double s = (double)Math.Sqrt(2.0) * b;

                Vector2[] vertices8 = new Vector2[8];
                vertices8[0] = new Vector2(0.5 * s, 0.0);
                vertices8[1] = new Vector2(0.5 * w, b);
                vertices8[2] = new Vector2(0.5 * w, b + s);
                vertices8[3] = new Vector2(0.5 * s, w);
                vertices8[4] = new Vector2(-0.5 * s, w);
                vertices8[5] = new Vector2(-0.5 * w, b + s);
                vertices8[6] = new Vector2(-0.5 * w, b);
                vertices8[7] = new Vector2(-0.5 * s, 0.0);

                _polygons[2].Set(vertices8, 8);
		    }

		    {
			    _polygons[3].SetAsBox(0.5f, 0.5f);
		    }

		    {
			    _circle._radius = 0.5f;
		    }

		    _bodyIndex = 0;
	    }


	    void Create(int index)
	    {
		    if (_bodies[_bodyIndex] != null)
		    {
			    _world.DestroyBody(_bodies[_bodyIndex]);
			    _bodies[_bodyIndex] = null;
		    }

		    BodyDef bd = new BodyDef();
            bd.type = BodyType.Dynamic;

		    double x = Rand.RandomFloat(-2.0f, 2.0f);
		    bd.position = new Vector2(x, 10.0f);
            bd.angle = Rand.RandomFloat(-Alt.Box2D.Settings.b2_pi, Alt.Box2D.Settings.b2_pi);

		    if (index == 4)
		    {
			    bd.angularDamping = 0.02f;
		    }

		    _bodies[_bodyIndex] = _world.CreateBody(bd);

		    if (index < 4)
		    {
			    FixtureDef fd = new FixtureDef();
			    fd.shape = _polygons[index];
			    fd.density = 1.0f;
			    fd.friction = 0.3f;
			    _bodies[_bodyIndex].CreateFixture(fd);
		    }
		    else
		    {
			    FixtureDef fd = new FixtureDef();
			    fd.shape = _circle;
			    fd.density = 1.0f;
			    fd.friction = 0.3f;

			    _bodies[_bodyIndex].CreateFixture(fd);
		    }

		    _bodyIndex = (_bodyIndex + 1) % k_maxBodies;
	    }


	    void DestroyBody()
	    {
		    for (int i = 0; i < k_maxBodies; ++i)
		    {
			    if (_bodies[i] != null)
			    {
				    _world.DestroyBody(_bodies[i]);
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
                for (int i = 0; i < k_maxBodies; i += 2)
                {
                    if (_bodies[i] != null)
                    {
                        bool active = _bodies[i].IsActive();
                        _bodies[i].SetActive(!active);
                    }
                }
            }
            if (e.KeyCode == Keys.D)
            {
                DestroyBody();
            }
	    }


	    public override void Step(Framework.Settings settings)
	    {
		    base.Step(settings);
            
            PolyShapesCallback callback = new PolyShapesCallback();
		    callback._circle._radius = 2.0f;
		    callback._circle._p = new Vector2(0.0f, 2.1f);
		    callback._transform.SetIdentity();
		    callback._debugDraw = _debugDraw;
            
		    AABB aabb;
		    callback._circle.ComputeAABB(out aabb, ref callback._transform, 0);

		    _world.QueryAABB(callback.ReportFixture, ref aabb);

		    Color color = new ColorR(0.4, 0.7, 0.8);
		    _debugDraw.DrawCircle(callback._circle._p, callback._circle._radius, color);

            _debugDraw.DrawString(50, _textLine, "Press 1-5 to drop stuff");
            _textLine += 15;
            _debugDraw.DrawString(50, _textLine, "Press a to (de)activate some bodies");
            _textLine += 15;
            _debugDraw.DrawString(50, _textLine, "Press d to destroy a body");
            _textLine += 15;
	    }


	    internal static Test Create()
	    {
		    return new PolyShapes();
	    }
    }
}
