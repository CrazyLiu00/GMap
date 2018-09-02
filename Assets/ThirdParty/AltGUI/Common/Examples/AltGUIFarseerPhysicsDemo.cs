//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;

using Alt.Sketch;

using Alt.FarseerPhysics;
using Alt.FarseerPhysics.Common;
using Alt.GUI.FarseerPhysics.Demo.Testbed;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Tests;


[AddComponentMenu("AltGUI/Examples/Common/Farseer Physics Demo")]
public abstract class AltGUIFarseerPhysicsDemo : MonoBehaviour
{	
	Alt.GUI.Timer m_Timer;

	Alt.TPSCounter m_TPSCounter = new Alt.TPSCounter();
	
	Alt.Sketch.Font m_InfoFont;

	double viewZoom = 1.0;
	bool rMouseDown;
	Point lastp;

	FarseerPhysicsTest m_Test;
	FarseerPhysicsGameSettings settings;


	protected abstract Alt.Sketch.Size ClientSize
	{
		get;
	}

	protected abstract void Invalidate();


	protected void Initialize ()
	{
		m_InfoFont = new Alt.Sketch.Font("Arial", 10.01, Alt.Sketch.FontStyle.Bold);

		settings = new FarseerPhysicsGameSettings();

		FarseerPhysics_onRestart();

		m_Timer = new Alt.GUI.Timer(10);
		m_Timer.Tick += Timer_Tick;
		m_Timer.Start();
	}
	
	
	void ResetView()
	{
		viewZoom = 1.0;

		if (m_Test == null)
		{
			return;
		}
		
		m_Test.m_OffsetX = 0;
		m_Test.m_OffsetY = 12;
	}


	void Timer_Tick(object sender, System.EventArgs e)
	{
		Invalidate();
	}
	
	
	protected void FarseerPhysics_onPaint(Alt.GUI.PaintEventArgs e)
	{
		if (m_Test == null)
		{
			return;
		}
		
		Alt.Sketch.Graphics graphics = e.Graphics;
		
		graphics.SmoothingMode = SmoothingMode.AntiAlias;
		
		
		m_Test.DebugView.m_Graphics = graphics;
		m_Test.DebugView.m_Font = m_InfoFont;


		Alt.Sketch.Size clientSize = ClientSize;
		double ClientWidth = clientSize.Width;
		double ClientHeight = clientSize.Height;
		
		m_Test.m_Scale = viewZoom * System.Math.Min(ClientWidth, ClientHeight) / 39;
		m_Test.m_CenterX = ClientWidth / 2 + m_Test.m_OffsetX * m_Test.m_Scale;
		m_Test.m_CenterY = ClientHeight / 2 + m_Test.m_OffsetY * m_Test.m_Scale;
		
		
		m_TPSCounter.Tick();
		
		settings.Hz = m_TPSCounter.TPS;
		
		m_Test.TextLine = 30;
		m_Test.Update(settings);

		
		m_Test.m_CenterX = ClientWidth / 2 + m_Test.m_OffsetX * m_Test.m_Scale;
		m_Test.m_CenterY = ClientHeight * 3 / 5 + m_Test.m_OffsetY * m_Test.m_Scale;
		m_Test.DebugView.SetViewTransform(m_Test.m_CenterX, m_Test.m_CenterY, m_Test.m_Scale);
		
		
		m_Test.DrawTitle(30, 15, "Car");
		
		m_Test.DebugView.RenderDebugData();
	}
	
	
	protected void FarseerPhysics_onMouseDown(Alt.GUI.MouseEventArgs e)
	{
		if (m_Test == null)
		{
			return;
		}
		
		if (e.Button == Alt.GUI.MouseButtons.Left)
		{
			m_Test.MouseDown(e, m_Test.ConvertScreenToWorld(e.X, e.Y));
		}
		else if (e.Button == Alt.GUI.MouseButtons.Right)
		{
			lastp = e.Location;
			rMouseDown = true;
		}
	}	
	
	protected void FarseerPhysics_onMouseUp(Alt.GUI.MouseEventArgs e)
	{
		if (m_Test == null)
		{
			return;
		}
		
		if (e.Button == Alt.GUI.MouseButtons.Left)
		{
			m_Test.MouseUp(e, m_Test.ConvertScreenToWorld(e.X, e.Y));
		}
		else if (e.Button == Alt.GUI.MouseButtons.Right)
		{
			rMouseDown = false;
		}
	}	
	
	protected void FarseerPhysics_onMouseMove(Alt.GUI.MouseEventArgs e)
	{
		if (m_Test == null)
		{
			return;
		}
		
		Alt.Sketch.Vector2 wdiff = m_Test.ConvertScreenToWorld(e.X, e.Y) - m_Test.ConvertScreenToWorld(lastp.X, lastp.Y);
		if (rMouseDown)
		{
			m_Test.m_OffsetX += wdiff.X;
			m_Test.m_OffsetY -= wdiff.Y;
		}
		lastp = e.Location;

		m_Test.MouseMove(e, m_Test.ConvertScreenToWorld(e.X, e.Y), wdiff);
	}
	
	protected void FarseerPhysics_onMouseWheel(Alt.GUI.MouseEventArgs e)
	{
		if (m_Test == null)
		{
			return;
		}
		
		var direction = e.Delta;
		if (direction < 0)
		{
			viewZoom /= 1.1;
		}
		else
		{
			viewZoom *= 1.1;
		}
	}

	
	
	protected void FarseerPhysics_onKeyDown(Alt.GUI.KeyEventArgs e)
	{
		if (m_Test == null)
		{
			return;
		}
		
		// Press 'z' to zoom out.
		if (e.KeyCode == Alt.GUI.Keys.Z)
		{
			viewZoom = System.Math.Min(1.1 * viewZoom, 20.0);
		}
		// Press 'x' to zoom in.
		else if (e.KeyCode == Alt.GUI.Keys.X)
		{
			viewZoom = System.Math.Max(0.9 * viewZoom, 0.02);
		}
		// Press home to reset the view.
		else if (e.KeyCode == Alt.GUI.Keys.Home)
		{
			ResetView();
		}
		// Press left to pan left.
		else if (e.KeyCode == Alt.GUI.Keys.Left)
		{
			m_Test.m_OffsetX -= 0.5;
		}
		// Press right to pan right.
		else if (e.KeyCode == Alt.GUI.Keys.Right)
		{
			m_Test.m_OffsetX += 0.5;
		}
		// Press down to pan down.
		else if (e.KeyCode == Alt.GUI.Keys.Down)
		{
			m_Test.m_OffsetY += 0.5;
		}
		// Press up to pan up.
		else if (e.KeyCode == Alt.GUI.Keys.Up)
		{
			m_Test.m_OffsetY -= 0.5;
		}

		else
		{
			m_Test.InjectKeyDown(e);
		}
	}
		
	protected void FarseerPhysics_onKeyUp(Alt.GUI.KeyEventArgs e)
	{
		if (m_Test == null)
		{
			return;
		}
		
		m_Test.InjectKeyUp(e);
	}


	protected void FarseerPhysics_onFarseerPhysicsEnableOrDisableFlag(DebugViewFlags flag)
	{
		if (m_Test == null)
		{
			return;
		}
		
		if ((m_Test.DebugView.Flags & flag) == flag)
		{
			m_Test.DebugView.RemoveFlags(flag);
		}
		else
		{
			m_Test.DebugView.AppendFlags(flag);
		}
	}

	
	protected void FarseerPhysics_onRestart()
	{
		m_Test = Alt.GUI.FarseerPhysics.Demo.Testbed.Tests.CarTest.Create();
		m_Test.Initialize();
	
		ResetView();

		FarseerPhysics_onFarseerPhysicsEnableOrDisableFlag(DebugViewFlags.DebugPanel);
		FarseerPhysics_onFarseerPhysicsEnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
	}
}
