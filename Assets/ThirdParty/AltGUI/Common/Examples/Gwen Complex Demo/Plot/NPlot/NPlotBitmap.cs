//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.GUI.NPlot;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
    class NPlotBitmap : DisposableObject
    {
        PlotSurface2D m_PlotSurface2D;
        public PlotSurface2D PlotSurface2D
        {
            get
            {
                return m_PlotSurface2D;
            }
            set
            {
                if (m_PlotSurface2D != value)
                {
                    m_PlotSurface2D = value;
                    m_fNeedUpdate = true;
                }
            }
        }


        Color m_BackgroundColor = Color.Transparent;
        public Color BackgroundColor
        {
            get
            {
                return m_BackgroundColor;
            }
            set
            {
                if (m_BackgroundColor != value)
                {
                    m_BackgroundColor = value;
                    m_fNeedUpdate = true;
                }
            }
        }


        int m_Width = 0;
        public int Width
        {
            get
            {
                return m_Width;
            }
            set
            {
                if (m_Width != value)
                {
                    m_Width = value;
                    m_fNeedUpdate = true;
                }
            }
        }


        int m_Height = 0;
        public int Height
        {
            get
            {
                return m_Height;
            }
            set
            {
                if (m_Height != value)
                {
                    m_Height = value;
                    m_fNeedUpdate = true;
                }
            }
        }


        public SizeI Size
        {
            get
            {
                return new SizeI(Width, Height);
            }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }


        bool m_fNeedUpdate = true;
        public void NeedUpdate()
        {
            m_fNeedUpdate = true;
        }


        Bitmap m_Bitmap;
        public Bitmap Bitmap
        {
            get
            {
                if (m_PlotSurface2D == null)
                {
                    return null;
                }

                int width = Width;
                int height = Height;

                if (width < 1 ||
                    height < 1)
                {
                    return null;
                }


                //  Recreate
                if (m_Bitmap != null &&
                    (m_Bitmap.PixelWidth != width || m_Bitmap.PixelHeight != height))
                {
                    m_Bitmap.Dispose();
                    m_Bitmap = null;
                }

                if (m_Bitmap == null)
                {
                    m_Bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                }

                if (m_Bitmap != null &&
                    m_fNeedUpdate)
                {
                    m_fNeedUpdate = false;

                    using (Graphics graphics = Graphics.FromImage(m_Bitmap))
                    {
                        graphics.Clear(BackgroundColor);

                        m_PlotSurface2D.Draw(graphics, new RectI(0, 0, width, height));
                    }
                }

                return m_Bitmap;
            }
        }


        public NPlotBitmap(PlotSurface2D plotSurface2D)
        {
            m_PlotSurface2D = plotSurface2D;
        }


        protected override void Dispose(bool disposing)
        {
            //  Not need to destroy m_Bitmap - it still can be in use

            base.Dispose(disposing);
        }
    }
}
