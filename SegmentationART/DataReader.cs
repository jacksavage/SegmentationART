using System.Collections.Generic;
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
            let vector = fields.Skip(2).Select(f => double.Parse(f)).ToArray()
            select new Phoneme(id[0], vector);

        public static Dictionary<char, double[]> ReadPhonemeLookup(string path) =>
            ReadPhonemes(path)
            .ToDictionary(p => p.ID, p => p.Vector);

        private record Word(string ID, string Phonemes);

        private static IEnumerable<Word> ReadWords(string path) =>
            from line in File.ReadLines(path)
            let fields = line.Split("\t")
            select new Word(fields[0], fields[1]);

        public static Dictionary<string, double[][]> ReadWordLookup(Dictionary<char, double[]> phonemes, string wordPath)
        {
            var words =
                ReadWords(wordPath)
                .ToDictionary(
                    w => w.ID,
                    w => w.Phonemes.Select(p => phonemes[p].ComplementCode()).ToArray()
                );

            return words;
        }
    }
}
