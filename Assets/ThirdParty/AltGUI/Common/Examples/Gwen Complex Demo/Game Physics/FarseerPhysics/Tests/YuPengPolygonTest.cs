/*
* Farseer Physics Engine:
* Copyright (c) 2012 Ian Qvist
*/

//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System.Collections.Generic;

using Alt.FarseerPhysics.Common;
using Alt.FarseerPhysics.Common.PolygonManipulation;
using Alt.GUI.FarseerPhysics.Demo.Testbed.Framework;

using Alt.Sketch;


namespace Alt.GUI.FarseerPhysics.Demo.Testbed.Tests
{
    public class YuPengPolygonTest : FarseerPhysicsTest
    {
        Vertices _clip;
        List<Vertices> _polygons;
        Vertices _selected;
        Vertices _subject;


        public override void Initialize()
        {
            Vector2 trans = new Vector2();
            _polygons = new List<Vertices>();

            _polygons.Add(PolygonTools.CreateGear(5f, 10, 0f, 6f));
            _polygons.Add(PolygonTools.CreateGear(4f, 15, 100f, 3f));

            trans.X = 0f;
            trans.Y = 8f;
            _polygons[0].Translate(ref trans);
            _polygons[1].Translate(ref trans);

            _polygons.Add(PolygonTools.CreateGear(5f, 10, 50f, 5f));

            trans.X = 22f;
            trans.Y = 17f;
            _polygons[2].Translate(ref trans);

            AddRectangle(5, 10);
            AddCircle(5, 32);

            trans.X = -20f;
            trans.Y = 8f;
            _polygons[3].Translate(ref trans);
            trans.Y = 20f;
            _polygons[4].Translate(ref trans);

            _polygons.Add(PolygonTools.CreateRectangle(5f, 5f));
            _polygons.Add(PolygonTools.CreateRectangle(5f, 5f));
            trans.X = 0f;
            trans.Y = 27f;
            _polygons[5].Translate(ref trans);
            _polygons[6].Translate(ref trans);

            _polygons.Add(PolygonTools.CreateRectangle(5f, 5f));
            _polygons.Add(PolygonTools.CreateRectangle(5f, 5f));
            trans.Y = 40f;
            _polygons[7].Translate(ref trans);
            trans.X = 5f;
            _polygons[8].Translate(ref trans);

            _polygons.Add(PolygonTools.CreateRectangle(5f, 5f));
            _polygons.Add(PolygonTools.CreateRectangle(5f, 5f));
            trans.Y = 35f;
            trans.X = 20f;
            _polygons[9].Translate(ref trans);
            trans.Y = 45f;
            trans.X = 25f;
            _polygons[10].Translate(ref trans);

            _subject = _polygons[5];
            _clip = _polygons[6];

            base.Initialize();
        }

        public override void Update(FarseerPhysicsGameSettings settings)
        {
            foreach (Vertices vertex in _polygons)
            {
                if (vertex != null)
                {
                    Vector2[] array = vertex.ToArray();
                    Color col = Color.SteelBlue;
                    if (!vertex.IsCounterClockWise())
                    {
                        col = Color.Aquamarine;
                    }
                    if (vertex == _selected)
                    {
                        col = Color.LightBlue;
                    }
                    if (vertex == _subject)
                    {
                        col = Color.Green;
                        if (vertex == _selected)
                        {
                            col = Color.LightGreen;
                        }
                    }
                    if (vertex == _clip)
                    {
                        col = Color.DarkRed;
                        if (vertex == _selected)
                        {
                            col = Color.IndianRed;
                        }
                    }
                    
                    DebugView.DrawPolygon(array, vertex.Count, col);
                    for (int j = 0; j < vertex.Count; ++j)
                    {
                        DebugView.DrawPoint(vertex[j], .2f, Color.Red);
                    }
                    
                }
            }

            DrawString("A,S,D = Create Rectangle");
            DrawString("Q,W,E = Create Circle");
            DrawString("Click to Drag polygons");
            DrawString("1 = Select Subject while dragging [green]");
            DrawString("2 = Select Clip while dragging [red]");
            DrawString("Space = Union");
            DrawString("Backspace = Subtract");
            DrawString("Shift = Intersect");
            DrawString("Holes are colored light blue");
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            // Add Circles
            if (e.KeyCode == Keys.Q)
                AddCircle(3, 8);

            // Add Circles
            if (e.KeyCode == Keys.W)
                AddCircle(4, 16);

            // Add Circles
            if (e.KeyCode == Keys.E)
                AddCircle(5, 32);

            // Add Rectangle
            if (e.KeyCode == Keys.A)
                AddRectangle(4, 8);

            // Add Rectangle
            if (e.KeyCode == Keys.S)
                AddRectangle(5, 2);

            // Add Rectangle
            if (e.KeyCode == Keys.D)
                AddRectangle(2, 5);

            // Perform a Union
            if (e.KeyCode == Keys.Space)
            {
                if (_subject != null && _clip != null)
				{
					PolyClipError _err;
					DoBooleanOperation(YuPengClipper.Union(_subject, _clip, out _err));
				}
            }

            // Perform a Subtraction
            if (e.KeyCode == Keys.Back)
            {
                if (_subject != null && _clip != null)
				{
					PolyClipError _err;
					DoBooleanOperation(YuPengClipper.Difference(_subject, _clip, out _err));
				}
            }

            // Perform a Intersection
            if (e.KeyCode == Keys.LeftShift)
            {
                if (_subject != null && _clip != null)
				{
					PolyClipError _err;
					DoBooleanOperation(YuPengClipper.Intersect(_subject, _clip, out _err));
				}
            }

            // Select Subject
            if (e.KeyCode == Keys.D1)
            {
                if (_selected != null)
                {
                    if (_clip == _selected)
                        _clip = null;

                    _subject = _selected;
                }
            }

            // Select Clip
            if (e.KeyCode == Keys.D2)
            {
                if (_selected != null)
                {
                    if (_subject == _selected)
                        _subject = null;

                    _clip = _selected;
                }
            }
        }


        public override void MouseDown(MouseEventArgs e, Vector2 p)
        {
            Vector2 position = p;

            if (e.Button == MouseButtons.Left)
            {
                foreach (Vertices vertices in _polygons)
                {
                    if (vertices == null)
                        continue;

                    if (vertices.PointInPolygon(ref position) == 1)
                    {
                        _selected = vertices;
                        break;
                    }
                }
            }

            base.MouseDown(e, p);
        }


        public override void MouseUp(MouseEventArgs e, Vector2 p)
        {
            if (e.Button == MouseButtons.Left)
            {
                _selected = null;
            }

            base.MouseUp(e, p);
        }


        public override void MouseMove(MouseEventArgs e, Vector2 p, Vector2 offset)
        {
            if (_selected != null)
            {
                Vector2 trans = offset;
                _selected.Translate(ref trans);
            }

            base.MouseMove(e, p, offset);
        }


        void DoBooleanOperation(IEnumerable<Vertices> result)
        {
            // Do the union
            _polygons.Remove(_subject);
            _polygons.Remove(_clip);
            _polygons.AddRange(result);
            _subject = null;
            _clip = null;
            _selected = null;
        }

        private void AddCircle(int radius, int numSides)
        {
            Vertices verts = PolygonTools.CreateCircle(radius, numSides);
            _polygons.Add(verts);
        }

        private void AddRectangle(int width, int height)
        {
            Vertices verts = PolygonTools.CreateRectangle(width, height);
            _polygons.Add(verts);
        }

        public static FarseerPhysicsTest Create()
        {
            return new YuPengPolygonTest();
        }
    }
}
