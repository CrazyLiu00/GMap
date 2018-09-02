//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.Sketch;


namespace Alt.GUI.Demo
{
    public class Geometry_Spiral : CacheableGeometry
    {
        double m_x;
        double m_y;
        double m_r1;
        double m_r2;
        //NoNeed	double m_step;
        double m_start_angle;

        double m_angle;
        double m_curr_r;
        double m_da;
        double m_dr;
        bool m_start;


        public Geometry_Spiral(double x, double y, double r1, double r2, double step)
            : this(x, y, r1, r2, step, 0)
        {
        }


        public Geometry_Spiral(double x, double y, double r1, double r2, double step, double start_angle)
            : this(x, y, r1, r2, step, start_angle, MathHelper.DegreesToRadians(4.0), step / 90.0)
        {
        }


        public Geometry_Spiral(double x, double y, double r1, double r2, double step, double start_angle, double da, double dr)
        {
            m_x = x;
            m_y = y;
            m_r1 = r1;
            m_r2 = r2;
			//NoNeed	m_step = step;
            m_start_angle = start_angle;
            m_angle = start_angle;
            m_da = da;
            m_dr = dr;
        }


        public override void Rewind(int path_id)
        {
            m_angle = m_start_angle;
            m_curr_r = m_r1;
            m_start = true;
        }


        public override GeometryVertexCommandAndFlags GetNextVertex(out double x, out double y)
        {
            if (m_curr_r > m_r2)
            {
                x = 0;
                y = 0;

                return GeometryVertexCommandAndFlags.CommandStop;
            }

            x = m_x + System.Math.Cos(m_angle) * m_curr_r;
            y = m_y + System.Math.Sin(m_angle) * m_curr_r;
            m_curr_r += m_dr;
            m_angle += m_da;
            
            if (m_start)
            {
                m_start = false;

                return GeometryVertexCommandAndFlags.CommandMoveTo;
            }

            return GeometryVertexCommandAndFlags.CommandLineTo;
        }
    }
}
