using SegmentationART;

var wordPhonemes = 
    DataReader.ReadWords(
        phonemePath: @"data\phonemes.tsv", 
        wordPath: @"data\words.tsv"
    );

var phonemeArt =
    new FuzzyArt(
        inputLength: 14,    // input vectors are 7 dimensional and complement coded
        choice: 0.1,        // ?
        learningRate: 1.0,  // fast learning
        vigilance: 0.95     // one node per phoneme
    );

