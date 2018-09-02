//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using Alt.NETType;


namespace Alt.GUI.Demo.FontAtlas
{
    class TreeNode
    {
        int m_x;
        int m_y;
        int m_width;
        int m_height;
        TreeNode m_Leaf1;
        TreeNode m_Leaf2;


        public TreeNode()
        {
            m_Leaf1 = null;
            m_Leaf2 = null;
        }

        public TreeNode(int x, int y, int width, int height)
        {
            m_x = x;
            m_y = y;
            m_width = width;
            m_height = height;
            m_Leaf1 = null;
            m_Leaf2 = null;
        }

        public void Set(int x, int y, int width, int height)
        {
            m_x = x;
            m_y = y;
            m_width = width;
            m_height = height;
            m_Leaf1 = null;
            m_Leaf2 = null;
        }

        public bool Add(ANTBitmapChar pFTBitmapChar)
        {
            if (pFTBitmapChar.IsEmpty()) return true;
            if (IsEmpty())
            {
                if (Fits(pFTBitmapChar))
                {
                    CreateBranches(pFTBitmapChar);
                    pFTBitmapChar.SetXY(m_x, m_y);
                    return true;
                }
                return false;
            }
            if (m_Leaf1.Add(pFTBitmapChar))
            {
                return true;
            }
            if (m_Leaf2.Add(pFTBitmapChar))
            {
                return true;
            }
            return false;
        }

        bool Fits(ANTBitmapChar pFTBitmapChar)
        {
            return pFTBitmapChar.GetWidth() <= m_width && pFTBitmapChar.GetHeight() <= m_height;
        }


        public bool IsEmpty()
        {
            return m_Leaf1 == null && m_Leaf2 == null;
        }


        void CreateBranches(ANTBitmapChar pFTBitmapChar)
        {
            int dx = m_width - pFTBitmapChar.GetWidth();
            int dy = m_height - pFTBitmapChar.GetHeight();
            // we split to give one very small leaf and one very big one
            // because it allows more efficent use of space
            // if you don't do this, the bottom right corner never gets used
            if (dx < dy)
            {
                //	split so the top is cut in half and the rest is one big rect below
                m_Leaf1 = new TreeNode();
                m_Leaf1.Set(m_x + pFTBitmapChar.GetWidth(), m_y,
                        m_width - pFTBitmapChar.GetWidth(),
                        pFTBitmapChar.GetHeight());
                m_Leaf2 = new TreeNode();
                m_Leaf2.Set(m_x, m_y + pFTBitmapChar.GetHeight(),
                        m_width, m_height - pFTBitmapChar.GetHeight());
            }
            else
            {
                //	m_Leaf1 = left (cut in half)
                m_Leaf1 = new TreeNode();
                m_Leaf1.Set(m_x, m_y + pFTBitmapChar.GetHeight(),
                        pFTBitmapChar.GetWidth(), m_height - pFTBitmapChar.GetHeight());
                // m_Leaf2 = right (not cut)
                m_Leaf2 = new TreeNode();
                m_Leaf2.Set(m_x + pFTBitmapChar.GetWidth(), m_y,
                        m_width - pFTBitmapChar.GetWidth(), m_height);
            }
        }
    }
}
