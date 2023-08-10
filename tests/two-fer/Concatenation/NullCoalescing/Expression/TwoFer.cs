using System;

public static class TwoFer
{
    public static string Speak(string input = null) =>
        "One for " + (input ?? "you") + ", one for me.";
}