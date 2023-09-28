using System;
using System.Collections.Generic;

public static class ProteinTranslation
{
    private static readonly Dictionary<string, string> lookup = new Dictionary<string, string>();

    private static void roboLoad(string protein, params string[] codons)
    {
        foreach (string codon in codons)
            lookup.Add(codon, protein);
    }

    static ProteinTranslation()
    {
        roboLoad("Methionine", "AUG");
        roboLoad("Phenylalanine", "UUU", "UUC");
        roboLoad("Leucine", "UUA", "UUG");
        roboLoad("Serine", "UCU", "UCC", "UCA", "UCG");
        roboLoad("Tyrosine", "UAU", "UAC");
        roboLoad("Cysteine", "UGU", "UGC");
        roboLoad("Tryptophan", "UGG");
        roboLoad("STOP", "UAA", "UAG", "UGA");
    }

    public static string[] Proteins(string strand)
    {
        var length = strand.Length;
        List<String> proteins = new List<String>();
        var endIndex = 3;
        while (endIndex <= length)
        {
            var codon = strand.Substring(endIndex - 3, 3);
            var protein = lookup[codon];
            switch (protein)
            {
                case "STOP":
                    return proteins.ToArray();
                default:
                    proteins.Add(protein);
                    break;
            }
            endIndex += 3;
        }
        return proteins.ToArray();
    }
}
