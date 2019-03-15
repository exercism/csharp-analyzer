using System;
                
public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate) => birthDate.AddSeconds(Math.Pow(10, 9));
}