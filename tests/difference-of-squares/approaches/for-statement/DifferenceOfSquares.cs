public static class DifferenceOfSquares
{
    public static int CalculateSquareOfSum(int max)
    {
        var sum = 0;

        for (var i = 1; i <= max; i++)
            sum += i;

        return sum * sum;
    }

    public static int CalculateSumOfSquares(int max)
    {
        var sumOfSquares = 0;

        for (var i = 1; i <= max; i++)
            sumOfSquares += i * i;

        return sumOfSquares;
    }

    public static int CalculateDifferenceOfSquares(int max) =>
        CalculateSquareOfSum(max) - CalculateSumOfSquares(max);
}
