namespace DefaultNamespace;

using System;

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

            j = +j;
            j = -j;

            ++j;
            --j;
            j--;
        }

        var a = !true;
        var b = true | false;
        var c = true & false;
        var d = true ^ false;
        var e = true || true;
        var f = true && true;
        var g = ""?.ToString();
        var h = ""!.ToString();
        var z = "" ?? "a";
        var za = a == b;
        var zb = a != b;
        
        Console.WriteLine(default(int));
        Console.WriteLine(nameof(i));

        return 0;
    }
}
