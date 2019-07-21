[COMMENT #1]
Use [string interpolation](https://csharp.net-tutorials.com/operators/the-string-interpolation-operator/) to dynamically build a string, rather than using string concatenation. The main benefit is less "noise"; whereas string concatenation requires `+` to be added between each string, string interpolation has no such limitation. As a result, string interpolation code is usually a bit easier to read.

[COMMENT #2]
Use the [null-coalescing operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator) to simplify your code, rather than explicitly checking for a value to equal `null` and then doing something with it.
