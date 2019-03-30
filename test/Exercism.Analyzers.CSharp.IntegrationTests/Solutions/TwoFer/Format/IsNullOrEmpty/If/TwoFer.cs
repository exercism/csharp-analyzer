using System;

public static class TwoFer
{
    public static string Name(string input = null)
    {
        if (string.IsNullOrEmpty(input))
            return "One for you, one for me.";

        return string.Format("One for {0}, one for me.", input);
    }
}