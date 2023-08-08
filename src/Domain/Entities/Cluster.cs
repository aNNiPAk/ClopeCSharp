namespace Domain.Entities;

public class Cluster
{
    /// <summary>
    /// Индентификатор кластера
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// (N) Количество транзакций в кластере
    /// </summary>
    public int NumberOfTransaction { get; private set; }

    /// <summary>
    /// (W) Ширина гистограммы (количество уникальных объектов в кластере)
    /// </summary>
    public int Width => UniqueItemsWithCount.Count;

    /// <summary>
    /// (S) Площадь гистограммы (количество всех объектов в кластере)
    /// </summary>
    public int Square { get; private set; }

    /// <summary>
    /// Уникальные обьекты и их количество вхождений в кластер
    /// </summary>
    public Dictionary<Item, int> UniqueItemsWithCount { get; } = new();

    public Cluster(int id)
    {
        Id = id;
    }

    /// <summary>
    ///  Количество вхождений объекта в кластер C (частота или высота отдельно взятого столбца гистограммы С)
    /// </summary>
    /// <param name="item">Обьект транзакции</param>
    /// <returns>Количество вхождений объекта в кластер C</returns>
    public int Occ(Item item)
    {
        return UniqueItemsWithCount.TryGetValue(item, out var count) ? count : 0;
    }

    /// <summary>
    /// Вычисляет и возвращает прибыль кластера.
    /// </summary>
    /// <param name="square">Количество всех объектов в кластере</param>
    /// <param name="numberOfTransaction">Количество транзакций в кластере</param>
    /// <param name="width">Количество уникальных объектов в кластере</param>
    /// <param name="repulsion">Коэффициент отталкивания</param>
    /// <returns>Вычисленная прибыль.</returns>
    private static double ComputeGradient(int square, int numberOfTransaction, int width, double repulsion)
    {
        return square * (double) numberOfTransaction / Math.Pow(width, repulsion);
    }

    /// <summary>
    /// Вычисляет и возвращает разницу в прибыли до и после добавления транзакции в кластер.
    /// </summary>
    /// <param name="transaction">Добавляемая транзакция.</param>
    /// <param name="repulsion">Коэффициент отталкивания.</param>
    /// <returns>Вычисленная разница в прибыли.</returns>
    public double DeltaAdd(Transaction transaction, double repulsion)
    {
        var squareNew = Square + transaction.ItemCount;
        var widthNew = Width;

        foreach (var item in transaction)
        {
            if (!UniqueItemsWithCount.ContainsKey(item)) widthNew++;
        }

        if (NumberOfTransaction == 0)
        {
            return ComputeGradient(squareNew, 1, widthNew, repulsion);
        }

        var profitCur = ComputeGradient(Square, NumberOfTransaction, Width, repulsion);
        var profitNew = ComputeGradient(squareNew, NumberOfTransaction + 1, widthNew, repulsion);

        return profitNew - profitCur;
    }

    #region Обновление кластера после изменения транзакций

    /// <summary>
    /// Обновление кластера после добавления новой транзакции.
    /// </summary>
    /// <param name="transaction">Добавленная транзакция.</param>
    public void UpdateAfterAdd(Transaction transaction)
    {
        foreach (var item in transaction)
        {
            if (UniqueItemsWithCount.ContainsKey(item))
            {
                UniqueItemsWithCount[item] += 1;
            }
            else
            {
                UniqueItemsWithCount[item] = 1;
            }
        }

        Update(NumberOfTransaction + 1);
    }

    /// <summary>
    /// Обновление кластера после удаления транзакции.
    /// </summary>
    /// <param name="transaction">Удаляемая транзакция.</param>
    public void UpdateAfterRemove(Transaction transaction)
    {
        foreach (var item in transaction)
        {
            if (!UniqueItemsWithCount.ContainsKey(item)) continue;

            UniqueItemsWithCount[item] -= 1;
            if (UniqueItemsWithCount[item] == 0)
            {
                UniqueItemsWithCount.Remove(item);
            }
        }

        Update(NumberOfTransaction - 1);
    }

    /// <summary>
    /// Обновляет площадь кластера (S) и количество транзакций (N).
    /// </summary>
    /// <param name="transactionCount">Обновленное количество транзакций.</param>
    private void Update(int transactionCount)
    {
        NumberOfTransaction = transactionCount;
        Square = 0;

        foreach (var count in UniqueItemsWithCount.Values)
        {
            Square += count;
        }
    }

    #endregion

    public override string ToString()
    {
        return $"ID:{Id};N:{NumberOfTransaction};W:{Width};S:{Square};";
    }
}