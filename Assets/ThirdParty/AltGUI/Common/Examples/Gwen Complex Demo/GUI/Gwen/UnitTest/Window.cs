//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_Window : GUnit
    {
        private int m_WindowCount;
        private Random rand;


        public GUnit_Window(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            rand = new Random();

			Alt.GUI.Temporary.Gwen.Control.Button button1 = new Alt.GUI.Temporary.Gwen.Control.Button(this);
            button1.SetText("Open a Window");
            button1.Clicked += OpenWindow;
            button1.SizeToContents();

			Alt.GUI.Temporary.Gwen.Control.Button button2 = new Alt.GUI.Temporary.Gwen.Control.Button(this);
            button2.SetText("Open a MessageBox");
            button2.SizeToContents();
            button2.Clicked += OpenMsgbox;
            Alt.GUI.Temporary.Gwen.Align.PlaceRightBottom(button2, button1, 10);

            m_WindowCount = 1;
        }


        void OpenWindow(Base control)
        {
			Alt.GUI.Temporary.Gwen.Control.WindowControl window = new Alt.GUI.Temporary.Gwen.Control.WindowControl(GetCurrentCanvas());
            window.Caption = String.Format("Window {0}", m_WindowCount);
            window.SetSize(rand.Next(200, 400), rand.Next(200, 400));
            window.SetPosition(rand.Next(700), rand.Next(400));

            window.ShouldCacheToTexture = true;

            m_WindowCount++;
        }


        void OpenMsgbox(Base control)
        {
			Alt.GUI.Temporary.Gwen.Control.MessageBox window = new Alt.GUI.Temporary.Gwen.Control.MessageBox(GetCurrentCanvas(),
                String.Format("Window {0}   MessageBox window = new MessageBox(GetCanvas(), String.Format(  MessageBox window = new MessageBox(GetCanvas(), String.Format(", m_WindowCount),
                "Message");
            window.SetPosition(rand.Next(700), rand.Next(400));

            window.ShouldCacheToTexture = true;

            m_WindowCount++;
        }
    }
}
