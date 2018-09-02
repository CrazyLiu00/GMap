//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.UI;


namespace UnityEditor.UI
{
    /// <summary>
    /// Editor class used to edit UI Images.
    /// </summary>
    [CustomEditor(typeof(AltSketchPaint), true)]
    [CanEditMultipleObjects]
    public class AltSketchPaintEditor : GraphicEditor
    {
		SerializedProperty m_MaxFPS;
		SerializedProperty m_RenderType;
		SerializedProperty m_onPaint;


        protected override void OnEnable()
        {
            base.OnEnable();

            SetShowNativeSize(true);
        
			m_MaxFPS	= serializedObject.FindProperty("m_MaxFPS");
			m_RenderType	= serializedObject.FindProperty("m_RenderType");
			m_onPaint    	= serializedObject.FindProperty("m_onPaint");
		}


        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            AppearanceControlsGUI();
			SetShowNativeSize(false);
            NativeSizeButtonGUI();

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(m_MaxFPS);
			EditorGUILayout.PropertyField(m_RenderType);

			EditorGUILayout.Space();
			// Draw the event notification options
			EditorGUILayout.PropertyField(m_onPaint);

            serializedObject.ApplyModifiedProperties();
        }


		void SetShowNativeSize(bool instant)
		{
			base.SetShowNativeSize(false//TEMP	m_Texture.objectReferenceValue != null
			                       , instant);
		}


        /// <summary>
        /// Allow the texture to be previewed.
        /// </summary>

        public override bool HasPreviewGUI()
        {
            AltSketchPaint paintImage = target as AltSketchPaint;
			return paintImage != null;
        }


        /// <summary>
        /// Draw the Image preview.
        /// </summary>

        public override void OnPreviewGUI(Rect rect, GUIStyle background)
        {
			AltSketchPaint paint = target as AltSketchPaint;
			Texture tex = paint.mainTexture;

            if (tex == null)
                return;

			Rect outer = paint.UVRect;
			outer.xMin *= paint.rectTransform.rect.width;
			outer.xMax *= paint.rectTransform.rect.width;
			outer.yMin *= paint.rectTransform.rect.height;
			outer.yMax *= paint.rectTransform.rect.height;

			SpriteDrawUtility.DrawSprite(tex, rect, outer, paint.UVRect, paint.canvasRenderer.GetColor());
        }


        /// <summary>
        /// Info String drawn at the bottom of the Preview
        /// </summary>

        public override string GetInfoString()
        {
            AltSketchPaint paint = target as AltSketchPaint;

            // Image size Text
            string text = string.Format("AltSketchPaint Size: {0}x{1}",
			                            Mathf.RoundToInt(Mathf.Abs(paint.rectTransform.rect.width)),
			                            Mathf.RoundToInt(Mathf.Abs(paint.rectTransform.rect.height)));

            return text;
        }
    }
}
