//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.Sketch;
using Alt.Sketch.Geometries;
using Alt.Sketch.Geometries.Reformers;
using Alt.Sketch.Geometries.Transformers;


namespace Alt.GUI.Demo.SimpleSVG
{
    // Basic path attributes
    public class SVGPathAttributes
    {
        public int index;
        public Color FillColor;
        public Color StrokeColor;
        public bool FillFlag;
        public bool StrokeFlag;
        public bool EvenOddFlag;
        public LineJoin LineJoin;
        public LineCap LineCap;
        public double MiterLimit;
        public double StrokeWidth;
        public Matrix4 Transform;


        public SVGPathAttributes()
        {
            index = 0;
            FillColor = Color.Transparent;
            StrokeColor = Color.Transparent;
            FillFlag = true;
            StrokeFlag = false;
            EvenOddFlag = false;
            LineJoin = LineJoin.Miter;
            LineCap = LineCap.Flat;
            MiterLimit = 4.0;
            StrokeWidth = 1.0;
            Transform = Matrix4.Identity;
        }


        public SVGPathAttributes(SVGPathAttributes attr)
        {
            CopyFrom(attr);
        }


        public void CopyFrom(SVGPathAttributes attr)
        {
            index = attr.index;
            FillColor = attr.FillColor;
            StrokeColor = attr.StrokeColor;
            FillFlag = attr.FillFlag;
            StrokeFlag = attr.StrokeFlag;
            EvenOddFlag = attr.EvenOddFlag;
            LineJoin = attr.LineJoin;
            LineCap = attr.LineCap;
            MiterLimit = attr.MiterLimit;
            StrokeWidth = attr.StrokeWidth;
            Transform = attr.Transform;
        }


        public SVGPathAttributes(SVGPathAttributes attr, int idx)
        {
            index = idx;
            FillColor = attr.FillColor;
            StrokeColor = attr.StrokeColor;
            FillFlag = attr.FillFlag;
            StrokeFlag = attr.StrokeFlag;
            EvenOddFlag = attr.EvenOddFlag;
            LineJoin = attr.LineJoin;
            LineCap = attr.LineCap;
            MiterLimit = attr.MiterLimit;
            StrokeWidth = attr.StrokeWidth;
            Transform = attr.Transform;
        }
    }



    // Path container and renderer. 
    public class SVGPath// : Geometry
    {
        GraphicsPath m_storage;
        List<SVGPathAttributes> m_attr_storage;
        List<SVGPathAttributes> m_attr_stack;
        Matrix4 m_transform;

        FlattenCurveGeometry m_curved;

        GeometryStroke m_curved_stroked;
        GeometryMatrix4Transformer m_curved_stroked_trans;

        GeometryMatrix4Transformer m_curved_trans;
        GeometryContour m_curved_trans_contour;


        public SVGPath()
        {
            m_storage = new GraphicsPath();
            m_attr_storage = new List<SVGPathAttributes>();
            m_attr_stack = new List<SVGPathAttributes>();
            m_transform = Matrix4.Identity;

            m_curved = new FlattenCurveGeometry(m_storage);

            m_curved_stroked = new GeometryStroke(m_curved);
            m_curved_stroked_trans = new GeometryMatrix4Transformer(m_curved_stroked, m_transform);

            m_curved_trans = new GeometryMatrix4Transformer(m_curved, m_transform);
            m_curved_trans_contour = new GeometryContour(m_curved_trans);

            m_curved_trans_contour.AutoDetectOrientation = false;
        }


        void RemoveAll()
        {
            m_storage.RemoveAll();
            m_attr_storage.Clear();
            m_attr_stack.Clear();

            m_transform = Matrix4.Identity;
            UpdateMatrixRelations();
        }


        // Use these functions as follows:
        // begin_path() when the XML tag <path> comes ("start_element" handler)
        // parse_path() on "d=" tag attribute
        // end_path() when parsing of the entire tag is done.
        public void begin_path()
        {
            PushAttr();
            int idx = m_storage.StartFigure();
            m_attr_storage.Add(new SVGPathAttributes(cur_attr(), idx));
        }


        public void parse_path(SVGPathTokenizer tok)
        {
            while (tok.next())
            {
                double[] arg = new double[10];
                int cmd = tok.last_command();
                int i;
                switch (cmd)
                {
                    case 'M':
                    case 'm':
                        arg[0] = tok.last_number();
                        arg[1] = tok.next(cmd);
                        MoveTo(arg[0], arg[1], cmd == 'm');
                        break;

                    case 'L':
                    case 'l':
                        arg[0] = tok.last_number();
                        arg[1] = tok.next(cmd);
                        LineTo(arg[0], arg[1], cmd == 'l');
                        break;

                    case 'V':
                    case 'v':
                        vline_to(tok.last_number(), cmd == 'v');
                        break;

                    case 'H':
                    case 'h':
                        hline_to(tok.last_number(), cmd == 'h');
                        break;

                    case 'Q':
                    case 'q':
                        arg[0] = tok.last_number();
                        for (i = 1; i < 4; i++)
                        {
                            arg[i] = tok.next(cmd);
                        }
                        Curve3(arg[0], arg[1], arg[2], arg[3], cmd == 'q');
                        break;

                    case 'T':
                    case 't':
                        arg[0] = tok.last_number();
                        arg[1] = tok.next(cmd);
                        Curve3(arg[0], arg[1], cmd == 't');
                        break;

                    case 'C':
                    case 'c':
                        arg[0] = tok.last_number();
                        for (i = 1; i < 6; i++)
                        {
                            arg[i] = tok.next(cmd);
                        }
                        Curve4(arg[0], arg[1], arg[2], arg[3], arg[4], arg[5], cmd == 'c');
                        break;

                    case 'S':
                    case 's':
                        arg[0] = tok.last_number();
                        for (i = 1; i < 4; i++)
                        {
                            arg[i] = tok.next(cmd);
                        }
                        Curve4(arg[0], arg[1], arg[2], arg[3], cmd == 's');
                        break;

                    case 'A':
                    case 'a':
                        throw new SVGException("parse_path: Command A: NOT IMPLEMENTED YET");

                    case 'Z':
                    case 'z':
                        CloseSubPath();
                        break;

                    default:
                        {
                            throw new SVGException("parse_path: Invalid Command %c", cmd);
                        }
                }
            }
        }


        public void end_path()
        {
            if (m_attr_storage.Count == 0)
            {
                throw new SVGException("end_path : The path was not begun");
            }

            SVGPathAttributes attr = cur_attr();
            int idx = m_attr_storage[m_attr_storage.Count - 1].index;
            attr.index = idx;
            m_attr_storage[m_attr_storage.Count - 1].CopyFrom(attr);// = attr;
            PopAttr();
        }


        // The following functions are essentially a "reflection" of
        // the respective SVG path commands.
        public void MoveTo(double x, double y)
        {
            MoveTo(x, y, false);
        }
        void MoveTo(double x, double y, bool rel)          // M, m
        {
            if (rel) m_storage.RelToAbs(ref x, ref y);
            m_storage.MoveTo(x, y);
        }


        public void LineTo(double x, double y)
        {
            LineTo(x, y, false);
        }
        void LineTo(double x, double y, bool rel)         // L, l
        {
            if (rel) m_storage.RelToAbs(ref x, ref y);
            m_storage.LineTo(x, y);
        }


        void hline_to(double x)
        {
            hline_to(x, false);
        }
        void hline_to(double x, bool rel)                   // H, h
        {
            double x2 = 0.0;
            double y2 = 0.0;
            if (m_storage.VerticesCount > 0)
            {
                m_storage.GetVertex(m_storage.VerticesCount - 1, out x2, out y2);
                if (rel) x += x2;
                m_storage.LineTo(x, y2);
            }
        }


        void vline_to(double y)
        {
            vline_to(y, false);
        }
        void vline_to(double y, bool rel)                   // V, v
        {
            double x2 = 0.0;
            double y2 = 0.0;
            if (m_storage.VerticesCount > 0)
            {
                m_storage.GetVertex(m_storage.VerticesCount - 1, out x2, out y2);
                if (rel) y += y2;
                m_storage.LineTo(x2, y);
            }
        }


        void Curve3(double x1, double y1,                   // Q, q
                    double x, double y)
        {
            Curve3(x1, y1, x, y, false);
        }
        void Curve3(double x1, double y1,                   // Q, q
                    double x, double y, bool rel)
        {
            if (rel)
            {
                m_storage.RelToAbs(ref x1, ref y1);
                m_storage.RelToAbs(ref x, ref y);
            }
            m_storage.Curve3(x1, y1, x, y);
        }


        void Curve3(double x, double y)
        {
            Curve3(x, y, false);
        }
        void Curve3(double x, double y, bool rel)           // T, t
        {
            //        throw new SVGException("Curve3(x, y) : NOT IMPLEMENTED YET");
            if (rel)
            {
                m_storage.Curve3Rel(x, y);
            }
            else
            {
                m_storage.Curve3(x, y);
            }
        }


        void Curve4(double x1, double y1,                   // C, c
                    double x2, double y2,
                    double x, double y)
        {
            Curve4(x1, y1, x2, y2, x, y, false);
        }
        void Curve4(double x1, double y1,                   // C, c
                    double x2, double y2,
                    double x, double y, bool rel)
        {
            if (rel)
            {
                m_storage.RelToAbs(ref x1, ref y1);
                m_storage.RelToAbs(ref x2, ref y2);
                m_storage.RelToAbs(ref x, ref y);
            }
            m_storage.Curve4(x1, y1, x2, y2, x, y);
        }


        void Curve4(double x2, double y2,                   // S, s
                    double x, double y)
        {
            Curve4(x2, y2, x, y, false);
        }
        void Curve4(double x2, double y2,                   // S, s
                    double x, double y, bool rel)
        {
            //throw new SVGException("Curve4(x2, y2, x, y) : NOT IMPLEMENTED YET");
            if (rel)
            {
                m_storage.Curve4Rel(x2, y2, x, y);
            }
            else
            {
                m_storage.Curve4(x2, y2, x, y);
            }
        }


        public void CloseSubPath()                               // Z, z
        {
            m_storage.ClosePolygon(GeometryVertexCommandAndFlags.FlagClose);
        }


        // Call these functions on <g> tag (start_element, end_element respectively)
        public void PushAttr()
        {
            m_attr_stack.Add(m_attr_stack.Count > 0 ?
                             new SVGPathAttributes(m_attr_stack[m_attr_stack.Count - 1]) :
                             new SVGPathAttributes());
        }


        public void PopAttr()
        {
            if (m_attr_stack.Count == 0)
            {
                throw new SVGException("PopAttr : Attribute stack is empty");
            }

            m_attr_stack.RemoveAt(m_attr_stack.Count - 1);// remove_last();
        }


        // Attribute setting functions.
        public void fill(Color f)
        {
            SVGPathAttributes attr = cur_attr();
            attr.FillColor = f;
            attr.FillFlag = true;
        }

        public void stroke(Color s)
        {
            SVGPathAttributes attr = cur_attr();
            attr.StrokeColor = s;
            attr.StrokeFlag = true;
        }

        void even_odd(bool flag)
        {
            cur_attr().EvenOddFlag = flag;
        }

        public void StrokeWidth(double w)
        {
            cur_attr().StrokeWidth = w;
        }

        public void fill_none()
        {
            cur_attr().FillFlag = false;
        }

        public void stroke_none()
        {
            cur_attr().StrokeFlag = false;
        }

        public void fill_opacity(double op)
        {
            cur_attr().FillColor.SetOpacity01(op);
        }

        public void stroke_opacity(double op)
        {
            cur_attr().StrokeColor.SetOpacity01(op);
        }

        public void LineJoin(LineJoin join)
        {
            cur_attr().LineJoin = join;
        }

        public void LineCap(LineCap cap)
        {
            cur_attr().LineCap = cap;
        }

        public void MiterLimit(double ml)
        {
            cur_attr().MiterLimit = ml;
        }


        public Matrix4 Transform
        {
            get
            {
                return cur_attr().Transform;
            }
            set
            {
                cur_attr().Transform = value;
            }
        }

        
        public void UpdateMatrixRelations()
        {
            m_curved_stroked_trans.Transform = m_transform;
            m_curved_trans.Transform = m_transform;
        }


        // Make all polygons CCW-oriented
        public void ArrangeOrientations()
        {
            m_storage.ArrangeOrientationsInAllPaths(GeometryVertexCommandAndFlags.FlagCCW);//path_flags_ccw);
        }


        // Expand all polygons 
        public double Expand
        {
            get
            {
                return m_curved_trans_contour.Width;
            }
            set
            {
                m_curved_trans_contour.Width = value;
            }
        }


        public int GetPathID(int index)
        {
            m_transform = m_attr_storage[index].Transform;
            UpdateMatrixRelations();

            return m_attr_storage[index].index;
        }



        public Rect Bounds
        {
            get
            {
                Rect result = new Rect();
                bool first = true;

                int num = m_attr_storage.Count;

                for (int i = 0; i < num; i++)
                {
                    SVGPathAttributes attr = m_attr_storage[i];
                    m_transform = attr.Transform;
                    UpdateMatrixRelations();

                    double scl = m_transform.Get2DScale();
                    m_curved.ApproximationScale = scl;
                    m_curved.AngleTolerance = 0.0;

                    Geometry geometry = null;

                    if (attr.FillFlag)
                    {
                        if (System.Math.Abs(m_curved_trans_contour.Width) < 0.0001)
                        {
                            geometry = new IndexedPathGeometry(m_curved_trans, attr.index);
                        }
                        else
                        {
                            m_curved_trans_contour.MiterLimit = attr.MiterLimit;

                            geometry = new IndexedPathGeometry(m_curved_trans_contour, attr.index);
                        }
                        
                        if (geometry != null)
                        {
                            if (first)
                            {
                                result = geometry.Bounds;
                                first = false;
                            }
                            else
                            {
                                result.Union(geometry.Bounds);
                            }
                        }
                    }

                    if (attr.StrokeFlag)
                    {
                        m_curved_stroked.Width = attr.StrokeWidth;
                        m_curved_stroked.LineJoin = attr.LineJoin;
                        m_curved_stroked.LineCap = attr.LineCap;
                        m_curved_stroked.MiterLimit = attr.MiterLimit;
                        m_curved_stroked.InnerJoin = Sketch.LineJoin.Round;
                        m_curved_stroked.ApproximationScale = scl;

                        // If the *visual* line width is considerable we 
                        // turn on processing of curve cusps.
                        //---------------------
                        if (attr.StrokeWidth * scl > 1.0)
                        {
                            m_curved.AngleTolerance = 0.2;
                        }

                        geometry = new IndexedPathGeometry(m_curved_stroked_trans, attr.index);

                        if (first)
                        {
                            result = geometry.Bounds;
                            first = false;
                        }
                        else
                        {
                            result.Union(geometry.Bounds);
                        }
                    }
                }

                return result;
            }
        }


        SVGPathAttributes cur_attr()
        {
            if (m_attr_stack.Count == 0)
            {
                throw new SVGException("cur_attr : Attribute stack is empty");
            }

            return m_attr_stack[m_attr_stack.Count - 1];
        }

        

        class IndexedPathGeometry : Geometry
        {
            Geometry m_GeometryVertexSource;
            int m_Index;


            public IndexedPathGeometry(Geometry vs, int index)
            {
                m_GeometryVertexSource = vs;
                m_Index = index;
            }


            public override void Rewind(int path_id)
            {
                m_GeometryVertexSource.Rewind(m_Index + path_id);
            }


            public override GeometryVertexCommandAndFlags GetNextVertex(out double x, out double y)
            {
                return m_GeometryVertexSource.GetNextVertex(out x, out y);
            }
        }



        public void Render(Graphics graphics)
        {
            Render(graphics, Matrix4.Identity);
        }

        public void Render(Graphics graphics, Matrix4 mtx)
        {
            Render(graphics, mtx, null);
        }

        public void Render(Graphics graphics, GeometryProcessor transformer)
        {
            Render(graphics, Matrix4.Identity, transformer);
        }


        public void Render(Graphics graphics, Matrix4 mtx, GeometryProcessor transformer)
        {
            for (int i = 0; i < m_attr_storage.Count; i++)
            {
                SVGPathAttributes attr = m_attr_storage[i];
                m_transform = attr.Transform;
                m_transform.Multiply(mtx, MatrixOrder.Append);
                UpdateMatrixRelations();

                double scl = m_transform.Get2DScale();
                m_curved.ApproximationScale = scl;
                m_curved.AngleTolerance = 0.0;

                if (attr.FillFlag)
                {
                    //TEMP  graphics.FillingRule(attr.EvenOddFlag ? FillEvenOdd : FillNonZero);

                    Color color = attr.FillColor;

                    if (System.Math.Abs(m_curved_trans_contour.Width) < 0.0001)
                    {
                        Geometry geometry = new IndexedPathGeometry(m_curved_trans, attr.index);
                        if (transformer == null)
                        {
                            graphics.FillGeometry(color, geometry);
                        }
                        else
                        {
                            transformer.Geometry = geometry;
                            graphics.FillGeometry(color, transformer);
                        }
                    }
                    else
                    {
                        m_curved_trans_contour.MiterLimit = attr.MiterLimit;

                        Geometry geometry = new IndexedPathGeometry(m_curved_trans_contour, attr.index);
                        if (transformer == null)
                        {
                            graphics.FillGeometry(color, geometry);
                        }
                        else
                        {
                            transformer.Geometry = geometry;
                            graphics.FillGeometry(color, transformer);
                        }
                    }
                }

                if (attr.StrokeFlag)
                {
                    m_curved_stroked.Width = attr.StrokeWidth;
                    m_curved_stroked.LineJoin = attr.LineJoin;
                    m_curved_stroked.LineCap = attr.LineCap;
                    m_curved_stroked.MiterLimit = attr.MiterLimit;
                    m_curved_stroked.InnerJoin = Sketch.LineJoin.Round;
                    m_curved_stroked.ApproximationScale = scl;

                    // If the *visual* line width is considerable we 
                    // turn on processing of curve cusps.
                    //---------------------
                    if (attr.StrokeWidth * scl > 1.0)
                    {
                        m_curved.AngleTolerance = 0.2;
                    }

                    Color color = attr.StrokeColor;

                    //TEMP  graphics.FillingRule(FillNonZero);

                    Geometry geometry = new IndexedPathGeometry(m_curved_stroked_trans, attr.index);
                    if (transformer == null)
                    {
                        graphics.FillGeometry(color, geometry);
                    }
                    else
                    {
                        transformer.Geometry = geometry;
                        graphics.FillGeometry(color, transformer);
                    }
                }
            }
        }
    }
}
