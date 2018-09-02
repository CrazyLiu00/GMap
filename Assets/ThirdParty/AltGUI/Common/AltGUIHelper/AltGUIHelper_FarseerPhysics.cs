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

using Alt.FarseerPhysics;
using Alt.FarseerPhysics.Common;


namespace Alt.GUI.FarseerPhysics
{
	[Serializable]
	public class FarseerPhysicsKeyEvent : UnityEvent<Alt.GUI.KeyEventArgs>
	{
	}

	[Serializable]
	public class FarseerPhysicsEnableOrDisableFlagEvent : UnityEvent<DebugViewFlags>
	{
	}
	
	[Serializable]
	public class FarseerPhysicsRestartEvent : UnityEvent
	{
	}
}


namespace Alt.GUI
{
	static partial class AltGUIHelper
	{	
		internal static FarseerPhysicsContainer Create_FarseerPhysicsContainer(Alt.GUI.Temporary.Gwen.Control.Base parent)
		{
			return new FarseerPhysicsContainer(parent);
		}
		
		
		internal class FarseerPhysicsContainer : Alt.GUI.Temporary.Gwen.Control.DoubleBufferedControl
		{
			public FarseerPhysicsContainer(Alt.GUI.Temporary.Gwen.Control.Base parent) :
				base(parent)
			{
				DoubleBuffered = false;
				KeyboardInputEnabled = true;
			}

			
			Alt.GUI.Temporary.Gwen.Control.Base m_PanelTop;

			
			protected override void OnLoad(EventArgs e)
			{
				base.OnLoad(e);
				
				
				Cursor = GUI.Cursors.Hand;
				
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
				}
				
				
				Focus();
			}
			
			
			void RestartButton_Click(object sender, EventArgs e)
			{
				Restart();
			}

			
			// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
			[SerializeField]
			Alt.GUI.FarseerPhysics.FarseerPhysicsKeyEvent m_onFarseerPhysicsKeyDown = new Alt.GUI.FarseerPhysics.FarseerPhysicsKeyEvent();
			public Alt.GUI.FarseerPhysics.FarseerPhysicsKeyEvent onFarseerPhysicsKeyDown
			{
				get
				{
					return m_onFarseerPhysicsKeyDown;
				}
			}
			
			// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
			[SerializeField]
			Alt.GUI.FarseerPhysics.FarseerPhysicsKeyEvent m_onFarseerPhysicsKeyUp = new Alt.GUI.FarseerPhysics.FarseerPhysicsKeyEvent();
			public Alt.GUI.FarseerPhysics.FarseerPhysicsKeyEvent onFarseerPhysicsKeyUp
			{
				get
				{
					return m_onFarseerPhysicsKeyUp;
				}
			}
			
			protected override void OnKeyDown(GUI.KeyEventArgs e)
			{
				base.OnKeyDown(e);
				
				
				// Press 'r' to reset.
				if (e.KeyCode == GUI.Keys.R)
				{
					Restart();
				}
				else if (e.KeyCode == GUI.Keys.P)
					// || newGamePad.IsButtonDown(Buttons.Start) && oldGamePad.IsButtonUp(Buttons.Start))
				{
					Pause();
				}
				else if (e.KeyCode == GUI.Keys.F1)
				{
					EnableOrDisableFlag(DebugViewFlags.Shape);
				}
				else if (e.KeyCode == GUI.Keys.F2)
				{
					EnableOrDisableFlag(DebugViewFlags.DebugPanel);
				}
				else if (e.KeyCode == GUI.Keys.F3)
				{
					EnableOrDisableFlag(DebugViewFlags.PerformanceGraph);
				}
				else if (e.KeyCode == GUI.Keys.F4)
				{
					EnableOrDisableFlag(DebugViewFlags.AABB);
				}
				else if (e.KeyCode == GUI.Keys.F5)
				{
					EnableOrDisableFlag(DebugViewFlags.CenterOfMass);
				}
				else if (e.KeyCode == GUI.Keys.F6)
				{
					EnableOrDisableFlag(DebugViewFlags.Joint);
				}
				else if (e.KeyCode == GUI.Keys.F7)
				{
					EnableOrDisableFlag(DebugViewFlags.ContactPoints);
					EnableOrDisableFlag(DebugViewFlags.ContactNormals);
				}
				else if (e.KeyCode == GUI.Keys.F8)
				{
					EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
				}
				else if (e.KeyCode == GUI.Keys.F9)
				{
					EnableOrDisableFlag(DebugViewFlags.PolygonPoints);
				}
				else
				{
					m_onFarseerPhysicsKeyDown.Invoke(e);
				}
			}
			
			protected override void OnKeyUp(GUI.KeyEventArgs e)
			{
				base.OnKeyUp(e);
				
				m_onFarseerPhysicsKeyUp.Invoke(e);
			}
			
			

			// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
			[SerializeField]
			Alt.GUI.FarseerPhysics.FarseerPhysicsEnableOrDisableFlagEvent m_onFarseerPhysicsEnableOrDisableFlag = new Alt.GUI.FarseerPhysics.FarseerPhysicsEnableOrDisableFlagEvent();
			public Alt.GUI.FarseerPhysics.FarseerPhysicsEnableOrDisableFlagEvent onFarseerPhysicsEnableOrDisableFlag
			{
				get
				{
					return m_onFarseerPhysicsEnableOrDisableFlag;
				}
			}
			void EnableOrDisableFlag(DebugViewFlags flag)
			{
				m_onFarseerPhysicsEnableOrDisableFlag.Invoke(flag);
			}
			
			
			protected override void OnPaint(GUI.PaintEventArgs e)
			{
				Alt.Sketch.Graphics graphics = e.Graphics;
				if (!DoubleBuffered)
				{
					graphics.FillRectangle(Alt.Sketch.Color.FromArgb((graphics is SoftwareGraphics) ? 128 : 192, Alt.Sketch.Color.DodgerBlue), ClientRectangle);
				}
				else
				{
					graphics.Clear(Alt.Sketch.Color.FromArgb((graphics is SoftwareGraphics) ? 128 : 192, Alt.Sketch.Color.DodgerBlue));
				}
				

				//Paint
				base.OnPaint(e);
			}
			
			protected override void Render(Alt.GUI.Temporary.Gwen.Skin.Base skin)
			{
				m_PanelTop.ClientBackColor = Alt.Sketch.Color.FromArgb((skin.Renderer.Graphics is SoftwareGraphics) ? 160 : 192, Alt.Sketch.Color.DodgerBlue);
				
				base.Render(skin);
			}


			// Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
			[SerializeField]
			Alt.GUI.FarseerPhysics.FarseerPhysicsRestartEvent m_onFarseerPhysicsRestartEvent = new Alt.GUI.FarseerPhysics.FarseerPhysicsRestartEvent();
			public Alt.GUI.FarseerPhysics.FarseerPhysicsRestartEvent onFarseerPhysicsRestartEvent
			{
				get
				{
					return m_onFarseerPhysicsRestartEvent;
				}
			}
			void Restart()
			{
				//StartTest(testIndex);
				m_onFarseerPhysicsRestartEvent.Invoke();
			}
			
			void Pause()
			{
				//settings.Pause = !settings.Pause;
			}
		}
	}
}
