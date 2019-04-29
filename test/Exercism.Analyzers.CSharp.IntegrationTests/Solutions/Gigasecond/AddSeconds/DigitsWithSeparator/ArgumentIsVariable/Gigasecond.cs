using System;

public static class Gigasecond
{   
    public static DateTime Add(DateTime birthDate)
    {
        var gigasecond = 1_000_000_000;
        return birthDate.AddSeconds(gigasecond);
    }
}