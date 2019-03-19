using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        return birthDate.AddSeconds(Math.Pow(10, 9));
    }
}