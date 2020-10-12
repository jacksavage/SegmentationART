using SegmentationART;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

var phonemes = DataReader.ReadPhonemeLookup(@"data\phonemes.tsv");
var words = DataReader.ReadWordLookup(phonemes, @"data\words.tsv");
var syllables = words.SelectMany(w => w.Value).Distinct();

// instantiate phoneme ART
var phonemeArt =
    new FuzzyArt(
        inputLength: 14,    // input vectors are 7 dimensional and complement coded
        choice: 0.1,        // todo ?
        learningRate: 1.0,  // fast learning
        vigilance: 0.95     // one node per phoneme
    );

// train phoneme ART
Console.WriteLine("Training phoneme-ART");
foreach (var phoneme in phonemes)
    phonemeArt.Learn(phoneme.Value);

// instantiate phoneme memory
var phonemeMem = 
    new WorkingMemory(
        inputSize: phonemeArt.Nodes.Count - 1,  // number of phonemes that phoneme ART knows
        q: 0.5                                  // todo ?
    );

// instantiate syllable ART
var syllableArt =
    new FuzzyArt(
        inputLength:phonemeMem.Output.Length,
        choice: 0.1,
        learningRate: 1.0,
        vigilance: 0.95
    );

// train syllable ART
Console.WriteLine("Training syllable-ART");
foreach (var syllable in syllables)
{
    foreach (var phoneme in syllable)
    {
        var input = phonemes[phoneme];
        var phonemeCode = phonemeArt.Predict(input);
        phonemeMem.Add(phonemeCode);
    }

    syllableArt.Learn(phonemeMem.Output);
    phonemeMem.Reset();
}

// instantiate syllable memory
var syllableMem =
    new WorkingMemory(
        inputSize: syllableArt.Nodes.Count - 1,  // number of syllables that syllable ART knows
        q: 0.5
    );

// instantiate word ART
var wordArt =
    new FuzzyArt(
        inputLength: syllableMem.Output.Length,
        choice: 0.1,
        learningRate: 1.0,
        vigilance: 0.95
    );

// train word ART
Console.WriteLine("Training word-ART");
foreach (var word in words)
{
    foreach (var phoneme in word.Value.SelectMany(s => s))
    {
        var phonemeInput = phonemes[phoneme];
        var phonemeCode = phonemeArt.Predict(phonemeInput);
        phonemeMem.Add(phonemeCode);
        
        // todo syllable ART has to predict multple syllables while this is happening?
        // or just after it?
    }

    var wordCode = wordArt.Learn(phonemeMem.Output);
    syllableMem.Reset();

    Console.WriteLine($"{word.Key,-15} [{string.Join(" ", word.Value)}] ({wordCode})");
}

Debugger.Break();
