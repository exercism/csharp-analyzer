public class Additional
{
    public void Test()
    {
        // The short type is only used in this file. As we should be including
        // any solution file, we _should_ see the tag for the short type
        // in the resulting analysis.json
        short x;
    }
}