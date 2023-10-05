public static class Pangram
{
    public static bool IsPangram(string input) {
        int phrasemask = 0;
        foreach (char letter in input)
        {
            if (letter > 96 && letter < 123)
                phrasemask |= 1 << (letter - 'a');
            else if (letter > 64 && letter < 91)
                phrasemask |= 1 << (letter - 'A');
        }
        return phrasemask == 67108863;
    }
}
