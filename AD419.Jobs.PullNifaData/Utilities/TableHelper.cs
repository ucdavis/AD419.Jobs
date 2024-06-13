using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;
using AD419.Jobs.PullNifaData.Attributes;
using AD419.Jobs.PullNifaData.Extensions;
using AD419.Jobs.PullNifaData.Models;
using Razor.Templating.Core;

namespace AD419.Jobs.PullNifaData.Utilities;

public static class TableHelper
{
    public static TableModel CreateTableModel<T>()
    {
        var tableModel = new TableModel
        {
            Name = typeof(T).GetCustomAttribute<TableAttribute>()?.Name ?? throw new Exception($"Missing TableAttribute on {typeof(T).Name}")
        };

        var columns = typeof(T).GetOrderedProperties().Select((p, i) =>
        {
            var columnModel = new ColumnModel
            {
                Name = p.Name,
                SqlType = p.GetSqlType(),
                Nullable = !p.GetIsRequired(),
                ColumnOrder = i
            };

            var keyOrderAttribute = p.GetCustomAttribute<DbKeyOrderAttribute>();
            if (keyOrderAttribute != null)
            {
                columnModel.KeyOrder = keyOrderAttribute.Order;
            }

            return columnModel;
        }).ToList();

        tableModel.Columns.AddRange(columns);
        return tableModel;
    }

    public static DataTable CreateDataTable<T>()
    {
        var dataTable = new DataTable();

        // Column order is important, since SqlBulkCopy works by ordinal position, not column name
        foreach (var property in typeof(T).GetOrderedProperties())
        {
            dataTable.Columns.Add(property.Name, property.GetUnwrappedType());
        }

        return dataTable;
    }
}