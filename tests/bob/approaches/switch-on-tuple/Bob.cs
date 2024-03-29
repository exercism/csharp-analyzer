using System.Linq;

public static class Bob
{
    private static bool IsShout(string input) => input.Any(c => char.IsLetter(c)) && input.ToUpper() == input;

    public static string Response(string statement)
    {
        var input = statement.TrimEnd();
        if (input == "")
            return "Fine. Be that way!";

        switch ((input.EndsWith('?'), IsShout(input)))
        {
            case (true, true): return "Calm down, I know what I'm doing!";
            case (_, true): return "Whoa, chill out!";
            case (true, _): return "Sure.";
            default: return "Whatever."; 
        }
    }
}
