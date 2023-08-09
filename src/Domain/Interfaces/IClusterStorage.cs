using System.Collections;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IClusterStorage: IEnumerable
{
    ///<summary>
    /// Хранение информации о кластерах. Где key - id кластера, value - сам кластер
    ///</summary>
    Dictionary<int, Cluster> Clusters { get; }

    ///<summary>
    /// Хранение информации о транзакциях. Где key - id кластера, value - список его транзакций
    ///</summary>
    Dictionary<int, List<Transaction>> Transactions { get; }

    ///<summary>
    /// Количество кластеров.
    ///</summary>
    int Length { get; }

    ///<summary>
    /// Получение списка транзакций внутри кластера.
    ///</summary>
    IList<Transaction> ClusterTransactions(Cluster cluster);

    ///<summary>
    /// Получение кластера по его уникальному идентификатору.
    ///</summary>
    Cluster GetClusterBy(int id);

    ///<summary>
    /// Создание нового кластера.
    ///</summary>
    Cluster CreateCluster();

    ///<summary>
    /// Добавление транзакции в кластер.
    ///</summary>
    void AddTransaction(int clusterId, Transaction transaction);

    ///<summary>
    /// Удаление транзакции из кластера.
    ///</summary>
    void RemoveTransaction(int clusterId, Transaction transaction);

    ///<summary>
    /// Перемещение транзакции из одного кластера в другой.
    ///</summary>
    void MoveTransaction(int clusterId, Transaction transaction);

    ///<summary>
    /// Удаление пустых кластеров.
    ///</summary>
    void RemoveEmpty();
}