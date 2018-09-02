//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.Temporary.Gwen.Control.Layout;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_ListBox : GUnit
    {
        public GUnit_ListBox(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            {
				Alt.GUI.Temporary.Gwen.Control.ListBox ctrl = new Alt.GUI.Temporary.Gwen.Control.ListBox(this);
                ctrl.SetPosition(10, 10);

                ctrl.AddRow("First");
                ctrl.AddRow("Blue");
                ctrl.AddRow("Yellow");
                ctrl.AddRow("Orange");
                ctrl.AddRow("Brown");
                ctrl.AddRow("Black");
                ctrl.AddRow("Green");
                ctrl.AddRow("Dog");
                ctrl.AddRow("Cat Blue");
                ctrl.AddRow("Shoes");
                ctrl.AddRow("Shirts");
                ctrl.AddRow("Chair");
                ctrl.AddRow("I'm autosized");
                ctrl.AddRow("Last");
                
                ctrl.AllowMultiSelect = true;
                ctrl.SelectRowsByRegex("Bl.e|Dog");

                ctrl.RowSelected += RowSelected;
                ctrl.RowUnselected += RowUnSelected;
                
                ctrl.SizeToContents();
            }

            {
				Alt.GUI.Temporary.Gwen.Control.Layout.Table ctrl = new Alt.GUI.Temporary.Gwen.Control.Layout.Table(this);
                ctrl.SetPosition(120, 10);

                ctrl.AddRow("First");
                ctrl.AddRow("Blue");
                ctrl.AddRow("Yellow");
                ctrl.AddRow("Orange");
                ctrl.AddRow("Brown");
                ctrl.AddRow("Black");
                ctrl.AddRow("Green");
                ctrl.AddRow("Dog");
                ctrl.AddRow("Cat Blue");
                ctrl.AddRow("Shoes");
                ctrl.AddRow("Shirts");
                ctrl.AddRow("Chair");
                ctrl.AddRow("I'm autosized");
                ctrl.AddRow("Last");

                ctrl.SizeToContents(0);
            }

            {
				Alt.GUI.Temporary.Gwen.Control.ListBox ctrl = new Alt.GUI.Temporary.Gwen.Control.ListBox(this);
                ctrl.SetBounds(220, 10, 200, 200);
                ctrl.ColumnCount = 3;
                //ctrl.AllowMultiSelect = true;
                ctrl.RowSelected += RowSelected;
                ctrl.RowUnselected += RowUnSelected;

                {
					Alt.GUI.Temporary.Gwen.Control.Layout.TableRow row = ctrl.AddRow("Baked Beans");
                    row.SetCellText(1, "Heinz");
                    row.SetCellText(2, "£3.50");
                }

                {
					Alt.GUI.Temporary.Gwen.Control.Layout.TableRow row = ctrl.AddRow("Bananas");
                    row.SetCellText(1, "Trees");
                    row.SetCellText(2, "£1.27");
                }

                {
					Alt.GUI.Temporary.Gwen.Control.Layout.TableRow row = ctrl.AddRow("Chicken");
                    row.SetCellText(1, "\u5355\u5143\u6D4B\u8BD5");
                    row.SetCellText(2, "£8.95");
                }
            }

            {
                // fixed-size table
				Alt.GUI.Temporary.Gwen.Control.Layout.Table table = new Alt.GUI.Temporary.Gwen.Control.Layout.Table(this);
                table.SetColumnCount(3);
                table.SetBounds(450, 10, 320, 100);
                table.SetColumnWidth(0, 100);
                table.SetColumnWidth(1, 100);
                table.SetColumnWidth(2, 100);
                var row1 = table.AddRow();
                row1.SetCellText(0, "Row 1");
                row1.SetCellText(1, "R1 cell 1");
                row1.SetCellText(2, "Row 1 cell 2");

                table.AddRow().Text = "Row 2, slightly bigger";
                table[1].SetCellText(1, "Center cell");

                table.AddRow().Text = "Row 3, medium";
                table[2].SetCellText(2, "Last cell");
            }

            {
                //Control.Label outer = new Label(this);
                //outer.SetBounds(340, 140, 300, 200);

                // autosized table
                Table table = new Table(this);
                table.SetColumnCount(3);
                table.SetPosition(450, 150);

                var row1 = table.AddRow();
                row1.SetCellText(0, "Row 1");
                row1.SetCellText(1, "R1 cell 1");
                row1.SetCellText(2, "Row 1 cell 2");

                table.AddRow().Text = "Row 2, slightly bigger";
                table[1].SetCellText(1, "Center cell");

                table.AddRow().Text = "Row 3, medium";
                table[2].SetCellText(2, "Last cell");

                table.SizeToContents(0);
            }
        }

        void RowSelected(Base control)
        {
			Alt.GUI.Temporary.Gwen.Control.ListBox list = control as Alt.GUI.Temporary.Gwen.Control.ListBox;

            string list_SelectedRows_Last_Text = "";
            foreach (TableRow row in list.SelectedRows)
            {
                list_SelectedRows_Last_Text = row.Text;
            }
            UnitPrint(String.Format("ListBox: RowSelected: {0} [{1}]",
                //list.SelectedRows.Last().Text,
                list_SelectedRows_Last_Text,
                list[list.SelectedRowIndex].Text));
        }

        void RowUnSelected(Base control)
        {
            // todo: how to determine which one was unselected (store somewhere)
            // or pass row as the event param?
            //ListBox list = control as ListBox;
            UnitPrint(String.Format("ListBox: OnRowUnselected"));
        }
    }
}
