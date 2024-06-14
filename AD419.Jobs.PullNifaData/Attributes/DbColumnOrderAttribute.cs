using System.Runtime.CompilerServices;

namespace AD419.Jobs.PullNifaData.Attributes;

/// <summary>
/// Ensures properties are enumerated in declaration order when reflecting
/// </summary>
[AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public sealed class DbColumnOrderAttribute : Attribute
{
    public int Order { get; }

    public DbColumnOrderAttribute([CallerLineNumber]int order = 0)
    {
        Order = order;
    }
}