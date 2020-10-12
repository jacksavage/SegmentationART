using SegmentationART;
using System;
using System.Diagnostics;

var phonemes = DataReader.ReadPhonemeLookup(@"data\phonemes.tsv");
var words = DataReader.ReadWordLookup(phonemes, @"data\words.tsv");

var phonemeArt =
    new FuzzyArt(
        inputLength: 14,    // input vectors are 7 dimensional and complement coded
        choice: 0.1,        // ?
        learningRate: 1.0,  // fast learning
        vigilance: 0.95     // one node per phoneme
    );

Console.WriteLine("Training phoneme-ART");
foreach (var phoneme in phonemes)
    Console.WriteLine($"{phoneme.Key} : {phonemeArt.Learn(phoneme.Value)}");

Debugger.Break();
