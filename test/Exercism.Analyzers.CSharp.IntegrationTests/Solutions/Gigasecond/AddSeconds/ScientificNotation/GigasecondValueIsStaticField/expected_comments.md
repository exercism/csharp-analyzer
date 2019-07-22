[COMMENT #1]
Consider converting the `Seconds` field to a `const`, as the value is intended never to change. Using constants is not only more common, there are also some subtle differences between a `const` and a `field`, which are explained in [this StackOverflow post](https://stackoverflow.com/questions/755685/static-readonly-vs-const#755693).

[COMMENT #2]
As the `Add` method only has a single statement, consider converting the method to an [expression-bodied method](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members#methods).
