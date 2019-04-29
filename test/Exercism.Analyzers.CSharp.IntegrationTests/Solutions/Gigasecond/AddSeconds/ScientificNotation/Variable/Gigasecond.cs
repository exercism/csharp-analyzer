using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        var date = birthDate.AddSeconds(1e9); 
        return (date);
    }
}