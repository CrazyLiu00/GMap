//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.HtmlRenderer;
using Alt.GUI.HtmlRenderer.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo.HtmlRenderer
{
    public class SampleForm : WindowControl
    {
		Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlToolTip _htmlToolTip;
		Alt.GUI.Temporary.Gwen.Control.Button _changeTooltipButton;
		Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlPanel _htmlPanel;
		Alt.GUI.Temporary.Gwen.Control.Base _htmlLabelHostingPanel;
		Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlLabel _htmlLabel;

        Brush m_TransparentBGBrush;


        public SampleForm(Base parent)
            : base(parent, "Sample Form", true)
        {
            DeleteOnClose = true;

            Size = new SizeI(530, 290);
        }


        public void Center()
        {
            Align.Center(this);
        }



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            //
            HorizontalSplitter splitter = new HorizontalSplitter(this);
            splitter.Dock = Pos.Fill;

            Base topContainer = new Base(splitter);
            topContainer.Dock = Pos.Fill;

            Base bottomContainer = new Base(splitter);
            bottomContainer.Dock = Pos.Fill;

            splitter.SetPanel(0, topContainer);
            splitter.SetPanel(1, bottomContainer);
            splitter.SetVValue(0.35f);
            splitter.Margin = new Margin(5);


            //  Top
			Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(topContainer);
            label.Text = "HtmlLabel:";
            label.Dock = Pos.Top;
            label.Alignment = Pos.Center;
            label.AutoSizeToContents = true;
            label.TextColor = Color.Green;
            label.Margin = new Margin(2, 2, 2, 5);

            _htmlLabelHostingPanel = new DoubleBufferedControl(topContainer);
            _htmlLabelHostingPanel.Dock = Pos.Fill;
            _htmlLabelHostingPanel.Paint += new GUI.PaintEventHandler(OnHtmlLabelHostingPanelPaint);
            
			_htmlLabel = new Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlLabel(_htmlLabelHostingPanel);
            _htmlLabel.Dock = Pos.Fill;
            _htmlLabel.AutoSize = false;
            _htmlLabel.AutoSizeHeightOnly = true;
            _htmlLabel.BackColor = Color.Empty;// Transparent;
            _htmlLabel.BaseStylesheet = null;
            _htmlLabel.Text = "This is an <b>HtmlLabel</b> on transparent background with <span style=\"color: re" +
                "d\">colors</span> and links: <a href=\"http://htmlrenderer.codeplex.com/\">HTML Renderer</a>";


            //  Bottom
			label = new Alt.GUI.Temporary.Gwen.Control.Label(bottomContainer);
            label.Text = "HtmlPanel:";
            label.Dock = Pos.Top;
            label.Alignment = Pos.Center;
            label.AutoSizeToContents = true;
            label.TextColor = Color.Green;
            label.Margin = new Margin(2, 2, 2, 5);
            
			_changeTooltipButton = new Alt.GUI.Temporary.Gwen.Control.Button(bottomContainer);
            _changeTooltipButton.Dock = Pos.Bottom;
            _changeTooltipButton.Text = "Click me to change tooltip";
            _changeTooltipButton.Click += new System.EventHandler(OnButtonClick);

			_htmlToolTip = new Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlToolTip(_changeTooltipButton);
            _htmlToolTip.SetToolTip(_changeTooltipButton, "When you click this button, this tooltip will be replaced for the text of the <b>HtmlLabel</b>");
            
			_htmlPanel = new Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlPanel(bottomContainer);
            _htmlPanel.Dock = Pos.Fill;
            _htmlPanel.AutoScroll = true;
            _htmlPanel.BackColor = Color.White;
            _htmlPanel.BaseStylesheet = null;
            _htmlPanel.Text = HtmlRenderer.Resources.SampleForm_HtmlPanel;


            //
            Center();


            //
            Bitmap bmp = new Bitmap(10, 10);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.FillRectangle(Brushes.LightGray, new RectI(0, 0, 5, 5));
                g.FillRectangle(Brushes.LightGray, new RectI(5, 5, 5, 5));
            }

            m_TransparentBGBrush = new TextureBrush(bmp, WrapMode.Tile);
        }


        void OnHtmlLabelHostingPanelPaint(object sender, GUI.PaintEventArgs e)
        {
            if (m_TransparentBGBrush != null)
            {
                e.Graphics.FillRectangle(m_TransparentBGBrush, _htmlLabelHostingPanel.ClientRectangle);
            }
        }


        void OnButtonClick(object sender, EventArgs e)
        {
            _htmlToolTip.SetToolTip(_changeTooltipButton, _htmlLabel.Text);
        }
    }
}
