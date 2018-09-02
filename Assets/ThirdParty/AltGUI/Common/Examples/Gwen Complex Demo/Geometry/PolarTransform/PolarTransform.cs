//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.Sketch;


namespace Alt.GUI.Demo
{
    class PolarTransform : ITransform
    {
        double m_BaseAngle;
        double m_BaseScale;
        double m_BaseX;
        double m_BaseY;
        double m_TranslationX;
        double m_TranslationY;
        double m_Spiral;


        public PolarTransform()
        {
            m_BaseAngle = 1.0;
            m_BaseScale = 1.0;
            m_BaseX = 0.0;
            m_BaseY = 0.0;
            m_TranslationX = 0.0;
            m_TranslationY = 0.0;
        }


        public double BaseScale
        {
            set
            {
                m_BaseScale = value;
            }
        }

        
        public double FullCircle
        {
            set
            {
                m_BaseAngle = 2.0 * System.Math.PI / value;
            }
        }


        public void SetBaseOffset(double dx, double dy)
        {
            m_BaseX = dx;
            m_BaseY = dy;
        }


        public void SetTranslation(double dx, double dy)
        {
            m_TranslationX = dx;
            m_TranslationY = dy;
        }


        public double Spiral
        {
            set
            {
                m_Spiral = value;
            }
        }


        public void Transform(ref double x, ref double y)
        {
            double x1 = (x + m_BaseX) * m_BaseAngle;
            double y1 = (y + m_BaseY) * m_BaseScale + (x * m_Spiral);
            x = System.Math.Cos(x1) * y1 + m_TranslationX;
            y = System.Math.Sin(x1) * y1 + m_TranslationY;
        }
    }
}
