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
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class DistanceTest : FarseerPhysicsTest
    {
        double _angleB;
        PolygonShape _polygonA;
        PolygonShape _polygonB;
        Vector2 _positionB = Vector2.Zero;
		Alt.FarseerPhysics.Common.Transform _transformA;
		Alt.FarseerPhysics.Common.Transform _transformB;

        
        DistanceTest()
        {
            {
                _transformA.SetIdentity();
                _transformA.p = new Vector2(0.0f, -0.2f);
                Vertices vertices = PolygonTools.CreateRectangle(10.0f, 0.2f);
                _polygonA = new PolygonShape(vertices, 0);
            }

            {
                _positionB = new Vector2(12.017401f, 0.13678508f);
                _angleB = -0.0109265f;
                _transformB.Set(_positionB, _angleB);

                Vertices vertices = PolygonTools.CreateRectangle(2.0f, 0.1f);
                _polygonB = new PolygonShape(vertices, 0);
            }
        }


        public override void Update(FarseerPhysicsGameSettings settings)
        {
            base.Update(settings);

            DistanceInput input = new DistanceInput();
            input.ProxyA.Set(_polygonA, 0);
            input.ProxyB.Set(_polygonB, 0);
            input.TransformA = _transformA;
            input.TransformB = _transformB;
            input.UseRadii = true;
            SimplexCache cache;
            cache.Count = 0;
            DistanceOutput output;
            Distance.ComputeDistance(out output, out cache, input);

            DrawString("Distance = " + output.Distance);
            DrawString("Iterations = " + output.Iterations);
            
            
            {
                Color color = new ColorR(0.9f, 0.9f, 0.9f);
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

            Vector2 x1 = output.PointA;
            Vector2 x2 = output.PointB;

            DebugView.DrawPoint(x1, 0.5f, new ColorR(1.0f, 0.0f, 0.0f));
            DebugView.DrawPoint(x2, 0.5f, new ColorR(1.0f, 0.0f, 0.0f));

            DebugView.DrawSegment(x1, x2, new ColorR(1.0f, 1.0f, 0.0f));
            
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
                _positionB.X -= 0.1;
            if (e.KeyCode == Keys.D)
                _positionB.X += 0.1;
            if (e.KeyCode == Keys.S)
                _positionB.Y -= 0.1;
            if (e.KeyCode == Keys.W)
                _positionB.Y += 0.1f;
            if (e.KeyCode == Keys.Q)
				_angleB += 0.1f * Alt.FarseerPhysics.Settings.Pi;
            if (e.KeyCode == Keys.E)
				_angleB -= 0.1f * Alt.FarseerPhysics.Settings.Pi;

            _transformB.Set(_positionB, _angleB);
        }


        internal static FarseerPhysicsTest Create()
        {
            return new DistanceTest();
        }
    }
}
