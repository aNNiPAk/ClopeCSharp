using Domain.Entities;
using Infrastructure.Repository;

namespace Infrastructure.Tests.Repository;

public class ClusterStorageTests
{
    private readonly ClusterStorage _storage = new();

    [Fact]
    public void TestClusterTransactions()
    {
        // Act
        var cluster = _storage.CreateCluster();
        var transaction = new Transaction("a");
        _storage.AddTransaction(cluster.Id, transaction);
        var transactions = _storage.ClusterTransactions(cluster);

        // Assert
        Assert.Single(transactions);
        Assert.Equal(transaction, transactions[0]);
    }

    [Fact]
    public void TestGetClusterBy()
    {
        // Act
        var cluster = _storage.CreateCluster();
        var retrievedCluster = _storage.GetClusterBy(cluster.Id);

        // Assert
        Assert.Equal(cluster, retrievedCluster);
    }

    [Fact]
    public void TestCreateCluster()
    {
        // Act
        var cluster = _storage.CreateCluster();

        // Assert
        Assert.NotNull(cluster);
        Assert.Equal(1, cluster.Id);
    }

    [Fact]
    public void TestAddTransaction()
    {
        // Act
        var cluster = _storage.CreateCluster();
        var transaction = new Transaction("a");
        _storage.AddTransaction(cluster.Id, transaction);

        // Assert
        Assert.Equal(cluster.Id, transaction.ClusterId);
        Assert.Single(_storage.Transactions[cluster.Id]);
        Assert.Equal(transaction, _storage.Transactions[cluster.Id][0]);
    }

    [Fact]
    public void TestRemoveTransaction()
    {
        // Act
        var cluster = _storage.CreateCluster();
        var transaction = new Transaction("a");
        _storage.AddTransaction(cluster.Id, transaction);
        _storage.RemoveTransaction(cluster.Id, transaction);

        // Assert
        Assert.Empty(_storage.Transactions[cluster.Id]);
    }

    [Fact]
    public void TestMoveTransaction()
    {
        // Act
        var cluster1 = _storage.CreateCluster();
        var cluster2 = _storage.CreateCluster();
        var transaction = new Transaction("a");
        _storage.AddTransaction(cluster1.Id, transaction);
        _storage.MoveTransaction(cluster2.Id, transaction);

        // Assert
        Assert.Empty(_storage.Transactions[cluster1.Id]);
        Assert.Single(_storage.Transactions[cluster2.Id]);
        Assert.Equal(transaction, _storage.Transactions[cluster2.Id][0]);
    }

    [Fact]
    public void TestRemoveEmpty()
    {
        // Act
        var cluster1 = _storage.CreateCluster();
        var cluster2 = _storage.CreateCluster();
        var transaction = new Transaction("a");
        _storage.AddTransaction(cluster2.Id, transaction);
        _storage.RemoveEmpty();

        // Assert
        Assert.Throws<KeyNotFoundException>(() => _storage.GetClusterBy(cluster1.Id));
    }

    [Fact]
    public void TestRemoveEmptyTwo()
    {
        // Arrange
        var _ = _storage.CreateCluster();
        var trans = new Transaction("a", "d");
        var secondCluster = _storage.CreateCluster();
        _storage.MoveTransaction(secondCluster.Id, trans);

        // Act
        _storage.RemoveEmpty();

        // Assert
        Assert.Equal(1, _storage.Length);
    }
}