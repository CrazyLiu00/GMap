//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_ComboBox : GUnit
    {
        public GUnit_ComboBox(Base parent)
            : base(parent)
        {
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            {
				Alt.GUI.Temporary.Gwen.Control.ComboBox combo = new Alt.GUI.Temporary.Gwen.Control.ComboBox(this);
                combo.SetPosition(50, 50);
                combo.Width = 200;

                combo.AddItem("Option One", "one");
                combo.AddItem("Number Two", "two");
                combo.AddItem("Door Three", "three");
                combo.AddItem("Four Legs", "four");
                combo.AddItem("Five Birds", "five");

                combo.ItemSelected += OnComboSelect;
            }

            {
                // Empty..
				Alt.GUI.Temporary.Gwen.Control.ComboBox combo = new Alt.GUI.Temporary.Gwen.Control.ComboBox(this);
                combo.SetPosition(50, 80);
                combo.Width = 200;
            }

            {
                // Empty..
				Alt.GUI.Temporary.Gwen.Control.ComboBox combo = new Alt.GUI.Temporary.Gwen.Control.ComboBox(this);
                combo.SetPosition(50, 110);
                combo.Width = 200;

                for (int i = 0; i < 500; i++)
                {
                    combo.AddItem(String.Format("Option {0}", i));
                }

                combo.ItemSelected += OnComboSelect;
            }
        }


        void OnComboSelect(Base control)
        {
			Alt.GUI.Temporary.Gwen.Control.ComboBox combo = control as Alt.GUI.Temporary.Gwen.Control.ComboBox;
            UnitPrint(String.Format("ComboBox: OnComboSelect: {0}", combo.SelectedItem.Text));
        }
    }
}
