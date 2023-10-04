using System;
using System.Linq;
using System.Collections.Generic;

public static class PalindromeProducts
{
    public static (int, IEnumerable<(int,int)>) Largest(int min, int max)
    {
        var largestProduct = int.MinValue;
        var factors = new List<(int, int)>();

        foreach(var (a, b) in ReversedFactorPairsInRange(min, max)) 
        {
            var product = a * b;
            if (a == b && product < largestProduct)
                break;

			if (IsPalindrome(product))
			{
				if (product > largestProduct)
				{
					largestProduct = product;
					factors.Clear();
				}
				if (product == largestProduct) 
				{
					factors.Add((a, b));
				}
			}
        }

        if (factors.Count == 0)
			throw new ArgumentException("No palindromes in the range");

		return (largestProduct, factors.ToArray());
    }

    public static (int, IEnumerable<(int,int)>) Smallest(int min, int max)
	{
		var smallestProduct = int.MaxValue;
		var factors = new List<(int, int)>();

		foreach(var (a, b) in FactorPairsInRange(min, max)) 
        {
			if (a > smallestProduct)
				break;

			var product = a * b;

			if (IsPalindrome(product))
			{
				if (product < smallestProduct)
				{
					smallestProduct = product;
					factors.Clear();
				}
				if (product == smallestProduct) 
				{
					factors.Add((a, b));
				}
			}
		}

		if (factors.Count == 0)
			throw new ArgumentException("No palindromes in the range");

		return (smallestProduct, factors.ToArray());
	}

    private static IEnumerable<(int, int)> ReversedFactorPairsInRange(int min, int max) 
    {
        for (var a = max; a >= min; --a)
            for (var b = a; b >= min; --b)
            yield return (b, a);
    }

    private static IEnumerable<(int, int)> FactorPairsInRange(int min, int max) 
    {
        for (var a = min; a <= max; ++a)
            for (var b = a; b <= max; ++b)
            yield return (a, b);
    }

    private static bool IsPalindrome(int number)
    =>  number.ToString().Equals(
        new String(number.ToString().Reverse().ToArray())
        );
}
