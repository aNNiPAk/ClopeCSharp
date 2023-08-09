using System.Collections;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Helpers;

namespace Infrastructure.Repository;

public class DataSourceNormalizer : IDataSource
{
    /// <summary>
    /// Источник данных
    /// </summary>
    private readonly IEnumerable<string> _dataSource;
    
    /// <summary>
    ///  Параметры нормализации
    /// </summary>
    private readonly NormalizeOptions _options;
    
    /// <summary>
    /// Словарь для хранения класса транзакции
    /// </summary>
    private readonly Dictionary<int, string> _transactionIdToClassMap;

    /// <summary>
    /// Возвращает нормализованные данные dataSource на основе NormalizeOptions
    /// </summary>
    /// <param name="dataSource">Перечисление строк, представляющих данные</param>
    /// <param name="options">Параметры нормализации, используемые для обработки данных</param>
    /// <param name="transactionIdToClassMap">Словарь для хранения класса транзакции</param>
    public DataSourceNormalizer(IEnumerable<string> dataSource, NormalizeOptions options, Dictionary<int, string> transactionIdToClassMap)
    {
        _dataSource = dataSource;
        _options = options;
        _transactionIdToClassMap = transactionIdToClassMap;
    }
    
    public IEnumerator<Transaction> GetEnumerator()
    {
        foreach (var line in _dataSource)
        {
            var newList = new List<string>();
            
            // Разделение строки на элементы с помощью SplitString из опций нормализации
            var list = line.Split(_options.SplitString);
            
            for (int i = 0; i < list.Length; i++)
            {
                // Пропуск столбца, если он указан в опциях нормализации
                if (_options.TestDataColumn != -1 && _options.TestDataColumn == i) continue;
                
                var item = list[i];
                
                // Пропуск элемента, если он равен RemoveDataValue из опций нормализации
                if (item == _options.RemoveDataValue) continue;
                
                // Добавление индекса к элементу, если он может повторяться
                if (!_options.HasUniqueDataValue) item += i;
                newList.Add(item);
            }

            var transaction = new Transaction(newList);
            
            
            if (_options.TestDataColumn != -1 && _options.TestDataColumn < list.Length)
            {
                _transactionIdToClassMap[transaction.Id] = list[_options.TestDataColumn];
            }
            
            yield return transaction;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}