using System;

public static class TwoFer
{
    public static string Name(string input = "you")
    {
        if (input == null)
            input = "you";
        return $"One for {input}, one for me.";
    }
}