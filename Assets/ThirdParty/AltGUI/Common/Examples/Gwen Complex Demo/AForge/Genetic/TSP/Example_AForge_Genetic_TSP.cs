// Traveling Salesman Problem using Genetic Algorithms
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright � AForge.NET, 2006-2011
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
using AForge.Genetic;
using Alt.GUI.AForge.Temporary.Gwen;
using Alt.GUI.Demo.AForge.Genetic.TSP;


//  ??? Unknown SILVERLIGHT error
namespace Alt.GUI.Demo
{
    class Example_AForge_Genetic_TSP : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 0;
                int y = 0;

                if (m_TopPanel != null &&
                    m_RightPanel != null)
                {
                    x = m_RightPanel.Width + m_RightPanel.Margin.Width;
                    y = m_TopPanel.Height + m_TopPanel.Margin.Height;
                }

                return new SizeI(x, y);
            }
        }


        string Description
        {
            get
            {
				return "AltGUI AForge.NET Traveling Salesman Problem using Genetic Algorithms";
            }
        }


        Bitmap Screenshot
        {
            get
            {
                return Example_NotAvailable_ScreenShot.LoadImage("Example_AForge_Genetic_TSP.gif");
            }
        }


		Alt.GUI.Temporary.Gwen.Control.Base m_TopPanel;
		Alt.GUI.Temporary.Gwen.Control.Base m_RightPanel;
        Chart mapControl;
		Alt.GUI.Temporary.Gwen.Control.Label label1;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown citiesCountBox;
		Alt.GUI.Temporary.Gwen.Control.Button generateMapButton;
		Alt.GUI.Temporary.Gwen.Control.GroupBox groupBox2;
		Alt.GUI.Temporary.Gwen.Control.Label label2;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown populationSizeBox;
		Alt.GUI.Temporary.Gwen.Control.Label label3;
		Alt.GUI.Temporary.Gwen.Control.ComboBox selectionBox;
		Alt.GUI.Temporary.Gwen.Control.Label label4;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown iterationsBox;
		Alt.GUI.Temporary.Gwen.Control.Label label5;
		Alt.GUI.Temporary.Gwen.Control.GroupBox groupBox3;
		Alt.GUI.Temporary.Gwen.Control.Button startButton;
		Alt.GUI.Temporary.Gwen.Control.Button stopButton;
		Alt.GUI.Temporary.Gwen.Control.Label label6;
		Alt.GUI.Temporary.Gwen.Control.TextBox currentIterationBox;
		Alt.GUI.Temporary.Gwen.Control.Label label7;
		Alt.GUI.Temporary.Gwen.Control.TextBox pathLengthBox;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox greedyCrossoverBox;


        int citiesCount = 20;
        int populationSize = 40;
        int iterations = 100;
        int selectionMethod = 0;
        bool greedyCrossover = true;

        double[,] map = null;

        Thread workerThread = null;
        volatile bool needToStop = false;


        public Example_AForge_Genetic_TSP(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


			Alt.GUI.Temporary.Gwen.Control.Label label;
#if SILVERLIGHT || UNITY_WEBPLAYER
			label = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            label.AutoSizeToContents = true;
            label.Text = //Description + "\n" + "(This example is not available in this Demo, please download SDK)";
				"THIS EXAMPLE IS NOT AVAILABLE IN THIS DEMO,\nPLEASE DOWNLOAD AltGUI SDK";
            label.TextColor = Color.Orange * 1.2;
            label.Dock = Pos.Top;
            label.Margin = new Margin(0, 0, 0, 5);
            label.Font = Example_NotAvailable_ScreenShot.Font;
#endif


            //  GUI
            {
                m_TopPanel = new Base(this);
                {
                    m_TopPanel.Dock = Pos.Top;
                    m_TopPanel.Height = 30;

					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_TopPanel);
                    label.Location = new PointI(0, 2);
                    label.AutoSizeToContents = true;
                    label.TextColor = Color.LightGreen;
                    label.Text = "Map";


					label1 = new Alt.GUI.Temporary.Gwen.Control.Label(m_TopPanel);
					citiesCountBox = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(m_TopPanel);
					generateMapButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_TopPanel);

                    label1.Location = new PointI(33, 2);
                    label1.Text = "Cities:";
                    label1.AutoSizeToContents = true;

                    citiesCountBox.Location = new PointI(75, 1);
                    citiesCountBox.Width = 50;
                    citiesCountBox.Min = 5;
                    citiesCountBox.Max = 50;
                    citiesCountBox.Increment = 10;

                    generateMapButton.Location = new PointI(135, 0);
                    generateMapButton.Text = "Generate";
                    generateMapButton.AutoSizeToContents = true;
                    generateMapButton.Click += new System.EventHandler(generateMapButton_Click);
                    generateMapButton.NormalTextColor = Color.Blue;
                }


                m_RightPanel = new Base(this);
                {
                    m_RightPanel.Dock = Pos.Right;
                    m_RightPanel.Width = 140;
                    m_RightPanel.Margin = new Margin(5, 0, 0, 0);

					groupBox2 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_RightPanel);
                    {
                        groupBox2.Location = new PointI(0, 0);
                        groupBox2.Size = new SizeI(140, 223);
                        groupBox2.Text = "Settings";

						greedyCrossoverBox = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(groupBox2);
						label5 = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
						iterationsBox = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox2);
						label4 = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
						selectionBox = new Alt.GUI.Temporary.Gwen.Control.ComboBox(groupBox2);
						label3 = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
						populationSizeBox = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(groupBox2);
						label2 = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);


                        label2.Location = new PointI(0, 10);
                        label2.AutoSizeToContents = true;
                        label2.Text = "Population size:";

                        populationSizeBox.Location = new PointI(0, 30);
                        populationSizeBox.Width = 130;
                        populationSizeBox.Min = 10;
                        populationSizeBox.Max = 100;
                        populationSizeBox.Increment = 10;


                        label3.Location = new PointI(0, 60);
                        label3.AutoSizeToContents = true;
                        label3.Text = "Selection method:";

                        selectionBox.AddItem("Elite").UserData = 0;
                        selectionBox.AddItem("Rank").UserData = 1;
                        selectionBox.AddItem("Roulette").UserData = 2;
                        selectionBox.Location = new PointI(0, 80);
                        selectionBox.Width = 130;


                        greedyCrossoverBox.Location = new PointI(0, 115);
                        greedyCrossoverBox.Text = "Greedy crossover";

                        
                        label4.Location = new PointI(0, 145);
                        label4.Size = new SizeI(60, 16);
                        label4.Text = "Iterations:";                                                

                        iterationsBox.Location = new PointI(0, 165);
                        iterationsBox.Width = 130;
                        iterationsBox.Min = 0;
                        iterationsBox.Max = 100000;
                        iterationsBox.Increment = 100;

                        label5.Location = new PointI(0, 185);
                        label5.AutoSizeToContents = true;
                        label5.Text = "( 0 - inifinity )";
                    }


					groupBox3 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_RightPanel);
                    {
                        groupBox3.Location = new PointI(0, 230);
                        groupBox3.Size = new SizeI(140, 122);
                        groupBox3.Text = "Current iteration";

						pathLengthBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox3);
						label7 = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox3);
						currentIterationBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox3);
						label6 = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox3);


                        label6.Location = new PointI(0, 10);
                        label6.AutoSizeToContents = true;
                        label6.Text = "Iteration:";

                        currentIterationBox.Location = new PointI(0, 30);
                        currentIterationBox.ReadOnly = true;
                        currentIterationBox.Width = 130;
                        currentIterationBox.Text = "";

                        
                        label7.Location = new PointI(0, 60);
                        label7.AutoSizeToContents = true;
                        label7.Text = "Path length:";

                        pathLengthBox.Location = new PointI(0, 80);
                        pathLengthBox.ReadOnly = true;
                        pathLengthBox.Width = 130;
                        pathLengthBox.Text = "";
                    }


					startButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_RightPanel);
					stopButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_RightPanel);

                    startButton.Location = new PointI(5, 370);
                    startButton.Width = 130;
                    startButton.Text = "Start";
                    startButton.Click += new System.EventHandler(startButton_Click);
                    startButton.NormalTextColor = Color.Green;

                    stopButton.Enabled = false;
                    stopButton.Location = new PointI(5, 395);
                    stopButton.Width = 130;
                    stopButton.Text = "Stop";
                    stopButton.Click += new System.EventHandler(stopButton_Click);
                    stopButton.NormalTextColor = Color.Red * 0.8;
                }


                mapControl = new Chart(this);
                mapControl.Dock = Pos.Fill;
            }


            // set up map control
            mapControl.RangeX = new Range(0, 1000);
            mapControl.RangeY = new Range(0, 1000);
            mapControl.AddDataSeries("map", Color.Red, Chart.SeriesType.Dots, 5, false);
            mapControl.AddDataSeries("path", Color.Blue, Chart.SeriesType.Line, 1, false);


            //
            greedyCrossoverBox.IsChecked = greedyCrossover;
            UpdateSettings();
            GenerateMap();
        }


        public override void Dispose()
        {
            // check if worker thread is running
            if (workerThread != null &&
                workerThread.IsAlive)
            {
                needToStop = true;
                while (!workerThread.Join(100))
                {
                    workerThread.Abort();
                    break;
                }
                workerThread = null;
            }

            base.Dispose();
        }


        // Thread safe updating of control's text property
		void SetText(Alt.GUI.Temporary.Gwen.Control.TextBox control, string text)
        {
            GUI.MethodInvoker m = delegate()
            {
                control.Text = text;
            };

            Invoke(m);
        }


        // Update settings controls
        void UpdateSettings()
        {
            citiesCountBox.Value = citiesCount;
            populationSizeBox.Value = populationSize;
            iterationsBox.Value = iterations;
        }


        // Enable/disale controls (safe for threading)
        void EnableControls(bool enable)
        {
            GUI.MethodInvoker m = delegate()
            {
                citiesCountBox.Enabled = enable;
                populationSizeBox.Enabled = enable;
                iterationsBox.Enabled = enable;
                selectionBox.Enabled = enable;

                generateMapButton.Enabled = enable;

                startButton.Enabled = enable;
                stopButton.Enabled = !enable;
            };

            Invoke(m);
        }


        // Generate new map for the Traivaling Salesman problem
        void GenerateMap()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);

            // create coordinates array
            map = new double[citiesCount, 2];

            for (int i = 0; i < citiesCount; i++)
            {
                map[i, 0] = rand.Next(1001);
                map[i, 1] = rand.Next(1001);
            }

            // set the map
            mapControl.UpdateDataSeries("map", map);
            // erase path if it is
            mapControl.UpdateDataSeries("path", null);
        }


        // On "Generate" button click - generate map
        void generateMapButton_Click(object sender, System.EventArgs e)
        {
            // get cities count
            try
            {
                citiesCount = Math.Max(5, Math.Min(50, (int)citiesCountBox.Value));
            }
            catch
            {
                citiesCount = 20;
            }
            citiesCountBox.Value = citiesCount;

            // regenerate map
            GenerateMap();
        }


        // On "Start" button click
        void startButton_Click(object sender, System.EventArgs e)
        {
            // get population size
            try
            {
                populationSize = Math.Max(10, Math.Min(100, (int)populationSizeBox.Value));
            }
            catch
            {
                populationSize = 40;
            }
            // iterations
            try
            {
                iterations = Math.Max(0, (int)iterationsBox.Value);
            }
            catch
            {
                iterations = 100;
            }
            // update settings controls
            UpdateSettings();

            selectionMethod = (int)selectionBox.SelectedItem.UserData;
            greedyCrossover = greedyCrossoverBox.IsChecked;

            // disable all settings controls except "Stop" button
            EnableControls(false);

#if !SILVERLIGHT && !UNITY_WEBPLAYER
            // run worker thread
            needToStop = false;
            workerThread = new Thread(new ThreadStart(SearchSolution));
            workerThread.Start();
#endif
        }


        // On "Stop" button click
        void stopButton_Click(object sender, System.EventArgs e)
        {
            // stop worker thread
            if (workerThread != null)
            {
                needToStop = true;
                while (!workerThread.Join(100))
                {
                }
                workerThread = null;
            }

#if SILVERLIGHT || UNITY_WEBPLAYER
            // enable settings controls
            EnableControls(true);
#endif
        }


        // Worker thread
        void SearchSolution()
        {
            // create fitness function
            TSPFitnessFunction fitnessFunction = new TSPFitnessFunction(map);
            // create population
            Population population = new Population(populationSize,
                (greedyCrossover) ? new TSPChromosome(map) : new PermutationChromosome(citiesCount),
                fitnessFunction,
                (selectionMethod == 0) ? (ISelectionMethod)new EliteSelection() :
                (selectionMethod == 1) ? (ISelectionMethod)new RankSelection() :
                (ISelectionMethod)new RouletteWheelSelection()
                );
            // iterations
            int i = 1;

            // path
            double[,] path = new double[citiesCount + 1, 2];

            // loop
            while (!needToStop)
            {
                // run one epoch of genetic algorithm
                population.RunEpoch();

                // display current path
                ushort[] bestValue = ((PermutationChromosome)population.BestChromosome).Value;

                for (int j = 0; j < citiesCount; j++)
                {
                    path[j, 0] = map[bestValue[j], 0];
                    path[j, 1] = map[bestValue[j], 1];
                }
                path[citiesCount, 0] = map[bestValue[0], 0];
                path[citiesCount, 1] = map[bestValue[0], 1];

                mapControl.UpdateDataSeries("path", path);

                // set current iteration's info
                SetText(currentIterationBox, i.ToString());
                SetText(pathLengthBox, fitnessFunction.PathLength(population.BestChromosome).ToString());

                // increase current iteration
                i++;

                //
                if ((iterations != 0) && (i > iterations))
                    break;
            }

            // enable settings controls
            EnableControls(true);
        }
    }
}
