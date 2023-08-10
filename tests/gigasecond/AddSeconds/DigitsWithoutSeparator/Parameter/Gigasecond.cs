using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        birthDate=birthDate.AddSeconds(1000000000);
        return birthDate;
    }
}