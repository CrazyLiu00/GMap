//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.GUI.Integration;
using Alt.Sketch;


namespace Alt.GUI
{
    public static partial class AltGUIIntegration
    {
        public static void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            PCL.PCLTools.Initialize();

            InitializePlatformSpecificTools();
        
			InstalledFontCollection.AddFontFiles("AltData/Fonts");
			InstalledFontCollection.AddFontFiles("AltData/Fonts/Open-Sans");
		}


        public static void Tick()
        {
            DoTick();
        }
    }
}
