using System;

public static class TwoFer
{
    public static string Name(string input = null)
    {
        return String.Format("One for {0}, one for me.", String.IsNullOrWhiteSpace(input) ?"you":input);
    }
}