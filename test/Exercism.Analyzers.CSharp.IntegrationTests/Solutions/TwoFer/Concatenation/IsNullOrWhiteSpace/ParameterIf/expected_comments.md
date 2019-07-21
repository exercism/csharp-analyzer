[COMMENT #1]
Use [string interpolation](https://csharp.net-tutorials.com/operators/the-string-interpolation-operator/) to dynamically build a string, rather than using string concatenation. The main benefit is less "noise"; whereas string concatenation requires `+` to be added between each string, string interpolation has no such limitation. As a result, string interpolation code is usually a bit easier to read.

[COMMENT #2]
Rather than using `string.IsNullOrWhiteSpace` to determine if the default name should be used, consider trying the [null-coalescing operator](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator) to simplify the code and consider the difference in behaviour.
