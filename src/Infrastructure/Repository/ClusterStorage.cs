using System.Collections;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repository;

public class ClusterStorage : IClusterStorage
{
    ///<summary>
    /// Хранение информации о кластерах. Где key - id кластера, value - сам кластер
    ///</summary>
    public Dictionary<int, Cluster> Clusters { get; } = new();
    
    ///<summary>
    /// Хранение информации о транзакциях. Где key - id кластера, value - список его транзакций
    ///</summary>
    public Dictionary<int, List<Transaction>> Transactions { get; } = new();

    ///<summary>
    /// Количество кластеров.
    ///</summary>
    public int Length => Clusters.Count;

    private int _nextId = 1;

    ///<summary>
    /// Получение списка транзакций внутри кластера.
    ///</summary>
    public IList<Transaction> ClusterTransactions(Cluster cluster)
    {
        if (cluster == null)
        {
            throw new ArgumentNullException(nameof(cluster));
        }

        if (Transactions.TryGetValue(cluster.Id, out var list))
        {
            return list;
        }

        throw new KeyNotFoundException($"No transactions found for cluster with ID {cluster.Id}");
    }
    
    ///<summary>
    /// Получение кластера по его уникальному идентификатору.
    ///</summary>
    public Cluster GetClusterBy(int id)
    {
        if (Clusters.TryGetValue(id, out Cluster cluster))
        {
            return cluster;
        }

        throw new KeyNotFoundException($"No cluster found with ID {id}");
    }
    
    ///<summary>
    /// Создание нового кластера.
    ///</summary>
    public Cluster CreateCluster()
    {
        var currentId = _nextId++;
        Clusters[currentId] = new Cluster(currentId);
        return Clusters[currentId];
    }

    ///<summary>
    /// Добавление транзакции в кластер.
    ///</summary>
    public void AddTransaction(int clusterId, Transaction transaction)
    {
        if (!Clusters.ContainsKey(clusterId))
        {
            throw new KeyNotFoundException($"No cluster found with ID {clusterId}");
        }

        if (!Transactions.TryGetValue(clusterId, out var list))
        {
            list = new List<Transaction>();
            Transactions[clusterId] = list;
        }

        list.Add(transaction);
        Clusters[clusterId].UpdateAfterAdd(transaction);
        transaction.ClusterId = clusterId;
    }

    ///<summary>
    /// Удаление транзакции из кластера.
    ///</summary>
    public void RemoveTransaction(int clusterId, Transaction transaction)
    {
        if (!Clusters.ContainsKey(clusterId))
        {
            throw new KeyNotFoundException($"No cluster found with ID {clusterId}");
        }

        if (Transactions.TryGetValue(clusterId, out var list) && list.Remove(transaction))
        {
            Clusters[clusterId].UpdateAfterRemove(transaction);
        }
    }

    ///<summary>
    /// Перемещение транзакции из одного кластера в другой.
    ///</summary>
    public void MoveTransaction(int clusterId, Transaction transaction)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        if (transaction.ClusterId != -1)
        {
            RemoveTransaction(transaction.ClusterId, transaction);
        }

        AddTransaction(clusterId, transaction);
    }

    ///<summary>
    /// Удаление пустых кластеров.
    ///</summary>
    public void RemoveEmpty()
    {
        List<int> clustersToRemove = new();

        foreach (var cluster in Clusters.Values)
        {
            if (!Transactions.ContainsKey(cluster.Id) || Transactions[cluster.Id].Count == 0)
            {
                clustersToRemove.Add(cluster.Id);
            }
        }

        foreach (var clusterId in clustersToRemove)
        {
            Clusters.Remove(clusterId);
            Transactions.Remove(clusterId);
        }
    }

    ///<summary>
    /// Получение итератора для перебора кластеров.
    ///</summary>
    public IEnumerator GetEnumerator()
    {
        return Clusters.Values.GetEnumerator();
    }

    // ///<summary>
    // /// Вывод информации о кластерах.
    // ///</summary>
    // public override string  ToString()
    // {
    //     var table = new ConsoleTable("Cluster", "Unique items (widt hustogramm)");
    //
    //     foreach (var cluster in Clusters)
    //     {
    //         var width = string.Join(", ", cluster.Value.UniqueItemsWithCount.Keys);
    //         table.AddRow(cluster.Value, width);
    //     }
    //
    //     return table.ToString() ?? string.Empty;
    // }
}