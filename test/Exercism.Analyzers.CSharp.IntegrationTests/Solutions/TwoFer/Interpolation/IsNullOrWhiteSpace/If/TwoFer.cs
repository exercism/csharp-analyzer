using System;

public static class TwoFer
{
    public static string Name(string input = null)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return "One for you, one for me.";
        }
        else
            return $"One for {name}, one for me.";
    }
}