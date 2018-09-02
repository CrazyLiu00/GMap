//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Alt.IO;
using Alt.Sketch;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace Alt.GUI.Box2D
{
	[Serializable]
	public class Box2DKeyEvent : UnityEvent<Alt.GUI.KeyEventArgs>
	{
	}
	
	[Serializable]
	public class Box2DRestartEvent : UnityEvent
	{
	}
	
	[Serializable]
	public class Box2DLaunchBomb : UnityEvent
	{
	}
}
	

namespace Alt.GUI
{
	static partial class AltGUIHelper
	{
		internal static Box2DContainer Create_Box2DContainer(Alt.GUI.Temporary.Gwen.Control.Base parent, Alt.Box2D.AltSketchDebugDraw debugDraw)
		{
			return new Box2DContainer(parent, debugDraw);
		}
		
		
		internal class Box2DContainer : Alt.GUI.Temporary.Gwen.Control.DoubleBufferedControl
		{
			Alt.Box2D.AltSketchDebugDraw m_DebugDraw;
			
			
			public Box2DContainer(Alt.GUI.Temporary.Gwen.Control.Base parent, Alt.Box2D.AltSketchDebugDraw debugDraw) :
				base(parent)
			{
				m_DebugDraw = debugDraw;
				
				DoubleBuffered = false;
				KeyboardInputEnabled = true;
			}
			
			
			double viewZoom = 1.0;
			bool rMouseDown;
			Point lastp;
			
			Alt.GUI.Temporary.Gwen.Control.Base m_PanelTop;
			
			double m_OffsetX;
			double m_OffsetY;
			double m_CenterX;
			double m_CenterY;
			double m_Scale;
			
			
			protected override void OnLoad(EventArgs e)
			{
				base.OnLoad(e);
				
				
				Cursor = Alt.GUI.Cursors.Hand;
				
				//  GUI
				{
					m_PanelTop = new Alt.GUI.Temporary.Gwen.Control.Base(this);
					m_PanelTop.Dock = Alt.GUI.Temporary.Gwen.Pos.Top;
					m_PanelTop.ClientBackColor = Alt.Sketch.Color.FromArgb(96, Alt.Sketch.Color.Black);
					m_PanelTop.Height = 38;
					
					Alt.GUI.Temporary.Gwen.Control.Base controlsMain = new Alt.GUI.Temporary.Gwen.Control.Base(m_PanelTop);
					controlsMain.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
					
					
					Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(controlsMain);
					label.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
					label.AutoSizeToContents = true;
					label.Text = "Use mouse device to operate with dynamic objects\nand scene zoom / offset";
					label.Font = new Alt.Sketch.Font("Arial", 10, Alt.Sketch.FontStyle.Bold);
					label.TextColor = Alt.Sketch.Color.LightBlue * 1.23;
					label.Margin = new Alt.GUI.Temporary.Gwen.Margin(5, 3, 5, 0);
					
					
					Alt.GUI.Temporary.Gwen.Control.Button refreshButton = new Alt.GUI.Temporary.Gwen.Control.Button(controlsMain);
					refreshButton.Dock = Alt.GUI.Temporary.Gwen.Pos.Right;
					refreshButton.Click += new EventHandler(RestartButton_Click);
					refreshButton.SetToolTipText("Restart");
					refreshButton.SetImage("AltData/Refresh.png", true);
					refreshButton.Width = 36;
					refreshButton.Margin = new Alt.GUI.Temporary.Gwen.Margin(0, 1, 0, 1);
					
					Alt.GUI.Temporary.Gwen.Control.Button ballButton = new Alt.GUI.Temporary.Gwen.Control.Button(controlsMain);
					ballButton.Dock = Alt.GUI.Temporary.Gwen.Pos.Right;
					ballButton.Click += new EventHandler(BallButton_Click);
					ballButton.Text = "PUSH BALL";
					ballButton.NormalTextColor = Alt.Sketch.Color.Green;
					ballButton.Margin = new Alt.GUI.Temporary.Gwen.Margin(5, 1, 5, 1);
				}
				
				
				Focus();
				
				
				viewZoom = 1.0;
				m_OffsetX = 0;
				m_OffsetY = 0;
			}
			
			
			void BallButton_Click(object sender, EventArgs e)
			{
				LaunchBomb();
			}
			
			void RestartButton_Click(object sender, EventArgs e)
			{
				Restart();
			}
						
			
			// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
			[SerializeField]
			Alt.GUI.Box2D.Box2DKeyEvent m_onBox2DKeyDown = new Alt.GUI.Box2D.Box2DKeyEvent();
			public Alt.GUI.Box2D.Box2DKeyEvent onBox2DKeyDown
			{
				get
				{
					return m_onBox2DKeyDown;
				}
			}

			protected override void OnKeyDown(Alt.GUI.KeyEventArgs e)
			{
				base.OnKeyDown(e);
				
				
				// Press 'z' to zoom out.
				if (e.KeyCode == Alt.GUI.Keys.Z)
				{
					viewZoom = Math.Min(1.1 * viewZoom, 20.0);
				}
				// Press 'x' to zoom in.
				else if (e.KeyCode == Alt.GUI.Keys.X)
				{
					viewZoom = Math.Max(0.9 * viewZoom, 0.02);
				}
				// Press 'r' to reset.
				else if (e.KeyCode == Alt.GUI.Keys.R)
				{
					Restart();
				}
				// Press space to launch a bomb.
				else if (e.KeyCode == Alt.GUI.Keys.Space ||
				         e.KeyCode == Alt.GUI.Keys.B)
				{
					LaunchBomb();
				}
				else if (e.KeyCode == Alt.GUI.Keys.P)
					// || newGamePad.IsButtonDown(Buttons.Start) && oldGamePad.IsButtonUp(Buttons.Start))
				{
					Pause();
				}
				// Press left to pan left.
				else if (e.KeyCode == Alt.GUI.Keys.Left)
				{
					m_OffsetX -= 10;
				}
				// Press right to pan right.
				else if (e.KeyCode == Alt.GUI.Keys.Right)
				{
					m_OffsetX += 10;
				}
				// Press down to pan down.
				else if (e.KeyCode == Alt.GUI.Keys.Down)
				{
					m_OffsetY += 10;
				}
				// Press up to pan up.
				else if (e.KeyCode == Alt.GUI.Keys.Up)
				{
					m_OffsetY -= 10;
				}
				// Press home to reset the view.
				else if (e.KeyCode == Alt.GUI.Keys.Home)
				{
					viewZoom = 1.0;
					m_OffsetX = 0;
					m_OffsetY = 0;
				}
				else
				{
					m_onBox2DKeyDown.Invoke(e);
				}
			}
			
			
			protected override void OnPaint(Alt.GUI.PaintEventArgs e)
			{
                Debug.Log("onpaint");
				Alt.Sketch.Graphics graphics = e.Graphics;
				if (!DoubleBuffered)
				{
					graphics.FillRectangle(Alt.Sketch.Color.FromArgb((graphics is SoftwareGraphics) ? 128 : 192, Alt.Sketch.Color.DodgerBlue), ClientRectangle);
				}
				else
				{
					graphics.Clear(Alt.Sketch.Color.FromArgb((graphics is SoftwareGraphics) ? 128 : 192, Alt.Sketch.Color.DodgerBlue));
				}

				int w = ClientWidth;
				int h = ClientHeight;

				m_Scale = viewZoom * System.Math.Min(w, h) / 23;
				m_CenterX = w / 2 + m_OffsetX;
				m_CenterY = h * 0.83 + m_OffsetY;
				m_DebugDraw.SetViewTransform(m_CenterX, m_CenterY, m_Scale);
				
				
				//Paint
				base.OnPaint(e);
			}
			
			protected override void Render(Alt.GUI.Temporary.Gwen.Skin.Base skin)
			{
                Debug.Log("Render");
				m_PanelTop.ClientBackColor = Alt.Sketch.Color.FromArgb((skin.Renderer.Graphics is SoftwareGraphics) ? 160 : 192, Alt.Sketch.Color.DodgerBlue);
				
				base.Render(skin);
			}
			
			
			internal Alt.Sketch.Vector2 ConvertScreenToWorld(double x, double y)
			{
				Matrix4 matrix = Matrix4.CreateTranslation(m_CenterX, m_CenterY);
				matrix.Scale(m_Scale, -m_Scale);
				matrix.Invert();
				
				matrix.Transform(ref x, ref y);
				return new Alt.Sketch.Vector2(x, y);
			}
			
			
			protected override void OnMouseDown(Alt.GUI.MouseEventArgs e)
			{
				base.OnMouseDown(e);
				
				if (e.Button == Alt.GUI.MouseButtons.Right)
				{
					lastp = e.Location;
					rMouseDown = true;
				}
			}
			
			
			protected override void OnMouseUp(Alt.GUI.MouseEventArgs e)
			{
				base.OnMouseUp(e);
				
				if (e.Button == Alt.GUI.MouseButtons.Right)
				{
					rMouseDown = false;
				}
			}
			
			
			protected override void OnMouseMove(Alt.GUI.MouseEventArgs e)
			{
				base.OnMouseMove(e);
				
				if (rMouseDown)
				{
					Alt.Sketch.Vector2 diff = e.Location - lastp;
					m_OffsetX += diff.X;
					m_OffsetY += diff.Y;
				}
				lastp = e.Location;
			}
			
			
			protected override void OnMouseWheel(Alt.GUI.MouseEventArgs e)
			{
				base.OnMouseWheel(e);
				
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
			
			
			// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
			[SerializeField]
			Alt.GUI.Box2D.Box2DRestartEvent m_onBox2DRestartEvent = new Alt.GUI.Box2D.Box2DRestartEvent();
			public Alt.GUI.Box2D.Box2DRestartEvent onBox2DRestartEvent
			{
				get
				{
					return m_onBox2DRestartEvent;
				}
			}
			void Restart()
			{
				m_onBox2DRestartEvent.Invoke();
			}
			
			void Pause()
			{
				//settings.pause = settings.pause > (uint)0 ? (uint)1 : (uint)0;
			}
			
			// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
			[SerializeField]
			Alt.GUI.Box2D.Box2DLaunchBomb m_onBox2DLaunchBomb = new Alt.GUI.Box2D.Box2DLaunchBomb();
			public Alt.GUI.Box2D.Box2DLaunchBomb onBox2DLaunchBomb
			{
				get
				{
					return m_onBox2DLaunchBomb;
				}
			}
			void LaunchBomb()
			{
				m_onBox2DLaunchBomb.Invoke();
			}
		}
	}
}
