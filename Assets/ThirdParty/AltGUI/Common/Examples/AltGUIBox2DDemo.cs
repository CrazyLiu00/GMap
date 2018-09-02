//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;

using Alt.Box2D;
using Alt.Sketch;


[AddComponentMenu("AltGUI/Examples/Common/Box2D Demo")]
public abstract class AltGUIBox2DDemo : MonoBehaviour
{	
	Alt.GUI.Timer m_Timer;

	Alt.GUI.Box2D.Demo.Testbed.Framework.Test m_Test;
	Alt.GUI.Box2D.Demo.Testbed.Framework.Settings settings;


	protected abstract Alt.Box2D.AltSketchDebugDraw DebugDraw
	{
		get;
	}

	protected abstract double TPS
	{
		get;
	}
		
	protected abstract Alt.Sketch.Vector2 ConvertScreenToWorld(double x, double y);

	protected abstract void Invalidate();

	
	protected void Initialize ()
	{
		settings = new Alt.GUI.Box2D.Demo.Testbed.Framework.Settings();
		Box2D_onRestart();

		m_Timer = new Alt.GUI.Timer(10);
		m_Timer.Tick += Timer_Tick;
		m_Timer.Start();
	}
	
	
	void Timer_Tick(object sender, System.EventArgs e)
	{
		Invalidate();
	}
	
	
	protected void Box2D_onPaint(Alt.GUI.PaintEventArgs e)
	{
		if (m_Test == null)
		{
			return;
		}
		
		Alt.Sketch.Graphics graphics = e.Graphics;
		
		graphics.SmoothingMode = SmoothingMode.AntiAlias;
		
		
		m_Test.SetTextLine(30);
		settings.hz = TPS;// settingsHz;
		
		m_Test.Step(settings);
		
		m_Test.DrawTitle(50, 15, "Dominos");
	}
	
	
	protected void Box2D_onMouseDown(Alt.GUI.MouseEventArgs e)
	{
		if (m_Test == null)
		{
			return;
		}
		
		if (e.Button == Alt.GUI.MouseButtons.Left)
		{
			Alt.Sketch.Vector2 p = ConvertScreenToWorld(e.X, e.Y);

			/*TEMP
			if (Alt.GUI.Control.IsShiftDown)
			{
				m_Test.ShiftMouseDown(p);
			}
			else*/
			{
				m_Test.MouseDown(p);
			}
		}
	}	
	
	protected void Box2D_onMouseUp(Alt.GUI.MouseEventArgs e)
	{
		if (m_Test == null)
		{
			return;
		}
		
		if (e.Button == Alt.GUI.MouseButtons.Left)
		{
			m_Test.MouseUp(ConvertScreenToWorld(e.X, e.Y));
		}
	}	
	
	protected void Box2D_onMouseMove(Alt.GUI.MouseEventArgs e)
	{
		if (m_Test == null)
		{
			return;
		}
		
		m_Test.MouseMove(ConvertScreenToWorld(e.X, e.Y));
	}


	protected void Box2D_onKeyDown(Alt.GUI.KeyEventArgs e)
	{
		if (m_Test == null)
		{
			return;
		}
		
		m_Test.InjectKeyDown(e);
	}

	
	protected void Box2D_onRestart()
	{
		m_Test = Alt.GUI.Box2D.Demo.Testbed.Tests.Dominos.Create();
		m_Test._debugDraw = DebugDraw;
	}
	
	protected void Box2D_onLaunchBomb()
	{
		if (m_Test == null)
		{
			return;
		}
		
		m_Test.LaunchBomb();
	}
}
