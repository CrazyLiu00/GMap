/*
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

using System;

using Alt.FarseerPhysics;
using Alt.FarseerPhysics.Collision;
using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Dynamics.Contacts;
using Alt.FarseerPhysics.Dynamics.Joints;

using Alt.IO;
using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Framework
{
    public class FarseerPhysicsTest
    {
        public double m_OffsetX;
        public double m_OffsetY;
        protected Vector2 ViewCenter
        {
            set
            {
                m_OffsetX = value.X;
                m_OffsetY = value.Y;
            }
        }

        public double m_CenterX;
        public double m_CenterY;
        public double m_Scale;


        internal AltSketchDebugViewExt DebugView;
        internal int StepCount;
        internal World World;
        FixedMouseJoint _fixedMouseJoint;
        internal int TextLine;


        protected FarseerPhysicsTest()
        {
            World = new World(new Vector2(0.0f, -10.0f));

            TextLine = 30;

            World.JointRemoved += JointRemoved;
            World.ContactManager.PreSolve += PreSolve;
            World.ContactManager.PostSolve += PostSolve;

            StepCount = 0;
        }


        public virtual void Initialize()
        {
            DebugView = new AltSketchDebugViewExt(World);
        }


        protected virtual void JointRemoved(Joint joint)
        {
            if (_fixedMouseJoint == joint)
            {
                _fixedMouseJoint = null;
            }
        }


        public void DrawTitle(int x, int y, string title)
        {
            DebugView.DrawString(x, y, title);
        }


        IntervalTimer m_UpdateIntervalTimer = new IntervalTimer();
        protected Int64 ElapsedTimeMSec
        {
            get
            {
                return m_UpdateIntervalTimer.ElapsedTimeMSec;
            }
        }

        public virtual void Update(FarseerPhysicsGameSettings settings)
        {
            double timeStep = Math.Min(ElapsedTimeMSec * 0.001, (1.0 / 30));
            m_UpdateIntervalTimer.Reset();

            if (settings.Pause)
            {
                if (settings.SingleStep)
                {
                    settings.SingleStep = false;
                }
                else
                {
                    timeStep = 0.0;
                }

                DrawString("****PAUSED****");
            }
            else
            {
                World.Step(timeStep);
            }

            if (timeStep > 0.0)
            {
                ++StepCount;
            }
        }


        protected virtual void OnKeyDown(KeyEventArgs e)
        {
        }
        public void InjectKeyDown(KeyEventArgs e)
        {
            OnKeyDown(e);
        }


        protected virtual void OnKeyUp(KeyEventArgs e)
        {
        }
        public void InjectKeyUp(KeyEventArgs e)
        {
            OnKeyUp(e);
        }


        public virtual void MouseDown(MouseEventArgs e, Vector2 p)
        {
            if (_fixedMouseJoint != null)
            {
                return;
            }

            Fixture fixture = World.TestPoint(p);

            if (fixture != null)
            {
                Body body = fixture.Body;
                _fixedMouseJoint = new FixedMouseJoint(body, p);
                _fixedMouseJoint.MaxForce = 1000.0f * body.Mass;
                World.AddJoint(_fixedMouseJoint);
                body.Awake = true;
            }
        }


        public virtual void MouseUp(MouseEventArgs e, Vector2 p)
        {
            if (_fixedMouseJoint != null)
            {
                World.RemoveJoint(_fixedMouseJoint);
                _fixedMouseJoint = null;
            }
        }


        public virtual void MouseMove(MouseEventArgs e, Vector2 p, Vector2 offset)
        {
            if (_fixedMouseJoint != null)
            {
                _fixedMouseJoint.WorldAnchorB = p;
            }
        }

        
        protected virtual void PreSolve(Contact contact, ref Manifold oldManifold)
        {
        }

        protected virtual void PostSolve(Contact contact, ContactVelocityConstraint impulse)
        {
        }


        protected void DrawString(string text)
        {
            DebugView.DrawString(50, TextLine, text);
            TextLine += 15;
        }


        public Vector2 ConvertScreenToWorld(double x, double y)
        {
            Matrix4 matrix = Matrix4.CreateTranslation(m_CenterX, m_CenterY);
            matrix.Scale(m_Scale, -m_Scale);
            matrix.Invert();

            matrix.Transform(ref x, ref y);

            return new Vector2(x, y);
        }


        public Vector2 ConvertWorldToScreen(Vector2 v)
        {
            Matrix4 matrix = Matrix4.CreateTranslation(m_CenterX, m_CenterY);
            matrix.Scale(m_Scale, -m_Scale);

            return matrix.Transform(v);
        }
    }
}
