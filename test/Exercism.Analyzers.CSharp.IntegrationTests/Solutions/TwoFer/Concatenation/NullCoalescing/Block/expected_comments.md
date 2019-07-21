[COMMENT #1]
Use [string interpolation](https://csharp.net-tutorials.com/operators/the-string-interpolation-operator/) to dynamically build a string, rather than using string concatenation. The main benefit is less "noise"; whereas string concatenation requires `+` to be added between each string, string interpolation has no such limitation. As a result, string interpolation code is usually a bit easier to read.

[COMMENT #2]
As the `Speak` method only has a single statement, consider converting the method to an [expression-bodied method](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members#methods).
