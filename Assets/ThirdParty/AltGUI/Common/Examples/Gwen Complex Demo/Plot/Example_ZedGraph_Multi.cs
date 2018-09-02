//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Text;

using Alt.Collections;
using Alt.ComponentModel;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.ZedGraph.Temporary.Gwen.Demo;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
    public class Example_ZedGraph_Multi : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 0;
                int y = 0;

                if (m_Caption != null)
                {
                    y = m_Caption.Height + m_Caption.Margin.Height;
                }

                return new SizeI(x, y);
            }
        }

        
		Alt.GUI.Temporary.Gwen.Control.Base m_ZedGraphPanel;
		Alt.GUI.Temporary.Gwen.Control.Label m_Caption;
		Alt.GUI.Temporary.Gwen.Control.Label m_InfoBox;
		Alt.GUI.Temporary.Gwen.Control.TreeControl m_ExamplesTreeView;
        readonly Color GroupColor = Color.LightGreen;
        readonly Color NormalColor = Color.White;
        readonly Color HoverColor = Color.Cyan;
        readonly Color SelectedColor = Color.Maroon * 1.4;

		const string TitlePrefix = "AltGUI.ZedGraph Demos: ";
        Hashtable demos;
        Hashtable typeToNodeTable;


        public Example_ZedGraph_Multi(Base parent)
            : base(parent)
        {
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            VerticalSplitter splitter = new VerticalSplitter(this);
            splitter.Dock = Pos.Fill;


            Alt.GUI.Temporary.Gwen.Control.Base leftContainer = new Alt.GUI.Temporary.Gwen.Control.Base(splitter);
            leftContainer.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
            leftContainer.Margin = new Alt.GUI.Temporary.Gwen.Margin(1);

            Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(leftContainer);
            label.Margin = new Alt.GUI.Temporary.Gwen.Margin(5, 3, 5, 9);
			label.Text = "AltGUI.ZedGraph Demos";
            label.TextColor = Color.Yellow;
            label.AutoSizeToContents = true;
            label.Dock = Alt.GUI.Temporary.Gwen.Pos.Top;

            m_ExamplesTreeView = new TreeControl(leftContainer);
            m_ExamplesTreeView.Selected += NodeSelected;
            m_ExamplesTreeView.ShouldDrawBackground = false;
            m_ExamplesTreeView.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;


            Base rightPanel = new Base(splitter);
            splitter.SetPanel(0, leftContainer);
            splitter.SetPanel(1, rightPanel);
            splitter.SetHValue(0.3f);


            //  Caption
			m_Caption = new Alt.GUI.Temporary.Gwen.Control.Label(rightPanel);
            m_Caption.Dock = Alt.GUI.Temporary.Gwen.Pos.Top;
            m_Caption.AutoSizeToContents = true;
            m_Caption.TextColor = Color.Cyan;
            m_Caption.Margin = new Alt.GUI.Temporary.Gwen.Margin(2, 3, 10, 5);
            m_Caption.Text = "";


            //  InfoBox
			m_InfoBox = new Alt.GUI.Temporary.Gwen.Control.Label(rightPanel);
            m_InfoBox.Dock = Alt.GUI.Temporary.Gwen.Pos.Bottom;
            m_InfoBox.AutoSizeToContents = true;
            m_InfoBox.TextColor = Color.LightBlue;
            m_InfoBox.Margin = new Margin(10, 10, 10, 10);
            m_InfoBox.Text = "";


            //  ZedGraphPanel
            m_ZedGraphPanel = new Base(rightPanel);
            m_ZedGraphPanel.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;


            demos = new Hashtable();
            typeToNodeTable = new Hashtable();

            BuildPrimaryTree();

            LoadDemos();
            m_ExamplesTreeView.ExpandAll();


            //Init("Combo Demo");
            Init("Initial Sample");
        }


        string TypeToName(DemoType type)
        {
            switch (type)
            {
                case DemoType.Pie:
                    return "Pie";
                case DemoType.General:
                default:
                    return "General";
                case DemoType.Bar:
                    return "Bar";
                case DemoType.Line:
                    return "Line";
                case DemoType.Special:
                    return "Special Features";
                case DemoType.Tutorial:
                    return "Tutorial";
            }
        }


        void BuildPrimaryTree()
        {
            //foreach (string name in Enum.GetNames(typeof(DemoType)))
            foreach (string name in EnumHelper.GetNames<DemoType>())
            {
                DemoType type = (DemoType)EnumHelper.Parse(typeof(DemoType), name);

				Alt.GUI.Temporary.Gwen.Control.TreeNode node = m_ExamplesTreeView.AddNode(TypeToName(type));

                node.NormalTextColor = GroupColor;
                node.HoverTextColor = HoverColor;
                
                node.IsSelectable = false;

                typeToNodeTable[type] = node;
            }
        }


        void NodeSelected(Base control)
        {
			Alt.GUI.Temporary.Gwen.Control.TreeNode node = control as Alt.GUI.Temporary.Gwen.Control.TreeNode;

            if (demos[node.Text] != null)
            {
                Init(node.Text);
            }
        }


        void Init(object key)
        {
            ZedGraphDemo demo = (ZedGraphDemo)this.demos[key];

            if (demo == null)
            {
                return;
            }

            m_ZedGraphPanel.DeleteAllChildren(false);

            demo.Initialize(m_ZedGraphPanel);

            demo.ZedGraphControl.Parent = null;
            demo.ZedGraphControl.Parent = m_ZedGraphPanel;

            demo.ZedGraphControl.Size = m_ZedGraphPanel.InnerBounds.Size;
            demo.ZedGraphControl.Dock = Pos.Fill;

            m_Caption.Text = TitlePrefix + demo.Title;

            m_InfoBox.Text = demo.Description;
            m_InfoBox.IsHidden = string.IsNullOrEmpty(m_InfoBox.Text);

            // tell the control to rescale itself
            demo.ZedGraphControl.AxisChange();
        }


        void LoadDemo(ZedGraphDemo demo)
        {
            foreach (DemoType type in demo.Types)
            {
				Alt.GUI.Temporary.Gwen.Control.TreeNode typeNode = (Alt.GUI.Temporary.Gwen.Control.TreeNode)typeToNodeTable[type];
                if (typeNode == null)
                {
                    // error, this shouldn't be reached
                    // TODO: do something about this
                }
                else
                {
					Alt.GUI.Temporary.Gwen.Control.TreeNode node = typeNode.AddNode(demo.Title);
                    node.NormalTextColor = NormalColor;
                    node.SelectedTextColor = SelectedColor;
                    node.HoverTextColor = HoverColor;
                }
            }
            // store the demo based on it's title
            demos[demo.Title] = demo;
        }


        void LoadDemos()
        {
            LoadDemo(new ComboDemo());
            LoadDemo(new PieChartDemo());
            LoadDemo(new LineStackingDemo());
            LoadDemo(new TransparentDemo1());
            LoadDemo(new LineGraphBandDemo());
            LoadDemo(new FilledBarGraphDemo());
            LoadDemo(new SineBarGraphDemo());
            LoadDemo(new MasterPaneDemo());
            LoadDemo(new MultiPieChartDemo());
            LoadDemo(new GradientByValueDemo());
            LoadDemo(new HiLowCloseDemo());
            LoadDemo(new HiLowBarDemo());
            LoadDemo(new StickItemDemo());
            LoadDemo(new HorizontalBarDemo());
            LoadDemo(new MultiYDemo());
            LoadDemo(new DualYDemo());
            LoadDemo(new FilledCurveDemo());
            LoadDemo(new SampleMultiPointListDemo());
            LoadDemo(new ErrorBarDemo());
            LoadDemo(new OverlayBarDemo());
            LoadDemo(new SortedOverlayBarDemo());
            LoadDemo(new CrossLineDemo());
            LoadDemo(new StepChartDemo());
            LoadDemo(new SmoothChartDemo());
            LoadDemo(new BaseTicDemo());
            LoadDemo(new HorizontalStackedBarDemo());
            LoadDemo(new VerticalBarsWithLabelsDemo());
            LoadDemo(new HorizontalBarsWithLabelsDemo());
            LoadDemo(new StackedBarsWithLabelsDemo());

            LoadDemo(new InitialSampleDemo());
            LoadDemo(new ModInitialSampleDemo());
            LoadDemo(new DateAxisSampleDemo());
            LoadDemo(new TextAxisSampleDemo());
            LoadDemo(new BarChartSampleDemo());
            LoadDemo(new HorizontalBarSampleDemo());
            LoadDemo(new StackedBarSampleDemo());
            LoadDemo(new PercentStkBarDemo());
            LoadDemo(new PieSampleDemo());
            LoadDemo(new MasterSampleDemo());
            LoadDemo(new SynchronizedPanes());
            LoadDemo(new JapaneseCandleStickDemo());
            LoadDemo(new OHLCBarDemo());
            LoadDemo(new ImageForSymbolDemo());
            LoadDemo(new MasterPaneLayout());
        }
    }
}
