//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using OxyPlot;

namespace Alt.Sketch.Ext.OxyPlot
{
    /// <summary>
    /// Extension method used to convert to/from Windows/Windows.Media classes.
    /// </summary>
    public static class ConverterExtensions
    {
        /// <summary>
        /// Calculate the distance between two points.
        /// </summary>
        /// <param name="p1">
        /// The first point.
        /// </param>
        /// <param name="p2">
        /// The second point.
        /// </param>
        /// <returns>
        /// The distance.
        /// </returns>
        public static double DistanceTo(PointI p1, PointI p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return Math.Sqrt((dx * dx) + (dy * dy));
        }


        /// <summary>
        /// Converts a color to a Brush.
        /// </summary>
        /// <param name="c">
        /// The color.
        /// </param>
        /// <returns>
        /// A SolidColorBrush.
        /// </returns>
        public static Brush ToBrush(OxyColor c)
        {
            return new SolidColorBrush(ToColor(c));
        }


        /// <summary>
        /// Converts an OxyColor to a Color.
        /// </summary>
        /// <param name="c">
        /// The color.
        /// </param>
        /// <returns>
        /// A Color.
        /// </returns>
        public static Color ToColor(OxyColor c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }


        /// <summary>
        /// Converts a HorizontalAlignment to a HorizontalTextAlign.
        /// </summary>
        /// <param name="alignment">
        /// The alignment.
        /// </param>
        /// <returns>
        /// A HorizontalTextAlign.
        /// </returns>
        public static global::OxyPlot.HorizontalAlignment ToHorizontalTextAlign(HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Center:
                    return global::OxyPlot.HorizontalAlignment.Center;
                case HorizontalAlignment.Right:
                    return global::OxyPlot.HorizontalAlignment.Right;
                default:
                    return global::OxyPlot.HorizontalAlignment.Left;
            }
        }


        /// <summary>
        /// Converts a Color to an OxyColor.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <returns>
        /// An OxyColor.
        /// </returns>
        public static OxyColor ToOxyColor(Color color)
        {
            return OxyColor.FromArgb(color.A, color.R, color.G, color.B);
        }


        /// <summary>
        /// Converts a nullable Color to an OxyColor.
        /// </summary>
        /// <param name="color">
        /// The color.
        /// </param>
        /// <returns>
        /// An OxyColor.
        /// </returns>
        public static OxyColor ToOxyColor(Color? color)
        {
            return color.HasValue ? ToOxyColor(color.Value) : null;
        }


        /// <summary>
        /// Converts a Brush to an OxyColor.
        /// </summary>
        /// <param name="brush">
        /// The brush.
        /// </param>
        /// <returns>
        /// An oxycolor.
        /// </returns>
        public static OxyColor ToOxyColor(Brush brush)
        {
            var scb = brush as SolidColorBrush;
            return scb != null ? ToOxyColor(scb.Color) : null;
        }


        /// <summary>
        /// Converts a Thickness to an OxyThickness.
        /// </summary>
        /// <returns>
        /// An OxyPlot thickness.
        /// </returns>
        /// <summary>
        /// Converts a ScreenPoint to a PointI.
        /// </summary>
        /// <param name="pt">
        /// The screen point.
        /// </param>
        /// <param name="aliased">
        /// use pixel alignment conversion if set to <c>true</c>.
        /// </param>
        /// <returns>
        /// A point.
        /// </returns>
        public static PointI ToPoint(ScreenPoint pt, bool aliased)
        {
            // adding 0.5 to get pixel boundary alignment, seems to work
            // http://weblogs.asp.net/mschwarz/archive/2008/01/04/silverlight-rectangles-paths-and-line-comparison.aspx
            // http://www.wynapse.com/Silverlight/Tutor/Silverlight_Rectangles_Paths_And_Lines_Comparison.aspx
            if (aliased)
            {
                return new PointI((int)pt.X, (int)pt.Y);
            }

            return new PointI((int)Math.Round(pt.X), (int)Math.Round(pt.Y));
        }


        /// <summary>
        /// Converts an OxyRect to a Rect.
        /// </summary>
        /// <param name="r">
        /// The rectangle.
        /// </param>
        /// <param name="aliased">
        /// use pixel alignment if set to <c>true</c>.
        /// </param>
        /// <returns>
        /// A rect.
        /// </returns>
        public static RectI ToRect(OxyRect r, bool aliased)
        {
            if (aliased)
            {
                var x = (int)r.Left;
                var y = (int)r.Top;
                var ri = (int)r.Right;
                var bo = (int)r.Bottom;
                return new RectI(x, y, ri - x, bo - y);
            }

            return new RectI(
                (int)Math.Round(r.Left), (int)Math.Round(r.Top), (int)Math.Round(r.Width), (int)Math.Round(r.Height));
        }


        /// <summary>
        /// Converts a point to a ScreenPoint.
        /// </summary>
        /// <param name="pt">
        /// The point.
        /// </param>
        /// <returns>
        /// A screen point.
        /// </returns>
        public static ScreenPoint ToScreenPoint(PointI pt)
        {
            return new ScreenPoint(pt.X, pt.Y);
        }


        /// <summary>
        /// Converts a PointI array to a ScreenPoint array.
        /// </summary>
        /// <param name="points">
        /// The points.
        /// </param>
        /// <returns>
        /// A ScreenPoint array.
        /// </returns>
        public static ScreenPoint[] ToScreenPointArray(PointI[] points)
        {
            if (points == null)
            {
                return null;
            }

            var pts = new ScreenPoint[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                pts[i] = ToScreenPoint(points[i]);
            }

            return pts;
        }
    }
}
