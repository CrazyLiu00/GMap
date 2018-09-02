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
    //  Step 2 — managing glyphs
    //  
    //  ...
    //  5. Advanced text rendering: transformation + centering + kerning
    //
    //  Example_AltNETType_FreeType2_Step2_Simple modified code in order to be able
    //  to easily transform the rendered string, for example to rotate it.

    class Example_AltNETType_FreeType2_Step2_Advanced : Example__Base
    {
        const int WIDTH = 790;
        const int HEIGHT = 490;

        //  origin is the upper left corner
        byte[,] image;


        const int MAX_GLYPHS = 256;

        class TGlyph
        {
            //  glyph index
            public int index;

            //  glyph origin on the baseline
            public ANT_Vector pos;

            //  glyph image
            public ANT_Glyph image;


            public TGlyph()
            {
            }
        }

        //  glyph image
        TGlyph[] glyphs = new TGlyph[MAX_GLYPHS];
        TGlyph glyph;
        int num_glyphs;


        Bitmap m_BackBuffer;


        public Example_AltNETType_FreeType2_Step2_Advanced(Base parent)
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
            ANT_Error error;

            string filename;
            string text;

            int num_chars;

            bool use_kerning;
            int previous;
            int pen_x, pen_y, n;


            filename = Common.Example_AltNETType_FontFileName;
            text = "AltNETType (transformation + centering + kerning)";// Common.Pangram;
            num_chars = text.Length;


            //  ... initialize library ...
            //  ... create face object ...
            //  ... set character size ...
            {
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
                error = ANT.ANT_Set_Char_Size(face, 28 * 64, 0, 96, 0);
                //  error handling omitted
            }

            //  a small shortcut
            slot = face.glyph;


            //  start at (0,0)
            pen_x = 0;
            pen_y = 0;

            num_glyphs = 0;
            use_kerning = ANT.ANT_HAS_KERNING(face);
            previous = 0;

            ANT_Vector delta = new ANT_Vector();
            for (n = 0; n < num_chars; n++)
            {
                glyph = glyphs[n];
                if (glyph == null)
                {
                    glyphs[n] = glyph = new TGlyph();
                }

                //  convert character code to glyph index
                glyph.index = ANT.ANT_Get_Char_Index(face, text[n]);

                //  retrieve kerning distance and move pen position
                if (use_kerning &&
                    previous != 0 &&
                    glyph.index != 0)
                {
                    ANT.ANT_Get_Kerning(face, previous, glyph.index, ANT_Kerning_Mode.ANT_KERNING_DEFAULT, delta);

                    pen_x += delta.x >> 6;
                }

                //  store current pen position
                if (glyph.pos == null)
                {
                    glyph.pos = new ANT_Vector();
                }
                glyph.pos.x = pen_x;
                glyph.pos.y = pen_y;

                //  load glyph image into the slot without rendering
                error = ANT.ANT_Load_Glyph(face, glyph.index, ANT_LOAD.ANT_LOAD_DEFAULT);
                if (error != ANT_Error.ANT_Err_Ok)
                {
                    //  ignore errors, jump to next glyph
                    continue;
                }

                //  extract glyph image and store it in our table
                error = ANT.ANT_Get_Glyph(face.glyph, out glyph.image);
                if (error != ANT_Error.ANT_Err_Ok)
                {
                    //  ignore errors, jump to next glyph
                    continue;
                }

                //  translate the glyph image now
                ANT.ANT_Glyph_Transform(glyph.image, null, glyph.pos);

                //  increment pen position
                pen_x += slot.advance.x >> 6;

                //  record current glyph index
                previous = glyph.index;

                //  increment number of glyphs
                num_glyphs++;
            }


            ANT_Vector start = new ANT_Vector();
            //  transformation matrix
            ANT_Matrix matrix = new ANT_Matrix();

            ANT_Glyph image;
            ANT_Vector pen = new ANT_Vector();
            ANT_BBox bbox = new ANT_BBox();

            double angle;


            ANT_BBox string_bbox;
            compute_string_bbox(out string_bbox);


            int my_target_width = WIDTH;
            int my_target_height = HEIGHT;


            //  compute string dimensions in integer pixels
            int string_width = string_bbox.xMax - string_bbox.xMin;
            int string_height = string_bbox.yMax - string_bbox.yMin;


            //  use -30 degrees
            angle = (330.0 / 360) * 3.14159 * 2;

            //  compute start pen position in 26.6 cartesian pixels
            start.x = (int)(((my_target_width - string_width * Math.Cos(angle) - string_height * Math.Sin(angle)) / 2 + string_bbox.yMax * Math.Sin(angle)) * 64);
            //  start.y = ((my_target_height - string_height) / 2) * 64;
            //  need to center bitmaps (not base line)
            start.y = (int)(((my_target_height - string_height * Math.Cos(angle) - string_width * Math.Sin(angle)) / 2 - string_bbox.yMin * Math.Cos(angle)) * 64);


            //  set up transform (a rotation here)
            matrix.xx = (int)(Math.Cos(angle) * 0x10000L);
            matrix.xy = (int)(-Math.Sin(angle) * 0x10000L);
            matrix.yx = (int)(Math.Sin(angle) * 0x10000L);
            matrix.yy = (int)(Math.Cos(angle) * 0x10000L);


            pen.CopyFrom(start);

            for (n = 0; n < num_glyphs; n++)
            {
                //  create a copy of the original glyph
                error = ANT.ANT_Glyph_Copy(glyphs[n].image, out image);
                if (error != ANT_Error.ANT_Err_Ok)
                {
                    continue;
                }

                //  transform copy (this will also translate it to the
                //  correct position
                ANT.ANT_Glyph_Transform(image, matrix, pen);

                //  check bounding box; if the transformed glyph image
                //  is not in our target surface, we can avoid rendering it
                ANT.ANT_Glyph_Get_CBox(image, ANT_Glyph_BBox_Mode.ANT_GLYPH_BBOX_PIXELS, bbox);
                if (bbox.xMax <= 0 ||
                    bbox.xMin >= my_target_width ||
                    bbox.yMax <= 0 ||
                    bbox.yMin >= my_target_height)
                {
                    //continue;
                }
                else
                {
                    //  convert glyph image to bitmap (destroy the glyph copy!)
                    error = ANT.ANT_Glyph_To_Bitmap(ref image, ANT_Render_Mode.ANT_RENDER_MODE_NORMAL,
                              null,     //  no additional translation
                              true);    //  destroy copy in "image"
                    if (error == ANT_Error.ANT_Err_Ok)
                    {
                        ANT_BitmapGlyph bit = (ANT_BitmapGlyph)image;

                        draw_bitmap(bit.bitmap, bit.left, my_target_height - bit.top);
                    }
                }

                //  increment pen position --
                //  we don't have access to a slot structure,
                //  so we have to use advances from glyph structure
                //  (which are in 16.16 fixed float format)
                pen.x += image.advance.x >> 10;
                pen.y += image.advance.y >> 10;

                ANT.ANT_Done_Glyph(ref image);
            }


            //
            show_image();

            ANT.ANT_Done_Face(ref face);
            ANT.ANT_Done_AltNETType(ref library);
        }

        void compute_string_bbox(out ANT_BBox abbox)
        {
            ANT_BBox bbox = new ANT_BBox();
            ANT_BBox glyph_bbox = new ANT_BBox();


            //  initialize string bbox to "empty" values
            bbox.xMin = bbox.yMin = 32000;
            bbox.xMax = bbox.yMax = -32000;

            //  for each glyph image, compute its bounding box,
            //  translate it, and grow the string bbox
            for (int n = 0; n < num_glyphs; n++)
            {
                TGlyph glyph = glyphs[n];

                ANT.ANT_Glyph_Get_CBox(glyph.image, ANT_Glyph_BBox_Mode.ANT_GLYPH_BBOX_PIXELS, glyph_bbox);

                glyph_bbox.xMin += glyph.pos.x;
                glyph_bbox.xMax += glyph.pos.x;
                glyph_bbox.yMin += glyph.pos.y;
                glyph_bbox.yMax += glyph.pos.y;

                if (glyph_bbox.xMin < bbox.xMin)
                {
                    bbox.xMin = glyph_bbox.xMin;
                }

                if (glyph_bbox.yMin < bbox.yMin)
                {
                    bbox.yMin = glyph_bbox.yMin;
                }

                if (glyph_bbox.xMax > bbox.xMax)
                {
                    bbox.xMax = glyph_bbox.xMax;
                }

                if (glyph_bbox.yMax > bbox.yMax)
                {
                    bbox.yMax = glyph_bbox.yMax;
                }
            }

            //  check that we really grew the string bbox
            if (bbox.xMin > bbox.xMax)
            {
                bbox.xMin = 0;
                bbox.yMin = 0;
                bbox.xMax = 0;
                bbox.yMax = 0;
            }

            //  return string bbox
            abbox = bbox;
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
                Brush brush = new LinearGradientBrush(Point.Zero, new Point(temp.PixelWidth, temp.PixelHeight), Color.Blue, Color.Red);
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
