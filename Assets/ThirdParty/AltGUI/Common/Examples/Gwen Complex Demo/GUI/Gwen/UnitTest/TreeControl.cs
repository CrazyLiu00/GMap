//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_TreeControl : GUnit
    {
        public GUnit_TreeControl(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            {
				Alt.GUI.Temporary.Gwen.Control.TreeControl ctrl = new Alt.GUI.Temporary.Gwen.Control.TreeControl(this);

                ctrl.AddNode("Node One");
				Alt.GUI.Temporary.Gwen.Control.TreeNode node = ctrl.AddNode("Node Two");
                node.AddNode("Node Two Inside");
                node.AddNode("Eyes");
                node.AddNode("Brown").AddNode("Node Two Inside").AddNode("Eyes").AddNode("Brown");
                node.AddNode("More");
                node.AddNode("Nodes");
                ctrl.AddNode("Node Three");

                ctrl.SetBounds(30, 30, 200, 200);
                ctrl.ExpandAll();

                ctrl.Selected += NodeSelected;
                ctrl.Expanded += NodeExpanded;
                ctrl.Collapsed += NodeCollapsed;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.TreeControl ctrl = new Alt.GUI.Temporary.Gwen.Control.TreeControl(this);

                ctrl.AllowMultiSelect = true;

                ctrl.AddNode("Node One");
				Alt.GUI.Temporary.Gwen.Control.TreeNode node = ctrl.AddNode("Node Two");
                node.AddNode("Node Two Inside");
                node.AddNode("Eyes");
				Alt.GUI.Temporary.Gwen.Control.TreeNode nodeTwo = node.AddNode("Brown").AddNode("Node Two Inside").AddNode("Eyes");
                nodeTwo.AddNode("Brown");
                nodeTwo.AddNode("Green");
                nodeTwo.AddNode("Slime");
                nodeTwo.AddNode("Grass");
                nodeTwo.AddNode("Pipe");
                node.AddNode("More");
                node.AddNode("Nodes");

                ctrl.AddNode("Node Three");

                ctrl.SetBounds(240, 30, 200, 200);
                ctrl.ExpandAll();

                ctrl.Selected += NodeSelected;
                ctrl.Expanded += NodeExpanded;
                ctrl.Collapsed += NodeCollapsed;
            }
        }

        void NodeCollapsed(Base control)
        {
			Alt.GUI.Temporary.Gwen.Control.TreeNode node = control as Alt.GUI.Temporary.Gwen.Control.TreeNode;
            UnitPrint(String.Format("Node collapsed: {0}", node.Text));
        }

        void NodeExpanded(Base control)
        {
			Alt.GUI.Temporary.Gwen.Control.TreeNode node = control as Alt.GUI.Temporary.Gwen.Control.TreeNode;
            UnitPrint(String.Format("Node expanded: {0}", node.Text));
        }

        void NodeSelected(Base control)
        {
			Alt.GUI.Temporary.Gwen.Control.TreeNode node = control as Alt.GUI.Temporary.Gwen.Control.TreeNode;
            UnitPrint(String.Format("Node selected: {0}", node.Text));
        }
    }
}
