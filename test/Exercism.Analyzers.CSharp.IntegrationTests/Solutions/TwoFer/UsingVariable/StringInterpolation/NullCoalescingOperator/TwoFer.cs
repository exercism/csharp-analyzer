using System;

public static class TwoFer
{
    public static string Name(string input = null)
    {
        var name = input ?? "you";
        return $"One for {name}, one for me.";
    }
}