//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace UnityEditor.UI
{
    /// <summary>
    /// This script adds the UI menu options to the Unity Editor.
    /// </summary>

	static partial class AltGUIUnityEditorMenu
    {
		public const string EditorAltGUIPath = "AltGUI/Unity UI/";
		public const string EditorUIPath = "GameObject/UI/AltGUI/";
		public const string EditorAssetsPath = "Assets/AltGUI/Unity UI/Create ";



		[MenuItem("Help/AltGUI Documentation")]
		static void ShowHelp0 (MenuCommand command)
		{
			ShowHelp();
		}
		
		[MenuItem("Help/AltGUI Support Forum")]
		static void ShowHelp01 (MenuCommand command)
		{
			Application.OpenURL("http://www.altsoftlab.com/forum");
		}


		/*TEMP
		[MenuItem("AltGUI/AltSketch/AltNETType Library [Component???]", false, 577)]
		public static void Add_AltNETTypeLibrary(MenuCommand menuCommand)
		{
		}*/

		
		[MenuItem("AltGUI/AltOS [Coming Soon...]", false, 577)]
		public static void AltOS(MenuCommand menuCommand)
		{
		}
		
		[MenuItem("AltGUI/AltStudio [Coming Soon...]", false, 577)]
		public static void AltStudio(MenuCommand menuCommand)
		{
		}
		


		[MenuItem(EditorAltGUIPath + AltOSHost.EditorName, false, AltOSHost.EditorID)]
		[MenuItem(EditorUIPath + AltOSHost.EditorName, false, AltOSHost.EditorID)]
		[MenuItem(EditorAssetsPath + AltOSHost.EditorName, false)]
		public static void Add_AltOSHost(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltOSHost>(menuCommand);
			/*TEMP
			GameObject root = CreateUIElementRoot(typeof(AltOSHost).Name, menuCommand, s_ThickGUIElementSize);
			if (root == null)
			{
				return;
			}
			
			GameObject childAltSketchPaint = CreateUIObject("AltOSHost", root);
			
			Image image = root.AddComponent<Image>();
			image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kInputFieldBackgroundPath);
			image.type = Image.Type.Sliced;
			image.color = s_DefaultSelectableColor;
			
			AltOSHost host = root.AddComponent<AltOSHost>();
			SetDefaultColorTransitionValues(host);
			
			AltOSView view = childAltSketchPaint.AddComponent<AltOSView>();
			
			RectTransform textRectTransform = childAltSketchPaint.GetComponent<RectTransform>();
			textRectTransform.anchorMin = Vector2.zero;
			textRectTransform.anchorMax = Vector2.one;
			textRectTransform.sizeDelta = Vector2.zero;
			textRectTransform.offsetMin = new Vector2(6, 6);
			textRectTransform.offsetMax = new Vector2(-6, -6);
			
			host.viewComponent = view;*/
		}



		[MenuItem(EditorAltGUIPath + AltSketchPaint.EditorName, false, AltSketchPaint.EditorID)]
		[MenuItem(EditorUIPath + AltSketchPaint.EditorName, false, AltSketchPaint.EditorID)]
		[MenuItem(EditorAssetsPath + AltSketchPaint.EditorName, false)]
		public static void Add_AltSketchPaint(MenuCommand menuCommand)
		{
			GameObject go = CreateUIElementRoot("AltSketchPaint", menuCommand, s_ImageGUIElementSize);
			if (go == null)
			{
				return;
			}
			
			go.AddComponent<AltSketchPaint>();
		}
		
		
		
		static T CreateAltGUIHostBasedObject<T>(MenuCommand menuCommand) where T : AltGUIControlHost
		{
			return CreateAltGUIHostBasedObject<T>(menuCommand, false);
		}
		static T CreateAltGUIHostBasedObject<T>(MenuCommand menuCommand, bool showBorder) where T : AltGUIControlHost
		{
			GameObject root = CreateUIElementRoot(typeof(T).Name, menuCommand, s_ThickGUIElementSize);
			if (root == null)
			{
				return null;
			}
			
			GameObject childAltSketchPaint = CreateUIObject("AltSketchPaint", root);
			
			if (showBorder)
			{
				Image image = root.AddComponent<Image>();
				image.sprite = AssetDatabase.GetBuiltinExtraResource<Sprite>(kInputFieldBackgroundPath);
				image.type = Image.Type.Sliced;
				image.color = s_DefaultSelectableColor;
			}

			T host = root.AddComponent<T>();
			SetDefaultColorTransitionValues(host);
			
			AltSketchPaint paint = childAltSketchPaint.AddComponent<AltSketchPaint>();
			
			RectTransform textRectTransform = childAltSketchPaint.GetComponent<RectTransform>();
			textRectTransform.anchorMin = Vector2.zero;
			textRectTransform.anchorMax = Vector2.one;
			textRectTransform.sizeDelta = Vector2.zero;
			if (showBorder)
			{
				textRectTransform.offsetMin = new Vector2(6, 6);
				textRectTransform.offsetMax = new Vector2(-6, -6);
			}
			
			host.paintComponent = paint;

			return host;
		}



		[MenuItem(EditorAltGUIPath + AltGUIControlHost.EditorName, false, AltGUIControlHost.EditorID)]
		[MenuItem(EditorUIPath + AltGUIControlHost.EditorName, false, AltGUIControlHost.EditorID)]
		[MenuItem(EditorAssetsPath + AltGUIControlHost.EditorName, false)]
		public static void Add_AltGUIHost(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUIControlHost>(menuCommand);
		}


		
		[MenuItem(EditorAltGUIPath + AltGUI3DPieChart.EditorName, false, AltGUI3DPieChart.EditorID)]
		[MenuItem(EditorUIPath + AltGUI3DPieChart.EditorName, false, AltGUI3DPieChart.EditorID)]
		public static void Add_AltGUI3DPieChart(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUI3DPieChart>(menuCommand);
		}
		
		
		[MenuItem(EditorAltGUIPath + AltGUIAForgeFilteredPictureBox.EditorName, false, AltGUIAForgeFilteredPictureBox.EditorID)]
		[MenuItem(EditorUIPath + AltGUIAForgeFilteredPictureBox.EditorName, false, AltGUIAForgeFilteredPictureBox.EditorID)]
		public static void Add_AltGUIAForgeFilteredPictureBox(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUIAForgeFilteredPictureBox>(menuCommand);
		}
		
		
		[MenuItem(EditorAltGUIPath + AltGUIAwesomium.EditorName, false, AltGUIAwesomium.EditorID)]
		[MenuItem(EditorUIPath + AltGUIAwesomium.EditorName, false, AltGUIAwesomium.EditorID)]
		public static void Add_AltGUIAwesomium(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUIAwesomium>(menuCommand);
		}
		
		
		[MenuItem(EditorAltGUIPath + AltGUIBox2D.EditorName, false, AltGUIBox2D.EditorID)]
		[MenuItem(EditorUIPath + AltGUIBox2D.EditorName, false, AltGUIBox2D.EditorID)]
		public static void Add_AltGUIBox2D(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUIBox2D>(menuCommand);
		}
		
		
		[MenuItem(EditorAltGUIPath + AltGUIFarseerPhysics.EditorName, false, AltGUIFarseerPhysics.EditorID)]
		[MenuItem(EditorUIPath + AltGUIFarseerPhysics.EditorName, false, AltGUIFarseerPhysics.EditorID)]
		public static void Add_AltGUIFarseerPhysics(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUIFarseerPhysics>(menuCommand);
		}
		
		
		[MenuItem(EditorAltGUIPath + AltGUIGMap.EditorName, false, AltGUIGMap.EditorID)]
		[MenuItem(EditorUIPath + AltGUIGMap.EditorName, false, AltGUIGMap.EditorID)]
		public static void Add_AltGUIGMap(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUIGMap>(menuCommand);
		}
		
		
		[MenuItem(EditorAltGUIPath + AltGUIHtmlLabel.EditorName, false, AltGUIHtmlLabel.EditorID)]
		[MenuItem(EditorUIPath + AltGUIHtmlLabel.EditorName, false, AltGUIHtmlLabel.EditorID)]
		public static void Add_AltGUIHtmlLabel(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUIHtmlLabel>(menuCommand);
		}
		
		
		[MenuItem(EditorAltGUIPath + AltGUIHtmlPanel.EditorName, false, AltGUIHtmlPanel.EditorID)]
		[MenuItem(EditorUIPath + AltGUIHtmlPanel.EditorName, false, AltGUIHtmlPanel.EditorID)]
		public static void Add_AltGUIHtmlPanel(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUIHtmlPanel>(menuCommand);
		}
		
		
		[MenuItem(EditorAltGUIPath + AltGUIICSharpCodeTextEditor.EditorName, false, AltGUIICSharpCodeTextEditor.EditorID)]
		[MenuItem(EditorUIPath + AltGUIICSharpCodeTextEditor.EditorName, false, AltGUIICSharpCodeTextEditor.EditorID)]
		public static void Add_AltGUIICSharpCodeTextEditor(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUIICSharpCodeTextEditor>(menuCommand, true);
		}
		
		
		[MenuItem(EditorAltGUIPath + AltGUINPlot.EditorName, false, AltGUINPlot.EditorID)]
		[MenuItem(EditorUIPath + AltGUINPlot.EditorName, false, AltGUINPlot.EditorID)]
		public static void Add_AltGUINPlot(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUINPlot>(menuCommand);
		}
		
		
		[MenuItem(EditorAltGUIPath + AltGUIOxyPlot.EditorName, false, AltGUIOxyPlot.EditorID)]
		[MenuItem(EditorUIPath + AltGUIOxyPlot.EditorName, false, AltGUIOxyPlot.EditorID)]
		public static void Add_AltGUIOxyPlot(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUIOxyPlot>(menuCommand);
		}

		
		[MenuItem(EditorAltGUIPath + AltGUIPictureBox.EditorName, false, AltGUIPictureBox.EditorID)]
		[MenuItem(EditorUIPath + AltGUIPictureBox.EditorName, false, AltGUIPictureBox.EditorID)]
		public static void Add_AltGUIPictureBox(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUIPictureBox>(menuCommand);
		}
		
		
		[MenuItem(EditorAltGUIPath + AltGUIQuickFont.EditorName, false, AltGUIQuickFont.EditorID)]
		[MenuItem(EditorUIPath + AltGUIQuickFont.EditorName, false, AltGUIQuickFont.EditorID)]
		public static void Add_AltGUIQuickFont(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUIQuickFont>(menuCommand);
		}
		
		
		[MenuItem(EditorAltGUIPath + AltGUIRichTextBox.EditorName, false, AltGUIRichTextBox.EditorID)]
		[MenuItem(EditorUIPath + AltGUIRichTextBox.EditorName, false, AltGUIRichTextBox.EditorID)]
		public static void Add_AltGUIRichTextBox(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUIRichTextBox>(menuCommand, true);
		}
		
		
		[MenuItem(EditorAltGUIPath + AltGUISvg.EditorName, false, AltGUISvg.EditorID)]
		[MenuItem(EditorUIPath + AltGUISvg.EditorName, false, AltGUISvg.EditorID)]
		public static void Add_AltGUISvg(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUISvg>(menuCommand);
		}
		
		
		[MenuItem(EditorAltGUIPath + AltGUIZedGraph.EditorName, false, AltGUIZedGraph.EditorID)]
		[MenuItem(EditorUIPath + AltGUIZedGraph.EditorName, false, AltGUIZedGraph.EditorID)]
		public static void Add_AltGUIZedGraph(MenuCommand menuCommand)
		{
			CreateAltGUIHostBasedObject<AltGUIZedGraph>(menuCommand);
		}
	}
}
