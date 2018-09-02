//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.


using System.Diagnostics;

#if WINDOWS_PHONE_7 || WINDOWS_PHONE_71
using Alt.Diagnostics;
#endif

using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Common.Decomposition;
using Alt.FarseerPhysics.Dynamics;
using Alt.FarseerPhysics.Factories;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class TextureVerticesTest : FarseerPhysicsTest
    {
        Stopwatch _sw = new Stopwatch();


        public override void Initialize()
        {
            base.Initialize();

            //Ground
            BodyFactory.CreateEdge(World, new Vector2(-40.0f, 0.0f), new Vector2(40.0f, 0.0f));

            //Load texture that will represent the physics body
            Bitmap polygonTexture = Bitmap.FromFile("AltData/FarseerPhysics/Testbed/Content/Texture.png");

            //Create an array to hold the data from the texture
            uint[] data = new uint[polygonTexture.PixelWidth * polygonTexture.PixelHeight];

            //Transfer the texture data to the array
            BitmapData bitmapData = polygonTexture.LockBits(ImageLockMode.ReadOnly);
            byte[] src_buffer = bitmapData.Scan0;
            System.Buffer.BlockCopy(src_buffer, 0, data, 0, src_buffer.Length);
            polygonTexture.UnlockBits(bitmapData);

            //Find the vertices that makes up the outline of the shape in the texture
            Vertices verts = PolygonTools.CreatePolygon(data, polygonTexture.PixelWidth);

            //For now we need to scale the vertices (result is in pixels, we use meters)
            Vector2 scale = new Vector2(0.07f, -0.07f);
            verts.Scale(ref scale);

            //We also need to move the polygon so that (0,0) is the center of the polygon.
            Vector2 centroid = -verts.GetCentroid();
            verts.Translate(ref centroid);

            _sw.Start();
            //Create a single body with multiple fixtures
            Body compund = BodyFactory.CreateCompoundPolygon(World, Triangulate.ConvexPartition(verts, TriangulationAlgorithm.Earclip), 1);
            compund.BodyType = BodyType.Dynamic;
            compund.Position = new Vector2(0, 20);
            _sw.Stop();
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            DrawString("Triangulation took " + _sw.ElapsedMilliseconds + " ms");
            

            base.Update(settings);
        }

        public static FarseerPhysicsTest Create()
        {
            return new TextureVerticesTest();
        }
    }
}