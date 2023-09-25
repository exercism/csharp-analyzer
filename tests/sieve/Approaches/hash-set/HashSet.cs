using System;
using System.Collections.Generic;

public static class Sieve
{
    public static IEnumerable<int> Primes(int max)
    {
        if (max < 0)
            throw new ArgumentOutOfRangeException(nameof(max));

        var primeMultiples = new HashSet<int>();

        for (var i = 2; i <= max; i++)
        {
            if (primeMultiples.Contains(i))
                continue;

            for (var j = i * 2; j <= max; j += i)
                primeMultiples.Add(j);

            yield return i;
        }
    }
}
