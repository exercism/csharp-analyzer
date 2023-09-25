public static class Isogram
{
    public static bool IsIsogram(string word) {
        int letter_flags = 0;
        foreach (char letter in word)
        {
            if (letter >= 'a' && letter <= 'z')
            {
                if ((letter_flags & (1 << (letter - 'a'))) != 0)
                    return false;
                else
                    letter_flags |= (1 << (letter - 'a'));
            }
            else if (letter >= 'A' && letter <= 'Z')
            {
                if ((letter_flags & (1 << (letter - 'A'))) != 0)
                    return false;
                else
                    letter_flags |= (1 << (letter - 'A'));
            }
        }
        return true;    
    }
}
