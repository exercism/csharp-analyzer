using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        birthDate = birthDate.AddSeconds(1e9); 
        return (birthDate);
    }
}