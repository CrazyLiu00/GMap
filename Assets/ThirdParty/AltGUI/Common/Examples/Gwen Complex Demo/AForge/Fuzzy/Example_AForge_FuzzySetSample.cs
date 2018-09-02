// Fyzzy Set sample application
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright � AForge.NET, 2005-2011
// contacts@aforgenet.com

using System;
using System.Collections.Generic;
using System.Text;

using Alt.ComponentModel;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;
using Alt.Threading;

using AForge;
using AForge.Fuzzy;
using Alt.GUI.AForge.Temporary.Gwen;


namespace Alt.GUI.Demo
{
    class Example_AForge_FuzzySetSample : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 0;
                int y = 0;

                if (m_TopPanel != null)
                {
                    y = m_TopPanel.Height + m_TopPanel.Margin.Height;
                }

                return new SizeI(x, y);
            }
        }


		Alt.GUI.Temporary.Gwen.Control.Base m_TopPanel;
		Alt.GUI.Temporary.Gwen.Control.Button runFuzzySetTestButton;
        Chart chart;
		Alt.GUI.Temporary.Gwen.Control.Label label1;
		Alt.GUI.Temporary.Gwen.Control.Label label2;
		Alt.GUI.Temporary.Gwen.Control.Label label3;
		Alt.GUI.Temporary.Gwen.Control.Label label4;
		Alt.GUI.Temporary.Gwen.Control.Button runLingVarTestButton;

        
        public Example_AForge_FuzzySetSample(Base parent)
            : base(parent)
        {
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            //  GUI
            {
				m_TopPanel = new Alt.GUI.Temporary.Gwen.Control.Base(this);
                {
                    m_TopPanel.Dock = Pos.Top;
                    m_TopPanel.Height = 30;
                    m_TopPanel.Margin = new Margin(0, 7, 0, 3);


					runFuzzySetTestButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_TopPanel);
                    runFuzzySetTestButton.Text = "Run Fuzzy Set Test";
                    runFuzzySetTestButton.AutoSizeToContents = true;
                    runFuzzySetTestButton.Dock = Pos.Left;
                    runFuzzySetTestButton.Click += new System.EventHandler(runFuzzySetTestButton_Click);
                    runFuzzySetTestButton.NormalTextColor = Color.Blue;

					runLingVarTestButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_TopPanel);
                    runLingVarTestButton.Text = "Run Linguistic Variable Test";
                    runLingVarTestButton.AutoSizeToContents = true;
                    runLingVarTestButton.Dock = Pos.Right;
                    runLingVarTestButton.Click += new System.EventHandler(runLingVarTestButton_Click);
                    runLingVarTestButton.NormalTextColor = Color.Green;
                }


                Base bottomPanel = new Base(this);
                {
                    bottomPanel.Dock = Pos.Bottom;
                    bottomPanel.Height = 30;
                    bottomPanel.Margin = new Margin(0, 10, 0, 0);

                    
					label1 = new Alt.GUI.Temporary.Gwen.Control.Label(bottomPanel);
					label2 = new Alt.GUI.Temporary.Gwen.Control.Label(bottomPanel);
					label4 = new Alt.GUI.Temporary.Gwen.Control.Label(bottomPanel);
					label3 = new Alt.GUI.Temporary.Gwen.Control.Label(bottomPanel);

                    label1.ClientBackColor = Color.LightBlue;
                    label1.Width = 100;
                    label1.Dock = Pos.Left;
                    label1.DrawBorder = true;
                    label1.BorderColor = Color.DodgerBlue * 0.6;
                    label1.Text = "Cool";
                    label1.TextAlign = ContentAlignment.MiddleCenter;

                    label2.ClientBackColor = Color.LightCoral;
                    label2.Width = 100;
                    label2.Dock = Pos.Left;
                    label2.DrawBorder = true;
                    label2.BorderColor = Color.DodgerBlue * 0.6;
                    label2.Text = "Warm";
                    label2.TextAlign = ContentAlignment.MiddleCenter;
                    label2.Margin = new Margin(7, 0, 0, 0);

                    label4.ClientBackColor = Color.Firebrick;
                    label4.Width = 100;
                    label4.Dock = Pos.Right;
                    label4.DrawBorder = true;
                    label4.BorderColor = Color.DodgerBlue * 0.6;
                    label4.Text = "Hot";
                    label4.TextAlign = ContentAlignment.MiddleCenter;

                    label3.ClientBackColor = Color.CornflowerBlue;
                    label3.Width = 100;
                    label3.Dock = Pos.Right;
                    label3.DrawBorder = true;
                    label3.BorderColor = Color.DodgerBlue * 0.6;
                    label3.Text = "Cold";
                    label3.TextAlign = ContentAlignment.MiddleCenter;
                    label3.Margin = new Margin(0, 0, 7, 0);
                }


                chart = new Chart(this);
                chart.Dock = Pos.Fill;
            }


            chart.RangeX = new Range(0, 50);
            chart.AddDataSeries("COLD", Color.CornflowerBlue, Chart.SeriesType.Line, 3, true);
            chart.AddDataSeries("COOL", Color.LightBlue, Chart.SeriesType.Line, 3, true);
            chart.AddDataSeries("WARM", Color.LightCoral, Chart.SeriesType.Line, 3, true);
            chart.AddDataSeries("HOT", Color.Firebrick, Chart.SeriesType.Line, 3, true);
        }
        

        // Testing basic funcionality of fuzzy sets
        void runFuzzySetTestButton_Click(object sender, EventArgs e)
        {
            ClearDataSeries();

            // create 2 fuzzy sets to represent the Cool and Warm temperatures
            TrapezoidalFunction function1 = new TrapezoidalFunction(13, 18, 23, 28);
            FuzzySet fsCool = new FuzzySet("Cool", function1);
            TrapezoidalFunction function2 = new TrapezoidalFunction(23, 28, 33, 38);
            FuzzySet fsWarm = new FuzzySet("Warm", function2);

            // get membership of some points to the cool fuzzy set
            double[,] coolValues = new double[20, 2];
            for (int i = 10; i < 30; i++)
            {
                coolValues[i - 10, 0] = i;
                coolValues[i - 10, 1] = fsCool.GetMembership(i);
            }

            // getting memberships of some points to the warm fuzzy set
            double[,] warmValues = new double[20, 2];
            for (int i = 20; i < 40; i++)
            {
                warmValues[i - 20, 0] = i;
                warmValues[i - 20, 1] = fsWarm.GetMembership(i);
            }

            // plot membership to a chart
            chart.UpdateDataSeries("COOL", coolValues);
            chart.UpdateDataSeries("WARM", warmValues);
        }


        // Testing basic funcionality of linguistic variables
        void runLingVarTestButton_Click(object sender, EventArgs e)
        {
            ClearDataSeries();

            // create a linguistic variable to represent temperature
            LinguisticVariable lvTemperature = new LinguisticVariable("Temperature", 0, 80);

            // create the linguistic labels (fuzzy sets) that compose the temperature 
            TrapezoidalFunction function1 = new TrapezoidalFunction(10, 15, TrapezoidalFunction.EdgeType.Right);
            FuzzySet fsCold = new FuzzySet("Cold", function1);
            TrapezoidalFunction function2 = new TrapezoidalFunction(10, 15, 20, 25);
            FuzzySet fsCool = new FuzzySet("Cool", function2);
            TrapezoidalFunction function3 = new TrapezoidalFunction(20, 25, 30, 35);
            FuzzySet fsWarm = new FuzzySet("Warm", function3);
            TrapezoidalFunction function4 = new TrapezoidalFunction(30, 35, TrapezoidalFunction.EdgeType.Left);
            FuzzySet fsHot = new FuzzySet("Hot", function4);

            // adding labels to the variable
            lvTemperature.AddLabel(fsCold);
            lvTemperature.AddLabel(fsCool);
            lvTemperature.AddLabel(fsWarm);
            lvTemperature.AddLabel(fsHot);

            // get membership of some points to the cool fuzzy set
            double[][,] chartValues = new double[4][,];
            for (int i = 0; i < 4; i++)
                chartValues[i] = new double[160, 2];

            // showing the shape of the linguistic variable - the shape of its labels memberships from start to end
            int j = 0;
            for (float x = 0; x < 80; x += 0.5f, j++)
            {
                double y1 = lvTemperature.GetLabelMembership("Cold", x);
                double y2 = lvTemperature.GetLabelMembership("Cool", x);
                double y3 = lvTemperature.GetLabelMembership("Warm", x);
                double y4 = lvTemperature.GetLabelMembership("Hot", x);

                chartValues[0][j, 0] = x;
                chartValues[0][j, 1] = y1;
                chartValues[1][j, 0] = x;
                chartValues[1][j, 1] = y2;
                chartValues[2][j, 0] = x;
                chartValues[2][j, 1] = y3;
                chartValues[3][j, 0] = x;
                chartValues[3][j, 1] = y4;
            }

            // plot membership to a chart
            chart.UpdateDataSeries("COLD", chartValues[0]);
            chart.UpdateDataSeries("COOL", chartValues[1]);
            chart.UpdateDataSeries("WARM", chartValues[2]);
            chart.UpdateDataSeries("HOT", chartValues[3]);
        }


        // Clear all data series data
        void ClearDataSeries()
        {
            chart.UpdateDataSeries("COLD", null);
            chart.UpdateDataSeries("COOL", null);
            chart.UpdateDataSeries("WARM", null);
            chart.UpdateDataSeries("HOT", null);
        }
    }
}
