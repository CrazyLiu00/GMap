//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.Sketch;


namespace Alt.GUI.Demo
{
    class SinTransform : ITransform
    {
        double m_SinXScale;
        double m_SinXScaleDiv2PI;
        double m_SinYScale;
        double m_Shift;
        double m_TranslationX;
        double m_TranslationY;


        public SinTransform()
        {
            m_SinXScale = 1.0;
            m_SinXScaleDiv2PI = m_SinXScale / (Math.PI * 2);
            m_SinYScale = 1.0;
            m_Shift = 0.0;
            m_TranslationX = 0.0;
            m_TranslationY = 0.0;
        }


        public void SetSinScale(double sx, double sy)
        {
            m_SinXScale = sx;
            m_SinXScaleDiv2PI = m_SinXScale / (Math.PI * 2);
            m_SinYScale = sy;
        }


        public double Shift
        {
            set
            {
                m_Shift = value;
            }
        }


        public void SetTranslation(double dx, double dy)
        {
            m_TranslationX = dx;
            m_TranslationY = dy;
        }


        public void Transform(ref double x, ref double y)
        {
            double x1 = x + m_Shift;
            x = x + m_TranslationX;
            y = y + System.Math.Sin(x1 / m_SinXScaleDiv2PI) * m_SinYScale + m_TranslationY;
        }
    }
}
