//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;
using Alt;
using Alt.Sketch;


namespace Alt.NETType
{
    sealed class SimpleFontCacheManager
    {
        public class GlyphCache
        {
            public int glyph_index;
            public object m_Data;
            public SimpleFont.GlyphDataType data_type;
            public Rect bounds;
            public double advance_x;
            public double advance_y;
        }



        class FontCache
        {
            Dictionary<int, GlyphCache> m_glyphs = new Dictionary<int, GlyphCache>();
            

            public FontCache()
            {
            }


            public GlyphCache FindGlyph(int glyph_code)
            {
                GlyphCache glyph;
                if (m_glyphs.TryGetValue(glyph_code, out glyph))
                {
                    return glyph;
                }

                return null;
            }


            public GlyphCache CacheGlyph(int glyph_code,
                                     int glyph_index,
                                     SimpleFont.GlyphDataType data_type,
                                     Rect bounds,
                                     double advance_x,
                                     double advance_y)
            {
                GlyphCache glyph;
                if (m_glyphs.TryGetValue(glyph_code, out glyph))
                {
                    // Already exists, do not overwrite
                    return null;
                }

                glyph = new GlyphCache();

                glyph.glyph_index = glyph_index;
                glyph.data_type = data_type;
                glyph.bounds = bounds;
                glyph.advance_x = advance_x;
                glyph.advance_y = advance_y;

                m_glyphs.Add(glyph_code, glyph);

                return glyph;
            }
        }



        class FontCachePool
        {
            Stack<string> m_FontsStack = new Stack<string>();
            Dictionary<string, FontCache> m_Fonts = new Dictionary<string, FontCache>();
            FontCache m_CurrentFont;


            public FontCachePool()
            {
                m_CurrentFont = null;
            }


            public void SetCurrentFont(string font_signature)
            {
                FontCache font = FindFont(font_signature);

                if (font == null)
                {
                    ClearFontsCacheIfNeed();

                    font = new FontCache();
                    m_Fonts.Add(font_signature, font);

                    m_FontsStack.Push(font_signature);
                }

                m_CurrentFont = font;
            }


            //  prevent memory growing
            void ClearFontsCacheIfNeed()
            {
                while (m_FontsStack.Count > 100)
                {
                    string font_signature = m_FontsStack.Pop();
                    m_Fonts.Remove(font_signature);
                }
            }


            public FontCache font()
            {
                return m_CurrentFont;
            }


            public GlyphCache FindGlyph(int glyph_code)
            {
                if (m_CurrentFont != null)
                {
                    return m_CurrentFont.FindGlyph(glyph_code);
                }

                return null;
            }


            public GlyphCache CacheGlyph(int glyph_code,
                                     int glyph_index,
                                     SimpleFont.GlyphDataType data_type,
                                     Rect bounds,
                                     double advance_x,
                                     double advance_y)
            {
                if (m_CurrentFont != null)
                {
                    return m_CurrentFont.CacheGlyph(glyph_code,
                                                   glyph_index,
                                                   data_type,
                                                   bounds,
                                                   advance_x,
                                                   advance_y);
                }

                return null;
            }


            public FontCache FindFont(string font_signature)
            {
                FontCache font;
                if (m_Fonts.TryGetValue(font_signature, out font))
                {
                    return font;
                }

                return null;
            }
        }



        FontCachePool m_Fonts;
        int m_ChangeStamp;
        GlyphCache m_PrevGlyph;
        GlyphCache m_LastGlyph;

        SimpleFont m_SimpleFont;


        public SimpleFontCacheManager()
        {
            m_Fonts = new FontCachePool();
            m_ChangeStamp = -1;

            m_SimpleFont = new SimpleFont();
        }


        public GlyphCache GetGlyph(int glyph_code)
        {
            Synchronize();

            GlyphCache gl = m_Fonts.FindGlyph(glyph_code);

            if (gl != null)
            {
                m_PrevGlyph = m_LastGlyph;

                return m_LastGlyph = gl;
            }
            else
            {
                if (m_SimpleFont.PrepareGlyph(glyph_code))
                {
                    m_PrevGlyph = m_LastGlyph;
                    m_LastGlyph = m_Fonts.CacheGlyph(glyph_code,
                                                       m_SimpleFont.GlyphIndex,
                                                       m_SimpleFont.DataType,
                                                       m_SimpleFont.Bounds,
                                                       m_SimpleFont.AdvanceX,
                                                       m_SimpleFont.AdvanceY);
                    m_SimpleFont.WriteGlyphTo(m_LastGlyph);//.data);

                    return m_LastGlyph;
                }
            }

            return null;
        }


        public bool AddKerning(ref double x, ref double y)
        {
            if (m_PrevGlyph != null &&
                m_LastGlyph != null)
            {
                return m_SimpleFont.AddKerning(m_PrevGlyph.glyph_index,
                                            m_LastGlyph.glyph_index,
                                            ref x, ref y);
            }
            return false;
        }


        void Synchronize()
        {
            if (m_ChangeStamp != m_SimpleFont.ChangeStamp)
            {
                m_Fonts.SetCurrentFont(m_SimpleFont.Signature);
                m_ChangeStamp = m_SimpleFont.ChangeStamp;

                m_PrevGlyph = m_LastGlyph = null;
            }
        }


        public double Height
        {
            get
            {
                return m_SimpleFont.Height;
            }
            set
            {
                m_SimpleFont.Height = value;
            }
        }


        public double Width
        {
            get
            {
                return m_SimpleFont.Width;
            }
            set
            {
                m_SimpleFont.Width = value;
            }
        }


        public bool Hinting
        {
            get
            {
                return m_SimpleFont.Hinting;
            }
            set
            {
                m_SimpleFont.Hinting = value;
            }
        }


        public bool FlipY
        {
            get
            {
                return m_SimpleFont.FlipY;
            }
            set
            {
                m_SimpleFont.FlipY = value;
            }
        }


        public bool LoadFont(string font_name, SimpleFont.GlyphRenderType ren_type)
        {
            return m_SimpleFont.LoadFont(font_name, ren_type);
        }


        public void Transform(Matrix4 affine)
        {
            m_SimpleFont.Transform(affine);
        }
    }
}
