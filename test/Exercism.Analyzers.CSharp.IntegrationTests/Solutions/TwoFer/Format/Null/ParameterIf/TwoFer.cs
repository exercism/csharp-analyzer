using System;

public static class TwoFer
{
    public static string Name(string input = null)
    {
        if (input == null)
            input = "you";

        return String.Format("One for {0}, one for me.", input);
    }
}