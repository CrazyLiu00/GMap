// Kohonen SOM 2D Organizing
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
    class Example_AForge_2DOrganizing : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 0;
                int y = 0;

                if (m_TopPanel2 != null &&
                    m_RightPanel != null)
                {
                    x = m_RightPanel.Width + m_RightPanel.Margin.Width - 11;
                    y = m_TopPanel2.Height + m_TopPanel2.Margin.Height;
                }

                return new SizeI(x, y);
            }
        }


		Alt.GUI.Temporary.Gwen.Control.Base m_TopPanel1;
		Alt.GUI.Temporary.Gwen.Control.Base m_TopPanel2;
		Alt.GUI.Temporary.Gwen.Control.Base m_RightPanel;
		Alt.GUI.Temporary.Gwen.Control.Base pointsPanel;
		Alt.GUI.Temporary.Gwen.Control.Base mapPanel;
		Alt.GUI.Temporary.Gwen.Control.Button generateButton;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox showConnectionsCheck;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox showInactiveCheck;
		Alt.GUI.Temporary.Gwen.Control.GroupBox groupBox2;
		Alt.GUI.Temporary.Gwen.Control.TextBox sizeBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox currentIterationBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox rateBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox iterationsBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox radiusBox;
		Alt.GUI.Temporary.Gwen.Control.Button stopButton;
		Alt.GUI.Temporary.Gwen.Control.Button startButton;


        const int groupRadius = 20;
        const int pointsCount = 100;
        int[,] points = new int[pointsCount, 2];	// x, y
        double[][] trainingSet = new double[pointsCount][];
        int[, ,] map;

        int networkSize = 15;
        int iterations = 500;
        double learningRate = 0.3;
        int learningRadius = 3;

        Random rand = new Random();
        Thread workerThread = null;
        volatile bool needToStop = false;


        public Example_AForge_2DOrganizing(Base parent)
            : base(parent)
        {
            //  GUI
            {
				m_RightPanel = new Alt.GUI.Temporary.Gwen.Control.Base(this);
                {
                    m_RightPanel.Dock = Pos.Right;
                    m_RightPanel.Width = 140;
                    m_RightPanel.Margin = new Margin(5, 0, 0, 0);

					groupBox2 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_RightPanel);
                    {
                        groupBox2.Location = new PointI(0, 0);
                        groupBox2.Size = new SizeI(140, 287);
                        groupBox2.Text = "Neural Network";


						Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 10);
                        label.AutoSizeToContents = true;
                        label.Text = "Size:";

						sizeBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox2);
                        sizeBox.Location = new PointI(0, 30);
                        sizeBox.Width = 130;

						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 50);
                        label.AutoSizeToContents = true;
                        label.Text = "(neurons count = size * size)";


						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 80);
                        label.AutoSizeToContents = true;
                        label.Text = "Iteraions:";

						iterationsBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox2);
                        iterationsBox.Location = new PointI(0, 100);
                        iterationsBox.Width = 130;

						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 125);
                        label.AutoSizeToContents = true;
                        label.Text = "Initial learning rate:";

						rateBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox2);
                        rateBox.Location = new PointI(0, 145);
                        rateBox.Width = 130;

						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 170);
                        label.AutoSizeToContents = true;
                        label.Text = "Initial radius:";

						radiusBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox2);
                        radiusBox.Location = new PointI(0, 190);
                        radiusBox.Width = 130;


						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 225);
                        label.AutoSizeToContents = true;
                        label.Text = "Current iteration:";

						currentIterationBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox2);
                        currentIterationBox.Location = new PointI(0, 245);
                        currentIterationBox.Width = 130;
                        currentIterationBox.ReadOnly = true;
                    }


					startButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_RightPanel);
					stopButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_RightPanel);

                    startButton.Location = new PointI(5, 315);
                    startButton.Width = 130;
                    startButton.Text = "Start";
                    startButton.Click += new System.EventHandler(startButton_Click);
                    startButton.NormalTextColor = Color.Green;

                    stopButton.Enabled = false;
                    stopButton.Location = new PointI(5, 340);
                    stopButton.Width = 130;
                    stopButton.Text = "Stop";
                    stopButton.Click += new System.EventHandler(stopButton_Click);
                    stopButton.NormalTextColor = Color.Red * 0.8;


					Alt.GUI.Temporary.Gwen.Control.VerticalSplitter splitter = new Alt.GUI.Temporary.Gwen.Control.VerticalSplitter(this);
                    {
                        splitter.Dock = Pos.Fill;
                        splitter.SetHValue(0.5);


						Alt.GUI.Temporary.Gwen.Control.Base leftPanel = new Alt.GUI.Temporary.Gwen.Control.Base(splitter);
                        {
                            leftPanel.Dock = Pos.Fill;

                            m_TopPanel1 = new Base(leftPanel);
                            {
                                m_TopPanel1.Dock = Pos.Top;
                                m_TopPanel1.Height = 70;

								Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(m_TopPanel1);
                                label.Location = new PointI(0, 2);
                                label.AutoSizeToContents = true;
                                label.TextColor = Color.LightPink;
                                label.Text = "Points";


								generateButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_TopPanel1);
                                generateButton.Location = new PointI(0, 25);
                                generateButton.Text = "Generate";
                                generateButton.AutoSizeToContents = true;
                                generateButton.Click += new System.EventHandler(generateButton_Click);
                                generateButton.NormalTextColor = Color.Blue;
                            }


                            pointsPanel = new Base(leftPanel);
                            pointsPanel.Dock = Pos.Fill;
                            pointsPanel.Paint += new GUI.PaintEventHandler(this.pointsPanel_Paint);
                            pointsPanel.ClientBackColor = Color.White;
                            pointsPanel.DrawBorder = true;
                            pointsPanel.BorderColor = Color.DodgerBlue;


                            splitter.SetPanel(0, leftPanel);
                        }


                        Base rightPanel = new Base(splitter);
                        {
                            rightPanel.Dock = Pos.Fill;

                            m_TopPanel2 = new Base(rightPanel);
                            {
                                m_TopPanel2.Dock = Pos.Top;
                                m_TopPanel2.Height = 70;

								Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(m_TopPanel2);
                                label.Location = new PointI(0, 2);
                                label.AutoSizeToContents = true;
                                label.TextColor = Color.LightPink;
                                label.Text = "Map";


								showConnectionsCheck = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(m_TopPanel2);
                                showConnectionsCheck.Location = new PointI(0, 23);
                                showConnectionsCheck.Text = "Show Connections";
                                showConnectionsCheck.IsChecked = true;
                                showConnectionsCheck.CheckedChanged += new System.EventHandler(this.showConnectionsCheck_CheckedChanged);

								showInactiveCheck = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(m_TopPanel2);
                                showInactiveCheck.Location = new PointI(0, 45);
                                showInactiveCheck.Text = "Show Inactive Neurons";
                                showInactiveCheck.IsChecked = true;
                                showInactiveCheck.CheckedChanged += new System.EventHandler(this.showInactiveCheck_CheckedChanged);
                            }


                            mapPanel = new Base(rightPanel);
                            mapPanel.Dock = Pos.Fill;
                            mapPanel.Paint += new GUI.PaintEventHandler(this.mapPanel_Paint);
                            mapPanel.ClientBackColor = Color.White;
                            mapPanel.DrawBorder = true;
                            mapPanel.BorderColor = Color.DodgerBlue;


                            splitter.SetPanel(1, rightPanel);
                        }
                    }
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            GeneratePoints();
            UpdateSettings();
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
            sizeBox.Text = networkSize.ToString();
            iterationsBox.Text = iterations.ToString();
            rateBox.Text = learningRate.ToString();
            radiusBox.Text = learningRadius.ToString();
        }


        // On "Generate" button click
        void generateButton_Click(object sender, System.EventArgs e)
        {
            GeneratePoints();
        }


        // Generate point
        void GeneratePoints()
        {
            int width = pointsPanel.ClientRectangle.Width;
            int height = pointsPanel.ClientRectangle.Height;
            int diameter = groupRadius * 2;

            // generate groups of ten points
            for (int i = 0; i < pointsCount; )
            {
                int cx = rand.Next(width);
                int cy = rand.Next(height);

                // generate group
                for (int j = 0; (i < pointsCount) && (j < 10); )
                {
                    int x = cx + rand.Next(diameter) - groupRadius;
                    int y = cy + rand.Next(diameter) - groupRadius;

                    // check if wee are not out
                    if ((x < 0) || (y < 0) || (x >= width) || (y >= height))
                    {
                        continue;
                    }

                    // add point
                    points[i, 0] = x;
                    points[i, 1] = y;

                    j++;
                    i++;
                }
            }

            map = null;
            pointsPanel.Invalidate();
            mapPanel.Invalidate();
        }


        // Paint points
        void pointsPanel_Paint(object sender, GUI.PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            using (Brush brush = new SolidColorBrush(Color.Blue))
            {
                // draw all points
                for (int i = 0, n = points.GetLength(0); i < n; i++)
                {
                    g.FillEllipse(brush, points[i, 0] - 2, points[i, 1] - 2, 5, 5);
                }
            }
        }


        // Paint map
        void mapPanel_Paint(object sender, GUI.PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (map != null)
            {
                //
                bool showConnections = showConnectionsCheck.IsChecked;
                bool showInactive = showInactiveCheck.IsChecked;

                // pens and brushes
                Brush brush = new SolidColorBrush(Color.Blue);
                Brush brushGray = new SolidColorBrush(Color.FromArgb(192, 192, 192));
                Pen pen = new Pen(Color.Blue, 1);
                Pen penGray = new Pen(Color.FromArgb(192, 192, 192), 1);

                // lock
                System.Threading.Monitor.Enter(this);

                if (showConnections)
                {
                    // draw connections
                    for (int i = 0, n = map.GetLength(0); i < n; i++)
                    {
                        for (int j = 0, k = map.GetLength(1); j < k; j++)
                        {
                            if ((!showInactive) && (map[i, j, 2] == 0))
                                continue;

                            // left
                            if ((i > 0) && ((showInactive) || (map[i - 1, j, 2] == 1)))
                            {
                                g.DrawLine(((map[i, j, 2] == 0) || (map[i - 1, j, 2] == 0)) ? penGray : pen, map[i, j, 0], map[i, j, 1], map[i - 1, j, 0], map[i - 1, j, 1]);
                            }

                            // right
                            if ((i < n - 1) && ((showInactive) || (map[i + 1, j, 2] == 1)))
                            {
                                g.DrawLine(((map[i, j, 2] == 0) || (map[i + 1, j, 2] == 0)) ? penGray : pen, map[i, j, 0], map[i, j, 1], map[i + 1, j, 0], map[i + 1, j, 1]);
                            }

                            // top
                            if ((j > 0) && ((showInactive) || (map[i, j - 1, 2] == 1)))
                            {
                                g.DrawLine(((map[i, j, 2] == 0) || (map[i, j - 1, 2] == 0)) ? penGray : pen, map[i, j, 0], map[i, j, 1], map[i, j - 1, 0], map[i, j - 1, 1]);
                            }

                            // bottom
                            if ((j < k - 1) && ((showInactive) || (map[i, j + 1, 2] == 1)))
                            {
                                g.DrawLine(((map[i, j, 2] == 0) || (map[i, j + 1, 2] == 0)) ? penGray : pen, map[i, j, 0], map[i, j, 1], map[i, j + 1, 0], map[i, j + 1, 1]);
                            }
                        }
                    }
                }

                // draw the map
                for (int i = 0, n = map.GetLength(0); i < n; i++)
                {
                    for (int j = 0, k = map.GetLength(1); j < k; j++)
                    {
                        if ((!showInactive) && (map[i, j, 2] == 0))
                            continue;

                        // draw the point
                        g.FillEllipse((map[i, j, 2] == 0) ? brushGray : brush, map[i, j, 0] - 2, map[i, j, 1] - 2, 5, 5);
                    }
                }

                // unlock
                System.Threading.Monitor.Exit(this);

                brush.Dispose();
                brushGray.Dispose();
                pen.Dispose();
                penGray.Dispose();
            }
        }


        // Enable/disale controls (safe for threading)
        void EnableControls(bool enable)
        {
            GUI.MethodInvoker m = delegate()
            {
                sizeBox.Enabled = enable;
                iterationsBox.Enabled = enable;
                rateBox.Enabled = enable;
                radiusBox.Enabled = enable;

                startButton.Enabled = enable;
                generateButton.Enabled = enable;
                stopButton.Enabled = !enable;
            };

            Invoke(m);
        }


        // Show/hide connections on map
        void showConnectionsCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            mapPanel.Invalidate();
        }


        // Show/hide inactive neurons on map
        void showInactiveCheck_CheckedChanged(object sender, System.EventArgs e)
        {
            mapPanel.Invalidate();
        }


        // On "Start" button click
        void startButton_Click(object sender, System.EventArgs e)
        {
            // get network size
            try
            {
                networkSize = Math.Max(5, Math.Min(50, int.Parse(sizeBox.Text)));
            }
            catch
            {
                networkSize = 15;
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
                learningRate = 0.3;
            }
            // get radius
            try
            {
                learningRadius = Math.Max(1, Math.Min(30, int.Parse(radiusBox.Text)));
            }
            catch
            {
                learningRadius = 3;
            }
            // update settings controls
            UpdateSettings();

            // disable all settings controls except "Stop" button
            EnableControls(false);

            // generate training set
            for (int i = 0; i < pointsCount; i++)
            {
                // create new training sample
                trainingSet[i] = new double[2] { points[i, 0], points[i, 1] };
            }

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
            Neuron.RandRange = new Range(0, Math.Max(pointsPanel.ClientRectangle.Width, pointsPanel.ClientRectangle.Height));

            // create network
            DistanceNetwork network = new DistanceNetwork(2, networkSize * networkSize);

            // create learning algorithm
            SOMLearning trainer = new SOMLearning(network, networkSize, networkSize);

            // create map
            map = new int[networkSize, networkSize, 3];

            double fixedLearningRate = learningRate / 10;
            double driftingLearningRate = fixedLearningRate * 9;

            // iterations
            int i = 0;

            // loop
            while (!needToStop)
            {
                trainer.LearningRate = driftingLearningRate * (iterations - i) / iterations + fixedLearningRate;
                trainer.LearningRadius = (double)learningRadius * (iterations - i) / iterations;

                // run training epoch
                trainer.RunEpoch(trainingSet);

                // update map
                UpdateMap(network);

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


        // Update map
        void UpdateMap(DistanceNetwork network)
        {
            // get first layer
            Layer layer = network.Layers[0];

            // lock
            System.Threading.Monitor.Enter(this);

            // run through all neurons
            for (int i = 0; i < layer.Neurons.Length; i++)
            {
                Neuron neuron = layer.Neurons[i];

                int x = i % networkSize;
                int y = i / networkSize;

                map[y, x, 0] = (int)neuron.Weights[0];
                map[y, x, 1] = (int)neuron.Weights[1];
                map[y, x, 2] = 0;
            }

            // collect active neurons
            for (int i = 0; i < pointsCount; i++)
            {
                network.Compute(trainingSet[i]);
                int w = network.GetWinner();

                map[w / networkSize, w % networkSize, 2] = 1;
            }

            // unlock
            System.Threading.Monitor.Exit(this);

            //
            mapPanel.Invalidate();
        }
    }
}
