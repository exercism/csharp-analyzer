using System;
using System.Collections.Generic;

public static class ProteinTranslation
{
    public static string[] Proteins(string strand)
    {
        var length = strand.Length;
        List<String> proteins = new List<String>();
        var endIndex = 3;
        while (endIndex <= length)
        {
            var codon = strand.Substring(endIndex - 3, 3);
            var protein = ToProtein(codon);
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

    private static string ToProtein(string input) =>
        input switch
        {
            "AUG" => "Methionine",
            "UUU" => "Phenylalanine",
            "UUC" => "Phenylalanine",
            "UUA" => "Leucine",
            "UUG" => "Leucine",
            "UCU" => "Serine",
            "UCC" => "Serine",
            "UCA" => "Serine",
            "UCG" => "Serine",
            "UAU" => "Tyrosine",
            "UAC" => "Tyrosine",
            "UGU" => "Cysteine",
            "UGC" => "Cysteine",
            "UGG" => "Tryptophan",
            "UAA" => "STOP",
            "UAG" => "STOP",
            "UGA" => "STOP",
            _ => throw new Exception("Invalid sequence")
        };
}
