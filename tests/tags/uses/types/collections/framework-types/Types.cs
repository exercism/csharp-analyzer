using System;
using System.Collections.Generic;

public static class Types
{
    public static void Collections()
    {
        var explicitArray = new int[4];
        var jaggedArray = new int[3][];
        var multiDimensionalArray = new int[4, 2];
        var initializedArray = new[] { 0, 1, 2, 4 };
        var list = new List<int>();
        var dictionary = new Dictionary<int, int>();
        var hashSet = new HashSet<int>();
        var stack = new Stack<int>();
        var queue = new Queue<int>();
        var linkedList = new LinkedList<int>();
    }
    
    public static void SortedCollections()
    {
        var sortedList = new SortedList<int, int>();
        var sortedDictionary = new SortedDictionary<int, int>();
        var sortedSet = new SortedSet<int>();
    }

    public static void Other()
    {
        Span<char> span = new char[64];
        Memory<char> memory = new char[64];
        ReadOnlySpan<char> readOnlySpan = new char[64];
        ReadOnlyMemory<char> readOnlyMemory = new char[64];
    }
}