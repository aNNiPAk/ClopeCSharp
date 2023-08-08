using Domain.Entities;
using Infrastructure.Helpers;
using Infrastructure.Repository;

namespace Infrastructure.Tests.Repository;

public class DataSourceNormalizerTests
{
    [Fact]
    public void GetEnumerator_ReturnsNormalizedData()
    {
        // Arrange
        var dataSource = new List<string>
        {
            "1,2,3",
            "4,5,6"
        };
        var options = new NormalizeOptions
        {
            SplitString = ",",
            TestDataColumn = -1,
            RemoveDataValue = "5",
            HasUniqueDataValue = true
        };
        var transactionClass = new Dictionary<int, string>();
        var normalizer = new DataSourceNormalizer(dataSource, options, transactionClass);

        // Act
        var result = normalizer.ToList();

        // Assert
        Assert.Collection(result,
            item => Assert.Equal(new Transaction(new List<string> { "1", "2", "3" }), item),
            item => Assert.Equal(new Transaction(new List<string> { "4", "6" }), item));
    }

    [Fact]
    public void GetEnumerator_SkipsTestDataColumn()
    {
        // Arrange
        var dataSource = new List<string>
        {
            "1,2,3",
            "4,5,6"
        };
        var options = new NormalizeOptions
        {
            SplitString = ",",
            TestDataColumn = 1,
            RemoveDataValue = "",
            HasUniqueDataValue = true
        };
        var transactionClass = new Dictionary<int, string>();
        var normalizer = new DataSourceNormalizer(dataSource, options, transactionClass);

        // Act
        var result = normalizer.ToList();

        // Assert
        Assert.Collection(result,
            item => Assert.Equal(new Transaction(new List<string> { "1", "3" }), item),
            item => Assert.Equal(new Transaction(new List<string> { "4", "6" }), item));
    }

    [Fact]
    public void GetEnumerator_AddsIndexToNonUniqueDataValues()
    {
        // Arrange
        var dataSource = new List<string>
        {
            "1,1,3",
            "4,5,5"
        };
        var options = new NormalizeOptions
        {
            SplitString = ",",
            TestDataColumn = -1,
            RemoveDataValue = "",
            HasUniqueDataValue = false
        };
        var transactionClass = new Dictionary<int, string>();
        var normalizer = new DataSourceNormalizer(dataSource, options, transactionClass);

        // Act
        var result = normalizer.ToList();

        // Assert
        Assert.Collection(result,
            item => Assert.Equal(new Transaction(new List<string> { "10", "11", "32" }), item),
            item => Assert.Equal(new Transaction(new List<string> { "40", "51", "52" }), item));
    }
}