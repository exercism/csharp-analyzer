using System;

public static class Gigasecond
{
//    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(1000000000);
    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(Math.Pow(2, 9));
}