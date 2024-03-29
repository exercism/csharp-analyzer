using System;
using System.Collections.Generic;
using System.Linq;

public enum Classification
{
    Perfect,
    Abundant,
    Deficient
}

public static class PerfectNumbers
{
    public static Classification Classify(int number)
    {
        var sum = Enumerable.Range(1, number - 1)
            .Sum(n => number % n == 0 ? n : 0);
            
        return (sum < number, sum > number) switch
        {
            (true, _) => Classification.Deficient,
            (_, true) => Classification.Abundant,
            _ => Classification.Perfect,
        };
    }
}
