// Parallel computations sample application
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


namespace Alt.GUI.Demo
{
    class Example_AForge_ParallelTest : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                int x = 0;
                int y = 0;

                if (m_ConsoleTop != null)
                {
                    y = m_ConsoleTop.Height + m_ConsoleTop.Margin.Height;
                    x += 5;
                }

                return new SizeI(x, y);
            }
        }


		Alt.GUI.Temporary.Gwen.Control.Label m_ConsoleTop;
		Alt.GUI.Temporary.Gwen.Control.Label m_Console;
        GUI.Timer m_Timer;
        object m_Lock = new object();
        List<string> m_ConsoleCache = new List<string>();
        Thread m_BGThread;


        public Example_AForge_ParallelTest(Base parent)
            : base(parent)
        {
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


			m_ConsoleTop = new Alt.GUI.Temporary.Gwen.Control.Label(this);
            m_ConsoleTop.Margin = Margin.Two;
            m_ConsoleTop.Dock = Pos.Top;
            m_ConsoleTop.AutoSizeToContents = true;
            m_ConsoleTop.TextColor = Color.Yellow;
            m_ConsoleTop.Text = "Console:";


            Base bg = new Base(this);
            bg.Dock = Pos.Fill;
            bg.ClientBackColor = Color.Black;
            bg.DrawBorder = true;
            bg.BorderColor = Color.DodgerBlue;

            ScrollControl scrollControl = new ScrollControl(bg);
            scrollControl.Dock = Pos.Fill;
            scrollControl.EnableScroll(true, true);
            scrollControl.AutoHideBars = true;
            scrollControl.Margin = Margin.Five;

			m_Console = new Alt.GUI.Temporary.Gwen.Control.Label(scrollControl);
            m_Console.Margin = Margin.Two;
            m_Console.Location = PointI.Zero;
            m_Console.AutoSizeToContents = true;
            m_Console.TextColor = Color.White;
            m_Console.MouseInputEnabled = true;


            m_Timer = new GUI.Timer();
            m_Timer.Interval = 1;
            m_Timer.Tick += new EventHandler(Timer_Tick);
            m_Timer.Enabled = true;


            m_BGThread = new Thread(new ThreadStart(this.RunTest));
#if !SILVERLIGHT && !UNITY_WEBPLAYER
            m_BGThread.SetApartmentState(ApartmentState.STA);
#endif
            m_BGThread.Start();
        }


        public override void Dispose()
        {
            if (m_Timer != null)
            {
                m_Timer.Stop();
                m_Timer.Dispose();
                m_Timer = null;
            }

            if (m_BGThread != null &&
                m_BGThread.IsAlive)
            {
                m_BGThread.Abort();
            }

            base.Dispose();
        }


        void Timer_Tick(object sender, EventArgs e)
        {
            lock (m_Lock)
            {
                if (m_ConsoleCache.Count > 0)
                {
                    string text = m_Console.Text;

                    foreach (string s in m_ConsoleCache)
                    {
                        text += s;
                    }
                    m_ConsoleCache.Clear();

                    m_Console.Text = text;
                }
            }
        }


        void Console_WriteLine(string str)
        {
            Console_Write(str + "\n");
        }

        void Console_Write(string str)
        {
            lock (m_Lock)
            {
                str = str.Replace("\t", "    ");

                m_ConsoleCache.Add(str);
            }
        }


        void RunTest()
        {
            int matrixSize = 250;
            int runs = 10;
            int tests = 5;

            double test1time = 0;
            double test2time = 0;

            Console_WriteLine("Starting test with " + global::AForge.Parallel.ThreadsCount + " threads");

            // allocate matrixes for all tests
            double[,] a = new double[matrixSize, matrixSize];
            double[,] b = new double[matrixSize, matrixSize];
            double[,] c1 = new double[matrixSize, matrixSize];
            double[,] c2 = new double[matrixSize, matrixSize];

            Random rand = new Random();

            // fill source matrixes with random numbers
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    a[i, j] = rand.NextDouble();
                    b[i, j] = rand.NextDouble();
                }
            }

            // run specified number of tests
            for (int test = 0; test < tests; test++)
            {
                // test 1
                DateTime start = DateTime.Now;

                for (int run = 0; run < runs; run++)
                {
                    Test1(a, b, c1);
                }

                DateTime end = DateTime.Now;
                TimeSpan span = end - start;

                Console_Write(span.TotalMilliseconds.ToString("F3") + "\t | ");
                test1time += span.TotalMilliseconds;

                // test 2
                start = DateTime.Now;

                for (int run = 0; run < runs; run++)
                {
                    Test2(a, b, c2);
                }

                end = DateTime.Now;
                span = end - start;

                Console_Write(span.TotalMilliseconds.ToString("F3") + "\t | ");
                test2time += span.TotalMilliseconds;

                Console_WriteLine(" ");
            }

            // provide average performance
            test1time /= tests;
            test2time /= tests;

            Console_WriteLine("----------- AVG -----------");
            Console_WriteLine(test1time.ToString("F3") + "\t | " + test2time.ToString("F3") + "\t | ");

            Console_WriteLine("Done");
        }


        // Test #1 - multiply 2 square matrixes without using parallel computations
        void Test1(double[,] a, double[,] b, double[,] c)
        {
            int s = a.GetLength(0);

            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < s; j++)
                {
                    double v = 0;

                    for (int k = 0; k < s; k++)
                    {
                        v += a[i, k] * b[k, j];
                    }

                    c[i, j] = v;
                }
            }
        }


        // Test #2 - multiply 2 square matrixes using parallel computations
        void Test2(double[,] a, double[,] b, double[,] c)
        {
            int s = a.GetLength(0);

            global::AForge.Parallel.For(0, s, delegate(int i)
            {
                for (int j = 0; j < s; j++)
                {
                    double v = 0;

                    for (int k = 0; k < s; k++)
                    {
                        v += a[i, k] * b[k, j];
                    }

                    c[i, j] = v;
                }
            }
            );
        }
    }
}
