using System;

public static class Gigasecond
{   
    public static DateTime Add(DateTime birthDate)
    {
        var gigasecond = Math.Pow(10, 9);
        return birthDate.AddSeconds(gigasecond);
    }
}