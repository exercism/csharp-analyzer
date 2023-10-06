public class Example
{
    public void Test()
    {
        // The ulong type is only used in this file. As we should be ignoring
        // any example file, we should _not_ see the tag for the ulong type
        // in the resulting analysis.json
        ulong x;
    }
}
