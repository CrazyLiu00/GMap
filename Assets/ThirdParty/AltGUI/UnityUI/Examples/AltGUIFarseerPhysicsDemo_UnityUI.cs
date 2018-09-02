//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/Farseer Physics Demo")]
public class AltGUIFarseerPhysicsDemo_UnityUI : AltGUIFarseerPhysicsDemo
{	
	AltGUIFarseerPhysics FarseerPhysics
	{
		get
		{
			return gameObject.GetComponent<AltGUIFarseerPhysics>();
		}
	}
	
	
	protected override void Invalidate()
	{
		AltGUIFarseerPhysics farseerPhysics = FarseerPhysics;
		if (farseerPhysics == null)
		{
			return;
		}
		
		farseerPhysics.Invalidate();
	}


	protected override Alt.Sketch.Size ClientSize
	{
		get
		{
			AltGUIFarseerPhysics farseerPhysics = FarseerPhysics;
			if (farseerPhysics == null)
			{
				return Alt.Sketch.Size.One;
			}
			
			return farseerPhysics.ClientSize;
		}
	}


	// Use this for initialization
	void Start ()
	{
		AltGUIFarseerPhysics farseerPhysics = FarseerPhysics;
		if (farseerPhysics == null)
		{
			return;
		}

		farseerPhysics.onPaint.AddListener(FarseerPhysics_onPaint);
		
		farseerPhysics.onFarseerPhysicsMouseDown.AddListener(FarseerPhysics_onMouseDown);
		farseerPhysics.onFarseerPhysicsMouseUp.AddListener(FarseerPhysics_onMouseUp);
		farseerPhysics.onFarseerPhysicsMouseMove.AddListener(FarseerPhysics_onMouseMove);
		farseerPhysics.onMouseWheel.AddListener(FarseerPhysics_onMouseWheel);

		farseerPhysics.onFarseerPhysicsKeyDown.AddListener(FarseerPhysics_onKeyDown);
		farseerPhysics.onFarseerPhysicsKeyDown.AddListener(FarseerPhysics_onKeyDown);

		farseerPhysics.onFarseerPhysicsEnableOrDisableFlag.AddListener(FarseerPhysics_onFarseerPhysicsEnableOrDisableFlag);

		farseerPhysics.onFarseerPhysicsRestartEvent.AddListener(FarseerPhysics_onRestart);

		Initialize ();
	}
}
