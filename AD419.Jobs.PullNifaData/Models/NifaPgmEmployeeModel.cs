using System.ComponentModel.DataAnnotations.Schema;
using AD419.Jobs.PullNifaData.Attributes;
using CsvHelper.Configuration.Attributes;
using static AD419.Jobs.PullNifaData.Attributes.DbAggregateAttribute;
using static AD419.Jobs.PullNifaData.Attributes.DbKeyOrderAttribute;

namespace AD419.Jobs.PullNifaData.Models;


[Table("UCD_NIFA_PGM_EMPLOYEE")]
public class NifaPgmEmployeeModel
{
    [Name("Run Date")]
    [DbColumnOrder]
    public DateTime RunDate { get; set; }

    [Name("Award ID")]
    [DbColumnOrder]
    [DbKeyOrder(0)]
    public string AwardID { get; set; } = "";

    [Name("Project ID")]
    [DbColumnOrder]
    [DbKeyOrder(1)]
    public string ProjectID { get; set; } = "";

    [Name("Person ID")]
    [DbColumnOrder]
    [DbKeyOrder(2)]
    public string PersonID { get; set; } = "";

    [Name("Person Name")]
    [DbColumnOrder]
    public string PersonName { get; set; } = "";

    [Name("Person Number")]
    [DbColumnOrder]
    public string PersonNumber { get; set; } = "";

    [Name("Role")]
    [DbColumnOrder]
    [DbKeyOrder(3)]
    public string Role { get; set; } = "";

    [Name("Role Start Date")]
    [DbColumnOrder]
    [DbKeyOrder(4)]
    [DbAggregate(Aggregate.Min)]
    public DateTime? RoleStartDate { get; set; }

    [Name("Role End Date")]
    [DbColumnOrder]
    [DbKeyOrder(5, KeySort.Desc)]
    [DbAggregate(Aggregate.Max)]
    public DateTime? RoleEndDate { get; set; }

    [Name("Person Start Date")]
    [DbColumnOrder]
    [DbKeyOrder(6)]
    [DbAggregate(Aggregate.Min)]
    public DateTime? PersonStartDate { get; set; }

    [Name("Person End Date")]
    [DbColumnOrder]
    [DbKeyOrder(7, KeySort.Desc)]
    [DbAggregate(Aggregate.Max)]
    public DateTime? PersonEndDate { get; set; }

    [Name("Source")]
    [DbColumnOrder]
    [DbKeyOrder(8)]
    public string Source { get; set; } = "";
}