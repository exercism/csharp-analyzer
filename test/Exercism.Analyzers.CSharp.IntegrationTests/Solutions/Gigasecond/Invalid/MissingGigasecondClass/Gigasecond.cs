using System;

public static class Program
{
    public static DateTime Add(DateTime birthDate)
    {
        return birthDate.AddSeconds(1000000000);
    }
}