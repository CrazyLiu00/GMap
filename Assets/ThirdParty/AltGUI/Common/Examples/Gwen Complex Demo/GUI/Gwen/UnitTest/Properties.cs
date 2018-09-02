//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_Properties : GUnit
    {
        public GUnit_Properties(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            {
                Properties props = new Properties(this);
                props.ValueChanged += OnChanged;

                props.SetBounds(10, 10, 150, 300);

                {
                    {
                        //NoNeed	PropertyRow pRow =
						props.Add("First Name");
                    }

                    props.Add("Middle Name");
                    props.Add("Last Name");
                }
            }

            {
                PropertyTree ptree = new PropertyTree(this);
                ptree.SetBounds(200, 10, 200, 200);

                {
                    Properties props = ptree.Add("Item One");
                    props.ValueChanged += OnChanged;

                    props.Add("Middle Name");
                    props.Add("Last Name");
                    props.Add("Four");
                }

                {
                    Properties props = ptree.Add("Item Two");
                    props.ValueChanged += OnChanged;
                    
                    props.Add("More Items");
                    props.Add("Bacon", new Alt.GUI.Temporary.Gwen.Control.Property.Check(props), "1");
                    props.Add("To Fill");
                    props.Add("Colour", new Alt.GUI.Temporary.Gwen.Control.Property.Color(props), "255 0 0");
                    props.Add("Out Here");
                }

                ptree.ExpandAll();
            }
        }


        void OnChanged(Base control)
        {
            PropertyRow row = control as PropertyRow;
            UnitPrint(String.Format("minWavelength changed: {0}", row.Value));
        }
    }
}
