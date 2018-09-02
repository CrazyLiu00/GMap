//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.Collections;
using Alt.GUI.PieChart.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.Temporary.Gwen.ControlInternal;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
    class Example_PieChart : Example__Base
    {
        PieChartControl m_panelDrawing;

		Alt.GUI.Temporary.Gwen.Control.Base m_BottomPanel;
		Alt.GUI.Temporary.Gwen.Control.Base m_ControlsPanel;
		Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup groupBox1;
		Alt.GUI.Temporary.Gwen.Control.CheckBox checkBox1;
		Alt.GUI.Temporary.Gwen.Control.CheckBox checkBox2;
		Alt.GUI.Temporary.Gwen.Control.CheckBox checkBox3;
		Alt.GUI.Temporary.Gwen.Control.CheckBox checkBox4;
		Alt.GUI.Temporary.Gwen.Control.CheckBox checkBox5;
		Alt.GUI.Temporary.Gwen.Control.CheckBox checkBox6;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownValue1;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownValue2;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownValue3;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownValue4;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownValue5;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownValue6;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownDisplacement1;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownDisplacement2;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownDisplacement3;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownDisplacement4;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownDisplacement5;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownDisplacement6;
        ColorButton buttonColor1;
        ColorButton buttonColor2;
        ColorButton buttonColor3;
        ColorButton buttonColor4;
        ColorButton buttonColor5;
        ColorButton buttonColor6;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxText1;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxText2;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxText3;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxText5;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxText4;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxText6;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxToolTip1;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxToolTip2;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxToolTip3;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxToolTip4;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxToolTip5;
		Alt.GUI.Temporary.Gwen.Control.TextBox textBoxToolTip6;
		Alt.GUI.Temporary.Gwen.Control.GroupBox groupBox4;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownLeftMargin;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownRightMargin;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownTopMargin;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownBottomMargin;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownAngle;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownPieHeight;
		Alt.GUI.Temporary.Gwen.Control.ComboBox comboBoxEdgeType;
		Alt.GUI.Temporary.Gwen.Control.GroupBox groupBox6;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton radioButtonShadowStyleGradual;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton radioButtonShadowStyleUniform;
		Alt.GUI.Temporary.Gwen.Control.LabeledRadioButton radioButtonShadowStyleNone;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown numericUpDownEdgeLineWidth;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox checkBoxFitChart;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown m_numericUpDownAlpha;



        Color[] Colors
        {
            get
            {
                ArrayList colors = new ArrayList();
                if (buttonColor1.Enabled)
                    colors.Add(Color.FromArgb((int)m_numericUpDownAlpha.Value, buttonColor1.Color));
                if (buttonColor2.Enabled)
                    colors.Add(Color.FromArgb((int)m_numericUpDownAlpha.Value, buttonColor2.Color));
                if (buttonColor3.Enabled)
                    colors.Add(Color.FromArgb((int)m_numericUpDownAlpha.Value, buttonColor3.Color));
                if (buttonColor4.Enabled)
                    colors.Add(Color.FromArgb((int)m_numericUpDownAlpha.Value, buttonColor4.Color));
                if (buttonColor5.Enabled)
                    colors.Add(Color.FromArgb((int)m_numericUpDownAlpha.Value, buttonColor5.Color));
                if (buttonColor6.Enabled)
                    colors.Add(Color.FromArgb((int)m_numericUpDownAlpha.Value, buttonColor6.Color));

                return (Color[])colors.ToArray(typeof(Color));
            }
        }

        decimal[] Values
        {
            get
            {
                ArrayList values = new ArrayList();
                if (numericUpDownValue1.Enabled)
                    values.Add((decimal)numericUpDownValue1.Value);
                if (numericUpDownValue2.Enabled)
                    values.Add((decimal)numericUpDownValue2.Value);
                if (numericUpDownValue3.Enabled)
                    values.Add((decimal)numericUpDownValue3.Value);
                if (numericUpDownValue4.Enabled)
                    values.Add((decimal)numericUpDownValue4.Value);
                if (numericUpDownValue5.Enabled)
                    values.Add((decimal)numericUpDownValue5.Value);
                if (numericUpDownValue6.Enabled)
                    values.Add((decimal)numericUpDownValue6.Value);

                return (decimal[])values.ToArray(typeof(decimal));
            }
        }

        double[] Displacements
        {
            get
            {
                ArrayList displacements = new ArrayList();
                if (numericUpDownDisplacement1.Enabled)
                    displacements.Add((double)numericUpDownDisplacement1.Value);
                if (numericUpDownDisplacement2.Enabled)
                    displacements.Add((double)numericUpDownDisplacement2.Value);
                if (numericUpDownDisplacement3.Enabled)
                    displacements.Add((double)numericUpDownDisplacement3.Value);
                if (numericUpDownDisplacement4.Enabled)
                    displacements.Add((double)numericUpDownDisplacement4.Value);
                if (numericUpDownDisplacement5.Enabled)
                    displacements.Add((double)numericUpDownDisplacement5.Value);
                if (numericUpDownDisplacement6.Enabled)
                    displacements.Add((double)numericUpDownDisplacement6.Value);

                return (double[])displacements.ToArray(typeof(double));
            }
        }

        string[] Texts
        {
            get
            {
                ArrayList texts = new ArrayList();
                if (textBoxText1.Enabled)
                    texts.Add(textBoxText1.Text);
                if (textBoxText2.Enabled)
                    texts.Add(textBoxText2.Text);
                if (textBoxText3.Enabled)
                    texts.Add(textBoxText3.Text);
                if (textBoxText4.Enabled)
                    texts.Add(textBoxText4.Text);
                if (textBoxText5.Enabled)
                    texts.Add(textBoxText5.Text);
                if (textBoxText6.Enabled)
                    texts.Add(textBoxText6.Text);

                return (string[])texts.ToArray(typeof(string));
            }
        }

        string[] ToolTips
        {
            get
            {
                ArrayList toolTips = new ArrayList();
                if (textBoxToolTip1.Enabled)
                    toolTips.Add(textBoxToolTip1.Text);
                if (textBoxToolTip2.Enabled)
                    toolTips.Add(textBoxToolTip2.Text);
                if (textBoxToolTip3.Enabled)
                    toolTips.Add(textBoxToolTip3.Text);
                if (textBoxToolTip4.Enabled)
                    toolTips.Add(textBoxToolTip4.Text);
                if (textBoxToolTip5.Enabled)
                    toolTips.Add(textBoxToolTip5.Text);
                if (textBoxToolTip6.Enabled)
                    toolTips.Add(textBoxToolTip6.Text);

                return (string[])toolTips.ToArray(typeof(string));
            }
        }



        public Example_PieChart(Base parent)
            : base(parent)
        {
            m_BottomPanel = new Base(this);
            m_BottomPanel.Height = 264;
            m_BottomPanel.Dock = Pos.Bottom;

            m_ControlsPanel = new Base(m_BottomPanel);
            m_ControlsPanel.Size = new SizeI(590, 264);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            m_panelDrawing = new PieChartControl(this);
            //m_panelDrawing.DoubleBuffered = false;
            m_panelDrawing.Dock = Pos.Fill;
            m_panelDrawing.ClientBackColor = Color.FromArgb(0, 128, 128, 128);// Color.Transparent;// WhiteSmoke;
            m_panelDrawing.TextColor = Color.Black;
            //m_panelDrawing.DrawBorder = true;
            //m_panelDrawing.BorderColor = Color.DodgerBlue;
            m_panelDrawing.Margin = new Margin(8, 8, 8, 8);
            m_panelDrawing.ToolTips = null;


			groupBox6 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_ControlsPanel);
            groupBox6.Location = new PointI(8, 8);
            groupBox6.Size = new SizeI(320, 248);
            groupBox6.Text = "Pie slices";

            int inGroupBoxOffset = -12;
            int numericUpDownWidth = 45;

			Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox6);
            label.Location = new PointI(32, 20 + inGroupBoxOffset);
            label.Text = "Value";
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.AutoSizeToContents = true;

			label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox6);
            label.Location = new PointI(80, 20 + inGroupBoxOffset);
            label.Text = "Offset";
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.AutoSizeToContents = true;

			label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox6);
            label.Location = new PointI(128, 20 + inGroupBoxOffset);
            label.Text = "Color";
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.AutoSizeToContents = true;

			label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox6);
            label.Location = new PointI(168, 20 + inGroupBoxOffset);
            label.Text = "Text";
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.AutoSizeToContents = true;

			label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox6);
            label.Location = new PointI(248, 20 + inGroupBoxOffset);
            label.Text = "ToolTip";
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.AutoSizeToContents = true;



            int checkBoxOffset = 4;

			checkBox1 = new Alt.GUI.Temporary.Gwen.Control.CheckBox(groupBox6);
            checkBox1.IsChecked = true;
            checkBox1.Location = new PointI(8, 38 + inGroupBoxOffset + checkBoxOffset);
            checkBox1.CheckedChanged += new System.EventHandler(checkBox1_CheckedChanged);

			checkBox2 = new Alt.GUI.Temporary.Gwen.Control.CheckBox(groupBox6);
            checkBox2.IsChecked = true;
            checkBox2.Location = new PointI(8, 73 + inGroupBoxOffset + checkBoxOffset);
            checkBox2.CheckedChanged += new System.EventHandler(checkBox2_CheckedChanged);

			checkBox3 = new Alt.GUI.Temporary.Gwen.Control.CheckBox(groupBox6);
            checkBox3.IsChecked = true;
            checkBox3.Location = new PointI(8, 108 + inGroupBoxOffset + checkBoxOffset);
            checkBox3.CheckedChanged += new System.EventHandler(checkBox3_CheckedChanged);

			checkBox4 = new Alt.GUI.Temporary.Gwen.Control.CheckBox(groupBox6);
            checkBox4.IsChecked = true;
            checkBox4.Location = new PointI(8, 143 + inGroupBoxOffset + checkBoxOffset);
            checkBox4.CheckedChanged += new System.EventHandler(checkBox4_CheckedChanged);

			checkBox5 = new Alt.GUI.Temporary.Gwen.Control.CheckBox(groupBox6);
            checkBox5.IsChecked = true;
            checkBox5.Location = new PointI(8, 178 + inGroupBoxOffset + checkBoxOffset);
            checkBox5.CheckedChanged += new System.EventHandler(checkBox5_CheckedChanged);

			checkBox6 = new Alt.GUI.Temporary.Gwen.Control.CheckBox(groupBox6);
            checkBox6.IsChecked = true;
            checkBox6.Location = new PointI(8, 213 + inGroupBoxOffset + checkBoxOffset);
            checkBox6.CheckedChanged += new System.EventHandler(checkBox6_CheckedChanged);


			numericUpDownValue1 = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox6);
            numericUpDownValue1.Width = numericUpDownWidth;
            numericUpDownValue1.Location = new PointI(32, 40 + inGroupBoxOffset);
            numericUpDownValue1.Value = 10;
            numericUpDownValue1.ValueChanged += new GwenEventHandler(numericUpDownValue_ValueChanged);

			numericUpDownValue2 = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox6);
            numericUpDownValue2.Width = numericUpDownWidth;
            numericUpDownValue2.Location = new PointI(32, 75 + inGroupBoxOffset);
            numericUpDownValue2.Value = 15;
            numericUpDownValue2.ValueChanged += new GwenEventHandler(numericUpDownValue_ValueChanged);

			numericUpDownValue3 = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox6);
            numericUpDownValue3.Width = numericUpDownWidth;
            numericUpDownValue3.Location = new PointI(32, 110 + inGroupBoxOffset);
            numericUpDownValue3.Value = 20;
            numericUpDownValue3.ValueChanged += new GwenEventHandler(numericUpDownValue_ValueChanged);

			numericUpDownValue4 = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox6);
            numericUpDownValue4.Width = numericUpDownWidth;
            numericUpDownValue4.Location = new PointI(32, 145 + inGroupBoxOffset);
            numericUpDownValue4.Value = 60;
            numericUpDownValue4.ValueChanged += new GwenEventHandler(numericUpDownValue_ValueChanged);

			numericUpDownValue5 = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox6);
            numericUpDownValue5.Width = numericUpDownWidth;
            numericUpDownValue5.Location = new PointI(32, 180 + inGroupBoxOffset);
            numericUpDownValue5.Value = 25;
            numericUpDownValue5.ValueChanged += new GwenEventHandler(numericUpDownValue_ValueChanged);

			numericUpDownValue6 = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox6);
            numericUpDownValue6.Width = numericUpDownWidth;
            numericUpDownValue6.Location = new PointI(32, 215 + inGroupBoxOffset);
            numericUpDownValue6.Value = 25;
            numericUpDownValue6.ValueChanged += new GwenEventHandler(numericUpDownValue_ValueChanged);


			numericUpDownDisplacement1 = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox6);
            numericUpDownDisplacement1.Width = numericUpDownWidth;
            numericUpDownDisplacement1.DecimalPlaces = 2;
            numericUpDownDisplacement1.Increment = 0.05f;
            numericUpDownDisplacement1.Location = new PointI(80, 40 + inGroupBoxOffset);
            numericUpDownDisplacement1.Maximum = 1;
            numericUpDownDisplacement1.Minimum = 0;
            numericUpDownDisplacement1.Value = 0.2f;
            numericUpDownDisplacement1.ValueChanged += new GwenEventHandler(numericUpDownDisplacement_ValueChanged);

			numericUpDownDisplacement2 = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox6);
            numericUpDownDisplacement2.Width = numericUpDownWidth;
            numericUpDownDisplacement2.DecimalPlaces = 2;
            numericUpDownDisplacement2.Increment = 0.05f;
            numericUpDownDisplacement2.Location = new PointI(80, 75 + inGroupBoxOffset);
            numericUpDownDisplacement2.Maximum = 1;
            numericUpDownDisplacement2.Minimum = 0;
            numericUpDownDisplacement2.Value = 0.05f;
            numericUpDownDisplacement2.ValueChanged += new GwenEventHandler(numericUpDownDisplacement_ValueChanged);

			numericUpDownDisplacement3 = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox6);
            numericUpDownDisplacement3.Width = numericUpDownWidth;
            numericUpDownDisplacement3.DecimalPlaces = 2;
            numericUpDownDisplacement3.Increment = 0.05f;
            numericUpDownDisplacement3.Location = new PointI(80, 110 + inGroupBoxOffset);
            numericUpDownDisplacement3.Maximum = 1;
            numericUpDownDisplacement3.Minimum = 0;
            numericUpDownDisplacement3.Value = 0.05f;
            numericUpDownDisplacement3.ValueChanged += new GwenEventHandler(numericUpDownDisplacement_ValueChanged);

			numericUpDownDisplacement4 = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox6);
            numericUpDownDisplacement4.Width = numericUpDownWidth;
            numericUpDownDisplacement4.DecimalPlaces = 2;
            numericUpDownDisplacement4.Increment = 0.05f;
            numericUpDownDisplacement4.Location = new PointI(80, 145 + inGroupBoxOffset);
            numericUpDownDisplacement4.Maximum = 1;
            numericUpDownDisplacement4.Minimum = 0;
            numericUpDownDisplacement4.Value = 0.05f;
            numericUpDownDisplacement4.ValueChanged += new GwenEventHandler(numericUpDownDisplacement_ValueChanged);

			numericUpDownDisplacement5 = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox6);
            numericUpDownDisplacement5.Width = numericUpDownWidth;
            numericUpDownDisplacement5.DecimalPlaces = 2;
            numericUpDownDisplacement5.Increment = 0.05f;
            numericUpDownDisplacement5.Location = new PointI(80, 180 + inGroupBoxOffset);
            numericUpDownDisplacement5.Maximum = 1;
            numericUpDownDisplacement5.Minimum = 0;
            numericUpDownDisplacement5.Value = 0.05f;
            numericUpDownDisplacement5.ValueChanged += new GwenEventHandler(numericUpDownDisplacement_ValueChanged);

			numericUpDownDisplacement6 = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox6);
            numericUpDownDisplacement6.Width = numericUpDownWidth;
            numericUpDownDisplacement6.DecimalPlaces = 2;
            numericUpDownDisplacement6.Increment = 0.05f;
            numericUpDownDisplacement6.Location = new PointI(80, 215 + inGroupBoxOffset);
            numericUpDownDisplacement6.Maximum = 1;
            numericUpDownDisplacement6.Minimum = 0;
            numericUpDownDisplacement6.Value = 0.05f;
            numericUpDownDisplacement6.ValueChanged += new GwenEventHandler(numericUpDownDisplacement_ValueChanged);


            buttonColor1 = new ColorButton(groupBox6);
            buttonColor1.Color = Color.Red;
            buttonColor1.Location = new PointI(136, 40 + inGroupBoxOffset);
            buttonColor1.Size = new SizeI(20, 20);
            buttonColor1.Click += new System.EventHandler(buttonColor1_Click);

            buttonColor2 = new ColorButton(groupBox6);
            buttonColor2.Color = Color.LimeGreen;
            buttonColor2.Location = new PointI(136, 75 + inGroupBoxOffset);
            buttonColor2.Size = new SizeI(20, 20);
            buttonColor2.Click += new System.EventHandler(buttonColor2_Click);

            buttonColor3 = new ColorButton(groupBox6);
            buttonColor3.Color = Color.Blue;
            buttonColor3.Location = new PointI(136, 110 + inGroupBoxOffset);
            buttonColor3.Size = new SizeI(20, 20);
            buttonColor3.Click += new System.EventHandler(buttonColor3_Click);

            buttonColor4 = new ColorButton(groupBox6);
            buttonColor4.Color = Color.Yellow;
            buttonColor4.Location = new PointI(136, 145 + inGroupBoxOffset);
            buttonColor4.Size = new SizeI(20, 20);
            buttonColor4.Click += new System.EventHandler(buttonColor4_Click);

            buttonColor5 = new ColorButton(groupBox6);
            buttonColor5.Color = Color.Firebrick;
            buttonColor5.Location = new PointI(136, 180 + inGroupBoxOffset);
            buttonColor5.Size = new SizeI(20, 20);
            buttonColor5.Click += new System.EventHandler(buttonColor5_Click);

            buttonColor6 = new ColorButton(groupBox6);
            buttonColor6.Color = Color.DeepSkyBlue;
            buttonColor6.Location = new PointI(136, 215 + inGroupBoxOffset);
            buttonColor6.Size = new SizeI(20, 20);
            buttonColor6.Click += new System.EventHandler(buttonColor6_Click);


			textBoxText1 = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox6);
            textBoxText1.Location = new PointI(168, 40 + inGroupBoxOffset);
            textBoxText1.Size = new SizeI(72, 20);
            textBoxText1.Text = "red";
            textBoxText1.TextChanged += new GwenEventHandler(textBoxText_TextChanged);

			textBoxText2 = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox6);
            textBoxText2.Location = new PointI(168, 75 + inGroupBoxOffset);
            textBoxText2.Size = new SizeI(72, 20);
            textBoxText2.Text = "green";
            textBoxText2.TextChanged += new GwenEventHandler(textBoxText_TextChanged);

			textBoxText3 = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox6);
            textBoxText3.Location = new PointI(168, 110 + inGroupBoxOffset);
            textBoxText3.Size = new SizeI(72, 20);
            textBoxText3.Text = "blue";
            textBoxText3.TextChanged += new GwenEventHandler(textBoxText_TextChanged);

			textBoxText4 = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox6);
            textBoxText4.Location = new PointI(168, 145 + inGroupBoxOffset);
            textBoxText4.Size = new SizeI(72, 20);
            textBoxText4.Text = "yellow";
            textBoxText4.TextChanged += new GwenEventHandler(textBoxText_TextChanged);

			textBoxText5 = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox6);
            textBoxText5.Location = new PointI(168, 180 + inGroupBoxOffset);
            textBoxText5.Size = new SizeI(72, 20);
            textBoxText5.Text = "brown";
            textBoxText5.TextChanged += new GwenEventHandler(textBoxText_TextChanged);

			textBoxText6 = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox6);
            textBoxText6.Location = new PointI(168, 215 + inGroupBoxOffset);
            textBoxText6.Size = new SizeI(72, 20);
            textBoxText6.Text = "cyan";
            textBoxText6.TextChanged += new GwenEventHandler(textBoxText_TextChanged);


			textBoxToolTip1 = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox6);
            textBoxToolTip1.Location = new PointI(248, 40 + inGroupBoxOffset);
            textBoxToolTip1.Size = new SizeI(64, 20);
            textBoxToolTip1.Text = "";
            textBoxToolTip1.TextChanged += new GwenEventHandler(textBoxToolTip_TextChanged);

			textBoxToolTip2 = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox6);
            textBoxToolTip2.Location = new PointI(248, 75 + inGroupBoxOffset);
            textBoxToolTip2.Size = new SizeI(64, 20);
            textBoxToolTip2.Text = "";
            textBoxToolTip2.TextChanged += new GwenEventHandler(textBoxToolTip_TextChanged);

			textBoxToolTip3 = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox6);
            textBoxToolTip3.Location = new PointI(248, 110 + inGroupBoxOffset);
            textBoxToolTip3.Size = new SizeI(64, 20);
            textBoxToolTip3.Text = "";
            textBoxToolTip3.TextChanged += new GwenEventHandler(textBoxToolTip_TextChanged);

			textBoxToolTip4 = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox6);
            textBoxToolTip4.Location = new PointI(248, 145 + inGroupBoxOffset);
            textBoxToolTip4.Size = new SizeI(64, 20);
            textBoxToolTip4.Text = "";
            textBoxToolTip4.TextChanged += new GwenEventHandler(textBoxToolTip_TextChanged);

			textBoxToolTip5 = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox6);
            textBoxToolTip5.Location = new PointI(248, 180 + inGroupBoxOffset);
            textBoxToolTip5.Size = new SizeI(64, 20);
            textBoxToolTip5.Text = "";
            textBoxToolTip5.TextChanged += new GwenEventHandler(textBoxToolTip_TextChanged);

			textBoxToolTip6 = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox6);
            textBoxToolTip6.Location = new PointI(248, 215 + inGroupBoxOffset);
            textBoxToolTip6.Size = new SizeI(64, 20);
            textBoxToolTip6.Text = "";
            textBoxToolTip6.TextChanged += new GwenEventHandler(textBoxToolTip_TextChanged);



			groupBox4 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_ControlsPanel);
            groupBox4.Location = new PointI(336, 8);
            groupBox4.Size = new SizeI(248, 72);
            groupBox4.Text = "Margins";

            inGroupBoxOffset = -15;


			label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox4);
            label.Location = new PointI(12, 22 + inGroupBoxOffset);
            label.Size = new SizeI(40, 16);
            label.Text = "Left:";
            label.AutoSizeToContents = true;

			label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox4);
            label.Location = new PointI(12, 46 + inGroupBoxOffset);
            label.Size = new SizeI(40, 16);
            label.Text = "Right:";
            label.AutoSizeToContents = true;

			label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox4);
            label.Location = new PointI(132, 22 + inGroupBoxOffset);
            label.Size = new SizeI(40, 16);
            label.Text = "Top:";
            label.AutoSizeToContents = true;

			label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox4);
            label.Location = new PointI(132, 46 + inGroupBoxOffset);
            label.Size = new SizeI(44, 16);
            label.Text = "Bottom:";
            label.AutoSizeToContents = true;


			numericUpDownLeftMargin = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox4);
            numericUpDownLeftMargin.Location = new PointI(64, 20 + inGroupBoxOffset);
            numericUpDownLeftMargin.Maximum = 20;
            numericUpDownLeftMargin.Size = new SizeI(48, 20);
            numericUpDownLeftMargin.Value = 10;
            numericUpDownLeftMargin.ValueChanged += new GwenEventHandler(numericUpDownLeftMargin_ValueChanged);

			numericUpDownRightMargin = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox4);
            numericUpDownRightMargin.Location = new PointI(64, 44 + inGroupBoxOffset);
            numericUpDownRightMargin.Maximum = 20;
            numericUpDownRightMargin.Size = new SizeI(48, 20);
            numericUpDownRightMargin.Value = 10;
            numericUpDownRightMargin.ValueChanged += new GwenEventHandler(numericUpDownRightMargin_ValueChanged);

			numericUpDownTopMargin = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox4);
            numericUpDownTopMargin.Location = new PointI(188, 20 + inGroupBoxOffset);
            numericUpDownTopMargin.Maximum = 20;
            numericUpDownTopMargin.Size = new SizeI(48, 20);
            numericUpDownTopMargin.Value = 10;
            numericUpDownTopMargin.ValueChanged += new GwenEventHandler(numericUpDownTopMargin_ValueChanged);

			numericUpDownBottomMargin = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox4);
            numericUpDownBottomMargin.Location = new PointI(188, 44 + inGroupBoxOffset);
            numericUpDownBottomMargin.Maximum = 20;
            numericUpDownBottomMargin.Size = new SizeI(48, 20);
            numericUpDownBottomMargin.Value = 10;
            numericUpDownBottomMargin.ValueChanged += new GwenEventHandler(numericUpDownBottomMargin_ValueChanged);



			checkBoxFitChart = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(m_ControlsPanel);
            checkBoxFitChart.Location = new PointI(336, 88);
            checkBoxFitChart.Text = "Fit chart to panel";
            checkBoxFitChart.CheckedChanged += new System.EventHandler(checkBoxFitChart_CheckedChanged);



			groupBox1 = new Alt.GUI.Temporary.Gwen.Control.RadioButtonGroup(m_ControlsPanel, "Shadow style");
            groupBox1.Location = new PointI(485, 95);//84);
            groupBox1.Size = new SizeI(104, 62);//82);


            radioButtonShadowStyleNone = groupBox1.AddOption("None");
            radioButtonShadowStyleNone.Tag = "";
            radioButtonShadowStyleNone.CheckedChanged += new System.EventHandler(radioButtonShadowStyle_Changed);

            radioButtonShadowStyleUniform = groupBox1.AddOption("Uniform");
            radioButtonShadowStyleUniform.Select();
            radioButtonShadowStyleUniform.Tag = "";
            radioButtonShadowStyleUniform.CheckedChanged += new System.EventHandler(radioButtonShadowStyle_Changed);

            /*TEMP
            radioButtonShadowStyleGradual = groupBox1.AddOption("Gradual");
            //radioButtonShadowStyleGradual.Select();
            radioButtonShadowStyleGradual.Tag = "";
            radioButtonShadowStyleGradual.CheckedChanged += new System.EventHandler(radioButtonShadowStyle_Changed);*/



			numericUpDownPieHeight = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(m_ControlsPanel);
            numericUpDownPieHeight.DecimalPlaces = 2;
            numericUpDownPieHeight.Increment = 0.05f;
            numericUpDownPieHeight.Location = new PointI(408, 114);
            numericUpDownPieHeight.Maximum = 0.5f;
            numericUpDownPieHeight.Size = new SizeI(48, 20);
            numericUpDownPieHeight.Value = 0.25f;
            numericUpDownPieHeight.ValueChanged += new GwenEventHandler(numericUpDownPieHeight_ValueChanged);


			label = new Alt.GUI.Temporary.Gwen.Control.Label(m_ControlsPanel);
            label.Location = new PointI(336, 116);
            label.Size = new SizeI(72, 16);
            label.Text = "Pie Height:";
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.AutoSizeToContents = true;


			numericUpDownAngle = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(m_ControlsPanel);
            numericUpDownAngle.Increment = 10;
            numericUpDownAngle.Location = new PointI(408, 144);
            numericUpDownAngle.Maximum = 400;
            numericUpDownAngle.Minimum = -360;
            numericUpDownAngle.Size = new SizeI(48, 20);
            numericUpDownAngle.Value = -30;
            numericUpDownAngle.ValueChanged += new GwenEventHandler(numericUpDownAngle_ValueChanged);


			label = new Alt.GUI.Temporary.Gwen.Control.Label(m_ControlsPanel);
            label.Location = new PointI(336, 146);
            label.Size = new SizeI(72, 16);
            label.Text = "Initial angle:";
            label.AutoSizeToContents = true;



			comboBoxEdgeType = new Alt.GUI.Temporary.Gwen.Control.ComboBox(m_ControlsPanel);
            comboBoxEdgeType.Location = new PointI(408, 178);
            comboBoxEdgeType.Size = new SizeI(176, 21);
            comboBoxEdgeType.ItemSelected += new GwenEventHandler(comboBoxEdgeType_SelectedIndexChanged);


			label = new Alt.GUI.Temporary.Gwen.Control.Label(m_ControlsPanel);
            label.Location = new PointI(336, 180);
            label.Size = new SizeI(72, 16);
            label.Text = "Edge color:";
            label.AutoSizeToContents = true;



			label = new Alt.GUI.Temporary.Gwen.Control.Label(m_ControlsPanel);
            label.Location = new PointI(336, 208);
            label.Size = new SizeI(72, 16);
            label.Text = "Edge width:";
            label.AutoSizeToContents = true;


			numericUpDownEdgeLineWidth = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(m_ControlsPanel);
            numericUpDownEdgeLineWidth.DecimalPlaces = 1;
            numericUpDownEdgeLineWidth.Increment = 0.5f;
            numericUpDownEdgeLineWidth.Location = new PointI(408, 206);
            numericUpDownEdgeLineWidth.Maximum = 5;
            numericUpDownEdgeLineWidth.Size = new SizeI(48, 20);
            numericUpDownEdgeLineWidth.Value = 3;
            numericUpDownEdgeLineWidth.ValueChanged += new GwenEventHandler(numericUpDownEdgeLineWidth_ValueChanged);


			label = new Alt.GUI.Temporary.Gwen.Control.Label(m_ControlsPanel);
            label.Location = new PointI(336, 236);
            label.Size = new SizeI(48, 16);
            label.Text = "Alpha:";
            label.AutoSizeToContents = true;


			m_numericUpDownAlpha = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(m_ControlsPanel);
            m_numericUpDownAlpha.Increment = 5;
            m_numericUpDownAlpha.Location = new PointI(408, 234);
            m_numericUpDownAlpha.Maximum = 255;
            m_numericUpDownAlpha.Size = new SizeI(48, 20);
            m_numericUpDownAlpha.Value = 130;
            m_numericUpDownAlpha.ValueChanged += new GwenEventHandler(m_numericUpDownAlpha_ValueChanged);



            FillEdgeColorTypeListBox();

            InitializeChart();
        }



        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (m_BottomPanel != null)
            {
                int x = (m_BottomPanel.Width - m_ControlsPanel.Width) / 2;
                if (x < 0)
                {
                    x = 0;
                }

                m_ControlsPanel.X = x;
            }
        }



        void ClientRefresh()
        {
            if (m_panelDrawing != null)
            {
                m_panelDrawing.Refresh();
            }
        }



        void InitializeChart()
        {
            SetValues();
            SetPieDisplacements();
            SetColors();
            SetTexts();
            SetToolTips();
            m_panelDrawing.LeftMargin = (float)numericUpDownLeftMargin.Value;
            m_panelDrawing.RightMargin = (float)numericUpDownRightMargin.Value;
            m_panelDrawing.TopMargin = (float)numericUpDownTopMargin.Value;
            m_panelDrawing.BottomMargin = (float)numericUpDownBottomMargin.Value;
            m_panelDrawing.FitChart = checkBoxFitChart.IsChecked;
            m_panelDrawing.SliceRelativeHeight = (float)numericUpDownPieHeight.Value;
            m_panelDrawing.InitialAngle = (float)numericUpDownAngle.Value;
            m_panelDrawing.EdgeLineWidth = (float)numericUpDownEdgeLineWidth.Value;
        }

        void FillEdgeColorTypeListBox()
        {
            foreach (Alt.GUI.PieChart.EdgeColorType ect in
                //Enum.GetValues(typeof(Alt.Sketch.Ext.PieChart.EdgeColorType))
                EnumHelper.GetValues<Alt.GUI.PieChart.EdgeColorType>()
                )
            {
				Alt.GUI.Temporary.Gwen.Control.MenuItem item = comboBoxEdgeType.AddItem(ect.ToString(), ect.ToString());
                item.Tag = ect;

                if (ect == Alt.GUI.PieChart.EdgeColorType.EnhancedContrast)//DarkerThanSurface)
                {
                    comboBoxEdgeType.SelectedItem = item;
                }
            }
        }

        void UpdateChart()
        {
            SetValues();
            SetPieDisplacements();
            SetColors();
            SetTexts();
            SetToolTips();
        }

        void SetValues()
        {
            m_panelDrawing.Values = Values;

            ClientRefresh();
        }

        void SetPieDisplacements()
        {
            m_panelDrawing.SliceRelativeDisplacements = Displacements;

            ClientRefresh();
        }

        void SetColors()
        {
            m_panelDrawing.Colors = Colors;

            ClientRefresh();
        }

        void SetTexts()
        {
            m_panelDrawing.Texts = Texts;

            ClientRefresh();
        }

        void SetToolTips()
        {
            m_panelDrawing.ToolTips = ToolTips;

            ClientRefresh();
        }



        void checkBox1_CheckedChanged(object sender, System.EventArgs e)
        {
            numericUpDownValue1.Enabled = checkBox1.IsChecked;
            numericUpDownDisplacement1.Enabled = checkBox1.IsChecked;
            buttonColor1.Enabled = checkBox1.IsChecked;
            textBoxText1.Enabled = checkBox1.IsChecked;
            textBoxToolTip1.Enabled = checkBox1.IsChecked;
            UpdateChart();
        }

        void checkBox2_CheckedChanged(object sender, System.EventArgs e)
        {
            numericUpDownValue2.Enabled = checkBox2.IsChecked;
            numericUpDownDisplacement2.Enabled = checkBox2.IsChecked;
            buttonColor2.Enabled = checkBox2.IsChecked;
            textBoxText2.Enabled = checkBox2.IsChecked;
            textBoxToolTip2.Enabled = checkBox2.IsChecked;
            UpdateChart();
        }

        void checkBox3_CheckedChanged(object sender, System.EventArgs e)
        {
            numericUpDownValue3.Enabled = checkBox3.IsChecked;
            numericUpDownDisplacement3.Enabled = checkBox3.IsChecked;
            buttonColor3.Enabled = checkBox3.IsChecked;
            textBoxText3.Enabled = checkBox3.IsChecked;
            textBoxToolTip3.Enabled = checkBox3.IsChecked;
            UpdateChart();
        }

        void checkBox4_CheckedChanged(object sender, System.EventArgs e)
        {
            numericUpDownValue4.Enabled = checkBox4.IsChecked;
            numericUpDownDisplacement4.Enabled = checkBox4.IsChecked;
            buttonColor4.Enabled = checkBox4.IsChecked;
            textBoxText4.Enabled = checkBox4.IsChecked;
            textBoxToolTip4.Enabled = checkBox4.IsChecked;
            UpdateChart();
        }

        void checkBox5_CheckedChanged(object sender, System.EventArgs e)
        {
            numericUpDownValue5.Enabled = checkBox5.IsChecked;
            numericUpDownDisplacement5.Enabled = checkBox5.IsChecked;
            buttonColor5.Enabled = checkBox5.IsChecked;
            textBoxText5.Enabled = checkBox5.IsChecked;
            textBoxToolTip5.Enabled = checkBox5.IsChecked;
            UpdateChart();
        }

        void checkBox6_CheckedChanged(object sender, System.EventArgs e)
        {
            numericUpDownValue6.Enabled = checkBox6.IsChecked;
            numericUpDownDisplacement6.Enabled = checkBox6.IsChecked;
            buttonColor6.Enabled = checkBox6.IsChecked;
            textBoxText6.Enabled = checkBox6.IsChecked;
            textBoxToolTip6.Enabled = checkBox6.IsChecked;
            UpdateChart();
        }


        void numericUpDownValue_ValueChanged(Base control)
        {
            SetValues();
        }

        void numericUpDownDisplacement_ValueChanged(Base control)
        {
            SetPieDisplacements();
        }

        void textBoxText_TextChanged(Base control)
        {
            SetTexts();
        }

        void textBoxToolTip_TextChanged(Base control)
        {
            SetToolTips();
        }


        ColorButton m_CurrentColorButton;
        void ShowColorDialog(ColorButton colorButton)
        {
            m_CurrentColorButton = colorButton;

			Alt.GUI.Temporary.Gwen.Control.Menu menu = new Alt.GUI.Temporary.Gwen.Control.Menu(GetCanvas());
            menu.SetSize(256, 180);
            menu.DeleteOnClose = true;
            menu.IconMarginDisabled = true;

            HSVColorPicker picker = new HSVColorPicker(menu);
            picker.Dock = Pos.Fill;
            picker.SetSize(256, 128);

            picker.SetColor(m_CurrentColorButton.Color, false, true);
            picker.ColorChanged += OnColorChanged;

            menu.Open(Pos.Right | Pos.Top);
        }
        void OnColorChanged(Base control)
        {
            HSVColorPicker picker = control as HSVColorPicker;
            m_CurrentColorButton.Color = picker.SelectedColor;

            SetColors();
        }

        void buttonColor1_Click(object sender, System.EventArgs e)
        {
            ShowColorDialog(buttonColor1);
        }

        void buttonColor2_Click(object sender, System.EventArgs e)
        {
            ShowColorDialog(buttonColor2);
        }

        void buttonColor3_Click(object sender, System.EventArgs e)
        {
            ShowColorDialog(buttonColor3);
        }

        void buttonColor4_Click(object sender, System.EventArgs e)
        {
            ShowColorDialog(buttonColor4);
        }

        void buttonColor5_Click(object sender, System.EventArgs e)
        {
            ShowColorDialog(buttonColor5);
        }

        void buttonColor6_Click(object sender, System.EventArgs e)
        {
            ShowColorDialog(buttonColor6);
        }


        void numericUpDownLeftMargin_ValueChanged(Base control)
        {
            m_panelDrawing.LeftMargin = numericUpDownLeftMargin.Value;

            ClientRefresh();
        }

        void numericUpDownRightMargin_ValueChanged(Base control)
        {
            m_panelDrawing.RightMargin = numericUpDownRightMargin.Value;

            ClientRefresh();
        }

        void numericUpDownTopMargin_ValueChanged(Base control)
        {
            m_panelDrawing.TopMargin = numericUpDownTopMargin.Value;

            ClientRefresh();
        }

        void numericUpDownBottomMargin_ValueChanged(Base control)
        {
            m_panelDrawing.BottomMargin = numericUpDownBottomMargin.Value;

            ClientRefresh();
        }


        void checkBoxFitChart_CheckedChanged(object sender, System.EventArgs e)
        {
            m_panelDrawing.FitChart = checkBoxFitChart.IsChecked;

            ClientRefresh();
        }


        void radioButtonShadowStyle_Changed(object sender, System.EventArgs e)
        {
            if (radioButtonShadowStyleNone.IsChecked)
            {
                m_panelDrawing.ShadowStyle = Alt.GUI.PieChart.ShadowStyle.NoShadow;
            }
            else if (radioButtonShadowStyleUniform.IsChecked)
            {
                m_panelDrawing.ShadowStyle = Alt.GUI.PieChart.ShadowStyle.UniformShadow;
            }
            else
            {
                //TEMP  m_panelDrawing.ShadowStyle = Alt.Sketch.Ext.PieChart.ShadowStyle.GradualShadow;
            }

            ClientRefresh();
        }


        void numericUpDownPieHeight_ValueChanged(Base control)
        {
            m_panelDrawing.SliceRelativeHeight = numericUpDownPieHeight.Value;

            ClientRefresh();
        }

        void numericUpDownAngle_ValueChanged(Base control)
        {
            m_panelDrawing.InitialAngle = numericUpDownAngle.Value;

            ClientRefresh();
        }


        void numericUpDownEdgeLineWidth_ValueChanged(Base control)
        {
            m_panelDrawing.EdgeLineWidth = numericUpDownEdgeLineWidth.Value;

            ClientRefresh();
        }

        void m_numericUpDownAlpha_ValueChanged(Base control)
        {
            SetColors();
        }


        void comboBoxEdgeType_SelectedIndexChanged(Base control)
        {
            if (comboBoxEdgeType.SelectedItem == null ||
                comboBoxEdgeType.SelectedItem.Tag == null)
            {
                return;
            }

            m_panelDrawing.EdgeColorType = (Alt.GUI.PieChart.EdgeColorType)comboBoxEdgeType.SelectedItem.Tag;

            ClientRefresh();
        }
    }
}
