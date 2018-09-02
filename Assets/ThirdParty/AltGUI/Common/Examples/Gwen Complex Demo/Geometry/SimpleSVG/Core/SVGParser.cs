//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.IO;
using Alt.Sketch;
using Alt.Xml;


namespace Alt.GUI.Demo.SimpleSVG
{
    public class SVGParser
    {
        SVGPath m_SVGPath;
        SVGPathTokenizer m_SVGPathTokenizer;
        string m_Title;
        bool m_TitleFlag;
        bool m_PathFlag;
        string m_AttrName;
        string m_AttrValue;


        public SVGParser(SVGPath path)
        {
            m_SVGPath = path;
            m_SVGPathTokenizer = new SVGPathTokenizer();
            m_TitleFlag = false;
            m_PathFlag = false;
        }


        public void Parse(string fname)
        {
            XmlElement root = null;
            if (Alt.IO.VirtualFile.Exists(fname))
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;

                //doc.Load(fname);
                System.IO.Stream stream = Alt.IO.VirtualFile.OpenRead(fname);
                if (stream == null)
                {
                    return;
                }
                doc.Load(stream);

                root = doc.DocumentElement;
            }

            if (root == null)
            {
                return;
            }


            XmlNodeList nl = root.GetElementsByTagName("title");
            if (nl != null)
            {
                foreach (XmlNode node in nl)
                {
                    m_Title = node.InnerText;

                    int len = m_Title.Length;
                    int space = ' ';
                    for (int i = 0; i < len; i++)
                    {
                        if (m_Title[i] < space)
                        {
                            m_Title = m_Title.Replace(m_Title[i], ' ');
                        }
                    }

                    break;
                }
            }


            nl = root.ChildNodes;
            if (nl != null)
            {
                foreach (XmlNode n in nl)
                {
                    parse_node(n);
                }
            }
        }


        public string Title
        {
            get
            {
                return m_Title;
            }
        }


        // XML event handlers
        static void start_element(object data, string el, string[] attr)
        {
            SVGParser self = (SVGParser)data;

            if (string.Equals(el, "title"))
            {
                self.m_TitleFlag = true;
            }
            else if (string.Equals(el, "g"))
            {
                self.m_SVGPath.PushAttr();
                self.parse_attr(attr);
            }
            else if (string.Equals(el, "path"))
            {
                if (self.m_PathFlag)
                {
                    throw new SVGException("start_element: Nested path");
                }
                self.m_SVGPath.begin_path();
                self.parse_path(attr);
                self.m_SVGPath.end_path();
                self.m_PathFlag = true;
            }
            else if (string.Equals(el, "rect"))
            {
                self.parse_rect(attr);
            }
            else if (string.Equals(el, "line"))
            {
                self.parse_line(attr);
            }
            else if (string.Equals(el, "polyline"))
            {
                self.parse_poly(attr, false);
            }
            else if (string.Equals(el, "polygon"))
            {
                self.parse_poly(attr, true);
            }
            //else if(string.Equals(el, "<OTHER_ELEMENTS>")) 
            //{
            //}
            // . . .
        }


        static void end_element(object data, string el)
        {
            SVGParser self = (SVGParser)data;

            if (string.Equals(el, "title"))
            {
                self.m_TitleFlag = false;
            }
            else if (string.Equals(el, "g"))
            {
                self.m_SVGPath.PopAttr();
            }
            else if (string.Equals(el, "path"))
            {
                self.m_PathFlag = false;
            }
            //else if(string.Equals(el, "<OTHER_ELEMENTS>")) 
            //{
            //}
            // . . .
        }


        static void content(object data, string s)
        {
            SVGParser self = (SVGParser)data;

            // m_TitleFlag signals that the <title> tag is being parsed now.
            // The following code concatenates the pieces of content of the <title> tag.
            if (self.m_TitleFlag)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    self.m_Title += s;
                }
            }
        }


        void parse_attr(string[] attr)
        {
            int i;
            for (i = 0; attr[i] != null; i += 2)
            {
                if (string.Equals(attr[i], "style"))
                {
                    parse_style(attr[i + 1]);
                }
                else
                {
                    parse_attr(attr[i], attr[i + 1]);
                }
            }
        }


        void parse_path(string[] attr)
        {
            int i;

            for (i = 0; attr[i] != null; i += 2)
            {
                // The <path> tag can consist of the path itself ("d=") 
                // as well as of other parameters like "style=", "transform=", etc.
                // In the last case we simply rely on the function of parsing 
                // attributes (see 'else' branch).
                if (string.Equals(attr[i], "d"))
                {
                    m_SVGPathTokenizer.set_path_str(attr[i + 1]);
                    m_SVGPath.parse_path(m_SVGPathTokenizer);
                }
                else
                {
                    // Create a temporary single pair "name-value" in order
                    // to avoid multiple calls for the same attribute.
                    string[] tmp = new string[4];
                    tmp[0] = attr[i];
                    tmp[1] = attr[i + 1];
                    tmp[2] = null;
                    tmp[3] = null;
                    parse_attr(tmp);
                }
            }
        }


        static Color parse_color(string str)
        {
            return Color.Parse(str);
        }


        static double parse_double(string str)
        {
            str.Trim();
            return double.Parse(str, System.Globalization.CultureInfo.InvariantCulture);
        }


        void parse_poly(string[] attr, bool close_flag)
        {
            int i;
            double x = 0.0;
            double y = 0.0;

            m_SVGPath.begin_path();
            for (i = 0; attr[i] != null; i += 2)
            {
                if (!parse_attr(attr[i], attr[i + 1]))
                {
                    if (string.Equals(attr[i], "points"))
                    {
                        m_SVGPathTokenizer.set_path_str(attr[i + 1]);
                        if (!m_SVGPathTokenizer.next())
                        {
                            throw new SVGException("parse_poly: Too few coordinates");
                        }
                        x = m_SVGPathTokenizer.last_number();
                        if (!m_SVGPathTokenizer.next())
                        {
                            throw new SVGException("parse_poly: Too few coordinates");
                        }
                        y = m_SVGPathTokenizer.last_number();
                        m_SVGPath.MoveTo(x, y);
                        while (m_SVGPathTokenizer.next())
                        {
                            x = m_SVGPathTokenizer.last_number();
                            if (!m_SVGPathTokenizer.next())
                            {
                                throw new SVGException("parse_poly: Odd number of coordinates");
                            }
                            y = m_SVGPathTokenizer.last_number();
                            m_SVGPath.LineTo(x, y);
                        }
                    }
                }
            }

            if (close_flag)
            {
                m_SVGPath.CloseSubPath();
            }

            m_SVGPath.end_path();
        }


        void parse_rect(string[] attr)
        {
            int i;
            double x = 0.0;
            double y = 0.0;
            double w = 0.0;
            double h = 0.0;

            m_SVGPath.begin_path();

            for (i = 0; attr[i] != null; i += 2)
            {
                if (!parse_attr(attr[i], attr[i + 1]))
                {
                    if (string.Equals(attr[i], "x"))
                        x = parse_double(attr[i + 1]);
                    if (string.Equals(attr[i], "y"))
                        y = parse_double(attr[i + 1]);
                    if (string.Equals(attr[i], "width"))
                        w = parse_double(attr[i + 1]);
                    if (string.Equals(attr[i], "Height"))
                        h = parse_double(attr[i + 1]);
                    // rx - to be implemented 
                    // ry - to be implemented
                }
            }

            if (w != 0.0 && h != 0.0)
            {
                if (w < 0.0) throw new SVGException("parse_rect: Invalid width: " + w.ToString());
                if (h < 0.0) throw new SVGException("parse_rect: Invalid Height: " + h.ToString());

                m_SVGPath.MoveTo(x, y);
                m_SVGPath.LineTo(x + w, y);
                m_SVGPath.LineTo(x + w, y + h);
                m_SVGPath.LineTo(x, y + h);
                m_SVGPath.CloseSubPath();
            }

            m_SVGPath.end_path();
        }


        void parse_line(string[] attr)
        {
            int i;
            double x1 = 0.0;
            double y1 = 0.0;
            double x2 = 0.0;
            double y2 = 0.0;

            m_SVGPath.begin_path();
            for (i = 0; attr[i] != null; i += 2)
            {
                if (!parse_attr(attr[i], attr[i + 1]))
                {
                    if (string.Equals(attr[i], "x1"))
                        x1 = parse_double(attr[i + 1]);
                    if (string.Equals(attr[i], "y1"))
                        y1 = parse_double(attr[i + 1]);
                    if (string.Equals(attr[i], "x2"))
                        x2 = parse_double(attr[i + 1]);
                    if (string.Equals(attr[i], "y2"))
                        y2 = parse_double(attr[i + 1]);
                }
            }

            m_SVGPath.MoveTo(x1, y1);
            m_SVGPath.LineTo(x2, y2);
            m_SVGPath.end_path();
        }
        

        void parse_style(string str)
        {
            while (!string.IsNullOrEmpty(str))
            {                
                str = str.TrimStart();

                string nv = null;
                int nv_end = str.IndexOf(';');
                if (nv_end >= 0)
                {
                    nv = str.Substring(0, nv_end);
                    str = str.Remove(0, nv_end + 1);
                }
                else
                {
                    nv = str;
                    str = null;
                }


                if (nv != null)
                {
                    nv.TrimEnd();
                    parse_name_value(nv);
                }
            }
        }


        void parse_transform(string str)
        {
            while (str.Length > 0)
            {
                if (char.IsLower(str[0]))
                {
                    if (str.StartsWith("matrix"))
                    {
                        str = str.Remove(0, parse_matrix(str));
                    }
                    else if (str.StartsWith("translate"))
                    {
                        str = str.Remove(0, parse_translate(str));
                    }
                    else if (str.StartsWith("rotate"))
                    {
                        str = str.Remove(0, parse_rotate(str));
                    }
                    else if (str.StartsWith("scale"))
                    {
                        str = str.Remove(0, parse_scale(str));
                    }
                    else if (str.StartsWith("skewX"))
                    {
                        str = str.Remove(0, parse_skew_x(str));
                    }
                    else if (str.StartsWith("skewY"))
                    {
                        str = str.Remove(0, parse_skew_y(str));
                    }
                    else
                    {
                        str = str.Remove(0, 1);
                    }
                }
                else
                {
                    str = str.Remove(0, 1);
                }
            }
        }


        static bool is_numeric(char c)
        {
            return "0123456789+-.eE".Contains(c.ToString());
        }


        static int parse_transform_args(string s, double[] args, ref int na)
        {
            int max_na = args.Length;

            int len = s.IndexOf(')') + 1;
            int n = s.IndexOf('(');
            if (n >= 0)
            {
                s = s.Remove(0, n + 1);
            }

            n = s.IndexOf(')');
            if (n >= 0)
            {
                s = s.Remove(n);
            }

            string[] values = s.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            n = System.Math.Min(values.Length, max_na);
            for (int i = 0; i < n; i++)
            {
                args[i] = double.Parse(values[i].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                na++;
            }

            return len;
        }


        int parse_matrix(string str)
        {
            double[] args = new double[6];
            int na = 0;
            int len = parse_transform_args(str, args, ref na);
            
            if (na != 6)
            {
                throw new SVGException("parse_matrix: Invalid number of arguments");
            }

            m_SVGPath.Transform = m_SVGPath.Transform.Multiply(new Matrix4(args[0], args[1], args[2], args[3], args[4], args[5]), MatrixOrder.Append);

            return len;
        }


        int parse_translate(string str)
        {
            double[] args = new double[2];
            int na = 0;
            int len = parse_transform_args(str, args, ref na);
            
            if (na == 1)
            {
                args[1] = 0.0;
            }

            m_SVGPath.Transform = m_SVGPath.Transform.Multiply(Matrix4.CreateTranslation(args[0], args[1]), MatrixOrder.Append);

            return len;
        }


        int parse_rotate(string str)
        {
            double[] args = new double[3];
            int na = 0;
            int len = parse_transform_args(str, args, ref na);
            if (na == 1)
            {
                m_SVGPath.Transform = m_SVGPath.Transform.Multiply(Matrix4.CreateRotation(MathHelper.DegreesToRadians(args[0])), MatrixOrder.Append);
            }
            else if (na == 3)
            {
                Matrix4 t = Matrix4.CreateTranslation(-args[1], -args[2]);
                t.Rotate(args[0], MatrixOrder.Append);
                t.Translate(args[1], args[2], MatrixOrder.Append);
                m_SVGPath.Transform = m_SVGPath.Transform.Multiply(t, MatrixOrder.Append);
            }
            else
            {
                throw new SVGException("parse_rotate: Invalid number of arguments");
            }

            return len;
        }


        int parse_scale(string str)
        {
            double[] args = new double[2];
            int na = 0;
            int len = parse_transform_args(str, args, ref na);
            if (na == 1) args[1] = args[0];
            m_SVGPath.Transform = m_SVGPath.Transform.Multiply(Matrix4.CreateScaling(args[0], args[1]), MatrixOrder.Append);

            return len;
        }


        int parse_skew_x(string str)
        {
            double[] arg = new double[1];
            int na = 0;
            int len = parse_transform_args(str, arg, ref na);
            m_SVGPath.Transform = m_SVGPath.Transform.Multiply(Matrix4.CreateSkewing(MathHelper.DegreesToRadians(arg[0]), 0.0), MatrixOrder.Append);

            return len;
        }


        int parse_skew_y(string str)
        {
            double[] arg = new double[1];
            int na = 0;
            int len = parse_transform_args(str, arg, ref na);
            m_SVGPath.Transform = m_SVGPath.Transform.Multiply(Matrix4.CreateSkewing(0.0, MathHelper.DegreesToRadians(arg[0])), MatrixOrder.Append);

            return len;
        }


        bool parse_attr(string name, string value)
        {
            if (string.Equals(name, "style"))
            {
                parse_style(value);
            }
            else if (string.Equals(name, "fill"))
            {
                if (string.Equals(value, "none"))
                {
                    m_SVGPath.fill_none();
                }
                else
                {
                    m_SVGPath.fill(parse_color(value));
                }
            }
            else if (string.Equals(name, "fill-opacity"))
            {
                m_SVGPath.fill_opacity(parse_double(value));
            }
            else if (string.Equals(name, "stroke"))
            {
                if (string.Equals(value, "none"))
                {
                    m_SVGPath.stroke_none();
                }
                else
                {
                    m_SVGPath.stroke(parse_color(value));
                }
            }
            else if (string.Equals(name, "stroke-width"))
            {
                m_SVGPath.StrokeWidth(parse_double(value));
            }
            else if (string.Equals(name, "stroke-linecap"))
            {
                if (string.Equals(value, "butt"))
                    m_SVGPath.LineCap(LineCap.Flat);
                else if (string.Equals(value, "round"))
                    m_SVGPath.LineCap(LineCap.Round);
                else if (string.Equals(value, "square"))
                    m_SVGPath.LineCap(LineCap.Square);
            }
            else if (string.Equals(name, "stroke-linejoin"))
            {
                if (string.Equals(value, "miter"))
                    m_SVGPath.LineJoin(LineJoin.Miter);
                else if (string.Equals(value, "round"))
                    m_SVGPath.LineJoin(LineJoin.Round);
                else if (string.Equals(value, "bevel"))
                    m_SVGPath.LineJoin(LineJoin.Bevel);
            }
            else if (string.Equals(name, "stroke-miterlimit"))
            {
                m_SVGPath.MiterLimit(parse_double(value));
            }
            else if (string.Equals(name, "stroke-opacity"))
            {
                m_SVGPath.stroke_opacity(parse_double(value));
            }
            else if (string.Equals(name, "transform"))
            {
                parse_transform(value);
            }
            //else if(string.Equals(name, "<OTHER_ATTRIBUTES>")) 
            //{
            //}
            // . . .
            else
            {
                return false;
            }

            return true;
        }


        bool parse_name_value(string nvs)
        {
            string[] nv = nvs.Split(new char[] { ':' });
            if (nv.Length == 2)
            {
                m_AttrName = nv[0].Trim();
                m_AttrValue = nv[1].Trim();

                return parse_attr(m_AttrName, m_AttrValue);
            }

            return false;
        }


        void parse_node(XmlNode node)
        {
            string name = node.Name;

            string[] attr = null;
            XmlAttributeCollection ac = node.Attributes;
            if (ac != null &&
                ac.Count > 0)
            {
                attr = new string[ac.Count * 2 + 1];

                int i = 0;
                foreach (XmlAttribute a in ac)
                {
                    attr[i] = a.Name;
                    attr[i + 1] = a.Value;

                    i += 2;
                }

                attr[i] = null;
            }
            else
            {
                attr = new string[1];
                attr[0] = null;
            }


            start_element(this, name, attr);


            XmlNodeList nl = node.ChildNodes;
            if (nl != null)
            {
                foreach (XmlNode n in nl)
                {
                    parse_node(n);
                }
            }


            end_element(this, name);
        }
    }
}
