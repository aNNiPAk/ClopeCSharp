using Infrastructure.Helpers;

namespace Infrastructure.Tests.Helpers;

public class NormalizeOptionsTests
{
    [Fact]
    public void DefaultValuesTest()
    {
        var options = new NormalizeOptions();
        Assert.Equal(",", options.SplitString);
        Assert.Equal("?", options.RemoveDataValue);
        Assert.Equal(-1, options.TestDataColumn);
        Assert.False(options.HasUniqueDataValue);
    }

    [Fact]
    public void CustomValuesTest()
    {
        var options = new NormalizeOptions
        {
            SplitString = ";",
            RemoveDataValue = "!",
            TestDataColumn = 2,
            HasUniqueDataValue = true
        };
        Assert.Equal(";", options.SplitString);
        Assert.Equal("!", options.RemoveDataValue);
        Assert.Equal(2, options.TestDataColumn);
        Assert.True(options.HasUniqueDataValue);
    }
}