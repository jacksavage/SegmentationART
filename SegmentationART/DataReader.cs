﻿using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SegmentationART
{
    internal class DataReader
    {
        private record Phoneme(char ID, double[] Vector);

        private static IEnumerable<Phoneme> ReadPhonemes(string path) =>
            from line in File.ReadLines(path)
            let fields = line.Split("\t")
            let id = string.IsNullOrWhiteSpace(fields[1]) ? fields[0] : fields[1]
            let vector = fields.Skip(2).Take(7).Select(f => double.Parse(f)).ToArray()
            select new Phoneme(id[0], vector.ComplementCode());

        public static Dictionary<char, double[]> ReadPhonemeLookup(string path) =>
            ReadPhonemes(path)
            .ToDictionary(p => p.ID, p => p.Vector);

        private record Word(string ID, string Phonemes);

        private static IEnumerable<Word> ReadWords(string path) =>
            from line in File.ReadLines(path)
            let fields = line.Split("\t")
            select new Word(fields[0], fields[1]);

        public static Dictionary<string, string[]> ReadWordLookup(Dictionary<char, double[]> phonemes, string wordPath)
        {
            var words =
                ReadWords(wordPath)
                .ToDictionary(
                    w => w.ID,
                    w => w.Phonemes.Split(" ").ToArray()
                );

            return words;
        }
    }
}
