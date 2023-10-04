namespace DefaultNamespace;

public static class Program
{
    public static int Main()
    {
        var i = 6 > 5 ? 3 : 4;

        for (var j = 0; j < 5; j++)
        {
            if (j == 0)
            {
                continue;
            }
            else if (j == 4)
            {
                break;
            }
        }

        return 0;
    }
}
