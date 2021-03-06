﻿using System;

namespace SegmentationART
{
    internal class WorkingMemory
    {
        public double[] Output { get; }
        private readonly double Q;
        private double R;

        public WorkingMemory(int inputSize, double q)
        {
            if (q <= 0.0 || q >= 1.0) throw new ArgumentOutOfRangeException(nameof(q));

            Output = new double[inputSize * 2];
            Reset(inputSize);

            Q = q;
        }

        public void Reset() => Reset(Output.Length / 2);

        private void Reset(int inputSize)
        {
            Array.Clear(Output, 0, inputSize);
            for (var i = inputSize; i < Output.Length; i++)
                Output[i] = 1.0;

            R = 1;
        }

        public void Add(int index)
        {
            // will throw if index is out of range
            // this means our working memory is not large enough to represent
            // all of the states from the previous ART network
            // todo could make this a dynamic data structure, but that would make
            // the following ART network input dynamic (requiring the length of
            // its node weights to be dynamic)

            // update memory location for this index with a decaying activation
            Output[index] = Math.Pow(Q, R - 1.0);
            Output[Output.Length / 2 + index] = 1.0 - Output[index];

            // increment R
            R++;
        }
    }
}
