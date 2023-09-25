public static class Pangram
{
    public static bool IsPangram(string input) {
        int phrasemask = 0;
        foreach (char letter in input)
        {
            // a-z
            if (letter > 96 && letter < 123)
                phrasemask |= 1 << (letter - 'a');
            // A - Z
            else if (letter > 64 && letter < 91)
                phrasemask |= 1 << (letter - 'A');
        }
        //26 binary 1s
        return phrasemask == 67108863;        
    }
}
