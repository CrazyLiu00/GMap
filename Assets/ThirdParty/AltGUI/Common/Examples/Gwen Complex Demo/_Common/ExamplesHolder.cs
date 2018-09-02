//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Reflection;

using Alt.IO;
using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;
using Alt.GUI.Temporary.Gwen.Control.Layout;
using Alt.Sketch;


namespace Alt.GUI.Demo
{
    class ExamplesHolder : DockBase
    {
        class ExampleNode
        {
            ExamplesHolder m_Holder;
            Type m_ExampleType;
            Base m_ExampleParent;
            internal Example__Base m_Example;
            public Example__Base Example
            {
                get
                {
                    if (m_Example == null &&
                        m_ExampleType != null)
                    {
                        if (m_ExampleType.IsSubclassOf(typeof(Example__Base)))
                        {
                            Example__Base example = null;
                            try
                            {
                                example = (Example__Base)Activator.CreateInstance(m_ExampleType, new object[] { m_ExampleParent });
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());

                                try
                                {
                                    example = (Example__Base)Activator.CreateInstance(m_ExampleType);
                                }
                                catch (Exception ex2)
                                {
                                    Console.WriteLine(ex2.ToString());

                                    example = null;
                                }
                            }

                            if (example != null)
                            {
                                m_Example = example;
                                m_Example.Parent = m_ExampleParent;
                                m_Example.Dock = Pos.Fill;
                                m_Example.Hide();
                                m_Example.Holder = m_Holder;
                            }
                        }
                    }

                    return m_Example;
                }
            }


            public ExampleNode(Type type, Example__Base example, Base parent, ExamplesHolder holder)
            {
                m_ExampleType = type;
                m_Example = example;
                m_ExampleParent = parent;
                m_Holder = holder;

                if (m_Example != null)
                {
                    m_Example.Parent = m_ExampleParent;
                    m_Example.Dock = Pos.Fill;
                    m_Example.Hide();
                    m_Example.Holder = m_Holder;
                }
            }
        }


        Example__Base m_LastControl;
		readonly Alt.GUI.Temporary.Gwen.Control.StatusBar m_StatusBar;
		readonly Alt.GUI.Temporary.Gwen.Control.CollapsibleList m_ExamplesList;

        class CenterControl : Center
        {
            public string Info;

            public CenterControl(Base parent)
                : base(parent)
            {
            }

            Font m_Font;
            protected override void OnPaint(GUI.PaintEventArgs e)
            {
                base.OnPaint(e);

                if (!string.IsNullOrEmpty(Info))
                {
                    if (m_Font == null)
                    {
                        m_Font = new Font("Arial", 14, FontStyle.Bold);
                    }

                    SizeI size = e.Graphics.MeasureString(Info, m_Font).ToSizeI();

                    e.Graphics.DrawString(Info, m_Font, Brushes.White,
                        new Point((Width - size.Width) / 2, (Height - size.Height) / 2));
                }
            }
        }
        readonly CenterControl m_Center;

        public void SetCenterInfo(string text)
        {
            if (m_Center != null)
            {
                m_Center.Info = text;
            }
        }


        internal double FPS; // set this in your rendering loop


        internal RectI LogoArea
        {
            get
            {
                RectI result = RectI.Empty;
                if (m_Center != null)
                {
                    result = m_Center.RectangleToScreen(m_Center.ClientRectangle);

                    if (m_LastControl != null)
                    {
                        SizeI offset = m_LastControl.LogoRightTopOffset;
                        result -= offset;
                        result.Y += offset.Height;
                    }
                }

                return result;
            }
        }


        internal RectI ExampleArea
        {
            get
            {
                if (m_Center != null)
                {
                    return m_Center.RectangleToScreen(m_Center.ClientRectangle);
                }

                return RectI.Empty;
            }
        }


        public ExamplesHolder(Base parent) :
            base(parent)
        {
            Dock = Pos.Fill;


            Base mainLeftContainer = new Base(this);

            Base leftContainer = new Base(mainLeftContainer);
            leftContainer.Dock = Pos.Fill;
            leftContainer.Margin = new Margin(1);

			Alt.GUI.Temporary.Gwen.Control.Label label = new Alt.GUI.Temporary.Gwen.Control.Label(leftContainer);
            label.Margin = new Margin(7, 4, 5, 3);
			label.Text = "AltGUI Examples";
            label.TextColor = Color.LimeGreen * 1.4;
            //label.Alignment = Pos.Center;
            label.AutoSizeToContents = true;
            label.Dock = Pos.Top;


            Base examplesContainer = new Base(leftContainer);
            examplesContainer.Dock = Pos.Fill;

            m_ExamplesList = new CollapsibleList(examplesContainer);
            m_ExamplesList.Dock = Pos.Fill;
            m_ExamplesList.Margin = new Margin(0, 6, 0, 3);
            m_ExamplesList.ShouldDrawBackground = false;


			m_StatusBar = new Alt.GUI.Temporary.Gwen.Control.StatusBar(this);
            m_StatusBar.ShouldDrawBackground = false;
#if !DEBUG
            m_StatusBar.Hide();
#endif
            m_StatusBar.Dock = Pos.Bottom;


            m_ExamplesList.ShouldCacheToTexture = true;


            m_Center = new CenterControl(this);
            m_Center.Dock = Pos.Fill;
            m_Center.Margin = new Margin(1, 1, 1, 1);


            VerticalSplitter splitter = new VerticalSplitter(this);
            splitter.Dock = Pos.Fill;
            splitter.SetPanel(0, mainLeftContainer);
            splitter.SetPanel(1, m_Center);
            splitter.SetHValue(0.21f);


			Alt.GUI.Temporary.Gwen.Control.Button startButton = null;


            bool cat_ShouldCacheToTexture = false;
            {
                Color categoryColor = Color.Cyan * 1.2;
                Color color = Color.WhiteSmoke;
                CollapsibleCategory cat;


                cat = AddCategory("HTML", categoryColor);
				cat.CollapsingEnabled = false;
                {
                    cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;

                    //color = Color.Red;
                    RegisterDemo("HTML/CSS2 Renderer", cat, typeof(Example_HtmlRenderer_Multi), color);

                    //color = Color.Red;
                    RegisterDemo("HTML/CSS2 Renderer (Bitmap)", cat, typeof(Example_HtmlRenderer_RenderToBitmap), color);
                }
                //cat.IsCollapsed = true;


#if !WINDOWS_PHONE && !WINDOWS_PHONE_7 && !WINDOWS_PHONE_71 && !ANDROID && !__IOS__ || SILVERLIGHT
                cat = AddCategory("GIS", categoryColor);
				cat.CollapsingEnabled = false;
                {
                    cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;

                    //  GMap.NET Intractive Demo
                    //color = Color.CadetBlue;
#if SILVERLIGHT || UNITY_WEBPLAYER
                    RegisterDemo("GMap.NET (Interactive)", cat, typeof(Example_GMap_NotAvailable), color);
#else
                    RegisterDemo("GMap.NET (Interactive)", cat, typeof(Example_GMap), color);
#endif

                    //  GMap.NET
                    //color = Color.CadetBlue;
#if SILVERLIGHT || UNITY_WEBPLAYER
                    RegisterDemo("GMap.NET (Big Map Maker)", cat, typeof(Example_GMap_BigMapMaker_NotAvailable), color);
#else
                    RegisterDemo("GMap.NET (Big Map Maker)", cat, typeof(Example_GMap_BigMapMaker), color);
#endif
                }
                //cat.IsCollapsed = true;
#endif


                cat = AddCategory("Plot (Interactive)", categoryColor);
				cat.CollapsingEnabled = false;
                {
                    cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;

                    //  NPlot
                    color = Color.WhiteSmoke;// OliveDrab;
                    RegisterDemo("NPlot", cat, typeof(Example_NPlot_Multi), color);

                    //  OxyPlot
                    //color = Color.OliveDrab;
                    RegisterDemo("OxyPlot", cat, typeof(Example_OxyPlot_Multi), color);

                    //  PieChart
                    color = Color.WhiteSmoke;// OliveDrab;
                    RegisterDemo("Pie Chart", cat, typeof(Example_PieChart), color);

                    //  ZedGraph
                    //color = Color.OliveDrab;
                    RegisterDemo("ZedGraph", cat, typeof(Example_ZedGraph_Multi), color);
                }
                //cat.IsCollapsed = true;


                cat = AddCategory("AltNETType = FreeType.NET", categoryColor);//"Drawing"
				cat.CollapsingEnabled = false;
                {
                    cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;

                    color = Color.WhiteSmoke;
                    RegisterDemo("FreeType 2 Step 1", cat, typeof(Example_AltNETType_FreeType2_Step1), color);
                    RegisterDemo("FreeType 2 Step 2 Simple", cat, typeof(Example_AltNETType_FreeType2_Step2_Simple), color);
                    RegisterDemo("FreeType 2 Step 2 Advanced", cat, typeof(Example_AltNETType_FreeType2_Step2_Advanced), color);
                    RegisterDemo("Simple Font Cache Manager", cat, typeof(Example_AltNETType_SimpleFontCacheManager), color);
                    RegisterDemo("Outline Transformations", cat, typeof(Example_AltNETType_Outline_Transformations), color);
                    RegisterDemo("Font Atlas", cat, typeof(Example_AltNETType_FontAtlas), color);
                }
                //cat.IsCollapsed = true;


                cat = AddCategory("Geometry (Interactive)", categoryColor);
				cat.CollapsingEnabled = false;
                {
                    cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;

                    //  Vector Text
                    color = Color.WhiteSmoke;
					startButton = RegisterDemo("Transformed Text (Single Path)", cat, typeof(Example_VectorText_TransformedCurve1), color);
                    RegisterDemo("Transformed Text (Double Path)", cat, typeof(Example_VectorText_TransformedCurve2), color);
                    //TEMP  RegisterDemo("Vector Text Transformations", cat, typeof(Example_VectorText_Transformations), color);

                    //  Boolean
                    RegisterDemo("Boolean (CombinedGeometry)", cat, typeof(Example_CombinedGeometry), color);

                    //  Simple SVG
                    RegisterDemo("Simple SVG", cat, typeof(Example_SimpleSVG), color);

                    //  Lens Effect
                    RegisterDemo("Lens Effect SVG", cat, typeof(Example_LensEffectSVG), color);

                    //  Bilinear / Perspective Transformations
                    RegisterDemo("Bilinear / Perspective", cat, typeof(Example_Bilinear_Perspective), color);

                    //  Polar Transform
                    RegisterDemo("Polar Transform", cat, typeof(Example_PolarTransform), color);

                    //  Sin Transform
                    RegisterDemo("Sin Transform", cat, typeof(Example_SinTransform), color);

                    //  Outline
                    RegisterDemo("Outline SVG", cat, typeof(Example_Outline_SVG), color);

                    //  BSpline
                    RegisterDemo("Interactive B-Spline", cat, typeof(Example_BSpline), color);

                    //  Contour
                    RegisterDemo("Contour Tool & Poly Orientation", cat, typeof(Example_Contour), color);

                    //  Text Outline
                    RegisterDemo("Text Outline", cat, typeof(Example_TextOutline), color);

                    //  Affine Transformer
                    RegisterDemo("Affine Transformer", cat, typeof(Example_AffineTransformer), color);
                }
                //cat.IsCollapsed = true;


                cat = AddCategory("Graphics", categoryColor);//"Drawing"
				cat.CollapsingEnabled = false;
                {
                    cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;

//#if !ANDROID //&& !SILVERLIGHT && !WINDOWS_PHONE && !WINDOWS_PHONE_7 && !WINDOWS_PHONE_71
#if !UNITY_5
                    //  ExtBrush
                    color = Color.WhiteSmoke;
                    RegisterDemo("ExtBrush", cat, typeof(Example_ExtBrush), color);
#endif
//#endif

                    //  Clipper
                    color = Color.WhiteSmoke;
                    RegisterDemo("Clipper", cat, typeof(Example_Clipper), color);

                    //  SVG
                    color = Color.WhiteSmoke;
                    RegisterDemo("SVG", cat, typeof(Example_SVG), color);

                    //  Alpha Mask
                    color = Color.WhiteSmoke;
                    //TEMP  RegisterDemo("Alpha Mask", cat, typeof(Example_AlphaMask), color);

                    //  Brushes
                    color = Color.WhiteSmoke;
                    //RegisterDemo("Brushes", cat, typeof(Example_Brushes), color);
                }
                //cat.IsCollapsed = true;


                cat = AddCategory("Doc", categoryColor);
				cat.CollapsingEnabled = false;
                {
                    cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;

                    //  PdfSharp
                    //color = Color.Red;
                    RegisterDemo("PdfSharp", cat, typeof(Example_PdfSharp_Multi), color);
                }
                //cat.IsCollapsed = true;


                cat = AddCategory("Scientific Computing", categoryColor);
				cat.CollapsingEnabled = false;
                {
                    cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;

                    //  AForge
                    color = Color.WhiteSmoke;// OliveDrab;
                    RegisterDemo("AForge.NET", cat, typeof(Example_AForge_Multi), color);
                }
                //cat.IsCollapsed = true;


                cat = AddCategory("GUI", categoryColor);
				cat.CollapsingEnabled = false;
                {
                    cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;

                    //  Gwen
                    color = Color.WhiteSmoke;// CadetBlue;
                    if (Alt.Sketch.Config.Font_NoAntiAliasMaxSize < 10)
                    {
                        color = Color.White;
                    }

                    RegisterDemo("Gwen Skinned", cat, typeof(Example_Gwen_UnitTest_Skin), color);
                    RegisterDemo("Gwen Simple", cat, typeof(Example_Gwen_UnitTest_Simple), color);

                    //  QuickFont
                    //color = Color.LightBlue;
                    RegisterDemo("QuickFont", cat, typeof(Example_QuickFont), color);
					//	start long time background initialization
					Example_QuickFont.Initialize();

                    //  Gif
                    color = Color.WhiteSmoke;
                    RegisterDemo("Animated Gif in PictureBox", cat, typeof(Example_GifInPictureBox), color);
                }
                //cat.IsCollapsed = true;
				
				
				cat = AddCategory("Game Physics", categoryColor);
				cat.CollapsingEnabled = false;
				{
					cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;
					
					//  Phisics
					color = Color.WhiteSmoke;
					RegisterDemo("Box2D", cat, typeof(Example_Box2D), color);
					
					//  Can't load type FarseerPhysics World (or some member of it) - needs researches
					#if !WINDOWS_PHONE && !WINDOWS_PHONE_7 && !WINDOWS_PHONE_71 
					RegisterDemo("Farseer Physics", cat, typeof(Example_FarseerPhysics), color);
					#endif
				}
				//cat.IsCollapsed = true;


#if !WINDOWS_PHONE && !WINDOWS_PHONE_7 && !WINDOWS_PHONE_71 && !ANDROID && !__IOS__
                cat = AddCategory("THIRD PARTY", categoryColor);
				cat.CollapsingEnabled = false;
                {
                    cat.ShouldCacheToTexture = cat_ShouldCacheToTexture;

                    //  SVG
                    color = Color.WhiteSmoke;
                    RegisterDemo("Awesomium Web Browser", cat, typeof(Example_Awesomium), color);
                }
                //cat.IsCollapsed = true;
#endif
            }

            
			PrintText("AltGUI Gwen Demo Started!");


			OnExampleSelect(startButton);
        }


        public void Shutdown()
        {
            foreach (Base b in m_ExamplesList.Children)
            {
                CollapsibleCategory cat = b as CollapsibleCategory;

                foreach (Base button in cat.Children)
                {
                    ExampleNode node = button.UserData as ExampleNode;

                    if (node != null &&
                        node.m_Example != null)
                    {
                        if (node.m_Example == m_LastControl)
                        {
                            node.m_Example = null;
                            continue;
                        }

                        try
                        {
                            node.m_Example.Dispose();
                            node.m_Example = null;
                        }
                        catch
                        {
                        }
                    }
                }
            }

            if (m_LastControl != null)
            {
                if (m_Center != null)
                {
                    m_Center.RemoveChild(m_LastControl, false);
                }

                try
                {
                    m_LastControl.Dispose();
                    m_LastControl = null;
                }
                catch
                {
                }
            }

            m_Center.Children.Clear();
        }


        CollapsibleCategory AddCategory(string name, Color textColor)
        {
            CollapsibleCategory cat = m_ExamplesList.Add(name);
            cat.TextColor = textColor;
            cat.UseCurrentColorAsNormal = true;

            return cat;
        }


		Alt.GUI.Temporary.Gwen.Control.Button RegisterDemo(string name, CollapsibleCategory cat, Type exampleType, Color textColor)
        {
            return RegisterDemo(name, cat, exampleType, null, textColor);
        }

		Alt.GUI.Temporary.Gwen.Control.Button RegisterDemo(string name, CollapsibleCategory cat, Type exampleType, Example__Base example, Color textColor)
        {
			Alt.GUI.Temporary.Gwen.Control.Button btn = RegisterDemo(name, cat, exampleType, example);
            btn.TextColor = textColor;
            btn.UseCurrentColorAsNormal = true;

            return btn;
        }

		Alt.GUI.Temporary.Gwen.Control.Button RegisterDemo(string name, CollapsibleCategory cat, Type exampleType)
        {
            return RegisterDemo(name, cat, exampleType, null);
        }

		Alt.GUI.Temporary.Gwen.Control.Button RegisterDemo(string name, CollapsibleCategory cat, Type exampleType, Example__Base example)
        {
			Alt.GUI.Temporary.Gwen.Control.Button btn = cat.Add(name);
            btn.UserData = new ExampleNode(exampleType, example, m_Center, this);
            btn.Clicked += OnExampleSelect;

            return btn;
        }


		internal Alt.GUI.Temporary.Gwen.Control.Button RegisterDemo(string category, string caption, Type exampleType, Color categoryTextColor, Color captionTextColor)
        {
            CollapsibleCategory cat = null;
            foreach (Base b in m_ExamplesList.Children)
            {
                cat = b as CollapsibleCategory;
                if (cat != null &&
                    string.Equals(cat.Text, category))
                {
                    break;
                }

                cat = null;
            }

            if (cat == null)
            {
                cat = AddCategory(category, categoryTextColor);
            }

            return RegisterDemo(caption, cat, exampleType, captionTextColor);
        }


		void OnExampleSelect(Base control)
        {
			if (control == null)
			{
				return;
			}

            if (m_LastControl != null)
            {
                m_LastControl.OnDeactivate();
                m_LastControl.Hide();
            }

            Example__Base example = (control.UserData as ExampleNode).Example;
            if (example != null)
            {
                example.Show();
                example.OnActivate();
            }
            m_LastControl = example;
        }


        internal void PrintText(string str)
        {
        }


        protected override void Render(Alt.GUI.Temporary.Gwen.Skin.Base skin)
        {
            if (m_StatusBar != null)
            {
                //m_StatusBar.Text = String.Format("{0} fps. {1}", FPS.ToString("F1").Replace(',', '.'), Note);
            }


            if (m_TickTimer == null)
            {
                m_TickTimer = new IntervalTimer(10);
                m_TickTimer.Start();
            }
            ProcessTick();


            base.Render(skin);
        }


        IntervalTimer m_TickTimer;
        void ProcessTick()
        {
            try
            {
                double delta = m_TickTimer.ElapsedTime;
                if (m_TickTimer.IsTimeOver &&
                    m_LastControl != null)
                {
                    m_LastControl.OnTick(delta);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
