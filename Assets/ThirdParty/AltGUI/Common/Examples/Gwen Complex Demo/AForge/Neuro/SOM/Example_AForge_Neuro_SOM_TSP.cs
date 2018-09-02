// Traveling Salesman Problem using Elastic Net
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
using AForge.Neuro;
using AForge.Neuro.Learning;
using Alt.GUI.AForge.Temporary.Gwen;


namespace Alt.GUI.Demo
{
    class Example_AForge_Neuro_SOM_TSP : Example__Base
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


		Alt.GUI.Temporary.Gwen.Control.Base m_TopPanel;
		Alt.GUI.Temporary.Gwen.Control.Base m_RightPanel;
        Chart chart;
		Alt.GUI.Temporary.Gwen.Control.Label label1;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown citiesCountBox;
		Alt.GUI.Temporary.Gwen.Control.Button generateMapButton;
		Alt.GUI.Temporary.Gwen.Control.GroupBox groupBox2;
		Alt.GUI.Temporary.Gwen.Control.TextBox neuronsBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox currentIterationBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox rateBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox iterationsBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox radiusBox;
		Alt.GUI.Temporary.Gwen.Control.Button stopButton;
		Alt.GUI.Temporary.Gwen.Control.Button startButton;


        int citiesCount = 10;
        int neurons = 20;
        int iterations = 5000;//500;
        double learningRate = 0.5;
        double learningRadius = 0.5;

        double[,] map = null;
        Random rand = new Random();

        Thread workerThread = null;
        volatile bool needToStop = false;


        public Example_AForge_Neuro_SOM_TSP(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            //  GUI
            {
                m_TopPanel = new Base(this);
                {
                    m_TopPanel.Dock = Pos.Top;
                    m_TopPanel.Height = 30;

					Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(m_TopPanel);
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


				m_RightPanel = new Alt.GUI.Temporary.Gwen.Control.Base(this);
                {
                    m_RightPanel.Dock = Pos.Right;
                    m_RightPanel.Width = 140;
                    m_RightPanel.Margin = new Margin(5, 0, 0, 0);

					groupBox2 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_RightPanel);
                    {
                        groupBox2.Location = new PointI(0, 0);
                        groupBox2.Size = new SizeI(140, 273);
                        groupBox2.Text = "Neural Network";


						Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 10);
                        label.AutoSizeToContents = true;
                        label.Text = "Neurons:";

						neuronsBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox2);
                        neuronsBox.Location = new PointI(0, 30);
                        neuronsBox.Width = 130;


						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 65);
                        label.AutoSizeToContents = true;
                        label.Text = "Iteraions:";

						iterationsBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox2);
                        iterationsBox.Location = new PointI(0, 85);
                        iterationsBox.Width = 130;

						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 110);
                        label.AutoSizeToContents = true;
                        label.Text = "Initial learning rate:";

						rateBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox2);
                        rateBox.Location = new PointI(0, 130);
                        rateBox.Width = 130;

						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 155);
                        label.AutoSizeToContents = true;
                        label.Text = "Learning radius:";

						radiusBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox2);
                        radiusBox.Location = new PointI(0, 175);
                        radiusBox.Width = 130;


						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 210);
                        label.AutoSizeToContents = true;
                        label.Text = "Current iteration:";

						currentIterationBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox2);
                        currentIterationBox.Location = new PointI(0, 230);
                        currentIterationBox.Width = 130;
                        currentIterationBox.ReadOnly = true;
                    }


					startButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_RightPanel);
					stopButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_RightPanel);

                    startButton.Location = new PointI(5, 300);
                    startButton.Width = 130;
                    startButton.Text = "Start";
                    startButton.Click += new System.EventHandler(startButton_Click);
                    startButton.NormalTextColor = Color.Green;

                    stopButton.Enabled = false;
                    stopButton.Location = new PointI(5, 325);
                    stopButton.Width = 130;
                    stopButton.Text = "Stop";
                    stopButton.Click += new System.EventHandler(stopButton_Click);
                    stopButton.NormalTextColor = Color.Red * 0.8;
                }


                chart = new Chart(this);
                chart.Dock = Pos.Fill;
            }


            // initialize chart
            chart.AddDataSeries("cities", Color.Red, Chart.SeriesType.Dots, 5, false);
            chart.AddDataSeries("path", Color.Blue, Chart.SeriesType.Line, 1, false);
            chart.RangeX = new Range(0, 1000);
            chart.RangeY = new Range(0, 1000);

            //
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
            neuronsBox.Text = neurons.ToString();
            iterationsBox.Text = iterations.ToString();
            rateBox.Text = learningRate.ToString();
            radiusBox.Text = learningRadius.ToString();
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
            chart.UpdateDataSeries("cities", map);
            // erase path if it is
            chart.UpdateDataSeries("path", null);
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


        // Enable/disale controls (safe for threading)
        void EnableControls(bool enable)
        {
            GUI.MethodInvoker m = delegate()
            {
                neuronsBox.Enabled = enable;
                iterationsBox.Enabled = enable;
                rateBox.Enabled = enable;
                radiusBox.Enabled = enable;

                startButton.Enabled = enable;
                generateMapButton.Enabled = enable;
                stopButton.Enabled = !enable;
            };

            Invoke(m);
        }


        // On "Start" button click
        void startButton_Click(object sender, System.EventArgs e)
        {
            // get network size
            try
            {
                neurons = Math.Max(5, Math.Min(50, int.Parse(neuronsBox.Text)));
            }
            catch
            {
                neurons = 20;
            }
            // get iterations count
            try
            {
                iterations = Math.Max(10, Math.Min(1000000, int.Parse(iterationsBox.Text)));
            }
            catch
            {
                iterations = 500;
            }
            // get learning rate
            try
            {
                learningRate = Math.Max(0.00001, Math.Min(1.0, double.Parse(rateBox.Text)));
            }
            catch
            {
                learningRate = 0.5;
            }
            // get learning radius
            try
            {
                learningRadius = Math.Max(0.00001, Math.Min(1.0, double.Parse(radiusBox.Text)));
            }
            catch
            {
                learningRadius = 0.5;
            }
            // update settings controls
            UpdateSettings();

            // disable all settings controls except "Stop" button
            EnableControls(false);

            // run worker thread
            needToStop = false;
            workerThread = new Thread(new ThreadStart(SearchSolution));
            workerThread.Start();
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
        }


        // Worker thread
        void SearchSolution()
        {
            // set random generators range
            Neuron.RandRange = new Range(0, 1000);

            // create network
            DistanceNetwork network = new DistanceNetwork(2, neurons);

            // create learning algorithm
            ElasticNetworkLearning trainer = new ElasticNetworkLearning(network);

            double fixedLearningRate = learningRate / 20;
            double driftingLearningRate = fixedLearningRate * 19;

            // path
            double[,] path = new double[neurons + 1, 2];

            // input
            double[] input = new double[2];

            // iterations
            int i = 0;

            // loop
            while (!needToStop)
            {
                // update learning speed & radius
                trainer.LearningRate = driftingLearningRate * (iterations - i) / iterations + fixedLearningRate;
                trainer.LearningRadius = learningRadius * (iterations - i) / iterations;

                // set network input
                int currentCity = rand.Next(citiesCount);
                input[0] = map[currentCity, 0];
                input[1] = map[currentCity, 1];

                // run one training iteration
                trainer.Run(input);

                // show current path
                for (int j = 0; j < neurons; j++)
                {
                    path[j, 0] = network.Layers[0].Neurons[j].Weights[0];
                    path[j, 1] = network.Layers[0].Neurons[j].Weights[1];
                }
                path[neurons, 0] = network.Layers[0].Neurons[0].Weights[0];
                path[neurons, 1] = network.Layers[0].Neurons[0].Weights[1];

                chart.UpdateDataSeries("path", path);

                // increase current iteration
                i++;

                // set current iteration's info
                SetText(currentIterationBox, i.ToString());

                // stop ?
                if (i >= iterations)
                    break;
            }

            // enable settings controls
            EnableControls(true);
        }
    }
}
