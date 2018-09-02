//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
    public abstract class Example__Base : Base
    {
        internal ExamplesHolder Holder;


        protected internal virtual SizeI LogoRightTopOffset
        {
            get
            {
                return SizeI.Zero;
            }
        }


        internal Example__Base() :
            this(null)
        {
        }

        public Example__Base(Base parent) :
            base(parent)
        {
            Margin = new Margin(2);

            MouseInputEnabled = true;
            KeyboardInputEnabled = true;
        }


        public void UnitPrint(String str)
        {
            if (Holder != null)
            {
                Holder.PrintText(str);
            }
        }


        protected internal virtual void OnActivate()
        {
        }


        protected internal virtual void OnDeactivate()
        {
        }


        protected internal virtual void OnTick(double delta)
        {
        }
    }



    public abstract class Example__Base_Multi : Example__Base
    {
        class ExampleNode
        {
            Type m_ExampleType;
            public string m_Description;


            public ExampleNode(Type type, string description)
            {
                m_ExampleType = type;
                m_Description = description;
            }


            public Example__Base CreateExample(Base parent)
            {
                if (m_ExampleType != null)
                {
                    if (m_ExampleType.IsSubclassOf(typeof(Example__Base)))
                    {
                        Example__Base example = null;
                        try
                        {
                            example = (Example__Base)Activator.CreateInstance(m_ExampleType, new object[] { parent });
                        }
                        catch
                        {
                            try
                            {
                                example = (Example__Base)Activator.CreateInstance(m_ExampleType);
                            }
                            catch
                            {
                                example = null;
                            }
                        }

                        if (example != null)
                        {
                            example.Parent = parent;
                            example.Dock = Pos.Fill;

                            return example;
                        }
                    }
                }

                return null;
            }
        }



        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                SizeI offset = SizeI.Empty;

                if (m_Caption != null)
                {
                    offset.Height = m_Caption.Height + m_Caption.Margin.Height;
                }

                if (m_LastControl != null)
                {
                    offset += m_LastControl.LogoRightTopOffset;
                }

                return offset;
            }
        }


        Example__Base m_LastControl;

		Alt.GUI.Temporary.Gwen.Control.Base m_ExamplePanel;
		Alt.GUI.Temporary.Gwen.Control.Label m_Caption;
		Alt.GUI.Temporary.Gwen.Control.TreeControl m_ExamplesTreeView;
		Alt.GUI.Temporary.Gwen.Control.VerticalSplitter m_Splitter;
        readonly Color GroupColor = Color.LightGreen;
        readonly Color NormalColor = Color.White;
        readonly Color HoverColor = Color.Cyan;
        readonly Color SelectedColor = Color.Maroon * 1.4;

        protected string TitlePrefix = "Examples: ";


        public Example__Base_Multi(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            m_Splitter = new VerticalSplitter(this);
            m_Splitter.Dock = Pos.Fill;


            Alt.GUI.Temporary.Gwen.Control.Base leftContainer = new Alt.GUI.Temporary.Gwen.Control.Base(m_Splitter);
            leftContainer.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
            leftContainer.Margin = new Alt.GUI.Temporary.Gwen.Margin(1);

            Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(leftContainer);
            label.Margin = new Alt.GUI.Temporary.Gwen.Margin(5, 3, 5, 9);
            label.Text = "Examples";
            label.TextColor = Color.Yellow;
            label.AutoSizeToContents = true;
            label.Dock = Alt.GUI.Temporary.Gwen.Pos.Top;

            m_ExamplesTreeView = new TreeControl(leftContainer);
            m_ExamplesTreeView.Selected += NodeSelected;
            m_ExamplesTreeView.ShouldDrawBackground = false;
            m_ExamplesTreeView.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;


            Base rightPanel = new Base(m_Splitter);
            m_Splitter.SetPanel(0, leftContainer);
            m_Splitter.SetPanel(1, rightPanel);
            m_Splitter.SetHValue(0.3f);


            //  Caption
			m_Caption = new Alt.GUI.Temporary.Gwen.Control.Label(rightPanel);
            m_Caption.Dock = Alt.GUI.Temporary.Gwen.Pos.Top;
            m_Caption.AutoSizeToContents = true;
            m_Caption.TextColor = Color.Cyan;
            m_Caption.Margin = new Alt.GUI.Temporary.Gwen.Margin(2, 3, 10, 5);
            m_Caption.Text = "";


            //  ZedGraphPanel
            m_ExamplePanel = new Base(rightPanel);
            m_ExamplePanel.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;


            RegisterExamples();
            m_ExamplesTreeView.ExpandAll();
        }


        protected void SplitterSetHValue(double value)
        {
            m_Splitter.SetHValue((float)value);
        }


        void NodeSelected(Base control)
        {
			Alt.GUI.Temporary.Gwen.Control.TreeNode node = control as Alt.GUI.Temporary.Gwen.Control.TreeNode;

            if (node != null &&
                node.UserData != null)
            {
                ExampleNode example = node.UserData as ExampleNode;
                if (example != null)
                {
                    if (m_LastControl != null)
                    {
                        if (m_LastControl != null)
                        {
                            if (m_ExamplePanel != null)
                            {
                                m_ExamplePanel.RemoveChild(m_LastControl, false);
                            }

                            try
                            {
                                m_LastControl.Dispose();
                                m_LastControl = null;
                            }
                            catch
                            {
                            }
                        }
                    }

                    m_LastControl = example.CreateExample(m_ExamplePanel);
                    m_Caption.Text = TitlePrefix + (string.IsNullOrEmpty(example.m_Description) ? node.Text : example.m_Description);
                }
            }
        }

        protected void Start(string name)
        {
            Start(name, null, null);
        }

        protected void Start(string name, string subCategory, string category)
        {
            bool inCat = !string.IsNullOrEmpty(category);
            bool inSub = !string.IsNullOrEmpty(subCategory);

            if (!string.IsNullOrEmpty(name))
            {
				foreach (Alt.GUI.Temporary.Gwen.Control.TreeNode node in m_ExamplesTreeView.Children)
                {
                    if (node != null)
                    {
                        if (inCat)
                        {
                            if (!string.Equals(node.Text, category))
                            {
                                continue;
                            }
                        }

                        if (!inCat &&
                            !inSub &&
                            node.IsSelectable &&
                            string.Equals(node.Text, name))
                        {
                            NodeSelected(node);
                            return;
                        }
                        else if (!node.IsSelectable)
                        {
							foreach (Alt.GUI.Temporary.Gwen.Control.TreeNode subNode in node.Children)
                            {
                                if (subNode != null)
                                {
                                    if (inSub)
                                    {
                                        if (!string.Equals(subNode.Text, subCategory))
                                        {
                                            continue;
                                        }
                                    }

                                    if (!inSub &&
                                        subNode.IsSelectable)
                                    {
                                        if (string.Equals(subNode.Text, name))
                                        {
                                            NodeSelected(subNode);
                                            return;
                                        }
                                    }
                                    else if (!subNode.IsSelectable)
                                    {
										foreach (Alt.GUI.Temporary.Gwen.Control.TreeNode n in subNode.Children)
                                        {
                                            if (n != null &&
                                                n.IsSelectable &&
                                                string.Equals(n.Text, name))
                                            {
                                                NodeSelected(n);
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        protected void RegisterExampleCategory(string category)
        {
            if (!string.IsNullOrEmpty(category))
            {
				foreach (Alt.GUI.Temporary.Gwen.Control.TreeNode node in m_ExamplesTreeView.Children)
                {
                    if (node != null &&
                        string.Equals(node.Text, category))
                    {
                        return;
                    }
                }
            }

			Alt.GUI.Temporary.Gwen.Control.TreeNode categoryNode = m_ExamplesTreeView.AddNode(category);
            categoryNode.NormalTextColor = GroupColor;
            categoryNode.HoverTextColor = HoverColor;
            categoryNode.IsSelectable = false;
        }


        protected void RegisterExampleSubCategory(string subCategory, string category)
        {
			Alt.GUI.Temporary.Gwen.Control.TreeNode categoryNode = null;
            if (!string.IsNullOrEmpty(category))
            {
				foreach (Alt.GUI.Temporary.Gwen.Control.TreeNode node in m_ExamplesTreeView.Children)
                {
                    if (node != null &&
                        string.Equals(node.Text, category))
                    {
                        categoryNode = node;
                        break;
                    }
                }
            }
            if (categoryNode == null)
            {
                return;
            }

			Alt.GUI.Temporary.Gwen.Control.TreeNode subCategoryNode = categoryNode.AddNode(subCategory);
            subCategoryNode.NormalTextColor = GroupColor;
            subCategoryNode.HoverTextColor = HoverColor;
            subCategoryNode.IsSelectable = false;
        }


        protected void RegisterExample(string name, Type exampleType)
        {
            RegisterExample(name, string.Empty, exampleType);
        }

        protected void RegisterExample(string name, Type exampleType, string description)
        {
            RegisterExample(name, string.Empty, exampleType, description);
        }

        protected void RegisterExample(string name, string category, Type exampleType)
        {
            RegisterExample(name, category, exampleType, null);
        }

        protected void RegisterExample(string name, string category, Type exampleType, string description)
        {
            RegisterExample(name, null, category, exampleType, description);
        }

        protected void RegisterExample(string name, string subCategory, string category, Type exampleType)
        {
            RegisterExample(name, subCategory, category, exampleType, null);
        }

        protected void RegisterExample(string name, string subCategory, string category, Type exampleType, string description)
        {
            bool inSub = !string.IsNullOrEmpty(subCategory);

			Alt.GUI.Temporary.Gwen.Control.TreeNode parent = m_ExamplesTreeView;
            if (!string.IsNullOrEmpty(category))
            {
				foreach (Alt.GUI.Temporary.Gwen.Control.TreeNode node in m_ExamplesTreeView.Children)
                {
                    if (node != null &&
                        string.Equals(node.Text, category))
                    {
                        parent = node;

                        if (!inSub)
                        {
                            break;
                        }

						foreach (Alt.GUI.Temporary.Gwen.Control.TreeNode subNode in node.Children)
                        {
                            if (subNode != null &&
                                string.Equals(subNode.Text, subCategory))
                            {
                                parent = subNode;
                                break;
                            }
                        }

                        break;
                    }
                }
            }

			Alt.GUI.Temporary.Gwen.Control.TreeNode exampleNode = parent.AddNode(name);
            exampleNode.NormalTextColor = NormalColor;
            exampleNode.SelectedTextColor = SelectedColor;
            exampleNode.HoverTextColor = HoverColor;

            exampleNode.UserData = new ExampleNode(exampleType, description);
        }


        protected virtual void RegisterExamples()
        {
        }
    }
}
