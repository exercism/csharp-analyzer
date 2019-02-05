using System;

public static class Gigasecond
{
    public static DateTime Add(DateTime birthDate)
    {
        // This is some comment on the functionality
        return birthDate.AddSeconds(1e9);   
        // And some more comments
    }
}