using System;
using System.Collections.Generic;
using System.Linq;

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

        public static double[] ArrayFunc(this double[] left, double[] right, Func<double, double, double> func)
        {
            var result = new double[left.Length];

            for (var i = 0; i < result.Length; i++)
                result[i] = func(left[i], right[i]);

            return result;
        }

        public static double[] FuzzyIntersection(this double[] left, double[] right) =>
            left.ArrayFunc(right, Math.Min);

        public static double CityBlockNorm(this IEnumerable<double> x) =>
            x.Select(Math.Abs).Sum();
    }
}
