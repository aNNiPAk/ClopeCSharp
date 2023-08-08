using Domain.Entities;

namespace Domain.Tests.Entities;

public class ClusterTests
{
    [Fact]
    public void TestClusterConstructor()
    {
        // Act
        var cluster = new Cluster(1);

        // Assert
        Assert.Equal(1, cluster.Id);
        Assert.Equal(0, cluster.NumberOfTransaction);
        Assert.Equal(0, cluster.Width);
        Assert.Equal(0, cluster.Square);
        Assert.Empty(cluster.UniqueItemsWithCount);
    }

    [Fact]
    public void TestClusterOcc()
    {
        // Arrange
        var cluster = new Cluster(1);
        var item = new Item("1");

        // Act & Assert
        Assert.Equal(0, cluster.Occ(item));

        // Arrange
        cluster.UniqueItemsWithCount[item] = 5;

        // Act & Assert
        Assert.Equal(5, cluster.Occ(item));
    }

    [Fact]
    public void TestClusterDeltaAdd()
    {
        // Arrange
        var cluster = new Cluster(1);
        var transaction = new Transaction("a", "b");

        // Act
        var delta = cluster.DeltaAdd(transaction, 2.0);

        // Assert
        Assert.Equal(0.5, delta);
    }
    
    [Fact]
    public void TestClusterCommon()
    {
        // Arrange
        var cluster = new Cluster(1);

        var itemA = new Item("a");
        var itemB = new Item("b");
        var itemC = new Item("c");
        var itemD = new Item("d");
        var transaction = new Transaction[]
        {
            new(itemA, itemB),
            new(itemA, itemB, itemC),
            new(itemA, itemC, itemD)
        };
        
        // Act
        cluster.UpdateAfterAdd(transaction[0]);
        cluster.UpdateAfterAdd(transaction[1]);
        cluster.UpdateAfterAdd(transaction[2]);

        // Assert
        // Проверка ширины гистограммы
        Assert.Equal(4, cluster.Width);
        
        // Частоты
        Assert.Equal(3, cluster.Occ(itemA));
        Assert.Equal(2, cluster.Occ(itemB));
        Assert.Equal(2, cluster.Occ(itemC));
        Assert.Equal(1, cluster.Occ(itemD));
        
        // Площади
        Assert.Equal(8, cluster.Square);

    }

    [Fact]
    public void TestClusterUpdateAfterAdd()
    {
        // Arrange
        var cluster = new Cluster(1);
        var item1 = new Item("a");
        var item2 = new Item("b");
        var transaction = new Transaction(item1, item2);

        // Act
        cluster.UpdateAfterAdd(transaction);

        // Assert
        Assert.Equal(2, cluster.Square);
        Assert.Equal(2, cluster.Width);
        Assert.Equal(1, cluster.NumberOfTransaction);
        Assert.Equal(1, cluster.UniqueItemsWithCount[item1]);
        Assert.Equal(1, cluster.UniqueItemsWithCount[item2]);
    }

    [Fact]
    public void TestClusterUpdateAfterRemove()
    {
        // Arrange
        var cluster = new Cluster(1);
        var item1 = new Item("a");
        var item2 = new Item("b");
        var transaction = new Transaction(item1, item2);

        // Act
        cluster.UpdateAfterAdd(transaction);
        cluster.UpdateAfterRemove(transaction);

        // Assert
        Assert.Equal(0, cluster.Square);
        Assert.Equal(0, cluster.Width);
        Assert.Equal(0, cluster.NumberOfTransaction);
        Assert.Empty(cluster.UniqueItemsWithCount);
    }
    
    [Fact]
    public void TestDeltaAddEvaluative()
    {
        // Arrange
        double r = 2;
    
        var trans = new Transaction[]
        {
            new("a", "b"),
            new("a", "b", "c"),
            new("a", "c", "d"),
            new("d", "e"),
            new("d", "e", "f")
        };
    
        var clusters = new Cluster[]
        {
            new(0),
            new(1)
        };
    
        // Act && Assert
        var delta = clusters[0].DeltaAdd(trans[0], r);
        Assert.Equal(1.0/2, delta);
        
        clusters[0].UpdateAfterAdd(trans[0]);

        delta = clusters[0].DeltaAdd(trans[1], r);
        Assert.Equal(11.0/18, delta);

        var newDelta = clusters[1].DeltaAdd(trans[1], r);
        Assert.Equal(1.0/3, newDelta);
    }
}