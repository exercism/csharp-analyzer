using System.Collections.Generic;
using System.Linq;

public static class ParallelLetterFrequency
{
    public static Dictionary<char, int> Calculate(IEnumerable<string> texts)
    {
        return texts.AsParallel().Aggregate(new Dictionary<char, int>(), AddCount);
    }

    private static Dictionary<char, int> AddCount(Dictionary<char, int> letterCounts, string text)
    {
        foreach (var letterCount in text.ToLower().Where(char.IsLetter).GroupBy(c => c))
        {
            if (letterCounts.TryGetValue(letterCount.Key, out var count))
                letterCounts[letterCount.Key] = letterCount.Count() + count;
            else
                letterCounts[letterCount.Key] = letterCount.Count();
        }

        return letterCounts;
    }
}
