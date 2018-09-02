//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Alt.IO;
using Alt.Sketch;


namespace Alt.GUI.Demo.HtmlRenderer
{
    class Resources
    {
        public static System.IO.Stream CustomFont
        {
            get
            {
                return Alt.IO.VirtualFile.OpenRead("AltData/HtmlRenderer/CustomFont.ttf");
            }
        }


        static Dictionary<string, Bitmap> m_Cache = new Dictionary<string, Bitmap>();

        static Bitmap GetBitmap(string name)
        {
            Bitmap result;
            if (m_Cache.TryGetValue(name, out result))
            {
                return result;
            }

            System.IO.Stream file = Alt.IO.VirtualFile.OpenRead("AltData/HtmlRenderer/" + name);
            if (file != null)
            {
                result = Bitmap.FromStream(file) as Bitmap;
            }

            m_Cache.Add(name, result);

            return result;
        }


        public static Bitmap html32
        {
            get
            {
                return GetBitmap("html32.png");
            }
        }


        public static Bitmap favorites32
        {
            get
            {
                return GetBitmap("favorites32.png");
            }
        }


        public static Bitmap font32
        {
            get
            {
                return GetBitmap("font32.png");
            }
        }


        public static Bitmap comment16
        {
            get
            {
                return GetBitmap("comment16.gif");
            }
        }


        public static Bitmap image32
        {
            get
            {
                return GetBitmap("image32.png");
            }
        }


        public static Bitmap method16
        {
            get
            {
                return GetBitmap("method16.gif");
            }
        }


        public static Bitmap property16
        {
            get
            {
                return GetBitmap("property16.gif");
            }
        }


        public static Bitmap Event16
        {
            get
            {
                return GetBitmap("Event16.png");
            }
        }


        public static string Tooltip
        {
            get
            {
                return
                    /*"<b>Select any document on the right</b>" +
                    "<p>You can edit the HTML of any document using the text on the bottom</p>" +
                    "<hr>" +
                    "<blockquote>" +
                    "<img src=\"StarIcon\">Yes, this is an <b>HtmlToolTip</b> and its quite <b color=blue>cool" +
                    "</blockquote>";*/
                    "<b>HtmlPanel control showing <u>HTML Renderer</u> capabilities</b>" +
		            "<table border=\"0\" style=\"margin: 10px 20px;\">" +
                            "<tr>" +
                                "<td width=\"32\" style=\"padding: 2px 5px 0 0\">" +
                                    "<img src=\"HtmlIcon\" />" +
                                "</td>" +
                                "<td>" +
                                    "You can select html samples on the left or you can edit <br/> the HTML of any document using the editor on the bottom." +
                                "</td>" +
                            "</tr>" +
                        "</table>" +
		            "<hr/>" +
		            "<br/>" +
		            "<div>" +
			            "This is an <b>HtmlToolTip</b> and it's very <b color=blue>COOL</b>!!!<br/>" +
			            "You can even click on the <a href=\"https://htmlrenderer.codeplex.com/\">links</a>!" +
		            "</div>";
            }
        }


        public static string SampleForm_HtmlPanel
        {
            get
            {
                return
                    "This is an <b>HtmlPanel</b> with <span style=\"color: red\">" +
                    "colors</span> and links: <a href=\"http://htmlrenderer.codeplex.com/\">" +
                    "HTML Renderer</a><div style=\"font-size: 1.2em; padding-top: 10px;\" >" +
                    "If there is more text than the size of the control scrollbars will appear." +
                    "</div><br/>Click me to change my <font color=darkblue>Text</font> property.";
            }
        }
    }
}
