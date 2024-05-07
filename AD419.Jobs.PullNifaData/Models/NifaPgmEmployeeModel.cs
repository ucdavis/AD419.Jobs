using AD419.Jobs.PullNifaData.Attributes;
using CsvHelper.Configuration.Attributes;

namespace AD419.Jobs.PullNifaData.Models;


public class NifaPgmEmployeeModel
{
    [Name("Run Date")]
    [ColumnOrder]
    public DateTime RunDate { get; set; }

    [Name("Award ID")]
    [ColumnOrder]
    public string AwardID { get; set; } = "";

    [Name("Project ID")]
    [ColumnOrder]
    public string ProjectID { get; set; } = "";

    [Name("Person ID")]
    [ColumnOrder]
    public string PersonID { get; set; } = "";

    [Name("Person Name")]
    [ColumnOrder]
    public string PersonName { get; set; } = "";

    [Name("Person Number")]
    [ColumnOrder]
    public string PersonNumber { get; set; } = "";

    [Name("Role")]
    [ColumnOrder]
    public string Role { get; set; } = "";

    [Name("Role Start Date")]
    [ColumnOrder]
    public DateTime? RoleStartDate { get; set; }

    [Name("Role End Date")]
    [ColumnOrder]
    public DateTime? RoleEndDate { get; set; }

    [Name("Person Start Date")]
    [ColumnOrder]
    public DateTime? PersonStartDate { get; set; }

    [Name("Person End Date")]
    [ColumnOrder]
    public DateTime? PersonEndDate { get; set; }
}