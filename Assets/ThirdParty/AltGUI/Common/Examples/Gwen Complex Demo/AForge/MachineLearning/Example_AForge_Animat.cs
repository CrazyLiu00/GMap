// Animat sample application
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright � AForge.NET, 2006-2011
// contacts@aforgenet.com

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Alt.ComponentModel;
using Alt.IO;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;
using Alt.Threading;

using AForge;
using AForge.MachineLearning;
using Alt.GUI.Demo.AForge.Animat;


namespace Alt.GUI.Demo
{
    class Example_AForge_Animat : Example__Base
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
                    y = m_TopPanel.Height - 20;
                }

                return new SizeI(x, y);
            }
        }


		Alt.GUI.Temporary.Gwen.Control.Base m_TopPanel;
		Alt.GUI.Temporary.Gwen.Control.Base m_RightPanel;
        CellWorld cellWorld;
		Alt.GUI.Temporary.Gwen.Control.TextBox worldSizeBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox iterationBox;
		Alt.GUI.Temporary.Gwen.Control.Button stopButton;
		Alt.GUI.Temporary.Gwen.Control.Button startLearningButton;
		Alt.GUI.Temporary.Gwen.Control.Button showSolutionButton;
		Alt.GUI.Temporary.Gwen.Control.TextBox learningRateBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox explorationRateBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox iterationsBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox goalRewardBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox wallRewardBox;
		Alt.GUI.Temporary.Gwen.Control.TextBox moveRewardBox;
		Alt.GUI.Temporary.Gwen.Control.ComboBox algorithmCombo;

		Alt.GUI.Temporary.Gwen.Control.Button loadButton1;
		Alt.GUI.Temporary.Gwen.Control.Button loadButton2;
		Alt.GUI.Temporary.Gwen.Control.Button loadButton3;
		Alt.GUI.Temporary.Gwen.Control.Button loadButton4;
		Alt.GUI.Temporary.Gwen.Control.Button loadButton5;


        // map and its dimension
        int[,] map = null;
        int[,] mapToDisplay = null;
        int mapWidth;
        int mapHeight;

        // agent' start and stop position
        int agentStartX;
        int agentStartY;
        int agentStopX;
        int agentStopY;

        // flag to stop background job
        volatile bool needToStop = false;

        // worker thread
        Thread workerThread = null;

        // learning settings
        int learningIterations = 100;
        double explorationRate = 0.5;
        double learningRate = 0.5;

        double moveReward = 0;
        double wallReward = -1;
        double goalReward = 1;

        // Q-Learning algorithm
        QLearning qLearning = null;
        // Sarsa algorithm
        Sarsa sarsa = null;


        public Example_AForge_Animat(Base parent)
            : base(parent)
        {
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


					label = new Alt.GUI.Temporary.Gwen.Control.Label(m_TopPanel);
                    label.Location = new PointI(33, 2);
                    label.Text = "World size:";
                    label.AutoSizeToContents = true;


					worldSizeBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(m_TopPanel);
                    worldSizeBox.Location = new PointI(102, 0);
                    worldSizeBox.ReadOnly = true;
                    worldSizeBox.Width = 40;


					loadButton1 = new Alt.GUI.Temporary.Gwen.Control.Button(m_TopPanel);
                    loadButton1.Location = new PointI(160, 0);
                    loadButton1.Width = 80;
                    loadButton1.Text = "Sample 1";
                    loadButton1.Click += new System.EventHandler(loadButton1_Click);
                    loadButton1.NormalTextColor = Color.Red * 0.8;

					loadButton2 = new Alt.GUI.Temporary.Gwen.Control.Button(m_TopPanel);
                    loadButton2.Location = new PointI(250, 0);
                    loadButton2.Width = 80;
                    loadButton2.Text = "Sample 2";
                    loadButton2.Click += new System.EventHandler(loadButton2_Click);
                    loadButton2.NormalTextColor = Color.Brown;

					loadButton3 = new Alt.GUI.Temporary.Gwen.Control.Button(m_TopPanel);
                    loadButton3.Location = new PointI(340, 0);
                    loadButton3.Width = 80;
                    loadButton3.Text = "Test 1";
                    loadButton3.Click += new System.EventHandler(loadButton3_Click);
                    loadButton3.NormalTextColor = Color.Green;

					loadButton4 = new Alt.GUI.Temporary.Gwen.Control.Button(m_TopPanel);
                    loadButton4.Location = new PointI(430, 0);
                    loadButton4.Width = 80;
                    loadButton4.Text = "Test 2";
                    loadButton4.Click += new System.EventHandler(loadButton4_Click);
                    loadButton4.NormalTextColor = Color.Blue;

					loadButton5 = new Alt.GUI.Temporary.Gwen.Control.Button(m_TopPanel);
                    loadButton5.Location = new PointI(520, 0);
                    loadButton5.Width = 80;
                    loadButton5.Text = "Test 3";
                    loadButton5.Click += new System.EventHandler(loadButton5_Click);
                    loadButton5.NormalTextColor = Color.Violet * 0.8;
                }


                m_RightPanel = new Base(this);
                {
                    m_RightPanel.Dock = Pos.Right;
                    m_RightPanel.Width = 140;
                    m_RightPanel.Margin = new Margin(5, 0, 0, 0);

					Alt.GUI.Temporary.Gwen.Control.GroupBox groupBox = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_RightPanel);
                    {
                        groupBox.Location = new PointI(0, 0);
                        groupBox.Size = new SizeI(140, 363);
                        groupBox.Text = "Settings";


						Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox);
                        label.Location = new PointI(0, 10);
                        label.AutoSizeToContents = true;
                        label.Text = "Learning algorithm:";

						algorithmCombo = new Alt.GUI.Temporary.Gwen.Control.ComboBox(groupBox);
                        algorithmCombo.Location = new PointI(0, 30);
                        algorithmCombo.Width = 130;
                        algorithmCombo.AddItem("Q-Learning").UserData = 0;
                        algorithmCombo.AddItem("Sarsa").UserData = 1;


						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox);
                        label.Location = new PointI(0, 65);
                        label.AutoSizeToContents = true;
                        label.Text = "Initial exploration rate:";

						explorationRateBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox);
                        explorationRateBox.Location = new PointI(0, 85);
                        explorationRateBox.Width = 130;

						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox);
                        label.Location = new PointI(0, 110);
                        label.AutoSizeToContents = true;
                        label.Text = "Initial learning rate:";

						learningRateBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox);
                        learningRateBox.Location = new PointI(0, 130);
                        learningRateBox.Width = 130;

						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox);
                        label.Location = new PointI(0, 155);
                        label.AutoSizeToContents = true;
                        label.Text = "Learning iterations:";

						iterationsBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox);
                        iterationsBox.Location = new PointI(0, 175);
                        iterationsBox.Width = 130;


						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox);
                        label.Location = new PointI(0, 210);
                        label.AutoSizeToContents = true;
                        label.Text = "Move reward:";

						moveRewardBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox);
                        moveRewardBox.Location = new PointI(0, 230);
                        moveRewardBox.Width = 130;
                        moveRewardBox.ReadOnly = true;

						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox);
                        label.Location = new PointI(0, 255);
                        label.AutoSizeToContents = true;
                        label.Text = "Wall reward:";

						wallRewardBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox);
                        wallRewardBox.Location = new PointI(0, 275);
                        wallRewardBox.Width = 130;
                        wallRewardBox.ReadOnly = true;

						label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox);
                        label.Location = new PointI(0, 300);
                        label.AutoSizeToContents = true;
                        label.Text = "Goal reward:";

						goalRewardBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox);
                        goalRewardBox.Location = new PointI(0, 320);
                        goalRewardBox.Width = 130;
                        goalRewardBox.ReadOnly = true;
                    }



					groupBox = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_RightPanel);
                    {
                        groupBox.Location = new PointI(0, 380);
                        groupBox.Size = new SizeI(140, 173);
                        groupBox.Text = "Learning";


						Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox);
                        label.Location = new PointI(0, 10);
                        label.AutoSizeToContents = true;
                        label.Text = "Iteration:";

						iterationBox = new Alt.GUI.Temporary.Gwen.Control.TextBox(groupBox);
                        iterationBox.Location = new PointI(0, 30);
                        iterationBox.Width = 130;
                        iterationBox.ReadOnly = true;


						startLearningButton = new Alt.GUI.Temporary.Gwen.Control.Button(groupBox);
                        startLearningButton.Location = new PointI(5, 70);
                        startLearningButton.Width = 120;
                        startLearningButton.Text = "Start";
                        startLearningButton.Click += new System.EventHandler(startLearningButton_Click);
                        startLearningButton.NormalTextColor = Color.Green;

						stopButton = new Alt.GUI.Temporary.Gwen.Control.Button(groupBox);
                        stopButton.Location = new PointI(5, 100);
                        stopButton.Width = 120;
                        stopButton.Text = "Stop";
                        stopButton.Click += new System.EventHandler(stopButton_Click);
                        stopButton.NormalTextColor = Color.Red;

						showSolutionButton = new Alt.GUI.Temporary.Gwen.Control.Button(groupBox);
                        showSolutionButton.Location = new PointI(5, 130);
                        showSolutionButton.Width = 120;
                        showSolutionButton.Text = "Show solution";
                        showSolutionButton.Click += new System.EventHandler(showSolutionButton_Click);
                        showSolutionButton.NormalTextColor = Color.Blue;
                    }
                }


                cellWorld = new CellWorld(this);
                cellWorld.Coloring = null;
                cellWorld.Map = null;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            
            // set world colors
            cellWorld.Coloring = new Color[] { Color.White, Color.Green, Color.Black, Color.Red };

            // show settings
            ShowSettings();

            loadButton3_Click(null, EventArgs.Empty);
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (cellWorld != null)
            {
                int wh = System.Math.Min(ClientWidth, ClientHeight);

                cellWorld.Size = new SizeI(wh, wh);
                cellWorld.X = (ClientWidth - wh) / 2;
                cellWorld.Y = (ClientHeight - wh) / 2;
            }
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


        // Enable/disale controls (safe for threading)
        void EnableControls(bool enable)
        {
            GUI.MethodInvoker m = delegate()
            {
                loadButton1.Enabled = enable;
                loadButton2.Enabled = enable;
                loadButton3.Enabled = enable;
                loadButton4.Enabled = enable;
                loadButton5.Enabled = enable;

                algorithmCombo.Enabled = enable;
                explorationRateBox.Enabled = enable;
                learningRateBox.Enabled = enable;
                iterationsBox.Enabled = enable;

                moveRewardBox.Enabled = enable;
                wallRewardBox.Enabled = enable;
                goalRewardBox.Enabled = enable;

                startLearningButton.Enabled = enable;
                showSolutionButton.Enabled = enable;
                stopButton.Enabled = !enable;
            };

            Invoke(m);
        }


        // Show settings
        void ShowSettings()
        {
            explorationRateBox.Text = explorationRate.ToString();
            learningRateBox.Text = learningRate.ToString();
            iterationsBox.Text = learningIterations.ToString();

            moveRewardBox.Text = moveReward.ToString();
            wallRewardBox.Text = wallReward.ToString();
            goalRewardBox.Text = goalReward.ToString();
        }


        // Get settings
        void GetSettings()
        {
            // explortion rate
            try
            {
                explorationRate = Math.Max(0.0, Math.Min(1.0, double.Parse(explorationRateBox.Text)));
            }
            catch
            {
                explorationRate = 0.5;
            }
            // learning rate
            try
            {
                learningRate = Math.Max(0.0, Math.Min(1.0, double.Parse(learningRateBox.Text)));
            }
            catch
            {
                learningRate = 0.5;
            }
            // learning iterations
            try
            {
                learningIterations = Math.Max(10, Math.Min(100000, int.Parse(iterationsBox.Text)));
            }
            catch
            {
                learningIterations = 100;
            }

            // move reward
            try
            {
                moveReward = Math.Max(-100, Math.Min(100, double.Parse(moveRewardBox.Text)));
            }
            catch
            {
                moveReward = 0;
            }
            // wall reward
            try
            {
                wallReward = Math.Max(-100, Math.Min(100, double.Parse(wallRewardBox.Text)));
            }
            catch
            {
                wallReward = -1;
            }
            // goal reward
            try
            {
                goalReward = Math.Max(-100, Math.Min(100, double.Parse(goalRewardBox.Text)));
            }
            catch
            {
                goalReward = 1;
            }
        }


        void loadButton1_Click(object sender, EventArgs e)
        {
            loadButton_Click("sample1");
        }

        void loadButton2_Click(object sender, EventArgs e)
        {
            loadButton_Click("sample2");
        }

        void loadButton3_Click(object sender, EventArgs e)
        {
            loadButton_Click("test1");
        }

        void loadButton4_Click(object sender, EventArgs e)
        {
            loadButton_Click("test2");
        }

        void loadButton5_Click(object sender, EventArgs e)
        {
            loadButton_Click("test3");
        }

        void loadButton_Click(string sample)
        {
            System.IO.Stream stream = Alt.IO.VirtualFile.OpenRead("AltData/AForge/MachineLearning_" + sample + ".map");
            if (stream == null)
            {
                return;
            }


            System.IO.StreamReader reader = null;

            try
            {
                // open selected file
                reader = new System.IO.StreamReader(stream);
                string str = null;
                // line counter
                int lines = 0;
                int j = 0;

                // read the file
                while ((str = reader.ReadLine()) != null)
                {
                    str = str.Trim();

                    // skip comments and empty lines
                    if ((str == string.Empty) || (str[0] == ';') || (str[0] == '\0'))
                        continue;

                    // split the string
                    string[] strs = str.Split(' ');

                    // check the line
                    if (lines == 0)
                    {
                        // get world size
                        mapWidth = int.Parse(strs[0]);
                        mapHeight = int.Parse(strs[1]);
                        map = new int[mapHeight, mapWidth];
                    }
                    else if (lines == 1)
                    {
                        // get agents count
                        if (int.Parse(strs[0]) != 1)
                        {
							new Alt.GUI.Temporary.Gwen.Control.MessageBox(this, "The application supports only 1 agent in the worlds", "Error");
                            break;
                        }
                    }
                    else if (lines == 2)
                    {
                        // agent position
                        agentStartX = int.Parse(strs[0]);
                        agentStartY = int.Parse(strs[1]);
                        agentStopX = int.Parse(strs[2]);
                        agentStopY = int.Parse(strs[3]);

                        // check position
                        if (
                            (agentStartX < 0) || (agentStartX >= mapWidth) ||
                            (agentStartY < 0) || (agentStartY >= mapHeight) ||
                            (agentStopX < 0) || (agentStopX >= mapWidth) ||
                            (agentStopY < 0) || (agentStopY >= mapHeight)
                            )
                        {
							new Alt.GUI.Temporary.Gwen.Control.MessageBox(this, "Agent's start and stop coordinates should be inside the world area ", "Error");
                            break;
                        }
                    }
                    else if (lines > 2)
                    {
                        // map lines
                        if (j < mapHeight)
                        {
                            for (int i = 0; i < mapWidth; i++)
                            {
                                map[j, i] = int.Parse(strs[i]);
                                if (map[j, i] > 1)
                                    map[j, i] = 1;
                            }
                            j++;
                        }
                    }
                    lines++;
                }

                // set world's map
                mapToDisplay = (int[,])map.Clone();
                mapToDisplay[agentStartY, agentStartX] = 2;
                mapToDisplay[agentStopY, agentStopX] = 3;
                cellWorld.Map = mapToDisplay;

                // show world's size
                worldSizeBox.Text = string.Format("{0} x {1}", mapWidth, mapHeight);

                // enable learning button
                startLearningButton.Enabled = true;
            }
            catch (Exception)
            {
				new Alt.GUI.Temporary.Gwen.Control.MessageBox(this, "Failed reading the map file", "Error");
                return;
            }
            finally
            {
                // close file
                if (reader != null)
                {
                    reader.Close();
                }
            }
        }


        // On "Start" learning button click
        void startLearningButton_Click(object sender, EventArgs e)
        {
            // get settings
            GetSettings();
            ShowSettings();

            iterationBox.Text = string.Empty;

            // destroy algorithms
            qLearning = null;
            sarsa = null;

            if (((int)algorithmCombo.SelectedItem.UserData) == 0)
            {
                // create new QLearning algorithm's instance
                qLearning = new QLearning(256, 4, new TabuSearchExploration(4, new EpsilonGreedyExploration(explorationRate)));
                workerThread = new Thread(new ThreadStart(QLearningThread));
            }
            else
            {
                // create new Sarsa algorithm's instance
                sarsa = new Sarsa(256, 4, new TabuSearchExploration(4, new EpsilonGreedyExploration(explorationRate)));
                workerThread = new Thread(new ThreadStart(SarsaThread));
            }

            // disable all settings controls except "Stop" button
            EnableControls(false);

            // run worker thread
            needToStop = false;
            workerThread.Start();
        }


        // On "Stop" button click
        void stopButton_Click(object sender, EventArgs e)
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


        // On "Show Solution" button
        void showSolutionButton_Click(object sender, EventArgs e)
        {
            // check if learning algorithm was run before
            if ((qLearning == null) && (sarsa == null))
                return;

            // disable all settings controls except "Stop" button
            EnableControls(false);

            // run worker thread
            needToStop = false;
            workerThread = new Thread(new ThreadStart(ShowSolutionThread));
            workerThread.Start();
        }


        // Q-Learning thread
        void QLearningThread()
        {
            int iteration = 0;
            // curent coordinates of the agent
            int agentCurrentX, agentCurrentY;
            // exploration policy
            TabuSearchExploration tabuPolicy = (TabuSearchExploration)qLearning.ExplorationPolicy;
            EpsilonGreedyExploration explorationPolicy = (EpsilonGreedyExploration)tabuPolicy.BasePolicy;

            // loop
            while ((!needToStop) && (iteration < learningIterations))
            {
                // set exploration rate for this iteration
                explorationPolicy.Epsilon = explorationRate - ((double)iteration / learningIterations) * explorationRate;
                // set learning rate for this iteration
                qLearning.LearningRate = learningRate - ((double)iteration / learningIterations) * learningRate;
                // clear tabu list
                tabuPolicy.ResetTabuList();

                // reset agent's coordinates to the starting position
                agentCurrentX = agentStartX;
                agentCurrentY = agentStartY;

                // steps performed by agent to get to the goal
                int steps = 0;

                while ((!needToStop) && ((agentCurrentX != agentStopX) || (agentCurrentY != agentStopY)))
                {
                    steps++;
                    // get agent's current state
                    int currentState = GetStateNumber(agentCurrentX, agentCurrentY);
                    // get the action for this state
                    int action = qLearning.GetAction(currentState);
                    // update agent's current position and get his reward
                    double reward = UpdateAgentPosition(ref agentCurrentX, ref agentCurrentY, action);
                    // get agent's next state
                    int nextState = GetStateNumber(agentCurrentX, agentCurrentY);
                    // do learning of the agent - update his Q-function
                    qLearning.UpdateState(currentState, action, reward, nextState);

                    // set tabu action
                    tabuPolicy.SetTabuAction((action + 2) % 4, 1);
                }

                System.Diagnostics.Debug.WriteLine(steps);

                iteration++;

                // show current iteration
                SetText(iterationBox, iteration.ToString());
            }

            // enable settings controls
            EnableControls(true);
        }


        // Sarsa thread
        void SarsaThread()
        {
            int iteration = 0;
            // curent coordinates of the agent
            int agentCurrentX, agentCurrentY;
            // exploration policy
            TabuSearchExploration tabuPolicy = (TabuSearchExploration)sarsa.ExplorationPolicy;
            EpsilonGreedyExploration explorationPolicy = (EpsilonGreedyExploration)tabuPolicy.BasePolicy;

            // loop
            while ((!needToStop) && (iteration < learningIterations))
            {
                // set exploration rate for this iteration
                explorationPolicy.Epsilon = explorationRate - ((double)iteration / learningIterations) * explorationRate;
                // set learning rate for this iteration
                sarsa.LearningRate = learningRate - ((double)iteration / learningIterations) * learningRate;
                // clear tabu list
                tabuPolicy.ResetTabuList();

                // reset agent's coordinates to the starting position
                agentCurrentX = agentStartX;
                agentCurrentY = agentStartY;

                // steps performed by agent to get to the goal
                int steps = 1;
                // previous state and action
                int previousState = GetStateNumber(agentCurrentX, agentCurrentY);
                int previousAction = sarsa.GetAction(previousState);
                // update agent's current position and get his reward
                double reward = UpdateAgentPosition(ref agentCurrentX, ref agentCurrentY, previousAction);

                while ((!needToStop) && ((agentCurrentX != agentStopX) || (agentCurrentY != agentStopY)))
                {
                    steps++;

                    // set tabu action
                    tabuPolicy.SetTabuAction((previousAction + 2) % 4, 1);

                    // get agent's next state
                    int nextState = GetStateNumber(agentCurrentX, agentCurrentY);
                    // get agent's next action
                    int nextAction = sarsa.GetAction(nextState);
                    // do learning of the agent - update his Q-function
                    sarsa.UpdateState(previousState, previousAction, reward, nextState, nextAction);

                    // update agent's new position and get his reward
                    reward = UpdateAgentPosition(ref agentCurrentX, ref agentCurrentY, nextAction);

                    previousState = nextState;
                    previousAction = nextAction;
                }

                if (!needToStop)
                {
                    // update Q-function if terminal state was reached
                    sarsa.UpdateState(previousState, previousAction, reward);
                }

                System.Diagnostics.Debug.WriteLine(steps);

                iteration++;

                // show current iteration
                SetText(iterationBox, iteration.ToString());
            }

            // enable settings controls
            EnableControls(true);
        }


        // Show solution thread
        void ShowSolutionThread()
        {
            // set exploration rate to 0, so agent uses only what he learnt
            TabuSearchExploration tabuPolicy = null;
            EpsilonGreedyExploration exploratioPolicy = null;

            if (qLearning != null)
                tabuPolicy = (TabuSearchExploration)qLearning.ExplorationPolicy;
            else
                tabuPolicy = (TabuSearchExploration)sarsa.ExplorationPolicy;

            exploratioPolicy = (EpsilonGreedyExploration)tabuPolicy.BasePolicy;

            exploratioPolicy.Epsilon = 0;
            tabuPolicy.ResetTabuList();

            // curent coordinates of the agent
            int agentCurrentX = agentStartX, agentCurrentY = agentStartY;

            // pripate the map to display
            Array.Copy(map, mapToDisplay, mapWidth * mapHeight);
            mapToDisplay[agentStartY, agentStartX] = 2;
            mapToDisplay[agentStopY, agentStopX] = 3;

            while (!needToStop)
            {
                // dispay the map
                cellWorld.Map = mapToDisplay;
                // sleep for a while
                Thread.Sleep(200);

                // check if we have reached the end point
                if ((agentCurrentX == agentStopX) && (agentCurrentY == agentStopY))
                {
                    // restore the map
                    mapToDisplay[agentStartY, agentStartX] = 2;
                    mapToDisplay[agentStopY, agentStopX] = 3;

                    agentCurrentX = agentStartX;
                    agentCurrentY = agentStartY;

                    cellWorld.Map = mapToDisplay;
                    Thread.Sleep(200);
                }

                // remove agent from current position
                mapToDisplay[agentCurrentY, agentCurrentX] = 0;

                // get agent's current state
                int currentState = GetStateNumber(agentCurrentX, agentCurrentY);
                // get the action for this state
                int action = (qLearning != null) ? qLearning.GetAction(currentState) : sarsa.GetAction(currentState);
                // update agent's current position and get his reward
                //NoNeed	double reward =
				UpdateAgentPosition(ref agentCurrentX, ref agentCurrentY, action);

                // put agent to the new position
                mapToDisplay[agentCurrentY, agentCurrentX] = 2;
            }

            // enable settings controls
            EnableControls(true);
        }


        // Update agent position and return reward for the move
        double UpdateAgentPosition(ref int currentX, ref int currentY, int action)
        {
            // default reward is equal to moving reward
            double reward = moveReward;
            // moving direction
            int dx = 0, dy = 0;

            switch (action)
            {
                case 0:         // go to north (up)
                    dy = -1;
                    break;
                case 1:         // go to east (right)
                    dx = 1;
                    break;
                case 2:         // go to south (down)
                    dy = 1;
                    break;
                case 3:         // go to west (left)
                    dx = -1;
                    break;
            }

            int newX = currentX + dx;
            int newY = currentY + dy;

            // check new agent's coordinates
            if (
                (map[newY, newX] != 0) ||
                (newX < 0) || (newX >= mapWidth) ||
                (newY < 0) || (newY >= mapHeight)
                )
            {
                // we found a wall or got outside of the world
                reward = wallReward;
            }
            else
            {
                currentX = newX;
                currentY = newY;

                // check if we found the goal
                if ((currentX == agentStopX) && (currentY == agentStopY))
                    reward = goalReward;
            }

            return reward;
        }


        // Get state number from agent's receptors in the specified position
        int GetStateNumber(int x, int y)
        {
            int c1 = (map[y - 1, x - 1] != 0) ? 1 : 0;
            int c2 = (map[y - 1, x] != 0) ? 1 : 0;
            int c3 = (map[y - 1, x + 1] != 0) ? 1 : 0;
            int c4 = (map[y, x + 1] != 0) ? 1 : 0;
            int c5 = (map[y + 1, x + 1] != 0) ? 1 : 0;
            int c6 = (map[y + 1, x] != 0) ? 1 : 0;
            int c7 = (map[y + 1, x - 1] != 0) ? 1 : 0;
            int c8 = (map[y, x - 1] != 0) ? 1 : 0;

            return c1 |
                (c2 << 1) |
                (c3 << 2) |
                (c4 << 3) |
                (c5 << 4) |
                (c6 << 5) |
                (c7 << 6) |
                (c8 << 7);
        }
    }
}
