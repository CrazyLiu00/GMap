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
    public class VaryingRestitution : Test
    {
        VaryingRestitution()
	    {
		    {
			    BodyDef bd = new BodyDef();
			    Body ground = _world.CreateBody(bd);

			    PolygonShape shape = new PolygonShape();
			    shape.SetAsEdge(new Vector2(-40.0, 0.0), new Vector2(40.0, 0.0));
			    ground.CreateFixture(shape, 0.0f);
		    }

		    {
                CircleShape shape = new CircleShape();
			    shape._radius = 1.0f;

			    FixtureDef fd = new FixtureDef();
			    fd.shape = shape;
			    fd.density = 1.0f;

			    double[] restitution = new double[7] {0.0f, 0.1f, 0.3f, 0.5f, 0.75f, 0.9f, 1.0f};

			    for (int i = 0; i < 7; ++i)
			    {
				    BodyDef bd = new BodyDef();
                    bd.type = BodyType.Dynamic;
				    bd.position = new Vector2(-10.0f + 3.0f * i, 20.0f);

				    Body body = _world.CreateBody(bd);

				    fd.restitution = restitution[i];
				    body.CreateFixture(fd);
			    }
		    }
	    }


        internal static Test Create()
	    {
		    return new VaryingRestitution();
	    }
    }
}
