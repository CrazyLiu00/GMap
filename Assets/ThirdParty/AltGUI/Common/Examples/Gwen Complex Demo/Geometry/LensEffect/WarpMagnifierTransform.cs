//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using Alt.Sketch;


namespace Alt.GUI.Demo
{
    class WarpMagnifierTransform : ITransform
    {
        double m_CenterX;
        double m_CenterY;
        double m_Magnification;
        double m_Radius;
        double m_Radius2;
        double m_Radius2Magnification2;
        double m_RadiusMulMagnificationMinus1;


        WarpMagnifierTransform(WarpMagnifierTransform from)
        {
            m_CenterX = from.m_CenterX;
            m_CenterY = from.m_CenterY;
            m_Magnification = from.m_Magnification;
            m_Radius = from.m_Radius;
            m_Radius2 = from.m_Radius2;
            m_Radius2Magnification2 = from.m_Radius2Magnification2;
            m_RadiusMulMagnificationMinus1 = from.m_RadiusMulMagnificationMinus1;
        }


        public WarpMagnifierTransform()
        {
            m_CenterX = 0.0;
            m_CenterY = 0.0;
            m_Magnification = 1.0;
            m_Radius = 1.0;
            m_Radius2 = m_Radius * m_Radius;
            m_Radius2Magnification2 = m_Radius2 * m_Magnification * m_Magnification;
            m_RadiusMulMagnificationMinus1 = m_Radius * (m_Magnification - 1.0);
        }


        public Point Center
        {
            get
            {
                return new Point(m_CenterX, m_CenterY);
            }
            set
            {
                m_CenterX = value.X;
                m_CenterY = value.Y;
            }
        }

        public double Magnification
        {
            get
            {
                return m_Magnification;
            }
            set
            {
                m_Magnification = value;
                m_Radius2Magnification2 = m_Radius2 * m_Magnification * m_Magnification;
                m_RadiusMulMagnificationMinus1 = m_Radius * (m_Magnification - 1.0);
            }
        }

        public double Radius
        {
            get
            {
                return m_Radius;
            }
            set
            {
                m_Radius = value;
                m_Radius2 = m_Radius * m_Radius;
                m_Radius2Magnification2 = m_Radius2 * m_Magnification * m_Magnification;
                m_RadiusMulMagnificationMinus1 = m_Radius * (m_Magnification - 1.0);
            }
        }


        public void Transform(ref double x, ref double y)
        {
            double dx = x - m_CenterX;
            double dy = y - m_CenterY;
            double r2 = dx * dx + dy * dy;
            if (r2 < m_Radius2)
            {
                x = m_CenterX + dx * m_Magnification;
                y = m_CenterY + dy * m_Magnification;
                return;
            }

            double r = System.Math.Sqrt(r2);

            double m = (r + m_RadiusMulMagnificationMinus1) / r;
            x = m_CenterX + dx * m;
            y = m_CenterY + dy * m;
        }


        public void InverseTransform(ref double x, ref double y)
        {
            double dx = x - m_CenterX;
            double dy = y - m_CenterY;
            double r2 = dx * dx + dy * dy;

            if (r2 < m_Radius2Magnification2)
            {
                x = m_CenterX + dx / m_Magnification;
                y = m_CenterY + dy / m_Magnification;
            }
            else
            {
                double r = System.Math.Sqrt(r2);

                double rnew = r - m_RadiusMulMagnificationMinus1;
                x = m_CenterX + dx * rnew / r;
                y = m_CenterY + dy * rnew / r;
            }
        }


        public object Clone()
        {
            return new WarpMagnifierTransform(this);
        }
    }
}
