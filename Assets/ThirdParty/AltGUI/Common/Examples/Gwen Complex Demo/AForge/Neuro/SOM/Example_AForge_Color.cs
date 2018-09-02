// Color Clustering using Kohonen SOM
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


namespace Alt.GUI.Demo
{
    class Example_AForge_Color : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 0;
                int y = 0;

                if (m_RightPanel != null)
                {
                    x = m_RightPanel.Width + m_RightPanel.Margin.Width;
                }

                return new SizeI(x, y);
            }
        }


		Alt.GUI.Temporary.Gwen.Control.Base m_RightPanel;
		Alt.GUI.Temporary.Gwen.Control.Base mapPanel;
		Alt.GUI.Temporary.Gwen.Control.Button randomizeButton;
		Alt.GUI.Temporary.Gwen.Control.GroupBox groupBox2;
		Alt.GUI.Temporary.Gwen.Control.TextBox currentIterationBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox rateBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox iterationsBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox radiusBox;
		Alt.GUI.Temporary.Gwen.Control.Button stopButton;
		Alt.GUI.Temporary.Gwen.Control.Button startButton;


        DistanceNetwork network;
        Bitmap mapBitmap;
        Random rand = new Random();

        int iterations = 5000;
        double learningRate = 0.1;
        double radius = 15;

        Thread workerThread = null;
        volatile bool needToStop = false;


        public Example_AForge_Color(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            //  GUI
            {
                m_RightPanel = new Base(this);
                {
                    m_RightPanel.Dock = Pos.Right;
                    m_RightPanel.Width = 140;
                    m_RightPanel.Margin = new Margin(5, 0, 0, 0);


					Alt.GUI.Temporary.Gwen.Control.GroupBox groupBox = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_RightPanel);
                    {
                        groupBox.Location = new PointI(0, 0);
                        groupBox.Size = new SizeI(140, 50);
                        groupBox.Text = "Map";


						randomizeButton = new Alt.GUI.Temporary.Gwen.Control.Button(groupBox);
                        randomizeButton.Location = new PointI(5, 7);
                        randomizeButton.Text = "Generate";
                        randomizeButton.Width = 120;
                        randomizeButton.Click += new System.EventHandler(randomizeButton_Click);
                        randomizeButton.NormalTextColor = Color.Blue;
                    }


					groupBox2 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_RightPanel);
                    {
                        groupBox2.Location = new PointI(0, 60);
                        groupBox2.Size = new SizeI(140, 219);
                        groupBox2.Text = "Neural Network";


						Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 10);
                        label.AutoSizeToContents = true;
                        label.Text = "Iteraions:";

						iterationsBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox2);
                        iterationsBox.Location = new PointI(0, 30);
                        iterationsBox.Width = 130;

						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 55);
                        label.AutoSizeToContents = true;
                        label.Text = "Initial learning rate:";

						rateBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox2);
                        rateBox.Location = new PointI(0, 75);
                        rateBox.Width = 130;

						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 100);
                        label.AutoSizeToContents = true;
                        label.Text = "Initial radius:";

						radiusBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox2);
                        radiusBox.Location = new PointI(0, 120);
                        radiusBox.Width = 130;


						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
                        label.Location = new PointI(0, 155);
                        label.AutoSizeToContents = true;
                        label.Text = "Current iteration:";

						currentIterationBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox2);
                        currentIterationBox.Location = new PointI(0, 175);
                        currentIterationBox.Width = 130;
                        currentIterationBox.ReadOnly = true;
                    }


					startButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_RightPanel);
					stopButton = new Alt.GUI.Temporary.Gwen.Control.Button(m_RightPanel);

                    startButton.Location = new PointI(5, 295);
                    startButton.Width = 130;
                    startButton.Text = "Start";
                    startButton.Click += new System.EventHandler(startButton_Click);
                    startButton.NormalTextColor = Color.Green;

                    stopButton.Enabled = false;
                    stopButton.Location = new PointI(5, 320);
                    stopButton.Width = 130;
                    stopButton.Text = "Stop";
                    stopButton.Click += new System.EventHandler(stopButton_Click);
                    stopButton.NormalTextColor = Color.Red * 0.8;
                }


                mapPanel = new Base(this);
                mapPanel.Dock = Pos.Fill;
                mapPanel.Paint += new GUI.PaintEventHandler(this.mapPanel_Paint);
            }


            // Create network
            network = new DistanceNetwork(3, 100 * 100);

            // Create map bitmap
            mapBitmap = new Bitmap(200, 200, PixelFormat.Format24bppRgb);

            //
            RandomizeNetwork();
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
            iterationsBox.Text = iterations.ToString();
            rateBox.Text = learningRate.ToString();
            radiusBox.Text = radius.ToString();
        }


        // On "Rundomize" button clicked
        void randomizeButton_Click(object sender, System.EventArgs e)
        {
            RandomizeNetwork();
        }


        // Radnomize weights of network
        void RandomizeNetwork()
        {
            Neuron.RandRange = new Range(0, 255);

            // randomize net
            network.Randomize();

            // update map
            UpdateMap();
        }


        // Update map from network weights
        void UpdateMap()
        {
            // lock
            System.Threading.Monitor.Enter(this);

            // lock bitmap
            BitmapData mapData = mapBitmap.LockBits(new RectI(0, 0, 200, 200),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = mapData.Stride;
            int offset = stride - 200 * 3;
            Layer layer = network.Layers[0];

                byte[] ptr = mapData.Scan0;
                int ptr_index = 0;

                // for all rows
                for (int y = 0, i = 0; y < 100; y++)
                {
                    // for all pixels
                    for (int x = 0; x < 100; x++, i++, ptr_index += 6)
                    {
                        Neuron neuron = layer.Neurons[i];

                        // red
                        ptr[ptr_index + 2] = ptr[ptr_index + 2 + 3] = ptr[ptr_index + 2 + stride] = ptr[ptr_index + 2 + 3 + stride] =
                            (byte)Math.Max(0, Math.Min(255, neuron.Weights[0]));
                        // green
                        ptr[ptr_index + 1] = ptr[ptr_index + 1 + 3] = ptr[ptr_index + 1 + stride] = ptr[ptr_index + 1 + 3 + stride] =
                            (byte)Math.Max(0, Math.Min(255, neuron.Weights[1]));
                        // blue
                        ptr[ptr_index + 0] = ptr[ptr_index + 0 + 3] = ptr[ptr_index + 0 + stride] = ptr[ptr_index + 0 + 3 + stride] =
                            (byte)Math.Max(0, Math.Min(255, neuron.Weights[2]));
                    }

                    ptr_index += offset;
                    ptr_index += stride;
                }

            // unlock image
            mapBitmap.UnlockBits(mapData);

            // unlock
            System.Threading.Monitor.Exit(this);

            // invalidate maps panel
            mapPanel.Invalidate();
        }


        // Paint map
        void mapPanel_Paint(object sender, GUI.PaintEventArgs e)
        {
            if (mapBitmap == null)
            {
                return;
            }

            Graphics g = e.Graphics;

            // lock
            System.Threading.Monitor.Enter(this);

            // drat image
            g.DrawImage(mapBitmap, (ClientWidth - mapBitmap.PixelWidth) / 2, (ClientHeight - mapBitmap.PixelHeight) / 2);

            // unlock
            System.Threading.Monitor.Exit(this);
        }


        // Enable/disale controls (safe for threading)
        void EnableControls(bool enable)
        {
            GUI.MethodInvoker m = delegate()
            {
                iterationsBox.Enabled = enable;
                rateBox.Enabled = enable;
                radiusBox.Enabled = enable;

                startButton.Enabled = enable;
                randomizeButton.Enabled = enable;
                stopButton.Enabled = !enable;
            };

            Invoke(m);
        }


        // On "Start" button click
        void startButton_Click(object sender, System.EventArgs e)
        {
            // get iterations count
            try
            {
                iterations = Math.Max(10, Math.Min(1000000, int.Parse(iterationsBox.Text)));
            }
            catch
            {
                iterations = 5000;
            }
            // get learning rate
            try
            {
                learningRate = Math.Max(0.00001, Math.Min(1.0, double.Parse(rateBox.Text)));
            }
            catch
            {
                learningRate = 0.1;
            }
            // get radius
            try
            {
                radius = Math.Max(5, Math.Min(75, int.Parse(radiusBox.Text)));
            }
            catch
            {
                radius = 15;
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
            // create learning algorithm
            SOMLearning trainer = new SOMLearning(network);

            // input
            double[] input = new double[3];

            double fixedLearningRate = learningRate / 10;
            double driftingLearningRate = fixedLearningRate * 9;

            // iterations
            int i = 0;

            // loop
            while (!needToStop)
            {
                trainer.LearningRate = driftingLearningRate * (iterations - i) / iterations + fixedLearningRate;
                trainer.LearningRadius = (double)radius * (iterations - i) / iterations;

                input[0] = rand.Next(256);
                input[1] = rand.Next(256);
                input[2] = rand.Next(256);

                trainer.Run(input);

                // update map once per 50 iterations
                if ((i % 10) == 9)
                {
                    UpdateMap();
                }

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
