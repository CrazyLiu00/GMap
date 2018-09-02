//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;


namespace Alt.GUI.Demo.Gwen.UnitTest
{
    public class GUnit_TextBox : GUnit
    {
        Font m_Font1;
        Font m_Font2;
        Font m_Font3;


        public GUnit_TextBox(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int row = 0;

            double fontScale = 1;
#if SILVERLIGHT
            fontScale = 0.8;
#endif

            m_Font1 = new Font("Consolas", System.Math.Round(14 * fontScale)); // fixed width font!
            m_Font2 = new Font("Impact", System.Math.Round(50 * fontScale));
            m_Font3 = new Font("Arial", System.Math.Round(14 * fontScale));

            {
				Alt.GUI.Temporary.Gwen.Control.TextBox textBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(this);
                textBox.SetText("Type something here");
                textBox.SetPosition(10, 10 + 25 * row);
                textBox.TextChanged += OnEdit;
                textBox.SubmitPressed += OnSubmit;
                row++;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.TextBoxPassword textBox = new Alt.GUI.Temporary.Gwen.Control.TextBoxPassword(this);
                //textBox.MaskCharacter = '@';
                textBox.SetText("secret");
                textBox.TextChanged += OnEdit;
                textBox.SetPosition(10, 10 + 25 * row);
                row++;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.TextBox textBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(this);
                textBox.SetText("Select All Text On Focus");
                textBox.SetPosition(10, 10 + 25 * row);
                textBox.SelectAllOnFocus = true;
                row++;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.TextBox textBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(this);
                textBox.SetText("Different Coloured Text, for some reason");
                textBox.TextColor = Color.ForestGreen;
                textBox.SetPosition(10, 10 + 25 * row);
                row++;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.TextBox textBox = new Alt.GUI.Temporary.Gwen.Control.TextBoxNumeric(this);
                textBox.SetText("200456698");
                textBox.TextColor = Color.LightCoral;
                textBox.SetPosition(10, 10 + 25 * row);
                row++;
            }

            row++;

            {
				Alt.GUI.Temporary.Gwen.Control.TextBox textBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(this);
                textBox.SetText("OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
                textBox.TextColor = Color.Black;
                textBox.SetPosition(10, 10 + 28 * row);
                textBox.Font = m_Font3;
                textBox.SizeToContents();
                row++;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.TextBox textBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(this);
                textBox.SetText("..............................................................");
                textBox.TextColor = Color.Black;
                textBox.SetPosition(10, 10 + 28 * row);
                textBox.Font = m_Font3;
                textBox.SizeToContents();
                row++;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.TextBox textBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(this);
                textBox.SetText("public override void SetText(string str, bool doEvents = true)");
                textBox.TextColor = Color.Black;
                textBox.SetPosition(10, 10 + 28 * row);
                textBox.Font = m_Font3;
                textBox.SizeToContents();
                row++;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.TextBox textBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(this);
                textBox.SetText("あおい　うみから　やってきた");
                textBox.TextColor = Color.Black;
                textBox.SetPosition(10, 10 + 28 * row);
                textBox.Font = m_Font3;
                textBox.SizeToContents();
                row++;
            }

            row++;

            {
				Alt.GUI.Temporary.Gwen.Control.TextBox textBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(this);
                textBox.SetText("OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
                textBox.TextColor = Color.Black;
                textBox.SetPosition(10, 10 + 28 * row);
                textBox.Font = m_Font1;
                textBox.SizeToContents();
                row++;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.TextBox textBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(this);
                textBox.SetText("..............................................................");
                textBox.TextColor = Color.Black;
                textBox.SetPosition(10, 10 + 28 * row);
                textBox.Font = m_Font1;
                textBox.SizeToContents();
                row++;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.TextBox textBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(this);
                textBox.SetText("public override void SetText(string str, bool doEvents = true)");
                textBox.TextColor = Color.Black;
                textBox.SetPosition(10, 10 + 28 * row);
                textBox.Font = m_Font1;
                textBox.SizeToContents();
                row++;
            }

            {
				Alt.GUI.Temporary.Gwen.Control.TextBox textBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(this);
                textBox.SetText("あおい　うみから　やってきた");
                textBox.TextColor = Color.Black;
                textBox.SetPosition(10, 10 + 28 * row);
                textBox.Font = m_Font1;
                textBox.SizeToContents();
                row++;
            }

            row++;

            {
				Alt.GUI.Temporary.Gwen.Control.TextBox textBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(this);
                textBox.SetText("Different Font (autosized)");
                textBox.SetPosition(10, 10 + 28 * row);
                textBox.Font = m_Font2;
                textBox.SizeToContents();

                row += 2;
            }
        }


        public override void Dispose()
        {
            if (m_Font1 != null)
            {
                m_Font1.Dispose();
                m_Font1 = null;
            }

            if (m_Font2 != null)
            {
                m_Font2.Dispose();
                m_Font2 = null;
            }

            if (m_Font3 != null)
            {
                m_Font3.Dispose();
                m_Font3 = null;
            }

            base.Dispose();
        }


        void OnEdit(Base control)
        {
			Alt.GUI.Temporary.Gwen.Control.TextBox box = control as Alt.GUI.Temporary.Gwen.Control.TextBox;
            UnitPrint(String.Format("TextBox: OnEdit: {0}", box.Text));
        }


        void OnSubmit(Base control)
        {
			Alt.GUI.Temporary.Gwen.Control.TextBox box = control as Alt.GUI.Temporary.Gwen.Control.TextBox;
            UnitPrint(String.Format("TextBox: OnSubmit: {0}", box.Text));
        }
    }
}
