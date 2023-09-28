using System;

public static class DifferenceOfSquares
{
    public static int CalculateSquareOfSum(int max)
        => (int)Math.Pow((max * (max + 1)) / 2, 2);

    public static int CalculateSumOfSquares(int max)
        => (max * (max + 1) * (2 * max + 1)) / 6;

    public static int CalculateDifferenceOfSquares(int max)
        => Math.Abs(CalculateSumOfSquares(max) - CalculateSquareOfSum(max));
}