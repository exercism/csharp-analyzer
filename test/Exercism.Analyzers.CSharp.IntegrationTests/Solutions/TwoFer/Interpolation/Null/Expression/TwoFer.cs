using System;

public static class TwoFer
{
    public static string Name(string input = null) =>
        $"One for {(input == null ? "you" : input)}, one for me.";
}