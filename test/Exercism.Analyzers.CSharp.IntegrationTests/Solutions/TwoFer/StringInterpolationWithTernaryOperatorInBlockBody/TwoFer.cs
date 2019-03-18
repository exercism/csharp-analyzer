using System;

public static class TwoFer
{
    public static string Name(string input = null)
    {
        return $"One for {input == null ? "you" : input}, one for me.";
    }
}