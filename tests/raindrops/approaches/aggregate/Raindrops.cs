using System.Linq;

public static class Raindrops
{
    private static readonly (int, string)[]  drips = { (3, "Pling"), (5, "Plang"), (7, "Plong") };

    public static string Convert(int number)
    {
        var drops = drips.Aggregate("", (acc, drop) => number % drop.Item1 == 0 ? acc + drop.Item2 : acc);
        return drops.Length > 0 ? drops : number.ToString();
    }
}
