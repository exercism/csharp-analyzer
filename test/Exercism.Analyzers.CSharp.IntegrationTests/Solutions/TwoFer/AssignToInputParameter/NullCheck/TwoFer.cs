using System;

public static class TwoFer
{
    public static string Name(string input = null)
    {
        if (input == null)
            input = "you";
        return $"One for {input}, one for me.";
    }
}