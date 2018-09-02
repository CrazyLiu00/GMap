//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

//#define USE_ExtBrush

using UnityEngine;

using Alt.Sketch;


[AddComponentMenu("AltGUI/Examples/Common/AltSketch Paint Demo")]
public abstract class AltSketchPaintDemo : MonoBehaviour
{	
	protected bool UseHardwareRender
	{
		get
		{
#if UNITY_5 && USE_ExtBrush
			return true;
#else
			return false;
#endif
		}
	}


	Alt.GUI.Timer m_Timer;

	Brush m_MaterialBrush1;
	Brush m_MaterialBrush2;
	Brush m_MaterialBrush3;
	Brush m_MaterialBrush4;

	int m_CurrentBrush = 2;

	Pen m_ContourPen;


	// Use this for initialization
	protected virtual void Start ()
	{
		m_ContourPen = new Pen(Alt.Sketch.Color.FromArgb(192, Alt.Sketch.Color.DodgerBlue), 3);

		m_Timer = new Alt.GUI.Timer(15);
		m_Timer.Tick += Timer_Tick;
		m_Timer.Start();
	}


	protected virtual void Timer_Tick(object sender, System.EventArgs e)
	{
		offset += offset_change_dirrection;
		int k = 30;
		if (offset < -k ||
			offset > k)
		{
			offset_change_dirrection = -offset_change_dirrection;

			m_CurrentBrush++;
			if (m_CurrentBrush > 3)
			{
				m_CurrentBrush = 0;
			}
		}
	}


	int offset = 0;
	int offset_change_dirrection = -1;
	protected void Paint_onPaint(Alt.GUI.PaintEventArgs e)
	{
#if UNITY_5 && USE_ExtBrush
		if (e.Graphics is SoftwareGraphics)
		{
			return;
		}

		Alt.Sketch.Color colorMultiplier;
#endif

		int opacity = 128;

		if (m_MaterialBrush1 == null)
		{
#if UNITY_5 && USE_ExtBrush
			Bitmap image = Bitmap.FromFile("AltData/About.gif");
			
			ExtBrush brush;
			brush = new ExtBrush(); brush.MaterialName = "Material1"; brush.SetExtParameter("Image", image); m_MaterialBrush1 = brush; 
			brush = new ExtBrush(); brush.MaterialName = "Material2"; brush.SetExtParameter("Image", image); m_MaterialBrush2 = brush;
			brush = new ExtBrush(); brush.MaterialName = "Material3"; brush.SetExtParameter("Image", image); m_MaterialBrush3 = brush;
			brush = new ExtBrush(); brush.MaterialName = "Material4"; brush.SetExtParameter("Image", image); m_MaterialBrush4 = brush;
#else
			SolidColorBrush brush;
			brush = new SolidColorBrush(Alt.Sketch.Color.FromArgb(opacity, Alt.Sketch.Color.Red)); m_MaterialBrush1 = brush; 
			brush = new SolidColorBrush(Alt.Sketch.Color.FromArgb(opacity, Alt.Sketch.Color.Green)); m_MaterialBrush2 = brush; 
			brush = new SolidColorBrush(Alt.Sketch.Color.FromArgb(opacity, Alt.Sketch.Color.DodgerBlue)); m_MaterialBrush3 = brush; 
			brush = new SolidColorBrush(Alt.Sketch.Color.FromArgb(opacity, Alt.Sketch.Color.Yellow)); m_MaterialBrush4 = brush; 
#endif
		}


		Brush currentBrush;

		switch (m_CurrentBrush)
		{
		case 0:
			currentBrush = m_MaterialBrush1;
#if UNITY_5 && USE_ExtBrush
			colorMultiplier = Alt.Sketch.Color.LightCoral;
#endif
			break;
		case 1:
			currentBrush = m_MaterialBrush2;
#if UNITY_5 && USE_ExtBrush
			colorMultiplier = Alt.Sketch.Color.Green;
#endif
			break;
		case 2:
			currentBrush = m_MaterialBrush3;
#if UNITY_5 && USE_ExtBrush
			colorMultiplier = Alt.Sketch.Color.Red;
#endif
			break;
		case 3:
			currentBrush = m_MaterialBrush4;
#if UNITY_5 && USE_ExtBrush
			colorMultiplier = Alt.Sketch.Color.Cyan;
#endif
			break;
		default:
			return;
		}

		Alt.Sketch.Graphics graphics = e.Graphics;
		graphics.SmoothingMode = SmoothingMode.None;
		
		Alt.Sketch.Rect rect = e.ClipRectangle;
		rect.Deflate(50 + offset, 50 + offset);
		
		graphics.FillRoundedRectangle(currentBrush, rect, 20
#if UNITY_5 && USE_ExtBrush
		                              , colorMultiplier
#else
		                              , UseHardwareRender ? Alt.Sketch.Color.FromArgb(opacity, Alt.Sketch.Color.White) : Alt.Sketch.Color.White
#endif
		                              );
		
		graphics.SmoothingMode = SmoothingMode.AntiAlias;
		graphics.DrawRoundedRectangle(m_ContourPen, rect, 20);
	}
}
