using System;
                
public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate) => birthDate.Add(TimeSpan.FromSeconds(1000000000));
}