using System;

public static class Gigasecond
{
    private static readonly double Seconds = Math.Pow(10, 9);
    
    public static DateTime Add(DateTime birthDate)
    {
        return birthDate.AddSeconds(Seconds);
    }
}