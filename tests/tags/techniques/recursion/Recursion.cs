public static class Recursion
{
    private static int MethodRecursion(int i)
    {
        if (i == 0)
            return 1;

        return i * MethodRecursion(i - 1);
    }
    
    private static int LocalMethodRecursion(int i)
    {
        int Inner(int j)
        {
            if (j == 0)
                return 1;

            return Inner(j - 1) * j;
        }

        return Inner(i);
    }
    
    private static int LocalAndOuterMethodRecursion(int i)
    {
        int Inner(int j)
        {
            if (j == 0)
                return 1;

            return LocalAndOuterMethodRecursion(j - 1) * j;
        }

        return Inner(i);
    }
    private static int NonRecursiveMethod(int i)
    {
        return i + 1;
    }
    
    private static int NonRecursiveLocalFunction(int i)
    {
        int Inner(int j)
        {
            return j + 1;
        }
        
        return Inner(i);
    }
}