using System.ComponentModel;
using System.Data;
using System.Text;

namespace AD419Functions.Data;

public class DataTable<T> : DataTable
{
    private readonly PropertyDescriptorCollection _properties = TypeDescriptor.GetProperties(typeof(T));

    public DataTable(string tableName) : base(tableName)
    {
        foreach (PropertyDescriptor prop in _properties)
        {
            Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        }
    }

    public void Add(T item)
    {
        DataRow row = NewRow();

        // TODO: Use something faster like compiled expressions or Dotnext.Reflection
        foreach (PropertyDescriptor prop in _properties)
        {
            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        }

        Rows.Add(row);
    }

    /// <summary>
    /// Generates a heap table (no indexes) DDL statement for this DataTable
    /// </summary>
    public string GenerateDDL()
    {
        var columnDefinitions = new List<string>();
        foreach (DataColumn column in Columns)
        {
            columnDefinitions.Add(column.DataType.ToString() switch {
                // 255 appears to be a default for strings in AggieEnterprise db
                "System.String" => $"[{column.ColumnName}] NVARCHAR({(column.MaxLength == -1 ? 255 : column.MaxLength)})",
                "System.Int32" => $"[{column.ColumnName}] INT",
                "System.Int64" => $"[{column.ColumnName}] BIGINT",
                "System.Boolean" => $"[{column.ColumnName}] BIT",
                "System.DateTime" => $"[{column.ColumnName}] DATETIME",
                "System.Decimal" => $"[{column.ColumnName}] DECIMAL(18,2)",
                _ => throw new Exception($"Unknown data type: {column.DataType}")
            });
        }
        var ddl = $@"CREATE TABLE {TableName} ({Environment.NewLine}{string.Join($",{Environment.NewLine}", columnDefinitions)})";
        return ddl;
    }
}