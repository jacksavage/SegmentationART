using System;
using System.Collections.Generic;
using System.Linq;

namespace SegmentationART
{
    internal class FuzzyArt
    {
        public List<double[]> Nodes { get; } = new();
        public int InputLength { get; }
        public double Choice { get; }
        public double LearningRate { get; }
        public double Vigilance { get; }

        public FuzzyArt(int inputLength, double choice, double learningRate, double vigilance)
        {
            // check that params are in proper ranges
            // todo replace with logging
            if (inputLength < 1) throw new ArgumentOutOfRangeException(nameof(inputLength));
            if (choice <= 0) throw new ArgumentOutOfRangeException(nameof(choice));
            if (learningRate < 0 || learningRate > 1) throw new ArgumentOutOfRangeException(nameof(learningRate));
            if (vigilance < 0 || vigilance > 1) throw new ArgumentOutOfRangeException(nameof(vigilance));

            // add an uncommitted node
            // the last node will always be considered uncommitted
            Nodes.Add(Util.Ones(inputLength));

            // store the params for reference later
            InputLength = inputLength;
            Choice = choice;
            LearningRate = learningRate;
            Vigilance = vigilance;
        }

        public int Learn(double[] input)
        {
            // check input length
            // todo replace exception with log message
            if (input.Length != InputLength) throw new ArgumentException(null, nameof(input));

            // find a node to learn this input
            // the 'First' function will throw if none of the nodes pass the vigilance criterion
            // however, this should not occur bc the final node is "uncommitted" and should always pass
            // cache 'fuzInt' so we don't need to recalculate it during learning
            // store the 'id' so that we can return it to indicate which node learned the input
            var learnerQuery =
                from id in Enumerable.Range(0, Nodes.Count)
                let weights = Nodes[id]
                let fuzInt = input.FuzzyIntersection(weights)
                let fuzIntNorm = fuzInt.CityBlockNorm()
                orderby fuzIntNorm / (Choice + weights.CityBlockNorm())
                where fuzIntNorm / input.CityBlockNorm() >= Vigilance
                select (id, weights, fuzInt);
            var learner = learnerQuery.First();

            // if the uncommitted node was selected then a "reset" occurs
            // this means a new uncommitted node must be added
            if (learner.id == Nodes.Count - 1) Nodes.Add(Util.Ones(InputLength));

            // learn the input
            for (int i = 0; i < learner.weights.Length; i++)
            {
                learner.weights[i] =
                    (1 - LearningRate) * learner.weights[i] +
                    LearningRate * learner.fuzInt[i];
            }

            // return the ID (code) for the node which learned the input
            return learner.id;
        }
    }
}
