using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        const double seconds = 1e9;
        return birthDate.AddSeconds(seconds);
    }
}