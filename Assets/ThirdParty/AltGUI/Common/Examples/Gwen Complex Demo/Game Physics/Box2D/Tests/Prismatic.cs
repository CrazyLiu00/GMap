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
    public class Prismatic : Test
    {
        PrismaticJoint _joint;

        
        public Prismatic()
	    {
		    Body ground = null;
		    {
			    BodyDef bd = new BodyDef();
			    ground = _world.CreateBody(bd);

			    PolygonShape shape = new PolygonShape();
			    shape.SetAsEdge(new Vector2(-40.0, 0.0), new Vector2(40.0, 0.0));
			    ground.CreateFixture(shape, 0.0);
		    }

		    {
			    PolygonShape shape = new PolygonShape();
			    shape.SetAsBox(2.0, 0.5);

			    BodyDef bd = new BodyDef();
                bd.type = BodyType.Dynamic;
			    bd.position = new Vector2(-10.0, 10.0);
                bd.angle = 0.5 * Alt.Box2D.Settings.b2_pi;
			    Body body = _world.CreateBody(bd);
			    body.CreateFixture(shape, 5.0);

                PrismaticJointDef pjd = new PrismaticJointDef();

			    // Bouncy limit
                Vector2 axis = new Vector2(2, 1);
			    axis.Normalize();
			    pjd.Initialize(ground, body, new Vector2(0.0, 0.0), axis);

			    // Non-bouncy limit
			    //pjd.Initialize(ground, body, new Vector2(-10.0, 10.0), new Vector2(1.0, 0.0));

			    pjd.motorSpeed = 10.0;
			    pjd.maxMotorForce = 1000.0;
			    pjd.enableMotor = true;
			    pjd.lowerTranslation = 0.0;
			    pjd.upperTranslation = 20.0;
			    pjd.enableLimit = true;

			    _joint = (PrismaticJoint)_world.CreateJoint(pjd);
		    }
	    }


	    protected override void OnKeyDown(KeyEventArgs e)
	    {
            if (e.KeyCode == Keys.L)
            {
                _joint.EnableLimit(!_joint.IsLimitEnabled());
            }
            if (e.KeyCode == Keys.M)
            {
                _joint.EnableMotor(!_joint.IsMotorEnabled());
            }
            if (e.KeyCode == Keys.P)
            {
                _joint.SetMotorSpeed(-_joint.GetMotorSpeed());
            }
	    }


	    public override void Step(Framework.Settings settings)
	    {
		    base.Step(settings);
		    _debugDraw.DrawString(50, _textLine, "Keys: (l) limits, (m) motors, (p) speed");
		    _textLine += 15;
		    double force = _joint.GetMotorForce();
            _debugDraw.DrawString(50, _textLine, "Motor Force = {0:n}", (double)force);
		    _textLine += 15;
	    }


	    internal static Test Create()
	    {
		    return new Prismatic();
	    }
    }
}
