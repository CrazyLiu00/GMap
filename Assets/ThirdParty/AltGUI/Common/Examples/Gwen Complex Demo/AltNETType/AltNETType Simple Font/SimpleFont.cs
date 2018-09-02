//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;
using Alt;
using Alt.Sketch;


namespace Alt.NETType
{
    sealed class SimpleFont : IDisposable
    {
        public enum GlyphDataType : byte
        {
            Invalid = 0,
            Mono,
            Gray8,
            Outline
        }


        public enum GlyphRenderType : byte
        {
            Mono,
            Gray8,
            Outline
        }



        [ThreadStatic]
        static ANT_Library m_Library = null;
        static ANT_Library Library
        {
            get
            {
                if (m_Library == null)
                {
                    ANT.ANT_Init_AltNETType(out m_Library);
                }

                return m_Library;
            }
        }



        string m_Name;
        public string Name
        {
            get
            {
                return m_Name;
            }
        }


        string m_Signature;
        public string Signature
        {
            get
            {
                return m_Signature;
            }
        }


        UInt32 m_Height = 0;
        public double Height
        {
            get
            {
                return m_Height / 64.0;
            }
            //private
            set
            {
                if (m_Height == (uint)(value * 64.0))
                {
                    return;
                }

                m_Height = (uint)(value * 64.0);

                if (m_CurrentFace != null)
                {
                    UpdateCharSize();
                }
            }
        }


        UInt32 m_Width = 0;
        public double Width
        {
            get
            {
                return m_Width / 64.0;
            }
            //private
            set
            {
                if (m_Width == (uint)(value * 64.0))
                {
                    return;
                }

                m_Width = (uint)(value * 64.0);

                if (m_CurrentFace != null)
                {
                    UpdateCharSize();
                }
            }
        }


        double m_AdvanceX = 0;
        public double AdvanceX
        {
            get
            {
                return m_AdvanceX;
            }
        }

        double m_AdvanceY = 0;
        public double AdvanceY
        {
            get
            {
                return m_AdvanceY;
            }
        }


        ANT_Error m_ANTLastError = ANT_Error.ANT_Err_Ok;
        public ANT_Error LastError
        {
            get
            {
                return m_ANTLastError;
            }
        }


        int m_Resolution = 0;
        public Int32 Resolution
        {
            get
            {
                return m_Resolution;
            }
            set
            {
                if (m_Resolution == value)
                {
                    return;
                }

                m_Resolution = value;

                UpdateCharSize();
            }
        }


        ANT_Encoding m_CharMap = ANT_Encoding.ANT_ENCODING_UNICODE;//FT_ENCODING_NONE;
        public ANT_Encoding CharMap
        {
            get
            {
                return m_CharMap;
            }
            set
            {
                if (m_CharMap == value)
                {
                    return;
                }

                if (m_CurrentFace != null)
                {
                    m_ANTLastError = ANT.ANT_Select_Charmap(m_CurrentFace, m_CharMap);
                    if (m_ANTLastError == ANT_Error.ANT_Err_Ok)
                    {
                        UpdateSignature();
                    }
                }
            }
        }


        bool m_Hinting = true;
        public bool Hinting
        {
            get
            {
                return m_Hinting;
            }
            set
            {
                if (m_Hinting == value)
                {
                    return;
                }

                m_Hinting = value;

                if (m_CurrentFace != null)
                {
                    UpdateSignature();
                }
            }
        }

        bool m_FlipY = false;
        public bool FlipY
        {
            get
            {
                return m_FlipY;
            }
            set
            {
                if (m_FlipY == value)
                {
                    return;
                }

                m_FlipY = value;

                if (m_CurrentFace != null)
                {
                    UpdateSignature();
                }
            }
        }


        int m_ChangeStamp = 0;
        public int ChangeStamp
        {
            get
            {
                return m_ChangeStamp;
            }
        }


        int m_GlyphIndex = 0;
        public int GlyphIndex
        {
            get
            {
                return m_GlyphIndex;
            }
        }


        GlyphDataType m_GlyphDataType = GlyphDataType.Invalid;
        public GlyphDataType DataType
        {
            get
            {
                return m_GlyphDataType;
            }
        }


        internal Rect m_Bounds;
        public Rect Bounds
        {
            get
            {
                return m_Bounds;
            }
        }



        int m_FaceIndex = 0;
        ANT_Face m_CurrentFace = null;  // handle to the current face object
        GlyphRenderType m_GlyphRenderType = GlyphRenderType.Gray8;

        System.Collections.Generic.Dictionary<int, ANT_Face> m_Faces;      // A pool of font faces
        System.Collections.Generic.Dictionary<string, int> m_FaceNames;

        Matrix4 m_Matrix;

        Geometry m_CurrentGeometry;
        Bitmap m_CurrentBitmap;


        public SimpleFont()
        {
            m_Faces = new Dictionary<int, ANT_Face>();
            m_FaceNames = new Dictionary<string, int>();

            m_Bounds = new Rect(0, 0, 1, 1);
            m_Matrix = Matrix4.Identity;
        }


        public void Dispose()
        {
            if (m_Faces != null)
            {
                foreach (ANT_Face face in m_Faces.Values)
                {
                    ANT_Face f = face;
                    ANT.ANT_Done_Face(ref f);
                }

                m_Faces.Clear();
                m_Faces = null;
            }

            if (m_FaceNames != null)
            {
                m_FaceNames.Clear();
                m_FaceNames = null;
            }

            m_Signature = null;
        }


        int FindFace(string face_name)
        {
            int n;
            if (m_FaceNames.TryGetValue(face_name, out n))
            {
                return n;
            }

            return -1;
        }


        public double Ascender
        {
            get
            {
                if (m_CurrentFace != null)
                {
                    return m_CurrentFace.ascender * Height / m_CurrentFace.height;
                }

                return 0;
            }
        }


        public double Descender
        {
            get
            {
                if (m_CurrentFace != null)
                {
                    return m_CurrentFace.descender * Height / m_CurrentFace.height;
                }

                return 0;
            }
        }


        public bool Attach(string file_name)
        {
            if (m_CurrentFace != null)
            {
                m_ANTLastError = ANT.ANT_Attach_File(m_CurrentFace, file_name);
                return m_ANTLastError == ANT_Error.ANT_Err_Ok;
            }

            return false;
        }


        public void Transform(Matrix4 affine)
        {
            if (m_Matrix == affine)
            {
                return;
            }

            m_Matrix = affine;

            if (m_CurrentFace != null)
            {
                UpdateSignature();
            }
        }


        void UpdateSignature()
        {
            if (m_CurrentFace != null &&
                m_Name != null)
            {
                m_Signature = Alt.StringFormatter.sprintf("%s,%u,%d,%d:%dx%d,%d,%d",
                        m_Name,
                        m_CharMap,
                        (int)m_GlyphRenderType,
                        m_Resolution,
                        m_Height,
                        m_Width,
                        m_Hinting ? 1 : 0,
                        m_FlipY ? 1 : 0);

                if (m_GlyphRenderType == GlyphRenderType.Outline)
                {
                    m_Signature += Alt.StringFormatter.sprintf(",%08X%08X%08X%08X%08X%08X",
                        ANT.ANT_double_to_int16p16(m_Matrix.sx),
                        ANT.ANT_double_to_int16p16(m_Matrix.shy),
                        ANT.ANT_double_to_int16p16(m_Matrix.shx),
                        ANT.ANT_double_to_int16p16(m_Matrix.sy),
                        ANT.ANT_double_to_int16p16(m_Matrix.tx),
                        ANT.ANT_double_to_int16p16(m_Matrix.ty));
                }

                m_ChangeStamp++;
            }
        }


        void UpdateCharSize()
        {
            if (m_CurrentFace != null)
            {
                if (m_Resolution != 0)
                {
                    ANT.ANT_Set_Char_Size(m_CurrentFace,
                                     (int)m_Width,       // char_width in 1/64th of points
                                     (int)m_Height,      // char_height in 1/64th of points
                                     m_Resolution,  // horizontal device resolution 
                                     m_Resolution); // vertical device resolution 
                }
                else
                {
                    ANT.ANT_Set_Pixel_Sizes(m_CurrentFace,
                                       (int)(m_Width >> 6),    // pixel_width
                                       (int)(m_Height >> 6));  // pixel_height
                }

                UpdateSignature();
            }
        }


        public bool AddKerning(int first, int second, ref double x, ref double y)
        {
            if (m_CurrentFace != null &&
                first != 0 &&
                second != 0 &&
                ANT.ANT_HAS_KERNING(m_CurrentFace.face_flags))
            {
                ANT_Vector add_kerning_delta = new ANT_Vector();

                ANT.ANT_Get_Kerning(m_CurrentFace, first, second, ANT_Kerning_Mode.ANT_KERNING_DEFAULT, add_kerning_delta);
                double dx = ANT.ANT_int26p6_to_double(add_kerning_delta.x);
                double dy = ANT.ANT_int26p6_to_double(add_kerning_delta.y);
                if (m_GlyphRenderType == GlyphRenderType.Outline)
                {
                    m_Matrix.Transform2x2(ref dx, ref dy);
                }

                x += dx;
                y += dy;

                return true;
            }

            return false;
        }


        public bool LoadFont(string font_name, GlyphRenderType ren_type)
        {
            return LoadFont(font_name, ren_type, null, 0);
        }

        public bool LoadFont(string font_name, GlyphRenderType ren_type, byte[] font_mem, int font_mem_size)
        {
            int face_index = 0;

            bool ret = false;

            ANT_Library library = Library;
            if (library != null)
            {
                m_ANTLastError = ANT_Error.ANT_Err_Ok;

                int idx = FindFace(font_name);
                if (idx >= 0)
                {
                    if (m_CurrentFace == m_Faces[idx] &&
                        m_GlyphRenderType == ren_type)
                    {
                        return true;
                    }

                    m_CurrentFace = m_Faces[idx];
                    m_Name = font_name;
                }
                else
                {
                    ANT_Face face;
                    if (font_mem != null &&
                        font_mem_size != 0)
                    {
                        m_ANTLastError = ANT.ANT_New_Memory_Face(library,
                                                            font_mem,
                                                          font_mem_size,
                                                          face_index,
                                                          out face);
                    }
                    else
                    {
                        m_ANTLastError = ANT.ANT_New_Face(library,
                                                   font_name,
                                                   face_index,
                                                   out face);
                    }

                    if (m_ANTLastError == ANT_Error.ANT_Err_Ok)
                    {
                        m_FaceIndex++;
                        m_Faces.Add(m_FaceIndex, face);

                        m_FaceNames.Add(font_name, m_FaceIndex);

                        m_CurrentFace = face;
                        m_Name = font_name;

                        UpdateCharSize();
                    }
                    else
                    {
                        m_CurrentFace = null;
                        m_Name = null;
                    }
                }


                if (m_ANTLastError == ANT_Error.ANT_Err_Ok)
                {
                    ret = true;

                    switch (ren_type)
                    {
                        case GlyphRenderType.Mono:
                            m_GlyphRenderType = GlyphRenderType.Mono;
                            break;

                        case GlyphRenderType.Gray8:
                            m_GlyphRenderType = GlyphRenderType.Gray8;
                            break;

                        case GlyphRenderType.Outline:
                            if (ANT.ANT_IS_SCALABLE(m_CurrentFace.face_flags))
                            {
                                m_GlyphRenderType = GlyphRenderType.Outline;
                            }
                            else
                            {
                                m_GlyphRenderType = GlyphRenderType.Gray8;
                            }
                            break;
                    }

                    UpdateSignature();
                }
            }

            return ret;
        }


        public bool PrepareGlyph(int glyph_code)
        {
            m_GlyphIndex = (int)ANT.ANT_Get_Char_Index(m_CurrentFace, glyph_code);
            m_ANTLastError = ANT.ANT_Load_Glyph(m_CurrentFace, m_GlyphIndex,
                m_Hinting ? ANT_LOAD.ANT_LOAD_DEFAULT : ANT_LOAD.ANT_LOAD_NO_HINTING);

            if (m_ANTLastError == ANT_Error.ANT_Err_Ok)
            {
                m_AdvanceX = ANT.ANT_int26p6_to_double(m_CurrentFace.glyph.advance.x);
                m_AdvanceY = ANT.ANT_int26p6_to_double(m_CurrentFace.glyph.advance.y);

                switch (m_GlyphRenderType)
                {
                    case GlyphRenderType.Mono:
                    case GlyphRenderType.Gray8:

                        m_ANTLastError = ANT.ANT_Render_Glyph(m_CurrentFace.glyph,
                            m_GlyphRenderType == GlyphRenderType.Mono ?
                            ANT_Render_Mode.ANT_RENDER_MODE_MONO : ANT_Render_Mode.ANT_RENDER_MODE_NORMAL);

                        if (m_ANTLastError == ANT_Error.ANT_Err_Ok)
                        {
                            m_CurrentBitmap = ANTExt.ToBitmap(m_CurrentFace.glyph.bitmap, m_FlipY);
                            if (m_CurrentBitmap != null)
                            {
                                m_Bounds.Left = m_CurrentFace.glyph.bitmap_left;
                                m_Bounds.Top = m_CurrentFace.glyph.bitmap_top;
                                m_Bounds.Right = m_CurrentBitmap.PixelWidth + 1;
                                m_Bounds.Bottom = m_CurrentBitmap.PixelHeight + 1;
                            }
                        }
                        else
                        {
                            m_Bounds = new Rect(0, 0, m_AdvanceX, m_AdvanceY);
                        }

                        m_GlyphDataType = m_GlyphRenderType == GlyphRenderType.Mono ?
                            GlyphDataType.Mono : GlyphDataType.Gray8;

                        return true;

                    case GlyphRenderType.Outline:

                        m_CurrentGeometry = ANTExt.ANT_Outline_ToGeometry(m_CurrentFace.glyph.outline,
                                                        m_FlipY,
                                                        m_Matrix);
                        if (m_CurrentGeometry != null)
                        {
                            m_Bounds = m_CurrentGeometry.Bounds;
                            //m_Bounds.BoundFloorCeiling();

                            m_GlyphDataType = GlyphDataType.Outline;

                            m_Matrix.Transform(ref m_AdvanceX, ref m_AdvanceY);

                            return true;
                        }

                        m_Bounds = Rect.Empty;

                        break;
                }
            }

            return false;
        }


        internal void WriteGlyphTo(SimpleFontCacheManager.GlyphCache data)
        {
            if (data != null)
            {
                switch (m_GlyphDataType)
                {
                    case GlyphDataType.Outline:
                        data.m_Data = m_CurrentGeometry;
                        break;
                    case GlyphDataType.Mono:
                    case GlyphDataType.Gray8:
                        data.m_Data = m_CurrentBitmap;
                        break;
                }
            }
        }
    }
}
