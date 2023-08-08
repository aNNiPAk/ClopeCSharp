using Domain.Entities;

namespace Domain.Interfaces;

public interface IDataSource : IEnumerable<Transaction>
{
}