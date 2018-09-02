//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System.Diagnostics;

using Alt.FarseerPhysics.Collision.Shapes;
using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Common.PhysicsLogic;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class ExplosionTest : FarseerPhysicsTest
    {
        const int ColumnCount = 5;
        const int RowCount = 16;
        Body[] _bodies = new Body[RowCount * ColumnCount];
        RealExplosion _realExplosion;
        int[] _indices = new int[RowCount * ColumnCount];
        Vector2 _mousePos;
        double _force;
        double _radius;

        
        ExplosionTest()
        {
            //Ground
            BodyFactory.CreateEdge(World, new Vector2(-40.0, 0.0), new Vector2(40.0, 0.0));

            double[] xs = new[] { -10.0, -5.0, 0.0, 5.0, 10.0 };

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
                    Body body = BodyFactory.CreateBody(World);
                    body.BodyType = BodyType.Dynamic;
                    body.Position = new Vector2(xs[j] + x, 0.752f + 1.54f * i);
                    body.UserData = _indices[n];
                    _bodies[n] = body;

                    Fixture fixture = body.CreateFixture(shape);
                    fixture.Friction = 0.3f;

                    //First column is unaffected by the explosion
                    if (j == 0)
                    {
                        body.PhysicsLogicFilter.IgnorePhysicsLogic(PhysicsLogicType.Explosion);
                    }
                }
            }

            _radius = 5;
            _force = 3;
            _realExplosion = new RealExplosion(World);
        }


        public override void MouseDown(MouseEventArgs e, Vector2 p)
        {
            _mousePos = p;

            base.MouseDown(e, p);
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.OemComma)
            {
                _realExplosion.Activate(_mousePos, _radius, _force);
            }
            if (e.KeyCode == Keys.A)
            {
                _radius = MathHelper.Clamp(_radius - 0.1f, 0, 20);
            }
            if (e.KeyCode == Keys.S)
            {
                _radius = MathHelper.Clamp(_radius + 0.1f, 0, 20);
            }
            if (e.KeyCode == Keys.D)
            {
                _force = MathHelper.Clamp(_force - 0.1f, 0, 20);
            }
            if (e.KeyCode == Keys.F)
            {
                _force = MathHelper.Clamp(_force + 0.1f, 0, 20);
            }

            base.OnKeyDown(e);
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            base.Update(settings);

            DrawString("Press: (,) to explode at mouse position.");
            
            DrawString("Press: (A) to decrease the explosion radius, (S) to increase it.");
            
            DrawString("Press: (D) to decrease the explosion power, (F) to increase it.");
            
            // Fighting against double decimals
            double powernumber = (double)((int)(_force * 10)) / 10;
            DrawString("Power: " + powernumber);

            Color color = new ColorR(0.4f, 0.7f, 0.8f);
            
            DebugView.DrawCircle(_mousePos, _radius, color);

            
        }

        internal static FarseerPhysicsTest Create()
        {
            return new ExplosionTest();
        }
    }
}