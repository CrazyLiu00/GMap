//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using Alt.Sketch;
using Alt.Sketch.Geometries;


namespace Alt.NETType
{
    static partial class ANTExt
    {
        class ANTOutlineGeometry : Geometry
        {
            class GeometryItem
            {
                public double m_X;
                public double m_Y;
                public GeometryVertexCommandAndFlags m_VertexFlagsAndCommand;


                public GeometryItem(double x, double y, GeometryVertexCommandAndFlags f)
                {
                    m_X = x;
                    m_Y = y;
                    m_VertexFlagsAndCommand = f;
                }
            }


            List<GeometryItem> m_Vertices = new List<GeometryItem>();
            int m_VertexID = 0;
            bool m_Closed = true;


            public ANTOutlineGeometry()
            {
            }


            public void RemoveAll()
            {
                m_Vertices.Clear();

                Modified();
            }


            public void MoveTo(double x, double y)
            {
                m_Vertices.Add(new GeometryItem(x, y, GeometryVertexCommandAndFlags.CommandMoveTo));

                Modified();
            }


            public void LineTo(double x, double y)
            {
                m_Vertices.Add(new GeometryItem(x, y, GeometryVertexCommandAndFlags.CommandLineTo));

                Modified();
            }


            public void Сurve3(double x_ctrl, double y_ctrl,
                        double x_to, double y_to)
            {
                m_Vertices.Add(new GeometryItem(x_ctrl, y_ctrl, GeometryVertexCommandAndFlags.CommandCurve3));
                m_Vertices.Add(new GeometryItem(x_to, y_to, GeometryVertexCommandAndFlags.CommandCurve3));

                Modified();
            }


            public void Сurve4(double x_ctrl1, double y_ctrl1,
                        double x_ctrl2, double y_ctrl2,
                        double x_to, double y_to)
            {
                m_Vertices.Add(new GeometryItem(x_ctrl1, y_ctrl1, GeometryVertexCommandAndFlags.CommandCurve4));
                m_Vertices.Add(new GeometryItem(x_ctrl2, y_ctrl2, GeometryVertexCommandAndFlags.CommandCurve4));
                m_Vertices.Add(new GeometryItem(x_to, y_to, GeometryVertexCommandAndFlags.CommandCurve4));

                Modified();
            }


            public void ClosePolygon()
            {
                Modified();
            }


            int m_VerticesCount;
            public override void Rewind(int pathId)
            {
                m_VertexID = 0;
                m_Closed = true;

                m_VerticesCount = m_Vertices.Count;
            }


            public override GeometryVertexCommandAndFlags GetNextVertex(out double x, out double y)
            {
                if (m_VerticesCount < 2 ||
                    m_VertexID > m_VerticesCount)
                {
                    x = 0;
                    y = 0;

                    return GeometryVertexCommandAndFlags.CommandStop;
                }

                if (m_VertexID == m_VerticesCount)
                {
                    x = 0;
                    y = 0;

                    m_VertexID++;

                    return GeometryVertexCommandAndFlags.CommandEndPoly | GeometryVertexCommandAndFlags.FlagClose;
                }

                GeometryItem v = m_Vertices[m_VertexID];
                x = v.m_X;
                y = v.m_Y;
                GeometryVertexCommandAndFlags cmd = v.m_VertexFlagsAndCommand;


                if (IsMoveTo(cmd) &&
                    !m_Closed)
                {
                    x = 0;
                    y = 0;

                    m_Closed = true;

                    return GeometryVertexCommandAndFlags.CommandEndPoly | GeometryVertexCommandAndFlags.FlagClose;
                }

                m_Closed = false;
                m_VertexID++;

                return cmd;
            }
        }



        [ThreadStatic]
        static ANT_Vector outline_v_last = new ANT_Vector();

        [ThreadStatic]
        static ANT_Vector outline_v_control = new ANT_Vector();

        [ThreadStatic]
        static ANT_Vector outline_v_start = new ANT_Vector();

        [ThreadStatic]
        static ANT_Vector outline_vec = new ANT_Vector();

        [ThreadStatic]
        static ANT_Vector outline_v_middle = new ANT_Vector();

        [ThreadStatic]
        static ANT_Vector outline_vec1 = new ANT_Vector();

        [ThreadStatic]
        static ANT_Vector outline_vec2 = new ANT_Vector();

        public static Geometry ANT_Outline_ToGeometry(ANT_Outline outline, bool flipY, Matrix4 mtx)
        {
            if (outline == null)
            {
                return null;
            }

            ANTOutlineGeometry path = new ANTOutlineGeometry();

            double x1, y1, x2, y2, x3, y3;

            int n;         // index of contour in outline
            int first;     // index of first point in contour
            int tag;       // current point's state

            first = 0;

            ANT_Vector_Array points = outline.points;

            for (n = 0; n < outline.n_contours; n++)
            {
                int last;  // index of last point in contour

                last = outline.contours[n];

                int limit = last;

                outline_v_start.CopyFrom(points[first]);

                outline_v_last.CopyFrom(points[last]);

                outline_v_control.CopyFrom(outline_v_start);

                int point_index = first;

                ANT_Byte_Array tags_buf = outline.tags;
                int tags_index = first;
                tag = ANT.ANT_CURVE_TAG(tags_buf[tags_index + 0]);

                // A contour cannot start with a cubic control point!
                if (tag == ANT_CURVE_Tag.ANT_CURVE_TAG_CUBIC)
                {
                    return null;
                }

                // check first point to determine origin
                if (tag == ANT_CURVE_Tag.ANT_CURVE_TAG_CONIC)
                {
                    // first point is conic control.  Yes, this happens.
                    if (ANT.ANT_CURVE_TAG(tags_buf[last]) == ANT_CURVE_Tag.ANT_CURVE_TAG_ON)
                    {
                        // start at last point if it is on the curve
                        outline_v_start.CopyFrom(outline_v_last);

                        limit--;
                    }
                    else
                    {
                        // if both first and last points are conic,
                        // start at their middle and record its position
                        // for closure
                        outline_v_start.x = (outline_v_start.x + outline_v_last.x) / 2;
                        outline_v_start.y = (outline_v_start.y + outline_v_last.y) / 2;

                        outline_v_last.CopyFrom(outline_v_start);
                    }

                    point_index--;
                    tags_index--;
                }

                x1 = ANT.ANT_int26p6_to_double(outline_v_start.x);
                y1 = ANT.ANT_int26p6_to_double(outline_v_start.y);

                if (flipY)
                {
                    y1 = -y1;
                }

                mtx.Transform(ref x1, ref y1);

                path.MoveTo(x1, y1);

                while (point_index < limit)
                {
                    point_index++;
                    ANT_Vector point = points[point_index];

                    tags_index++;

                    tag = ANT.ANT_CURVE_TAG(tags_buf[tags_index + 0]);
                    switch (tag)
                    {
                        case ANT_CURVE_Tag.ANT_CURVE_TAG_ON:  // emit a single line_to
                            {
                                x1 = ANT.ANT_int26p6_to_double(point.x);
                                y1 = ANT.ANT_int26p6_to_double(point.y);

                                if (flipY)
                                {
                                    y1 = -y1;
                                }

                                mtx.Transform(ref x1, ref y1);

                                path.LineTo(x1, y1);

                                continue;
                            }

                        case ANT_CURVE_Tag.ANT_CURVE_TAG_CONIC:  // consume conic arcs
                            {
                                outline_v_control.x = point.x;
                                outline_v_control.y = point.y;

                            Do_Conic:
                                if (point_index < limit)
                                {
                                    point_index++;
                                    point = points[point_index];

                                    tags_index++;
                                    tag = ANT.ANT_CURVE_TAG(tags_buf[tags_index + 0]);

                                    outline_vec.CopyFrom(point);

                                    if (tag == ANT_CURVE_Tag.ANT_CURVE_TAG_ON)
                                    {
                                        x1 = ANT.ANT_int26p6_to_double(outline_v_control.x);
                                        y1 = ANT.ANT_int26p6_to_double(outline_v_control.y);
                                        x2 = ANT.ANT_int26p6_to_double(outline_vec.x);
                                        y2 = ANT.ANT_int26p6_to_double(outline_vec.y);

                                        if (flipY)
                                        {
                                            y1 = -y1;
                                            y2 = -y2;
                                        }

                                        mtx.Transform(ref x1, ref y1);
                                        mtx.Transform(ref x2, ref y2);
                                        path.Сurve3(x1,
                                                    y1,
                                                    x2,
                                                    y2);
                                        continue;
                                    }

                                    if (tag != ANT_CURVE_Tag.ANT_CURVE_TAG_CONIC)
                                    {
                                        return null;
                                    }

                                    outline_v_middle.x = (outline_v_control.x + outline_vec.x) / 2;
                                    outline_v_middle.y = (outline_v_control.y + outline_vec.y) / 2;

                                    x1 = ANT.ANT_int26p6_to_double(outline_v_control.x);
                                    y1 = ANT.ANT_int26p6_to_double(outline_v_control.y);
                                    x2 = ANT.ANT_int26p6_to_double(outline_v_middle.x);
                                    y2 = ANT.ANT_int26p6_to_double(outline_v_middle.y);

                                    if (flipY)
                                    {
                                        y1 = -y1;
                                        y2 = -y2;
                                    }

                                    mtx.Transform(ref x1, ref y1);
                                    mtx.Transform(ref x2, ref y2);

                                    path.Сurve3(x1,
                                                y1,
                                                x2,
                                                y2);

                                    outline_v_control.CopyFrom(outline_vec);

                                    goto Do_Conic;
                                }

                                x1 = ANT.ANT_int26p6_to_double(outline_v_control.x);
                                y1 = ANT.ANT_int26p6_to_double(outline_v_control.y);
                                x2 = ANT.ANT_int26p6_to_double(outline_v_start.x);
                                y2 = ANT.ANT_int26p6_to_double(outline_v_start.y);

                                if (flipY)
                                {
                                    y1 = -y1;
                                    y2 = -y2;
                                }

                                mtx.Transform(ref x1, ref y1);
                                mtx.Transform(ref x2, ref y2);

                                path.Сurve3(x1,
                                            y1,
                                            x2,
                                            y2);

                                goto Close;
                            }

                        default:
                            {
                                if (point_index + 1 > limit ||
                                    ANT.ANT_CURVE_TAG(tags_buf[tags_index + 1]) != ANT_CURVE_Tag.ANT_CURVE_TAG_CUBIC)
                                {
                                    return null;
                                }

                                outline_vec1.x = points[point_index + 0].x;
                                outline_vec1.y = points[point_index + 0].y;
                                outline_vec2.x = points[point_index + 1].x;
                                outline_vec2.y = points[point_index + 1].y;

                                point_index += 2;
                                {
                                    point = points[point_index];
                                }

                                tags_index += 2;

                                if (point_index <= limit)
                                {
                                    outline_vec.CopyFrom(point);

                                    x1 = ANT.ANT_int26p6_to_double(outline_vec1.x);
                                    y1 = ANT.ANT_int26p6_to_double(outline_vec1.y);
                                    x2 = ANT.ANT_int26p6_to_double(outline_vec2.x);
                                    y2 = ANT.ANT_int26p6_to_double(outline_vec2.y);
                                    x3 = ANT.ANT_int26p6_to_double(outline_vec.x);
                                    y3 = ANT.ANT_int26p6_to_double(outline_vec.y);

                                    if (flipY)
                                    {
                                        y1 = -y1;
                                        y2 = -y2;
                                        y3 = -y3;
                                    }

                                    mtx.Transform(ref x1, ref y1);
                                    mtx.Transform(ref x2, ref y2);
                                    mtx.Transform(ref x3, ref y3);

                                    path.Сurve4(x1,
                                                y1,
                                                x2,
                                                y2,
                                                x3,
                                                y3);

                                    continue;
                                }

                                x1 = ANT.ANT_int26p6_to_double(outline_vec1.x);
                                y1 = ANT.ANT_int26p6_to_double(outline_vec1.y);
                                x2 = ANT.ANT_int26p6_to_double(outline_vec2.x);
                                y2 = ANT.ANT_int26p6_to_double(outline_vec2.y);
                                x3 = ANT.ANT_int26p6_to_double(outline_v_start.x);
                                y3 = ANT.ANT_int26p6_to_double(outline_v_start.y);

                                if (flipY)
                                {
                                    y1 = -y1;
                                    y2 = -y2;
                                    y3 = -y3;
                                }

                                mtx.Transform(ref x1, ref y1);
                                mtx.Transform(ref x2, ref y2);
                                mtx.Transform(ref x3, ref y3);

                                path.Сurve4(x1,
                                            y1,
                                            x2,
                                            y2,
                                            x3,
                                            y3);

                                goto Close;
                            }
                    }
                }

                path.ClosePolygon();

            Close:
                first = last + 1;
            }

            return path;
        }



        class bitset_iterator
        {
            byte[] m_bits;
            byte m_mask;
            int m_buffer_offset;


            public bitset_iterator(byte[] bits, int buffer_offset)
            {
                m_bits = bits;
                m_buffer_offset = buffer_offset;
                m_mask = 0x80;
            }

            public void Inc()
            {
                m_mask >>= 1;

                if (m_mask == 0)
                {
                    m_buffer_offset++;
                    m_mask = 0x80;
                }
            }

            public bool bit()
            {
                return (m_bits[m_buffer_offset] & m_mask) != 0;
            }

            public void Reset(int buffer_offset)
            {
                m_buffer_offset = buffer_offset;
                m_mask = 0x80;
            }
        }


        
        public static Bitmap ToBitmap(ANT_Bitmap bitmap)
        {
            return ToBitmap(bitmap, false);
        }

        const byte cover_full = 255;
        public static Bitmap ToBitmap(ANT_Bitmap bitmap, bool flipY)
        {
            if (bitmap == null)
            {
                return null;
            }
            
            byte[] buf = bitmap.buffer;
            if (buf == null ||
                buf.Length == 0)
            {
                return null;
            }
            
            
            int buf_index = 0;

            int pitch = bitmap.pitch;
            //NoNeed	int bpitch = pitch;

            if (!flipY)
            {
                buf_index += bitmap.pitch * (bitmap.rows - 1);
                pitch = -pitch;
            }

            int bitmap_rows = bitmap.rows;
            int bitmap_width = bitmap.width;

            bitset_iterator bits = new bitset_iterator(bitmap.buffer, buf_index);

            Bitmap result = new Bitmap(bitmap_width, bitmap_rows, PixelFormat.Format32bppArgb);
            BitmapData bitmapData = result.LockBits(ImageLockMode.WriteOnly);
            byte[] resultData = bitmapData.Scan0;
            int resultStride = bitmapData.Stride;
            int resultBpp = result.ByteDepth;


            int y = 3;
            int i, x;

            switch (bitmap.pixel_mode)
            {
                case ANT_Pixel_Mode.ANT_PIXEL_MODE_MONO:
                    {
                        for (i = 0; i < bitmap_rows; i++, y += resultStride, buf_index += pitch)
                        {
                            bits.Reset(buf_index);

                            for (x = 0; x < bitmap_width; x++)
                            {
                                if (bits.bit())
                                {
                                    resultData[y + x * resultBpp] = cover_full;
                                }

                                bits.Inc();
                            }
                        }
                    }

                    break;


                case ANT_Pixel_Mode.ANT_PIXEL_MODE_GRAY:
                case ANT_Pixel_Mode.ANT_PIXEL_MODE_LCD:
                case ANT_Pixel_Mode.ANT_PIXEL_MODE_LCD_V:
                    {
                        for (i = 0; i < bitmap_rows; i++, y += resultStride, buf_index += pitch)
                        {
                            int p_index = buf_index;

                            for (x = 0; x < bitmap_width; x++, p_index++)
                            {
                                if (buf[p_index] != 0)
                                {
                                    resultData[y + x * resultBpp] = //ras.apply_gamma(buf[p_index]);
                                        buf[p_index];
                                }
                            }
                        }
                    }

                    break;


                case ANT_Pixel_Mode.ANT_PIXEL_MODE_GRAY2:
                    {
                        for (i = 0; i < bitmap_rows; i++, y += resultStride, buf_index += pitch)
                        {
                            int p_index = buf_index;

                            for (x = 0; x < bitmap_width; x++)
                            {
                                byte b = 0;

                                switch (x % 4)
                                {
                                    case 0:
                                        {
                                            b = (byte)((buf[p_index] & 0x3) * 85);
                                            break;
                                        }
                                    case 1:
                                        {
                                            b = (byte)(((buf[p_index] & 0xC) >> 2) * 85);
                                            break;
                                        }
                                    case 2:
                                        {
                                            b = (byte)(((buf[p_index] & 0x30) >> 4) * 85);
                                            break;
                                        }
                                    case 3:
                                        {
                                            b = (byte)(((buf[p_index] & 0xC0) >> 6) * 85);

                                            p_index++;

                                            break;
                                        }
                                }

                                if (b != 0)
                                {
                                    resultData[y + x * resultBpp] = //ras.apply_gamma(buf[p_index]);
                                        b;
                                }
                            }
                        }
                    }

                    break;

                case ANT_Pixel_Mode.ANT_PIXEL_MODE_GRAY4:
                    {
                        for (i = 0; i < bitmap_rows; i++, y += resultStride, buf_index += pitch)
                        {
                            int p_index = buf_index;

                            for (x = 0; x < bitmap_width; x++)
                            {
                                byte b = 0;

                                switch (x % 2)
                                {
                                    case 0:
                                        {
                                            b = (byte)((buf[p_index] & 0x0F) * 17);
                                            break;
                                        }
                                    case 1:
                                        {
                                            b = (byte)(((buf[p_index] & 0xF0) >> 4) * 17);

                                            p_index++;

                                            break;
                                        }
                                }

                                if (b != 0)
                                {
                                    resultData[y + x * resultBpp] = //ras.apply_gamma(buf[p_index]);
                                        b;
                                }
                            }
                        }
                    }

                    break;

                default:
                    {
                        //ANT_Pixel_Mode pm = bitmap.pixel_mode;
                    }

                    break;
            }


            result.UnlockBits(bitmapData);


            return result;
        }
    }
}
