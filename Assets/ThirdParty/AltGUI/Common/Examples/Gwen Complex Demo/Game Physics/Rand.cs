//  Original source code has been modified by AltSoftLab Inc. 2012-2015
//  This source code is provided "as is" without express or implied warranty of any kind.

using System;


namespace Alt
{
    public static class Rand
    {
        public static int RAND_LIMIT = 32767;
        public static Random Random = new Random();
        //Box2D public static Random rand = new Random(0x2eed2eed);
        public static Random rand
        {
            get
            {
                return Random;
            }
        }


        /// <summary>
        /// Random number in range [-1,1]
        /// </summary>
        /// <returns></returns>
        public static double RandomFloat()
        {
            return (double)(Random.NextDouble() * 2.0 - 1.0);
        }


        /// <summary>
        /// Random floating point number in range [lo, hi]
        /// </summary>
        /// <param name="lo">The lo.</param>
        /// <param name="hi">The hi.</param>
        /// <returns></returns>
        public static double RandomFloat(double lo, double hi)
        {
            double r = (double)Random.NextDouble();
            r = (hi - lo) * r + lo;
            return r;
        }
    }
}
