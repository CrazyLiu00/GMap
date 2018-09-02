// Fuzzy Auto Guided Vehicle Sample
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright � AForge.NET, 2005-2011
// contacts@aforgenet.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

using Alt.ComponentModel;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.Sketch;
using Alt.Threading;

using AForge.Fuzzy;


namespace Alt.GUI.Demo
{
    class Example_AForge_FuzzyAGV : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 0;
                int y = 0;

                if (m_TopLabel != null &&
                    m_ControlsPanel != null)
                {
                    x = m_ControlsPanel.Width + 5;
                    y = m_TopLabel.Height + m_TopLabel.Margin.Height;
                }

                return new SizeI(x, y);
            }
        }

        
        string RunLabel;
        PointI InitialPos;
        bool FirstInference;
        //int LastX;
        //int LastY;
        double Angle;
        Bitmap OriginalMap, InitialMap;
        InferenceSystem IS;
        Thread thMovement;


		Alt.GUI.Temporary.Gwen.Control.Label m_TopLabel;
		Alt.GUI.Temporary.Gwen.Control.Base m_ControlsPanel;
		Alt.GUI.Temporary.Gwen.Control.PictureBox pbTerrain;
		Alt.GUI.Temporary.Gwen.Control.Button btnRun;
		Alt.GUI.Temporary.Gwen.Control.NumericUpDown txtInterval;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox cbLasers;
		Alt.GUI.Temporary.Gwen.Control.GroupBox groupBox1;
		Alt.GUI.Temporary.Gwen.Control.Label txtRight;
		Alt.GUI.Temporary.Gwen.Control.Label txtLeft;
		Alt.GUI.Temporary.Gwen.Control.Label txtFront;
		Alt.GUI.Temporary.Gwen.Control.Label lbl;
		Alt.GUI.Temporary.Gwen.Control.Label label2;
		Alt.GUI.Temporary.Gwen.Control.Label label1;
		Alt.GUI.Temporary.Gwen.Control.GroupBox groupBox2;
		Alt.GUI.Temporary.Gwen.Control.Label label3;
		Alt.GUI.Temporary.Gwen.Control.Label txtAngle;
		Alt.GUI.Temporary.Gwen.Control.Button btnStep;
		Alt.GUI.Temporary.Gwen.Control.Label label4;
		Alt.GUI.Temporary.Gwen.Control.PictureBox pbRobot;
		Alt.GUI.Temporary.Gwen.Control.Button btnReset;
		Alt.GUI.Temporary.Gwen.Control.GroupBox gbComandos;
		Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox cbTrajeto;


        public Example_AForge_FuzzyAGV(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            //  GUI
            {
				m_TopLabel = new Alt.GUI.Temporary.Gwen.Control.Label(this);
                m_TopLabel.AutoSizeToContents = true;
                m_TopLabel.Text = "Left click the image to draw passages (white), right click the image to draw walls (black).";
                m_TopLabel.Dock = Pos.Top;
                m_TopLabel.TextColor = Color.Yellow;
                m_TopLabel.Margin = new Margin(0, 5, 0, 5);


				m_ControlsPanel = new Alt.GUI.Temporary.Gwen.Control.Base(this);
                m_ControlsPanel.Width = 139;
                m_ControlsPanel.Dock = Pos.Right;


				pbTerrain = new Alt.GUI.Temporary.Gwen.Control.PictureBox(this);
                pbTerrain.Dock = Pos.Fill;
                pbTerrain.ErrorImage = null;
                pbTerrain.Image = Bitmap.FromFile("AltData/AForge/FuzzyAGV_BG.gif");
                pbTerrain.InitialImage = null;
				pbTerrain.SizeMode = PictureBoxSizeMode.Normal;// AutoSize;
                pbTerrain.MouseMove += new GUI.MouseEventHandler(pbTerrain_MouseMove);
                pbTerrain.MouseDown += new GUI.MouseEventHandler(pbTerrain_MouseDown);


                int groupBox_dy = -10;
				groupBox1 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_ControlsPanel);
                {
                    groupBox1.Location = new PointI(0, 0);
                    groupBox1.Name = "groupBox1";
                    groupBox1.Size = new SizeI(139, 83);
                    groupBox1.Text = "Sensor readings:";


					lbl = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox1);
					label2 = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox1);
					label1 = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox1);

                    lbl.Location = new PointI(0, 54 + groupBox_dy);
                    lbl.AutoSizeToContents = true;
                    lbl.Text = "Right (pixels):";

                    label2.Location = new PointI(0, 35 + groupBox_dy);
                    label2.AutoSizeToContents = true;
                    label2.Text = "Left (pixels):";

                    label1.Location = new PointI(0, 16 + groupBox_dy);
                    label1.AutoSizeToContents = true;
                    label1.Text = "Frontal (pixels):";


					txtRight = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox1);
					txtLeft = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox1);
					txtFront = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox1);

                    txtRight.Location = new PointI(100, 54 + groupBox_dy);
                    txtRight.AutoSizeToContents = true;
                    txtRight.Text = "0";
                    txtRight.TextAlign = ContentAlignment.MiddleLeft;

                    txtLeft.Location = new PointI(100, 35 + groupBox_dy);
                    txtLeft.AutoSizeToContents = true;
                    txtLeft.Text = "0";
                    txtLeft.TextAlign = ContentAlignment.MiddleLeft;

                    txtFront.Location = new PointI(100, 16 + groupBox_dy);
                    txtFront.AutoSizeToContents = true;
                    txtFront.Text = "0";
                    txtFront.TextAlign = ContentAlignment.MiddleLeft;
                }

				groupBox2 = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_ControlsPanel);
                {
                    groupBox2.Location = new PointI(0, 94);
                    groupBox2.Size = new SizeI(139, 45);
                    groupBox2.Text = "Output:";


					label3 = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);
					txtAngle = new Alt.GUI.Temporary.Gwen.Control.Label(groupBox2);

                    label3.Location = new PointI(0, 16 + groupBox_dy);
                    label3.AutoSizeToContents = true;
                    label3.Text = "Angle (degrees):";

                    txtAngle.Location = new PointI(98, 16 + groupBox_dy);
                    txtAngle.AutoSizeToContents = true;
                    txtAngle.Text = "0,00";
                    txtAngle.TextAlign = ContentAlignment.MiddleLeft;
                }

				gbComandos = new Alt.GUI.Temporary.Gwen.Control.GroupBox(m_ControlsPanel);
                {
                    gbComandos.Location = new PointI(0, 150);
                    gbComandos.Size = new SizeI(139, 205);
                    gbComandos.Text = "Tools:";


					btnStep = new Alt.GUI.Temporary.Gwen.Control.Button(gbComandos);
					btnRun = new Alt.GUI.Temporary.Gwen.Control.Button(gbComandos);
					txtInterval = new Alt.GUI.Temporary.Gwen.Control.NumericUpDown(gbComandos);
					cbLasers = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(gbComandos);

					cbTrajeto = new Alt.GUI.Temporary.Gwen.Control.LabeledCheckBox(gbComandos);
					btnReset = new Alt.GUI.Temporary.Gwen.Control.Button(gbComandos);
					label4 = new Alt.GUI.Temporary.Gwen.Control.Label(gbComandos);

                    btnStep.Location = new PointI(0, 109 + groupBox_dy);
                    btnStep.Size = new SizeI(128, 23);
                    btnStep.Text = "One Step";
                    btnStep.Click += new System.EventHandler(button3_Click);
                    btnStep.NormalTextColor = Color.Blue;

                    btnRun.Location = new PointI(0, 138 + groupBox_dy);
                    btnRun.Size = new SizeI(128, 23);
                    btnRun.Text = "Run";
                    btnRun.Click += new System.EventHandler(btnRun_Click);
                    btnRun.NormalTextColor = Color.Green;

                    txtInterval.Location = new PointI(0, 83 + groupBox_dy);
                    txtInterval.Size = new SizeI(127, 20);
                    txtInterval.Min = 1;
                    txtInterval.Max = 100;
                    txtInterval.Value = 20;

                    cbLasers.IsChecked = true;
                    cbLasers.Location = new PointI(0, 40 + groupBox_dy);
                    cbLasers.Text = "Show Beams";

                    cbTrajeto.Location = new PointI(0, 16 + groupBox_dy);
                    cbTrajeto.Text = "Track Path";

                    btnReset.Location = new PointI(0, 167 + groupBox_dy);
                    btnReset.Size = new SizeI(128, 23);
                    btnReset.Text = "Restart";
                    btnReset.Click += new System.EventHandler(btnReset_Click);
                    btnReset.NormalTextColor = Color.Red * 0.8;

                    label4.Location = new PointI(0, 65 + groupBox_dy);
                    label4.AutoSizeToContents = true;
                    label4.Text = "Move Interval (ms):";
                }


                Bitmap robot = new Bitmap(10, 10);
                using (Graphics graphics = Graphics.FromImage(robot))
                {
                    graphics.FillCircle(Brushes.Blue, robot.PixelRectangle);
                }
				pbRobot = new Alt.GUI.Temporary.Gwen.Control.PictureBox(pbTerrain);
                pbRobot.Image = robot;
                pbRobot.Size = robot.PixelSize;
            }


            Angle = 0;
            OriginalMap = new Bitmap(pbTerrain.Image);
            InitialMap = new Bitmap(pbTerrain.Image);

            InitFuzzyEngine();
            FirstInference = true;
            pbRobot.Top = InitialMap.PixelHeight - 55;
            pbRobot.Left = 60;
            InitialPos = pbRobot.Location;
            RunLabel = btnRun.Text;
        }

        /// <summary>
        /// Stoping the movement thread
        /// </summary>
        public override void Dispose()
        {
            StopMovement();

            base.Dispose();
        }


        // Hardcode initializing the Fuzzy Inference System
        void InitFuzzyEngine()
        {
            // Linguistic labels (fuzzy sets) that compose the distances
            FuzzySet fsNear = new FuzzySet("Near", new TrapezoidalFunction(15, 50, TrapezoidalFunction.EdgeType.Right));
            FuzzySet fsMedium = new FuzzySet("Medium", new TrapezoidalFunction(15, 50, 60, 100));
            FuzzySet fsFar = new FuzzySet("Far", new TrapezoidalFunction(60, 100, TrapezoidalFunction.EdgeType.Left));

            // Right Distance (Input)
            LinguisticVariable lvRight = new LinguisticVariable("RightDistance", 0, 120);
            lvRight.AddLabel(fsNear);
            lvRight.AddLabel(fsMedium);
            lvRight.AddLabel(fsFar);

            // Left Distance (Input)
            LinguisticVariable lvLeft = new LinguisticVariable("LeftDistance", 0, 120);
            lvLeft.AddLabel(fsNear);
            lvLeft.AddLabel(fsMedium);
            lvLeft.AddLabel(fsFar);

            // Front Distance (Input)
            LinguisticVariable lvFront = new LinguisticVariable("FrontalDistance", 0, 120);
            lvFront.AddLabel(fsNear);
            lvFront.AddLabel(fsMedium);
            lvFront.AddLabel(fsFar);

            // Linguistic labels (fuzzy sets) that compose the angle
            FuzzySet fsVN = new FuzzySet("VeryNegative", new TrapezoidalFunction(-40, -35, TrapezoidalFunction.EdgeType.Right));
            FuzzySet fsN = new FuzzySet("Negative", new TrapezoidalFunction(-40, -35, -25, -20));
            FuzzySet fsLN = new FuzzySet("LittleNegative", new TrapezoidalFunction(-25, -20, -10, -5));
            FuzzySet fsZero = new FuzzySet("Zero", new TrapezoidalFunction(-10, 5, 5, 10));
            FuzzySet fsLP = new FuzzySet("LittlePositive", new TrapezoidalFunction(5, 10, 20, 25));
            FuzzySet fsP = new FuzzySet("Positive", new TrapezoidalFunction(20, 25, 35, 40));
            FuzzySet fsVP = new FuzzySet("VeryPositive", new TrapezoidalFunction(35, 40, TrapezoidalFunction.EdgeType.Left));

            // Angle
            LinguisticVariable lvAngle = new LinguisticVariable("Angle", -50, 50);
            lvAngle.AddLabel(fsVN);
            lvAngle.AddLabel(fsN);
            lvAngle.AddLabel(fsLN);
            lvAngle.AddLabel(fsZero);
            lvAngle.AddLabel(fsLP);
            lvAngle.AddLabel(fsP);
            lvAngle.AddLabel(fsVP);

            // The database
            Database fuzzyDB = new Database();
            fuzzyDB.AddVariable(lvFront);
            fuzzyDB.AddVariable(lvLeft);
            fuzzyDB.AddVariable(lvRight);
            fuzzyDB.AddVariable(lvAngle);

            // Creating the inference system
            IS = new InferenceSystem(fuzzyDB, new CentroidDefuzzifier(1000));

            // Going Straight
            IS.NewRule("Rule 1", "IF FrontalDistance IS Far THEN Angle IS Zero");
            // Going Straight (if can go anywhere)
            IS.NewRule("Rule 2", "IF FrontalDistance IS Far AND RightDistance IS Far AND LeftDistance IS Far THEN Angle IS Zero");
            // Near right wall
            IS.NewRule("Rule 3", "IF RightDistance IS Near AND LeftDistance IS Not Near THEN Angle IS LittleNegative");
            // Near left wall
            IS.NewRule("Rule 4", "IF RightDistance IS Not Near AND LeftDistance IS Near THEN Angle IS LittlePositive");
            // Near front wall - room at right
            IS.NewRule("Rule 5", "IF RightDistance IS Far AND FrontalDistance IS Near THEN Angle IS Positive");
            // Near front wall - room at left
            IS.NewRule("Rule 6", "IF LeftDistance IS Far AND FrontalDistance IS Near THEN Angle IS Negative");
            // Near front wall - room at both sides - go right
            IS.NewRule("Rule 7", "IF RightDistance IS Far AND LeftDistance IS Far AND FrontalDistance IS Near THEN Angle IS Positive");
        }


        // Run one epoch of the Fuzzy Inference System 
        void DoInference()
        {
            // Setting inputs
            IS.SetInput("RightDistance", Convert.ToSingle(txtRight.Text));
            IS.SetInput("LeftDistance", Convert.ToSingle(txtLeft.Text));
            IS.SetInput("FrontalDistance", Convert.ToSingle(txtFront.Text));

            // Setting outputs
            try
            {
                double NewAngle = IS.Evaluate("Angle");
                txtAngle.Text = NewAngle.ToString("##0.#0");
                Angle += NewAngle;
            }
            catch (Exception)
            {
            }
        }


        // AGV's terrain drawing
        void pbTerrain_MouseDown(object sender, GUI.MouseEventArgs e)
        {
            pbTerrain.Image = CopyImage(OriginalMap);
            //LastX = e.X;
            //LastY = e.Y;
        }


        // AGV's terrain drawing
        void pbTerrain_MouseMove(object sender, GUI.MouseEventArgs e)
        {
            Brush brush = null;

            if (e.Button == GUI.MouseButtons.Left)
            {
                brush = Brushes.White;
            }
            else if (e.Button == GUI.MouseButtons.Right)
            {
                brush = Brushes.Black;
            }
            else
            {
                return;
            }

            using (Graphics g = Graphics.FromImage(pbTerrain.Image))
            {
                g.FillRectangle(brush, e.X - 40, e.Y - 40, 80, 80);
            }

            //LastX = e.X;
            //LastY = e.Y;
         
            OriginalMap = CopyImage(pbTerrain.Image as Bitmap);
            pbTerrain.Refresh();
        }


        // Getting sensors measures
        WeakReference m_ExceptionMessageBox = new WeakReference(null);
        void GetMeasures()
        {
            // Getting AGV's position
            pbTerrain.Image = CopyImage(OriginalMap);
            Bitmap b = pbTerrain.Image as Bitmap;
            PointI pPos = new PointI(pbRobot.Left + 5, pbRobot.Top + 5);

            // AGV on the wall
            Color b_GetPixel = b.GetPixel(pPos.X, pPos.Y);
            if ((b_GetPixel.R == 0) && (b_GetPixel.G == 0) && (b_GetPixel.B == 0))
            {
                if (btnRun.Text != RunLabel)
                {
                    btnRun_Click(btnRun, null);
                }
                string Msg = "The vehicle is on the solid area!";

                //  prevent multiple MessageBoxes
                if (m_ExceptionMessageBox.Target == null ||
				    (m_ExceptionMessageBox.Target as Alt.GUI.Temporary.Gwen.Control.MessageBox).IsDisposed)
                {
					m_ExceptionMessageBox = new WeakReference(new Alt.GUI.Temporary.Gwen.Control.MessageBox(this, Msg, "Error"));
                }

                throw new Exception(Msg);
            }

            // Getting distances
            PointI pFrontObstacle = GetObstacle(pPos, b, -1, 0);
            PointI pLeftObstacle = GetObstacle(pPos, b, 1, 90);
            PointI pRightObstacle = GetObstacle(pPos, b, 1, -90);

            // Showing beams
            using (Graphics g = Graphics.FromImage(b))
            {
                if (cbLasers.IsChecked)
                {
                    g.DrawLine(Pens.Green, pFrontObstacle, pPos);
                    g.DrawLine(Pens.Red, pLeftObstacle, pPos);
                    g.DrawLine(Pens.Red, pRightObstacle, pPos);
                }

                // Drawing AGV
                if (btnRun.Text != RunLabel)
                {
                    g.FillEllipse(Brushes.Navy, pPos.X - 5, pPos.Y - 5, 10, 10);
                }
            }

            pbTerrain.Refresh();

            // Updating distances texts
            txtFront.Text = GetDistance(pPos, pFrontObstacle).ToString();
            txtLeft.Text = GetDistance(pPos, pLeftObstacle).ToString();
            txtRight.Text = GetDistance(pPos, pRightObstacle).ToString();
        }


        // Calculating distances
        int GetDistance(PointI p1, PointI p2)
        {
            return (Convert.ToInt32(Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2))));
        }


        // Finding obstacles
        PointI GetObstacle(PointI Start, Bitmap Map, int Inc, int AngleOffset)
        {
            PointI p = new PointI(Start.X, Start.Y);

            double rad = ((Angle + 90 + AngleOffset) * Math.PI) / 180;
            int IncX = 0;
            int IncY = 0;
            int Offset = 0;

            while ((p.X + IncX >= 0) && (p.X + IncX < Map.PixelWidth) && (p.Y + IncY >= 0) && (p.Y + IncY < Map.PixelHeight))
            {
                Color Map_GetPixel = Map.GetPixel(p.X + IncX, p.Y + IncY);
                if ((Map_GetPixel.R == 0) && (Map_GetPixel.G == 0) && (Map_GetPixel.B == 0))
                {
                    break;
                }
                Offset += Inc;
                IncX = Convert.ToInt32(Offset * Math.Cos(rad));
                IncY = Convert.ToInt32(Offset * Math.Sin(rad));
            }
            p.X += IncX;
            p.Y += IncY;

            return p;
        }


        // Copying bitmaps
        Bitmap CopyImage(Bitmap Src)
        {
            return new Bitmap(Src);
        }


        // Restarting the AGVs simulation
        void btnReset_Click(object sender, System.EventArgs e)
        {
            Angle = 0;
            pbTerrain.Image = new Bitmap(InitialMap);
            OriginalMap = new Bitmap(InitialMap);
            FirstInference = true;
            pbRobot.Location = InitialPos;
            txtFront.Text = "0";
            txtLeft.Text = "0";
            txtRight.Text = "0";
            txtAngle.Text = "0,00";
        }


        // Moving the AGV
        void MoveAGV()
        {
            double rad = ((Angle + 90) * Math.PI) / 180;
            int Offset = 0;
            int Inc = -4;

            Offset += Inc;
            int IncX = Convert.ToInt32(Offset * Math.Cos(rad));
            int IncY = Convert.ToInt32(Offset * Math.Sin(rad));

            // Leaving the track 
            if (cbTrajeto.IsChecked)
            {
                using (Graphics g = Graphics.FromImage(OriginalMap))
                {
                    PointI p1 = new PointI(pbRobot.Left + pbRobot.Width / 2, pbRobot.Top + pbRobot.Height / 2);
                    PointI p2 = new PointI(p1.X + IncX, p1.Y + IncY);
                    g.DrawLine(Pens.Blue, p1, p2);
                }
            }

            pbRobot.Top = pbRobot.Top + IncY;
            pbRobot.Left = pbRobot.Left + IncX;
        }


        // Starting and stopping the AGV's moviment a
        void btnRun_Click(object sender, System.EventArgs e)
        {
			Alt.GUI.Temporary.Gwen.Control.Button b = (sender as Alt.GUI.Temporary.Gwen.Control.Button);

            if (b.Text == RunLabel)
            {
                b.Text = "Stop";
                b.NormalTextColor = Color.Red * 0.8;
                btnStep.Enabled = false;
                btnReset.Enabled = false;
                txtInterval.Enabled = false;
                cbLasers.Enabled = false;
                cbTrajeto.Enabled = false;
                pbTerrain.Enabled = false;
                pbRobot.Hide();
                StartMovement();
            }
            else
            {
                StopMovement();
                b.Text = RunLabel;
                b.NormalTextColor = Color.Green;
                btnReset.Enabled = true;
                btnStep.Enabled = true;
                txtInterval.Enabled = true;
                cbLasers.Enabled = true;
                cbTrajeto.Enabled = true;
                pbTerrain.Enabled = true;
                pbRobot.Show();
                pbTerrain.Image = CopyImage(OriginalMap);
                pbTerrain.Refresh();
            }
        }


        // One step of the AGV
        void button3_Click(object sender, System.EventArgs e)
        {
            pbRobot.Hide();
            AGVStep();
            pbRobot.Show();
        }


        // Thread for the AGVs movement
        void StartMovement()
        {
            thMovement = new Thread(new ThreadStart(MoveCycle));
            thMovement.IsBackground = true;
            thMovement.Priority = ThreadPriority.AboveNormal;
            thMovement.Start();
        }


        // Thread main cycle
        void MoveCycle()
        {
            try
            {
                while (Thread.CurrentThread.IsAlive)
                {
                    GUI.MethodInvoker mi = new GUI.MethodInvoker(AGVStep);
                    this.BeginInvoke(mi);
                    Thread.Sleep(Convert.ToInt32(txtInterval.Value));
                }
            }
            catch
#if !SILVERLIGHT && !UNITY_WEBPLAYER
 			(System.Threading.ThreadInterruptedException)
#endif
            {
            }
        }


        // One step of the AGV
        void AGVStep()
        {
            try
            {
                if (FirstInference)
                {
                    GetMeasures();
                }

                DoInference();
                MoveAGV();
                GetMeasures();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }


        // Stop background thread
        void StopMovement()
        {
            if (thMovement != null)
            {
#if !SILVERLIGHT && !UNITY_WEBPLAYER
                thMovement.Interrupt();
#else
                thMovement.Abort();
#endif
                thMovement = null;
            }
        }
    }
}
