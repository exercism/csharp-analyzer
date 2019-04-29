using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        var date = birthDate.AddSeconds(1_000_000_000);
        return date;
    }
}