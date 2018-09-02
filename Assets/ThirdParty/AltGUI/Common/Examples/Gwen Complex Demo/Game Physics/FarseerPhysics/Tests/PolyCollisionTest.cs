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

using Alt.FarseerPhysics.Collision;
using Alt.FarseerPhysics.Collision.Shapes;
using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Dynamics.Contacts;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class PolyCollisionTest : FarseerPhysicsTest
    {
        double _angleB;
        PolygonShape _polygonA = new PolygonShape(1);
        PolygonShape _polygonB = new PolygonShape(1);
        Vector2 _positionB;

		Alt.FarseerPhysics.Common.Transform _transformA;
		Alt.FarseerPhysics.Common.Transform _transformB;

        
        PolyCollisionTest()
        {
            {
                _polygonA.Vertices = PolygonTools.CreateRectangle(0.2f, 0.4f);
                _transformA.Set(Vector2.Zero, 0.0f);
            }

            {
                _polygonB.Vertices = PolygonTools.CreateRectangle(0.5f, 0.5f);
                //_positionB = new Vector2(19.345284f, 1.5632932f);
                _positionB = new Vector2(0.345284f, 0.5632932f);

                _angleB = 1.9160721f;
                _transformB.Set(_positionB, _angleB);
            }
        }

        internal static FarseerPhysicsTest Create()
        {
            return new PolyCollisionTest();
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            Manifold manifold = new Manifold();
			Alt.FarseerPhysics.Collision.Collision.CollidePolygons(ref manifold, _polygonA, ref _transformA, _polygonB, ref _transformB);

            Vector2 normal;
            FixedArray2<Vector2> points;
            ContactSolver.WorldManifold.Initialize(ref manifold, ref _transformA, _polygonA.Radius, ref _transformB, _polygonB.Radius, out normal, out points);

            DrawString("Point count = " + manifold.PointCount);

            
            {
                Color color = new ColorR(0.9, 0.9, 0.9);
				Vector2[] v = new Vector2[Alt.FarseerPhysics.Settings.MaxPolygonVertices];
                for (int i = 0; i < _polygonA.Vertices.Count; ++i)
                {
                    v[i] = MathUtils.Mul(ref _transformA, _polygonA.Vertices[i]);
                }
                DebugView.DrawPolygon(v, _polygonA.Vertices.Count, color);

                for (int i = 0; i < _polygonB.Vertices.Count; ++i)
                {
                    v[i] = MathUtils.Mul(ref _transformB, _polygonB.Vertices[i]);
                }
                DebugView.DrawPolygon(v, _polygonB.Vertices.Count, color);
            }

            for (int i = 0; i < manifold.PointCount; ++i)
            {
                DebugView.DrawPoint(points[i], 0.1f, new ColorR(0.9f, 0.3f, 0.3f));
                DrawString(points[i].ToString());
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
				_angleB += 0.1f * Alt.FarseerPhysics.Settings.Pi;
            }
            if (e.KeyCode == Keys.E)
            {
				_angleB -= 0.1f * Alt.FarseerPhysics.Settings.Pi;
            }

            _transformB.Set(_positionB, _angleB);
        }
    }
}