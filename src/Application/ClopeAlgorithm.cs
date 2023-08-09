using Domain.Entities;
using Domain.Interfaces;

namespace Application;

public class ClopeAlgorithm
{
    private readonly IEnumerable<Transaction> _dataSource;
    private readonly IClusterStorage _clusterStorage;
    private readonly double _repulsion;

    public ClopeAlgorithm(IEnumerable<Transaction> dataSource, IClusterStorage clusterStorage, double repulsion)
    {
        _dataSource = dataSource;
        _clusterStorage = clusterStorage;
        _repulsion = repulsion;
    }

    public void Run()
    {
        Initialization();
        Iteration();
    }

    public void Initialization()
    {
        foreach (var transaction in _dataSource)
        {
            var bestCluster = BestClusterFor(transaction);
            _clusterStorage.MoveTransaction(bestCluster.Id, transaction);
        }
    }

    public void Iteration()
    {
        while (true)
        {
            var moved = false;
            
            foreach (var transaction in _dataSource)
            {
                var lastClusterId = transaction.ClusterId;
                var bestCluster = BestClusterFor(transaction);
        
                if (bestCluster.Id != lastClusterId)
                {
                    _clusterStorage.MoveTransaction(bestCluster.Id, transaction);
                    moved = true;
                }
            }
        
            if (!moved) break;
        }
        
        _clusterStorage.RemoveEmpty();
    }

    private Cluster BestClusterFor(Transaction transaction)
    {
        Cluster? bestCluster = null;

        if (_clusterStorage.Length > 0)
        {
            var tempW = transaction.ItemCount;
        
            var deltaMax = tempW / Math.Pow(tempW, _repulsion);
            
            foreach (var cluster in _clusterStorage.Clusters.Values)
            {
                var curDelta = cluster.DeltaAdd(transaction, _repulsion);
                
                if (curDelta > deltaMax)
                {
                    deltaMax = curDelta;
                    bestCluster = cluster;
                }
            }
        }

        return bestCluster ?? _clusterStorage.CreateCluster();
    }
}