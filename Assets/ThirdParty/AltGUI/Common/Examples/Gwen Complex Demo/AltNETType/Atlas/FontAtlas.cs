//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;

using Alt.NETType;
using Alt.Sketch;


namespace Alt.GUI.Demo.FontAtlas
{
    class FontAtlasManager : IDisposable
    {
        ANT_Library m_Library;
        List<ANTBitmapChar> m_BitmapChars = new List<ANTBitmapChar>();
        byte[] m_Texture;
        Bitmap m_Bitmap;
        List<ANTBitmapFont> m_BitmapFonts = new List<ANTBitmapFont>();
        ANTBitmapChar m_pShowAtlas;
        bool m_UseKerning;


        public FontAtlasManager()
        {
            m_UseKerning = false;
            m_Library = null;
        }

        public void Dispose()
        {
            ReleaseFontAtlas();
        }

        ~FontAtlasManager()
        {
            ReleaseFontAtlas();
        }


        void ReleaseFontAtlas()
        {
            if (m_BitmapChars != null)
            {
                foreach (ANTBitmapChar ch in m_BitmapChars)
                {
                    ch.Dispose();
                }

                m_BitmapChars.Clear();
                m_BitmapChars = null;

                foreach (ANTBitmapFont font in m_BitmapFonts)
                {
                    font.Dispose();
                }
                m_BitmapFonts.Clear();
                m_BitmapFonts = null;

                if (m_pShowAtlas != null)
                {
                    m_pShowAtlas.Dispose();
                    m_pShowAtlas = null;
                }

                ReleaseTexture();
                ReleaseLibrary();
            }
        }



        public void AddFont(string strFileName, int size, string szLetters)
        {
            if (string.IsNullOrEmpty(strFileName) ||
                !Alt.IO.VirtualFile.Exists(strFileName))
            {
                return;
            }


            if (m_Library == null)
            {
                if (ANT.ANT_Init_AltNETType(out m_Library) != ANT_Error.ANT_Err_Ok)
                {
                    throw new Exception("ANT_Init_FreeType failed");
                }
            }

            // The Object In Which FreeType Holds Information On A Given
            // Font Is Called A "face".
            ANT_Face face;

            string file = strFileName;
            // This Is Where We Load In The Font Information From The File.
            if (ANT.ANT_New_Face(m_Library, file, 0, out face) != ANT_Error.ANT_Err_Ok)
            {
                throw new Exception("no font: " + file);
            }

            // FreeType Measures Font Size In Terms Of 1/64ths Of Pixels.  
            ANT.ANT_Set_Char_Size(face, size * 64, size * 64, 72, 72);

            int len = szLetters.Length;
            int n;
            ANTBitmapChar pFTBitmapChar;
            ANT_Glyph pGlyph;
            ANTBitmapFont pANTBitmapFont = new ANTBitmapFont(this);
            pANTBitmapFont.LineHeight = face.size.metrics.height >> 6;
            pANTBitmapFont.SetFTFace(face);
            m_BitmapFonts.Add(pANTBitmapFont);
            char c;
            int height;
            int yOffset;
            int ixGlyph;
            for (n = 0; n < len; n++)
            {
                c = szLetters[n];
                // check that the character hasn't already been processed
                if (pANTBitmapFont.GetChar(c) == null)
                {
                    // Load The Glyph For Our Character.
                    ixGlyph = ANT.ANT_Get_Char_Index(face, c);
                    if (ixGlyph == 0)
                    {
                        Console.WriteLine("character doesn't exist in font: '" + c + "'");
                    }
                    else
                    {
                        if (ANT.ANT_Load_Glyph(face, ixGlyph, ANT_LOAD.ANT_LOAD_DEFAULT) != ANT_Error.ANT_Err_Ok)
                        {
                            throw new Exception("ANT_Load_Glyph failed");
                        }

                        // Move The Face's Glyph Into A Glyph Object.
                        if (ANT.ANT_Get_Glyph(face.glyph, out pGlyph) != ANT_Error.ANT_Err_Ok)
                        {
                            throw new Exception("ANT_Get_Glyph failed");
                        }

                        pFTBitmapChar = new ANTBitmapChar(c);
                        // all metrics dimensions are multiplied by 64, so we have to divide by 64
                        height = face.glyph.metrics.height >> 6;
                        yOffset = pANTBitmapFont.LineHeight - (face.glyph.metrics.horiBearingY >> 6);
                        pFTBitmapChar.SetOffsets(face.glyph.metrics.horiBearingX >> 6, yOffset);
                        pFTBitmapChar.SetSize(face.glyph.metrics.width >> 6, height);
                        pFTBitmapChar.SetXAdvance(face.glyph.metrics.horiAdvance >> 6);
                        pFTBitmapChar.SetGlyph(pGlyph);
                        m_BitmapChars.Add(pFTBitmapChar);
                        pANTBitmapFont.AddChar(c, pFTBitmapChar);
                    }
                }
            }
        }


        public void CreateAtlas()
        {
            int totalPixels = 0;
            foreach (ANTBitmapChar ch in m_BitmapChars)
            {
                totalPixels += ch.GetNumPixels();
            }

            int ixSize = 0;
            int texWidth = 32;
            int texHeight = 32;
            while (true)
            {
                if (totalPixels <= texWidth * texHeight)
                {
                    break;
                }
                GetNextTextureSize(ref texWidth, ref texHeight, ixSize);
                ixSize++;
            }

            m_BitmapChars.Sort(new GreaterSizeComparer());

            while (!BinPack(texWidth, texHeight))
            {
                GetNextTextureSize(ref texWidth, ref texHeight, ixSize);
                ixSize++;
            }

            byte[] pData = new byte[texWidth * texHeight];
            Array.Clear(pData, 0, pData.Length);

            foreach (ANTBitmapChar ch in m_BitmapChars)
            {
                ch.DrawToBitmap(pData, texWidth, texHeight);
                ch.ReleaseGlyph();
            }

            foreach (ANTBitmapFont font in m_BitmapFonts)
            {
                font.FinishCreating();
            }
            if (!m_UseKerning)
                ReleaseLibrary();

            m_Texture = pData;
            m_Bitmap = ToBitmap(m_Texture, texWidth, texHeight);

            m_pShowAtlas = new ANTBitmapChar();
            m_pShowAtlas.SetXY(0, 0);
            m_pShowAtlas.SetSize(texWidth, texHeight);
        }


        public ANTBitmapFont GetFont(int index)
        {
            if (index < 0)
            {
                index = 0;
            }

            if (index >= m_BitmapFonts.Count)
            {
                index = m_BitmapFonts.Count - 1;
            }

            if (index < 0)
            {
                return null;
            }

            return m_BitmapFonts[index];
        }


        public Bitmap Bitmap
        {
            get
            {
                return m_Bitmap;
            }
        }


        Bitmap ToBitmap(byte[] src, int w, int h)
        {
            Bitmap bitmap = new Bitmap(w, h);
            BitmapData data = bitmap.LockBits(ImageLockMode.WriteOnly);
            byte[] scan0 = data.Scan0;
            int stride = data.Stride;

            for (int y = 0; y < h; y++)
            {
                int destIndex = y * stride + 3;
                int srcIndex = y * w;
                for (int x = 0; x < w; x++, destIndex += 4, srcIndex++)
                {
                    scan0[destIndex] = src[srcIndex];
                }
            }

            bitmap.UnlockBits(data);

            return bitmap;
        }


        public bool UseKerning
        {
            get
            {
                return m_UseKerning;
            }
            set
            {
                m_UseKerning = value;
            }
        }


        public bool BinPack(int texWidth, int texHeight)
        {
            TreeNode pTreeNode = new TreeNode();
            pTreeNode.Set(0, 0, texWidth, texHeight);

            foreach (ANTBitmapChar ch in m_BitmapChars)
            {
                if (!pTreeNode.Add(ch))
                {
                    return false;
                }
            }

            return true;
        }


        public void ReleaseTexture()
        {
            if (m_Bitmap != null)
            {
                m_Bitmap.Dispose();
                m_Bitmap = null;
                m_Texture = null;
            }
        }


        public void ReleaseLibrary()
        {
            if (m_Library != null)
            {
                ANT.ANT_Done_AltNETType(ref m_Library);
                m_Library = null;
            }
        }


        // gets next biggest texture size in follwing sequence: 32x32, 64x32, 64x64, 128x64, etc.
        public void GetNextTextureSize(ref int texWidth, ref int texHeight, int ixSize)
        {
            if ((ixSize % 2) != 0)
            {
                texHeight *= 2;
            }
            else
            {
                texWidth *= 2;
            }

            if (texWidth > 1024 || texHeight > 1024)
            {
                throw new Exception("to many images to fit in one texture");
            }
        }


        public class GreaterSizeComparer : IComparer<ANTBitmapChar>
        {
            public int Compare(ANTBitmapChar x, ANTBitmapChar y)
            {
                if (x == null)
                {
                    if (y == null)
                    {
                        return 0;
                    }

                    return 1;
                }

                if (y == null)
                {
                    return -1;
                }

                return y.GetNumPixels() - x.GetNumPixels();
            }
        }
    }
}
