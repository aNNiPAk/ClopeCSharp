using Domain.Entities;
using Infrastructure.Repository;

namespace Application.Tests;

using Xunit;
using System.Collections.Generic;

public class ClopeTests
{
    [Fact]
    public void TestNewProcess()
    {
        // Arrange
        var trans = new List<Transaction>
        {
            new("a")
        };

        var storage = new ClusterStorage();
            
        // Act
        var clope = new ClopeAlgorithm(trans, storage, 1.8);
            
        // Assert
        Assert.NotNull(clope);
    }

    [Fact]
    public void TestBuildIntegration()
    {
        // Arrange
        var trans = new List<Transaction>
        {
            new("a", "b"),
            new("a", "b", "c"),
            new("a", "c", "d"),
            new("d", "e"),
            new("d", "e", "f"),
        };
            
        var storage = new ClusterStorage();
        var clope = new ClopeAlgorithm(trans, storage, 1.8);
            
        // Act
        clope.Run();

        var clusterTransactions = storage.Transactions;
        
        
        // Assert
        Assert.Equal(trans[0], clusterTransactions[1][0]);
        Assert.Equal(trans[1], clusterTransactions[1][1]);
        Assert.Equal(trans[2], clusterTransactions[1][2]);
        Assert.Equal(trans[3], clusterTransactions[2][0]);
        Assert.Equal(trans[4], clusterTransactions[2][1]);
    }

    [Fact]
    public void TestWithOtherOrders()
    {
        // Arrange
        var trans = new List<Transaction>
        {
            new("a", "b"),
            new("b", "a"),
            new("c", "d"),
            new("d", "c", "b"),
        };

        var storage = new ClusterStorage();
        var clope = new ClopeAlgorithm(trans, storage, 3.0);
            
        // Act
        clope.Run();

        var clusterTransactions = storage.Transactions;


        Assert.Equal(clusterTransactions[1][0], trans[0]);
        Assert.Equal(clusterTransactions[1][1], trans[1]);
        Assert.Equal(clusterTransactions[2][0], trans[3]);
        Assert.Equal(clusterTransactions[2][1], trans[4]);
    }
}