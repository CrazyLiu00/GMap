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
    public class Pyramid : Test
    {
		const int e_count = 20;


	    public Pyramid()
	    {
		    {
			    BodyDef bd = new BodyDef();
			    Body ground = _world.CreateBody(bd);

			    PolygonShape shape = new PolygonShape();
			    shape.SetAsEdge(new Vector2(-40.0, 0.0), new Vector2(40.0, 0.0));
			    ground.CreateFixture(shape, 0.0);
		    }

		    {
			    double a = 0.5;
			    PolygonShape shape = new PolygonShape();
			    shape.SetAsBox(a, a);

			    Vector2 x = new Vector2(-7.0, 0.75);
			    Vector2 y;
			    Vector2 deltaX = new Vector2(0.5625, 1.25);
                Vector2 deltaY = new Vector2(1.125, 0.0);

			    for (int i = 0; i < e_count; ++i)
			    {
				    y = x;

				    for (int j = i; j < e_count; ++j)
				    {
					    BodyDef bd = new BodyDef();
                        bd.type = BodyType.Dynamic;
					    bd.position = y;
					    Body body = _world.CreateBody(bd);
					    body.CreateFixture(shape, 5.0);

					    y += deltaY;
				    }

				    x += deltaX;
			    }
		    }
	    }


	    //void Step(Framework.Settings settings)
	    //{
	    //	// We need higher accuracy for the pyramid.
	    //	int velocityIterations = settings.velocityIterations;
	    //	int positionIterations = settings.positionIterations;
	    //	settings.velocityIterations = b2Max(8, velocityIterations);
	    //	settings.positionIterations = b2Max(1, positionIterations);
	    //	base.Step(settings);
	    //	settings.velocityIterations = velocityIterations;
	    //	settings.positionIterations = positionIterations;
	    //}


	    public static Test Create()
	    {
		    return new Pyramid();
	    }
    }
}
