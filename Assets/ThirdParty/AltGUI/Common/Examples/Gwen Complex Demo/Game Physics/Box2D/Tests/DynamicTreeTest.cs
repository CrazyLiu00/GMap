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
    public class DynamicTreeTest : Test
    {
        double _worldExtent;
        double _proxyExtent;

        DynamicTree _tree = new DynamicTree();
        AABB _queryAABB = new AABB();
        RayCastInput _rayCastInput = new RayCastInput();
        //RayCastOutput _rayCastOutput = new RayCastOutput();
        Actor _rayActor = new Actor();
        Actor[] _actors = new Actor[e_actorCount];
        bool _automated;
        
        static int e_actorCount = 128;


	    public DynamicTreeTest()
	    {
		    _worldExtent = 15.0f;
		    _proxyExtent = 0.5f;

		    for (int i = 0; i < e_actorCount; ++i)
		    {
                _actors[i] = new Actor();

			    Actor actor = _actors[i];
			    GetRandomAABB(out actor.aabb);
			    actor.proxyId = _tree.CreateProxy(ref actor.aabb, actor);
		    }

		    _stepCount = 0;

		    double h = _worldExtent;
		    _queryAABB.lowerBound = new Vector2(-3.0f, -4.0f + h);
		    _queryAABB.upperBound = new Vector2(5.0f, 6.0f + h);

		    _rayCastInput.p1 = new Vector2(-5.0f, 5.0f + h);
		    _rayCastInput.p2 = new Vector2(7.0f, -4.0f + h);
		    //_rayCastInput.p1 = new Vector2(0.0f, 2.0f + h);
		    //_rayCastInput.p2 = new Vector2(0.0f, -2.0f + h);
		    _rayCastInput.maxFraction = 1.0;

		    _automated = false;
	    }

	    internal static Test Create()
	    {
		    return new DynamicTreeTest();
	    }

	    public override void Step(Framework.Settings settings)
	    {
		    _rayActor = null;
		    for (int i = 0; i < e_actorCount; ++i)
		    {
			    _actors[i].fraction = 1.0f;
			    _actors[i].overlap = false;
		    }

		    if (_automated == true)
		    {
			    int actionCount = Math.Max(1, e_actorCount >> 2);

			    for (int i = 0; i < actionCount; ++i)
			    {
				    Action();
			    }
		    }

		    Query();
		    RayCast();

		    for (int i = 0; i < e_actorCount; ++i)
		    {
			    Actor actor = _actors[i];
			    if (actor.proxyId == -1)
				    continue;

			    Color ca = new ColorR(0.9, 0.9, 0.9);
			    if (actor == _rayActor && actor.overlap)
			    {
				    ca = new ColorR(0.9, 0.6, 0.6);
			    }
			    else if (actor == _rayActor)
			    {
				    ca = new ColorR(0.6, 0.9, 0.6);
			    }
			    else if (actor.overlap)
			    {
				    ca = new ColorR(0.6, 0.6, 0.9);
			    }

			    _debugDraw.DrawAABB(ref actor.aabb, ca);
		    }

		    Color c = new ColorR(0.7, 0.7, 0.7);
		    _debugDraw.DrawAABB(ref _queryAABB, c);

		    _debugDraw.DrawSegment(_rayCastInput.p1, _rayCastInput.p2, c);

		    Color c1 = new ColorR(0.2, 0.9, 0.2);
		    Color c2 = new ColorR(0.9, 0.2, 0.2);
		    _debugDraw.DrawPoint(_rayCastInput.p1, 6.0, c1);
		    _debugDraw.DrawPoint(_rayCastInput.p2, 6.0, c2);

		    if (_rayActor != null)
		    {
			    Color cr = new ColorR(0.2, 0.2, 0.9);
			    Vector2 p = _rayCastInput.p1 + _rayActor.fraction * (_rayCastInput.p2 - _rayCastInput.p1);
			    _debugDraw.DrawPoint(p, 6.0, cr);
		    }

            {
                int height = _tree.ComputeHeight();
                _debugDraw.DrawString(5, _textLine, "dynamic tree Height = %d", height);
                _textLine += 15;
            }

		    ++_stepCount;
	    }


	    protected override void OnKeyDown(KeyEventArgs e)
	    {
            if (e.KeyCode == Keys.A)
            {
                _automated = !_automated;
            }
            if (e.KeyCode == Keys.C)
            {
                CreateProxy();
            }
            if (e.KeyCode == Keys.D)
            {
                DestroyProxy();
            }
            if (e.KeyCode == Keys.M)
            {
                MoveProxy();
            }
	    }


	    bool QueryCallback(int proxyid)
	    {
            Actor actor = (Actor)_tree.GetUserData(proxyid);
		    actor.overlap = AABB.TestOverlap(ref _queryAABB, ref actor.aabb);
            return true;
	    }

	    double RayCastCallback(ref RayCastInput input, int proxyid)
	    {
            Actor actor = (Actor)_tree.GetUserData(proxyid);

            RayCastOutput output;
            bool hit = actor.aabb.RayCast(out output, ref input);

            if (hit)
		    {
                //_rayCastOutput = output;
                actor.fraction = output.fraction;
			    _rayActor = actor;

                return output.fraction;
		    }

            return input.maxFraction;
	    }

	    private class Actor
	    {
		    internal AABB aabb;
            internal double fraction;
            internal bool overlap;
            internal int proxyId;
	    };

	    private void GetRandomAABB(out AABB aabb)
	    {
            aabb = new AABB();

		    Vector2 w = new Vector2(2.0f * _proxyExtent, 2.0f * _proxyExtent);
		    //aabb.lowerBound.x = -_proxyExtent;
		    //aabb.lowerBound.y = -_proxyExtent + _worldExtent;
		    aabb.lowerBound.X = Rand.RandomFloat(-_worldExtent, _worldExtent);
		    aabb.lowerBound.Y = Rand.RandomFloat(0.0f, 2.0f * _worldExtent);
		    aabb.upperBound = aabb.lowerBound + w;
	    }

	    private void MoveAABB(ref AABB aabb)
	    {
            Vector2 d = Vector2.Zero;
		    d.X = Rand.RandomFloat(-0.5f, 0.5f);
		    d.Y = Rand.RandomFloat(-0.5f, 0.5f);
		    //d.x = 2.0f;
		    //d.y = 0.0f;
		    aabb.lowerBound += d;
		    aabb.upperBound += d;

		    Vector2 c0 = 0.5f * (aabb.lowerBound + aabb.upperBound);
		    Vector2 min; min = new Vector2(-_worldExtent, 0.0f);
		    Vector2 max; max = new Vector2(_worldExtent, 2.0f * _worldExtent);
		    Vector2 c = Vector2.Clamp(c0, min, max);

		    aabb.lowerBound += c - c0;
		    aabb.upperBound += c - c0;
	    }

	    private void CreateProxy()
	    {
		    for (int i = 0; i < e_actorCount; ++i)
		    {
                int j = Rand.rand.Next() % e_actorCount;
			    Actor actor = _actors[j];
			    if (actor.proxyId == -1)
			    {
				    GetRandomAABB(out actor.aabb);
				    actor.proxyId = _tree.CreateProxy(ref actor.aabb, actor);
				    return;
			    }
		    }
	    }

	    private void DestroyProxy()
	    {
		    for (int i = 0; i < e_actorCount; ++i)
		    {
                int j = Rand.rand.Next() % e_actorCount;
			    Actor actor = _actors[j];
			    if (actor.proxyId != -1)
			    {
				    _tree.DestroyProxy(actor.proxyId);
				    actor.proxyId = -1;
				    return;
			    }
		    }
	    }

	    private void MoveProxy()
	    {
		    for (int i = 0; i < e_actorCount; ++i)
		    {
                int j = Rand.rand.Next() % e_actorCount;
			    Actor actor = _actors[j];
			    if (actor.proxyId == -1)
			    {
				    continue;
			    }

                AABB aabb0 = actor.aabb;
                MoveAABB(ref actor.aabb);
                Vector2 displacement = actor.aabb.GetCenter() - aabb0.GetCenter();
                _tree.MoveProxy(actor.proxyId, ref actor.aabb, displacement);
                return;

		    }
	    }

	    private void Action()
	    {
		    int choice = Rand.rand.Next() % 20;

		    switch (choice)
		    {
		    case 0:
			    CreateProxy();
			    break;

		    case 1:
			    DestroyProxy();
			    break;

		    default:
			    MoveProxy();
                break;
		    }
	    }

	    
        void Query()
	    {
		    _tree.Query(QueryCallback, ref _queryAABB);

		    for (int i = 0; i < e_actorCount; ++i)
		    {
			    if (_actors[i].proxyId == -1)
			    {
				    continue;
			    }

			    bool overlap = AABB.TestOverlap(ref _queryAABB, ref _actors[i].aabb);
			    Debug.Assert(overlap == _actors[i].overlap);
		    }
	    }

	    
        void RayCast()
	    {
		    _rayActor = null;

		    RayCastInput input = _rayCastInput;

		    // Ray cast against the dynamic tree.
            _tree.RayCast(RayCastCallback, ref input);

		    // Brute force ray cast.
		    //Actor bruteActor = null;
		    //RayCastOutput bruteOutput;
		    for (int i = 0; i < e_actorCount; ++i)
		    {
			    if (_actors[i].proxyId == -1)
			    {
				    continue;
			    }

			    RayCastOutput output;
			    bool hit = _actors[i].aabb.RayCast(out output, ref input);
			    if (hit)
			    {
				    //bruteActor = _actors[i];
				    //bruteOutput = output;
				    input.maxFraction = output.fraction;
			    }
		    }
	    }
    }
}
