public class Editor
{
    public void Test()
    {
        // The uint type is only used in this file. As we should be ignoring
        // any editor file, we should _not_ see the tag for the uint type
        // in the resulting analysis.json
        uint x;
    }
}
