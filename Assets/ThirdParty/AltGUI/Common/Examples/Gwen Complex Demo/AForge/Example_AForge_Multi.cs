//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Text;

using Alt.ComponentModel;

using Alt.GUI.Temporary.Gwen;
using Alt.GUI.Temporary.Gwen.Control;


namespace Alt.GUI.Demo
{
    class Example_AForge_Multi : Example__Base_Multi
    {
        public Example_AForge_Multi(Base parent)
            : base(parent)
        {
            TitlePrefix = "AltGUI.AForge Demo: ";
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SplitterSetHValue(0.17);
        }


        protected override void RegisterExamples()
        {
            string run = null;

            string subCat = null;
            string cat = "Core";

#if !WINDOWS_PHONE_7 && !WINDOWS_PHONE_71 && !UNITY_WEBPLAYER
            RegisterExampleCategory(cat);
            {
                RegisterExample("Parallel Test", cat, typeof(Example_AForge_ParallelTest));
            }
#endif

            cat = "Fuzzy";
            RegisterExampleCategory(cat);
            {
                RegisterExample(run = "Fuzzy AGV", cat, typeof(Example_AForge_FuzzyAGV), "Fuzzy Auto Guided Vehicle Sample");
                RegisterExample("Fuzzy Sets", cat, typeof(Example_AForge_FuzzySetSample), "Fuzzy Sets Sample");
            }

            cat = "Genetic";
            RegisterExampleCategory(cat);
            {
                RegisterExample("TSP", cat, typeof(Example_AForge_Genetic_TSP), "Traveling Salesman Problem using Genetic Algorithms");
            }

            cat = "Imagine";
            RegisterExampleCategory(cat);
            {
                RegisterExample("Blobs Explorer", cat, typeof(Example_AForge_BlobsExplorer));
                RegisterExample("Filters Demo", cat, typeof(Example_AForge_FiltersDemo), "Image Processing filters demo");
                RegisterExample("Shape Checker", cat, typeof(Example_AForge_ShapeChecker), "Simple Shape Checker");
                RegisterExample("Textures Demo", cat, typeof(Example_AForge_TexturesDemo));
            }

            cat = "Machine Learning";
            RegisterExampleCategory(cat);
            {
                RegisterExample("Animat", cat, typeof(Example_AForge_Animat));
            }

            cat = "Neuro";
            RegisterExampleCategory(cat);
            {
                subCat = "SOM";
                RegisterExampleSubCategory(subCat, cat);
                {
                    RegisterExample("2D Organizing", subCat, cat, typeof(Example_AForge_2DOrganizing), "Kohonen SOM 2D Organizing");
                    RegisterExample("Color", subCat, cat, typeof(Example_AForge_Color), "Color Clustering using Kohonen SOM");
                    RegisterExample("TSP", subCat, cat, typeof(Example_AForge_Neuro_SOM_TSP), "Traveling Salesman Problem using Elastic Net");
                }
            }

            Start(run);
            //Start(run, subCat, cat);
        }
    }
}
