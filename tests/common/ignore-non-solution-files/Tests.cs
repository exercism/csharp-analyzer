public class Tests
{
    public void Test()
    {
        // The long type is only used in this file. As we should be ignoring
        // any test file, we should _not_ see the tag for the long type
        // in the resulting analysis.json
        long x;
    }
}