/*
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

using Alt.FarseerPhysics.Collision.Shapes;
using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class ShapeEditingTest : FarseerPhysicsTest
    {
        Body _body;
        Fixture _fixture2;

        
        ShapeEditingTest()
        {
            //Ground
            BodyFactory.CreateEdge(World, new Vector2(-40.0f, 0.0f), new Vector2(40.0f, 0.0f));

            _body = BodyFactory.CreateBody(World);
            _body.BodyType = BodyType.Dynamic;
            _body.Position = new Vector2(0.0f, 10.0f);

            Vertices box = PolygonTools.CreateRectangle(4.0f, 4.0f);
            PolygonShape shape2 = new PolygonShape(box, 10);
            _body.CreateFixture(shape2);

            _fixture2 = null;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.C && _fixture2 == null)
            {
                CircleShape shape = new CircleShape(3.0f, 10);
                shape.Position = new Vector2(0.5f, -4.0f);
                _fixture2 = _body.CreateFixture(shape);
                _body.Awake = true;
            }

            if (e.KeyCode == Keys.D && _fixture2 != null)
            {
                _body.DestroyFixture(_fixture2);
                _fixture2 = null;
                _body.Awake = true;
            }

            base.OnKeyDown(e);
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            base.Update(settings);
            DrawString("Press: (c) create a shape, (d) destroy a shape.");
            
        }

        internal static FarseerPhysicsTest Create()
        {
            return new ShapeEditingTest();
        }
    }
}