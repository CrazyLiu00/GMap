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

using Alt.Box2D;
using Alt.GUI.Box2D.Demo.Testbed.Framework;
using Alt.Sketch;


namespace Alt.GUI.Box2D.Demo.Testbed.Tests
{
    public class ShapeEditing : Test
    {
        Body _body;
        //Fixture _fixture1;
        Fixture _fixture2;

        
        public ShapeEditing()
	    {
		    {
			    BodyDef bd = new BodyDef();
			    Body ground = _world.CreateBody(bd);

			    PolygonShape shape = new PolygonShape();
			    shape.SetAsEdge(new Vector2(-40.0, 0.0), new Vector2(40.0, 0.0));
			    ground.CreateFixture(shape, 0.0);
		    }

            BodyDef bodydef = new BodyDef();
            bodydef.type = BodyType.Dynamic;
		    bodydef.position = new Vector2(0.0, 10.0);
		    _body = _world.CreateBody(bodydef);

		    PolygonShape shape2 = new PolygonShape();
		    shape2.SetAsBox(4.0f, 4.0f, new Vector2(0.0f, 0.0f), 0.0f);
		    //NoNeed	_fixture1 =
			_body.CreateFixture(shape2, 10.0);

		    _fixture2 = null;
	    }


	    protected override void OnKeyDown(KeyEventArgs e)
	    {
		    if (e.KeyCode == Keys.C
                && _fixture2 == null)
		    {
                CircleShape shape = new CircleShape();
			    shape._radius = 3.0f;
			    shape._p = new Vector2(0.5f, -4.0f);
			    _fixture2 = _body.CreateFixture(shape, 10.0f);
			    _body.SetAwake(true);
		    }

		    if (e.KeyCode == Keys.D
                && _fixture2 != null)
		    {
			    _body.DestroyFixture(_fixture2);
			    _fixture2 = null;
			    _body.SetAwake(true);
		    }
	    }


	    public override void Step(Framework.Settings settings)
	    {
		    base.Step(settings);
		    _debugDraw.DrawString(50, _textLine, "Press: (c) create a shape, (d) destroy a shape.");
		    _textLine += 15;
	    }


	    internal static Test Create()
	    {
		    return new ShapeEditing();
	    }
    }
}
