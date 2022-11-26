using System;

public static class Gigasecond
{
    public static void Add(DateTime birthDate)
    {
        var x = birthDate.AddSeconds(1e9);
    }
}