using System.Runtime.CompilerServices;

namespace AD419.Jobs.PullNifaData.Attributes;


/// <summary>
/// Identifies key columns and their position relative to other key columns
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class DbKeyOrderAttribute : Attribute
{
    public int Order { get; }
    public KeySort Sort { get; }

    public DbKeyOrderAttribute([CallerLineNumber] int order = 0, KeySort sort = KeySort.Asc)
    {
        Order = order;
        Sort = sort;
    }

    public enum KeySort
    {
        Asc,
        Desc
    }
}