using System;

public static class TwoFer
{
    public static string Speak(string input = null)
    {
        string name = string.IsNullOrEmpty(input) ? "you" : input;
        string[] sentence = { "One for ", name, ", one for me."};

        return String.Join("", sentence);
    }
}
