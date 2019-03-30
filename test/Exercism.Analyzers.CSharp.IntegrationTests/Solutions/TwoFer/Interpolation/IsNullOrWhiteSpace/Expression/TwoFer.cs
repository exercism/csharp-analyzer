using System;

public static class TwoFer
{
    public static string Name(string input = null) =>
        $"One for {(string.IsNullOrWhiteSpace(input) ? "you" : input)}, one for me.";
}