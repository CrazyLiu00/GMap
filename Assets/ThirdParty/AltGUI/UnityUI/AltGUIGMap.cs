//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.GUI;
using Alt.Sketch;


namespace UnityEngine.UI
{
    /// <summary>
	/// AltGUI GMap Control.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUIGMap.EditorName, AltGUIGMap.EditorID)]
	public class AltGUIGMap : AltGUIControlHost
    {
		new public const string EditorName = "GMap";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif


		public Alt.Sketch.Demo.GMap.NET.Map Map
		{
			get
			{
				return GwenChild as Alt.Sketch.Demo.GMap.NET.Map;
			}
		}


		public GMap.NET.PointLatLng Position
		{
			get
			{
				Alt.Sketch.Demo.GMap.NET.Map map = Map;
				if (map != null)
				{
					return map.Position;
				}

				return GMap.NET.PointLatLng.Empty;
			}
			set
			{
				Alt.Sketch.Demo.GMap.NET.Map map = Map;
				if (map != null)
				{
					map.Position = value;
				}
			}
		}


		// Use this for initialization
		protected override void Start ()
		{
			base.Start();

			GwenChild = AltGUIHelper.Create_GMap(GetOrCreateGwenCanvas());
		}
	}
}
