//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;

using Alt.NETType;
using Alt.Sketch;


namespace Alt.GUI.Demo.FontAtlas
{
    class ANTBitmapFont : IDisposable
    {
        int m_lineHeight;
        Dictionary<int, ANTBitmapChar> m_mapBitmapChar = new Dictionary<int, ANTBitmapChar>();
        FontAtlasManager m_pFontAtlas;
        ANT_Face m_pFTFace;


        public ANTBitmapFont(FontAtlasManager pFontAtlas)
        {
            m_pFontAtlas = pFontAtlas;
            m_pFTFace = null;
        }


        public void Dispose()
        {
            ReleaseFace();
        }
        ~ANTBitmapFont()
        {
            ReleaseFace();
        }


        // draws the string at position x, y (given in screen coordinates)
        public int DrawString(Graphics graphics, int x, int y, string text)
        {
            return DrawString(graphics, m_pFontAtlas.Bitmap, x, y, text);
        }

        public int DrawString(Graphics graphics, Bitmap bitmap, int x, int y, string text)
        {
            char c;
            int currX = x;
            int n = 0;
            ANT_Vector kerning = new ANT_Vector();
            int ixGlyph;
            int ixGlyphPrev = 0;
            bool hasKerning = false;
            if (m_pFTFace != null)
            {
                hasKerning = ANT.ANT_HAS_KERNING(m_pFTFace);
            }

            int len = text.Length;
            while (n < len)
            {
                c = text[n];

                ANTBitmapChar iter;
                if (m_mapBitmapChar.TryGetValue(c, out iter))
                {
                    if (hasKerning)
                    {
                        // get kerning
                        ixGlyph = ANT.ANT_Get_Char_Index(m_pFTFace, c);
                        if (hasKerning &&
                            ixGlyphPrev != 0 &&
                            ixGlyph != 0)
                        {
                            ANT.ANT_Get_Kerning(m_pFTFace, ixGlyphPrev, ixGlyph, ANT_Kerning_Mode.ANT_KERNING_DEFAULT, kerning);
                            currX += kerning.x >> 6;
                        }
                        ixGlyphPrev = ixGlyph;
                    }

                    iter.Render(graphics, bitmap, currX, y);
                    currX += iter.GetXAdvance();
                }
                n++;
            }
            return currX;
        }


        public int GetWidth(string text)
        {
            int currX = 0;
            int n = 0;
            char c;
            ANT_Vector kerning = new ANT_Vector();
            int ixGlyph;
            int ixGlyphPrev = 0;
            bool hasKerning = false;

            if (m_pFTFace != null)
            {
                hasKerning = ANT.ANT_HAS_KERNING(m_pFTFace);
            }

            while (text[n] != 0)
            {
                c = text[n];

                ANTBitmapChar iter;
                if (m_mapBitmapChar.TryGetValue(c, out iter))
                {
                    if (hasKerning)
                    {
                        // get kerning
                        ixGlyph = ANT.ANT_Get_Char_Index(m_pFTFace, c);
                        if (hasKerning &&
                            ixGlyphPrev != 0 &&
                            ixGlyph != 0)
                        {
                            ANT.ANT_Get_Kerning(m_pFTFace, ixGlyphPrev, ixGlyph, ANT_Kerning_Mode.ANT_KERNING_DEFAULT, kerning);
                            currX += kerning.x >> 6;
                        }
                        ixGlyphPrev = ixGlyph;
                    }
                    currX += iter.GetXAdvance();
                }
                n++;
            }

            return currX;
        }


        public int LineHeight
        {
            get
            {
                return m_lineHeight;
            }
            set
            {
                m_lineHeight = value;
            }
        }


        public void AddChar(int charCode, ANTBitmapChar pFTBitmapChar)
        {
            m_mapBitmapChar[charCode] = pFTBitmapChar;
        }
        public void SetFTFace(ANT_Face pFTFace)
        {
            m_pFTFace = pFTFace;
        }


        public void FinishCreating()
        {
            bool hasKerning = false;
            if (m_pFTFace != null)
            {
                hasKerning = ANT.ANT_HAS_KERNING(m_pFTFace);
            }
            if (!hasKerning || !m_pFontAtlas.UseKerning)
                ReleaseFace();
        }


        public ANTBitmapChar GetChar(int charCode)
        {
            ANTBitmapChar iter;
            if (m_mapBitmapChar.TryGetValue(charCode, out iter))
            {
                return iter;
            }

            return null;
        }


        public void ReleaseFace()
        {
            if (m_pFTFace != null)
            {
                ANT.ANT_Done_Face(ref m_pFTFace);
                m_pFTFace = null;
            }
        }
    }
}
