using System;

namespace SegmentationART
{
    internal class WorkingMemory
    {
        public double[] Output { get; }
        private readonly double Q;

        public WorkingMemory(int length, double q)
        {
            Output = new double[length];
            Q = q;
        }

        public void Reset() => 
            Array.Clear(Output, 0, Output.Length);

        public void Add(int index)
        {
            // will throw if index is out of range
            // this means our working memory is not large enough to represent
            // all of the states from the previous ART network
            // todo could make this a dynamic data structure, but that would make
            // the following ART network input dynamic (requiring the length of
            // its node weights to be dynamic)

            // note: index = r - 1
            Output[index] = Math.Pow(Q, index);

            // todo could double the size of the memory and perform complement coding here
            // which would give better performance bc the input wouldn't need to be recoded
            // for every call to FuzzyArt.Learn
        }
    }
}
