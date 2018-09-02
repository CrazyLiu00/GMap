//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


[AddComponentMenu("AltGUI/Unity UI/Examples/_Main Unity UI Demo")]
public class AltGUI_UnityUI_Demo : MonoBehaviour
{
	static Color s_SavedTextColor = new Color(50f / 255f, 50f / 255f, 50f / 255f, 1f);
	static Color s_SelectedTextColor = new Color(0f / 255f, 128f / 255f, 255f / 255f, 1f);

	const string sDemoPrefix = "AltGUIDemo_";
	const string sMenuPanel = sDemoPrefix + "MenuPanel";
	const string sWorkPanel = sDemoPrefix + "WorkPanel";
	const string sCaptionText = sDemoPrefix + "CaptionText";
	const string sExamplesText = sDemoPrefix + "ExamplesText";

	Button[] m_MenuButtons;


	class Example
	{
		public string Name;
		public Button Button;
		public GameObject GameObject;

		//	deffered initialisation
		public delegate void ScriptInitActionDelegate();
		public ScriptInitActionDelegate ScriptInitAction;


		public Example(string name, Button button, GameObject gameObject, ScriptInitActionDelegate scriptInitAction)
		{
			Name = name;
			Button = button;
			GameObject = gameObject;
			ScriptInitAction = scriptInitAction;
		}
	}

	Dictionary<string, Example> m_Examples = new Dictionary<string, Example>();
	Example m_CurrentExample;


	RectTransform MenuPanel_RectTransform
	{
		get
		{
			GameObject goMenuPanel = GameObject.Find(sMenuPanel);
			if (goMenuPanel == null)
			{
				return null;
			}
			
			return goMenuPanel.GetComponent<RectTransform>();
		}
	}
	
	
	RectTransform WorkPanel_RectTransform
	{
		get
		{
			GameObject goWorkPanel = GameObject.Find(sWorkPanel);
			if (goWorkPanel == null)
			{
				return null;
			}
			
			return goWorkPanel.GetComponent<RectTransform>();
		}
	}
	
	
	string CaptionText
	{
		set
		{
			GameObject goCaptionText = GameObject.Find(sCaptionText);
			if (goCaptionText == null)
			{
				return;
			}
			
			Text text = goCaptionText.GetComponent<Text>();
			if (text == null)
			{
				return;
			}
			
			text.text = value ?? string.Empty;
		}
	}
	
	string ExamplesText
	{
		set
		{
			GameObject goExamplesText = GameObject.Find(sExamplesText);
			if (goExamplesText == null)
			{
				return;
			}
			
			Text text = goExamplesText.GetComponent<Text>();
			if (text == null)
			{
				return;
			}
			
			text.text = value ?? string.Empty;
		}
	}

	
	// Use this for initialization
	void Start ()
	{
		ExamplesText = "<b><color=yellow>AltGUI</color></b> Examples:";

		CreateBaseUI();
		{
			LoadExamples();
		}
		CreateBaseUI2();
	
		StartExample("Gwen Complex Demo");
	}
	
	void CreateBaseUI()
	{
		//	MenuPanel
		{
			GameObject goMenuPanel = GameObject.Find(sMenuPanel);
			if (goMenuPanel == null)
			{
				return;
			}
			
			m_MenuButtons = goMenuPanel.GetComponentsInChildren<Button>();
			if (m_MenuButtons == null)
			{
				return;
			}
			
			RectTransform menuPanelRectTransform = MenuPanel_RectTransform;
			int dxy = 3;
			int dy = 0;
			float w = menuPanelRectTransform.rect.width - dxy * 2;
			float h = ((menuPanelRectTransform.rect.height - dxy * 2)) / (m_MenuButtons.Length != 0 ? m_MenuButtons.Length : 1) - dy;
			float x = menuPanelRectTransform.position.x;
			float y = menuPanelRectTransform.rect.height - dxy - h / 2;
			
			foreach (Button b in m_MenuButtons)
			{
				GameObject buttonGameObject = b.gameObject;
				buttonGameObject.SetActive(false);
				
				RectTransform goRectTransform = buttonGameObject.GetComponent<RectTransform>();
				goRectTransform.position = new Vector3(x, y);
				goRectTransform.sizeDelta = new Vector2(w, h);
				
				y -= h + dy;
			}
		}
	}
	
	void CreateBaseUI2()
	{
		//	MenuPanel
		{
			RectTransform menuPanelRectTransform = MenuPanel_RectTransform;
			int dxy = 3;
			int dy = 0;
			float w = menuPanelRectTransform.rect.width - dxy * 2;
			float h = ((menuPanelRectTransform.rect.height - dxy * 2)) / (m_Examples.Count != 0 ? m_Examples.Count : 1) - dy;
			float x = menuPanelRectTransform.position.x;
			float y = menuPanelRectTransform.rect.height - dxy - h / 2;
			
			foreach (Button b in m_MenuButtons)
			{
				GameObject buttonGameObject = b.gameObject;

				RectTransform goRectTransform = buttonGameObject.GetComponent<RectTransform>();
				goRectTransform.position = new Vector3(x, y);
				goRectTransform.sizeDelta = new Vector2(w, h);
				
				y -= h + dy;
			}
		}
	}


	void LoadExamples()
	{
		AddExample<AltGUIGwenComplexDemo_UnityUI>("Gwen Complex Demo", "AltGUIControlHost Gwen");
		AddExample<AltGUIGMapDemo_UnityUI>("GMap", "AltGUIGMap", false);
		AddExample<AltGUIHtmlPanelDemo_UnityUI>("Html Panel", "AltGUIHtmlPanel");
		AddExample<AltGUIHtmlLabelDemo_UnityUI>("Html Label", "AltGUIHtmlLabel");
		AddExample<AltGUIICSharpCodeTextEditorDemo_UnityUI>("ICSC TextEditor (Coming soon)", "AltGUIICSharpCodeTextEditor");
		AddExample<AltGUIRichTextBoxDemo_UnityUI>("RichTextBox (Coming soon)", "AltGUIRichTextBox", false);
		AddExample<AltGUIOxyPlotDemo_UnityUI>("OxyPlot", "AltGUIOxyPlot");
		AddExample<AltGUIOxyPlotMapDemo_UnityUI>("OxyPlot Map", "AltGUIOxyPlot Map", false);
		AddExample<AltGUINPlotDemo_UnityUI>("NPlot", "AltGUINPlot");
		AddExample<AltGUI3DPieChartDemo_UnityUI>("3DPie Chart", "AltGUI3DPieChart");
		AddExample<AltGUIZedGraphDemo_UnityUI>("ZedGraph", "AltGUIZedGraph");
		AddExample<AltGUISvgDemo_UnityUI>("SVG", "AltGUISvg");
		
		AddExample<AltGUIQuickFontDemo_UnityUI>("QuickFont", "AltGUIQuickFont");
		//	start long time background initialization
		AltGUIQuickFontDemo.Initialize();
		
		AddExample<AltGUIAForgeFilteredPictureBoxDemo_UnityUI>("AForge Filtered PictureBox", "AltGUIAForgeFilteredPictureBox", false);
		AddExample<AltGUIPictureBoxDemo_UnityUI>("PictureBox (GIF image)", "AltGUIPictureBox");
		AddExample<AltGUIBox2DDemo_UnityUI>("Box2D", "AltGUIBox2D");
		AddExample<AltGUIFarseerPhysicsDemo_UnityUI>("Farseer Physics", "AltGUIFarseerPhysics");
		AddExample<AltGUIControlHostDemo_UnityUI>("AltGUI Control Host", "AltGUIControlHost");
		AddExample<AltSketchPaintDemo_UnityUI>("AltSketch Paint", "AltSketchPaint Demo");
		AddExample<AltGUIAwesomiumDemo_UnityUI>("Awesomium", "AltGUIAwesomium");
	}

	
	void AddExample<TScriptClass>(string name, string controlName) where TScriptClass : MonoBehaviour
	{
		AddExample<TScriptClass>(name, controlName, true);
	}

	void AddExample<TScriptClass>(string name, string controlName, bool useInWeb) where TScriptClass : MonoBehaviour
	{
		//	Button
		Button button = null;
		{
			if (m_MenuButtons == null ||
			    m_MenuButtons.Length <= m_Examples.Count)
			{
				return;
			}

			string buttonName = "Button " + m_Examples.Count.ToString();
			foreach (Button b in m_MenuButtons)
			{
				if (string.Compare(b.gameObject.name, buttonName) == 0)
				{
					button = b;
					break;
				}
			}

			if (button == null)
			{
				return;
			}
		}


		//	Control
		GameObject controlGameObject = null;
		{
			GameObject goWorkPanel = GameObject.Find(sWorkPanel);
			if (goWorkPanel == null)
			{
				return;
			}
			
			UnityEngine.EventSystems.UIBehaviour[] uiBehaviours = goWorkPanel.GetComponentsInChildren<UnityEngine.EventSystems.UIBehaviour>();
			if (uiBehaviours == null)
			{
				return;
			}

			UnityEngine.EventSystems.UIBehaviour uiBehaviour = null;
			foreach (UnityEngine.EventSystems.UIBehaviour b in uiBehaviours)
			{
				if (string.Compare(b.gameObject.name, controlName) == 0)
				{
					uiBehaviour = b;
					break;
				}
			}
			
			if (uiBehaviour == null)
			{
				return;
			}

			controlGameObject = uiBehaviour.gameObject;
		}
		
		
		//
		controlGameObject.SetActive(false);
		
		#if UNITY_WEBPLAYER
		if (!useInWeb)
		{
			return;
		}
		#endif


		//
		button.gameObject.SetActive(true);
		
		Text text = button.GetComponentInChildren<Text>();
		text.fontSize = 13;
		text.text = name;

		button.onClick.AddListener(() => OnMenuButtonClick(name));


		m_Examples.Add(name, new Example(
			name,
			button,
			controlGameObject,
			delegate(){controlGameObject.AddComponent<TScriptClass>();}));
	}


	// Update is called once per frame
	void Update ()
	{
		//UpdateSize();
	}

	void UpdateSize()
	{
		if (m_CurrentExample == null)
		{
			return;
		}

		GameObject controlGameObject = m_CurrentExample.GameObject;

		RectTransform workPanelRectTransform = WorkPanel_RectTransform;
		RectTransform goRectTransform = controlGameObject.GetComponent<RectTransform>();
		goRectTransform.position = workPanelRectTransform.position;//new Vector3();
		goRectTransform.sizeDelta = new Vector2(workPanelRectTransform.rect.width - 10, workPanelRectTransform.rect.height - 10);
	}


	public void OnMenuButtonClick(string name)
	{
		StartExample(name);
	}


	void StartExample(string name)
	{
		if (m_CurrentExample != null)
		{
			if (m_CurrentExample.Name == name)
			{
				return;
			}
		}

		Example prevExample = m_CurrentExample;
		m_CurrentExample = null;

		if (!m_Examples.TryGetValue(name, out m_CurrentExample) ||
		    m_CurrentExample == null)
		{
			CaptionText = "Example '" + name + "'not found...";

			HideExample(prevExample);

			return;
		}


		CaptionText = "<b><color=yellow>AltGUI</color></b> Unity UI Example - <b><i><color=white>" + name + "</color></i></b>";

		SetMenuButtonTextColor(m_CurrentExample.Button, s_SelectedTextColor);

		UpdateSize();


		if (m_CurrentExample.ScriptInitAction != null)
		{
			m_CurrentExample.ScriptInitAction();
			m_CurrentExample.ScriptInitAction = null;
		}
		m_CurrentExample.GameObject.SetActive(true);


		HideExample(prevExample);


		UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(m_CurrentExample.GameObject);
	}

	void HideExample(Example example)
	{
		if (example != null)
		{
			SetMenuButtonTextColor(example.Button, s_SavedTextColor);
			example.GameObject.SetActive(false);
		}
	}


	void SetMenuButtonTextColor(Button button, Color color)
	{
		Text text = button.GetComponentInChildren<Text>();
		if (text != null)
		{
			s_SavedTextColor = text.color;
			text.color = color;
		}
	}
}
