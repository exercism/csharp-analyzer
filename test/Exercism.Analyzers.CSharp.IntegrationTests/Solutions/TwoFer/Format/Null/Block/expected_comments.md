[COMMENT #1]
A more common approach in C# is to use [string interpolation](https://csharp.net-tutorials.com/operators/the-string-interpolation-operator/), rather than using `string.Format` to dynamically build a string.

Note that string interpolation is just a compiler trick, also known as syntactic sugar. This means that when a string interpolation expression is compiled, the compiler will actually convert it to a `string.Format` call. The benefit of using string interpolation is thus purely visual.

[COMMENT #2]
As the `Speak` method only has a single statement, consider converting the method to an [expression-bodied method](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members#methods).
