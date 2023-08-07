using Domain.Entities;

namespace Domain.Tests.Entities;

public class ItemTests
{
    [Fact]
    public void Constructor_SetsValueAndHash()
    {
        // Arrange
        var value = "test";
        var expectedHash = value.GetHashCode();

        // Act
        var item = new Item(value);

        // Assert
        Assert.Equal(value, item.Value);
        Assert.Equal(expectedHash, item.Hash);
    }

    [Fact]
    public void GetHashCode_ReturnsHash()
    {
        // Arrange
        var value = "test";
        var expectedHash = value.GetHashCode();
        var item = new Item(value);

        // Act
        var hash = item.GetHashCode();

        // Assert
        Assert.Equal(expectedHash, hash);
    }

    [Fact]
    public void Equals_ReturnsTrueForEqualItems()
    {
        // Arrange
        var value = "test";
        var item1 = new Item(value);
        var item2 = new Item(value);

        // Act
        var result = item1.Equals(item2);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Equals_ReturnsFalseForDifferentItems()
    {
        // Arrange
        var value1 = "test1";
        var value2 = "test2";
        var item1 = new Item(value1);
        var item2 = new Item(value2);

        // Act
        var result = item1.Equals(item2);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ToString_ReturnsValue()
    {
        // Arrange
        var value = "test";
        var item = new Item(value);

        // Act
        var result = item.ToString();

        // Assert
        Assert.Equal(value, result);
    }
}