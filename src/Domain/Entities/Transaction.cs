using System.Collections;

namespace Domain.Entities;

public class Transaction : IEnumerable<Item>
{
    public IList<Item> Items { get; }
    public int Id { get; }
    public int ClusterId { get; set; } = -1;
    public int ItemCount => Items.Count;

    private static int _transactionId;

    public Transaction(IList<Item> items)
    {
        Items = items;
        Id = _transactionId++;
    }

    public Transaction(IEnumerable<string> items) : this(items.Select(item => new Item(item)).ToList())
    {
    }

    public Transaction(params string[] items) : this(items.Select(item => new Item(item)).ToList())
    {
    }

    public IEnumerator<Item> GetEnumerator()
    {
        return Items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public override string ToString()
    {
        return "{" + string.Join(", ", Items) + "}";
    }
}