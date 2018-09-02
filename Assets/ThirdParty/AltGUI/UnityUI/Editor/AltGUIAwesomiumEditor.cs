//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


namespace UnityEditor.UI
{
    [CanEditMultipleObjects]
	[CustomEditor(typeof(AltGUIAwesomium), true)]
	public class AltGUIAwesomiumEditor : AltGUIHostEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }


		protected override void OnDisable()
		{
			base.OnDisable();
		}


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}
