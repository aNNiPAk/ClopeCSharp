namespace Domain.Entities;

public class Item
{
    public string Value { get; }
    public int Hash { get; }

    public Item(string value)
    {
        Value = value;
        Hash = value.GetHashCode();
    }

    public override int GetHashCode()
    {
        return Hash;
    }
    
    public override bool Equals(object? obj)
    {
        return obj is Item item && Value == item.Value;
    }

    public override string ToString()
    {
        return Value;
    }
}