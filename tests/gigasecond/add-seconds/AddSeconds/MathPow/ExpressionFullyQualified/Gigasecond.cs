using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(System.Math.Pow(10, 9));
}