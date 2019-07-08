[COMMENT #1]
As the `Seconds` field is only used within its class, its visibility can, and almost always should, be set to `private`.

[COMMENT #2]
As the `Add` method only has a single statement, consider converting the method to an [expression-bodied method](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members#methods).