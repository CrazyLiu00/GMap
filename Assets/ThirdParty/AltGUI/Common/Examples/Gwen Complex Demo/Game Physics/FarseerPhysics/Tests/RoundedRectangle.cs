//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class RoundedRectangle : FarseerPhysicsTest
    {
        int _segments = 3;

        
        RoundedRectangle()
        {
            //Ground
            BodyFactory.CreateEdge(World, new Vector2(-40.0f, 0.0f), new Vector2(40.0f, 0.0f));

            //Arcs
            BodyFactory.CreateLineArc(World, MathHelper.Pi * 1.5f, 50, 2, false, new Vector2(-15, 10));
            BodyFactory.CreateLineArc(World, MathHelper.Pi, 50, 2, false, new Vector2(-20, 10));
            BodyFactory.CreateLineArc(World, MathHelper.Pi / 1.5f, 50, 2, false, new Vector2(-25, 10));
            BodyFactory.CreateLineArc(World, MathHelper.Pi / 2, 50, 2, false, new Vector2(-30, 10));

            BodyFactory.CreateLineArc(World, MathHelper.Pi * 1.5f, 50, 2, true, new Vector2(-15, 25));
            BodyFactory.CreateLineArc(World, MathHelper.Pi, 50, 2, true, new Vector2(-20, 25));
            BodyFactory.CreateLineArc(World, MathHelper.Pi / 1.5f, 50, 2, true, new Vector2(-25, 25));
            BodyFactory.CreateLineArc(World, MathHelper.Pi / 2, 50, 2, true, new Vector2(-30, 25));

            BodyFactory.CreateSolidArc(World, 1, MathHelper.Pi * 1.5f, 50, 2, new Vector2(-15, 40));
            BodyFactory.CreateSolidArc(World, 1, MathHelper.Pi, 50, 2, new Vector2(-20, 40));
            BodyFactory.CreateSolidArc(World, 1, MathHelper.Pi / 1.5f, 50, 2, new Vector2(-25, 40));
            BodyFactory.CreateSolidArc(World, 1, MathHelper.Pi / 2, 50, 2, new Vector2(-30, 40));

            Create(0);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
                _segments++;

            if (e.KeyCode == Keys.S && _segments > 0)
                _segments--;

            if (e.KeyCode == Keys.D)
                Create(0);

            if (e.KeyCode == Keys.F)
                Create(1);

            base.OnKeyDown(e);
        }

        private void Create(int type)
        {
            Vector2 position = new Vector2(0, 30);

            switch (type)
            {
                default:
                    Body rounded = BodyFactory.CreateRoundedRectangle(World, 10, 10, 2.5F, 2.5F, _segments, 10, position);
                    rounded.BodyType = BodyType.Dynamic;
                    break;
                case 1:
                    Body capsule = BodyFactory.CreateCapsule(World, 10, 2, (int)MathHelper.Max(_segments, 1), 3, (int)MathHelper.Max(_segments, 1), 10, position);
                    capsule.BodyType = BodyType.Dynamic;
                    break;
            }
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            base.Update(settings);
            DrawString("Segments: " + _segments + "\nPress: 'A' to increase segments, 'S' decrease segments\n'D' to create rectangle. 'F' to create capsule.");
        }

        internal static FarseerPhysicsTest Create()
        {
            return new RoundedRectangle();
        }
    }
}