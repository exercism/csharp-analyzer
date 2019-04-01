using System;

public static class TwoFer
{
    public static string Name(string input = null)
    {
        input = string.IsNullOrEmpty(input) ? "you" : input;

        return string.Format("One for {0}, one for me.", input);
    }
}