using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        return birthDate + TimeSpan.FromSeconds(1_000_000_000);
    }
}
