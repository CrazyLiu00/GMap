//  Copyright (C) 2012-2015 AltSoftLab Inc.
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;
using System.Collections.Generic;
using System.Text;


namespace Alt.GUI.Demo.SimpleSVG
{
    class SVGException : AltException
    {
        public SVGException(string text)
            : base(text)
        {
        }

        public SVGException(string format, params object[] args)
            : base(StringFormatter.sprintf(format, args))
        {
        }
    }
}
