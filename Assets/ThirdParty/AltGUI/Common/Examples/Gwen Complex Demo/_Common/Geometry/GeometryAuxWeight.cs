//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;
using Alt;
using Alt.Sketch;
using Alt.Sketch.Geometries;
using Alt.Sketch.Geometries.Reformers;
using Alt.Sketch.Geometries.Transformers;


namespace Alt.GUI.Demo
{
    class GeometryAuxWeight : GeometryReformer
    {
        Matrix4 m_mtx_zoom_in_y;
        Matrix4 m_mtx_zoom_out_y;
        GeometryTransformer m_trans_zoom_in_y;
        GeometryContour m_contour;
        GeometryTransformer m_trans_zoom_out;


        public GeometryAuxWeight(Geometry geometry)
        {
            m_mtx_zoom_in_y = Matrix4.CreateScaling(1, 100);
            m_mtx_zoom_out_y = Matrix4.CreateScaling(1, 1.0 / 100.0);
            m_trans_zoom_in_y = GeometryMatrix4Transformer.CreateOptimizedTransformer(geometry, m_mtx_zoom_in_y);
            m_contour = new GeometryContour(m_trans_zoom_in_y);
            m_trans_zoom_out = GeometryMatrix4Transformer.CreateOptimizedTransformer(m_contour, m_mtx_zoom_out_y);

            m_contour.AutoDetectOrientation = false;

            Geometry = m_trans_zoom_out;
        }


        public double Weight
        {
            get
            {
                return m_contour.Width;
            }
            set
            {
                m_contour.Width = value;

                Modified();
            }
        }
    }
}
