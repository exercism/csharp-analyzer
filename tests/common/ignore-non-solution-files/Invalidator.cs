public class Invalidator
{
    public void Test()
    {
        // The ushort type is only used in this file. As we should be ignoring
        // any invalidator file, we should _not_ see the tag for the ushort type
        // in the resulting analysis.json
        ushort x;
    }
}