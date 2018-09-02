//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.Sketch;
using Alt.Sketch.Geometries;
using Alt.Sketch.Geometries.Reformers;
using Alt.Sketch.Geometries.Transformers;


namespace Alt.GUI.Demo
{
    static class Common
    {
        public const string Example_AltNETType_FontFileName = "AltData/Fonts/Open-Sans/OpenSans-Regular.ttf";


        public const string Pangram = "A Quick Brown Fox Jumps Over The Lazy Dog 0123456789";


        public const string AltSketch_Vector_Graphics_Library = "AltSketch Vector Graphics Library";


        public const string ShortText =
            "AltSketch is a pure C# Vector Graphics Library for Games," +
            " Scientific Applications & other uses of Computer Graphics. " +
            "AltSketch doesn't depend on any graphic API or technology";



        //  AltSketch Outline GraphicsPath

        static GraphicsPath m_GraphicsPath_AltSketch;
        static Rect m_GraphicsPath_AltSketch_Bounds;
        static void Init_GraphicsPath_AltSketch()
        {
            if (m_GraphicsPath_AltSketch == null)
            {
                Font font = new Font("Times New Roman", 128, FontStyle.Regular);

                m_GraphicsPath_AltSketch = new GraphicsPath();
                m_GraphicsPath_AltSketch.AddString("AltSketch", font.FontFamily, FontStyle.Regular, font.Size, Point.Zero, StringFormat.GenericTypographic);
                m_GraphicsPath_AltSketch_Bounds = m_GraphicsPath_AltSketch.Bounds;
            }
        }

        public static GraphicsPath GraphicsPath_AltSketch
        {
            get
            {
                Init_GraphicsPath_AltSketch();

                return m_GraphicsPath_AltSketch;
            }
        }

        public static Rect GraphicsPath_AltSketch_Bounds
        {
            get
            {
                Init_GraphicsPath_AltSketch();

                return m_GraphicsPath_AltSketch_Bounds;
            }
        }
    }
}
