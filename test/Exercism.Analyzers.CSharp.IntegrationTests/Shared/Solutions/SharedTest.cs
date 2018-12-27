using Xunit;

public class SharedTest
{
    [Fact]
    public void Zero_is_even()
    {
        Assert.True(Shared.IsEven(0));
    }

    [Fact(Skip = "Remove to run test")]
    public void One_is_not_even()
    {
        Assert.False(Shared.IsEven(1));
    }

    [Fact(Skip = "Remove to run test")]
    public void Two_is_even()
    {
        Assert.True(Shared.IsEven(2));
    }
}