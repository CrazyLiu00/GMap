//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using Alt.FarseerPhysics.Collision;
using Alt.FarseerPhysics.Common;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class CheckPolygonTest : FarseerPhysicsTest
    {
        Vertices _vertices = new Vertices();

        
        CheckPolygonTest()
        {
        }

        
        public override void MouseDown(MouseEventArgs e, Vector2 p)
        {
            Vector2 worldPosition = p;

            if (e.Button == MouseButtons.Left)
            {
                _vertices.Add(worldPosition);
            }

            base.MouseDown(e, p);
        }


        public override void Update(FarseerPhysicsGameSettings settings)
        {
            DrawString("Use the mouse to create a polygon.");
            DrawString("Simple: " + _vertices.IsSimple());
            DrawString("Convex: " + _vertices.IsConvex());
            DrawString("CCW: " + _vertices.IsCounterClockWise());
            DrawString("Area: " + _vertices.GetArea());

            PolygonError returnCode = _vertices.CheckPolygon();

            if (returnCode == PolygonError.NoError)
                DrawString("Polygon is supported in Farseer Physics Engine");
            else
                DrawString("Polygon is NOT supported in Farseer Physics Engine. Reason: " + returnCode);

            

            for (int i = 0; i < _vertices.Count; i++)
            {
                Vector2 currentVertex = _vertices[i];
                Vector2 nextVertex = _vertices.NextVertex(i);

                DebugView.DrawPoint(currentVertex, 0.1f, Color.Yellow);
                DebugView.DrawSegment(currentVertex, nextVertex, Color.Red);
            }

            DebugView.DrawPoint(_vertices.GetCentroid(), 0.1f, Color.Green);

            AABB aabb = _vertices.GetAABB();
            DebugView.DrawAABB(ref aabb, Color.HotPink);

            
            base.Update(settings);
        }

        internal static FarseerPhysicsTest Create()
        {
            return new CheckPolygonTest();
        }
    }
}