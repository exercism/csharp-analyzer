using System;

public static class TwoFer
{
    public static string Speak(string input = "you")
    {
        return string.Concat("One for ", input, ", one for me.");
    }
}
