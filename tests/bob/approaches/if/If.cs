using System.Linq;

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
        return string.IsNullOrWhiteSpace(message);
    }

    private static bool IsYell(string message)
    {
        return message.Any(char.IsLetter) && message.ToUpperInvariant() == message;
    }

    private static bool IsQuestion(string message)
    {
        return message.TrimEnd().EndsWith("?");
    }
}
