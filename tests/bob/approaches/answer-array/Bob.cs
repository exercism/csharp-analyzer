using System.Linq;

public static class Bob
{
    private static readonly string[] answers = {"Whatever.", "Sure.", "Whoa, chill out!", "Calm down, I know what I'm doing!"};
    
    public static string Response(string statement)
    {
        var input = statement.TrimEnd();
        if (input == "")
            return "Fine. Be that way!";

        var isShout = input.Any(c => char.IsLetter(c)) && input.ToUpper() == input ? 2: 0;
        
        var isQuestion = input.EndsWith('?') ? 1: 0;

        return answers[isShout + isQuestion];
    }
}
