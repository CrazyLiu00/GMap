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
    public class LineJoint : Test
    {
        public LineJoint()
	    {
		    Body ground = null;
		    {
			    PolygonShape shape = new PolygonShape();
			    shape.SetAsEdge(new Vector2(-40.0, 0.0), new Vector2(40.0, 0.0));

			    BodyDef bd = new BodyDef();
			    ground = _world.CreateBody(bd);
			    ground.CreateFixture(shape, 0.0);
		    }

		    {
			    PolygonShape shape = new PolygonShape();
			    shape.SetAsBox(0.5, 2.0);

			    BodyDef bd = new BodyDef();
                bd.type = BodyType.Dynamic;
			    bd.position = new Vector2(0.0, 7.0);
			    Body body = _world.CreateBody(bd);
			    body.CreateFixture(shape, 1.0);

			    LineJointDef jd = new LineJointDef();
			    Vector2 axis = new Vector2(2.0, 1.0);
			    axis.Normalize();
			    jd.Initialize(ground, body, new Vector2(0.0, 8.5), axis);
			    jd.motorSpeed = 0.0;
			    jd.maxMotorForce = 100.0;
			    jd.enableMotor = true;
			    jd.lowerTranslation = -4.0;
			    jd.upperTranslation = 4.0;
			    jd.enableLimit = true;
			    _world.CreateJoint(jd);
		    }
	    }


        internal static Test Create()
	    {
		    return new LineJoint();
	    }
    }
}
