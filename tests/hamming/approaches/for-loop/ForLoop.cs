using System;

public static class Hamming
{
    public static int Distance(string strand1, string strand2)
    {
        if (strand1.Length != strand2.Length)
            throw new ArgumentException("Strands have different length");

        var distance = 0;

        for (var i = 0; i < strand1.Length; i++)
        {
            if (strand1[i] != strand2[i])
                distance++;
        }

        return distance;
    }
}
