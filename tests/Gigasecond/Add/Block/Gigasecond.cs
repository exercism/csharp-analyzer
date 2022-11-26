using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        return birthDate.Add(TimeSpan.FromSeconds(1000000000));
    }
}