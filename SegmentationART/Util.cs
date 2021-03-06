﻿using System;
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

        public static V[] ArrayFunc<T, U, V>(this T[] left, U[] right, Func<T, U, V> func) =>
            ArrayFill(left.Length, i => func(left[i], right[i]));

        public static double[] FuzzyIntersection(this double[] left, double[] right) =>
            left.ArrayFunc(right, Math.Min);

        public static double CityBlockNorm(this IEnumerable<double> x) =>
            x.Select(Math.Abs).Sum();

        public static T[] ArrayFill<T>(int size, Func<int, T> func)
        {
            var result = new T[size];
            
            for (var i = 0; i < result.Length; i++)
                result[i] = func(i);
            
            return result;
        }

        public static double[] Ones(int size) => FillNumber(size, 1.0);

        public static double[] Zeros(int size) => FillNumber(size, 0.0);

        private static double[] FillNumber(int size, double num) =>
            ArrayFill(size, i => num);
    }
}
