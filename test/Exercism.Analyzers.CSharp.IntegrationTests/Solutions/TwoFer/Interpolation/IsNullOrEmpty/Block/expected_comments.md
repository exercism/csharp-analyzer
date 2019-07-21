[COMMENT #1]
Rather than using `string.IsNullOrEmpty` to determine if the default name should be used, consider trying the [null-coalescing operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator) to simplify the code and consider the difference in behaviour.

[COMMENT #2]
As the `Speak` method only has a single statement, consider converting the method to an [expression-bodied method](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members#methods).
