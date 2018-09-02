//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.


using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Dynamics.Contacts;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class DeletionTest : FarseerPhysicsTest
    {
        DeletionTest()
        {
            //Ground body
            Body ground = BodyFactory.CreateEdge(World, new Vector2(-40.0f, 0.0f), new Vector2(40.0f, 0.0f));
            ground.OnCollision += OnCollision;
            ground.OnSeparation += OnSeparation;
        }

        private bool OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            return true;
        }

        private void OnSeparation(Fixture fixtureA, Fixture fixtureB)
        {
            fixtureB.Body.Dispose();
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            Body body = BodyFactory.CreateCircle(World, 0.4f, 1);
            body.Position = new Vector2(Rand.RandomFloat(-35, 35), 10);
            body.BodyType = BodyType.Dynamic;
            body.Restitution = 1f;

            base.Update(settings);
        }

        public static FarseerPhysicsTest Create()
        {
            return new DeletionTest();
        }
    }
}