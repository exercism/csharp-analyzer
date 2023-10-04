using System;

public static class CollatzConjecture
{
    public static int Steps(int number)
    {
        if (number <= 0)
            throw new ArgumentOutOfRangeException(nameof(number));

        var currentNumber = number;
        var stepCount = 0;

        while (currentNumber != 1)
        {
            if (currentNumber % 2 == 0)
                currentNumber = currentNumber / 2;
            else
                currentNumber = currentNumber * 3 + 1;

            stepCount++;
        }

        return stepCount;
    }
}
