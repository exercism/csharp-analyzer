namespace DefaultNamespace;

public class Person
{
    public void VoidReturning()
    {
    }
    
    public void Overload1()
    {
    }
    
    public void Overload2()
    {
    }

    public int Parameters(int x, int y)
    {
        return x + y;
    }

    public int DefaultValue(int x = 0)
    {
        return x;
    }
    
    public void Varargs(params int[] xs)
    {
    }
    
    public int Returning()
    {
        return 1;
    }
    
    public int NamedArgument()
    {
        return DefaultValue(x: 2);
    }

    public int LocalFunction()
    {
        int InnerFunction()
        {
            return 2;
        }

        return InnerFunction();
    }
}
