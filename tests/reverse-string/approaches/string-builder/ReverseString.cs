using System.Text;

public static class ReverseString
{
    public static string Reverse(string input)
    {
        var chars = new StringBuilder();
        for (var i = input.Length - 1; i >= 0; i--)
        {
            chars.Append(input[i]);
        }
        return chars.ToString();
    }
}
