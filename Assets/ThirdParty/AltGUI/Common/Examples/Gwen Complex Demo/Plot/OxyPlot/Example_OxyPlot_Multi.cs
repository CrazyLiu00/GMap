//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.ComponentModel;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.Temporary.Gwen.Control.OxyPlot;
using Alt.GUI.Demo.OxyPlot.ExampleLibrary;
using Alt.Sketch;
using Alt.Sketch.Ext.OxyPlot;

using OxyPlot;


namespace Alt.GUI.Demo
{
    public class Example_OxyPlot_Multi : Example__Base
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


		Alt.GUI.Temporary.Gwen.Control.Base m_OxyPlotPanel;
		Alt.GUI.Temporary.Gwen.Control.Label m_Caption;
		Alt.GUI.Temporary.Gwen.Control.Label m_Info;
		Alt.GUI.Temporary.Gwen.Control.TreeControl m_ExamplesTreeView;
        readonly Color GroupColor = Color.LightGreen;
        readonly Color NormalColor = Color.White;
        readonly Color HoverColor = Color.Cyan;
        readonly Color SelectedColor = Color.Maroon * 1.4;

		const string TitlePrefix = "AltGUI.OxyPlot Demo: ";
        Plot m_Plot;

        OxyPlot.Example_OxyPlot_Multi_ViewModel vm = new OxyPlot.Example_OxyPlot_Multi_ViewModel();


        public Example_OxyPlot_Multi(Base parent)
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
			label.Text = "AltGUI.OxyPlot Demos";
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
            splitter.SetHValue(0.3);


            //  Caption
			m_Caption = new Alt.GUI.Temporary.Gwen.Control.Label(rightPanel);
            m_Caption.Dock = Alt.GUI.Temporary.Gwen.Pos.Top;
            m_Caption.AutoSizeToContents = true;
            m_Caption.TextColor = Color.Cyan;
            m_Caption.Margin = new Alt.GUI.Temporary.Gwen.Margin(2, 3, 10, 5);
            m_Caption.Text = "";


            //  Info
			m_Info = new Alt.GUI.Temporary.Gwen.Control.Label(rightPanel);
            m_Info.Dock = Alt.GUI.Temporary.Gwen.Pos.Bottom;
            m_Info.AutoSizeToContents = true;
            m_Info.TextColor = Color.LightBlue;
            m_Info.Margin = new Alt.GUI.Temporary.Gwen.Margin(2, 10, 3, 5);
            m_Info.Text = "Use the mouse-wheel or right mouse button to manipulate with graph";


            //  ZedGraphPanel
            m_OxyPlotPanel = new Base(rightPanel);
            m_OxyPlotPanel.Dock = Alt.GUI.Temporary.Gwen.Pos.Fill;
            
            PlotModel plotModel = new PlotModel();

            plotModel.Annotations = null;
            plotModel.AutoAdjustPlotMargins = true;
            plotModel.Axes = null;
            plotModel.AxisTierDistance = 4D;
            plotModel.Background = null;
            plotModel.Culture = null;
            plotModel.DefaultColors = null;
            plotModel.DefaultFont = "Arial";//Segoe UI";
            plotModel.DefaultFontSize = 12D;
            plotModel.IsLegendVisible = true;
            plotModel.LegendBackground = null;
            plotModel.LegendBorder = null;
            plotModel.LegendBorderThickness = 1D;
            plotModel.LegendColumnSpacing = 0D;
            plotModel.LegendFont = null;
            plotModel.LegendFontSize = 12D;
            plotModel.LegendFontWeight = 400D;
            plotModel.LegendItemAlignment = global::OxyPlot.HorizontalAlignment.Left;
            plotModel.LegendItemOrder = global::OxyPlot.LegendItemOrder.Normal;
            plotModel.LegendItemSpacing = 24D;
            plotModel.LegendMargin = 8D;
            plotModel.LegendOrientation = global::OxyPlot.LegendOrientation.Vertical;
            plotModel.LegendPadding = 8D;
            plotModel.LegendPlacement = global::OxyPlot.LegendPlacement.Inside;
            plotModel.LegendPosition = global::OxyPlot.LegendPosition.RightTop;
            plotModel.LegendSymbolLength = 16D;
            plotModel.LegendSymbolMargin = 4D;
            plotModel.LegendSymbolPlacement = global::OxyPlot.LegendSymbolPlacement.Left;
            plotModel.LegendTextColor = null;
            plotModel.LegendTitle = null;
            plotModel.LegendTitleColor = null;
            plotModel.LegendTitleFont = null;
            plotModel.LegendTitleFontSize = 12D;
            plotModel.LegendTitleFontWeight = 700D;
            plotModel.PlotAreaBackground = null;
            plotModel.PlotAreaBorderColor = null;
            plotModel.PlotAreaBorderThickness = 1D;
            plotModel.PlotType = global::OxyPlot.PlotType.XY;
            plotModel.Series = null;
            plotModel.Subtitle = null;
            plotModel.SubtitleColor = null;
            plotModel.SubtitleFont = null;
            plotModel.SubtitleFontSize = 14D;
            plotModel.SubtitleFontWeight = 400D;
            plotModel.TextColor = null;
            plotModel.Title = null;
            plotModel.TitleColor = null;
            plotModel.TitleFont = null;
            plotModel.TitleFontSize = 18D;
            plotModel.TitleFontWeight = 700D;
            plotModel.TitlePadding = 6D;

            m_Plot = new Plot(rightPanel);
            m_Plot.Dock = Pos.Fill;
            m_Plot.KeyboardPanHorizontalStep = 0.1D;
            m_Plot.KeyboardPanVerticalStep = 0.1D;
            m_Plot.Model = plotModel;
            m_Plot.Name = "m_Plot";
            m_Plot.PanCursor = GUI.Cursors.Hand;
            m_Plot.ZoomHorizontalCursor = GUI.Cursors.SizeWE;
            m_Plot.ZoomRectangleCursor = GUI.Cursors.SizeNWSE;
            m_Plot.ZoomVerticalCursor = GUI.Cursors.SizeNS;


            //
            InitTree();

            //  Start example
            foreach (var ex in vm.Examples)
            {
                if (ex.Category == "HeatMapSeries" &&
                    ex.Title == "Peaks")
                {
                    vm.SelectedExample = ex;
                    InitPlot();
                }
            }
        }

        
        void InitTree()
        {
			Alt.GUI.Temporary.Gwen.Control.TreeNode node = null;
            foreach (var ex in vm.Examples)
            {
                if (node == null ||
                    node.Text != ex.Category)
                {
                    node = m_ExamplesTreeView.AddNode(ex.Category);

                    node.NormalTextColor = GroupColor;
                    node.HoverTextColor = HoverColor;

                    node.IsSelectable = false;
                }
                
				Alt.GUI.Temporary.Gwen.Control.TreeNode n = node.AddNode(ex.Title);
                n.Tag = ex;
                n.NormalTextColor = NormalColor;
                n.SelectedTextColor = SelectedColor;
                n.HoverTextColor = HoverColor;
            }
            InitPlot();
        }


        void NodeSelected(Base control)
        {
            ExampleInfo ei = control.Tag as ExampleInfo;
            if (ei == null)
            {
                return;
            }

            vm.SelectedExample = ei;
            InitPlot();
        }


        void InitPlot()
        {
            m_Plot.Model = vm.SelectedExample != null ? vm.SelectedExample.PlotModel : null;
            m_Plot.ClientBackColor = vm.PlotBackground;

            if (vm.SelectedExample != null)
            {
                m_Caption.Text = TitlePrefix + "[" + vm.SelectedExample.Category + "] " + vm.SelectedExample.Title;
            }
        }
    }
}