using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        const double seconds = 1_000_000_000;
        return birthDate.AddSeconds(seconds);
    }
}