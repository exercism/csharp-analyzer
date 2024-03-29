using System;

public static class Darts
{
    public static int Score(double x, double y)
    {
        var distanceFromCenter = Math.Sqrt(x * x + y * y);

        if (distanceFromCenter > 10.0)
            return 0;

        if (distanceFromCenter > 5.0)
            return 1;

        if (distanceFromCenter > 1.0)
            return 5;

        return 10;
    }
}
