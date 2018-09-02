//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System.Diagnostics;

#if WINDOWS_PHONE_7 || WINDOWS_PHONE_71
using Alt.Diagnostics;
#endif

using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;
using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Common.Decomposition;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Factories;

using Alt.IO;
using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class TriangulationTest : FarseerPhysicsTest
    {
        int _fileCounter;
        string _nextFileName;
        Stopwatch _sw = new Stopwatch();
        double[] _timings = new double[6];
        Body[] _bodies = new Body[6];
        string[] _names = new[] { "Seidel", "Seidel (trapezoids)", "Delauny", "Earclip", "Flipcode", "Bayazit" };


        public override void Initialize()
        {
            base.Initialize();

            CreateBody(LoadNextDataFile());
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.T)
            {
                CreateBody(LoadNextDataFile());
            }

            base.OnKeyDown(e);
        }

        
        void CreateBody(Vertices vertices)
        {
            if (vertices == null)
            {
                return;
            }

            World.Clear();

            _sw.Start();
            _bodies[0] = BodyFactory.CreateCompoundPolygon(World, Triangulate.ConvexPartition(vertices, TriangulationAlgorithm.Seidel), 1);
            _bodies[0].Position = new Vector2(-30, 28);
            _timings[0] = _sw.ElapsedMilliseconds;

            //_sw.Restart();
            _sw.Stop(); _sw.Start();
            _bodies[1] = BodyFactory.CreateCompoundPolygon(World, Triangulate.ConvexPartition(vertices, TriangulationAlgorithm.SeidelTrapezoids), 1);
            _bodies[1].Position = new Vector2(0, 28);
            _timings[1] = _sw.ElapsedMilliseconds;

            //_sw.Restart();
            _sw.Stop(); _sw.Start();
            _bodies[2] = BodyFactory.CreateCompoundPolygon(World, Triangulate.ConvexPartition(vertices, TriangulationAlgorithm.Delauny), 1);
            _bodies[2].Position = new Vector2(30, 28);
            _timings[2] = _sw.ElapsedMilliseconds;

            //_sw.Restart();
            _sw.Stop(); _sw.Start();
            _bodies[3] = BodyFactory.CreateCompoundPolygon(World, Triangulate.ConvexPartition(vertices, TriangulationAlgorithm.Earclip), 1);
            _bodies[3].Position = new Vector2(-30, 5);
            _timings[3] = _sw.ElapsedMilliseconds;

            //_sw.Restart();
            _sw.Stop(); _sw.Start();
            _bodies[4] = BodyFactory.CreateCompoundPolygon(World, Triangulate.ConvexPartition(vertices, TriangulationAlgorithm.Flipcode), 1);
            _bodies[4].Position = new Vector2(0, 5);
            _timings[4] = _sw.ElapsedMilliseconds;

            //_sw.Restart();
            _sw.Stop(); _sw.Start();
            _bodies[5] = BodyFactory.CreateCompoundPolygon(World, Triangulate.ConvexPartition(vertices, TriangulationAlgorithm.Bayazit), 1);
            _bodies[5].Position = new Vector2(30, 5);
            _timings[5] = _sw.ElapsedMilliseconds;

            _sw.Stop();
        }

        
        Vertices LoadNextDataFile()
        {
            string[] files = IO.VirtualDirectory.GetFiles("AltData/FarseerPhysics/Testbed/Data/", "*.dat");
            _nextFileName = files[_fileCounter];

            if (_fileCounter++ >= files.Length - 1)
            {
                _fileCounter = 0;
            }

            string[] lines = null;//File.ReadAllLines(_nextFileName);
            {
                System.IO.Stream stream = Alt.IO.VirtualFile.OpenRead(_nextFileName);
                if (stream == null)
                {
                    return null;
                }

                using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                {
                    System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

                    while (!reader.EndOfStream)
                    {
                        list.Add(reader.ReadLine());
                    }

                    lines = list.ToArray();
                }

                if (lines == null)
                {
                    return null;
                }
            }

            Vertices vertices = new Vertices(lines.Length);

            foreach (string line in lines)
            {
                string[] split = line.Split(' ');
                vertices.Add(new Vector2(double.Parse(split[0]), double.Parse(split[1])));
            }

            return vertices;
        }


        public override void Update(FarseerPhysicsGameSettings settings)
        {
            DrawString("Loaded: " + _nextFileName + " - Press T for next");

            Vector2 offset = new Vector2(-6, 12);

            for (int i = 0; i < _names.Length; i++)
            {
                string title = string.Format("{0}: {1} ms - {2} triangles", _names[i], _timings[i], _bodies[i].FixtureList.Count);

                Vector2 screenPosition = ConvertWorldToScreen(_bodies[i].Position + offset);
                DebugView.DrawString((int)screenPosition.X, (int)screenPosition.Y, title);
            }

            base.Update(settings);
        }


        public static FarseerPhysicsTest Create()
        {
            return new TriangulationTest();
        }
    }
}
