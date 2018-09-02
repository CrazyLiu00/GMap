//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/Box2D Demo")]
public class AltGUIBox2DDemo_UnityUI : AltGUIBox2DDemo
{	
	AltGUIBox2D Box2D
	{
		get
		{
			return gameObject.GetComponent<AltGUIBox2D>();
		}
	}


	protected override Alt.Box2D.AltSketchDebugDraw DebugDraw
	{
		get
		{
			AltGUIBox2D box2D = Box2D;
			if (box2D == null)
			{
				return null;
			}
			
			return box2D.DebugDraw;
		}
	}
		
	protected override double TPS
	{
		get
		{
			AltGUIBox2D box2D = Box2D;
			if (box2D == null)
			{
				return 30;
			}

			return box2D.TPS;
		}
	}
	
	protected override Alt.Sketch.Vector2 ConvertScreenToWorld(double x, double y)
	{
		AltGUIBox2D box2D = Box2D;
		if (box2D == null)
		{
			return new Alt.Sketch.Vector2(x, y);
		}

		return box2D.ConvertScreenToWorld(x, y);
	}
	
	protected override void Invalidate()
	{
		AltGUIBox2D box2D = Box2D;
		if (box2D == null)
		{
			return;
		}

		box2D.Invalidate();
	}

	
	// Use this for initialization
	void Start ()
	{
		AltGUIBox2D box2D = Box2D;
		if (box2D == null)
		{
			return;
		}

		box2D.onPaint.AddListener(Box2D_onPaint);

		box2D.onBox2DMouseDown.AddListener(Box2D_onMouseDown);
		box2D.onBox2DMouseUp.AddListener(Box2D_onMouseUp);
		box2D.onBox2DMouseMove.AddListener(Box2D_onMouseMove);

		box2D.onBox2DKeyDown.AddListener(Box2D_onKeyDown);

		box2D.onBox2DRestartEvent.AddListener(Box2D_onRestart);
		box2D.onBox2DLaunchBomb.AddListener(Box2D_onLaunchBomb);

		Initialize ();
	}
}
