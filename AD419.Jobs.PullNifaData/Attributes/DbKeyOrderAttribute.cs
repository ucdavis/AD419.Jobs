using System.Runtime.CompilerServices;

namespace AD419.Jobs.PullNifaData.Attributes;

/// <summary>
/// Used to identify key columns and their position relative to other key columns
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class DbKeyOrderAttribute : Attribute
{
    private readonly int order_;
    public DbKeyOrderAttribute([CallerLineNumber]int order = 0)
    {
        order_ = order;
    }

    public int Order { get { return order_; } }
}