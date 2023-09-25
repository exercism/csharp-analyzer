public static class DifferenceOfSquares
{
    public static int CalculateSquareOfSum(int max)
    {
        var sum = max * (max + 1) / 2;
        return sum * sum;
    }

    public static int CalculateSumOfSquares(int max) =>
        (max * (max + 1) * ((max * 2) + 1)) / 6;

    public static int CalculateDifferenceOfSquares(int max) =>
        CalculateSquareOfSum(max) - CalculateSumOfSquares(max);
}
