//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;

using Alt.Sketch;


namespace Alt.GUI
{
    static partial class UnityHelper
    {
		/// <summary>
		/// Destroy the specified object immediately, unless not in the editor, in which case the regular Destroy is used instead.
		/// </summary>
		
		public static void DestroyImmediate (UnityEngine.Object obj)
		{
			if (obj != null)
			{
				if (UnityEngine.Application.isEditor)
				{
					UnityEngine.Object.DestroyImmediate(obj);
				}
				else
				{
					UnityEngine.Object.Destroy(obj);
				}
			}
		}
	}
}
