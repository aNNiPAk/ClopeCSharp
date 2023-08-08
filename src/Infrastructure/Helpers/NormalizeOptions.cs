namespace Infrastructure.Helpers;

public class NormalizeOptions
{
    public string SplitString { get; init; } = ",";
    public string RemoveDataValue { get; init; } = "?";
    public int TestDataColumn { get; init; } = -1;
    public bool HasUniqueDataValue { get; init; }
}