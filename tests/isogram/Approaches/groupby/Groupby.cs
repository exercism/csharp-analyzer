using System.Linq;

public static class Isogram
{
    public static bool IsIsogram(string word)
    {
        return word.ToLower().Where(Char.IsLetter).GroupBy(ltr => ltr).All(ltr_grp => ltr_grp.Count() == 1);
    }
}
