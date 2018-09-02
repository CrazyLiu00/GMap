//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

using Alt.ComponentModel;
using Alt.IO;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.HtmlRenderer;
using Alt.GUI.HtmlRenderer.Temporary.Gwen;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
    class Example_HtmlRenderer_Multi : Example_HtmlRenderer_Base
    {
        //  show generated html or regular update the web browser to show the new choice.
        LabeledCheckBox m_ShowTooltip;

#if !SILVERLIGHT
        LabeledCheckBox m_AntiAliasText;
        Alt.GUI.Temporary.Gwen.Control.Button m_OpenExternalViewButton;
        LabeledCheckBox m_ShowGeneratedHtmlCB;
#endif

        Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlPanel m_HtmlPanel;
        Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlToolTip m_HtmlToolTip;


        public Example_HtmlRenderer_Multi(Base parent) :
            base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


#if !SILVERLIGHT
            m_OpenExternalViewButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_LeftPanel);
            m_OpenExternalViewButton.Text = "Open External View";
            m_OpenExternalViewButton.Dock = Pos.Bottom;
            m_OpenExternalViewButton.Margin = new Margin(2, 5, 2, 0);
            m_OpenExternalViewButton.Click += new EventHandler(OnOpenExternalViewButtonClick);

            m_ShowGeneratedHtmlCB = new LabeledCheckBox(m_LeftPanel);
            m_ShowGeneratedHtmlCB.Text = "Show generated HTML";
            m_ShowGeneratedHtmlCB.Dock = Pos.Bottom;
            m_ShowGeneratedHtmlCB.Margin = new Margin(2, 10, 2, 2);
            m_ShowGeneratedHtmlCB.IsChecked = true;
#endif

            m_ShowTooltip = new LabeledCheckBox(m_LeftPanel);
            m_ShowTooltip.Text = "Show Tooltip";
            m_ShowTooltip.Dock = Pos.Bottom;
            m_ShowTooltip.Margin = new Margin(2, 10, 2, 2);
            //m_ShowTooltip.IsChecked = true;
            m_ShowTooltip.CheckChanged += new GwenEventHandler(ShowTooltip_CheckChanged);

#if !SILVERLIGHT
            m_AntiAliasText = new LabeledCheckBox(m_LeftPanel);
            m_AntiAliasText.Text = "AntiAlias Text";
            m_AntiAliasText.Dock = Pos.Bottom;
            m_AntiAliasText.Margin = new Margin(2, 10, 2, 2);
            //m_AntiAliasText.IsChecked = true;
            m_AntiAliasText.CheckChanged += new GwenEventHandler(AntiAliasText_CheckChanged);
#endif



            //  HtmlPanel
            m_HtmlPanel = new Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlPanel(m_RightPanel);
            m_HtmlPanel.Dock = Pos.Fill;
            m_HtmlPanel.ShouldDrawBackground = false;
            m_HtmlPanel.TextRenderingHint = TextRenderingHint.Default;

            m_HtmlPanel.AutoScroll = true;
            m_HtmlPanel.BackColor = Color.White;
            m_HtmlPanel.BaseStylesheet = null;

            m_HtmlPanel.RenderError += OnRenderError;
            m_HtmlPanel.LinkClicked += OnLinkClicked;
            m_HtmlPanel.StylesheetLoad += OnStylesheetLoad;
            m_HtmlPanel.ImageLoad += OnImageLoad;



            //  HtmlToolTip
            m_HtmlToolTip = new Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlToolTip(m_HtmlPanel);
            m_HtmlToolTip.ImageLoad += OnImageLoad;
            //m_HtmlToolTip.SetToolTip(m_HtmlPanel, HtmlRenderer.Resources.Tooltip);


            SelectFirstExample();
        }


        public override void SetHtml(string text)
        {
            GUI.Cursor cursor = m_HtmlPanel.Cursor;
            m_HtmlPanel.Cursor = GUI.Cursors.WaitCursor;

            m_HtmlPanel.Text = text;

            m_HtmlPanel.Cursor = cursor;
        }


        void ShowTooltip_CheckChanged(Base control)
        {
            m_HtmlToolTip.SetToolTip(m_HtmlPanel, m_ShowTooltip.IsChecked ? HtmlRenderer.Resources.Tooltip : null);
        }


        /// <summary>
        /// On specific link click handle it here.
        /// </summary>
        void OnLinkClicked(object sender, HtmlLinkClickedEventArgs e)
        {
            if (e.Link == "SayHello")
            {
                new Alt.GUI.Temporary.Gwen.Control.MessageBox(GetCanvas(), "Hello you!", "Link Clicked");

                e.Handled = true;
            }
            else if (e.Link == "ShowSampleForm")
            {
                //NoNeed	HtmlRenderer.SampleForm f =
				new HtmlRenderer.SampleForm(this);

                e.Handled = true;
            }
        }

        
#if !SILVERLIGHT
        /// <summary>
        /// Open the current html is external process - the default user browser.
        /// </summary>
        void OnOpenExternalViewButtonClick(object sender, EventArgs e)
        {
            var html = m_ShowGeneratedHtmlCB.IsChecked ? m_HtmlPanel.GetHtml() : m_HtmlPanel.Text;
            var tmpFile = Path.ChangeExtension(Path.GetTempFileName(), ".htm");
            File.WriteAllText(tmpFile, html);

            Diagnostics.ProcessHelper.Start(tmpFile);
        }


        void AntiAliasText_CheckChanged(Base control)
        {
            m_HtmlPanel.TextRenderingHint = m_AntiAliasText.IsChecked ? TextRenderingHint.AntiAliasGridFit : TextRenderingHint.Default;
        }
#endif
    }
}
