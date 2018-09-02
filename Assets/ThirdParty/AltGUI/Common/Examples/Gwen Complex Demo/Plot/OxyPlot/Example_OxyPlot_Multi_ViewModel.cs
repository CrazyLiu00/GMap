//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;

using Alt.ComponentModel;
using Alt.GUI.Demo.OxyPlot.ExampleLibrary;
using Alt.Sketch;
using Alt.Sketch.Ext.OxyPlot;

using OxyPlot;


namespace Alt.GUI.Demo.OxyPlot
{
    public class Example_OxyPlot_Multi_ViewModel : INotifyPropertyChanged
    {
        IEnumerable<ExampleInfo> examples;
        public IEnumerable<ExampleInfo> Examples
        {
            get
            {
                return examples;
            }
            set
            {
                examples = value; RaisePropertyChanged("Examples");
            }
        }


        ExampleInfo selectedExample;
        public ExampleInfo SelectedExample
        {
            get
            {
                return selectedExample;
            }
            set
            {
                selectedExample = value; RaisePropertyChanged("SelectedExample");
            }
        }


        public Example_OxyPlot_Multi_ViewModel()
        {
            this.Examples =
                //TEST  ExampleLibrary.Examples.GetList().OrderBy(e => e.Category);
                Alt.EnumerableHelper.OrderBy(ExampleLibrary.Examples.GetList(), e => e.Category);
        }


        public Color PlotBackground
        {
            get
            {
                return SelectedExample != null && SelectedExample.PlotModel.Background != null
                           ? ConverterExtensions.ToColor(SelectedExample.PlotModel.Background)
                           : Color.White;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string property)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(property));
            }
        }

    }
}
