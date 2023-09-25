using System.Text.RegularExpressions;

public static class Bob
{
    public static string Response(string message)
    {
        if (IsSilence(message))
            return "Fine. Be that way!";

        if (IsYell(message) && IsQuestion(message))
            return "Calm down, I know what I'm doing!";

        if (IsYell(message))
            return "Whoa, chill out!";

        if (IsQuestion(message))
            return "Sure.";

        return "Whatever.";
    }

    private static bool IsSilence(string message)
    {
        return Regex.IsMatch(message, @"^\s*$");
    }

    private static bool IsYell(string message)
    {
        return Regex.IsMatch(message, @"^[^\p{Ll}]*\p{Lu}[^\p{Ll}]*$");
    }

    private static bool IsQuestion(string message)
    {
        return Regex.IsMatch(message, @"\?\s*$");
    }
}
