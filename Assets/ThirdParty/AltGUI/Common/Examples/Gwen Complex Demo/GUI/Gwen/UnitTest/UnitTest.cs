//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.Temporary.Gwen.Control.Layout;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class UnitTest : DockBase
    {
		Alt.GUI.Temporary.Gwen.Control.Base m_LastControl;
		readonly Alt.GUI.Temporary.Gwen.Control.StatusBar m_StatusBar;
		readonly Alt.GUI.Temporary.Gwen.Control.ListBox m_TextOutput;
		Alt.GUI.Temporary.Gwen.Control.TabButton m_Button;
		readonly Alt.GUI.Temporary.Gwen.Control.CollapsibleList m_List;
        internal readonly Center m_Center;
		readonly Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox m_DebugCheck;

        public double Fps; // set this in your rendering loop
        public String Note; // additional text to display in status bar


        public UnitTest(Base parent) :
            this(parent, true)
        {
        }

        public UnitTest(Base parent, bool showStatusBar) :
            base(parent)
        {
            Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
            //NoNeed    SetSize(1024, 768);

            m_List = new CollapsibleList(this);
            m_List.ShouldCacheToTexture = true;

            LeftDock.TabControl.AddPage("Unit tests", m_List);
            LeftDock.Width = 150;

			m_TextOutput = new Alt.GUI.Temporary.Gwen.Control.ListBox(BottomDock);
            m_TextOutput.ShouldCacheToTexture = true;

            m_Button = BottomDock.TabControl.AddPage("Output", m_TextOutput);
            m_Button.ShouldCacheToTexture = true;
            BottomDock.Height = 120;

			m_StatusBar = new Alt.GUI.Temporary.Gwen.Control.StatusBar(this);
            m_StatusBar.Dock = Alt.GUI.Temporary.Gwen.Pos.Bottom;
            //m_StatusBar.ShouldCacheToTexture = true;
            m_StatusBar.IsHidden = !showStatusBar;

			m_DebugCheck = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(m_StatusBar);
            m_DebugCheck.Text = "Debug outlines";
            m_DebugCheck.CheckChanged += DebugCheckChanged;
            m_StatusBar.AddControl(m_DebugCheck, true);

            m_Center = new Center(this);
            m_Center.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
            GUnit test;

            bool cat_ShouldCacheToTexture = false;
            {
                CollapsibleCategory cat = m_List.Add("Non-Interactive");
                {
                    test = new GUnit_Label(m_Center);
                    RegisterUnitTest("Label", cat, test);
                    test = new GUnit_RichLabel(m_Center);
                    RegisterUnitTest("RichLabel", cat, test);
                    test = new GUnit_GroupBox(m_Center);
                    RegisterUnitTest("GroupBox", cat, test);
                    test = new GUnit_ProgressBar(m_Center);
                    RegisterUnitTest("ProgressBar", cat, test);
                    test = new GUnit_ImagePanel(m_Center);
                    RegisterUnitTest("ImagePanel", cat, test);
                    test = new GUnit_StatusBar(m_Center);
                    RegisterUnitTest("StatusBar", cat, test);
                }
                cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;
            }

            {
                CollapsibleCategory cat = m_List.Add("Standard");
                {
                    test = new GUnit_Button(m_Center);
                    RegisterUnitTest("Button", cat, test);
                    test = new GUnit_TextBox(m_Center);
                    RegisterUnitTest("TextBox", cat, test);
                    test = new GUnit_CheckBox(m_Center);
                    RegisterUnitTest("CheckBox", cat, test);
                    test = new GUnit_RadioButton(m_Center);
                    RegisterUnitTest("RadioButton", cat, test);
                    test = new GUnit_ComboBox(m_Center);
                    RegisterUnitTest("ComboBox", cat, test);
                    test = new GUnit_ListBox(m_Center);
                    RegisterUnitTest("ListBox", cat, test);
                    test = new GUnit_NumericUpDown(m_Center);
                    RegisterUnitTest("NumericUpDown", cat, test);
                    test = new GUnit_Slider(m_Center);
                    RegisterUnitTest("Slider", cat, test);
                    test = new GUnit_MenuStrip(m_Center);
                    RegisterUnitTest("MenuStrip", cat, test);
                    test = new GUnit_CrossSplitter(m_Center);
                    RegisterUnitTest("CrossSplitter", cat, test);
                }
                cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;
            }
            
            {
                CollapsibleCategory cat = m_List.Add("Containers");
                {
                    test = new GUnit_Window(m_Center);
                    RegisterUnitTest("Window", cat, test);
                    test = new GUnit_TreeControl(m_Center);
                    RegisterUnitTest("TreeControl", cat, test);
                    test = new GUnit_Properties(m_Center);
                    RegisterUnitTest("Properties", cat, test);
                    test = new GUnit_TabControl(m_Center);
                    RegisterUnitTest("TabControl", cat, test);
                    test = new GUnit_ScrollControl(m_Center);
                    RegisterUnitTest("ScrollControl", cat, test);
                    test = new GUnit_Docking(m_Center);
                    RegisterUnitTest("Docking", cat, test);
                }
                cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;
            }
            
            {
                CollapsibleCategory cat = m_List.Add("Non-standard");
                {
                    test = new GUnit_CollapsibleList(m_Center);
                    RegisterUnitTest("CollapsibleList", cat, test);
                    test = new GUnit_ColorPickers(m_Center);
                    RegisterUnitTest("Color pickers", cat, test);
                    test = new GUnit_PictureBox(m_Center);
					RegisterUnitTest("PictureBox", cat, test);
                }
                cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;
            }

            m_StatusBar.SendToBack();
			PrintText("AltGUI.Temporary.Gwen Unit Test started!");
        }


        public void RegisterUnitTest(String name, CollapsibleCategory cat, GUnit test)
        {
			Alt.GUI.Temporary.Gwen.Control.Button btn = cat.Add(name);
            test.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
            test.Hide();
            test.UnitTest = this;
            btn.UserData = test;
            btn.Clicked += OnCategorySelect;
        }


        private void DebugCheckChanged(Base control)
        {
            if (m_DebugCheck.IsChecked)
                m_Center.DrawDebugOutlines = true;
            else
                m_Center.DrawDebugOutlines = false;
            Invalidate();
        }


        private void OnCategorySelect(Base control)
        {
            if (m_LastControl != null)
            {
                m_LastControl.Hide();
            }
            Base test = control.UserData as Base;
            test.Show();
            m_LastControl = test;
        }


        public void PrintText(String str)
        {
            m_TextOutput.AddRow(str);
            m_TextOutput.ScrollToBottom();
        }


        protected override void Layout(Alt.GUI.Temporary.Gwen.Skin.Base skin)
        {
            base.Layout(skin);
        }


        protected override void Render(Alt.GUI.Temporary.Gwen.Skin.Base skin)
        {
			m_StatusBar.Text = String.Format("AltGUI.Temporaty.Gwen Unit Test - {0:F0} fps. {1}", Fps, Note);

            base.Render(skin);
        }
    }
}
