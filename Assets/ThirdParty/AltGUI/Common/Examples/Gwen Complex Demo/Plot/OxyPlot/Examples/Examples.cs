// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Examples.cs" company="OxyPlot">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Reflection;

using OxyPlot;


namespace Alt.GUI.Demo.OxyPlot.ExampleLibrary
{
    public static class Examples
    {
        public static List<ExampleInfo> GetList()
        {
            var list = new List<ExampleInfo>();


            var assemblyTypes =
#if WINDOWS_PHONE_7 || WINDOWS_PHONE_71
                new Type[]
                {
                    typeof(AnnotationExamples),
                    typeof(AreaSeriesExamples),
                    typeof(AxisExamples),
                    typeof(BoxPlotSeriesExamples),
                    typeof(CartesianAxesExamples),
                    typeof(CategoryAxisExamples),
                    typeof(ClosedIssues),
                    typeof(ContourSeriesExamples),
                    typeof(CustomSeriesExamples),
                    typeof(DateTimeAxisExamples),
                    typeof(DiscreteDataSeriesExamples),
                    typeof(FilteringExamples),

                    //  FinancialSeriesExamples
                    typeof(HighLowSeriesExamples),
                    typeof(CandleStickSeriesExamples),

                    typeof(FunctionSeriesExamples),
                    typeof(HeatMapSeriesExamples),
                    typeof(ItemsSourceExamples),
                    typeof(LegendExamples),
                    typeof(LinearAxisExamples),
                    typeof(LineSeriesExamples),
                    typeof(LogarithmicAxisExamples),
                    typeof(MiscExamples),
                    typeof(MouseEventExamples),
                    typeof(OpenIssues),
                    //typeof(PerformanceExamples),
                    typeof(PieSeriesExamples),
                    typeof(PolarPlotExamples),
                    typeof(ScatterSeriesExamples),
                    typeof(ShowMeTheNumbersExamples),
                    typeof(TimeSpanAxisExamples),
                    typeof(TrackerExamples),

                    //  BarSeries
                    typeof(BarSeriesExamples),
                    typeof(ColumnSeriesExamples),
                    typeof(ErrorColumnSeriesExamples),
                    typeof(IntervalBarSeriesExamples),
                    typeof(RectangleBarSeriesExamples),
                    typeof(TornadoBarSeriesExamples),
                };
#else
                typeof(Examples).Assembly.GetTypes();
#endif

            foreach (var type in assemblyTypes)
            {
                var examplesAttributes =
                    //TEST  type.GetCustomAttributes(typeof(ExamplesAttribute), true).ToArray();
                    Alt.EnumerableHelper.ToArray(type.GetCustomAttributes(typeof(ExamplesAttribute), true));

                if (examplesAttributes.Length == 0)
                {
                    continue;
                }

                var examplesAttribute = examplesAttributes[0] as ExamplesAttribute;
                var types = new List<Type>();
                var baseType = type;
                while (baseType != null)
                {
                    types.Add(baseType);
                    baseType = baseType.BaseType;
                }

                foreach (var t in types)
                {
                    foreach (var method in t.GetMethods(BindingFlags.Public | BindingFlags.Static))
                    {
                        var exampleAttributes = method.GetCustomAttributes(typeof(ExampleAttribute), true);
                        if (exampleAttributes.Length == 0)
                        {
                            continue;
                        }

                        var exampleAttribute = (ExampleAttribute)exampleAttributes[0];
                        if (examplesAttribute != null)
                        {
                            list.Add(new ExampleInfo(examplesAttribute.Category, exampleAttribute.Title, method));
                        }
                    }
                }
            }

            return list;
        }
    }
}