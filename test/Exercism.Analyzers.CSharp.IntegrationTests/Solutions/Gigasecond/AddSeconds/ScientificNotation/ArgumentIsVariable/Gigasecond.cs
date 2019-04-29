using System;

public static class Gigasecond
{   
    public static DateTime Add(DateTime birthDate)
    {
        var gigasecond = 1E9;
        return birthDate.AddSeconds(gigasecond);
    }
}