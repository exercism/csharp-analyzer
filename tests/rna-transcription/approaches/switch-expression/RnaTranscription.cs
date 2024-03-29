using System.Linq;

public static class RnaTranscription
{
    public static string ToRna(string dna) =>
        new(dna.Select(Complement).ToArray());

    private static char Complement(char nucleotide) =>
        nucleotide switch
        {
            'G' => 'C',
            'C' => 'G',
            'T' => 'A',
            'A' => 'U'
        };
}
