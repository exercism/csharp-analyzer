using System.Linq;

public static class AssignAndReturn
{
    public static int AssignInMethod(int year)
    {
        var x = 1;
        return x;
    }
    
    public static int AssignInBlock(int year)
    {
        if (true)
        {
            var x = 1;
            return x;    
        }

        return 2;
    }
    
    public static int AssignInLambda(int year)
    {
        return Enumerable.Range(1, 10).Select((i) =>
        {
            var x = i;
            return x;
        }).Max();
    }
}