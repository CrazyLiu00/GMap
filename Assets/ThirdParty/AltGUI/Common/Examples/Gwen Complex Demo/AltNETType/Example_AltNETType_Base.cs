//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.NETType;
using Alt.Sketch;
using Alt.Sketch.Geometries;
using Alt.Sketch.Geometries.Reformers;
using Alt.Sketch.Geometries.Transformers;


namespace Alt.GUI.Demo
{
    class Example_AltNETType_Base : Example__Base
    {
		Alt.GUI.Temporary.Gwen.Control.Base m_FontPanel;
		Alt.GUI.Temporary.Gwen.Control.ComboBox m_FontFamily;
		Alt.GUI.Temporary.Gwen.Control.ComboBox m_FontStyle;
		Dictionary<string, Alt.GUI.Temporary.Gwen.Control.MenuItem> m_FamilyMenus = new Dictionary<string, Alt.GUI.Temporary.Gwen.Control.MenuItem>();
		Dictionary<FontStyle, Alt.GUI.Temporary.Gwen.Control.MenuItem> m_FontStyleMenus = new Dictionary<FontStyle, Alt.GUI.Temporary.Gwen.Control.MenuItem>();


        protected string FontFileName
        {
            get
            {
                string filename = null;

                if (m_FontFamily != null)
                {
					Alt.GUI.Temporary.Gwen.Control.MenuItem label = m_FontFamily.SelectedItem;
                    if (label != null)
                    {
                        FontFamily family = label.Tag as FontFamily;
                        if (family != null)
                        {
                            //int n = int.Parse(m_FontStyle.SelectedItem.Name, System.Globalization.CultureInfo.InvariantCulture);
                            FontStyle style = FontStyle.Regular;
                            if (m_FontStyle != null)
                            {
								Alt.GUI.Temporary.Gwen.Control.MenuItem item = m_FontStyle.SelectedItem;
                                if (item != null)
                                {
                                    style = (FontStyle)m_FontStyle.SelectedItem.Tag;
                                }
                            }

                            filename = family.GetFileName(style);
                        }
                    }
                }

                return filename;
            }
        }


        protected Example_AltNETType_Base(Base parent)
            : base(parent)
        {
        }


        protected void CreateFontPanel()
        {
            //  Font
            m_FontPanel = new Base(this);
            m_FontPanel.Dock = Pos.Bottom;
            m_FontPanel.Margin = new Margin(0, 10, 0, 0);


            //  Font Family
			Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(m_FontPanel);
            label.AutoSizeToContents = true;
            label.Dock = Pos.Top;
            label.Margin = new Margin(0, 0, 0, 3);
            label.Text = "Font Family:";


            FontFamily[] families = (new InstalledFontCollection()).Families;

			m_FontFamily = new Alt.GUI.Temporary.Gwen.Control.ComboBox(m_FontPanel);
            m_FontFamily.Dock = Pos.Top;
            m_FontFamily.Margin = new Margin(0, 0, 0, 7);

            foreach (FontFamily family in families)
            {
				Alt.GUI.Temporary.Gwen.Control.MenuItem item = m_FontFamily.AddItem(family.Name);
                item.Tag = family;

                m_FamilyMenus.Add(family.Name, item);
            }

            m_FontFamily.ItemSelected += OnFontFamilySelect;


            //  Font Style
			label = new Alt.GUI.Temporary.Gwen.Control.Label(m_FontPanel);
            label.AutoSizeToContents = true;
            label.Dock = Pos.Top;
            label.Margin = new Margin(0, 0, 0, 3);
            label.Text = "Font Style:";


			m_FontStyle = new Alt.GUI.Temporary.Gwen.Control.ComboBox(m_FontPanel);
            m_FontStyle.Dock = Pos.Top;


            m_FontStyle.ItemSelected += OnFontStyleSelect;


            //  Set font style
            OnFontFamilySelect(null);
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            //  Font
            m_FontPanel.Height = (m_FontStyle.Location + m_FontStyle.Size).Y;
        }


        void OnFontStyleSelect(Base control)
        {
            ClientRefresh();
        }


		Alt.GUI.Temporary.Gwen.Control.MenuItem AddFontStyle(FontFamily family, FontStyle style)
        {
            if (!family.IsStyleAvailable(style))
            {
                return null;
            }

			Alt.GUI.Temporary.Gwen.Control.MenuItem item = m_FontStyle.AddItem(style.ToString());
            item.Tag = style;

            m_FontStyle.SelectedItem = item;

            m_FontStyleMenus.Add(style, item);

            return item;
        }

        void OnFontFamilySelect(Base control)
        {
            m_FontStyle.DeleteAll();
            m_FontStyleMenus.Clear();

			Alt.GUI.Temporary.Gwen.Control.MenuItem label = m_FontFamily.SelectedItem;
            if (label == null)
            {
                return;
            }

            FontFamily family = label.Tag as FontFamily;
            if (family == null)
            {
                return;
            }


            AddFontStyle(family, FontStyle.Bold);
            AddFontStyle(family, FontStyle.BoldItalic);
            AddFontStyle(family, FontStyle.Italic);
            AddFontStyle(family, FontStyle.Regular);


            ClientRefresh();
        }


        protected virtual void ClientRefresh()
        {
        }


        protected bool SelectFontFamily(string family)
        {
			Alt.GUI.Temporary.Gwen.Control.MenuItem item;
            if (m_FamilyMenus.TryGetValue(family, out item))
            {
                m_FontFamily.SelectedItem = item;

                return true;
            }

            return false;
        }


        protected bool SelectFontStyle(FontStyle style)
        {
			Alt.GUI.Temporary.Gwen.Control.MenuItem item;
            if (m_FontStyleMenus.TryGetValue(style, out item))
            {
                m_FontStyle.SelectedItem = item;

                return true;
            }

            return false;
        }
    }
}
