using static AD419.Jobs.PullNifaData.Attributes.DbAggregateAttribute;
using static AD419.Jobs.PullNifaData.Attributes.DbKeyOrderAttribute;

namespace AD419.Jobs.PullNifaData.Models;

public class TableModel
{
    public string Name { get; set; } = "";
    public List<ColumnModel> Columns { get; set; } = new();
}

public class ColumnModel
{
    public string Name { get; set; } = "";
    public string SqlType { get; set; } = "";
    public bool Nullable { get; set; }
    public int ColumnOrder { get; set; }
    public int? KeyOrder { get; set; }
    public KeySort KeySort { get; set; }
    public Aggregate? Aggregate { get; set; }
}