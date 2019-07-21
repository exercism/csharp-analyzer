[COMMENT #1]
A more common approach in C# is to use [string interpolation](https://csharp.net-tutorials.com/operators/the-string-interpolation-operator/), rather than using `string.Format` to dynamically build a string.

Note that string interpolation is just a compiler trick, also known as syntactic sugar. This means that when a string interpolation expression is compiled, the compiler will actually convert it to a `string.Format` call. The benefit of using string interpolation is thus purely visual.

[COMMENT #2]
Use the [null-coalescing operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator) to simplify your code, rather than explicitly checking for a value to equal `null` and then doing something with it.
