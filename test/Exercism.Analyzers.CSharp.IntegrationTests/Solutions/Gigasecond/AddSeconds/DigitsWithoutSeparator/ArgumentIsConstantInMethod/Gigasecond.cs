using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        const int seconds = 1000000000;
        return birthDate.AddSeconds(seconds);
    }
}