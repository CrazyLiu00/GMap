//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.


using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Dynamics.Contacts;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class LockTest : FarseerPhysicsTest
    {
        Body _rectangle;

        
        LockTest()
        {
            BodyFactory.CreateEdge(World, new Vector2(-20, 0), new Vector2(20, 0));

            _rectangle = BodyFactory.CreateRectangle(World, 2, 2, 1);
            _rectangle.BodyType = BodyType.Dynamic;
            _rectangle.Position = new Vector2(0, 10);
            _rectangle.OnCollision += OnCollision;

            //Properties and methods that were checking for lock before
            //Body.Enabled
            //Body.LocalCenter
            //Body.Mass
            //Body.Inertia
            //Fixture.DestroyFixture()
            //Body.SetTransformIgnoreContacts()
            //Fixture()
        }

        private bool OnCollision(Fixture fixturea, Fixture fixtureb, Contact manifold)
        {
            //_rectangle.CreateFixture(_rectangle.Shape); //Calls the constructor in Fixture
            //_rectangle.DestroyFixture(_rectangle);
            //_rectangle.Inertia = 40;
            //_rectangle.LocalCenter = new Vector2(-1, -1);
            //_rectangle.Mass = 10;
            _rectangle.Enabled = false;
            return false;
        }

        internal static FarseerPhysicsTest Create()
        {
            return new LockTest();
        }
    }
}