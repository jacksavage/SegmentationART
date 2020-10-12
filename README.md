# Segmentation ART
An implementation of Segmentation ART per the [1998 paper from Carpenter &amp; Wilson](https://open.bu.edu/handle/2144/2351)

This implementation is a work in progress. Currently provided are

* the lexicon data parsed from the paper with ORC + manual corrections and stored in tsv files
* a reader for that data
* a basic implementation of FuzzyART (using [LINQ](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/))
* a basic implementation of working memory
* a top level program to instantiate the three fuzzy ART modules, read the lexicon, and train phoneme-ART & syllable-ART on it

Still working on

* finishing the training process for word-ART
* implementing performance mode where FuzzyArt makes a hypothesis, feeds it a the WorkingMemory reset module, and also checks if a replay needs to occur
