using System.Data;
using FastMember;

namespace AD419.Jobs.PullNifaData.Extensions;

public static class DataTableExtensions
{
    public static void AddModelData<T>(this DataTable dataTable, T model)
        where T : class
    {
        // TypeAccessors are cached, so it's okay to call this in tight loops
        var accessor = TypeAccessor.Create(typeof(T));
        var row = dataTable.NewRow();
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            // SqlBulkCopy needs nulls to be represented by DBNull.Value
            var value = accessor[model, property.Name] ?? DBNull.Value;
            row[property.Name] = value;
        }
        dataTable.Rows.Add(row);
    }
}