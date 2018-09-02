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
	/// AltGUI HtmlLabel.
    /// </summary>

	[AddComponentMenu(AltGUIIntegration.EditorComponentMenuPath + AltGUIHtmlLabel.EditorName, AltGUIHtmlLabel.EditorID)]
	public class AltGUIHtmlLabel
		: AltGUIControlHost
    {
		new public const string EditorName = "HTML Label";
		new public const int EditorID = 779;
		
		#if UNITY_EDITOR
		internal override string GetEditorName()
		{
			return EditorName;
		}
		#endif

		
		public Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlLabel HtmlLabel
		{
			get
			{
				return GwenChild as Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlLabel;
			}
		}


		public string Text
		{
			get
			{
				Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlLabel htmlLabel = HtmlLabel;
				if (htmlLabel != null)
				{
					return htmlLabel.Text;
				}
				
				return string.Empty;
			}
			set
			{
				Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlLabel htmlLabel = HtmlLabel;
				if (htmlLabel != null)
				{
					htmlLabel.Text = value;
				}
			}
		}
		
		
		protected override void Start ()
		{
			base.Start ();

			GwenChild = AltGUIHelper.Create_HtmlLabel(GetOrCreateGwenCanvas());
			
			Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlLabel htmlLabel = HtmlLabel;
			if (htmlLabel == null)
			{
				return;
			}
					
			htmlLabel.RenderError += OnRenderError;
			htmlLabel.LinkClicked += OnLinkClicked;
			htmlLabel.StylesheetLoad += OnStylesheetLoad;
			htmlLabel.ImageLoad += OnImageLoad;
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
