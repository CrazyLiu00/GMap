//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI;
using Alt.Sketch;


partial class AltGUIControlHostDemo
{	
	public class DemoControl : UserControl
	{
		Alt.GUI.Timer m_Timer;

		double m_TargetX;
		double m_TargetY;
		Brush m_TargetBrush;
		Pen m_TargetPen;

		
		public DemoControl()
		{
			m_TargetBrush = new SolidColorBrush(Alt.Sketch.Color.FromArgb(80, Alt.Sketch.Color.Green));
			
			//  thickness 1.001 (or any not natural number) is for drawing smoothing lines
			//  in SmoothingMode.AntiAlias Graphics mode if you don't use RenderLinesByPolygons
			m_TargetPen = new Pen(Alt.Sketch.Color.FromArgb(192, Alt.Sketch.Color.Lime*0.75), 1.701);

			m_Timer = new Alt.GUI.Timer(10);
			m_Timer.Tick += Timer_Tick;
			m_Timer.Start();
		}


		void Timer_Tick(object sender, System.EventArgs e)
		{
			angledir = Math.Sign(angledir) * (Math.Sqrt(Math.Abs(maxAngle - Math.Abs(angle))) + 1) / 23;
			
			angle += angledir;
			if (angle < -maxAngle ||
			    angle > maxAngle)
			{
				angledir = -angledir;
			}

			Invalidate();
		}


		protected override void OnPaintBackground (Alt.GUI.PaintEventArgs pevent)
		{
			//base.OnPaintBackground (pevent);
		}


		const double maxAngle = 7;
		double angle = 0;
		double angledir = 0.5;

		protected override void OnPaint (Alt.GUI.PaintEventArgs e)
		{
			base.OnPaint (e);

			
			Alt.Sketch.Graphics graphics = e.Graphics;
			
			
			Matrix matrix = graphics.Transform;
			{
				//  correct offset
				Alt.Sketch.Rect clientRectangle = ClientRectangle;
				Alt.Sketch.Rect rect = clientRectangle;
				double targetX = (m_TargetX - (clientRectangle.Width / 2)) * clientRectangle.Width / rect.Width + clientRectangle.Width / 2;
				double targetY = (m_TargetY - (clientRectangle.Height / 2)) * clientRectangle.Height / rect.Height + clientRectangle.Height / 2;
				graphics.TranslateTransform(targetX, targetY);
				
				graphics.RotateTransform(angle);
				
				int radialStep = 30;
				int nSteps = 5;
				int lineDXY = radialStep * (nSteps + 1);
				
				graphics.FillCircle(m_TargetBrush, 0, 0, radialStep * nSteps);
				
				graphics.SmoothingMode = SmoothingMode.AntiAlias;
				
				graphics.DrawLine(m_TargetPen, 0, -lineDXY, 0, lineDXY);
				graphics.DrawLine(m_TargetPen, -lineDXY, 0, lineDXY, 0);
				
				for (int i = 1; i <= nSteps; i++)
				{
					graphics.DrawCircle(m_TargetPen, 0, 0, radialStep * i);
				}
			}
			graphics.Transform = matrix;
		}
	
	
		protected override void OnMouseMove(Alt.GUI.MouseEventArgs e)
		{
			base.OnMouseMove(e);
			
			if (m_TargetX != e.X ||
			    m_TargetY != e.Y)
			{
				m_TargetX = e.X;
				m_TargetY = e.Y;
				
				Refresh();
			}
		}
		
		
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			Alt.Sketch.Size clientSize = ClientSize;
			
			m_TargetX = clientSize.Width / 2;
			m_TargetY = clientSize.Height / 2;
		}
	}
}
