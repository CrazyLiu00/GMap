﻿/*
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

using Alt.Box2D;
using Alt.GUI.Box2D.Demo.Testbed.Framework;
using Alt.Sketch;


namespace Alt.GUI.Box2D.Demo.Testbed.Tests
{
    public class PolyCollision : Test
    {
        PolygonShape _polygonA = new PolygonShape();
        PolygonShape _polygonB = new PolygonShape();

        Alt.Box2D.Transform _transformA = new Alt.Box2D.Transform();
        Alt.Box2D.Transform _transformB = new Alt.Box2D.Transform();

        Vector2 _positionB;
        double _angleB;

        
        public PolyCollision()
	    {
            {
                _polygonA.SetAsEdge(new Vector2(20.0, 0.0), new Vector2(20.0, 20.0));
                _transformA.Set(new Vector2(0.0, 0.0), 0.0);
            }

            {
                _polygonB.SetAsBox(0.25f, 0.25f);
                _positionB = new Vector2(19.345284f, 1.5632932f);
                _angleB = 1.9160721f;
                _transformB.Set(_positionB, _angleB);
            }

	    }

	    internal static Test Create()
	    {
		    return new PolyCollision();
	    }

	    public override void Step(Framework.Settings settings)
	    {
            Manifold manifold = new Manifold();
		    Collision.CollidePolygons(ref manifold, _polygonA, ref _transformA, _polygonB, ref _transformB);

		    WorldManifold worldManifold = new WorldManifold(ref manifold, ref _transformA, _polygonA._radius, ref _transformB, _polygonB._radius);

            _debugDraw.DrawString(50, _textLine, "point count = {0:n}", manifold._pointCount);
		    _textLine += 15;

		    {
			    Color color = new ColorR(0.9, 0.9, 0.9);
                FixedArray8<Vector2> v = new FixedArray8<Vector2>();
			    for (int i = 0; i < _polygonA._vertexCount; ++i)
			    {
				    v[i] = MathUtils.Multiply(ref _transformA, _polygonA._vertices[i]);
			    }
			    _debugDraw.DrawPolygon(ref v, _polygonA._vertexCount, color);

			    for (int i = 0; i < _polygonB._vertexCount; ++i)
			    {
				    v[i] = MathUtils.Multiply(ref _transformB, _polygonB._vertices[i]);
			    }
			    _debugDraw.DrawPolygon(ref v, _polygonB._vertexCount, color);
		    }

		    for (int i = 0; i < manifold._pointCount; ++i)
		    {
			    _debugDraw.DrawPoint(worldManifold._points[i], 0.5f, new ColorR(0.9, 0.3, 0.3));
		    }
	    }


	    protected override void OnKeyDown(KeyEventArgs e)
	    {
            if (e.KeyCode == Keys.A)
            {
                _positionB.X -= 0.1f;
            }
            if (e.KeyCode == Keys.D)
            {
                _positionB.X += 0.1f;
            }
            if (e.KeyCode == Keys.S)
            {
                _positionB.Y -= 0.1f;
            }
            if (e.KeyCode == Keys.W)
            {
                _positionB.Y += 0.1f;
            }
            if (e.KeyCode == Keys.Q)
            {
                _angleB += 0.1 * Alt.Box2D.Settings.b2_pi;
            }
            if (e.KeyCode == Keys.E)
            {
                _angleB -= 0.1 * Alt.Box2D.Settings.b2_pi;
            }

		    _transformB.Set(_positionB, _angleB);
	    }
    }
}
