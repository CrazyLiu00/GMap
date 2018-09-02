//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


namespace UnityEditor.UI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AltGUIControlHost), true)]
    public class AltGUIHostEditor : SelectableEditor
    {
        SerializedProperty m_PaintComponent;
		SerializedProperty m_onPaint;


        protected override void OnEnable()
        {
            base.OnEnable();

			m_PaintComponent 	= serializedObject.FindProperty("m_PaintComponent");
			m_onPaint    		= serializedObject.FindProperty("m_onPaint");
		}


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            base.OnInspectorGUI();

            EditorGUILayout.Space();

			EditorGUILayout.PropertyField(m_PaintComponent);

			if (m_PaintComponent != null && m_PaintComponent.objectReferenceValue != null)
            {
				//AltSketchPaint paint = m_PaintComponent.objectReferenceValue as AltSketchPaint;
            }
			
			EditorGUILayout.Space();
			// Draw the event notification options
			EditorGUILayout.PropertyField(m_onPaint);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
