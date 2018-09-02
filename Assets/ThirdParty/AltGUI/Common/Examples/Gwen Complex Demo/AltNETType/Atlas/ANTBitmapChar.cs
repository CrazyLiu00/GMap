//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.NETType;
using Alt.Sketch;


namespace Alt.GUI.Demo.FontAtlas
{
    // each ANTBitmapChar represents a character from the fnt file
    // reads paramaters from fnt file and creates textured quad   
    class ANTBitmapChar : IDisposable
    {
        int m_x, m_y;
        int m_width;
        int m_height;
        int m_xOffset;
        int m_yOffset;
        int m_xAdvance;
        int m_charCode;
        ANT_Glyph m_pGlyph;


        public ANTBitmapChar()
        {
            m_x = 0;
            m_y = 0;
            m_width = 0;
            m_height = 0;
            m_xOffset = 0;
            m_yOffset = 0;
            m_xAdvance = 0;
            m_pGlyph = null;
        }


        public void Dispose()
        {
        }


        public ANTBitmapChar(int charCode)
        {
            m_charCode = charCode;
        }


        public void Render(Graphics graphics, Bitmap bitmap, int x, int y)
        {
            if (IsEmpty())
            {
                return;
            }

            x += m_xOffset;
            y += m_yOffset;

            RectI srcRect = new RectI(m_x, m_y, m_width, m_height);
            graphics.DrawImage(bitmap, new PointI(x, y), srcRect);
        }


        public void SetXY(int x, int y)
        {
            m_x = x;
            m_y = y;
        }

        public int GetX2()
        {
            return m_x + m_width;
        }

        public int GetY2()
        {
            return m_x + m_width;
        }

        public int GetXAdvance()
        {
            return m_xAdvance;
        }

        public void SetXAdvance(int xAdvance)
        {
            m_xAdvance = xAdvance;
        }

        public int GetHeight()
        {
            return m_height;
        }

        public int GetTotalHeight()
        {
            return m_yOffset + m_height;
        }

        public int GetWidth()
        {
            return m_width;
        }

        public void SetSize(int width, int height)
        {
            m_width = width;
            m_height = height;
        }

        public void SetOffsets(int xOffset, int yOffset)
        {
            m_xOffset = xOffset;
            m_yOffset = yOffset;
        }

        public int GetYOffset()
        {
            return m_yOffset;
        }

        public void ReduceYOffset(int amount)
        {
            m_yOffset -= amount;
        }

        public bool IsLoaded()
        {
            return m_x >= 0;
        }

        public int GetNumPixels()
        {
            return m_width * m_height;
        }

        public int GetCharCode()
        {
            return m_charCode;
        }

        public void SetCharCode(int charCode)
        {
            m_charCode = charCode;
        }


        public void SetGlyph(ANT_Glyph pGlyph)
        {
            m_pGlyph = pGlyph;
        }


        public void DrawToBitmap(byte[] pData, int texWidth, int texHeight)
        {
            if (IsEmpty())
            {
                return;
            }
            
            // Convert The Glyph To A Bitmap.
            ANT_Error error = ANT.ANT_Glyph_To_Bitmap(ref m_pGlyph, ANT_Render_Mode.ANT_RENDER_MODE_NORMAL, null, true);
            if (error == ANT_Error.ANT_Err_Ok)
            {
                ANT_BitmapGlyph bitmap_glyph = (ANT_BitmapGlyph)m_pGlyph;

                // This Reference Will Make Accessing The Bitmap Easier.
                ANT_Bitmap bitmap = bitmap_glyph.bitmap;

                int x, y = 0;
                int index;
                for (y = 0; y < bitmap.rows; y++)
                {
                    for (x = 0; x < bitmap.width; x++)
                    {
                        index = (m_y + y) * texWidth + m_x + x;
                        pData[index] = bitmap.buffer[y * bitmap.width + x];
                    }
                }
            }
        }


        public bool IsEmpty()
        {
            return m_width == 0 || m_height == 0;
        }


        public void ReleaseGlyph()
        {
            if (m_pGlyph != null)
            {
                ANT.ANT_Done_Glyph(ref m_pGlyph);
                m_pGlyph = null;
            }
        }
    }
}
