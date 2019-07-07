[COMMENT #1]
Consider writing `Math.Pow(10, 9)` as `1e9`.

[COMMENT #2]
Consider converting the `gigasecond` variable to a `const`, as the value is intended never to change. Usually, `const` values are defined at the class level, as they are frequently used in multiple methods. In this case, the `const` value can be defined within the method itself, which means that other methods cannot use its value.