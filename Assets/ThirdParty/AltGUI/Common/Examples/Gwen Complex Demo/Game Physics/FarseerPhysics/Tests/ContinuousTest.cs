﻿/*
* Farseer Physics Engine:
* Copyright (c) 2012 Ian Qvist
* 
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

using System.Collections.Generic;
using Alt.FarseerPhysics.Collision;
using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class ContinuousTest : FarseerPhysicsTest
    {
        double _angularVelocity;
        Body _box;
        Body _ground;

        
        ContinuousTest()
        {
            List<Vertices> list = new List<Vertices>();
            list.Add(PolygonTools.CreateLine(new Vector2(-10.0f, 0.0f), new Vector2(10.0f, 0.0f)));
            list.Add(PolygonTools.CreateRectangle(0.2f, 1.0f, new Vector2(0.5f, 1.0f), 0));

            _ground = BodyFactory.CreateCompoundPolygon(World, list, 0);

            _box = BodyFactory.CreateRectangle(World, 4, 0.2f, 1);
            _box.Position = new Vector2(0, 20);
            _box.BodyType = BodyType.Dynamic;
            //_box.Body.Rotation = 0.1f;

            //_angularVelocity = 46.661274f;
            _angularVelocity = Rand.RandomFloat(-50.0f, 50.0f);
            _box.LinearVelocity = new Vector2(0.0f, -100.0f);
            _box.AngularVelocity = _angularVelocity;

            Distance.GJKCalls = 0; Distance.GJKIters = 0; Distance.GJKMaxIters = 0;
            TimeOfImpact.TOICalls = 0; TimeOfImpact.TOIIters = 0;
            TimeOfImpact.TOIRootIters = 0; TimeOfImpact.TOIMaxRootIters = 0;
        }

        
        void Launch()
        {
            _box.SetTransform(new Vector2(0.0f, 20.0f), 0.0f);
            _angularVelocity = Rand.RandomFloat(-50.0f, 50.0f);
            _box.LinearVelocity = new Vector2(0.0f, -100.0f);
            _box.AngularVelocity = _angularVelocity;
        }

        
        public override void Update(FarseerPhysicsGameSettings settings)
        {
            base.Update(settings);

            DrawString("Press C to toggle CCD. CCD is Currently: " + (_ground.IgnoreCCD ? "Off" : "On"));

            if (Distance.GJKCalls > 0)
                DrawString(string.Format("GJK calls = {0:n}, Ave GJK iters = {1:n}, Max GJK iters = {2:n}", Distance.GJKCalls, Distance.GJKIters / (double)Distance.GJKCalls, Distance.GJKMaxIters));

            if (TimeOfImpact.TOICalls > 0)
            {
                DrawString(string.Format("TOI calls = {0:n}, Ave TOI iters = {1:n}, Max TOI iters = {2:n}", TimeOfImpact.TOICalls, TimeOfImpact.TOIIters / (double)TimeOfImpact.TOICalls, TimeOfImpact.TOIMaxRootIters));
                DrawString(string.Format("Ave TOI root iters = {0:n}, Max TOI root iters = {1:n}", TimeOfImpact.TOIRootIters / (double)TimeOfImpact.TOICalls, TimeOfImpact.TOIMaxRootIters));
            }

            if (StepCount % 60 == 0)
                Launch();
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.C)
            {
                _ground.IgnoreCCD = !_ground.IgnoreCCD;
            }
        }


        internal static FarseerPhysicsTest Create()
        {
            return new ContinuousTest();
        }
    }
}
