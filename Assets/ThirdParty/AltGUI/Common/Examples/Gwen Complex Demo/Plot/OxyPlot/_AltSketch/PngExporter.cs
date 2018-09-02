//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using OxyPlot;

namespace Alt.Sketch.Ext.OxyPlot
{
    /// <summary>
    /// The png exporter.
    /// </summary>
    public static class PngExporter
    {
        /// <summary>
        /// The export.
        /// </summary>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="Height">
        /// The Height.
        /// </param>
        /// <param name="background">
        /// The background.
        /// </param>
        public static void Export(PlotModel model, string fileName, int width, int height)//, Brush background = null)
        {
            Export(model, fileName, width, height, null);
        }
        public static void Export(PlotModel model, string fileName, int width, int height, Brush background)// = null)
        {
            using (var bm = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    if (background != null)
                    {
                        g.FillRectangle(background, 0, 0, width, height);
                    }

                    var rc = new GraphicsRenderContext { RendersToScreen = false };
                    rc.SetGraphicsTarget(g);
                    model.Update();
                    model.Render(rc, width, height);
                    bm.Save(fileName, ImageFormat.Png);
                }
            }
        }
    }
}
