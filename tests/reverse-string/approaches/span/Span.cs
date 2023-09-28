Span<char> chars = stackalloc[input.Length];
for (var i = 0; i < input.Length; i++)
{
    chars[input.Length - 1 - i] = input[i];
}
return new string(chars);
