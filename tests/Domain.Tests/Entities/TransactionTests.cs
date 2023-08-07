using Domain.Entities;

namespace Domain.Tests.Entities;

public class TransactionTests
{
    [Fact]
    public void TestTransactionConstructor()
    {
        // Arrange
        var items = new List<Item> { new("item1"), new("item2") };
        
        // Act
        var transaction = new Transaction(items);
        
        // Assert
        Assert.Equal(items, transaction.Items);
        Assert.Equal(-1, transaction.ClusterId);
    }

    [Fact]
    public void TestTransactionConstructorWithStrings()
    {
        // Arrange
        var items = new List<string> { "item1", "item2" };
        
        // Act
        var transaction = new Transaction(items);
        
        // Assert
        Assert.Equal(items.Count, transaction.Items.Count);
        Assert.Equal(-1, transaction.ClusterId);
    }

    [Fact]
    public void TestTransactionConstructorWithParams()
    {
        // Act
        var transaction = new Transaction("item1", "item2");
        
        // Assert
        Assert.Equal(2, transaction.Items.Count);
        Assert.Equal(-1, transaction.ClusterId);
    }

    [Fact]
    public void GetEnumeratorTest()
    {
        // Arrange
        var items = new List<Item> { new("item1"), new("item2") };
        var transaction = new Transaction(items);
        
        // Act
        using var enumerator = transaction.GetEnumerator();
        
        // Assert
        Assert.True(enumerator.MoveNext());
        Assert.Equal(items[0], enumerator.Current);
        Assert.True(enumerator.MoveNext());
        Assert.Equal(items[1], enumerator.Current);
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void ToStringTest()
    {
        // Arrange
        var items = new List<Item> { new("item1"), new("item2") };
        
        // Act
        var transaction = new Transaction(items);
        
        // Assert
        Assert.Equal("{item1, item2}", transaction.ToString());
    }
}