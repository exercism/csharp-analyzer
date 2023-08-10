using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        var ret = (birthDate.AddSeconds(Math.Pow(10, 9)));
        return ret;
    }
}