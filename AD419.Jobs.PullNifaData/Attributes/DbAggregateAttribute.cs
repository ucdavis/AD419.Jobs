using System.Runtime.CompilerServices;

namespace AD419.Jobs.PullNifaData.Attributes;

/// <summary>
/// Identifies columns that will have an aggregate function applied
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class DbAggregateAttribute : Attribute
{
    public Aggregate AggregateFn { get; }

    public DbAggregateAttribute(Aggregate aggregateFn)
    {
        AggregateFn = aggregateFn;
    }

    public enum Aggregate
    {
        Max,
        Min,
        // Add more as needed...
    }
}