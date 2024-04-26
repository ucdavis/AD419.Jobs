using AD419.Jobs.PullNifaData.Attributes;
using CsvHelper.Configuration.Attributes;

namespace AD419.Jobs.Models;


public class NifaPgmEmployeeModel
{
    [Name("Run Date")]
    [Order]
    public DateTime RunDate { get; set; }

    [Name("Award ID")]
    [Order]
    public string AwardID { get; set; } = "";

    [Name("Project ID")]
    [Order]
    public string ProjectID { get; set; } = "";

    [Name("Person ID")]
    [Order]
    public string PersonID { get; set; } = "";

    [Name("Person Name")]
    [Order]
    public string PersonName { get; set; } = "";

    [Name("Person Number")]
    [Order]
    public string PersonNumber { get; set; } = "";

    [Name("Role")]
    [Order]
    public string Role { get; set; } = "";

    [Name("Role Start Date")]
    [Order]
    public DateTime? RoleStartDate { get; set; }

    [Name("Role End Date")]
    [Order]
    public DateTime? RoleEndDate { get; set; }

    [Name("Person Start Date")]
    [Order]
    public DateTime? PersonStartDate { get; set; }

    [Name("Person End Date")]
    [Order]
    public DateTime? PersonEndDate { get; set; }
}