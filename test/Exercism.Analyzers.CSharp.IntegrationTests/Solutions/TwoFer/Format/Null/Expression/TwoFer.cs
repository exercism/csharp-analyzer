using System;

public static class TwoFer
{
    public static string Name(string input = null) =>
        String.Format("One for {0}, one for me.", input == null ? "you": input);
}