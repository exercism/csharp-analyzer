using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        const double seconds = 1000000000;
        return birthDate.AddSeconds(seconds);
    }
}