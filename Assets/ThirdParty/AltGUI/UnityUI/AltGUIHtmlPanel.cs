//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.GUI;
using Alt.GUI.HtmlRenderer;
using Alt.Sketch;


namespace UnityEngine.UI
{
    /// <summary>
	/// AltGUI HtmlPanel.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUIHtmlPanel.EditorName, AltGUIHtmlPanel.EditorID)]
	public class AltGUIHtmlPanel
		: AltGUIControlHost
    {
		new public const string EditorName = "HTML Panel";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif

		
		public Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlPanel HtmlPanel
		{
			get
			{
				return GwenChild as Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlPanel;
			}
		}
		
		
		public string Text
		{
			get
			{
				Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlPanel htmlPanel = HtmlPanel;
				if (htmlPanel != null)
				{
					return htmlPanel.Text;
				}
				
				return string.Empty;
			}
			set
			{
				Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlPanel htmlPanel = HtmlPanel;
				if (htmlPanel != null)
				{
					htmlPanel.Text = value;
				}
			}
		}
		
		
		protected override void Start ()
		{
			base.Start ();

			GwenChild = AltGUIHelper.Create_HtmlPanel(GetOrCreateGwenCanvas());		

			Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlPanel htmlPanel = HtmlPanel;
			if (htmlPanel == null)
			{
				return;
			}

			htmlPanel.RenderError += OnRenderError;
			htmlPanel.LinkClicked += OnLinkClicked;
			htmlPanel.StylesheetLoad += OnStylesheetLoad;
			htmlPanel.ImageLoad += OnImageLoad;
		}


		/// <summary>
		/// On specific link click handle it here.
		/// </summary>
		void OnLinkClicked(object sender, HtmlLinkClickedEventArgs e)
		{
		}

		
		/// <summary>
		/// Handle stylesheet resolve.
		/// </summary>
		void OnStylesheetLoad(object sender, HtmlStylesheetLoadEventArgs e)
		{
			AltGUIHelper.HtmlRendererTools.OnStylesheetLoad(sender, e);
		}
		
		
		/// <summary>
		/// On image load in renderer set the image by event async.
		/// </summary>
		void OnImageLoad(object sender, HtmlImageLoadEventArgs e)
		{
			AltGUIHelper.HtmlRendererTools.OnImageLoad(sender, e);
		}
		
		
		/// <summary>
		/// Show error raised from html renderer.
		/// </summary>
		void OnRenderError(object sender, HtmlRenderErrorEventArgs e)
		{
			AltGUIHelper.HtmlRendererTools.OnRenderError(sender, e);
		}
	}
}
