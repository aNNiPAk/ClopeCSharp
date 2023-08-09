using ConsoleTables;
using Domain.Interfaces;

namespace CLI;

public class DrawingReport
{
    private readonly IClusterStorage _clusterStorage;
    private readonly Dictionary<int, string> _transactionIdToClassMap;

    public DrawingReport(IClusterStorage clusterStorage, Dictionary<int, string> transactionIdToClassMap)
    {
        _clusterStorage = clusterStorage;
        _transactionIdToClassMap = transactionIdToClassMap;
    }

    public void Print()
    {
        var distinct = _transactionIdToClassMap.Values.Distinct().ToList();
        var table = new ConsoleTable(new[] { "Cluster id" }.Concat(distinct).ToArray());
        var totals = distinct.ToDictionary(value => value, value => 0);

        foreach (var (clusterId, cluster) in _clusterStorage.Transactions)
        {
            var dict = distinct.ToDictionary(value => value, value => 0);
            foreach (var transaction in cluster)
            {
                if (_transactionIdToClassMap.TryGetValue(transaction.Id, out var value))
                {
                    dict[value]++;
                }
            }

            foreach (var value in distinct)
            {
                totals[value] += dict[value];
            }

            var x = new[] { clusterId.ToString() }.Concat(dict.Values.Select(v => v.ToString())).ToArray();
            table.AddRow(x);
        }

        table.AddRow(new[] { "Итого" }.Concat(totals.Values.Select(x => x.ToString())).ToArray());
        table.Write();
    }
}