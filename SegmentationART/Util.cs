using System;

namespace SegmentationART
{
    internal static class Util
    {
        public static double[] ComplementCode(this double[] x)
        {
            var result = new double[2 * x.Length];
            Array.Copy(x, 0, result, 0, x.Length);

            for (var i = 0; i < x.Length; i++)
                result[i + x.Length] = 1.0 - x[i];

            return result;
        }
    }
}
