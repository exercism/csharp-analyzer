using System;

public static class CollatzConjecture
{
    public static int Steps(int number)
    {
        if (number <= 0)
            throw new ArgumentOutOfRangeException(nameof(number));

        return Steps(number, 0);
    }

    private static int Steps(int number, int stepCount)
    {
        if (number == 1)
            return stepCount;

        if (number % 2 == 0)
            return Steps(number / 2, stepCount + 1);

        return Steps(number * 3 + 1, stepCount + 1);
    }
}
