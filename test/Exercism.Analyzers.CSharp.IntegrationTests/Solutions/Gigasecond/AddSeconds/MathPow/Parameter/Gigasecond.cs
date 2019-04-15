using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        birthDate = birthDate.AddSeconds(Math.Pow(10, 9));
        return birthDate;
    }
}