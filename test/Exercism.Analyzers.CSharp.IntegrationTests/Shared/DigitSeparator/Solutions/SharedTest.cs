// This file was auto-generated based on version 1.1.0 of the canonical data.

using System;
using Xunit;

public class SharedTest
{
    [Fact]
    public void Is_positive()
    {
        Assert.True(Shared.Number() > 0);
    }
}