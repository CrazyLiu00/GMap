//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.Temporary.Gwen.Control.Layout;
using Alt.Sketch;
using Alt.Threading;


namespace Alt.GUI.Demo
{
    public class Example_Awesomium : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 15;
                int y = 25;

                if (m_Awesomium != null &&
                    m_Awesomium.m_WebViewPanel != null)
                {
                    PointI pt = m_Awesomium.m_WebViewPanel.PointToScreen(new PointI(m_Awesomium.m_WebViewPanel.Width, 0));
                    y += pt.Y;
                    x += GetCanvas().Width - pt.X;
                }

                return new SizeI(x, y);
            }
        }


        string Description
        {
            get
            {
                return "Awesomium Web Browser";
            }
        }


        Alt.GUI.Temporary.Gwen.Control.Awesomium m_Awesomium;

        Alt.GUI.Temporary.Gwen.Control.Label m_TopLabel;
		const string label_TOP_text = "Awesomium Web Browser AltGUI Example";


        public Example_Awesomium(Base parent) :
            base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


			Alt.GUI.Temporary.Gwen.Control.Base topPanel = new Alt.GUI.Temporary.Gwen.Control.Base(this);
            topPanel.Dock = Pos.Top;
            topPanel.Height = 22;

			m_TopLabel = new Alt.GUI.Temporary.Gwen.Control.Label(topPanel);
            m_TopLabel.AutoSizeToContents = true;
            m_TopLabel.Text = label_TOP_text;
            m_TopLabel.TextColor = Color.Yellow;
            m_TopLabel.Dock = Pos.Left;
            m_TopLabel.Margin = new Margin(5, 3, 0, 5);


            m_Awesomium = new Alt.GUI.Temporary.Gwen.Control.Awesomium(this);
            m_Awesomium.Margin = Margin.Zero;
            m_Awesomium.Dock = Pos.Fill;
        }


        public override void Dispose()
        {
            if (m_Awesomium != null)
            {
                this.RemoveChild(m_Awesomium, false);
                m_Awesomium.Dispose();
                m_Awesomium = null;
            }

            base.Dispose();
        }
    }
}
