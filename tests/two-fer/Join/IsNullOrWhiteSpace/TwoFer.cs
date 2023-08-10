public static class TwoFer
{
    public static string Speak(string input = null)
    {
        string name = string.IsNullOrWhiteSpace(input) ? "you" : input;
        string[] quote = { "One for ", name, ", one for me."};

        return System.String.Join("", quote);
    }
}
