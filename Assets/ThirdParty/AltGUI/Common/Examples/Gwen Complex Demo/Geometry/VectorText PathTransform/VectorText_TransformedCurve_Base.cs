//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;

using Alt.GUI.InteractiveGeometry;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.NETType;
using Alt.Sketch;
using Alt.Sketch.Geometries;
using Alt.Sketch.Geometries.Reformers;
using Alt.Sketch.Geometries.Transformers;


namespace Alt.GUI.Demo
{
    class VectorText_TransformedCurve_Base : InteractiveGeometryContainer
    {
        public VectorText_TransformedCurve_Base(Base parent)
            : base(parent)
        {
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        

        string CurveTransformedText
        {
            get
            {
                return Common.ShortText;
            }
        }

        protected const int CurveTransformedText_FontSize = 36;
        Font m_CurveTransformedText_Font;
        Font CurveTransformedText_Font
        {
            get
            {
                if (m_CurveTransformedText_Font == null)
                {
                    m_CurveTransformedText_Font = new Font("Times New Roman", CurveTransformedText_FontSize, FontStyle.Regular);
                }

                return m_CurveTransformedText_Font;
            }
        }

        protected int FontAscentInPixels
        {
            get
            {
                return CurveTransformedText_Font.AscentInPixels;
            }
        }

        Dictionary<string, Tuple<Geometry, double>> m_CurveTransformedTextGeometryCache = new Dictionary<string, Tuple<Geometry, double>>();
        protected bool TextUpperBaseLine = false;
        Tuple<Geometry, double> CreateCurveTransformedTextGeometry(string text)
        {
            double baseOffset;

            Font font = CurveTransformedText_Font;

            GraphicsPath textPath = new GraphicsPath();
            textPath.AddString(text, font.FontFamily, FontStyle.Regular, font.Size * 96 / 72, Point.Zero, StringFormat.GenericTypographic);

            if (TextUpperBaseLine)
            {
                baseOffset = 0;
                textPath.Transform(Matrix4.CreateTranslation(0, -FontAscentInPixels - 3));
            }
            else
            {
                baseOffset = -textPath.Bounds.Y + 2;
                textPath.Transform(Matrix4.CreateTranslation(0, baseOffset));
            }

            FlattenCurveGeometry curvedTextPath = new FlattenCurveGeometry(textPath);
            if (TextUpperBaseLine)
            {
                curvedTextPath.ApproximationScale = 2.0;
            }
            else
            {
                curvedTextPath.ApproximationScale = 5.0;
            }

            GeometrySegmentator segmentedCurvedTextPath = new GeometrySegmentator(curvedTextPath);
            //segmentedCurvedTextPath.ApproximationScale = 3.0;

            return new Tuple<Geometry, double>(segmentedCurvedTextPath, baseOffset);
        }

        Tuple<Geometry, double> GetOrCreateCurveTransformedTextGeometry(string text)
        {
            Tuple<Geometry, double> geometry;
            if (m_CurveTransformedTextGeometryCache.TryGetValue(text, out geometry))
            {
                return geometry;
            }

            geometry = CreateCurveTransformedTextGeometry(text);
            m_CurveTransformedTextGeometryCache.Add(text, geometry);

            return geometry;
        }

        Rect[] m_CurveTransformedTextCharacterRanges;
        Rect[] CurveTransformedTextCharacterRanges
        {
            get
            {
                if (m_CurveTransformedTextCharacterRanges == null)
                {
                    using (Graphics graphics = Graphics.CreateDefaultGraphics())
                    {
                        string text = CurveTransformedText;
                        int len = text.Length;
                        CharacterRange[] ranges = new CharacterRange[len];
                        for (int i = 0; i < len; i++)
                        {
                            ranges[i] = new CharacterRange(i, 1);
                        }
                        StringFormat sf = new StringFormat();
                        sf.SetMeasurableCharacterRanges(ranges);

                        m_CurveTransformedTextCharacterRanges =
                            graphics.MeasureCharacterRanges(text,
                                CurveTransformedText_Font,
                                new Rect(Point.Zero, graphics.MeasureString(text, CurveTransformedText_Font)),
                                sf);
                    }
                }

                return m_CurveTransformedTextCharacterRanges;
            }
        }

        protected Tuple<Geometry, double> GetCurveTransformedTextGeometry(double textWidth)
        {
            Rect[] characterRanges = CurveTransformedTextCharacterRanges;

            int n = 0;
            int len = characterRanges.Length;
            for (int i = 0; i < len; i++)
            {
                if (characterRanges[i].Right > textWidth)
                {
                    break;
                }

                n++;
            }

            return GetOrCreateCurveTransformedTextGeometry(CurveTransformedText.Substring(0, n));
        }


        protected override void Render(Alt.GUI.Temporary.Gwen.Skin.Base skin)
        {
            DoubleBuffered = !(skin.Renderer.Graphics is SoftwareGraphics);
            SoftwareRender = true;

            base.Render(skin);
        }
    }
}
