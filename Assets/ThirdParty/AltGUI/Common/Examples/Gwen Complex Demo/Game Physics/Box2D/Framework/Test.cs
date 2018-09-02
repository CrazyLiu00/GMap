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

using Alt.Box2D;
using Alt.Sketch;


namespace Alt.GUI.Box2D.Demo.Testbed.Framework
{
    // This is called when a joint in the world is implicitly destroyed
    // because an attached body is destroyed. This gives us a chance to
    // nullify the mouse joint.
    public class DestructionListener : IDestructionListener
    {
        public void SayGoodbye(Fixture fixture)
        {
        }

        public void SayGoodbye(Joint joint)
        {
            if (test._mouseJoint == joint)
            {
                test._mouseJoint = null;
            }
            else
            {
                test.JointDestroyed(joint);
            }
        }

        public Test test;
    }



    public struct ContactPoint
    {
        public Fixture fixtureA;
        public Fixture fixtureB;
        public Vector2 normal;
        public Vector2 position;
        public PointState state;
    }



    public class Test : IContactListener
    {
        internal Body _groundBody;
        internal ContactPoint[] _points = new ContactPoint[k_maxContactPoints];
        internal int _pointCount;
        internal DestructionListener _destructionListener = new DestructionListener();
        
		AltSketchDebugDraw m_debugDraw;
		internal AltSketchDebugDraw _debugDraw
		{
			get
			{
				return m_debugDraw;
			}
			set
			{
				m_debugDraw = value;

				if (_world != null)
				{
					_world.DebugDraw = _debugDraw;
				}
			}
		}

        internal int _textLine;
        internal World _world;
        internal Body _bomb;
        internal MouseJoint _mouseJoint;
        internal Vector2 _bombSpawnPoint;
        internal bool _bombSpawning;
        internal Vector2 _mouseWorld;
        internal int _stepCount;

        public static int k_maxContactPoints = 2048;


        public Test() :
			this(new AltSketchDebugDraw())
        {
		}

		public Test(AltSketchDebugDraw debugDraw)
        {
			if (debugDraw == null)
			{
				debugDraw = new AltSketchDebugDraw();
			}
			_debugDraw = debugDraw;

            Vector2 gravity;
            gravity = new Vector2(0.0f, -10.0f);
            bool doSleep = true;
            _world = new World(gravity, doSleep);
            _textLine = 30;

            _destructionListener.test = this;
            _world.DestructionListener = _destructionListener;
            _world.ContactListener = this;
            _world.DebugDraw = _debugDraw;

            _bombSpawning = false;

            _stepCount = 0;

            BodyDef bodyDef = new BodyDef();
            _groundBody = _world.CreateBody(bodyDef);
        }


        public void SetTextLine(int line)
        {
            _textLine = line;
        }


        public void DrawTitle(int x, int y, string title)
        {
            _debugDraw.DrawString(x, y, title);
        }


        public virtual void Step(Settings settings)
        {
            UnityEngine.Debug.Log("step");

            double timeStep = settings.hz > 0.0f ? 1.0f / settings.hz : 0.0f;

            if (settings.pause > 0)
            {
                if (settings.singleStep > 0)
                {
                    settings.singleStep = 0;
                }
                else
                {
                    timeStep = 0.0f;
                }

                _debugDraw.DrawString(50, _textLine, "****PAUSED****");
                _textLine += 15;
            }

            uint flags = 0;
            flags += settings.drawShapes * (uint)DebugDrawFlags.Shape;
            flags += settings.drawJoints * (uint)DebugDrawFlags.Joint;
            flags += settings.drawAABBs * (uint)DebugDrawFlags.AABB;
            flags += settings.drawPairs * (uint)DebugDrawFlags.Pair;
            flags += settings.drawCOMs * (uint)DebugDrawFlags.CenterOfMass;
            _debugDraw.Flags = (DebugDrawFlags)flags;

            _world.WarmStarting = (settings.enableWarmStarting > 0);
            _world.ContinuousPhysics = (settings.enableContinuous > 0);

            _pointCount = 0;

            _world.Step(timeStep, settings.velocityIterations, settings.positionIterations);

            _world.DrawDebugData();

            if (timeStep > 0.0f)
            {
                ++_stepCount;
            }

            if (settings.drawStats > 0)
            {
                _debugDraw.DrawString(50, _textLine, "bodies/contacts/joints/proxies = {0:n}/{1:n}/{2:n}",
                    _world.BodyCount, _world.ContactCount, _world.JointCount, _world.ProxyCount);
                _textLine += 15;
            }

            if (_mouseJoint != null)
            {
                Vector2 p1 = _mouseJoint.GetAnchorB();
                Vector2 p2 = _mouseJoint.GetTarget();

                _debugDraw.DrawPoint(p1, 0.5f, new ColorR(0.0f, 1.0f, 0.0f));
                _debugDraw.DrawPoint(p1, 0.5f, new ColorR(0.0f, 1.0f, 0.0f));
                _debugDraw.DrawSegment(p1, p2, new ColorR(0.8f, 0.8f, 0.8f));
            }

            if (_bombSpawning)
            {
                _debugDraw.DrawPoint(_bombSpawnPoint, 0.5f, new ColorR(0.0f, 0.0f, 1.0f));
                _debugDraw.DrawSegment(_mouseWorld, _bombSpawnPoint, new ColorR(0.8f, 0.8f, 0.8f));
            }

            if (settings.drawContactPoints > 0)
            {
                //double k_impulseScale = 0.1f;
                const double k_axisScale = 0.3f;

                for (int i = 0; i < _pointCount; ++i)
                {
                    ContactPoint point = _points[i];

                    if (point.state == PointState.Add)
                    {
                        // Add
                        _debugDraw.DrawPoint(point.position, 1.5f, new ColorR(0.3f, 0.95f, 0.3f));
                    }
                    else if (point.state == PointState.Persist)
                    {
                        // Persist
                        _debugDraw.DrawPoint(point.position, 0.65f, new ColorR(0.3f, 0.3f, 0.95f));
                    }

                    if (settings.drawContactNormals == 1)
                    {
                        Vector2 p1 = point.position;
                        Vector2 p2 = p1 + k_axisScale * point.normal;
                        _debugDraw.DrawSegment(p1, p2, new ColorR(0.4f, 0.9f, 0.4f));
                    }
                    else if (settings.drawContactForces == 1)
                    {
                        //Vector2 p1 = point.position;
                        //Vector2 p2 = p1 + k_forceScale * point.normalForce * point.normal;
                        //DrawSegment(p1, p2, Color(0.9f, 0.9f, 0.3f));
                    }

                    if (settings.drawFrictionForces == 1)
                    {
                        //Vector2 tangent = b2Cross(point.normal, 1.0f);
                        //Vector2 p1 = point.position;
                        //Vector2 p2 = p1 + k_forceScale * point.tangentForce * tangent;
                        //DrawSegment(p1, p2, Color(0.9f, 0.9f, 0.3f));
                    }
                }
            }
        }


        protected virtual void OnKeyDown(KeyEventArgs e)
        {
        }
        public void InjectKeyDown(KeyEventArgs e)
        {
            OnKeyDown(e);
        }


        public void ShiftMouseDown(Vector2 p)
        {
            _mouseWorld = p;

            if (_mouseJoint != null)
            {
                return;
            }

            SpawnBomb(p);
        }


        public virtual void MouseDown(Vector2 p)
        {
            _mouseWorld = p;

            if (_mouseJoint != null)
            {
                return;
            }

            // Make a small box.
            AABB aabb;
            Vector2 d = new Vector2(0.001, 0.001);
            aabb.lowerBound = p - d;
            aabb.upperBound = p + d;

            Fixture _fixture = null;

            // Query the world for overlapping shapes.
            _world.QueryAABB(
                (fixtureProxy) =>
                {
                    var fixture = fixtureProxy.fixture;
                    Body body = fixture.GetBody();
                    if (body.GetType() == BodyType.Dynamic)
                    {
                        bool inside = fixture.TestPoint(p);
                        if (inside)
                        {
                            _fixture = fixture;

                            // We are done, terminate the query.
                            return false;
                        }
                    }

                    // Continue the query.
                    return true;
                }, ref aabb);

            if (_fixture != null)
            {
                Body body = _fixture.GetBody();
                MouseJointDef md = new MouseJointDef();
                md.bodyA = _groundBody;
                md.bodyB = body;
                md.target = p;
                md.maxForce = 1000.0f * body.GetMass();
                _mouseJoint = (MouseJoint)_world.CreateJoint(md);
                body.SetAwake(true);
            }
        }


        public virtual void MouseUp(Vector2 p)
        {
            if (_mouseJoint != null)
            {
                _world.DestroyJoint(_mouseJoint);
                _mouseJoint = null;
            }

            if (_bombSpawning)
            {
                CompleteBombSpawn(p);
            }
        }


        public virtual void MouseMove(Vector2 p)
        {
            _mouseWorld = p;

            if (_mouseJoint != null)
            {
                _mouseJoint.SetTarget(p);
            }
        }


        public void LaunchBomb()
        {
            Vector2 p = new Vector2(Rand.RandomFloat(-15.0f, 15.0f), 30.0f);
            Vector2 v = -5.0f * p;
            LaunchBomb(p, v);
        }


        public void LaunchBomb(Vector2 position, Vector2 velocity)
        {
            if (_bomb != null)
            {
                _world.DestroyBody(_bomb);
                _bomb = null;
            }

            BodyDef bd = new BodyDef();
            bd.type = BodyType.Dynamic;
            bd.position = position;

            bd.bullet = true;
            _bomb = _world.CreateBody(bd);
            _bomb.SetLinearVelocity(velocity);

            CircleShape circle = new CircleShape();
            circle._radius = 0.3f;

            FixtureDef fd = new FixtureDef();
            fd.shape = circle;
            fd.density = 20.0f;
            fd.restitution = 0.0f;

            Vector2 minV = position - new Vector2(0.3, 0.3);
            Vector2 maxV = position + new Vector2(0.3, 0.3);

            AABB aabb;
            aabb.lowerBound = minV;
            aabb.upperBound = maxV;

            _bomb.CreateFixture(fd);
        }


        public void SpawnBomb(Vector2 worldPt)
        {
            _bombSpawnPoint = worldPt;
            _bombSpawning = true;
        }


        public void CompleteBombSpawn(Vector2 p)
        {
            if (_bombSpawning == false)
            {
                return;
            }

            double multiplier = 30.0f;
            Vector2 vel = _bombSpawnPoint - p;
            vel *= multiplier;
            LaunchBomb(_bombSpawnPoint, vel);
            _bombSpawning = false;
        }


        // Let derived tests know that a joint was destroyed.
        public virtual void JointDestroyed(Joint joint)
        {
        }


        // Callbacks for derived classes.
        public virtual void BeginContact(Contact contact)
        {
        }


        public virtual void EndContact(Contact contact)
        {
        }


        public virtual void PreSolve(Contact contact, ref Manifold oldManifold)
        {
            Manifold manifold;
            contact.GetManifold(out manifold);

            if (manifold._pointCount == 0)
            {
                return;
            }

            Fixture fixtureA = contact.GetFixtureA();
            Fixture fixtureB = contact.GetFixtureB();

            FixedArray2<PointState> state1, state2;
            Collision.GetPointStates(out state1, out state2, ref oldManifold, ref manifold);

            WorldManifold worldManifold;
            contact.GetWorldManifold(out worldManifold);

            for (int i = 0; i < manifold._pointCount && _pointCount < k_maxContactPoints; ++i)
            {
                if (fixtureA == null)
                {
                    _points[i] = new ContactPoint();
                }
                ContactPoint cp = _points[_pointCount];
                cp.fixtureA = fixtureA;
                cp.fixtureB = fixtureB;
                cp.position = worldManifold._points[i];
                cp.normal = worldManifold._normal;
                cp.state = state2[i];
                _points[_pointCount] = cp;
                ++_pointCount;
            }
        }


        public virtual void PostSolve(Contact contact, ref ContactImpulse impulse)
        {
        }
    }
}
