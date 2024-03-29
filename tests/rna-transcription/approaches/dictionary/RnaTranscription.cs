using System.Collections.Generic;
using System.Linq;

public static class RnaTranscription
{
    public static string ToRna(string dna) =>
        new(dna.Select(nucleotide => Complements[nucleotide]).ToArray());

    private static readonly Dictionary<char, char> Complements =
        new() { ['G'] = 'C', ['C'] = 'G', ['T'] = 'A', ['A'] = 'U' };
}
