//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/AltSketch Paint Demo")]
public class AltSketchPaintDemo_UnityUI : AltSketchPaintDemo
{	
	AltSketchPaint Paint
	{
		get
		{
			return gameObject.GetComponent<AltSketchPaint>();
		}
	}


	// Use this for initialization
	protected override void Start ()
	{
		base.Start();

		AltSketchPaint paint = Paint;
		if (paint == null)
		{
			return;
		}
		
		//	requires for ExtBrush demo
		if (UseHardwareRender)
		{
			paint.RenderType = Alt.Sketch.Rendering.RenderType.Hardware;
		}

		paint.onPaint.AddListener(Paint_onPaint);
	}


	protected override void Timer_Tick(object sender, System.EventArgs e)
	{
		base.Timer_Tick(sender, e);

		AltSketchPaint paint = Paint;
		if (paint == null)
		{
			return;
		}

		paint.Invalidate();
	}
}
