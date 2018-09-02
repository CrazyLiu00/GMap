//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;
using Alt.FarseerPhysics.Collision;
using Alt.FarseerPhysics.Common.Decomposition;
using Alt.FarseerPhysics.Common.TextureTools;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Factories;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class DestructibleTerrainTest : FarseerPhysicsTest
    {
        double _circleRadius = 2.5f;
        Terrain _terrain;
        AABB _terrainArea;

        
        DestructibleTerrainTest()
        {
            World = new World(new Vector2(0, -10));

            _terrainArea = new AABB(new Vector2(0, 0), 100, 100);
            _terrain = new Terrain(World, _terrainArea)
                           {
                               PointsPerUnit = 10,
                               CellSize = 50,
                               SubCellSize = 5,
                               Decomposer = TriangulationAlgorithm.Earclip,
                               Iterations = 2,
                           };

            _terrain.Initialize();
        }

        public override void Initialize()
        {
            Bitmap texture = Bitmap.FromFile("AltData/FarseerPhysics/Testbed/Content/Terrain.png");

            sbyte[,] data = new sbyte[texture.PixelWidth, texture.PixelHeight];

            for (int y = 0; y < texture.PixelHeight; y++)
            {
                for (int x = 0; x < texture.PixelWidth; x++)
                {
                    //If the color on the coordinate is black, we include it in the terrain.
                    bool inside = texture.GetPixel(x, y) == Color.Black;

                    if (!inside)
                        data[x, y] = 1;
                    else
                        data[x, y] = -1;
                }
            }

            _terrain.ApplyData(data, new Vector2(250, 250));

            base.Initialize();
        }


        public override void MouseDown(MouseEventArgs e, Vector2 p)
        {
            Vector2 position = p;

            if (e.Button == MouseButtons.Right)
            {
                DrawCircleOnMap(position, -1);
                _terrain.RegenerateTerrain();

                
                DebugView.DrawSolidCircle(position, _circleRadius, Vector2.UnitY, Color.Blue * 0.5f);
                
            }
            else if (e.Button == MouseButtons.Left)
            {
                DrawCircleOnMap(position, 1);
                _terrain.RegenerateTerrain();

                
                DebugView.DrawSolidCircle(position, _circleRadius, Vector2.UnitY, Color.Red * 0.5f);
                
            }
            else if (e.Button == MouseButtons.Middle)
            {
                Body circle = BodyFactory.CreateCircle(World, 1, 1);
                circle.BodyType = BodyType.Dynamic;
                circle.Position = position;
            }
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.G)
                _circleRadius += 0.05f;
            else if (e.KeyCode == Keys.H)
                _circleRadius -= 0.05f;

            if (e.KeyCode == Keys.T)
            {
                _terrain.Decomposer++;

                if (_terrain.Decomposer > TriangulationAlgorithm.Seidel)
                    _terrain.Decomposer--;
            }
            else if (e.KeyCode == Keys.Y)
            {
                _terrain.Decomposer--;

                if (_terrain.Decomposer < TriangulationAlgorithm.Bayazit)
                    _terrain.Decomposer++;
            }

            base.OnKeyDown(e);
        }

        private void DrawCircleOnMap(Vector2 center, sbyte value)
        {
            for (double by = -_circleRadius; by < _circleRadius; by += 0.1f)
            {
                for (double bx = -_circleRadius; bx < _circleRadius; bx += 0.1f)
                {
                    if ((bx * bx) + (by * by) < _circleRadius * _circleRadius)
                    {
                        double ax = bx + center.X;
                        double ay = by + center.Y;
                        _terrain.ModifyTerrain(new Vector2(ax, ay), value);
                    }
                }
            }
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            
            DebugView.DrawAABB(ref _terrainArea, Color.Red * 0.5f);
            

            DrawString("Left click and drag the mouse to destroy terrain!");
            DrawString("Right click and drag the mouse to create terrain!");
            DrawString("Middle click to create circles!");
            DrawString("Press t or y to cycle between decomposers: " + _terrain.Decomposer);
            TextLine += 25;
            DrawString("Press g or h to decrease/increase circle radius: " + _circleRadius);

            base.Update(settings);
        }

        internal static FarseerPhysicsTest Create()
        {
            return new DestructibleTerrainTest();
        }
    }
}
