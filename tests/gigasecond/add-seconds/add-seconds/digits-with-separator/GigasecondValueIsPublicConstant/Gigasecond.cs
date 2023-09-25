using System;

public static class Gigasecond
{
    public const int Seconds = 1_000_000_000;
    
    public static DateTime Add(DateTime birthDate)
    {
        return birthDate.AddSeconds(Seconds);
    }
}