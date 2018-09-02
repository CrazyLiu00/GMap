//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_CollapsibleList : GUnit
    {
        public GUnit_CollapsibleList(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            CollapsibleList control = new CollapsibleList(this);
            //control.ClientBackColor = Color.CornflowerBlue;
            control.SetSize(100, 200);
            control.SetPosition(10, 10);
            control.ItemSelected += OnSelection;
            control.CategoryCollapsed += OnCollapsed;

            {
                CollapsibleCategory cat = control.Add("Category One");
                cat.TextColor = Color.LightGreen;
                cat.UseCurrentColorAsNormal = true;
                cat.Add("Hello");
                cat.Add("Two");
                cat.Add("Three");
                cat.Add("Four");
            }

            {
                CollapsibleCategory cat = control.Add("Shopping");
                cat.TextColor = Color.Yellow;
                cat.UseCurrentColorAsNormal = true;
                cat.Add("Special");
                cat.Add("Two Noses");
                cat.Add("Orange ears");
                cat.Add("Beer");
                cat.Add("Three Eyes");
                cat.Add("Special");
                cat.Add("Two Noses");
                cat.Add("Orange ears");
                cat.Add("Beer");
                cat.Add("Three Eyes");
                cat.Add("Special");
                cat.Add("Two Noses");
                cat.Add("Orange ears");
                cat.Add("Beer");
                cat.Add("Three Eyes");
            }

            {
                CollapsibleCategory cat = control.Add("Category Two");
                cat.TextColor = Color.Pink;
                cat.UseCurrentColorAsNormal = true;
                cat.Add("Hello 2");
                cat.Add("Two 2");
                cat.Add("Three 2");
                cat.Add("Four 2");
            }
        }


        void OnSelection(Base control)
        {
            CollapsibleList list = control as CollapsibleList;
            UnitPrint(String.Format("CollapsibleList: Selected: {0}", list.GetSelectedButton().Text));
        }


        void OnCollapsed(Base control)
        {
            CollapsibleCategory cat = control as CollapsibleCategory;
            UnitPrint(String.Format("CollapsibleCategory: CategoryCollapsed: {0} {1}", cat.Text, cat.IsCollapsed));
        }
    }
}
