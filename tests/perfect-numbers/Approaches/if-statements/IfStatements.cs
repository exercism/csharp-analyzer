using System;
using System.Linq;

public enum Classification
{
    Perfect,
    Abundant,
    Deficient
}

public class PerfectNumbers
{
    public static Classification Classify(int number)
    {
        if (number < 1)
            throw new ArgumentOutOfRangeException(nameof(number));

        var sumOfFactors = Enumerable.Range(1, number / 2)
            .Where(factor => number % factor == 0)
            .Sum();

        if (sumOfFactors < number)
            return Classification.Deficient;

        if (sumOfFactors > number)
            return Classification.Abundant;

        return Classification.Perfect;
    }
}
