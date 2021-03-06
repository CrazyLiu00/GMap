﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClosedIssues.cs" company="OxyPlot">
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
// <summary>
//   Defines the ClosedIssues type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Alt.GUI.Demo.OxyPlot.ExampleLibrary
{
    [Examples("Z9 Issues (closed)")]
    public class ClosedIssues : ExamplesBase
    {
        [Example("#9990: Major grid lines in front of minor")]
        public static PlotModel GridLinesBothDifferentColors()
        {
            var plotModel1 = new PlotModel
                {
                    Title = "Issue #9990", 
                    Subtitle = "Minor gridlines should be below major gridlines"
                };
            var leftAxis = new LinearAxis
                {
                    MajorGridlineStyle = LineStyle.Solid, 
                    MajorGridlineColor = OxyColors.Black, 
                    MajorGridlineThickness = 4, 
                    MinorGridlineStyle = LineStyle.Solid, 
                    MinorGridlineColor = OxyColors.LightBlue, 
                    MinorGridlineThickness = 4, 
                };
            plotModel1.Axes.Add(leftAxis);
            var bottomAxis = new LinearAxis(AxisPosition.Bottom)
                {
                    MajorGridlineStyle = LineStyle.Solid, 
                    MajorGridlineColor = OxyColors.Black, 
                    MajorGridlineThickness = 4, 
                    MinorGridlineStyle = LineStyle.Solid,
                    MinorGridlineColor = OxyColors.LightBlue, 
                    MinorGridlineThickness = 4, 
                };
            plotModel1.Axes.Add(bottomAxis);
            return plotModel1;
        }

        [Example("D445576: Invisible contour series")]
        public static PlotModel InvisibleContourSeries()
        {
            var model = new PlotModel("Invisible contour series");
            var cs = new ContourSeries
            {
                IsVisible = false,
                ColumnCoordinates = global::OxyPlot.ArrayHelper.CreateVector(-1, 1, 0.05),
                RowCoordinates = global::OxyPlot.ArrayHelper.CreateVector(-1, 1, 0.05)
            };
            cs.Data = global::OxyPlot.ArrayHelper.Evaluate((x, y) => x + y, cs.ColumnCoordinates, cs.RowCoordinates);
            model.Series.Add(cs);
            return model;
        }
    }
}