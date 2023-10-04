using System;
using System.Collections.Generic;
using System.Linq;

public static class CollatzConjecture
{
    public static int Steps(int number)
    {
        if (number <= 0)
            throw new ArgumentOutOfRangeException(nameof(number));

        return Sequence(number).Count();
    }

    private static IEnumerable<int> Sequence(int number)
    {
        var currentNumber = number;

        while (currentNumber != 1)
        {
            if (currentNumber % 2 == 0)
                currentNumber = currentNumber / 2;
            else
                currentNumber = currentNumber * 3 + 1;

            yield return currentNumber;
        }
    }
}
