//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Alt.IO;
using Alt.Sketch;


namespace Alt.GUI
{
	static partial class AltGUIHelper
	{
		internal static Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlLabel Create_HtmlLabel(Alt.GUI.Temporary.Gwen.Control.Base parent)
		{
			Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlLabel htmlLabel = new Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlLabel(parent);
			
			htmlLabel.AutoSize = false;
			htmlLabel.AutoSizeHeightOnly = true;
			htmlLabel.BackColor = Alt.Sketch.Color.Transparent;
			htmlLabel.BaseStylesheet = null;

			return htmlLabel;
		}
		
		
		internal static Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlPanel Create_HtmlPanel(Alt.GUI.Temporary.Gwen.Control.Base parent)
		{
			Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlPanel htmlPanel = new Alt.GUI.HtmlRenderer.Temporary.Gwen.HtmlPanel(parent);
			
			htmlPanel.ShouldDrawBackground = false;
			htmlPanel.TextRenderingHint = TextRenderingHint.Default;
			
			htmlPanel.AutoScroll = true;
			htmlPanel.BackColor = Alt.Sketch.Color.White;
			htmlPanel.BaseStylesheet = null;

			return htmlPanel;
		}
		
		

		internal static class HtmlRendererTools
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


			/// <summary>
			/// Handle stylesheet resolve.
			/// </summary>
			public static void OnStylesheetLoad(object sender, HtmlStylesheetLoadEventArgs e)
			{
				var stylesheet = GetStylesheet(e.Src);
				if (stylesheet != null)
				{
					e.SetStyleSheet = stylesheet;
				}
			}
			
			/// <summary>
			/// Get stylesheet by given key.
			/// </summary>
			static string GetStylesheet(string src)
			{
				if (src == "StyleSheet")
				{
					return @"h1, h2, h3 { color: navy; font-weight:normal; }
	                    h1 { margin-bottom: .47em }
	                    h2 { margin-bottom: .3em }
	                    h3 { margin-bottom: .4em }
	                    ul { margin-top: .5em }
	                    ul li {margin: .25em}
	                    body { font:10pt Tahoma }
			            pre  { border:solid 1px gray; background-color:#eee; padding:1em }
	                    .gray    { color:gray; }
	                    .example { background-color:#efefef; corner-radius:5px; padding:0.5em; }
	                    .whitehole { background-color:white; corner-radius:10px; padding:15px; }
	                    .caption { font-size: 1.1em }
	                    .comment { color: green; margin-bottom: 5px; margin-left: 3px; }
	                    .comment2 { color: green; }";
				}
				
				return null;
			}
			
			
			/// <summary>
			/// On image load in renderer set the image by event async.
			/// </summary>
			public static void OnImageLoad(object sender, HtmlImageLoadEventArgs e)
			{
				var img = TryLoadResourceImage(e.Src);
				if (img != null)
				{
					e.Callback(img);
				}
				
				if (!e.Handled && e.Attributes != null)
				{
					if (e.Attributes.ContainsKey("byevent"))
					{
						int delay;
						if (int.TryParse(e.Attributes["byevent"], out delay))
						{
							e.Handled = true;
							System.Threading.ThreadPool.QueueUserWorkItem(state =>
							                                              {
								System.Threading.Thread.Sleep(delay);
								e.Callback("https://fbcdn-sphotos-a-a.akamaihd.net/hphotos-ak-snc7/c0.44.403.403/p403x403/318890_10151195988833836_1081776452_n.jpg");
							});
						}
						else
						{
							e.Callback("http://sphotos-a.xx.fbcdn.net/hphotos-ash4/c22.0.403.403/p403x403/263440_10152243591765596_773620816_n.jpg");
						}
					}
					else if (e.Attributes.ContainsKey("byrect"))
					{
						var split = e.Attributes["byrect"].Split(',');
						var rect = new RectI(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2]), int.Parse(split[3]));
						e.Callback(html32, rect);
					}
				}
			}
			
			/// <summary>
			/// Get image by resource key.
			/// </summary>
			static Bitmap TryLoadResourceImage(string src)
			{
				switch (src)
				{
				case "HtmlIcon":
					return html32;
				case "StarIcon":
					return favorites32;
				case "FontIcon":
					return font32;
				case "CommentIcon":
					return comment16;
				case "ImageIcon":
					return image32;
				case "MethodIcon":
					return method16;
				case "PropertyIcon":
					return property16;
				case "EventIcon":
					return Event16;
				}
				return null;
			}
			
			
			/// <summary>
			/// Show error raised from html renderer.
			/// </summary>
			public static void OnRenderError(object sender, HtmlRenderErrorEventArgs e)
			{
				/*TEST
				#if DEBUG || !(WINDOWS_PHONE_7 || WINDOWS_PHONE_71 || ANDROID || __IOS__)
				new Alt.GUI.Temporary.Gwen.Control.MessageBox(this//GetCanvas()
				                                              , e.Message + (e.Exception != null ? "\r\n" + e.Exception : null), "Error in Html Renderer");
				#endif*/
			}
	    }
	}
}
