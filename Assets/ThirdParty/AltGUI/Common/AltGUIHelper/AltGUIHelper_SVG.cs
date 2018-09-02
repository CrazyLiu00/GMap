//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Alt.IO;
using Alt.Sketch;

using Alt.Sketch.Svg;
using Alt.Sketch.Svg.Transforms;


namespace Alt.GUI
{
	static partial class AltGUIHelper
	{
		internal static SVGControl Create_SVGControl()
		{
			return new SVGControl();
		}



		internal class SVGControl : UserControl
		{			
			SvgDocument m_svgDoc;
			Alt.Sketch.Rect m_SVGPathBounds;
			double m_SmallScale;
			
			
			double m_x;
			double m_y;
			double m_dx;
			double m_dy;
			double m_Scale;
			double m_Rotation = 0;
			double m_Opacity = 1;
			bool m_DragFlag;
			
			
			public SVGControl()
			{
				BackColor = Alt.Sketch.Color.Transparent;
			}

			protected override void OnResize (EventArgs e)
			{
				base.OnResize (e);

				Size clientSize = ClientSize;
				m_x = clientSize.Width / 2;
				m_y = clientSize.Height / 2;
				m_dx = 0.0;
				m_dy = 0.0;
				m_DragFlag = false;
			}
			
			
			public void LoadSVG(string fileName)
			{
				try
				{
					m_svgDoc = SvgDocument.Open(fileName);
					if (m_svgDoc == null)
					{
						return;
					}
					
					m_SVGPathBounds = new Alt.Sketch.Rect(m_svgDoc.Bounds.X, m_svgDoc.Bounds.Y, m_svgDoc.Bounds.Width, m_svgDoc.Bounds.Height);

					Size clientSize = ClientSize;
					m_Scale = System.Math.Min(clientSize.Width / m_SVGPathBounds.Width, clientSize.Height / m_SVGPathBounds.Height);
					
					Invalidate();
				}
				catch (Exception ex)//SvgException)
				{
					Console.WriteLine(ex.ToString());
					
					return;
				}
			}
			
			
			protected override void OnPaint(Alt.GUI.PaintEventArgs e)
			{
				base.OnPaint(e);
				
				if (m_svgDoc == null)
				{
					return;
				}
				
				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				
				Matrix4 mtx = Matrix4.CreateTranslation(-m_SVGPathBounds.Center.X, -m_SVGPathBounds.Center.Y);
				double scale = m_Scale;
				mtx.Scale(scale, scale, MatrixOrder.Append);
				mtx.Rotate(m_Rotation, MatrixOrder.Append);
				mtx.Translate(m_x, m_y, MatrixOrder.Append);
				
				e.Graphics.Transform = mtx.ToMatrix();
				
				e.Graphics.Opacity = m_Opacity;
				m_svgDoc.Draw(e.Graphics);
			}
			
			
			protected override void OnMouseDown(Alt.GUI.MouseEventArgs e)
			{
				base.OnMouseDown(e);
				
				m_dx = e.X - m_x;
				m_dy = e.Y - m_y;

				if (e.Button == Alt.GUI.MouseButtons.Left)
				{
					m_DragFlag = true;
				}
			}
			
			
			protected override void OnMouseMove(Alt.GUI.MouseEventArgs e)
			{
				base.OnMouseMove(e);
				
				if (m_DragFlag)
				{
					m_x = e.X - m_dx;
					m_y = e.Y - m_dy;
					
					Invalidate ();
				}
			}
			
			
			protected override void OnMouseUp(Alt.GUI.MouseEventArgs e)
			{
				base.OnMouseUp(e);
				
				if (e.Button == Alt.GUI.MouseButtons.Left)
				{
					m_DragFlag = false;
				}
			}
			
			
			protected override void OnMouseWheel(Alt.GUI.MouseEventArgs e)
			{
				base.OnMouseWheel(e);
				
				m_Scale += 0.001f * e.Delta;
				
				Invalidate();
			}
		}
	}
}
