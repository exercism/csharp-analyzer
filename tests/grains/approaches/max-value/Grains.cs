using System;

public static class Grains
{
    public static double Square(int i)
    {
        if (i is <= 0 or > 64)
            throw new ArgumentOutOfRangeException(nameof(i));

        return Math.Pow(2, i - 1);
    }

    public static double Total()
    {
        return System.UInt64.MaxValue;
    }
}