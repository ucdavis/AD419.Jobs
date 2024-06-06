
using System.Reflection;
using AD419.Jobs.PullNifaData.Attributes;
using FastMember;

namespace AD419.Jobs.PullNifaData.Extensions;

public static class TypeExtensions
{
    /// <summary>
    /// Get properties in the order they are declared via <seealso cref="DbColumnOrderAttribute"/>
    /// </summary>
    public static IEnumerable<PropertyInfo> GetOrderedProperties(this Type type)
    {
        return type.GetProperties()
            .Where(p => Attribute.IsDefined(p, typeof(DbColumnOrderAttribute)))
            .OrderBy(p => ((DbColumnOrderAttribute)p.GetCustomAttributes(typeof(DbColumnOrderAttribute), false).Single()).Order);
    }
}