using System;
using System.Linq;

public class Program
{
    public void Test()
    {   
        var numbers = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        var passLambda = numbers.Where(x => x % 2 == 0);

        Func<int, bool> isOdd = x => x % 2 != 0;
        var passLocalFunction = numbers.Where(isOdd);
        
        var passMethod = numbers.Where(IsEven);
    }

    public bool IsEven(int i) => i % 2 == 0;

    public Func<int, int> ReturnFunction(int a) => (int b) => a + b;

    public bool FunctionParameter(Func<int, bool> filter, int i)
    {
        return filter(i);
    }
    
    public void ActionParameter(Action<int> callback, int i)
    {
        callback(i);
    }
    
    public int LocalFunction(int i)
    {
        int Inner() => i + 2;
        return Inner();
    }
}