using System.Collections;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repository;

public class TransactionsMemoryStorage: IDataSource
{
    private readonly IEnumerable<Transaction> _transactions;

    public TransactionsMemoryStorage(IEnumerable<Transaction> transactions)
    {
        _transactions = transactions;
    }

    public IEnumerator<Transaction> GetEnumerator()
    {
        return _transactions.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}