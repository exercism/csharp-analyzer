using System;

public static class Gigasecond
{
    public const double Seconds = 1e9;
    
    public static DateTime Add(DateTime birthDate)
    {
        return birthDate.AddSeconds(Seconds);
    }
}