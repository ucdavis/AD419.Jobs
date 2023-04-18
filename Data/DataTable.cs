using System.ComponentModel;
using System.Data;
using System.Text;

namespace AD419Functions.Data;

public class DataTable<T> : DataTable
{
    private static readonly PropertyDescriptorCollection _properties = TypeDescriptor.GetProperties(typeof(T));

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

        // TODO: If performance becomes an issue, use something faster like compiled expressions or Dotnext.Reflection.
        //       Or just make make column init/load operations abstract.
        foreach (PropertyDescriptor prop in _properties)
        {
            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        }

        Rows.Add(row);
    }
}