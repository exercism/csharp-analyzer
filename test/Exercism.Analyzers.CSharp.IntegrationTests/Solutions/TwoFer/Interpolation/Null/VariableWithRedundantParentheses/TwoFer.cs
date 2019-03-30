using System;

public static class TwoFer
{
    public static string Name(string input = null)
    {
        var name = (input == null ? "you" : input);
        return $"One for {name}, one for me.";
    }
}