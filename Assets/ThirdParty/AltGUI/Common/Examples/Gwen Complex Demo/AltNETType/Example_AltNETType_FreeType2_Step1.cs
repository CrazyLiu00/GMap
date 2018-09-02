//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.NETType;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
    //  FreeType 2 Tutorial
    //  Step 1 — simple glyph loading
    //  
    //  example1.c (AltNETType port)
    //
    //  This small program shows how to print a rotated string with the
    //  AltNETType (FreeType.NET) library.

    class Example_AltNETType_FreeType2_Step1 : Example__Base
    {
        const int WIDTH = 790;
        const int HEIGHT = 451;


        //  origin is the upper left corner
        byte[,] image;


        Bitmap m_BackBuffer;


        public Example_AltNETType_FreeType2_Step1(Base parent)
            : base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            image = new byte[HEIGHT, WIDTH];

            DoWork();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.DrawImage(m_BackBuffer, new Point(5, (Height - m_BackBuffer.PixelHeight) / 2));
        }


        void DoWork()
        {
            ANT_Library library;
            ANT_Face face;

            ANT_GlyphSlot slot;
            //  transformation matrix
            ANT_Matrix matrix = new ANT_Matrix();
            //  untransformed origin
            ANT_Vector pen = new ANT_Vector();
            ANT_Error error;

            string filename;
            string text;

            double angle;
            int target_height;
            int n, num_chars;


            filename = Common.Example_AltNETType_FontFileName;
            text = "AltNETType = FreeType.NET";
            num_chars = text.Length;
            //  use 25 degrees
            angle = (25.0 / 360) * 3.14159 * 2;
            target_height = HEIGHT;

            //  initialize library
            error = ANT.ANT_Init_AltNETType(out library);
            //  error handling omitted

            //  create face object
            error = ANT.ANT_New_Face(library, filename, 0, out face);
            //  error handling omitted
            if (error != ANT_Error.ANT_Err_Ok)
            {
                return;
            }

            //  use 50pt at 100dpi
            //  set character size
            error = ANT.ANT_Set_Char_Size(face, 50 * 64, 0, 96, 0);
            //  error handling omitted

            slot = face.glyph;


            //  set up matrix
            matrix.xx = (int)(Math.Cos(angle) * 0x10000L);
            matrix.xy = (int)(-Math.Sin(angle) * 0x10000L);
            matrix.yx = (int)(Math.Sin(angle) * 0x10000L);
            matrix.yy = (int)(Math.Cos(angle) * 0x10000L);

            //  the pen position in 26.6 cartesian space coordinates;
            //  start at (25, 503) relative to the upper left corner
            pen.x = 15 * 64;
            pen.y = (target_height - (HEIGHT - 20)) * 64;

            for (n = 0; n < num_chars; n++)
            {
                //  set transformation
                ANT.ANT_Set_Transform(face, matrix, pen);

                //  load glyph image into the slot (erase previous one)
                error = ANT.ANT_Load_Char(face, text[n], ANT_LOAD.ANT_LOAD_RENDER);
                if (error != ANT_Error.ANT_Err_Ok)
                {
                    //  ignore errors
                    continue;
                }

                //  now, draw to our target surface (convert position)
                draw_bitmap(slot.bitmap, slot.bitmap_left, target_height - slot.bitmap_top);

                //  increment pen position
                pen.x += slot.advance.x;
                pen.y += slot.advance.y;
            }


            show_image();

            ANT.ANT_Done_Face(ref face);
            ANT.ANT_Done_AltNETType(ref library);
        }


        //  Replace this function with something useful.
        void draw_bitmap(ANT_Bitmap bitmap, int x, int y)
        {
            int i, j, p, q;
            int x_max = x + bitmap.width;
            int y_max = y + bitmap.rows;

            byte[] bitmap_buffer = bitmap.buffer;
            int bitmap_width = bitmap.width;

            for (i = x, p = 0; i < x_max; i++, p++)
            {
                for (j = y, q = 0; j < y_max; j++, q++)
                {
                    if (i < 0 ||
                        j < 0 ||
                        i >= WIDTH ||
                        j >= HEIGHT)
                    {
                        continue;
                    }

                    image[j, i] |= bitmap_buffer[q * bitmap_width + p];
                }
            }
        }


        void show_image()
        {
            Bitmap temp = new Bitmap(WIDTH, HEIGHT, PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage(temp))
            {
                Brush brush = new LinearGradientBrush(Point.Zero, new Point(temp.PixelWidth, temp.PixelHeight), Color.Red, Color.GreenYellow);
                graphics.FillRectangle(brush, temp.PixelRectangle);
            }


            BitmapData tempData = temp.LockBits(ImageLockMode.WriteOnly);
            byte[] scan0 = tempData.Scan0;
            int index = -1;
            {
                for (int i = 0; i < HEIGHT; i++)
                {
                    for (int j = 0; j < WIDTH; j++)
                    {
                        scan0[index += 4] = image[i, j];
                    }
                }
            }
            temp.UnlockBits(tempData);


            m_BackBuffer = new Bitmap(WIDTH, HEIGHT, PixelFormat.Format24bppRgb);
            using (Graphics graphics = Graphics.FromImage(m_BackBuffer))
            {
                graphics.Clear(Color.WhiteSmoke);

                graphics.DrawImage(temp, Point.Zero);
            }
        }
    }
}
