//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using Alt.FarseerPhysics.Controllers;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    internal class SimpleWindForceTest : FarseerPhysicsTest
    {
        SimpleWindForce _simpleWind;

        double _strength;

        
        SimpleWindForceTest()
        {
            _simpleWind = new SimpleWindForce();
            _simpleWind.Direction = new Vector2(0.7f, 0.2f);
            _simpleWind.Variation = 1.0f;
            _simpleWind.Strength = 5;
            _simpleWind.Position = new Vector2(0, 20);
            _simpleWind.DecayStart = 5f;
            _simpleWind.DecayEnd = 10f;
            _simpleWind.DecayMode = AbstractForceController.DecayModes.Step;
            _simpleWind.ForceType = AbstractForceController.ForceTypes.Point;

            _strength = 1.0f;

            World.AddController(_simpleWind);
            World.Gravity = Vector2.Zero;

            const int countX = 10;
            const int countY = 10;

            for (int x = 0; x < countX; x++)
            {
                for (int y = 0; y < countY; y++)
                {
                    Body currentFixture = BodyFactory.CreateRectangle(World, 1f, 1f, 5f,
                                                                      new Vector2(x * 2 - countX, y * 2 + 5));
                    //Fixture currentFixture = BodyFactory.CreateCircle(World, 0.2f, 10f, new Vector2(x - countX, y  + 5));
                    currentFixture.BodyType = BodyType.Dynamic;
                    currentFixture.Friction = 0.5f;
                    currentFixture.SetTransform(currentFixture.Position, 0.6f);
                    //currentFixture.CollidesWith = Category.Cat10;
                }
            }

            Body floor = BodyFactory.CreateRectangle(World, 100, 1, 1, new Vector2(0, 0));
            Body ceiling = BodyFactory.CreateRectangle(World, 100, 1, 1, new Vector2(0, 40));
            Body right = BodyFactory.CreateRectangle(World, 1, 100, 1, new Vector2(35, 0));
            Body left = BodyFactory.CreateRectangle(World, 1, 100, 1, new Vector2(-35, 0));

            floor.Friction = 0.2f;
            ceiling.Friction = 0.2f;
            right.Friction = 0.2f;
            left.Friction = 0.2f;
        }

        public void DrawPointForce()
        {
            DebugView.DrawPoint(_simpleWind.Position, 2, Color.Red);
            DebugView.DrawCircle(_simpleWind.Position, _simpleWind.DecayStart, Color.Green);
            DebugView.DrawCircle(_simpleWind.Position, _simpleWind.DecayEnd, Color.Red);
        }

        public void DrawLineForce()
        {
            Vector2 drawVector;
            drawVector = _simpleWind.Direction;
            drawVector.Normalize();
            drawVector *= _strength;
            DebugView.DrawArrow(_simpleWind.Position, _simpleWind.Position + drawVector, 2, 1f, true, Color.Red);
        }

        public void DrawNoneForce()
        {
            DebugView.DrawPoint(_simpleWind.Position, 2, Color.Red);
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            DrawString("SimpleWindForce | Mouse: Direction | Left-Click: Position | W/S: Variation");
            DrawString("Wind Strength:" + _simpleWind.Strength);
            DrawString("Variation:" + _simpleWind.Variation);

            
            //DebugView.DrawSegment(SimpleWind.Position, SimpleWind.Direction-SimpleWind.Position, Color.Red);
            DrawPointForce();
            
            base.Update(settings);
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
                _strength += 1f;

            if (e.KeyCode == Keys.A)
                _strength -= 1f;

            if (e.KeyCode == Keys.W)
                _simpleWind.Variation += 0.1;

            if (e.KeyCode == Keys.S)
                _simpleWind.Variation -= 0.1;

            base.OnKeyDown(e);
        }


        public override void MouseDown(MouseEventArgs e, Vector2 p)
        {
            //base.Mouse(state, oldState);
            Vector2 mouseWorld = p;
            _simpleWind.Direction = mouseWorld - _simpleWind.Position;
            _simpleWind.Strength = _strength;

            if (e.Button == MouseButtons.Left)
            {
                _simpleWind.Position = mouseWorld;
                _simpleWind.Direction = mouseWorld + new Vector2(0, 1);
                
                //NeedToProcess Input.Mouse.SetPosition(state.X, state.Y + 10);
            }
        }


        internal static FarseerPhysicsTest Create()
        {
            return new SimpleWindForceTest();
        }
    }
}
