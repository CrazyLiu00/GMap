//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

using Alt.ComponentModel;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.HtmlRenderer;
using Alt.IO;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
    abstract class Example_HtmlRenderer_Base : Example__Base
    {
        protected internal override SizeI LogoRightTopOffset
        {
            get
            {
                return new SizeI(15, 0);
            }
        }


        protected Base m_LeftPanel;
        protected Base m_RightPanel;
        TreeControl m_ExamplesTreeView;
        Alt.GUI.Temporary.Gwen.Control.TreeNode m_HTMLRendererSamplesRoot;

        readonly Color GroupColor = Color.LightGreen;
        readonly Color NormalColor = Color.WhiteSmoke;
        readonly Color HoverColor = Color.Cyan;
        readonly Color SelectedColor = Color.Maroon * 1.4;


        /// <summary>
        /// the private font used for the demo
        /// </summary>
        readonly PrivateFontCollection _privateFont = new PrivateFontCollection();

        /// <summary>
        /// the html samples to show in the demo
        /// </summary>
        readonly Dictionary<string, string> m_Samples = new Dictionary<string, string>();


        public Example_HtmlRenderer_Base(Base parent) :
            base(parent)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            VerticalSplitter splitter = new VerticalSplitter(this);
            splitter.Dock = Pos.Fill;


            //  LeftPanel
            m_LeftPanel = new Base(splitter);
            m_LeftPanel.Dock = Pos.Fill;
            m_LeftPanel.Margin = new Margin(1);

            Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(m_LeftPanel);
            label.Margin = new Margin(5, 3, 5, 9);
			label.Text = "AltGUI.HtmlRenderer\n" + "Demos";
            label.TextColor = Color.Yellow;
            label.AutoSizeToContents = true;
            label.Dock = Pos.Top;


            //  RightPanel
            m_RightPanel = new Base(splitter);
            //m_RightPanel.DrawBorder = true;
            //m_RightPanel.BorderColor = Color.DodgerBlue;

            splitter.SetPanel(0, m_LeftPanel);
            splitter.SetPanel(1, m_RightPanel);
            splitter.SetHValue(0.2f);


            //  ExamplesTreeView
            m_ExamplesTreeView = new TreeControl(m_LeftPanel);
            m_ExamplesTreeView.Selected += OnSamplesTreeViewAfterSelect;
            m_ExamplesTreeView.ShouldDrawBackground = false;
            m_ExamplesTreeView.Dock = Pos.Fill;


            //
            LoadSamples();

            LoadCustomFonts();
        }

        /// <summary>
        /// Load custom fonts to be used by renderer htmls
        /// </summary>
        void LoadCustomFonts()
        {
            // load custom font font into private fonts collection
            /*NoNeed
            var file = Path.GetTempFileName();
            File.WriteAllBytes(file, HtmlRenderer.Resources.CustomFont);
            _privateFont.AddFontFile(file);*/
            _privateFont.AddFontStream(HtmlRenderer.Resources.CustomFont);

            // add the fonts to renderer
            foreach (var fontFamily in _privateFont.Families)
            {
                Alt.GUI.HtmlRenderer.HtmlRender.AddFontFamily(fontFamily);
            }
        }


        /// <summary>
        /// Loads the tree of document samples
        /// </summary>
        void LoadSamples()
        {
            m_HTMLRendererSamplesRoot = m_ExamplesTreeView.AddNode("HTML Renderer");
            m_HTMLRendererSamplesRoot.NormalTextColor = GroupColor;
            m_HTMLRendererSamplesRoot.HoverTextColor = HoverColor;
            m_HTMLRendererSamplesRoot.IsSelectable = false;

            Alt.GUI.Temporary.Gwen.Control.TreeNode testSamplesRoot = m_ExamplesTreeView.AddNode("Test Samples");
            testSamplesRoot.NormalTextColor = GroupColor;
            testSamplesRoot.HoverTextColor = HoverColor;
            testSamplesRoot.IsSelectable = false;

            Alt.GUI.Temporary.Gwen.Control.TreeNode perfTestSamplesRoot = m_ExamplesTreeView.AddNode("Performance Samples");
            perfTestSamplesRoot.NormalTextColor = GroupColor;
            perfTestSamplesRoot.HoverTextColor = HoverColor;
            perfTestSamplesRoot.IsSelectable = false;

            //var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            List<string> fnames = new List<string>();
            var fn = Alt.IO.VirtualDirectory.GetFiles("AltData/HtmlRenderer/Samples");
            if (fn != null &&
                fn.Length > 0)
            {
                fnames.AddRange(fn);
            }
            fn = Alt.IO.VirtualDirectory.GetFiles("AltData/HtmlRenderer/TestSamples");
            if (fn != null &&
                fn.Length > 0)
            {
                fnames.AddRange(fn);
            }

            var names = fnames.ToArray();

            Array.Sort(names);
            foreach (string name in names)
            {
                int extPos = name.LastIndexOf('.');
                int namePos = extPos > 0 && name.Length > 1 ? name.LastIndexOf('.', extPos - 1) : 0;
                string ext = name.Substring(extPos >= 0 ? extPos : 0);
                string shortName = namePos > 0 && name.Length > 2 ? name.Substring(namePos + 1, name.Length - namePos - ext.Length - 1) : name;

                if (".htm".IndexOf(ext) >= 0)
                {
                    var resourceStream = //Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
                        Alt.IO.VirtualFile.OpenRead(name);
                    if (resourceStream != null)
                    {
                        using (System.IO.StreamReader sreader = new System.IO.StreamReader(resourceStream
#if !SILVERLIGHT
                            , Encoding.Default
#endif
                            ))
                        {
                            m_Samples[name] = sreader.ReadToEnd();
                        }

                        Alt.GUI.Temporary.Gwen.Control.TreeNode node;
                        if (name.Contains("TestSamples"))//."))
                        {
                            node = testSamplesRoot.AddNode(shortName);
                        }
                        else if (name.Contains("PerfSamples"))
                        {
                            node = perfTestSamplesRoot.AddNode(shortName);
                        }
                        else
                        {
                            node = m_HTMLRendererSamplesRoot.AddNode(shortName);
                        }
                        node.Tag = name;

                        node.NormalTextColor = NormalColor;
                        node.SelectedTextColor = SelectedColor;
                        node.HoverTextColor = HoverColor;
                    }
                }
            }


            if (perfTestSamplesRoot.Children.Count < 1)
            {
                m_ExamplesTreeView.RemoveChild(perfTestSamplesRoot, true);
            }


            m_ExamplesTreeView.ExpandAll();
            //root.ExpandAll();
            //testSamplesRoot.ExpandAll();
        }


        protected void SelectFirstExample()
        {
            if (m_HTMLRendererSamplesRoot.Children.Count > 0)
            {
                (m_HTMLRendererSamplesRoot.Children[0] as Alt.GUI.Temporary.Gwen.Control.TreeNode).IsSelected = true;
            }
        }


        /// <summary>
        /// On tree view node click load the html to the html panel and html editor.
        /// </summary>
        void OnSamplesTreeViewAfterSelect(Base control)
        {
            Alt.GUI.Temporary.Gwen.Control.TreeNode node = control as Alt.GUI.Temporary.Gwen.Control.TreeNode;

            var name = node.Tag as string;
            if (!string.IsNullOrEmpty(name) &&
                m_Samples.ContainsKey(name))
            {
                SetHtml(m_Samples[name]);
            }
        }


        public virtual void SetHtml(string text)
        {
        }
        


        /// <summary>
        /// Handle stylesheet resolve.
        /// </summary>
        protected static void OnStylesheetLoad(object sender, HtmlStylesheetLoadEventArgs e)
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
        protected static void OnImageLoad(object sender, HtmlImageLoadEventArgs e)
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
                    e.Callback(Demo.HtmlRenderer.Resources.html32, rect);
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
                    return HtmlRenderer.Resources.html32;
                case "StarIcon":
                    return HtmlRenderer.Resources.favorites32;
                case "FontIcon":
                    return HtmlRenderer.Resources.font32;
                case "CommentIcon":
                    return HtmlRenderer.Resources.comment16;
                case "ImageIcon":
                    return HtmlRenderer.Resources.image32;
                case "MethodIcon":
                    return HtmlRenderer.Resources.method16;
                case "PropertyIcon":
                    return HtmlRenderer.Resources.property16;
                case "EventIcon":
                    return HtmlRenderer.Resources.Event16;
            }
            return null;
        }


        /// <summary>
        /// Show error raised from html renderer.
        /// </summary>
        protected void OnRenderError(object sender, HtmlRenderErrorEventArgs e)
        {
#if DEBUG || !(WINDOWS_PHONE_7 || WINDOWS_PHONE_71 || ANDROID || __IOS__)
            new Alt.GUI.Temporary.Gwen.Control.MessageBox(this//GetCanvas()
                , e.Message + (e.Exception != null ? "\r\n" + e.Exception : null), "Error in Html Renderer");
#endif
        }
    }
}
