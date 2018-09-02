//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_CrossSplitter : GUnit
    {
        int m_CurZoom;
		Alt.GUI.Temporary.Gwen.Control.CrossSplitter m_Splitter;


        public GUnit_CrossSplitter(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            m_CurZoom = 0;

			m_Splitter = new Alt.GUI.Temporary.Gwen.Control.CrossSplitter(this);
            m_Splitter.SetPosition(0, 0);
            m_Splitter.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;

            {
				Alt.GUI.Temporary.Gwen.Control.VerticalSplitter splitter = new Alt.GUI.Temporary.Gwen.Control.VerticalSplitter(m_Splitter);
				Alt.GUI.Temporary.Gwen.Control.Button button1 = new Alt.GUI.Temporary.Gwen.Control.Button(splitter);
                button1.SetText("Vertical left");
				Alt.GUI.Temporary.Gwen.Control.Button button2 = new Alt.GUI.Temporary.Gwen.Control.Button(splitter);
                button2.SetText("Vertical right");
                splitter.SetPanel(0, button1);
                splitter.SetPanel(1, button2);
                m_Splitter.SetPanel(0, splitter);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.HorizontalSplitter splitter = new Alt.GUI.Temporary.Gwen.Control.HorizontalSplitter(m_Splitter);
				Alt.GUI.Temporary.Gwen.Control.Button button1 = new Alt.GUI.Temporary.Gwen.Control.Button(splitter);
                button1.SetText("Horizontal up");
				Alt.GUI.Temporary.Gwen.Control.Button button2 = new Alt.GUI.Temporary.Gwen.Control.Button(splitter);
                button2.SetText("Horizontal down");
                splitter.SetPanel(0, button1);
                splitter.SetPanel(1, button2);
                m_Splitter.SetPanel(1, splitter);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.HorizontalSplitter splitter = new Alt.GUI.Temporary.Gwen.Control.HorizontalSplitter(m_Splitter);
				Alt.GUI.Temporary.Gwen.Control.Button button1 = new Alt.GUI.Temporary.Gwen.Control.Button(splitter);
                button1.SetText("Horizontal up");
				Alt.GUI.Temporary.Gwen.Control.Button button2 = new Alt.GUI.Temporary.Gwen.Control.Button(splitter);
                button2.SetText("Horizontal down");
                splitter.SetPanel(0, button1);
                splitter.SetPanel(1, button2);
                m_Splitter.SetPanel(2, splitter);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.VerticalSplitter splitter = new Alt.GUI.Temporary.Gwen.Control.VerticalSplitter(m_Splitter);
				Alt.GUI.Temporary.Gwen.Control.Button button1 = new Alt.GUI.Temporary.Gwen.Control.Button(splitter);
                button1.SetText("Vertical left");
				Alt.GUI.Temporary.Gwen.Control.Button button2 = new Alt.GUI.Temporary.Gwen.Control.Button(splitter);
                button2.SetText("Vertical right");
                splitter.SetPanel(0, button1);
                splitter.SetPanel(1, button2);
                m_Splitter.SetPanel(3, splitter);
            }

            //Status bar to hold unit testing buttons
			Alt.GUI.Temporary.Gwen.Control.StatusBar pStatus = new Alt.GUI.Temporary.Gwen.Control.StatusBar(this);
            pStatus.Dock = Alt.GUI.Temporary.Gwen.Pos.Bottom;

            {
				Alt.GUI.Temporary.Gwen.Control.Button pButton = new Alt.GUI.Temporary.Gwen.Control.Button(pStatus);
                pButton.SetText("Zoom");
                pButton.Clicked += ZoomTest;
                pStatus.AddControl(pButton, false);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.Button pButton = new Alt.GUI.Temporary.Gwen.Control.Button(pStatus);
                pButton.SetText("UnZoom");
                pButton.Clicked += UnZoomTest;
                pStatus.AddControl(pButton, false);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.Button pButton = new Alt.GUI.Temporary.Gwen.Control.Button(pStatus);
                pButton.SetText("CenterPanels");
                pButton.Clicked += CenterPanels;
                pStatus.AddControl(pButton, true);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.Button pButton = new Alt.GUI.Temporary.Gwen.Control.Button(pStatus);
                pButton.SetText("Splitters");
                pButton.Clicked += ToggleSplitters;
                pStatus.AddControl(pButton, true);
            }
        }

		void ZoomTest(Alt.GUI.Temporary.Gwen.Control.Base control)
        {
            m_Splitter.Zoom(m_CurZoom);
            m_CurZoom++;
            if (m_CurZoom == 4)
                m_CurZoom = 0;
        }

		void UnZoomTest(Alt.GUI.Temporary.Gwen.Control.Base control)
        {
            m_Splitter.UnZoom();
        }

		void CenterPanels(Alt.GUI.Temporary.Gwen.Control.Base control)
        {
            m_Splitter.CenterPanels();
            m_Splitter.UnZoom();
        }

		void ToggleSplitters(Alt.GUI.Temporary.Gwen.Control.Base control)
        {
            m_Splitter.SplittersVisible = !m_Splitter.SplittersVisible;
        }

        protected override void Layout(Alt.GUI.Temporary.Gwen.Skin.Base skin)
        {            
        }
    }
}
