using System.Linq;

public static class Pangram
{
    private const string Letters = "abcdefghijklmnopqrstuvwxyz";
    
    public static bool IsPangram(string input)
    {
        var lowerCaseInput = input.ToLower();
        return Letters.All(letter => lowerCaseInput.Contains(letter));
    }
}
