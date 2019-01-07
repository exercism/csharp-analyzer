using System;

public static class Shared
{
    public static string Number() => DateTime.Now.AddSeconds(1e6).ToString();
}